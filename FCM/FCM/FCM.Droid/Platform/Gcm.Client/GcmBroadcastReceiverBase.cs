

using Android.App;
using Android.Content;


namespace Gcm.Client
{
	public abstract class GcmBroadcastReceiverBase<TIntentService> : BroadcastReceiver where TIntentService : GcmServiceBase
	{
		public override void OnReceive(Context context, Intent intent)
		{
			Logger.Debug("OnReceive: " + intent.Action);
			var className = context.PackageName + Constants.DEFAULT_INTENT_SERVICE_CLASS_NAME;

			Logger.Debug("GCM IntentService Class: " + className);

			GcmServiceBase.RunIntentInService(context, intent, typeof(TIntentService));
			SetResult(Result.Ok, null, null);
		}
	}
}