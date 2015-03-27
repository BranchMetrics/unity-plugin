using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using com.mobileapptracking;

/// <para>
/// This class demonstrates the basic features of the MAT Unity Plugin and
/// its ability to have one project work with Android, iOS, and Windows Phone 8.
/// </para>
using System.Text;


public class MATSample : MonoBehaviour {

    #if (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8 || UNITY_METRO)
    string MAT_ADVERTISER_ID = null;
    string MAT_CONVERSION_KEY = null;
    string MAT_PACKAGE_NAME = null;
    #endif

    void Awake ()
    {
        #if (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8 || UNITY_METRO)
        MAT_ADVERTISER_ID = "877";
        MAT_CONVERSION_KEY = "8c14d6bbe466b65211e781d62e301eec";
        MAT_PACKAGE_NAME = "com.hasoffers.unitytestapp";

        print ("Awake called: " + MAT_ADVERTISER_ID + ", " + MAT_CONVERSION_KEY);
        #endif

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
        
        if (GUI.Button (new Rect (10, Screen.height/10, Screen.width - 20, Screen.height/10), "Start MAT"))
        {
            print ("Start MAT clicked");
            #if (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8 || UNITY_METRO)
                MATBinding.Init(MAT_ADVERTISER_ID, MAT_CONVERSION_KEY);
                MATBinding.SetPackageName(MAT_PACKAGE_NAME);
                MATBinding.SetFacebookEventLogging(true, false);
            #endif
            #if UNITY_IPHONE
                MATBinding.CheckForDeferredDeeplinkWithTimeout(750); // 750 ms
            #endif
        }

        else if (GUI.Button (new Rect (10, 2*Screen.height/10, Screen.width - 20, Screen.height/10), "Set Delegate"))
        {
            print ("Set Delegate clicked");
            #if (UNITY_ANDROID || UNITY_IPHONE)
            MATBinding.SetDelegate(true);

            #endif
            #if UNITY_WP8
            MATBinding.SetMATResponse(new SampleWP8MATResponse());
            #endif
            #if UNITY_METRO
            MATBinding.SetMATResponse(new SampleWinStoreMATResponse());
            #endif
        }

        else if (GUI.Button (new Rect (10, 3*Screen.height/10, Screen.width - 20, Screen.height/10), "Enable Debug Mode"))
        {
            print ("Enable Debug Mode clicked");
            #if (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8 || UNITY_METRO)
            // NOTE: !!! ONLY FOR DEBUG !!!
            // !!! Make sure you set to false
            //     OR
            //     remove the setDebugMode and setAllowDuplicates calls for Production builds !!!
            MATBinding.SetDebugMode(true);
            #endif
        }

        else if (GUI.Button (new Rect (10, 4*Screen.height/10, Screen.width - 20, Screen.height/10), "Allow Duplicates"))
        {
            print ("Allow Duplicates clicked");
            #if (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8 || UNITY_METRO)
            // NOTE: !!! ONLY FOR DEBUG !!!
            // !!! Make sure you set to false
            //     OR
            //     remove the setDebugMode and setAllowDuplicates calls for Production builds !!!
            MATBinding.SetAllowDuplicates(true);
            #endif
        }

        else if (GUI.Button (new Rect (10, 5*Screen.height/10, Screen.width - 20, Screen.height/10), "Measure Session"))
        {
            print ("Measure Session clicked");
            #if (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8 || UNITY_METRO)
            MATBinding.MeasureSession();
            #endif
        }

        else if (GUI.Button (new Rect (10, 6*Screen.height/10, Screen.width - 20, Screen.height/10), "Measure Action"))
        {
            print ("Measure Action clicked");
            #if (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8 || UNITY_METRO)
            MATBinding.MeasureAction("evt11");
            MATBinding.MeasureActionWithRefId("evt12", "ref111");
            MATBinding.MeasureActionWithRevenue("evt13", 0.35, "CAD", "ref112");
            #endif
        }

        else if (GUI.Button (new Rect (10, 7*Screen.height/10, Screen.width - 20, Screen.height/10), "Measure Action With Event Items"))
        {
            print ("Measure Action With Event Items clicked");
            
            #if (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8 || UNITY_METRO)
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
            
            MATItem[] arr = { item1, item2 };
            
            // transaction state may be set to the value received from the iOS/Android app store.
            int transactionState = 1;
            
            string receiptData = null;
            string receiptSignature = null;
            
            #if UNITY_IPHONE
            receiptData = getSampleiTunesIAPReceipt();
            #endif

            MATBinding.MeasureActionWithEventItems("event7WithReceipt", arr, 10.0, @"AUD", "ref222", transactionState, receiptData, receiptSignature);

            #endif
        }

        else if (GUI.Button (new Rect (10, 8*Screen.height/10, Screen.width - 20, Screen.height/10), "Test Setter Methods"))
        {
            print ("Test Setter Methods clicked");
            #if (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8 || UNITY_METRO)
            MATBinding.SetAge(34);
            MATBinding.SetAllowDuplicates(true);
            MATBinding.SetAppAdTracking(true);
            MATBinding.SetDebugMode(true);
            MATBinding.SetEventAttribute1("test_attribute1");
            MATBinding.SetEventAttribute2("test_attribute2");
            MATBinding.SetEventAttribute3("test_attribute3");
            MATBinding.SetEventAttribute4("test_attribute4");
            MATBinding.SetEventAttribute5("test_attribute5");
            MATBinding.SetEventContentType("testContentType");
            MATBinding.SetEventContentId("testContentId");
            MATBinding.SetEventDate1(DateTime.UtcNow);
            MATBinding.SetEventDate2(DateTime.UtcNow.Add(new TimeSpan((new DateTime(2,1,1)).Ticks)));
            MATBinding.SetEventLevel(3);
            MATBinding.SetEventQuantity(2);
            MATBinding.SetEventRating(4.5f);
            MATBinding.SetEventSearchString("testSearchString");
            MATBinding.SetExistingUser(false);
            MATBinding.SetFacebookUserId("temp_facebook_user_id");
            MATBinding.SetGender(0);
            MATBinding.SetGoogleUserId("temp_google_user_id");
            MATBinding.SetLocation(111,222,333);
            //MATBinding.SetPackageName(MAT_PACKAGE_NAME);
            MATBinding.SetPayingUser(true);
            MATBinding.SetTwitterUserId("twitter_user_id");
            MATBinding.SetUserId("temp_user_id");
            MATBinding.SetUserName("temp_user_name");
            MATBinding.SetUserEmail("tempuser@tempcompany.com");
            //iOS-specific Features
            #if UNITY_IPHONE
            #if UNITY_5_0
            MATBinding.SetAppleAdvertisingIdentifier(UnityEngine.iOS.Device.advertisingIdentifier, UnityEngine.iOS.Device.advertisingTrackingEnabled);
            #else
            MATBinding.SetAppleAdvertisingIdentifier(UnityEngine.iPhone.advertisingIdentifier, UnityEngine.iPhone.advertisingTrackingEnabled);
            #endif
            MATBinding.SetAppleVendorIdentifier("87654321-4321-4321-4321-210987654321");
            MATBinding.SetDelegate(true);
            MATBinding.SetJailbroken(false);
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
            //MATBinding.SetPublisherId("");
            //MATBinding.SetOfferId("");
            MATBinding.SetPublisherReferenceId("publisher_ref_id");
            MATBinding.SetPublisherSub1("publisher_sub1");
            MATBinding.SetPublisherSub2("publisher_sub2");
            MATBinding.SetPublisherSub3("publisher_sub3");
            MATBinding.SetPublisherSub4("publisher_sub4");
            MATBinding.SetPublisherSub5("publisher_sub5");
            MATBinding.SetPublisherSubAd("publisher_sub_ad");
            MATBinding.SetPublisherSubAdgroup("publisher_sub_adgroup");
            MATBinding.SetPublisherSubCampaign("publisher_sub_campaign");
            MATBinding.SetPublisherSubKeyword("publisher_sub_keyword");
            MATBinding.SetPublisherSubPublisher("publisher_sub_publisher");
            MATBinding.SetPublisherSubSite("publisher_sub_site");
            MATBinding.SetAdvertiserSubAd("advertiser_sub_ad");
            MATBinding.SetAdvertiserSubAdgroup("advertiser_sub_adgroup");
            MATBinding.SetAdvertiserSubCampaign("advertiser_sub_campaign");
            MATBinding.SetAdvertiserSubKeyword("advertiser_sub_keyword");
            MATBinding.SetAdvertiserSubPublisher("advertiser_sub_publisher");
            MATBinding.SetAdvertiserSubSite("advertiser_sub_site");
            #endif
            //Windows Phone 8 Specific Features
            #if UNITY_WP8
            MATBinding.SetAppName("testWP8_AppName");
            MATBinding.SetAppVersion("testWP8_AppVersion");
            MATBinding.SetLastOpenLogId("testWP8_LastOpenLogId");
            MATBinding.SetOSVersion("testWP8_OS");
            #endif
            //Android and iOS-specific Features
            #if (UNITY_ANDROID || UNITY_IPHONE)
            MATBinding.SetCurrencyCode("CAD");
            MATBinding.SetTRUSTeId("1234567890");
            #endif

            #endif
        }

        else if (GUI.Button (new Rect (10, 9*Screen.height/10, Screen.width - 20, Screen.height/10), "Test Getter Methods"))
        {
            print ("Test Getter Methods clicked");
            #if (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8 || UNITY_METRO)
            print ("isPayingUser = " + MATBinding.GetIsPayingUser());
            print ("matId     = " + MATBinding.GetMATId());
            print ("openLogId = " + MATBinding.GetOpenLogId());
            #endif
        }
    }

    public static string getSampleiTunesIAPReceipt ()
    {
		return "dGhpcyBpcyBhIHNhbXBsZSBpb3MgYXBwIHN0b3JlIHJlY2VpcHQ=";
    }
}

// Used to test MATResponse functionality
public class SampleWP8MATResponse : MATWP8.MATResponse
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

// Used to test MATResponse functionality
public class SampleWinStoreMATResponse : MATWinStore.MATResponse
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
