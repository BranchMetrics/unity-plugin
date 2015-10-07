using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

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

        // Ads helper plugin
        private AndroidJavaObject ajcAlliances;

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
            if (ajcMAT == null)
            {
                ajcMAT = new AndroidJavaClass("com.mobileapptracker.MobileAppTracker");
                ajcUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                ajcCurrentActivity = ajcUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

                ajcInstance = ajcMAT.CallStatic<AndroidJavaObject>("init", ajcCurrentActivity, advertiserId, conversionKey);
                ajcInstance.Call("setPluginName", "unity");

                ajcAlliances = new AndroidJavaObject("com.tune.unityutils.CrossPromoUnityPlugin");
            }
        }

        public void ShowBanner(string placement)
        {
            ajcAlliances.Call("showBanner", placement);
        }

        public void ShowBanner(string placement, MATAdMetadata metadata)
        {
            AndroidJavaObject objRequest = GetAdMetadataJavaObject(metadata);
            ajcAlliances.Call("showBanner", placement, objRequest);
        }

        public void ShowBanner(String placement, MATAdMetadata metadata, MATBannerPosition position)
        {
            AndroidJavaObject objRequest = GetAdMetadataJavaObject(metadata);
            AndroidJavaObject objPosition;
            if (position == MATBannerPosition.TOP_CENTER)
            {
                objPosition = new AndroidJavaClass("com.tune.crosspromo.TuneBannerPosition").GetStatic<AndroidJavaObject>("TOP_CENTER");
            }
            else
            {
                objPosition = new AndroidJavaClass("com.tune.crosspromo.TuneBannerPosition").GetStatic<AndroidJavaObject>("BOTTOM_CENTER");
            }
            ajcAlliances.Call("showBanner", placement, objRequest, objPosition);
        }

        public void HideBanner()
        {
            ajcAlliances.Call("hideBanner");
        }

        public void DestroyBanner()
        {
            ajcAlliances.Call("destroyBanner");
        }

        public void CacheInterstitial(string placement)
        {
            ajcAlliances.Call("cacheInterstitial", placement);
        }

        public void CacheInterstitial(string placement, MATAdMetadata metadata)
        {
            AndroidJavaObject objMetadata = GetAdMetadataJavaObject(metadata);
            ajcAlliances.Call("cacheInterstitial", placement, objMetadata);
        }

        public void ShowInterstitial(string placement)
        {
            ajcAlliances.Call("showInterstitial", placement);
        }

        public void ShowInterstitial(string placement, MATAdMetadata metadata)
        {
            AndroidJavaObject objMetadata = GetAdMetadataJavaObject(metadata);
            ajcAlliances.Call("showInterstitial", placement, objMetadata);
        }

        public void DestroyInterstitial()
        {
            ajcAlliances.Call("destroyInterstitial");
        }

        public void MeasureSession()
        {
            ajcInstance.Call("setReferralSources", ajcCurrentActivity);
            ajcInstance.Call("measureSession");
        }

        public void MeasureEvent(string eventName)
        {
            ajcInstance.Call("measureEvent", eventName);
        }

        public void MeasureEvent(int eventId)
        {
            ajcInstance.Call("measureEvent", eventId);
        }

        public void MeasureEvent(MATEvent tuneEvent)
        {
            AndroidJavaObject objEvent = GetTuneEventJavaObject(tuneEvent);
            ajcInstance.Call("measureEvent", objEvent);
        }

        public void CheckForDeferredDeeplink()
        {
            // Get the predefined Unity MAT dplink listener
            AndroidJavaObject objListener = new AndroidJavaObject("com.tune.unityutils.TUNEUnityDeeplinkListener");
            ajcInstance.Call("checkForDeferredDeeplink", objListener);
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
            ajcInstance.Call("setAppAdTrackingEnabled", adTrackingEnabled);
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
                AndroidJavaObject objResponse = new AndroidJavaObject("com.tune.unityutils.TUNEUnityListener");
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

        public void SetPhoneNumber(string phoneNumber)
        {
            ajcInstance.Call("setPhoneNumber", phoneNumber);
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
            if (preloadData.agencyId != null) {
                objPreloadData = objPreloadData.Call<AndroidJavaObject>("withAgencyId", preloadData.agencyId);
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

        private AndroidJavaObject GetTuneEventJavaObject(MATEvent tuneEvent)
        {
            // Convert C# MATEvent to new Java MATEvent object
            AndroidJavaObject objTuneEvent;
            if (tuneEvent.name == null) {
                objTuneEvent = new AndroidJavaObject("com.mobileapptracker.MATEvent", tuneEvent.id);
            } else {
                objTuneEvent = new AndroidJavaObject("com.mobileapptracker.MATEvent", tuneEvent.name);
            }
            // Set the optional params if they exist
            if (tuneEvent.revenue != null) {
                objTuneEvent = objTuneEvent.Call<AndroidJavaObject>("withRevenue", tuneEvent.revenue);
            }
            if (tuneEvent.currencyCode != null) {
                objTuneEvent = objTuneEvent.Call<AndroidJavaObject>("withCurrencyCode", tuneEvent.currencyCode);
            }
            if (tuneEvent.advertiserRefId != null) {
                objTuneEvent = objTuneEvent.Call<AndroidJavaObject>("withAdvertiserRefId", tuneEvent.advertiserRefId);
            }
            if (tuneEvent.eventItems != null) {
                // Convert MATItem[] to Arraylist<MATEventItem>
                AndroidJavaObject objArrayList = new AndroidJavaObject("java.util.ArrayList");
                foreach (MATItem item in tuneEvent.eventItems)
                {
                    // Convert MATItem to MATEventItem
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
                    // Add to list of MATEventItems
                    objArrayList.Call<bool>("add", objEventItem);
                }
                objTuneEvent = objTuneEvent.Call<AndroidJavaObject>("withEventItems", objArrayList);
            }
            if (tuneEvent.receipt != null && tuneEvent.receiptSignature != null) {
                objTuneEvent = objTuneEvent.Call<AndroidJavaObject>("withReceipt", tuneEvent.receipt, tuneEvent.receiptSignature);
            }
            if (tuneEvent.contentType != null) {
                objTuneEvent = objTuneEvent.Call<AndroidJavaObject>("withContentType", tuneEvent.contentType);
            }
            if (tuneEvent.contentId != null) {
                objTuneEvent = objTuneEvent.Call<AndroidJavaObject>("withContentId", tuneEvent.contentId);
            }
            if (tuneEvent.level != null) {
                objTuneEvent = objTuneEvent.Call<AndroidJavaObject>("withLevel", tuneEvent.level);
            }
            if (tuneEvent.quantity != null) {
                objTuneEvent = objTuneEvent.Call<AndroidJavaObject>("withQuantity", tuneEvent.quantity);
            }
            if (tuneEvent.searchString != null) {
                objTuneEvent = objTuneEvent.Call<AndroidJavaObject>("withSearchString", tuneEvent.searchString);
            }
            if (tuneEvent.date1 != null) {
                double milliseconds = new TimeSpan(((DateTime)tuneEvent.date1).Ticks).TotalMilliseconds;
                //datetime starts in 1970
                DateTime datetime = new DateTime(1970, 1, 1);
                double millisecondsFrom1970 = milliseconds - (new TimeSpan(datetime.Ticks)).TotalMilliseconds;
                // Convert C# DateTime to Java Date
                AndroidJavaObject objDouble = new AndroidJavaObject("java.lang.Double", millisecondsFrom1970);
                long longDate = objDouble.Call<long>("longValue");
                AndroidJavaObject objDate = new AndroidJavaObject("java.util.Date", longDate);
                objTuneEvent = objTuneEvent.Call<AndroidJavaObject>("withDate1", objDate);
            }
            if (tuneEvent.date2 != null) {
                double milliseconds = new TimeSpan(((DateTime)tuneEvent.date2).Ticks).TotalMilliseconds;
                //datetime starts in 1970
                DateTime datetime = new DateTime(1970, 1, 1);
                double millisecondsFrom1970 = milliseconds - (new TimeSpan(datetime.Ticks)).TotalMilliseconds;
                // Convert C# DateTime to Java Date
                AndroidJavaObject objDouble = new AndroidJavaObject("java.lang.Double", millisecondsFrom1970);
                long longDate = objDouble.Call<long>("longValue");
                AndroidJavaObject objDate = new AndroidJavaObject("java.util.Date", longDate);
                objTuneEvent = objTuneEvent.Call<AndroidJavaObject>("withDate2", objDate);
            }
            if (tuneEvent.attribute1 != null) {
                objTuneEvent = objTuneEvent.Call<AndroidJavaObject>("withAttribute1", tuneEvent.attribute1);
            }
            if (tuneEvent.attribute2 != null) {
                objTuneEvent = objTuneEvent.Call<AndroidJavaObject>("withAttribute2", tuneEvent.attribute2);
            }
            if (tuneEvent.attribute3 != null) {
                objTuneEvent = objTuneEvent.Call<AndroidJavaObject>("withAttribute3", tuneEvent.attribute3);
            }
            if (tuneEvent.attribute4 != null) {
                objTuneEvent = objTuneEvent.Call<AndroidJavaObject>("withAttribute4", tuneEvent.attribute4);
            }
            if (tuneEvent.attribute5 != null) {
                objTuneEvent = objTuneEvent.Call<AndroidJavaObject>("withAttribute5", tuneEvent.attribute5);
            }
            return objTuneEvent;
        }

        private AndroidJavaObject GetAdMetadataJavaObject(MATAdMetadata metadata)
        {
            // Convert C# MATAdMetadata to new Java TuneAdRequest object
            AndroidJavaObject objRequest = new AndroidJavaObject("com.tune.crosspromo.TuneAdMetadata");

            // Set debug mode boolean
            objRequest = objRequest.Call<AndroidJavaObject>("withDebugMode", metadata.getDebugMode());

            // Set gender as MATAdGender enum value
            AndroidJavaObject gender;
            if (metadata.getGender() == MATAdGender.MALE)
            {
                gender = new AndroidJavaClass("com.mobileapptracker.MATGender").GetStatic<AndroidJavaObject>("MALE");
            }
            else if (metadata.getGender() == MATAdGender.FEMALE)
            {
                gender = new AndroidJavaClass("com.mobileapptracker.MATGender").GetStatic<AndroidJavaObject>("FEMALE");
            }
            else
            {
                gender = new AndroidJavaClass("com.mobileapptracker.MATGender").GetStatic<AndroidJavaObject>("UNKNOWN");
            }
            objRequest = objRequest.Call<AndroidJavaObject>("withGender", gender);

            // Set latitude/longitude as doubles
            if (metadata.getLatitude() != 0.0 && metadata.getLongitude() != 0.0)
            {
                double latitude = Convert.ToDouble(metadata.getLatitude());
                double longitude = Convert.ToDouble(metadata.getLongitude());
                objRequest = objRequest.Call<AndroidJavaObject>("withLocation", latitude, longitude);
            }

            // Set birthdate
            if (metadata.getBirthDate().HasValue)
            {
                DateTime birthDate = metadata.getBirthDate().GetValueOrDefault();
                int year = birthDate.Year;
                int month = birthDate.Month;
                int day = birthDate.Day;
                objRequest = objRequest.Call<AndroidJavaObject>("withBirthDate", year, month, day);
            }

            // Set custom targets: create a new HashMap<String, String> and populate
            if (metadata.getCustomTargets().Count != 0)
            {
                AndroidJavaObject objMap = new AndroidJavaObject("java.util.HashMap");
                // Unity AndroidJavaObject Call doesn't support null responses, so use JNI to populate
                IntPtr methodPut = AndroidJNIHelper.GetMethodID(objMap.GetRawClass(), "put",
                                "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
                object[] args = new object[2];
                foreach (KeyValuePair<string, string> target in metadata.getCustomTargets())
                {
                    AndroidJavaObject key = new AndroidJavaObject("java.lang.String", target.Key + "");
                    AndroidJavaObject value = new AndroidJavaObject("java.lang.String", target.Value + "");
                    args[0] = key;
                    args[1] = value;
                    AndroidJNI.CallObjectMethod(objMap.GetRawObject(),methodPut, AndroidJNIHelper.CreateJNIArgArray(args));
                }
                objRequest = objRequest.Call<AndroidJavaObject>("withCustomTargets", objMap);
            }

            // Set keywords: create a new Set<String> and populate
            if (metadata.getKeywords().Count != 0)
            {
                AndroidJavaObject objSet = new AndroidJavaObject("java.util.HashSet");
                foreach (string keyword in metadata.getKeywords())
                {
                    objSet.Call<bool>("add", keyword);
                }
                objRequest = objRequest.Call<AndroidJavaObject>("withKeywords", objSet);
            }

            return objRequest;
        }
    }
    #endif
}
