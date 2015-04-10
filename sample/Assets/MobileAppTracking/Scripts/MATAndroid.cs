using UnityEngine;
using System;
using System.Collections;

namespace MATSDK
{
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

                ajcInstance = ajcMAT.CallStatic<AndroidJavaObject>("init", ajcCurrentActivity, advertiserId, conversionKey);
                ajcInstance.Call("setPluginName", "unity");
                ajcInstance.Call("setReferralSources", ajcCurrentActivity);
            }
        }

        public void MeasureSession()
        {
            ajcInstance.Call("measureSession");
        }

        public void MeasureEvent(string eventName)
        {
            ajcInstance.Call("measureEvent", eventName);
        }

        public void MeasureEvent(MATEvent matEvent)
        {
            AndroidJavaObject objEvent = GetMATEventJavaObject(matEvent);
            ajcInstance.Call("measureEvent", objEvent);
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


        public void SetDeferredDeeplink(bool enableDeferredDeeplink, int timeoutInMillis)
        {
            ajcInstance.Call("setDeferredDeeplink", enableDeferredDeeplink, timeoutInMillis);
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

        public void SetExistingUser(bool isExistingUser)
        {
            ajcInstance.Call("setExistingUser", isExistingUser);
        }

        public void SetFacebookEventLogging(bool fbEventLogging, bool limitEventAndDataUsage)
        {
            ajcInstance.Call("setFacebookEventLogging", fbEventLogging, ajcCurrentActivity, limitEventAndDataUsage);
        }

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

        // Preloaded app attribution
        public void SetPreloadedApp(MATPreloadData preloadData)
        {
            // Convert C# MATPreloadData to Java MATPreloadData
            AndroidJavaObject objPreloadData = new AndroidJavaObject("com.mobileapptracker.MATPreloadData", preloadData.publisherId);
            if (preloadData.offerId != null) {
                objPreloadData = objPreloadData.Call<AndroidJavaObject>("withOfferId", preloadData.offerId);
            }
            if (preloadData.publisherReferenceId != null) {
                objPreloadData = objPreloadData.Call<AndroidJavaObject>("withPublisherReferenceId", preloadData.publisherReferenceId);
            }
            if (preloadData.publisherSub1 != null) {
                objPreloadData = objPreloadData.Call<AndroidJavaObject>("withPublisherSub1", preloadData.publisherSub1);
            }
            if (preloadData.publisherSub2 != null) {
                objPreloadData = objPreloadData.Call<AndroidJavaObject>("withPublisherSub2", preloadData.publisherSub2);
            }
            if (preloadData.publisherSub3 != null) {
                objPreloadData = objPreloadData.Call<AndroidJavaObject>("withPublisherSub3", preloadData.publisherSub3);
            }
            if (preloadData.publisherSub4 != null) {
                objPreloadData = objPreloadData.Call<AndroidJavaObject>("withPublisherSub4", preloadData.publisherSub4);
            }
            if (preloadData.publisherSub5 != null) {
                objPreloadData = objPreloadData.Call<AndroidJavaObject>("withPublisherSub5", preloadData.publisherSub5);
            }
            if (preloadData.publisherSubAd != null) {
                objPreloadData = objPreloadData.Call<AndroidJavaObject>("withPublisherSubAd", preloadData.publisherSubAd);
            }
            if (preloadData.publisherSubAdgroup != null) {
                objPreloadData = objPreloadData.Call<AndroidJavaObject>("withPublisherSubAdgroup", preloadData.publisherSubAdgroup);
            }
            if (preloadData.publisherSubCampaign != null) {
                objPreloadData = objPreloadData.Call<AndroidJavaObject>("withPublisherSubCampaign", preloadData.publisherSubCampaign);
            }
            if (preloadData.publisherSubKeyword != null) {
                objPreloadData = objPreloadData.Call<AndroidJavaObject>("withPublisherSubKeyword", preloadData.publisherSubKeyword);
            }
            if (preloadData.publisherSubPublisher != null) {
                objPreloadData = objPreloadData.Call<AndroidJavaObject>("withPublisherSubPublisher", preloadData.publisherSubPublisher);
            }
            if (preloadData.publisherSubSite != null) {
                objPreloadData = objPreloadData.Call<AndroidJavaObject>("withPublisherSubSite", preloadData.publisherSubSite);
            }
            if (preloadData.advertiserSubAd != null) {
                objPreloadData = objPreloadData.Call<AndroidJavaObject>("withAdvertiserSubAd", preloadData.advertiserSubAd);
            }
            if (preloadData.advertiserSubAdgroup != null) {
                objPreloadData = objPreloadData.Call<AndroidJavaObject>("withAdvertiserSubAdgroup", preloadData.advertiserSubAdgroup);
            }
            if (preloadData.advertiserSubCampaign != null) {
                objPreloadData = objPreloadData.Call<AndroidJavaObject>("withAdvertiserSubCampaign", preloadData.advertiserSubCampaign);
            }
            if (preloadData.advertiserSubKeyword != null) {
                objPreloadData = objPreloadData.Call<AndroidJavaObject>("withAdvertiserSubKeyword", preloadData.advertiserSubKeyword);
            }
            if (preloadData.advertiserSubPublisher != null) {
                objPreloadData = objPreloadData.Call<AndroidJavaObject>("withAdvertiserSubPublisher", preloadData.advertiserSubPublisher);
            }
            if (preloadData.advertiserSubSite != null) {
                objPreloadData = objPreloadData.Call<AndroidJavaObject>("withAdvertiserSubSite", preloadData.advertiserSubSite);
            }

            ajcInstance.Call("setPreloadedApp", objPreloadData);
        }

        private AndroidJavaObject GetMATEventJavaObject(MATEvent matEvent)
        {
            // Convert C# MATEvent to new Java matEvent object
            AndroidJavaObject objMatEvent;
            if (matEvent.name == null) {
                objMatEvent = new AndroidJavaObject("com.mobileapptracker.MATEvent", matEvent.id);
            } else {
                objMatEvent = new AndroidJavaObject("com.mobileapptracker.MATEvent", matEvent.name);
            }
            // Set the optional params if they exist
            if (matEvent.revenue != null) {
                objMatEvent = objMatEvent.Call<AndroidJavaObject>("withRevenue", matEvent.revenue);
            }
            if (matEvent.currencyCode != null) {
                objMatEvent = objMatEvent.Call<AndroidJavaObject>("withCurrencyCode", matEvent.currencyCode);
            }
            if (matEvent.advertiserRefId != null) {
                objMatEvent = objMatEvent.Call<AndroidJavaObject>("withAdvertiserRefId", matEvent.advertiserRefId);
            }
            if (matEvent.eventItems != null) {
                // Convert MATItem[] to Arraylist<MATEventItem>
                AndroidJavaObject objArrayList = new AndroidJavaObject("java.util.ArrayList");
                foreach (MATItem item in matEvent.eventItems)
                {
                    // Convert MATItem to matEventItem
                    AndroidJavaObject objEventItem = new AndroidJavaObject("com.mobileapptracker.MATEventItem", item.name);
                    if (item.quantity != null) {
                        objEventItem = objEventItem.Call<AndroidJavaObject>("withQuantity", item.quantity);
                    }
                    if (item.unitPrice != null) {
                        objEventItem = objEventItem.Call<AndroidJavaObject>("withUnitPrice", item.unitPrice);
                    }
                    if (item.revenue != null) {
                        objEventItem = objEventItem.Call<AndroidJavaObject>("withRevenue", item.revenue);
                    }
                    if (item.attribute1 != null) {
                        objEventItem = objEventItem.Call<AndroidJavaObject>("withAttribute1", item.attribute1);
                    }
                    if (item.attribute2 != null) {
                        objEventItem = objEventItem.Call<AndroidJavaObject>("withAttribute2", item.attribute2);
                    }
                    if (item.attribute3 != null) {
                        objEventItem = objEventItem.Call<AndroidJavaObject>("withAttribute3", item.attribute3);
                    }
                    if (item.attribute4 != null) {
                        objEventItem = objEventItem.Call<AndroidJavaObject>("withAttribute4", item.attribute4);
                    }
                    if (item.attribute5 != null) {
                        objEventItem = objEventItem.Call<AndroidJavaObject>("withAttribute5", item.attribute5);
                    }
                    // Add to list of matEventItems
                    objArrayList.Call<bool>("add", objEventItem);
                }
                objMatEvent = objMatEvent.Call<AndroidJavaObject>("withEventItems", objArrayList);
            }
            if (matEvent.receipt != null && matEvent.receiptSignature != null) {
                objMatEvent = objMatEvent.Call<AndroidJavaObject>("withReceipt", matEvent.receipt, matEvent.receiptSignature);
            }
            if (matEvent.contentType != null) {
                objMatEvent = objMatEvent.Call<AndroidJavaObject>("withContentType", matEvent.contentType);
            }
            if (matEvent.contentId != null) {
                objMatEvent = objMatEvent.Call<AndroidJavaObject>("withContentId", matEvent.contentId);
            }
            if (matEvent.level != null) {
                objMatEvent = objMatEvent.Call<AndroidJavaObject>("withLevel", matEvent.level);
            }
            if (matEvent.quantity != null) {
                objMatEvent = objMatEvent.Call<AndroidJavaObject>("withQuantity", matEvent.quantity);
            }
            if (matEvent.searchString != null) {
                objMatEvent = objMatEvent.Call<AndroidJavaObject>("withSearchString", matEvent.searchString);
            }
            if (matEvent.date1 != null) {
                double milliseconds = new TimeSpan(((DateTime)matEvent.date1).Ticks).TotalMilliseconds;
                //datetime starts in 1970
                DateTime datetime = new DateTime(1970, 1, 1);
                double millisecondsFrom1970 = milliseconds - (new TimeSpan(datetime.Ticks)).TotalMilliseconds;
                // Convert C# DateTime to Java Date
                AndroidJavaObject objDouble = new AndroidJavaObject("java.lang.Double", millisecondsFrom1970);
                long longDate = objDouble.Call<long>("longValue");
                AndroidJavaObject objDate = new AndroidJavaObject("java.util.Date", longDate);
                objMatEvent = objMatEvent.Call<AndroidJavaObject>("withDate1", objDate);
            }
            if (matEvent.date2 != null) {
                double milliseconds = new TimeSpan(((DateTime)matEvent.date2).Ticks).TotalMilliseconds;
                //datetime starts in 1970
                DateTime datetime = new DateTime(1970, 1, 1);
                double millisecondsFrom1970 = milliseconds - (new TimeSpan(datetime.Ticks)).TotalMilliseconds;
                // Convert C# DateTime to Java Date
                AndroidJavaObject objDouble = new AndroidJavaObject("java.lang.Double", millisecondsFrom1970);
                long longDate = objDouble.Call<long>("longValue");
                AndroidJavaObject objDate = new AndroidJavaObject("java.util.Date", longDate);
                objMatEvent = objMatEvent.Call<AndroidJavaObject>("withDate2", objDate);
            }
            if (matEvent.attribute1 != null) {
                objMatEvent = objMatEvent.Call<AndroidJavaObject>("withAttribute1", matEvent.attribute1);
            }
            if (matEvent.attribute2 != null) {
                objMatEvent = objMatEvent.Call<AndroidJavaObject>("withAttribute2", matEvent.attribute2);
            }
            if (matEvent.attribute3 != null) {
                objMatEvent = objMatEvent.Call<AndroidJavaObject>("withAttribute3", matEvent.attribute3);
            }
            if (matEvent.attribute4 != null) {
                objMatEvent = objMatEvent.Call<AndroidJavaObject>("withAttribute4", matEvent.attribute4);
            }
            if (matEvent.attribute5 != null) {
                objMatEvent = objMatEvent.Call<AndroidJavaObject>("withAttribute5", matEvent.attribute5);
            }
            return objMatEvent;
        }
    }
    #endif
}