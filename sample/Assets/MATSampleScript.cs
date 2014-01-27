using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class MATSampleScript : MonoBehaviour {

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

        [DllImport ("mobileapptracker")]
        private static extern void setAllowDuplicateRequests(bool allowDups);
        [DllImport ("mobileapptracker")]
        private static extern void setCurrencyCode(string currencyCode);
        [DllImport ("mobileapptracker")]
        private static extern void setDebugMode(bool enable);
        [DllImport ("mobileapptracker")]
        private static extern void setPackageName(string packageName);
        [DllImport ("mobileapptracker")]
        private static extern void setSiteId(string siteId);
        [DllImport ("mobileapptracker")]
        private static extern void setTRUSTeId(string trusteId);
        [DllImport ("mobileapptracker")]
        private static extern void setUserId(string userId);
        [DllImport ("mobileapptracker")]
        private static extern void setFacebookUserId(string facebookUserId);
        [DllImport ("mobileapptracker")]
        private static extern void setGoogleUserId(string googleUserId);
        [DllImport ("mobileapptracker")]
        private static extern void setTwitterUserId(string twitterUserId);

        [DllImport ("mobileapptracker")]
        private static extern void startAppToAppTracking(string targetAppId, string advertiserId, string offerId, string publisherId, bool shouldRedirect);

        [DllImport ("mobileapptracker")]
        private static extern void trackAction(string action, bool isId, double revenue, string currencyCode);

        [DllImport ("mobileapptracker")]
        private static extern void trackActionWithEventItem(string action, bool isId, MATItem[] items, int eventItemCount, string refId, double revenue, string currency, int transactionState, string receiptData, string receiptSignature);

        [DllImport ("mobileapptracker")]
        private static extern int trackInstall();
        [DllImport ("mobileapptracker")]
        private static extern int trackUpdate();
        [DllImport ("mobileapptracker")]
        private static extern void trackInstallWithReferenceId(string refId);
        [DllImport ("mobileapptracker")]
        private static extern void trackUpdateWithReferenceId(string refId);

        // iOS-only functions that are imported for cross-platform coding convenience
        [DllImport ("mobileapptracker")]
        private static extern void setAppAdTracking(bool enable);
        [DllImport ("mobileapptracker")]
        private static extern void setDelegate(bool enable);
        [DllImport ("mobileapptracker")]
        private static extern void setMACAddress(string mac);
        [DllImport ("mobileapptracker")]
        private static extern void setODIN1(string odin1);
        [DllImport ("mobileapptracker")]
        private static extern void setOpenUDID(string openUDID);
        [DllImport ("mobileapptracker")]
        private static extern void setRedirectUrl(string redirectUrl);
        [DllImport ("mobileapptracker")]
        private static extern void setUIID(string uiid);
        [DllImport ("mobileapptracker")]
        private static extern void setAge(int age);
        [DllImport ("mobileapptracker")]
        private static extern void setGender(int gender);
        [DllImport ("mobileapptracker")]
        private static extern void setLocation(double latitude, double longitude, double altitude);
        [DllImport ("mobileapptracker")]
        private static extern void setShouldAutoGenerateVendorIdentifier(bool shouldAutoGenerate);
        [DllImport ("mobileapptracker")]
        private static extern void setShouldAutoGenerateAdvertiserIdentifier(bool shouldAutoGenerate);
        [DllImport ("mobileapptracker")]
        private static extern void setUseCookieTracking(bool useCookieTracking);
        [DllImport ("mobileapptracker")]
        private static extern void setAdvertiserIdentifier(string advertiserIdentifier);
        [DllImport ("mobileapptracker")]
        private static extern void setVendorIdentifier(string vendorIdentifier);
        [DllImport ("mobileapptracker")]
        private static extern string getSDKDataParameters();
    #endif

    #if UNITY_IPHONE
        // Main initializer method for MAT
        [DllImport ("__Internal")]
        private static extern void initNativeCode(string advertiserId, string conversionKey);

        // Methods to help debugging and testing
        [DllImport ("__Internal")]
        private static extern void setDebugMode(bool enable);
        [DllImport ("__Internal")]
        private static extern void setAllowDuplicateRequests(bool allowDuplicateRequests);

        // Method to get a json string representation of the tracking request params to be used by the MAT SDK
        [DllImport ("__Internal")]
        private static extern string getSDKDataParameters();

        // Method to enable MAT delegate success/failure callbacks
        [DllImport ("__Internal")]
        private static extern void setDelegate(bool enable);

        // Optional Setter Methods
        [DllImport ("__Internal")]
        private static extern void setAppAdTracking(bool enable);
        [DllImport ("__Internal")]
        private static extern void setMACAddress(string mac);
        [DllImport ("__Internal")]
        private static extern void setODIN1(string odin1);
        [DllImport ("__Internal")]
        private static extern void setOpenUDID(string openUDID);
        [DllImport ("__Internal")]
        private static extern void setCurrencyCode(string currencyCode);
        [DllImport ("__Internal")]
        private static extern void setPackageName(string packageName);
        [DllImport ("__Internal")]
        private static extern void setSiteId(string siteId);
        [DllImport ("__Internal")]
        private static extern void setTRUSTeId(string trusteTPID);
        [DllImport ("__Internal")]
        private static extern void setUIID(string uiid);
        [DllImport ("__Internal")]
        private static extern void setUserId(string userId);
        [DllImport ("__Internal")]
        private static extern void setFacebookUserId(string facebookUserId);
        [DllImport ("__Internal")]
        private static extern void setTwitterUserId(string twitterUserId);
        [DllImport ("__Internal")]
        private static extern void setGoogleUserId(string googleUserId);
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

        // Method to enable cookie based tracking
        [DllImport ("__Internal")]
        private static extern void setUseCookieTracking(bool useCookieTracking);

        // Methods for setting Apple related id
        [DllImport ("__Internal")]
        private static extern void setAppleAdvertisingIdentifier(string appleAdvertisingIdentifier);
        [DllImport ("__Internal")]
        private static extern void setAppleVendorIdentifier(string appleVendorIdentifier);
        [DllImport ("__Internal")]
        private static extern void setShouldAutoGenerateAppleVendorIdentifier(bool shouldAutoGenerate);
        [DllImport ("__Internal")]
        private static extern void setShouldAutoGenerateAppleAdvertisingIdentifier(bool shouldAutoGenerate);

        // Methods for app-to-app tracking
        [DllImport ("__Internal")]
        private static extern void startAppToAppTracking(string targetAppId, string advertiserId, string offerId, string publisherId, bool shouldRedirect);
        [DllImport ("__Internal")]
        private static extern void setRedirectUrl(string redirectUrl);

        // Methods to track custom in-app events
        [DllImport ("__Internal")]
        private static extern void trackAction(string action, bool isId, double revenue, string  currencyCode);
        [DllImport ("__Internal")]
        private static extern void trackActionWithEventItem(string action, bool isId, MATItem[] items, int eventItemCount, string refId, double revenue, string currency, int transactionState, string receipt, string receiptSignature);

        // Methods to track install, update events
        [DllImport ("__Internal")]
        private static extern void trackInstall();
        [DllImport ("__Internal")]
        private static extern void trackUpdate();
        [DllImport ("__Internal")]
        private static extern void trackInstallWithReferenceId(string refId);
        [DllImport ("__Internal")]
        private static extern void trackUpdateWithReferenceId(string refId);
    #endif

    void Awake ()
    {
        #if UNITY_ANDROID || UNITY_IPHONE
            string MAT_ADVERTISER_ID = null;
            string MAT_CONVERSION_KEY = null;

            #if UNITY_ANDROID
                // Android
                MAT_ADVERTISER_ID = "877";
                MAT_CONVERSION_KEY = "5afe3bc434184096023e3d8b2ae27e1c";
            #elif UNITY_IPHONE
                // iOS
                MAT_ADVERTISER_ID = "877";
                MAT_CONVERSION_KEY = "8c14d6bbe466b65211e781d62e301eec";
            #endif

            print("Awake called: " + MAT_ADVERTISER_ID + ", " + MAT_CONVERSION_KEY);

            initNativeCode(MAT_ADVERTISER_ID, MAT_CONVERSION_KEY);

            setDelegate(true);

            // NOTE: !!! ONLY FOR DEBUG !!!
            // !!! Make sure you set to false 
            //     OR 
            //     remove the setDebugMode and setAllowDuplicateRequests calls for Production builds !!!
            setDebugMode(true);
            setAllowDuplicateRequests(true);

            setAppAdTracking(true);

            setAge(28);
            setGender(1); // 0 = male, 1 = female
		
			setTwitterUserId("mytwitter");
			setGoogleUserId("mygoogle");
			setFacebookUserId("myfacebook");

            print("MATSampleScript Awake: sdk data parameters: " + getSDKDataParameters());

            print("MATSampleScript Awake: sdk data parameters: finished");
        #endif

        return;
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 350, 100), "Track Install"))
        {
            print("trackInstall clicked");
            #if UNITY_ANDROID || UNITY_IPHONE
                trackInstall();
            #endif
        }

        if (GUI.Button(new Rect(10, 120, 350, 100), "Track Action"))
        {
            print("trackAction clicked");
            #if UNITY_ANDROID || UNITY_IPHONE
                trackAction("evt11", false, 0.35, "CAD");
            #endif
        }

        if (GUI.Button(new Rect(10, 230, 350, 100), "Track Action With Event Items"))
        {
            print("trackActionWithEvent clicked");

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

                trackActionWithEventItem("event6With2Items", false, arr, arr.Length, null, 10, "USD", transactionState, receiptData, receiptSignature);
            #endif
        }
    }

    public void trackerDidSucceed(string data)
    {
        print("trackerDidSucceed: " + DecodeFrom64(data));
    }

    private void trackerDidFail(string error)
    {
        print("trackerDidFail: " + error);
    }

    /// <summary>
    /// The method to decode base64 strings.
    /// </summary>
    /// <param name="encodedData">A base64 encoded string.</param>
    /// <returns>A decoded string.</returns>
    public static string DecodeFrom64(string encodedString)
    {
        print("MATSampleScript.DecodeFrom64(string)");
        return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(encodedString));
    }

    public static string getSampleiTunesIAPReceipt()
    {
        return "{\"signature\" = \"AiuR/oePW4lQq2qAFerYcL/lU7HB7rqPKoddrjnqLUqvLywbSukO3OUwWwiRGE8iFiNvaqVF2CaG8siBkfkP5KYaeiTHT2RNLCIKyCfhOIr4q0wYCKwxNp2vdo3S8b+4boeSAXzgzBHCR1S1hTN5LuoeRzDeIWHoYT66CBweqDetAAADVzCCA1MwggI7oAMCAQICCGUUkU3ZWAS1MA0GCSqGSIb3DQEBBQUAMH8xCzAJBgNVBAYTAlVTMRMwEQYDVQQKDApBcHBsZSBJbmMuMSYwJAYDVQQLDB1BcHBsZSBDZXJ0aWZpY2F0aW9uIEF1dGhvcml0eTEzMDEGA1UEAwwqQXBwbGUgaVR1bmVzIFN0b3JlIENlcnRpZmljYXRpb24gQXV0aG9yaXR5MB4XDTA5MDYxNTIyMDU1NloXDTE0MDYxNDIyMDU1NlowZDEjMCEGA1UEAwwaUHVyY2hhc2VSZWNlaXB0Q2VydGlmaWNhdGUxGzAZBgNVBAsMEkFwcGxlIGlUdW5lcyBTdG9yZTETMBEGA1UECgwKQXBwbGUgSW5jLjELMAkGA1UEBhMCVVMwgZ8wDQYJKoZIhvcNAQEBBQADgY0AMIGJAoGBAMrRjF2ct4IrSdiTChaI0g8pwv/cmHs8p/RwV/rt/91XKVhNl4XIBimKjQQNfgHsDs6yju++DrKJE7uKsphMddKYfFE5rGXsAdBEjBwRIxexTevx3HLEFGAt1moKx509dhxtiIdDgJv2YaVs49B0uJvNdy6SMqNNLHsDLzDS9oZHAgMBAAGjcjBwMAwGA1UdEwEB/wQCMAAwHwYDVR0jBBgwFoAUNh3o4p2C0gEYtTJrDtdDC5FYQzowDgYDVR0PAQH/BAQDAgeAMB0GA1UdDgQWBBSpg4PyGUjFPhJXCBTMzaN+mV8k9TAQBgoqhkiG92NkBgUBBAIFADANBgkqhkiG9w0BAQUFAAOCAQEAEaSbPjtmN4C/IB3QEpK32RxacCDXdVXAeVReS5FaZxc+t88pQP93BiAxvdW/3eTSMGY5FbeAYL3etqP5gm8wrFojX0ikyVRStQ+/AQ0KEjtqB07kLs9QUe8czR8UGfdM1EumV/UgvDd4NwNYxLQMg4WTQfgkQQVy8GXZwVHgbE/UC6Y7053pGXBk51NPM3woxhd3gSRLvXj+loHsStcTEqe9pBDpmG5+sk4tw+GK3GMeEN5/+e1QT9np/Kl1nj+aBw7C0xsy0bFnaAd1cSS6xdory/CUvM6gtKsmnOOdqTesbp0bs8sn6Wqs0C9dgcxRHuOMZ2tm8npLUm7argOSzQ==\";\"purchase-info\" = \"ewoJIm9yaWdpbmFsLXB1cmNoYXNlLWRhdGUtcHN0IiA9ICIyMDEzLTA2LTE5IDEzOjMyOjE5IEFtZXJpY2EvTG9zX0FuZ2VsZXMiOwoJInVuaXF1ZS1pZGVudGlmaWVyIiA9ICJjODU0OGI1YWExZjM5NDA2NmY1ZWEwM2Q3ZGU0YTBiYzdjMTEyZDk5IjsKCSJvcmlnaW5hbC10cmFuc2FjdGlvbi1pZCIgPSAiMTAwMDAwMDA3Nzk2MDgzNSI7CgkiYnZycyIgPSAiMS4xIjsKCSJ0cmFuc2FjdGlvbi1pZCIgPSAiMTAwMDAwMDA4MzE1MjA1NCI7CgkicXVhbnRpdHkiID0gIjEiOwoJIm9yaWdpbmFsLXB1cmNoYXNlLWRhdGUtbXMiID0gIjEzNzE2NzM5MzkwMDAiOwoJInVuaXF1ZS12ZW5kb3ItaWRlbnRpZmllciIgPSAiQTM3MjFCQzctNDA3Qi00QzcyLTg4RDAtMTdGNjIwRUMzNzEzIjsKCSJwcm9kdWN0LWlkIiA9ICJjb20uaGFzb2ZmZXJzLmluYXBwcHVyY2hhc2V0cmFja2VyMS5iYWxsIjsKCSJpdGVtLWlkIiA9ICI2NTMyMzA4MjkiOwoJImJpZCIgPSAiY29tLkhhc09mZmVycy5JbkFwcFB1cmNoYXNlVHJhY2tlcjEiOwoJInB1cmNoYXNlLWRhdGUtbXMiID0gIjEzNzU4MTM2NDcxMDIiOwoJInB1cmNoYXNlLWRhdGUiID0gIjIwMTMtMDgtMDYgMTg6Mjc6MjcgRXRjL0dNVCI7CgkicHVyY2hhc2UtZGF0ZS1wc3QiID0gIjIwMTMtMDgtMDYgMTE6Mjc6MjcgQW1lcmljYS9Mb3NfQW5nZWxlcyI7Cgkib3JpZ2luYWwtcHVyY2hhc2UtZGF0ZSIgPSAiMjAxMy0wNi0xOSAyMDozMjoxOSBFdGMvR01UIjsKfQ==\";\"environment\" = \"Sandbox\";\"pod\" = \"100\";\"signing-status\" = \"0\";}";
    }
}
