using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace TuneSDK
{
    #if UNITY_ANDROID

    public class TuneAndroid
    {
        private static TuneAndroid instance;

        // Tune.java for Android is already encapsulated.
        private AndroidJavaClass ajcTune = new AndroidJavaClass("com.tune.Tune");
        private AndroidJavaClass ajcUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        private AndroidJavaObject ajcCurrentActivity;
        public AndroidJavaObject ajcInstance;

        private TuneAndroid()
        {
            ajcCurrentActivity = ajcUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        }

        public static TuneAndroid Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TuneAndroid();
                }
                return instance;
            }
        }

        /// <summary>
        /// Initializes the reference to the AndroidJavaClass Tune object.
        /// Does nothing if already initialized.
        /// </summary>
        public void Init(string advertiserId, string conversionKey)
        {
            ajcInstance = ajcTune.CallStatic<AndroidJavaObject>("init", ajcCurrentActivity, advertiserId, conversionKey);
            ajcInstance.Call("setPluginName", "unity");
        }

        /// <summary>
        /// Initializes the reference to the AndroidJavaClass Tune object.
        /// Does nothing if already initialized.
        /// </summary>
        public void Init(string advertiserId, string conversionKey, bool turnOnTMA)
        {
            ajcInstance = ajcTune.CallStatic<AndroidJavaObject>("init", ajcCurrentActivity, advertiserId, conversionKey, turnOnTMA);
            ajcInstance.Call("setPluginName", "unity");
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

        public void MeasureEvent(TuneEvent tuneEvent)
        {
            AndroidJavaObject objEvent = GetTuneEventJavaObject(tuneEvent);
            ajcInstance.Call("measureEvent", objEvent);
        }

        public void CheckForDeferredDeeplink()
        {
            // Get the predefined Unity TUNE deeplink listener
            AndroidJavaObject objListener = new AndroidJavaObject("com.tune.unityutils.TuneUnityDeeplinkListener");
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

        public void SetDeepLink(string deepLinkUrl)
        {
            ajcInstance.Call("setReferralUrl", deepLinkUrl);
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
                // Get the predefined Unity listener response and set it
                AndroidJavaObject objResponse = new AndroidJavaObject("com.tune.unityutils.TuneUnityListener");
                ajcInstance.Call("setListener", objResponse);
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
            AndroidJavaObject tuneGender;
            if (gender == 0)
            {
                tuneGender = new AndroidJavaClass("com.tune.TuneGender").GetStatic<AndroidJavaObject>("MALE");
            }
            else if (gender == 1)
            {
                tuneGender = new AndroidJavaClass("com.tune.TuneGender").GetStatic<AndroidJavaObject>("FEMALE");
            }
            else
            {
                tuneGender = new AndroidJavaClass("com.tune.TuneGender").GetStatic<AndroidJavaObject>("UNKNOWN");
                ajcInstance.Call("setGender", tuneGender);
            }
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

        public void SetShouldAutoCollectDeviceLocation(bool shouldAutoCollect)
        {
            ajcInstance.Call("setShouldAutoCollectDeviceLocation", shouldAutoCollect);
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
        public void SetPreloadedApp(TunePreloadData preloadData)
        {
            // Convert C# TunePreloadData to Java TunePreloadData
            AndroidJavaObject objPreloadData = new AndroidJavaObject("com.tune.TunePreloadData", preloadData.publisherId);
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

        /* In-App Marketing functions */

        // Profile API
        public void RegisterCustomProfileString(string variableName)
        {
            ajcInstance.Call("registerCustomProfileString", variableName);
        }

        public void RegisterCustomProfileString(string variableName, string defaultValue)
        {
            ajcInstance.Call("registerCustomProfileString", variableName, defaultValue);
        }

        public void RegisterCustomProfileDate(string variableName)
        {
            ajcInstance.Call("registerCustomProfileDate", variableName);
        }

        public void RegisterCustomProfileDate(string variableName, DateTime defaultValue)
        {
            // Convert C# DateTime to Java Date
            AndroidJavaObject date = GetJavaDate(defaultValue);
            ajcInstance.Call("registerCustomProfileDate", variableName, date);
        }

        public void RegisterCustomProfileNumber(string variableName)
        {
            ajcInstance.Call("registerCustomProfileNumber", variableName);
        }

        public void RegisterCustomProfileNumber(string variableName, int defaultValue)
        {
            ajcInstance.Call("registerCustomProfileNumber", variableName, defaultValue);
        }

        public void RegisterCustomProfileNumber(string variableName, double defaultValue)
        {
            ajcInstance.Call("registerCustomProfileNumber", variableName, defaultValue);
        }

        public void RegisterCustomProfileNumber(string variableName, float defaultValue)
        {
            ajcInstance.Call("registerCustomProfileNumber", variableName, defaultValue);
        }

        public void RegisterCustomProfileGeoLocation(string variableName)
        {
            ajcInstance.Call("registerCustomProfileGeolocation", variableName);
        }

        public void RegisterCustomProfileGeoLocation(string variableName, TuneLocation defaultValue)
        {
            // Convert C# TuneLocation to Java TuneLocation
            AndroidJavaObject objLocation = new AndroidJavaObject("com.tune.TuneLocation", defaultValue.longitude, defaultValue.latitude);
            ajcInstance.Call("registerCustomProfileGeolocation", variableName, objLocation);
        }

        public void SetCustomProfileString(string variableName, string value)
        {
            ajcInstance.Call("setCustomProfileStringValue", variableName, value);
        }

        public void SetCustomProfileDate(string variableName, DateTime value)
        {
            // Convert C# DateTime to Java Date
            AndroidJavaObject date = GetJavaDate(value);
            ajcInstance.Call("setCustomProfileDate", variableName, date);
        }

        public void SetCustomProfileNumber(string variableName, int value)
        {
            ajcInstance.Call("setCustomProfileNumber", variableName, value);
        }

        public void SetCustomProfileNumber(string variableName, double value)
        {
            ajcInstance.Call("setCustomProfileNumber", variableName, value);
        }

        public void SetCustomProfileNumber(string variableName, float value)
        {
            ajcInstance.Call("setCustomProfileNumber", variableName, value);
        }

        public void SetCustomProfileGeolocation(string variableName, TuneLocation value)
        {
            // Convert C# TuneLocation to Java TuneLocation
            AndroidJavaObject objLocation = new AndroidJavaObject("com.tune.TuneLocation", value.longitude, value.latitude);
            ajcInstance.Call("setCustomProfileGeolocation", variableName, objLocation);
        }

        public string GetCustomProfileString(string variableName)
        {
            return ajcInstance.Call<string>("getCustomProfileString", variableName);
        }

        public DateTime GetCustomProfileDate(string variableName)
        {
            AndroidJavaObject dateObj = ajcInstance.Call<AndroidJavaObject>("getCustomProfileDate", variableName);
            if (dateObj != null)
            {
                long unixTimeMillis = dateObj.Call<long>("getTime");
                // Add ms from dateObj to ms of 1/1/1970
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return epoch.AddMilliseconds(unixTimeMillis);
            }
            return new DateTime();
        }

        public double GetCustomProfileNumber(string variableName)
        {
            AndroidJavaObject numObj = ajcInstance.Call<AndroidJavaObject>("getCustomProfileNumber", variableName);
            if (numObj != null)
            {
                return numObj.Call<double>("doubleValue");
            }
            return -1;
        }

        public TuneLocation GetCustomProfileGeolocation(string variableName)
        {
            TuneLocation loc = new TuneLocation();
            AndroidJavaObject locObj = ajcInstance.Call<AndroidJavaObject>("getCustomProfileGeolocation", variableName);
            if (locObj != null)
            {
                double latitude = locObj.Call<double>("getLatitude");
                double longitude = locObj.Call<double>("getLongitude");
                loc.latitude = latitude;
                loc.longitude = longitude;
            }
            return loc;
        }

        public void ClearCustomProfileVariable(string variableName)
        {
            ajcInstance.Call("clearCustomProfileVariable", variableName);
        }

        public void ClearAllCustomProfileVariables()
        {
            ajcInstance.Call("clearAllCustomProfileVariables");
        }

        // Power Hook API
        public void RegisterPowerHook(string hookId, string friendlyName, string defaultValue)
        {
            ajcInstance.Call("registerPowerHook", hookId, friendlyName, defaultValue);
        }

        public string GetValueForHookById(string hookId)
        {
            return ajcInstance.Call<string>("getValueForHookById", hookId);
        }

        public void SetValueForHookById(string hookId, string value)
        {
            ajcInstance.Call("setValueForHookById", hookId, value);
        }

        public void OnPowerHooksChanged(bool listenForPowerHooksChanged)
        {
            if (listenForPowerHooksChanged)
            {
                // Get the predefined Unity TUNE power hooks changed listener
                AndroidJavaObject objListener = new AndroidJavaObject("com.tune.unityutils.TuneUnityPowerHooksListener");
                ajcInstance.Call("onPowerHooksChanged", objListener);
            }
        }

        // Experiment Details API

        public Dictionary<string, TunePowerHookExperimentDetails> GetPowerHookExperimentDetails()
        {
            // Construct Dictionary from Java Map
            Dictionary<string, TunePowerHookExperimentDetails> dict = new Dictionary<string, TunePowerHookExperimentDetails>();
            AndroidJavaObject detailsMap = ajcInstance.Call<AndroidJavaObject>("getPowerHookExperimentDetails");

            if (detailsMap != null)
            {
                // Iterate through Java Map and create C# Dictionary of TunePowerHookExperimentDetails
                AndroidJavaObject entrySet = detailsMap.Call<AndroidJavaObject>("entrySet");
                AndroidJavaObject iterator = entrySet.Call<AndroidJavaObject>("iterator");

                while (iterator.Call<bool>("hasNext"))
                {
                    AndroidJavaObject pair = iterator.Call<AndroidJavaObject>("next");
                    string key = pair.Call<string>("getKey");
                    AndroidJavaObject value = pair.Call<AndroidJavaObject>("getValue");
                    dict.Add(key, new TunePowerHookExperimentDetails(value));
                }
            }

            return dict;
        }

        // On First Playlist Downloaded API

        public void OnFirstPlaylistDownloaded(bool listenForFirstPlaylist)
        {
            if (listenForFirstPlaylist)
            {
                // Get the predefined Unity TUNE playlist downloaded listener
                AndroidJavaObject objListener = new AndroidJavaObject("com.tune.unityutils.TuneUnityFirstPlaylistListener");
                ajcInstance.Call("onFirstPlaylistDownloaded", objListener);
            }
        }

        public void OnFirstPlaylistDownloaded(bool listenForFirstPlaylist, long timeout)
        {
            if (listenForFirstPlaylist)
            {
                // Get the predefined Unity TUNE first playlist downloaded listener
                AndroidJavaObject objListener = new AndroidJavaObject("com.tune.unityutils.TuneUnityFirstPlaylistListener");
                ajcInstance.Call("onFirstPlaylistDownloaded", objListener, timeout);
            }
        }

        // Push API

        public void SetPushNotificationSenderId(string pushSenderId)
        {
            ajcInstance.Call("setPushNotificationSenderId", pushSenderId);
        }

        public void SetPushNotificationRegistrationId(string registrationId)
        {
            ajcInstance.Call("setPushNotificationRegistrationId", registrationId);
        }

        public void SetOptedOutOfPush(bool optedOutOfPush)
        {
            ajcInstance.Call("setOptedOutOfPush", optedOutOfPush);
        }

        public string GetDeviceToken()
        {
            return ajcInstance.Call<string>("getDeviceToken");
        }

        public bool DidUserManuallyDisablePush()
        {
            return ajcInstance.Call<bool>("didUserManuallyDisablePush");
        }

        public bool DidSessionStartFromTunePush()
        {
            return ajcInstance.Call<bool>("didSessionStartFromTunePush");
        }

        public TunePushInfo GetTunePushInfoForSession()
        {
            string campaignId = "";
            string pushId = "";
            Dictionary<string, string> extrasPayload = new Dictionary<string, string>();
            // AndroidJavaObject cannot be null, catch Exception:
            // https://github.com/nraboy/unity3d-floppy-clone-game/blob/master/Assets/GooglePlayGames/Platforms/Android/JavaUtil.cs#L73
            try {
                AndroidJavaObject tunePushInfoJava = ajcInstance.Call<AndroidJavaObject>("getTunePushInfoForSession");
                // TODO: convert AndroidJavaObject to TunePushInfo
                campaignId = tunePushInfoJava.Call<string>("getCampaignId");
                pushId = tunePushInfoJava.Call<string>("getPushId");
                AndroidJavaObject extrasPayloadJava = tunePushInfoJava.Call<AndroidJavaObject>("getExtrasPayload");
                extrasPayload = JsonToDictionary(extrasPayloadJava);
            } catch (Exception) {
            }
            TunePushInfo pushInfo = new TunePushInfo(campaignId, pushId, extrasPayload);
            return pushInfo;
        }

        // Segment API

        public bool IsUserInSegmentId(string segmentId)
        {
            return ajcInstance.Call<bool>("isUserInSegmentId", segmentId);
        }

        public bool IsUserInAnySegmentIds(string[] segmentIds)
        {
            // Convert string[] to Java ArrayList
            AndroidJavaObject objArrayList = new AndroidJavaObject("java.util.ArrayList");
            foreach (string segmentId in segmentIds)
            {
                // Add to list of segment ids
                objArrayList.Call<bool>("add", segmentId);
            }
            return ajcInstance.Call<bool>("isUserInAnySegmentIds", objArrayList);
        }

        public void ForceSetUserInSegmentId(string segmentId, bool isInSegment)
        {
            AndroidJavaClass ajcDebugUtilities = new AndroidJavaClass("com.tune.TuneDebugUtilities");
            ajcDebugUtilities.CallStatic("forceSetUserInSegmentId", segmentId, isInSegment);
        }

        // Helper functions

        private Dictionary<string, string> JsonToDictionary(AndroidJavaObject jsonObject)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            // Iterate through JSONObject's keys
            AndroidJavaObject keys = jsonObject.Call<AndroidJavaObject>("keys");

            while (keys.Call<bool>("hasNext"))
            {
                string key = keys.Call<string>("next");
                dict.Add(key, jsonObject.Call<string>("get", key));
            }

            return dict;
        }

        private AndroidJavaObject DictionaryToMap(Dictionary<string, string> dict)
        {
            AndroidJavaObject map = new AndroidJavaObject("java.util.HashMap");
            if (dict != null)
            {
                foreach (KeyValuePair<string, string> entry in dict)
                {
                    map.Call<string>("put", entry.Key, entry.Value);
                }
            }
            return map;
        }

        private AndroidJavaObject GetTuneEventJavaObject(TuneEvent tuneEvent)
        {
            // Convert C# TuneEvent to new Java TuneEvent object
            AndroidJavaObject objTuneEvent = new AndroidJavaObject("com.tune.TuneEvent", tuneEvent.name);

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
                // Convert TuneItem[] to Arraylist<TuneEventItem>
                AndroidJavaObject objArrayList = new AndroidJavaObject("java.util.ArrayList");
                foreach (TuneItem item in tuneEvent.eventItems)
                {
                    // Convert TuneItem to TuneEventItem
                    AndroidJavaObject objEventItem = new AndroidJavaObject("com.tune.TuneEventItem", item.name);
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
                    // Add to list of TuneEventItems
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
                AndroidJavaObject objDate = GetJavaDate((DateTime)tuneEvent.date1);
                objTuneEvent = objTuneEvent.Call<AndroidJavaObject>("withDate1", objDate);
            }
            if (tuneEvent.date2 != null) {
                AndroidJavaObject objDate = GetJavaDate((DateTime)tuneEvent.date2);
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

        private AndroidJavaObject GetJavaDate(DateTime date)
        {
            double milliseconds = new TimeSpan(date.Ticks).TotalMilliseconds;
            //datetime starts in 1970
            DateTime datetime = new DateTime(1970, 1, 1);
            double millisecondsFrom1970 = milliseconds - (new TimeSpan(datetime.Ticks)).TotalMilliseconds;
            // Convert C# DateTime to Java Date
            AndroidJavaObject objDouble = new AndroidJavaObject("java.lang.Double", millisecondsFrom1970);
            long longDate = objDouble.Call<long>("longValue");
            AndroidJavaObject objDate = new AndroidJavaObject("java.util.Date", longDate);
            return objDate;
        }
    }
    #endif
}
