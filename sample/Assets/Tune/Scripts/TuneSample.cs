using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using TuneSDK;
#if UNITY_METRO
using MATWinStore;
#endif

/// <para>
/// This class demonstrates the basic features of the TUNE Unity Plugin and
/// its ability to have one project work with Android, iOS, and Windows Phone 8.
/// </para>
public class TuneSample : MonoBehaviour {
    string TUNE_ADVERTISER_ID = null;
    string TUNE_CONVERSION_KEY = null;
    string TUNE_PACKAGE_NAME = null;

    static int titleFontSize = 50;
    static bool isTitleBold = true;

    Vector2 scrollPosition = Vector2.zero;

    void Awake ()
    {
        TUNE_ADVERTISER_ID = "877";
        TUNE_CONVERSION_KEY = "8c14d6bbe466b65211e781d62e301eec";
        TUNE_PACKAGE_NAME = "com.hasoffers.unitytestapp";

        print ("Awake called: " + TUNE_ADVERTISER_ID + ", " + TUNE_CONVERSION_KEY);

        #if UNITY_IOS
        UnityEngine.iOS.NotificationServices.RegisterForNotifications (UnityEngine.iOS.NotificationType.Alert |  UnityEngine.iOS.NotificationType.Badge |  UnityEngine.iOS.NotificationType.Sound);
        #endif

        return;
    }

    void Update()  // make it scroll with the user's finger
    {
        if (Input.touchCount == 0) return;
        Touch touch = Input.touches[0];
        if (touch.phase == TouchPhase.Moved)
        {
            float dt = Time.deltaTime / touch.deltaTime;
            if (dt == 0 || float.IsNaN(dt) || float.IsInfinity(dt))
                dt = 1.0f;
            Vector2 glassDelta = touch.deltaPosition * dt;

            scrollPosition.y += glassDelta.y;
        }
    }

    void OnGUI ()
    {
        GUIStyle headingLabelStyle = new GUIStyle();
        headingLabelStyle.fontStyle = isTitleBold ? FontStyle.Bold : FontStyle.Normal;
        headingLabelStyle.fontSize = titleFontSize;
        headingLabelStyle.alignment = TextAnchor.MiddleCenter;
        headingLabelStyle.normal.textColor = Color.white;

        GUI.skin.button.fontSize = 40;

        GUI.Label(new Rect(10, 5, Screen.width - 20, Screen.height/10), "TUNE Unity Test App", headingLabelStyle);

        scrollPosition = GUI.BeginScrollView(new Rect(10, 5 + Screen.height/10, Screen.width - 20, Screen.height), scrollPosition, new Rect(10, 0, Screen.width - 20, Screen.height * 1.2f), GUIStyle.none, GUIStyle.none);

        if (GUI.Button (new Rect (10, 0, Screen.width - 20, Screen.height/10), "Start TUNE SDK"))
        {
            print ("Start TUNE SDK clicked");
            #if (UNITY_ANDROID || UNITY_IOS || UNITY_METRO)
            Tune.Init(TUNE_ADVERTISER_ID, TUNE_CONVERSION_KEY);
            Tune.SetPackageName(TUNE_PACKAGE_NAME);
            Tune.SetFacebookEventLogging(true, false);
            #endif
            #if (UNITY_ANDROID || UNITY_IOS)
            Tune.CheckForDeferredDeeplink();
            Tune.AutomateIapEventMeasurement(true);

            // Demo In-App Marketing API calls
            Tune.RegisterPowerHook("hookId", "friendlyName", "defaultValue");
            Tune.SetPushNotificationSenderId("1080799636982");

            Tune.RegisterCustomProfileString("customString", "val1");
            Tune.RegisterCustomProfileDate("customDate", new DateTime(2015, 6, 1));
            Tune.RegisterCustomProfileNumber("customInt", 1);
            Tune.RegisterCustomProfileNumber("customDouble", 0.99);
            Tune.RegisterCustomProfileNumber("customFloat", 5.99f);
            TuneLocation loc = new TuneLocation();
            loc.latitude = 55.2;
            loc.longitude = 122.44;
            Tune.RegisterCustomProfileGeoLocation("customGeo", loc);

            Tune.RegisterPowerHook("titleFontBold", "Is Title Text Font Bold", "false");
            Tune.RegisterPowerHook("titleFontSize", "Title Text Font Size", "25");

            #endif
        }

        else if (GUI.Button (new Rect (10, 1*Screen.height/10, Screen.width - 20, Screen.height/10), "Set Delegate"))
        {
            print ("Set Delegate clicked");
            #if (UNITY_ANDROID || UNITY_IOS)
            Tune.SetDelegate(true);
            Tune.OnPowerHooksChanged(true);
            Tune.OnFirstPlaylistDownloaded(true);
            #endif
            #if UNITY_METRO
            Tune.SetMATResponse(new SampleWinMATResponse());
            #endif
        }

        else if (GUI.Button (new Rect (10, 2*Screen.height/10, Screen.width - 20, Screen.height/10), "Enable Debug Mode"))
        {
            print ("Enable Debug Mode clicked");
            // NOTE: !!! ONLY FOR DEBUG !!!
            // !!! Make sure you set to false
            //     OR
            //     remove the setDebugMode and setAllowDuplicates calls for Production builds !!!
            Tune.SetDebugMode(true);
        }

        else if (GUI.Button (new Rect (10, 3*Screen.height/10, Screen.width - 20, Screen.height/10), "Measure Session"))
        {
            print ("Measure Session clicked");
            Tune.MeasureSession();
        }

        else if (GUI.Button (new Rect (10, 4*Screen.height/10, Screen.width - 20, Screen.height/10), "Measure Event"))
        {
            print ("Measure Event clicked");
            Tune.MeasureEvent("evt11");
        }

        else if (GUI.Button (new Rect (10, 5*Screen.height/10, Screen.width - 20, Screen.height/10), "Measure Event With Event Items"))
        {
            print ("Measure Event With Event Items clicked");

            TuneItem item1 = new TuneItem("subitem1");
            item1.unitPrice = 5;
            item1.quantity = 5;
            item1.revenue = 3;
            item1.attribute2 = "attrValue12";
            item1.attribute3 = "attrValue13";
            item1.attribute4 = "attrValue14";
            item1.attribute5 = "attrValue15";

            TuneItem item2 = new TuneItem("subitem2");
            item2.unitPrice = 1;
            item2.quantity = 3;
            item2.revenue = 1.5;
            item2.attribute1 = "attrValue21";
            item2.attribute3 = "attrValue23";
            TuneItem[] eventItems = { item1, item2 };

            TuneEvent tuneEvent = new TuneEvent("purchase");
            tuneEvent.revenue = 10;
            tuneEvent.currencyCode = "AUD";
            tuneEvent.advertiserRefId = "ref222";
            tuneEvent.attribute1 = "test_attribute1";
            tuneEvent.attribute2 = "test_attribute2";
            tuneEvent.attribute3 = "test_attribute3";
            tuneEvent.attribute4 = "test_attribute4";
            tuneEvent.attribute5 = "test_attribute5";
            tuneEvent.contentType = "test_contentType";
            tuneEvent.contentId = "test_contentId";
            tuneEvent.date1 = DateTime.UtcNow;
            tuneEvent.date2 = DateTime.UtcNow.Add(new TimeSpan((new DateTime(2, 1, 1).Ticks)));
            tuneEvent.level = 3;
            tuneEvent.quantity = 2;
            tuneEvent.rating = 4.5;
            tuneEvent.searchString = "test_searchString";
            tuneEvent.eventItems = eventItems;
            // transaction state may be set to the value received from the iOS/Android app store.
            tuneEvent.transactionState = 1;

            #if UNITY_IOS
            tuneEvent.receipt = getSampleiTunesIAPReceipt();
            #endif

            Tune.MeasureEvent(tuneEvent);
        }

        else if (GUI.Button (new Rect (10, 6*Screen.height/10, Screen.width - 20, Screen.height/10), "Test Setter Methods"))
        {
            print ("Test Setter Methods clicked");
            Tune.SetAge(34);
            Tune.SetAppAdTracking(true);
            Tune.SetDebugMode(true);
            Tune.SetExistingUser(false);
            Tune.SetFacebookUserId("temp_facebook_user_id");
            Tune.SetGender(0);
            Tune.SetGoogleUserId("temp_google_user_id");
            Tune.SetLocation(111,222,333);
            //Tune.SetPackageName(MAT_PACKAGE_NAME);
            Tune.SetPayingUser(true);
            Tune.SetPhoneNumber("111-222-3333");
            Tune.SetTwitterUserId("twitter_user_id");
            Tune.SetUserId("temp_user_id");
            Tune.SetUserName("temp_user_name");
            Tune.SetUserEmail("tempuser@tempcompany.com");
            Tune.SetDeepLink("myapp://myval1/myval2");

            //iOS-specific Features
            #if UNITY_IOS

            #if UNITY_5
            Tune.SetAppleAdvertisingIdentifier(UnityEngine.iOS.Device.advertisingIdentifier, UnityEngine.iOS.Device.advertisingTrackingEnabled);
            Tune.SetAppleVendorIdentifier(UnityEngine.iOS.Device.vendorIdentifier);
            #else
            Tune.SetAppleAdvertisingIdentifier(UnityEngine.iPhone.advertisingIdentifier, UnityEngine.iPhone.advertisingTrackingEnabled);
            Tune.SetAppleVendorIdentifier(UnityEngine.iPhone.vendorIdentifier);
            #endif

            Tune.SetDelegate(true);
            Tune.SetJailbroken(false);
            Tune.SetShouldAutoCollectAppleAdvertisingIdentifier(true);
            Tune.SetShouldAutoCollectDeviceLocation(true);
            Tune.SetShouldAutoDetectJailbroken(true);
            Tune.SetShouldAutoGenerateVendorIdentifier(true);
            Tune.SetUseCookieTracking(false);
            #endif

            //Android-specific Features
            #if UNITY_ANDROID
            Tune.SetAndroidId("111111111111");
            Tune.SetDeviceId("123456789123456");
            Tune.SetGoogleAdvertisingId("12345678-1234-1234-1234-123456789012", true);
            Tune.SetMacAddress("AA:BB:CC:DD:EE:FF");
            #endif
            //Android and iOS-specific Features
            #if (UNITY_ANDROID || UNITY_IOS)
            Tune.SetCurrencyCode("CAD");
            Tune.SetTRUSTeId("1234567890");

            TunePreloadData pd = new TunePreloadData("1122334455");
            pd.advertiserSubAd = "some_adv_sub_ad_id";
            pd.publisherSub3 = "some_pub_sub3";
            Tune.SetPreloadedApp(pd);
            #endif
        }

        else if (GUI.Button (new Rect (10, 7*Screen.height/10, Screen.width - 20, Screen.height/10), "Test Getter Methods"))
        {
            print ("Test Getter Methods clicked");
            print ("isPayingUser = " + Tune.GetIsPayingUser());
            print ("tuneId     = " + Tune.GetTuneId());
            print ("openLogId = " + Tune.GetOpenLogId());
        }

        else if (GUI.Button (new Rect (10, 8*Screen.height/10, Screen.width - 20, Screen.height/10), "Get Custom Profile Variables"))
        {
            print ("Get Custom Profile Variables clicked");
            print ("Custom string = " + Tune.GetCustomProfileString("customString"));
            DateTime customDate = Tune.GetCustomProfileDate("customDate");
            print ("Custom date = " + customDate.ToString("MM/dd/yyyy"));
            print ("Custom int = " + Tune.GetCustomProfileNumber("customInt"));
            print ("Custom double = " + Tune.GetCustomProfileNumber("customDouble"));
            print ("Custom float = " + Tune.GetCustomProfileNumber("customFloat"));
            TuneLocation customLoc = Tune.GetCustomProfileGeolocation("customGeo");
            print ("Custom location = " + customLoc.latitude.ToString() + ", " + customLoc.longitude.ToString());
        }

        else if (GUI.Button (new Rect (10, 9*Screen.height/10, Screen.width - 20, Screen.height/10), "Get Experiment Details"))
        {
            print ("Get Experiment Details clicked");
            Dictionary<string, TunePowerHookExperimentDetails> dict = Tune.GetPowerHookExperimentDetails();
            foreach (KeyValuePair<string, TunePowerHookExperimentDetails> entry in dict)
            {
                // do something with entry.Value or entry.Key
                print("key is " + entry.Key);
                print("experiment name is " + entry.Value.experimentName);
                print("experiment id is " + entry.Value.experimentId);
                print("experiment type is " + entry.Value.experimentType);
                print("variant id is " + entry.Value.currentVariantId);
                print("variant name is " + entry.Value.currentVariantName);
                print("variant letter is " + entry.Value.currentVariantLetter);
                print("running is " + entry.Value.isRunning);
            }
        }

        else if (GUI.Button (new Rect (10, 10*Screen.height/10, Screen.width - 20, Screen.height/10), "Test Power Hooks"))
        {
            string titleFontBoldPowerHookVariable = Tune.GetValueForHookById("titleFontBold");
            string titleFontSizePowerHookVariable = Tune.GetValueForHookById("titleFontSize");

            string sampleHook = Tune.GetValueForHookById("hookId");
            print ("power hook values1: sample hook = " + sampleHook);
            print ("power hook values2: sample hook = " + Tune.GetValueForHookById("hookId"));

            print ("power hook values: isBold = " + titleFontBoldPowerHookVariable + ", size = " + titleFontSizePowerHookVariable);

            isTitleBold = titleFontBoldPowerHookVariable.Equals("true");
            titleFontSize = int.Parse(titleFontSizePowerHookVariable);
        }

        GUI.EndScrollView();
    }

    public static string getSampleiTunesIAPReceipt ()
    {
        return "dGhpcyBpcyBhIHNhbXBsZSBpb3MgYXBwIHN0b3JlIHJlY2VpcHQ=";
    }
}

#if UNITY_METRO
// Used to test Windows MATResponse functionality
public class SampleWinMATResponse : MATResponse
{
    // Make sure to attach MATDelegate.cs to the empty "MobileAppTracker" object
    MATDelegate message_receiver = GameObject.Find("MobileAppTracker").GetComponent<MATDelegate>();

    public void DidSucceedWithData(string response)
    {
        if (message_receiver != null)
            message_receiver.trackerDidSucceed("" + response);
    }

    public void DidFailWithError(string error)
    {
        if (message_receiver != null)
            message_receiver.trackerDidFail("" + error);
    }

    public void EnqueuedActionWithRefId(string refId)
    {
        if (message_receiver != null)
            message_receiver.trackerDidEnqueueRequest("" + refId);
    }
}
#endif
