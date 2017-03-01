using System;
using System.Collections.Generic;


using Android.App;
using Android.Content;
using Android.OS;

using Android.Content.PM;

namespace Gcm.Client
{
	public class GcmClient
	{
		const string BACKOFF_MS = "backoff_ms";
		const string GSF_PACKAGE = "com.google.android.gsf";
		const string PREFERENCES = "com.google.android.gcm";
		const int DEFAULT_BACKOFF_MS = 3000;
		const string PROPERTY_REG_ID = "regId";
		const string PROPERTY_APP_VERSION = "appVersion";
		const string PROPERTY_ON_SERVER = "onServer";

		//static GCMBroadcastReceiver sRetryReceiver;

		public static void CheckDevice(Context context)
		{
			var version = (int)Build.VERSION.SdkInt;
			if (version < 8)
				throw new InvalidOperationException("Device must be at least API Level 8 (instead of " + version + ")");

			var packageManager = context.PackageManager;

			try 
			{
				packageManager.GetPackageInfo(GSF_PACKAGE, 0); 
			}
			catch
			{
				throw new InvalidOperationException("Device does not have package " + GSF_PACKAGE);
			}
		}

		public static void CheckManifest(Context context)
		{
			var packageManager = context.PackageManager;
			var packageName = context.PackageName;
			var permissionName = packageName + ".permission.C2D_MESSAGE";

			if (string.IsNullOrEmpty (packageName))
				throw new NotSupportedException ("Your Android app must have a package name!");

			if (char.IsUpper (packageName [0]))
				throw new NotSupportedException ("Your Android app package name MUST start with a lowercase character.  Current Package Name: " + packageName);

			try
			{
				packageManager.GetPermissionInfo(permissionName, PackageInfoFlags.Permissions);
			}
			catch
			{
				throw new AccessViolationException("Application does not define permission: " + permissionName);
			}

			PackageInfo receiversInfo;

			try
			{
				receiversInfo = packageManager.GetPackageInfo(packageName, PackageInfoFlags.Receivers);
			}
			catch
			{
				throw new InvalidOperationException("Could not get receivers for package " + packageName);
			}

			var receivers = receiversInfo.Receivers;

			if (receivers == null || receivers.Count <= 0)
				throw new InvalidOperationException("No Receiver for package " + packageName);

			Logger.Debug("number of receivers for " + packageName + ": " + receivers.Count);

			var allowedReceivers = new HashSet<string>();

			foreach (var receiver in receivers)
			{
				if (Constants.PERMISSION_GCM_INTENTS.Equals(receiver.Permission))
					allowedReceivers.Add(receiver.Name);
			}

			if (allowedReceivers.Count <= 0)
				throw new InvalidOperationException("No receiver allowed to receive " + Constants.PERMISSION_GCM_INTENTS);

			CheckReceiver(context, allowedReceivers, Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK);
			CheckReceiver(context, allowedReceivers, Constants.INTENT_FROM_GCM_MESSAGE);
		}

		private static void CheckReceiver(Context context, HashSet<string> allowedReceivers, string action)
		{
			var pm = context.PackageManager;
			var packageName = context.PackageName;

			var intent = new Intent(action);
			intent.SetPackage(packageName);

			var receivers = pm.QueryBroadcastReceivers(intent, PackageInfoFlags.IntentFilters);

			if (receivers == null || receivers.Count <= 0)
				throw new InvalidOperationException("No receivers for action " + action);

			Logger.Debug("Found " + receivers.Count + " receivers for action " + action);

			foreach (var receiver in receivers)
			{
				var name = receiver.ActivityInfo.Name;
				if (!allowedReceivers.Contains(name))
					throw new InvalidOperationException("Receiver " + name + " is not set with permission " + Constants.PERMISSION_GCM_INTENTS);
			}
		}

		public static void Register(Context context, params string[] senderIds)
		{
			SetRetryBroadcastReceiver(context);
			ResetBackoff(context);

			internalRegister(context, senderIds);
		}

		internal static void internalRegister(Context context, params string[] senderIds)
		{
			if (senderIds == null || senderIds.Length <= 0)
				throw new ArgumentException("No senderIds");

			var senders = string.Join(",", senderIds);

			Logger.Debug("Registering app " + context.PackageName + " of senders " + senders);

			var intent = new Intent(Constants.INTENT_TO_GCM_REGISTRATION);
			intent.SetPackage(GSF_PACKAGE);
			intent.PutExtra(Constants.EXTRA_APPLICATION_PENDING_INTENT,
				PendingIntent.GetBroadcast(context, 0, new Intent(), 0));
			intent.PutExtra(Constants.EXTRA_SENDER, senders);

			context.StartService(intent);
		}

		public static void UnRegister(Context context)
		{
			SetRetryBroadcastReceiver(context);
			ResetBackoff(context);
			internalUnRegister(context);
		}

		internal static void internalUnRegister(Context context)
		{
			Logger.Debug("Unregistering app " + context.PackageName);

			var intent = new Intent(Constants.INTENT_TO_GCM_UNREGISTRATION);
			intent.SetPackage(GSF_PACKAGE);
			intent.PutExtra(Constants.EXTRA_APPLICATION_PENDING_INTENT,
				PendingIntent.GetBroadcast(context, 0, new Intent(), 0));

			context.StartService(intent);
		}

		static void SetRetryBroadcastReceiver(Context context)
		{
			return;

			/*			if (sRetryReceiver == null)
			{
				sRetryReceiver = new GCMBroadcastReceiver();
				var category = context.PackageName;

				var filter = new IntentFilter(GCMConstants.INTENT_FROM_GCM_LIBRARY_RETRY);
				filter.AddCategory(category);

				var permission = category + ".permission.C2D_MESSAGE";

				Log.Verbose(TAG, "Registering receiver");

				context.RegisterReceiver(sRetryReceiver, filter, permission, null);
			}*/
		}

		public static string GetRegistrationId(Context context)
		{
			var prefs = GetGCMPreferences(context);

			var registrationId = prefs.GetString(PROPERTY_REG_ID, "");

			int oldVersion = prefs.GetInt(PROPERTY_APP_VERSION, int.MinValue);
			int newVersion = GetAppVersion(context);

			if (oldVersion != int.MinValue && oldVersion != newVersion)
			{
				Logger.Debug("App version changed from " + oldVersion + " to " + newVersion + "; resetting registration id");

				ClearRegistrationId(context);
				registrationId = string.Empty;
			}

			return registrationId;
		}

		public static bool IsRegistered(Context context)
		{
			var registrationId = GetRegistrationId(context);

			return !string.IsNullOrEmpty(registrationId);
		}

		internal static string ClearRegistrationId(Context context)
		{
			return SetRegistrationId(context, "");
		}

		internal static string SetRegistrationId(Context context, string registrationId)
		{
			var prefs = GetGCMPreferences(context);

			var oldRegistrationId = prefs.GetString(PROPERTY_REG_ID, "");
			int appVersion = GetAppVersion(context);
			Logger.Debug("Saving registrationId on app version " + appVersion);
			var editor = prefs.Edit();
			editor.PutString(PROPERTY_REG_ID, registrationId);
			editor.PutInt(PROPERTY_APP_VERSION, appVersion);
			editor.Commit();
			return oldRegistrationId;
		}


		public static void SetRegisteredOnServer(Context context, bool flag)
		{
			var prefs = GetGCMPreferences(context);
			Logger.Debug("Setting registered on server status as: " + flag);
			var editor = prefs.Edit();
			editor.PutBoolean(PROPERTY_ON_SERVER, flag);
			editor.Commit();
		}

		public static bool IsRegisteredOnServer(Context context)
		{
			var prefs = GetGCMPreferences(context);
			bool isRegistered = prefs.GetBoolean(PROPERTY_ON_SERVER, false);
			Logger.Debug("Is registered on server: " + isRegistered);
			return isRegistered;
		}

		static int GetAppVersion(Context context)
		{
			try
			{
				var packageInfo = context.PackageManager.GetPackageInfo(context.PackageName, 0);
				return packageInfo.VersionCode;
			}
			catch
			{
				throw new InvalidOperationException("Could not get package name");
			}
		}

		internal static void ResetBackoff(Context context)
		{
			Logger.Debug("resetting backoff for " + context.PackageName);
			SetBackoff(context, DEFAULT_BACKOFF_MS);
		}

		internal static int GetBackoff(Context context)
		{
			var prefs = GetGCMPreferences(context);
			return prefs.GetInt(BACKOFF_MS, DEFAULT_BACKOFF_MS);
		}

		internal static void SetBackoff(Context context, int backoff)
		{
			var prefs = GetGCMPreferences(context);
			var editor = prefs.Edit();
			editor.PutInt(BACKOFF_MS, backoff);
			editor.Commit();
		}

		static ISharedPreferences GetGCMPreferences(Context context)
		{
			return context.GetSharedPreferences(PREFERENCES, FileCreationMode.Private);
		}
	}
}