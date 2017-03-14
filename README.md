# Fire-Cloud-Messaging-for-Xamarin-Forms-Android
So many people have asked for convenient and efficient way of implementing FCM (Firebase Cloud Messaging) for android in Xamarin Form. Actually FCM library cannot be installed in xamarin forms due to version conflict of android support library present in xamarin forms. So here is the work around

Firebase Cloud Messaging (FCM) is a cross-platform service that handles the sending, routing, and queueing of messages between server applications and mobile client apps. FCM is the successor to Google Cloud Messaging (GCM), and it is built on Google Play Services.

I have used the following project for GCM.Client implementation https://github.com/Redth/GCM.Client
If you want a similar implementation in your project then copy the GCM.Client folder in your project

In MainActivity.cs class declare

using Gcm.Client; at the top

and in class body

    static MainActivity instance = null;

    // Return the current activity instance.
        public static MainActivity CurrentActivity
        {
          get
          {
            return instance;
          }
        }
In OnCreate method

    instance = this;

    try
    {
      // Check to ensure everything's set up right
      GcmClient.CheckDevice(this);
      GcmClient.CheckManifest(this);

      // Register for push notifications
      System.Diagnostics.Debug.WriteLine("Registering...");
      GcmClient.Register(this, PushHandlerBroadcastReceiver.SENDER_IDS);
    }
    catch (Java.Net.MalformedURLException)
    {
      CreateAndShowDialog("There was an error creating the client. Verify the URL.", "Error");
    }
    catch (Exception e)
    {
      CreateAndShowDialog(e.Message, "Error");
    }
Declare the function

    private void CreateAndShowDialog(String message, String title)
    {
      AlertDialog.Builder builder = new AlertDialog.Builder(this);

      builder.SetMessage(message);
      builder.SetTitle(title);
      builder.Create().Show();
    }

Your app needs to import Gcm.Client folder into the project
In that folder following classes are present

Constants.cs //Constants for GCM. In that file you also need to provide SENDER_ID as derived from your firebase console application
https://console.firebase.google.com/

GcmBroadcastReceiverBase.cs //Receiver for push notification

GcmService.cs //A service for registering and receiving notification

GcmServiceBase.cs //Base class

InternalGcmClient.cs //Utility class for GCM Client

Following nuggets needs to be present in your application

Xamarin.Android.Support.v7.AppCompat 23.3.0

Xamarin.GooglePlayServices.Gcm 29.0.0.2

After doing all those tasks above, your project is ready to process push notification fired by firebase console

Regards,

Habib Ali

Mohammad Samiullah Farooqi

