using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using MATSDK;
#if UNITY_METRO
using MATWinStore;
#endif

/// <para>
/// This class demonstrates the basic features of the MAT Unity Plugin and
/// its ability to have one project work with Android, iOS, and Windows Phone 8.
/// </para>
public class MATSample : MonoBehaviour {
    string MAT_ADVERTISER_ID = null;
    string MAT_CONVERSION_KEY = null;
    string MAT_PACKAGE_NAME = null;

    void Awake ()
    {
        MAT_ADVERTISER_ID = "877";
        MAT_CONVERSION_KEY = "8c14d6bbe466b65211e781d62e301eec";
        MAT_PACKAGE_NAME = "com.hasoffers.unitytestapp";

        print ("Awake called: " + MAT_ADVERTISER_ID + ", " + MAT_CONVERSION_KEY);

        return;
    }

    void OnGUI ()
    {
        GUIStyle headingLabelStyle = new GUIStyle();
        headingLabelStyle.fontStyle = FontStyle.Bold;
        headingLabelStyle.fontSize = 50;
        headingLabelStyle.alignment = TextAnchor.MiddleCenter;
        headingLabelStyle.normal.textColor = Color.white;
        
        GUI.Label(new Rect(10, 5, Screen.width - 20, Screen.height/10), "MAT Unity Test App", headingLabelStyle);
        
        GUI.skin.button.fontSize = 40;
        
        if (GUI.Button (new Rect (10, Screen.height/10, Screen.width - 20, Screen.height/10), "Start MAT SDK"))
        {
            print ("Start MAT SDK clicked");
            #if (UNITY_ANDROID || UNITY_IPHONE || UNITY_METRO)
                MATBinding.Init(MAT_ADVERTISER_ID, MAT_CONVERSION_KEY);
                MATBinding.SetPackageName(MAT_PACKAGE_NAME);
                MATBinding.SetFacebookEventLogging(true, false);
            #endif
            #if (UNITY_ANDROID || UNITY_IPHONE)
                MATBinding.CheckForDeferredDeeplink();
                MATBinding.AutomateIapEventMeasurement(true);
            #endif
        }

        else if (GUI.Button (new Rect (10, 2*Screen.height/10, Screen.width - 20, Screen.height/10), "Set Delegate"))
        {
            print ("Set Delegate clicked");
            #if (UNITY_ANDROID || UNITY_IPHONE)
            MATBinding.SetDelegate(true);
            #endif
            #if UNITY_METRO
            MATBinding.SetMATResponse(new SampleWinMATResponse());
            #endif
        }

        else if (GUI.Button (new Rect (10, 3*Screen.height/10, Screen.width - 20, Screen.height/10), "Enable Debug Mode"))
        {
            print ("Enable Debug Mode clicked");
            // NOTE: !!! ONLY FOR DEBUG !!!
            // !!! Make sure you set to false
            //     OR
            //     remove the setDebugMode and setAllowDuplicates calls for Production builds !!!
            MATBinding.SetDebugMode(true);
        }

        else if (GUI.Button (new Rect (10, 4*Screen.height/10, Screen.width - 20, Screen.height/10), "Allow Duplicates"))
        {
            print ("Allow Duplicates clicked");
            // NOTE: !!! ONLY FOR DEBUG !!!
            // !!! Make sure you set to false
            //     OR
            //     remove the setDebugMode and setAllowDuplicates calls for Production builds !!!
            MATBinding.SetAllowDuplicates(true);
        }

        else if (GUI.Button (new Rect (10, 5*Screen.height/10, Screen.width - 20, Screen.height/10), "Measure Session"))
        {
            print ("Measure Session clicked");
            MATBinding.MeasureSession();
        }

        else if (GUI.Button (new Rect (10, 6*Screen.height/10, Screen.width - 20, Screen.height/10), "Measure Event"))
        {
            print ("Measure Event clicked");
            MATBinding.MeasureEvent("evt11");
        }

        else if (GUI.Button (new Rect (10, 7*Screen.height/10, Screen.width - 20, Screen.height/10), "Measure Event With Event Items"))
        {
            print ("Measure Event With Event Items clicked");
            
            MATItem item1 = new MATItem();
            item1.name = "subitem1";
            item1.unitPrice = 5;
            item1.quantity = 5;
            item1.revenue = 3;
            item1.attribute2 = "attrValue2";
            item1.attribute3 = "attrValue3";
            item1.attribute4 = "attrValue4";
            item1.attribute5 = "attrValue5";
            
            MATItem item2 = new MATItem();
            item2.name = "subitem2";
            item2.unitPrice = 1;
            item2.quantity = 3;
            item2.revenue = 1.5;
            item2.attribute1 = "attrValue1";
            item2.attribute3 = "attrValue3";

            MATItem[] eventItems = { item1, item2 };
            
            MATEvent tuneEvent = new MATEvent("purchase");
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

            #if UNITY_IPHONE
            tuneEvent.receipt = getSampleiTunesIAPReceipt();
            #endif
            
            MATBinding.MeasureEvent(tuneEvent);
        }

        else if (GUI.Button (new Rect (10, 8*Screen.height/10, Screen.width - 20, Screen.height/10), "Test Setter Methods"))
        {
            print ("Test Setter Methods clicked");
            MATBinding.SetAge(34);
            MATBinding.SetAllowDuplicates(true);
            MATBinding.SetAppAdTracking(true);
            MATBinding.SetDebugMode(true);
            MATBinding.SetExistingUser(false);
            MATBinding.SetFacebookUserId("temp_facebook_user_id");
            MATBinding.SetGender(0);
            MATBinding.SetGoogleUserId("temp_google_user_id");
            MATBinding.SetLocation(111,222,333);
            //MATBinding.SetPackageName(MAT_PACKAGE_NAME);
            MATBinding.SetPayingUser(true);
            MATBinding.SetPhoneNumber("111-222-3333");
            MATBinding.SetTwitterUserId("twitter_user_id");
            MATBinding.SetUserId("temp_user_id");
            MATBinding.SetUserName("temp_user_name");
            MATBinding.SetUserEmail("tempuser@tempcompany.com");

            //iOS-specific Features
            #if UNITY_IPHONE

            #if UNITY_5
            MATBinding.SetAppleAdvertisingIdentifier(UnityEngine.iOS.Device.advertisingIdentifier, UnityEngine.iOS.Device.advertisingTrackingEnabled);
            MATBinding.SetAppleVendorIdentifier(UnityEngine.iOS.Device.vendorIdentifier);
            #else
            MATBinding.SetAppleAdvertisingIdentifier(UnityEngine.iPhone.advertisingIdentifier, UnityEngine.iPhone.advertisingTrackingEnabled);
            MATBinding.SetAppleVendorIdentifier(UnityEngine.iPhone.vendorIdentifier);
            #endif

            MATBinding.SetDelegate(true);
            MATBinding.SetJailbroken(false);
            MATBinding.SetShouldAutoCollectAppleAdvertisingIdentifier(true);
            MATBinding.SetShouldAutoCollectDeviceLocation(true);
            MATBinding.SetShouldAutoDetectJailbroken(true);
            MATBinding.SetShouldAutoGenerateVendorIdentifier(true);
            MATBinding.SetUseCookieTracking(false);
            #endif

            //Android-specific Features
            #if UNITY_ANDROID
            MATBinding.SetAndroidId("111111111111");
            MATBinding.SetDeviceId("123456789123456");
            MATBinding.SetGoogleAdvertisingId("12345678-1234-1234-1234-123456789012", true);
            MATBinding.SetMacAddress("AA:BB:CC:DD:EE:FF");
            #endif
            //Android and iOS-specific Features
            #if (UNITY_ANDROID || UNITY_IPHONE)
            MATBinding.SetCurrencyCode("CAD");
            MATBinding.SetTRUSTeId("1234567890");

            MATPreloadData pd = new MATPreloadData("1122334455");
            pd.advertiserSubAd = "some_adv_sub_ad_id";
            pd.publisherSub3 = "some_pub_sub3";
            MATBinding.SetPreloadedApp(pd);
            #endif
        }

        else if (GUI.Button (new Rect (10, 9*Screen.height/10, Screen.width - 20, Screen.height/10), "Test Getter Methods"))
        {
            print ("Test Getter Methods clicked");
            print ("isPayingUser = " + MATBinding.GetIsPayingUser());
            print ("matId     = " + MATBinding.GetMATId());
            print ("openLogId = " + MATBinding.GetOpenLogId());
        }
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
