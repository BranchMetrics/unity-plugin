using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class MATSampleScript : MonoBehaviour
{
    public struct MATItem
    {
        public string   name;
        public double   unitPrice;
        public int      quantity;
        public double   revenue;
        public string   attribute1;
        public string   attribute2;
        public string   attribute3;
        public string   attribute4;
        public string   attribute5;
    }

    #if UNITY_ANDROID
        // Pass the name of the plugin's dynamic library.
        // Import any functions we will be using from the MAT lib.
        // (I've listed them all here)
        [DllImport ("mobileapptracker")]
        private static extern void initNativeCode(string advertiserId, string conversionKey);

        // Methods to help debugging and testing
        [DllImport ("mobileapptracker")]
        private static extern void setAllowDuplicates(bool allowDups);
        [DllImport ("mobileapptracker")]
        private static extern void setDebugMode(bool enable);

        [DllImport ("mobileapptracker")]
        private static extern void setAge(int age);
        [DllImport ("mobileapptracker")]
        private static extern void setAppAdTracking(bool enable);
        [DllImport ("mobileapptracker")]
        private static extern void setCurrencyCode(string currencyCode);
        [DllImport ("mobileapptracker")]
        private static extern void setEventAttribute1(string value);
        [DllImport ("mobileapptracker")]
        private static extern void setEventAttribute2(string value);
        [DllImport ("mobileapptracker")]
        private static extern void setEventAttribute3(string value);
        [DllImport ("mobileapptracker")]
        private static extern void setEventAttribute4(string value);
        [DllImport ("mobileapptracker")]
        private static extern void setEventAttribute5(string value);
        [DllImport ("mobileapptracker")]
        private static extern void setExistingUser(bool isExisting);
        [DllImport ("mobileapptracker")]
        private static extern void setFacebookUserId(string facebookUserId);
        [DllImport ("mobileapptracker")]
        private static extern void setGender(int gender);
        [DllImport ("mobileapptracker")]
        private static extern void setGoogleAdvertisingId(string advertisingId, bool limitAdTracking);
        [DllImport ("mobileapptracker")]
        private static extern void setGoogleUserId(string googleUserId);
        [DllImport ("mobileapptracker")]
        private static extern void setLocation(double latitude, double longitude, double altitude);
        [DllImport ("mobileapptracker")]
        private static extern void setPackageName(string packageName);
        [DllImport ("mobileapptracker")]
        private static extern void setSiteId(string siteId);
        [DllImport ("mobileapptracker")]
        private static extern void setTRUSTeId(string trusteId);
        [DllImport ("mobileapptracker")]
        private static extern void setTwitterUserId(string twitterUserId);
        [DllImport ("mobileapptracker")]
        private static extern void setUserEmail(string userEmail);
        [DllImport ("mobileapptracker")]
        private static extern void setUserId(string userId);
        [DllImport ("mobileapptracker")]
        private static extern void setUserName(string userName);

        [DllImport ("mobileapptracker")]
        private static extern void startAppToAppTracking(string targetAppId, string advertiserId, string offerId, string publisherId, bool shouldRedirect);

        // Tracking functions
        [DllImport ("mobileapptracker")]
        private static extern void measureAction(string action, double revenue, string currencyCode, string refId);
        [DllImport ("mobileapptracker")]
        private static extern void measureActionWithEventItems(string action, MATItem[] items, int eventItemCount, string refId, double revenue, string currency, int transactionState, string receiptData, string receiptSignature);
        [DllImport ("mobileapptracker")]
        private static extern int measureSession();

        // iOS-only functions that are imported for cross-platform coding convenience
        [DllImport ("mobileapptracker")]
        private static extern void setAppleAdvertisingIdentifier(string advertiserIdentifier, bool trackingEnabled);
        [DllImport ("mobileapptracker")]
        private static extern void setAppleVendorIdentifier(string vendorIdentifier);
        [DllImport ("mobileapptracker")]
        private static extern void setDelegate(bool enable);
        [DllImport ("mobileapptracker")]
        private static extern void setJailbroken(bool isJailbroken);
        [DllImport ("mobileapptracker")]
        private static extern void setRedirectUrl(string redirectUrl);
        [DllImport ("mobileapptracker")]
        private static extern void setShouldAutoDetectJailbroken(bool shouldAutoDetect);
        [DllImport ("mobileapptracker")]
        private static extern void setShouldAutoGenerateAppleVendorIdentifier(bool shouldAutoGenerate);
        [DllImport ("mobileapptracker")]
        private static extern void setUseCookieTracking(bool useCookieTracking);
        
        [DllImport ("mobileapptracker")]
        private static extern bool getIsPayingUser();
        [DllImport ("mobileapptracker")]
        private static extern string getMatId();
        [DllImport ("mobileapptracker")]
        private static extern string getOpenLogId();

    #endif

    #if UNITY_IPHONE
        // Main initializer method for MAT
        [DllImport ("__Internal")]
        private static extern void initNativeCode(string advertiserId, string conversionKey);

        // Methods to help debugging and testing
        [DllImport ("__Internal")]
        private static extern void setDebugMode(bool enable);
        [DllImport ("__Internal")]
        private static extern void setAllowDuplicates(bool allowDuplicateRequests);

        // Method to enable MAT delegate success/failure callbacks
        [DllImport ("__Internal")]
        private static extern void setDelegate(bool enable);

        // Optional Setter Methods
        [DllImport ("__Internal")]
        private static extern void setAppAdTracking(bool enable);
        [DllImport ("__Internal")]
        private static extern void setCurrencyCode(string currencyCode);
        [DllImport ("__Internal")]
        private static extern void setPackageName(string packageName);
        [DllImport ("__Internal")]
        private static extern void setSiteId(string siteId);
        [DllImport ("__Internal")]
        private static extern void setTRUSTeId(string trusteTPID);
        [DllImport ("__Internal")]
        private static extern void setUserEmail(string userEmail);
        [DllImport ("__Internal")]
        private static extern void setUserId(string userId);
        [DllImport ("__Internal")]
        private static extern void setUserName(string userName);
        [DllImport ("__Internal")]
        private static extern void setFacebookUserId(string facebookUserId);
        [DllImport ("__Internal")]
        private static extern void setTwitterUserId(string twitterUserId);
        [DllImport ("__Internal")]
        private static extern void setGoogleUserId(string googleUserId);
        [DllImport ("__Internal")]
		private static extern bool getIsPayingUser();
        [DllImport ("__Internal")]
        private static extern void setExistingUser(bool isExisting);
        [DllImport ("__Internal")]
        private static extern void setJailbroken(bool isJailbroken);
        [DllImport ("__Internal")]
        private static extern void setShouldAutoDetectJailbroken(bool shouldAutoDetect);
        [DllImport ("__Internal")]
        private static extern void setAge(int age);
        [DllImport ("__Internal")]
        private static extern void setGender(int gender);
        [DllImport ("__Internal")]
        private static extern void setLocation(double latitude, double longitude, double altitude);
        [DllImport ("__Internal")]
        private static extern void setEventAttribute1(string value);
        [DllImport ("__Internal")]
        private static extern void setEventAttribute2(string value);
        [DllImport ("__Internal")]
        private static extern void setEventAttribute3(string value);
        [DllImport ("__Internal")]
        private static extern void setEventAttribute4(string value);
        [DllImport ("__Internal")]
        private static extern void setEventAttribute5(string value);

        // Method to enable cookie based tracking
        [DllImport ("__Internal")]
        private static extern void setUseCookieTracking(bool useCookieTracking);

        // Methods for setting Apple related id
        [DllImport ("__Internal")]
        private static extern void setAppleAdvertisingIdentifier(string appleAdvertisingIdentifier, bool trackingEnabled);
        [DllImport ("__Internal")]
        private static extern void setAppleVendorIdentifier(string appleVendorIdentifier);
        [DllImport ("__Internal")]
        private static extern void setShouldAutoGenerateAppleVendorIdentifier(bool shouldAutoGenerate);
        
        // Methods for app-to-app tracking
        [DllImport ("__Internal")]
        private static extern void startAppToAppTracking(string targetAppId, string advertiserId, string offerId, string publisherId, bool shouldRedirect);
        [DllImport ("__Internal")]
        private static extern void setRedirectUrl(string redirectUrl);

        // Methods to track custom in-app events
        [DllImport ("__Internal")]
        private static extern void measureAction(string action, double revenue, string  currencyCode, string refId);
        [DllImport ("__Internal")]
        private static extern void measureActionWithEventItems(string action, MATItem[] items, int eventItemCount, string refId, double revenue, string currency, int transactionState, string receipt, string receiptSignature);

        // Methods to track install, update events
        [DllImport ("__Internal")]
        private static extern void measureSession();

        [DllImport ("__Internal")]
        private static extern string getMatId();
        [DllImport ("__Internal")]
        private static extern string getOpenLogId();

        // Android-only methods
        [DllImport ("__Internal")]
        private static extern void setGoogleAdvertisingId(string advertisingId, bool limitAdTracking);

    #endif

    #if UNITY_ANDROID || UNITY_IPHONE
        string MAT_ADVERTISER_ID = null;
        string MAT_CONVERSION_KEY = null;
    #endif

    void Awake ()
    {
        #if UNITY_ANDROID || UNITY_IPHONE
            MAT_ADVERTISER_ID = "877";
            MAT_CONVERSION_KEY = "8c14d6bbe466b65211e781d62e301eec";

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

        GUI.Label(new Rect(10, 5, Screen.width - 20, 90), "MAT Unity Test App", headingLabelStyle);

        GUI.skin.button.fontSize = 40;

        if (GUI.Button (new Rect (10, 110, Screen.width - 20, 90), "Start MAT"))
        {
            print ("Start MAT clicked");
            #if UNITY_ANDROID || UNITY_IPHONE
                initNativeCode(MAT_ADVERTISER_ID, MAT_CONVERSION_KEY);

                #if UNITY_IPHONE
                    setAppleAdvertisingIdentifier(iPhone.advertisingIdentifier, iPhone.advertisingTrackingEnabled);
                #endif
            #endif
        }

        if (GUI.Button (new Rect (10, 210, Screen.width - 20, 90), "Set Delegate"))
        {
            print ("Set Delegate clicked");
            #if UNITY_ANDROID || UNITY_IPHONE
                setDelegate(true);
            #endif
        }

        if (GUI.Button (new Rect (10, 310, Screen.width - 20, 90), "Enable Debug Mode"))
        {
            print ("Enable Debug Mode clicked");
            #if UNITY_ANDROID || UNITY_IPHONE
            // NOTE: !!! ONLY FOR DEBUG !!!
            // !!! Make sure you set to false 
            //     OR 
            //     remove the setDebugMode and setAllowDuplicates calls for Production builds !!!
                setDebugMode(true);
            #endif
        }

        if (GUI.Button (new Rect (10, 410, Screen.width - 20, 90), "Allow Duplicates"))
        {
            print ("Allow Duplicates clicked");
            #if UNITY_ANDROID || UNITY_IPHONE
                // NOTE: !!! ONLY FOR DEBUG !!!
                // !!! Make sure you set to false 
                //     OR 
                //     remove the setDebugMode and setAllowDuplicates calls for Production builds !!!
                setAllowDuplicates(true);
            #endif
        }

        if (GUI.Button (new Rect (10, 510, Screen.width - 20, 90), "Track Session"))
        {
            print ("Track Session clicked");
            #if UNITY_ANDROID || UNITY_IPHONE
                measureSession();
            #endif
        }

        if (GUI.Button (new Rect (10, 610, Screen.width - 20, 90), "Track Action"))
        {
            print ("Track Action clicked");
            #if UNITY_ANDROID || UNITY_IPHONE
                measureAction("evt11", 0.35, "CAD", "ref111");
            #endif
        }

        if (GUI.Button (new Rect (10, 710, Screen.width - 20, 90), "Track Action With Event Items"))
        {
            print ("Track Action With Event Items clicked");

            #if UNITY_ANDROID || UNITY_IPHONE
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

                measureActionWithEventItems("event6With2Items", arr, arr.Length, "ref222", 10, null, transactionState, receiptData, receiptSignature);
            #endif
        }

        if (GUI.Button (new Rect (10, 810, Screen.width - 20, 90), "Test Setter Methods"))
        {
            print ("Test Setter Methods clicked");
            #if UNITY_ANDROID || UNITY_IPHONE
                setAge(34);
                setAllowDuplicates(true);
                setAppAdTracking(true);
                setAppleAdvertisingIdentifier("12345678-1234-1234-1234-123456789012", true);
                setAppleVendorIdentifier("87654321-4321-4321-4321-210987654321");
                setCurrencyCode("CAD");
                setDebugMode(true);
                setDelegate(true);
                setEventAttribute1("test_attribute1");
                setEventAttribute2("test_attribute2");
                setEventAttribute3("test_attribute3");
                setEventAttribute4("test_attribute4");
                setEventAttribute5("test_attribute5");
                setExistingUser(false);
                setFacebookUserId("temp_facebook_user_id");
                setGender(0);
                setGoogleAdvertisingId("12345678-1234-1234-1234-123456789012", true);
                setGoogleUserId("temp_google_user_id");
                setJailbroken(false);
                setLocation(111,222,333);
                setPackageName("com.tempcompany.tempapp");
                setRedirectUrl("http://www.example.com");
                setShouldAutoDetectJailbroken(true);
                setShouldAutoGenerateAppleVendorIdentifier(true);
                setSiteId("mat_site_id");
                setTRUSTeId("1234567890");
                setTwitterUserId("temp_twitter_user_id");
                setUseCookieTracking(false);
                setUserEmail("tempuser@tempcompany.com");
                setUserId("temp_user_id");
                setUserName("temp_user_name");
            #endif
        }

        if (GUI.Button (new Rect (10, 910, Screen.width - 20, 90), "Test Getter Methods"))
        {
            print ("Test Getter Methods clicked");
            #if UNITY_ANDROID || UNITY_IPHONE
                print ("isPayingUser = " + getIsPayingUser());
                print ("matId     = " + getMatId());
                print ("openLogId = " + getOpenLogId());
            #endif
        }
    }

    public static string getSampleiTunesIAPReceipt ()
    {
        return "{\"signature\" = \"AiuR/oePW4lQq2qAFerYcL/lU7HB7rqPKoddrjnqLUqvLywbSukO3OUwWwiRGE8iFiNvaqVF2CaG8siBkfkP5KYaeiTHT2RNLCIKyCfhOIr4q0wYCKwxNp2vdo3S8b+4boeSAXzgzBHCR1S1hTN5LuoeRzDeIWHoYT66CBweqDetAAADVzCCA1MwggI7oAMCAQICCGUUkU3ZWAS1MA0GCSqGSIb3DQEBBQUAMH8xCzAJBgNVBAYTAlVTMRMwEQYDVQQKDApBcHBsZSBJbmMuMSYwJAYDVQQLDB1BcHBsZSBDZXJ0aWZpY2F0aW9uIEF1dGhvcml0eTEzMDEGA1UEAwwqQXBwbGUgaVR1bmVzIFN0b3JlIENlcnRpZmljYXRpb24gQXV0aG9yaXR5MB4XDTA5MDYxNTIyMDU1NloXDTE0MDYxNDIyMDU1NlowZDEjMCEGA1UEAwwaUHVyY2hhc2VSZWNlaXB0Q2VydGlmaWNhdGUxGzAZBgNVBAsMEkFwcGxlIGlUdW5lcyBTdG9yZTETMBEGA1UECgwKQXBwbGUgSW5jLjELMAkGA1UEBhMCVVMwgZ8wDQYJKoZIhvcNAQEBBQADgY0AMIGJAoGBAMrRjF2ct4IrSdiTChaI0g8pwv/cmHs8p/RwV/rt/91XKVhNl4XIBimKjQQNfgHsDs6yju++DrKJE7uKsphMddKYfFE5rGXsAdBEjBwRIxexTevx3HLEFGAt1moKx509dhxtiIdDgJv2YaVs49B0uJvNdy6SMqNNLHsDLzDS9oZHAgMBAAGjcjBwMAwGA1UdEwEB/wQCMAAwHwYDVR0jBBgwFoAUNh3o4p2C0gEYtTJrDtdDC5FYQzowDgYDVR0PAQH/BAQDAgeAMB0GA1UdDgQWBBSpg4PyGUjFPhJXCBTMzaN+mV8k9TAQBgoqhkiG92NkBgUBBAIFADANBgkqhkiG9w0BAQUFAAOCAQEAEaSbPjtmN4C/IB3QEpK32RxacCDXdVXAeVReS5FaZxc+t88pQP93BiAxvdW/3eTSMGY5FbeAYL3etqP5gm8wrFojX0ikyVRStQ+/AQ0KEjtqB07kLs9QUe8czR8UGfdM1EumV/UgvDd4NwNYxLQMg4WTQfgkQQVy8GXZwVHgbE/UC6Y7053pGXBk51NPM3woxhd3gSRLvXj+loHsStcTEqe9pBDpmG5+sk4tw+GK3GMeEN5/+e1QT9np/Kl1nj+aBw7C0xsy0bFnaAd1cSS6xdory/CUvM6gtKsmnOOdqTesbp0bs8sn6Wqs0C9dgcxRHuOMZ2tm8npLUm7argOSzQ==\";\"purchase-info\" = \"ewoJIm9yaWdpbmFsLXB1cmNoYXNlLWRhdGUtcHN0IiA9ICIyMDEzLTA2LTE5IDEzOjMyOjE5IEFtZXJpY2EvTG9zX0FuZ2VsZXMiOwoJInVuaXF1ZS1pZGVudGlmaWVyIiA9ICJjODU0OGI1YWExZjM5NDA2NmY1ZWEwM2Q3ZGU0YTBiYzdjMTEyZDk5IjsKCSJvcmlnaW5hbC10cmFuc2FjdGlvbi1pZCIgPSAiMTAwMDAwMDA3Nzk2MDgzNSI7CgkiYnZycyIgPSAiMS4xIjsKCSJ0cmFuc2FjdGlvbi1pZCIgPSAiMTAwMDAwMDA4MzE1MjA1NCI7CgkicXVhbnRpdHkiID0gIjEiOwoJIm9yaWdpbmFsLXB1cmNoYXNlLWRhdGUtbXMiID0gIjEzNzE2NzM5MzkwMDAiOwoJInVuaXF1ZS12ZW5kb3ItaWRlbnRpZmllciIgPSAiQTM3MjFCQzctNDA3Qi00QzcyLTg4RDAtMTdGNjIwRUMzNzEzIjsKCSJwcm9kdWN0LWlkIiA9ICJjb20uaGFzb2ZmZXJzLmluYXBwcHVyY2hhc2V0cmFja2VyMS5iYWxsIjsKCSJpdGVtLWlkIiA9ICI2NTMyMzA4MjkiOwoJImJpZCIgPSAiY29tLkhhc09mZmVycy5JbkFwcFB1cmNoYXNlVHJhY2tlcjEiOwoJInB1cmNoYXNlLWRhdGUtbXMiID0gIjEzNzU4MTM2NDcxMDIiOwoJInB1cmNoYXNlLWRhdGUiID0gIjIwMTMtMDgtMDYgMTg6Mjc6MjcgRXRjL0dNVCI7CgkicHVyY2hhc2UtZGF0ZS1wc3QiID0gIjIwMTMtMDgtMDYgMTE6Mjc6MjcgQW1lcmljYS9Mb3NfQW5nZWxlcyI7Cgkib3JpZ2luYWwtcHVyY2hhc2UtZGF0ZSIgPSAiMjAxMy0wNi0xOSAyMDozMjoxOSBFdGMvR01UIjsKfQ==\";\"environment\" = \"Sandbox\";\"pod\" = \"100\";\"signing-status\" = \"0\";}";
    }
}
