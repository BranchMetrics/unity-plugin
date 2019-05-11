using UnityEngine;
using System;

namespace TuneSDK
{
    #if UNITY_ANDROID

    public class TuneAndroid : ITuneNativeBridge
    {
        private AndroidJavaObject tune;

        public TuneAndroid(string advertiserId, string conversionKey, string packageName)
        {
            AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaClass tuneClass = new AndroidJavaClass("com.tune.Tune");

            if (packageName != null) {
                tune = tuneClass.CallStatic<AndroidJavaObject>("init", currentActivity, advertiserId, conversionKey);
            } else {
                tune = tuneClass.CallStatic<AndroidJavaObject>("init", currentActivity, advertiserId, conversionKey, packageName);
            }

            tune.Call("setPluginName", "unity");
        }

        public static string GetSDKVersion()
        {
            AndroidJavaClass tuneClass = new AndroidJavaClass("com.tune.Tune");
            return tuneClass.CallStatic<string>("getSDKVersion");
        }

        public static void SetDebugMode(bool debug)
        {
            AndroidJavaClass tuneClass = new AndroidJavaClass("com.tune.Tune");
            tuneClass.CallStatic("setDebugMode", debug);
        }

        // ITune.java or TuneInternal.java methods

        public void MeasureSession()
        {
            tune.Call("measureSessionInternal");
        }

        public void MeasureEvent(string eventName)
        {
            tune.Call("measureEvent", eventName);
        }

        public void MeasureEvent(TuneEvent tuneEvent)
        {
            AndroidJavaObject objEvent = GetTuneEventJavaObject(tuneEvent);
            tune.Call("measureEvent", objEvent);
        }

        // Deeplink methods

        public void UnregisterDeeplinkListener()
        {
            tune.Call("unregisterDeeplinkListener");
        }

        public void RegisterDeeplinkListener()
        {
            AndroidJavaObject objListener = new AndroidJavaObject("com.tune.unityutils.TuneUnityDeeplinkListener");
            tune.Call("registerDeeplinkListener", objListener);
        }

        public void RegisterCustomTuneLinkDomain(string domainSuffix)
        {
            tune.Call("registerCustomTuneLinkDomain", domainSuffix);
        }

        public bool IsTuneLink(string appLinkUrl)
        {
            return tune.Call<bool>("isTuneLink", appLinkUrl);
        }

        public bool IsPayingUser()
        {
            return tune.Call<bool>("isPayingUser");
        }

        public string GetTuneId()
        {
            return tune.Call<string>("getMatId");
        }

        public string GetOpenLogId()
        {
            return tune.Call<string>("getOpenLogId");
        }

        public bool IsPrivacyProtectedDueToAge()
        {
            return tune.Call<bool>("isPrivacyProtectedDueToAge");
        }

        public void SetAppAdTracking(bool adTrackingEnabled)
        {
            tune.Call("setAppAdTrackingEnabled", adTrackingEnabled);
        }

        public void SetExistingUser(bool isExistingUser)
        {
            tune.Call("setExistingUser", isExistingUser);
        }

        public void SetFacebookEventLogging(bool fbEventLogging, bool limitEventAndDataUsage)
        {
            tune.Call("setFacebookEventLogging", fbEventLogging, limitEventAndDataUsage);
        }

        public void SetPayingUser(bool isPayingUser)
        {
            tune.Call("setPayingUser", isPayingUser);
        }

        public void SetPhoneNumber(string phoneNumber)
        {
            tune.Call("setPhoneNumber", phoneNumber);
        }

        public bool SetPrivacyProtectedDueToAge(bool isPrivacyProtected)
        {
            return tune.Call<bool>("setPrivacyProtectedDueToAge", isPrivacyProtected);
        }

        public void SetUserEmail(string userEmail)
        {
            tune.Call("setUserEmail", userEmail);
        }

        public void SetUserId(string userId)
        {
            tune.Call("setUserId", userId);
        }

        public void SetUserName(string userName)
        {
            tune.Call("setUserName", userName);
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

            tune.Call("setPreloadedAppData", objPreloadData);
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
                foreach (TuneItem item in tuneEvent.eventItems) {
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
            if (tuneEvent.rating != null)
            {
                objTuneEvent = objTuneEvent.Call<AndroidJavaObject>("withRating", tuneEvent.rating);
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

        private AndroidJavaObject GetJavaDate(DateTime dateTime)
        {
            double milliseconds = new TimeSpan(dateTime.Ticks).TotalMilliseconds;
            //datetime starts in 1970
            DateTime datetime = new DateTime(1970, 1, 1);
            double millisecondsFrom1970 = milliseconds - (new TimeSpan(datetime.Ticks)).TotalMilliseconds;
            // Convert C# DateTime to Java Date
            AndroidJavaObject msFrom1970Double = new AndroidJavaObject("java.lang.Double", millisecondsFrom1970);
            long longDate = msFrom1970Double.Call<long>("longValue");
            AndroidJavaObject date = new AndroidJavaObject("java.util.Date", longDate);
            return date;
        }

        public void AutomateIapEventMeasurement(bool automate)
        {
            // iOS only
        }

        public void SetJailbroken(bool isJailbroken)
        {
            // iOS only
        }
    }
    #endif
}
