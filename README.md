# Firebase-Cloud-Messaging-for-Xamarin-Forms-Android-and-iOS
So many people have asked for convenient and efficient way of implementing FCM (Firebase Cloud Messaging) for android and iOS in Xamarin Form. Actually FCM library cannot be installed in xamarin forms due to version conflict of android support library present in xamarin forms. So here is the work around

Firebase Cloud Messaging (FCM) is a cross-platform service that handles the sending, routing, and queueing of messages between server applications and mobile client apps. FCM is the successor to Google Cloud Messaging (GCM), and it is built on Google Play Services.

## Getting Firebase Ready
___

### Android Firebase Cloud Messaging Setup
1. Login to https://console.firebase.google.com

![Create New Project](https://cloud.githubusercontent.com/assets/5241888/23959819/9519b406-097c-11e7-97c4-6d911f7abdf0.png)

2. Create and name project, the project name does not matter much at this point and can be anything you want.

![Creating Project](https://cloud.githubusercontent.com/assets/5241888/23959899/d0df1666-097c-11e7-80a6-2dd43b9912b1.png)

3. We will start with Android but iOS is the same up to this point. The **Package name** must match your build environment.

![Create Android](https://cloud.githubusercontent.com/assets/5241888/23960094/557c751c-097d-11e7-8806-01243f8bddb7.png)

4. Click **Continue** and **Finish** and let Firebase build your app.
5. Once it's ready go to your Android app settings.

![Android Settings](https://cloud.githubusercontent.com/assets/5241888/23960293/f5613072-097d-11e7-9846-2e07bf276c79.png)

6. Click on **Cloud Messaging** at the top.

![Firebase Menu](https://cloud.githubusercontent.com/assets/5241888/23960358/314db20e-097e-11e7-94ad-3034ebb1db98.png)

7. Under Project credentials you need to save your **Sender ID** as that is required for setting up Android.

![Project Credentails](https://cloud.githubusercontent.com/assets/5241888/23960555/b2e24c62-097e-11e7-8a6f-fc3f7f90c550.png)

### iOS Firebase Cloud Messaging Setup
___
1. Setup a second app (or first if iOS only).

<img width="795" alt="iOS app setup" src="https://cloud.githubusercontent.com/assets/5241888/23960848/775da384-097f-11e7-905f-3f24db3c8cf8.png">

2. Enter the **iOS bundle ID** this **has to** match the project you are going to set up.

<img width="683" alt="iOS Bundle ID setup" src="https://cloud.githubusercontent.com/assets/5241888/23960993/e943336a-097f-11e7-8bc9-cac7521f01c7.png">

3. Download the **GoogleService-Info.plist** as you will need this later in your project, it can be downloaded later under app settings on Firebase.

<img width="666" alt="Info.plist download" src="https://cloud.githubusercontent.com/assets/5241888/23961095/3d746f8a-0980-11e7-986a-a5bd4a3c183e.png">

4. Click **Continue** then **Finish.** Go to the iOS app settings then Cloud Messaging from the top Menu. You will see two places to upload APN certificates, one for Development and one for Production.

<img width="649" alt="Firebase APN push certs" src="https://cloud.githubusercontent.com/assets/5241888/23961414/3811ff48-0981-11e7-8f21-77006a2aa486.png">

5. The easiest way to get App ID Identifiers, Provisioning Profiles, and Certificates set up is to let XCode handle the setup. Open XCode and create a Single View app and give it the **iOS bundle ID** from before. In XCode if you want the Bundle Identifier of com.fcm.sample you give the **Product Name** *sample* and set **Organization Identifier** to *com.fcm*

![xcode setup](https://cloud.githubusercontent.com/assets/5241888/23962344/31db1940-0984-11e7-8ec6-6151b4ab9a71.png)

6. Once the app opens change the **team** to yours and let XCode setup your certificates.

![XCode Team setup](https://cloud.githubusercontent.com/assets/5241888/23962528/cbb1783e-0984-11e7-9454-7662869da724.png)

7. Under **Capabilities** turn *Push Notifications* on

![Turn Push ON](https://cloud.githubusercontent.com/assets/5241888/23962680/4343fd40-0985-11e7-8917-757f8e126c51.png)

8. Further down turn on *Background Modes* and enable *Remote notifications*

![Background Mode ON](https://cloud.githubusercontent.com/assets/5241888/23962772/90a8732c-0985-11e7-86cc-d5177cb61040.png)

9. Log on to https://developer.apple.com, click on *Certificates, IDs & Profiles*, Under *Identifiers* click *App IDs* you will see an ID for your app with a name of *XC com fcm sample* and an ID of *com.fcm.sample*. Click on the ID you are trying to configure with push.

<img width="646" alt="Apple Setup" src="https://cloud.githubusercontent.com/assets/5241888/23963034/5e80f454-0986-11e7-888d-22f726f326c2.png">

10. Click **Edit** then **Create Ceritificate** for your *Development SSL Certificate*

<img width="718" alt="Apple Cert Setup" src="https://cloud.githubusercontent.com/assets/5241888/23963202/eb51a2c0-0986-11e7-9a13-e2e48f1b7e58.png">

11. You will need to generate a **Certificate Signing Request**, open **Keychain Access** on your Mac, Click *Keychain Access* > *Certificate Assistand* > *Request a Certificate From a Certificate Authority...*

<img width="925" alt="Keychain get CSR" src="https://cloud.githubusercontent.com/assets/5241888/23963408/8eaa4580-0987-11e7-9328-8464d608c330.png">

12. Fill out the information and **Save to Disk**

<img width="607" alt="CSR Save to Disk" src="https://cloud.githubusercontent.com/assets/5241888/23963471/c36d599c-0987-11e7-9cfd-79aecff85979.png">

13. Upload your CSR to Apple, you can use the same CSR for both Production and Development.

<img width="725" alt="Cert upload" src="https://cloud.githubusercontent.com/assets/5241888/23963586/1e9be630-0988-11e7-9e77-16708ed2b8bb.png">

14. Download one or both certs and open them with **Keychain Access**. They can be found under *My Certificates* once opened.

<img width="147" alt="My Certs" src="https://cloud.githubusercontent.com/assets/5241888/23963713/93be6438-0988-11e7-9958-1e6f9716ff8e.png">

15. The certs will tell you if they are dev or production, expand them and export the **Private Key**, Give them a strong password, and a proper name.

<img width="814" alt="Dev export" src="https://cloud.githubusercontent.com/assets/5241888/23963897/21683fc0-0989-11e7-8c7e-2ecf10030e0a.png">
<img width="807" alt="Prod export" src="https://cloud.githubusercontent.com/assets/5241888/23963898/2238dd06-0989-11e7-929f-c8114681885c.png">
<img width="444" alt="Password strong" src="https://cloud.githubusercontent.com/assets/5241888/23964027/856f9464-0989-11e7-9394-042c96125815.png">
<img width="567" alt="Name cert" src="https://cloud.githubusercontent.com/assets/5241888/23964030/867d7416-0989-11e7-9ee5-8984d26ff740.png">

16. Upload Certificates to Firebase iOS app and enter the password you created.

<img width="649" alt="Firebase upload certs" src="https://cloud.githubusercontent.com/assets/5241888/23964468/06285f22-098b-11e7-9aeb-d5dc552ede4e.png">
<img width="479" alt="Firebase upload password" src="https://cloud.githubusercontent.com/assets/5241888/23964469/06e00424-098b-11e7-8408-75e1d0d75866.png">

## Done Getting Firebase Ready
___

### Xamarin Forms Android Setup and Info
___

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

### Xamarin Forms iOS Setup and Info
___
1. Add Package *Firebase APIs Cloud Messaging iOS Library*

![Package to add](https://cloud.githubusercontent.com/assets/5241888/23964858/5cb3d992-098c-11e7-9feb-f8cbcb287b9f.png)

2. Make sure info.plist Matches your **iOS Bundle ID** for **Bundle Identifier** the **Application Name** is what the user will see for your installed app.

![Xamarin iOS bundle setup](https://cloud.githubusercontent.com/assets/5241888/23964766/f82c86b8-098b-11e7-9cd4-9ad11aa912eb.png)

3. In the info.plist enable *Background Modes* and enable *Remote notifications* and in *Entitlements.plist* enable *Push Notifications*

![Background Modes](https://cloud.githubusercontent.com/assets/5241888/23965017/fc4ad06e-098c-11e7-8185-f29c8de252fa.png)

4. In **Project > FCM.iOS Options > Build > iOS Bundle Signing** set *Provisioning Profile* to *iOS Team Provisioning Profile: com.fcm.sample* or to the Profile for your **iOS Bundle ID*

![xamarin profile iOS profile setup](https://cloud.githubusercontent.com/assets/5241888/23965513/93e89bc6-098e-11e7-886b-7bb70e25338b.png)

5. Add your *GoogleService-Info.plist* to your project and **remove** the old one, it's only for an example, **Right Click** go to **Build Action** > and check **BundleResource**

![GoogleService plist](https://cloud.githubusercontent.com/assets/5241888/23965864/cd61ce8a-098f-11e7-8118-f96ba98fabb8.png)

When the App starts up it will attempt to register by first asking the user if the app can send Push messages to them, then depending if it's running on iOS >=9 or iOS 10 it will attempt to register to "/topics/all". If the app is open it will alert the user in app. If the app is closed it will display the message like a normal push.

Out of app message.
<img width="479" alt="Out Of App" src="https://cloud.githubusercontent.com/assets/5241888/23966755/f3bb14b2-0992-11e7-9738-af174af4f949.PNG">

In app message.
<img width="479" alt="In App" src="https://cloud.githubusercontent.com/assets/5241888/23966756/f3d2af14-0992-11e7-8ab5-d1955f589661.PNG">


