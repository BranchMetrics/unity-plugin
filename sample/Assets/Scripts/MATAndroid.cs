using UnityEngine;
using System;
using System.Collections;

namespace com.mobileapptracking
{
    /// <para>
    /// Struct used for storing MAT information.
    /// </para>
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
    public class MATAndroid
    {
        private static MATAndroid instance;

        //MobileAppTracker.java for Android is already encapsulated.
        private AndroidJavaClass ajcMAT;
        public AndroidJavaObject ajcInstance;
        private AndroidJavaClass ajcUnityPlayer;
        private AndroidJavaObject ajcCurrentActivity;

        private MATAndroid() {}

        public static MATAndroid Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MATAndroid();
                }
                return instance;
            }

        }

        /// <summary>
        /// Initializes the reference to the AndroidJavaClass MobileAppTracker object.
        /// Does nothing if already initialized.
        /// </summary>
        public void Init(string advertiserId, string conversionKey)
        {
            if(ajcMAT == null)
            {
                ajcMAT = new AndroidJavaClass("com.mobileapptracker.MobileAppTracker");
                ajcUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                ajcCurrentActivity = ajcUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

                ajcMAT.CallStatic("init", ajcCurrentActivity, advertiserId, conversionKey);
                ajcInstance = ajcMAT.CallStatic<AndroidJavaObject>("getInstance");
                ajcInstance.Call("setPluginName", "unity");
                ajcInstance.Call("setReferralSources", ajcCurrentActivity);

                // Start GAID fetch
                AndroidJavaObject objGAIDFetcher = new AndroidJavaObject("com.matunityutils.GAIDFetcher");
                objGAIDFetcher.Call("useUnityFetcherInterface");
                objGAIDFetcher.Call("fetchGAID", ajcCurrentActivity);
            }
        }

        public void MeasureSession()
        {
            ajcInstance.Call("measureSession");
        }

        public void MeasureAction(string eventName)
        {
            ajcInstance.Call("measureAction", eventName);
        }

        public void MeasureActionWithRefId(string eventName, string refId)
        {
            ajcInstance.Call("measureAction", eventName, 0.0, "", refId);
        }

        public void MeasureActionWithRevenue(string eventName, double revenue, string currency, string refId)
        {
            ajcInstance.Call("measureAction", eventName, revenue, currency, refId);
        }

        public void MeasureActionWithEventItems(string eventName, MATItem[] items, int eventItemCount, double revenue, string currency, string refId, int transactionState, string receipt, string receiptSignature)
        {
            // Convert MATItem to MATEventItem
            AndroidJavaObject objArrayList = new AndroidJavaObject("java.util.ArrayList");
            foreach (MATItem item in items)
            {
                AndroidJavaObject objEventItem = new AndroidJavaObject("com.mobileapptracker.MATEventItem",
                    item.name,
                    item.quantity,
                    item.unitPrice,
                    item.revenue,
                    item.attribute1,
                    item.attribute2,
                    item.attribute3,
                    item.attribute4,
                    item.attribute5);

                objArrayList.Call<bool>("add", objEventItem);
            }
            ajcInstance.Call("measureAction", eventName, objArrayList, revenue, currency, refId, receipt, receiptSignature);
        }

        public bool GetIsPayingUser()
        {
            return ajcInstance.Call<bool>("getIsPayingUser");
        }

        public string GetMatId()
        {
            return ajcInstance.Call<string>("getMatId");
        }

        public string GetOpenLogId()
        {
            return ajcInstance.Call<string>("getOpenLogId");
        }

        public void SetAge(int age)
        {
            ajcInstance.Call("setAge", age);
        }

        public void SetAllowDuplicates(bool allow)
        {
            ajcInstance.Call("setAllowDuplicates", allow);
        }

        public void SetAndroidId(string androidId)
        {
            ajcInstance.Call("setAndroidId", androidId);
        }

        public void SetAndroidIdMd5(string androidIdMd5)
        {
            ajcInstance.Call("setAndroidIdMd5", androidIdMd5);
        }

        public void SetAndroidIdSha1(string androidIdSha1)
        {
            ajcInstance.Call("setAndroidIdSha1", androidIdSha1);
        }

        public void SetAndroidIdSha256(string androidIdSha256)
        {
            ajcInstance.Call("setAndroidIdSha256", androidIdSha256);
        }

        public void SetAppAdTracking(bool adTrackingEnabled)
        {
            ajcInstance.Call("setAppAdTracking", adTrackingEnabled);
        }

        public void SetCurrencyCode(string currencyCode)
        {
            ajcInstance.Call("setCurrencyCode", currencyCode);
        }

        public void SetDebugMode(bool debugMode)
        {
            ajcInstance.Call("setDebugMode", debugMode);
        }

        public void SetDeviceId(string deviceId)
        {
            ajcInstance.Call("setDeviceId", deviceId);
        }

        public void SetDelegate(bool enable)
        {
            if (enable)
            {
                // Get the predefined Unity MAT response and set it
                AndroidJavaObject objResponse = new AndroidJavaObject("com.matunityutils.MATUnityResponse");
                ajcInstance.Call("setMATResponse", objResponse);
            }
        }

        public void SetEmailCollection(bool collectEmail)
        {
            ajcInstance.Call("setEmailCollection", collectEmail);
        }

        public void SetEventAttribute1(string eventAttribute)
        {
            ajcInstance.Call("setEventAttribute1", eventAttribute);
        }

        public void SetEventAttribute2(string eventAttribute)
        {
            ajcInstance.Call("setEventAttribute2", eventAttribute);
        }

        public void SetEventAttribute3(string eventAttribute)
        {
            ajcInstance.Call("setEventAttribute3", eventAttribute);
        }

        public void SetEventAttribute4(string eventAttribute)
        {
            ajcInstance.Call("setEventAttribute4", eventAttribute);
        }

        public void SetEventAttribute5(string eventAttribute)
        {
            ajcInstance.Call("setEventAttribute5", eventAttribute);
        }

        public void SetEventContentId(string eventContentId)
        {
            ajcInstance.Call("setEventContentId", eventContentId);
        }

        public void SetEventContentType(string eventContentType)
        {
            ajcInstance.Call("setEventContentType", eventContentType);
        }

        public void SetEventDate1(string eventDate)
        {
            AndroidJavaObject objDouble = new AndroidJavaObject("java.lang.Double", eventDate);
            AndroidJavaObject longDate = objDouble.Call<AndroidJavaObject>("longValue");
            AndroidJavaObject objDate = new AndroidJavaObject("java.util.Date", longDate);
            ajcInstance.Call("setEventDate1", objDate);
        }

        public void SetEventDate2(string eventDate)
        {
            AndroidJavaObject objDouble = new AndroidJavaObject("java.lang.Double", eventDate);
            AndroidJavaObject longDate = objDouble.Call<AndroidJavaObject>("longValue");
            AndroidJavaObject objDate = new AndroidJavaObject("java.util.Date", longDate);
            ajcInstance.Call("setEventDate2", objDate);
        }

        public void SetEventLevel(int eventLevel)
        {
            ajcInstance.Call("setEventLevel", eventLevel);
        }

        public void SetEventQuantity(int eventQuantity)
        {
            ajcInstance.Call("setEventQuantity", eventQuantity);
        }

        public void SetEventRating(float eventRating)
        {
            ajcInstance.Call("setEventRating", eventRating);
        }

        public void SetEventSearchString(string eventSearchString)
        {
            ajcInstance.Call("setEventSearchString", eventSearchString);
        }

        public void SetExistingUser(bool isExistingUser)
        {
            ajcInstance.Call("setExistingUser", isExistingUser);
        }

        /*public void SetFacebookEventLogging(bool fbEventLogging)
        {
            ajcInstance.Call("setFacebookEventLogging", fbEventLogging);
        }*/

        public void SetFacebookUserId(string facebookUserId)
        {
            ajcInstance.Call("setFacebookUserId", facebookUserId);
        }

        public void SetGender(int gender)
        {
            ajcInstance.Call("setGender", gender);
        }

        public void SetGoogleAdvertisingId(string googleAid, bool isLATEnabled)
        {
            ajcInstance.Call("setGoogleAdvertisingId", googleAid, isLATEnabled);
        }

        public void SetGoogleUserId(string googleUserId)
        {
            ajcInstance.Call("setGoogleUserId", googleUserId);
        }

        public void SetLocation(double latitude, double longitude, double altitude)
        {
            ajcInstance.Call("setLatitude", latitude);
            ajcInstance.Call("setLongitude", longitude);
            ajcInstance.Call("setAltitude", altitude);
        }

        public void SetMacAddress(string macAddress)
        {
            ajcInstance.Call("setMacAddress", macAddress);
        }

        public void SetPackageName(string packageName)
        {
            ajcInstance.Call("setPackageName", packageName);
        }

        public void SetPayingUser(bool isPayingUser)
        {
            ajcInstance.Call("setIsPayingUser", isPayingUser);
        }

        public void SetSiteId(string siteId)
        {
            ajcInstance.Call("setSiteId", siteId);
        }

        public void SetTRUSTeId(string tpid)
        {
            ajcInstance.Call("setTRUSTeId", tpid);
        }

        public void SetTwitterUserId(string twitterUserId)
        {
            ajcInstance.Call("setTwitterUserId", twitterUserId);
        }

        public void SetUserEmail(string userEmail)
        {
            ajcInstance.Call("setUserEmail", userEmail);
        }

        public void SetUserId(string userId)
        {
            ajcInstance.Call("setUserId", userId);
        }

        public void SetUserName(string userName)
        {
            ajcInstance.Call("setUserName", userName);
        }

        /* Pre-loaded app attribution setters */
        public void SetPublisherId(string publisherId)
        {
            ajcInstance.Call("setPublisherId", publisherId);
        }

        public void SetOfferId(string offerId)
        {
            ajcInstance.Call("setOfferId", offerId);
        }

        public void SetPublisherReferenceId(string refId)
        {
            ajcInstance.Call("setPublisherReferenceId", refId);
        }

        public void SetPublisherSub1(string sub1)
        {
            ajcInstance.Call("setPublisherSub1", sub1);
        }

        public void SetPublisherSub2(string sub2)
        {
            ajcInstance.Call("setPublisherSub2", sub2);
        }

        public void SetPublisherSub3(string sub3)
        {
            ajcInstance.Call("setPublisherSub3", sub3);
        }

        public void SetPublisherSub4(string sub4)
        {
            ajcInstance.Call("setPublisherSub4", sub4);
        }

        public void SetPublisherSub5(string sub5)
        {
            ajcInstance.Call("setPublisherSub5", sub5);
        }

        public void SetPublisherSubAd(string subAd)
        {
            ajcInstance.Call("setPublisherSubAd", subAd);
        }

        public void SetPublisherSubAdgroup(string subAdgroup)
        {
            ajcInstance.Call("setPublisherSubAdgroup", subAdgroup);
        }

        public void SetPublisherSubCampaign(string subCampaign)
        {
            ajcInstance.Call("setPublisherSubCampaign", subCampaign);
        }

        public void SetPublisherSubKeyword(string subKeyword)
        {
            ajcInstance.Call("setPublisherSubKeyword", subKeyword);
        }

        public void SetPublisherSubPublisher(string subPublisher)
        {
            ajcInstance.Call("setPublisherSubPublisher", subPublisher);
        }

        public void SetPublisherSubSite(string subSite)
        {
            ajcInstance.Call("setPublisherSubSite", subSite);
        }

        public void SetAdvertiserSubAd(string subAd)
        {
            ajcInstance.Call("setAdvertiserSubAd", subAd);
        }

        public void SetAdvertiserSubAdgroup(string subAdgroup)
        {
            ajcInstance.Call("setAdvertiserSubAdgroup", subAdgroup);
        }

        public void SetAdvertiserSubCampaign(string subCampaign)
        {
            ajcInstance.Call("setAdvertiserSubCampaign", subCampaign);
        }

        public void SetAdvertiserSubKeyword(string subKeyword)
        {
            ajcInstance.Call("setAdvertiserSubKeyword", subKeyword);
        }

        public void SetAdvertiserSubPublisher(string subPublisher)
        {
            ajcInstance.Call("setAdvertiserSubPublisher", subPublisher);
        }

        public void SetAdvertiserSubSite(string subSite)
        {
            ajcInstance.Call("setAdvertiserSubSite", subSite);
        }

        public string CheckForDeferredDeeplink(int timeoutInMillis)
        {
            return ajcInstance.Call<string>("checkForDeferredDeeplink", timeoutInMillis);
        }
    }
    #endif
}