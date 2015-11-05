using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
#if UNITY_METRO
using MATWinStore;
#endif

namespace MATSDK
{
    /// <para>
    /// MATBinding wraps Android, iOS, and Windows Tune/MobileAppTracking SDK
    /// functions to be used within Unity. The functions can be called within any
    /// C# script by typing MATBinding.*.
    /// </para>
    public class MATBinding : MonoBehaviour
    {
        /// <para>
        /// Initializes relevant information about the advertiser and
        /// conversion key on startup of Unity game.
        /// </para>
        /// <param name="advertiserId">the MAT advertiser ID for the app</param>
        /// <param name="conversionKey">the MAT advertiser key for the app</param>
        public static void Init(string advertiserId, string conversionKey)
        {
            if(!Application.isEditor)
            {
                #if UNITY_ANDROID
                MATAndroid.Instance.Init(advertiserId, conversionKey);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneInit(advertiserId, conversionKey );
                #endif
                #if UNITY_METRO
                MobileAppTracker.InitializeValues(advertiserId, conversionKey);
                #endif
            }
        }

        /// <para>
        /// Initializes relevant information about the advertiser and
        /// conversion key on startup of Unity game.
        /// </para>
        /// <param name="advertiserId">the MAT advertiser ID for the app</param>
        /// <param name="conversionKey">the MAT advertiser key for the app</param>
        /// <param name="packageName">the package name used when setting up the app in MobileAppTracking</param>
        /// <param name="wearable">should be set to YES when being initialized in a WatchKit extension, defaults to NO</param>
        public static void Init(string advertiserId, string conversionKey, string packageName, bool wearable)
        {
            if(!Application.isEditor)
            {
                #if UNITY_ANDROID
                MATAndroid.Instance.Init(advertiserId, conversionKey);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneInitForWearable(advertiserId, conversionKey, packageName, wearable);
                #endif
                #if UNITY_METRO
                MobileAppTracker.InitializeValues(advertiserId, conversionKey);
                #endif
            }
        }

        public static void CacheInterstitial(string placement)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.CacheInterstitial(placement);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneCacheInterstitial(placement);
                #endif
            }
        }

        public static void CacheInterstitial(string placement, MATAdMetadata metadata)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.CacheInterstitial(placement, metadata);
                #endif
                #if UNITY_IPHONE
                IntPtr metadataPtr = CreateMetadata(metadata);
                MATExterns.TuneCacheInterstitialWithMetadata(placement, metadataPtr);
                MATExterns.TuneRelease(metadataPtr);
                #endif
            }
        }

        public static void ShowInterstitial(string placement)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.ShowInterstitial(placement);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneShowInterstitial(placement);
                #endif
            }
        }

        public static void ShowInterstitial(string placement, MATAdMetadata metadata)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.ShowInterstitial(placement, metadata);
                #endif
                #if UNITY_IPHONE
                IntPtr metadataPtr = CreateMetadata(metadata);
                MATExterns.TuneShowInterstitialWithMetadata(placement, metadataPtr);
                MATExterns.TuneRelease(metadataPtr);
                #endif
            }
        }

        public static void DestroyInterstitial()
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.DestroyInterstitial();
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneDestroyInterstitial();
                #endif
            }
        }

        #if UNITY_IPHONE
        // Load an ad.
        private static IntPtr CreateMetadata(MATAdMetadata metadata)
        {
            IntPtr metadataPtr = MATExterns.TuneCreateMetadata();
            foreach (string keyword in metadata.getKeywords()) {
                MATExterns.TuneAddKeyword(metadataPtr, keyword);
            }

            if (metadata.getBirthDate().HasValue) {
                DateTime birthDate = metadata.getBirthDate().GetValueOrDefault();
                MATExterns.TuneSetBirthDate(metadataPtr, birthDate.Year, birthDate.Month, birthDate.Day);
            }

            MATExterns.TuneAdSetDebugMode(metadataPtr, (bool)metadata.getDebugMode ());
            MATExterns.TuneAdSetGender(metadataPtr, (int)metadata.getGender ());

            MATExterns.TuneSetLatitude(metadataPtr, (int)metadata.getLatitude ());
            MATExterns.TuneSetLongitude(metadataPtr, (int)metadata.getLongitude ());
            MATExterns.TuneSetAltitude(metadataPtr, (int)metadata.getAltitude ());

            foreach (KeyValuePair<string, string> entry in metadata.getCustomTargets()) {
                MATExterns.TuneSetCustomTargets(metadataPtr, entry.Key, entry.Value);
            }

            return metadataPtr;
        }
        #endif

        public static void ShowBanner(string placement)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.ShowBanner(placement);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneShowBanner(placement);
                #endif
            }
        }

        public static void ShowBanner(string placement, MATAdMetadata metadata)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.ShowBanner(placement, metadata);
                #endif
                #if UNITY_IPHONE
                ShowBanner(placement, metadata, MATBannerPosition.BOTTOM_CENTER);
                #endif
            }
        }

        public static void ShowBanner(string placement, MATAdMetadata metadata, MATBannerPosition position)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.ShowBanner(placement, metadata, position);
                #endif
                #if UNITY_IPHONE

                IntPtr metadataPtr = null == metadata ? IntPtr.Zero : CreateMetadata(metadata);

                MATExterns.TuneShowBannerWithPosition(placement, metadataPtr, position);

                if(IntPtr.Zero != metadataPtr)
                {
                    MATExterns.TuneRelease(metadataPtr);
                }
                #endif
            }
        }

        public static void HideBanner()
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.HideBanner();
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneHideBanner();
                #endif
            }
        }

        public static void LayoutBanner()
        {
            if (!Application.isEditor) {
                #if UNITY_IPHONE
                MATExterns.TuneLayoutBanner();
                #endif
            }
        }

        public static void DestroyBanner()
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.DestroyBanner();
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneDestroyBanner();
                #endif
            }
        }

        /// <para>
        /// Check if a deferred deep-link is available.
        /// </para>
        public static void CheckForDeferredDeeplink()
        {
            if(!Application.isEditor)
            {
                #if UNITY_ANDROID
                MATAndroid.Instance.CheckForDeferredDeeplink();
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneCheckForDeferredDeeplink();
                #endif
                #if UNITY_METRO
                //MATWinStore.MobileAppTracker.Instance.CheckForDeferredDeeplink();
                #endif
            }
        }

        /// <para>
        /// Enable auto-measurement of successful in-app-purchase (IAP) transactions as "purhcase" events in iOS.
        /// </para>
        /// <param name="automate">should auto-measure IAP transactions</param>
        public static void AutomateIapEventMeasurement(bool automate)
        {
            if(!Application.isEditor)
            {
                #if UNITY_IPHONE
                MATExterns.TuneAutomateIapEventMeasurement(automate);
                #endif
            }
        }

        /// <para>
        /// Sets the deep link url that was used to launch this app and the bundle id of the source application.
        /// </para>
        /// <param name="deepLinkUrl">URL fired by the source application to launch this app</param>
        public static void SetDeepLink(string deepLinkUrl)
        {
            if(!Application.isEditor)
            {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetDeepLink(deepLinkUrl);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneSetDeepLink(deepLinkUrl);
                #endif
            }
        }

        /// <para>
        /// Event measurement function, by event name.
        /// </para>
        /// <param name="eventName">event name in MAT system</param>
        public static void MeasureEvent(string eventName)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                MATAndroid.Instance.MeasureEvent(eventName);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneMeasureEventName(eventName);
                #endif
                #if UNITY_METRO
                MobileAppTracker.Instance.MeasureAction(eventName);
                #endif
            }
        }

        #if (UNITY_ANDROID || UNITY_IPHONE)
        /// <para>
        /// Event measurement function, by event id.
        /// </para>
        /// <param name="eventId">event id in MAT system</param>
        public static void MeasureEvent(int eventId)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                MATAndroid.Instance.MeasureEvent(eventId);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneMeasureEventId(eventId);
                #endif
//                #if UNITY_METRO
//                MATWinStore.MobileAppTracker.Instance.MeasureAction(eventId);
//                #endif
            }
        }
        #endif

        /// <para>
        /// Measures the event with MATEvent.
        /// </para>
        /// <param name="tuneEvent">MATEvent object of event values to measure</param>
        public static void MeasureEvent(MATEvent tuneEvent)
        {
            if (!Application.isEditor)
            {
                int itemCount = null == tuneEvent.eventItems ? 0 : tuneEvent.eventItems.Length;

                #if UNITY_ANDROID
                MATAndroid.Instance.MeasureEvent(tuneEvent);
                #endif
                
                #if UNITY_METRO
                // Set event characteristic values separately
                SetEventContentType(tuneEvent.contentType);
                SetEventContentId(tuneEvent.contentId);

                // Set event characteristic values separately
                SetEventContentType(tuneEvent.contentType);
                SetEventContentId(tuneEvent.contentId);
                if (tuneEvent.level != null)
                {
                    SetEventLevel((int)tuneEvent.level);
                }
                if (tuneEvent.quantity != null)
                {
                    SetEventQuantity((int)tuneEvent.quantity);
                }
                SetEventSearchString(tuneEvent.searchString);
                if (tuneEvent.rating != null)
                {
                    SetEventRating((float)tuneEvent.rating);
                }
                if (tuneEvent.date1 != null)
                {
                    SetEventDate1((DateTime)tuneEvent.date1);
                }
                if (tuneEvent.date2 != null)
                {
                    SetEventDate2((DateTime)tuneEvent.date2);
                }
                SetEventAttribute1(tuneEvent.attribute1);
                SetEventAttribute2(tuneEvent.attribute2);
                SetEventAttribute3(tuneEvent.attribute3);
                SetEventAttribute4(tuneEvent.attribute4);
                SetEventAttribute5(tuneEvent.attribute5);
                #endif
                
                #if UNITY_IPHONE
                MATEventIos eventIos = new MATEventIos(tuneEvent);

                byte[] receiptBytes = null == tuneEvent.receipt ? null : System.Convert.FromBase64String(tuneEvent.receipt);
                int receiptByteCount = null == receiptBytes ? 0 : receiptBytes.Length;

                // Convert MATItem to C-marshallable struct MATItemIos
                MATItemIos[] items = new MATItemIos[itemCount];
                for (int i = 0; i < itemCount; i++)
                {
                    items[i] = new MATItemIos(tuneEvent.eventItems[i]);
                }

                MATExterns.TuneMeasureEvent(eventIos, items, itemCount, receiptBytes, receiptByteCount);
                #endif

                #if UNITY_METRO
                //Convert MATItem[] to MATEventItem[]. These are the same things, but must be converted for recognition of
                //MobileAppTracker.cs.
                MATEventItem[] newarr = new MATEventItem[itemCount];
                //Conversion is necessary to prevent the need of referencing a separate class.
                for(int i = 0; i < itemCount; i++)
                {
                    MATItem item = tuneEvent.eventItems[i];
                    newarr[i] = new MATEventItem(item.name,
                                                 item.quantity == null ? 0 : (int)item.quantity,
                                                 item.unitPrice == null ? 0 : (double)item.unitPrice,
                                                 item.revenue == null ? 0 : (double)item.revenue,
                                                 item.attribute1,
                                                 item.attribute2,
                                                 item.attribute3,
                                                 item.attribute4,
                                                 item.attribute5);
                }

                List<MATEventItem> list =  newarr.ToList();
                MobileAppTracker.Instance.MeasureAction(tuneEvent.name,
                                                        tuneEvent.revenue == null ? 0 : (double)tuneEvent.revenue,
                                                        tuneEvent.currencyCode,
                                                        tuneEvent.advertiserRefId,
                                                        list);
                #endif
            }
        }

        /// <para>
        /// Main session measurement function; this function should be called at every app open.
        /// </para>
        public static void MeasureSession()
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.MeasureSession();
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneMeasureSession();
                #endif
                #if UNITY_METRO
                MobileAppTracker.Instance.MeasureSession();
                #endif
            }
        }

        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        /*----------------------------------Setters--------------------------------*/
        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////

        /// <para>
        /// Sets the user's age.
        /// </para>
        /// <param name="age">User age to track in MAT</param>
        public static void SetAge(int age)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetAge(age);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneSetAge(age);
                #endif
                #if UNITY_METRO
                MobileAppTracker.Instance.SetAge(age);
                #endif
            }
        }

        /// <para>
        /// Enables acceptance of duplicate installs from this device.
        /// </para>
        /// <param name="allow">whether to allow duplicate installs from device</param>
        public static void SetAllowDuplicates(bool allow)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetAllowDuplicates(allow);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneSetAllowDuplicates(allow);
                #endif
                #if UNITY_METRO
                MobileAppTracker.Instance.SetAllowDuplicates(allow);
                #endif
            }
        }

        /// <para>Sets whether app-level ad tracking is enabled.</para>
        /// <param name="adTrackingEnabled">true if user has opted out of ad tracking at the app-level, false if not</param>
        public static void SetAppAdTracking(bool adTrackingEnabled)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetAppAdTracking(adTrackingEnabled);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneSetAppAdTracking(adTrackingEnabled);
                #endif
                #if UNITY_METRO
                MobileAppTracker.Instance.SetAppAdTracking(adTrackingEnabled);
                #endif
            }
        }

        /// <para>
        /// Turns debug mode on or off, under tag "MobileAppTracker".
        /// </para>
        /// <param name="debug">whether to enable debug output</param>
        public static void SetDebugMode(bool debug)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetDebugMode(debug);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneSetDebugMode(debug);
                #endif
                #if UNITY_METRO
                MobileAppTracker.Instance.SetDebugMode(debug);
                #endif
            }
        }


        /// <para>
        /// Sets the first attribute associated with an app event.
        /// </para>
        /// <param name="eventAttribute">the attribute</param>
        private static void SetEventAttribute1(string eventAttribute)
        {
            if(!Application.isEditor)
            {
                #if UNITY_METRO
                MobileAppTracker.Instance.SetEventAttribute1(eventAttribute);
                #endif
            }
        }
        /// <para>
        /// Sets the second attribute associated with an app event.
        /// </para>
        /// <param name="eventAttribute">the attribute</param>
        private static void SetEventAttribute2(string eventAttribute)
        {
            if(!Application.isEditor)
            {
                #if UNITY_METRO
                MobileAppTracker.Instance.SetEventAttribute2(eventAttribute);
                #endif
            }
        }
        /// <para>
        /// Sets the third attribute associated with an app event.
        /// </para>
        /// <param name="eventAttribute">the attribute</param>
        private static void SetEventAttribute3(string eventAttribute)
        {
            if(!Application.isEditor)
            {
                #if UNITY_METRO
                MobileAppTracker.Instance.SetEventAttribute3(eventAttribute);
                #endif
            }
        }
        /// <para>
        /// Sets the fourth attribute associated with an app event.
        /// </para>
        /// <param name="eventAttribute">the attribute</param>
        private static void SetEventAttribute4(string eventAttribute)
        {
            if(!Application.isEditor)
            {
                #if UNITY_METRO
                MobileAppTracker.Instance.SetEventAttribute4(eventAttribute);
                #endif
            }
        }
        /// <para>
        /// Sets the fifth attribute associated with an app event.
        /// </para>
        /// <param name="eventAttribute">the attribute</param>
        private static void SetEventAttribute5(string eventAttribute)
        {
            if(!Application.isEditor)
            {
                #if UNITY_METRO
                MobileAppTracker.Instance.SetEventAttribute5(eventAttribute);
                #endif
            }
        }
        /// <para>
        /// Sets the content ID associated with an app event.
        /// </para>
        /// <param name="eventContentId">the content ID</param>
        private static void SetEventContentId(string eventContentId)
        {
            if(!Application.isEditor)
            {
                #if UNITY_METRO
                MobileAppTracker.Instance.SetEventContentId(eventContentId);
                #endif
            }
        }
        /// <para>
        /// Sets the content type associated with an app event.
        /// </para>
        /// <param name="eventContentType">the content type</param>
        private static void SetEventContentType(string eventContentType)
        {
            if(!Application.isEditor)
            {
                #if UNITY_METRO
                MobileAppTracker.Instance.SetEventContentType(eventContentType);
                #endif
            }
        }
        /// <para>
        /// Sets the first date associated with an app event.
        /// Should be 1/1/1970 and after.
        /// </para>
        /// <param name="eventDate">the date</param>
        private static void SetEventDate1(DateTime eventDate)
        {
            if(!Application.isEditor)
            {
                #if UNITY_METRO
                double milliseconds = new TimeSpan(eventDate.Ticks).TotalMilliseconds;
                
                // datetime starts in 1970
                DateTime datetime = new DateTime(1970, 1, 1);
                double millisecondsFrom1970 = milliseconds - (new TimeSpan(datetime.Ticks)).TotalMilliseconds;
                TimeSpan timeFrom1970 = TimeSpan.FromMilliseconds(millisecondsFrom1970);
                
                //Update to current time for c#
                datetime = datetime.Add(timeFrom1970);
                MobileAppTracker.Instance.SetEventDate1(datetime);
                #endif
            }
        }
        /// <para>
        /// Sets the second date associated with an app event.
        /// </para>
        /// <param name="eventDate">the date.</param>
        private static void SetEventDate2(DateTime eventDate)
        {
            if(!Application.isEditor)
            {
                #if UNITY_METRO
                double milliseconds = new TimeSpan(eventDate.Ticks).TotalMilliseconds;
                //datetime starts in 1970
                DateTime datetime = new DateTime(1970, 1, 1);
                double millisecondsFrom1970 = milliseconds - (new TimeSpan(datetime.Ticks)).TotalMilliseconds;
                
                TimeSpan timeFrom1970 = TimeSpan.FromMilliseconds(millisecondsFrom1970);
                //Update to current time for c#
                datetime = datetime.Add(timeFrom1970);
                MobileAppTracker.Instance.SetEventDate2(datetime);
                #endif
            }
        }
        /// <para>
        /// Sets the level associated with an app event.
        /// </para>
        /// <param name="eventLevel">the level</param>
        private static void SetEventLevel(int eventLevel)
        {
            if(!Application.isEditor)
            {
                #if UNITY_METRO
                MobileAppTracker.Instance.SetEventLevel(eventLevel);
                #endif
            }
        }
        /// <para>
        /// Sets the quantity associated with an app event.
        /// </para>
        /// <param name="eventQuantity">the quantity</param>
        private static void SetEventQuantity(int eventQuantity)
        {
            if(!Application.isEditor)
            {
                #if UNITY_METRO
                MobileAppTracker.Instance.SetEventQuantity(eventQuantity);
                #endif
            }
        }
        /// <para>
        /// Sets the rating associated with an app event.
        /// </para>
        /// <param name="eventRating">the rating</param>
        private static void SetEventRating(float eventRating)
        {
            if(!Application.isEditor)
            {
                #if UNITY_METRO
                MobileAppTracker.Instance.SetEventRating(eventRating);
                #endif
            }
        }
        /// <para>
        /// Sets the search string associated with an app event.
        /// </para>
        /// <param name="eventSearchString">the search string</param>
        private static void SetEventSearchString(string eventSearchString)
        {
            if(!Application.isEditor)
            {
                #if UNITY_METRO
                MobileAppTracker.Instance.SetEventSearchString(eventSearchString);
                #endif
            }
        }

        /// <para>
        /// Sets whether app was previously installed prior to version with Tune SDK.
        /// </para>
        /// <param name="isExistingUser">
        /// existing true if this user already had the app installed prior to updating to Tune version
        /// </param>
        public static void SetExistingUser(bool isExistingUser)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetExistingUser(isExistingUser);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneSetExistingUser(isExistingUser);
                #endif
                #if UNITY_METRO
                MobileAppTracker.Instance.SetExistingUser(isExistingUser);
                #endif
            }
        }

        /// <para>
        /// Set whether the Tune events should also be logged to the Facebook SDK. This flag is ignored if the Facebook SDK is not present.
        /// </para>
        /// <param name="enable">Whether to send Tune events to FB as well</param>
        /// <param name="limitEventAndDataUsabe">Whether data such as that generated through FBAppEvents and sent to Facebook should be restricted from being used for other than analytics and conversions. Defaults to NO. This value is stored on the device and persists across app launches.</param>
        public static void SetFacebookEventLogging(bool enable, bool limit)
        {
            if(!Application.isEditor)
            {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetFacebookEventLogging(enable, limit);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneSetFacebookEventLogging(enable, limit);
                #endif
                #if UNITY_METRO
                //MobileAppTracker.Instance.SetFacebookEventLogging(enable, limit);
                #endif
            }
        }

        /// <para>
        /// Sets the user ID to associate with Facebook.
        /// </para>
        /// <param name="fbUserId">Facebook User ID</param>
        public static void SetFacebookUserId(string fbUserId)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetFacebookUserId(fbUserId);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneSetFacebookUserId(fbUserId);
                #endif
                #if UNITY_METRO
                MobileAppTracker.Instance.SetFacebookUserId(fbUserId);
                #endif
            }
        }

        /// <para>
        /// Sets the user gender.
        /// </para>
        /// <param name="gender">use TuneGenderMale, TuneGenderFemale, TuneGenderUnknown</param>
        public static void SetGender(int gender)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetGender(gender);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneSetGender(gender);
                #endif

                #if UNITY_METRO
                MATGender gender_temp;
                if(gender == 0)
                    gender_temp = MATGender.MALE;
                else if (gender == 1)
                    gender_temp = MATGender.FEMALE;
                else
                    gender_temp = MATGender.NONE;

                MobileAppTracker.Instance.SetGender(gender_temp);
                #endif
            }
        }

        /// <para>
        /// Sets the user ID to associate with Google.
        /// </para>
        /// <param name="googleUserId">Google user ID.</param>
        public static void SetGoogleUserId(string googleUserId)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetGoogleUserId(googleUserId);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneSetGoogleUserId(googleUserId);
                #endif
                #if UNITY_METRO
                MobileAppTracker.Instance.SetGoogleUserId(googleUserId);
                #endif
            }
        }

        /// <para>
        /// Sets the user's latitude, longitude, and altitude.
        /// </para>
        /// <param name="latitude">user's latitude</param>
        /// <param name="longitude">user's longitude</param>
        /// <param name="altitude">user's altitude</param>
        public static void SetLocation(double latitude, double longitude, double altitude)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetLocation(latitude, longitude, altitude);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneSetLocation(latitude, longitude, altitude);
                #endif
                #if UNITY_METRO
                MobileAppTracker.Instance.SetLatitude(latitude);
                MobileAppTracker.Instance.SetLongitude(longitude);
                MobileAppTracker.Instance.SetAltitude(altitude);
                #endif
            }
        }

        /// <para>
        /// Sets the name of the package.
        /// </para>
        /// <param name="packageName">Package name</param>
        public static void SetPackageName(string packageName)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetPackageName(packageName);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneSetPackageName(packageName);
                #endif
                #if UNITY_METRO
                MobileAppTracker.Instance.SetPackageName(packageName);
                #endif
            }
        }

        /// <para>
        /// Set whether the user is generating revenue for the app or not.
        /// If measureAction is called with a non-zero revenue, this is automatically set to true.
        /// </para>
        /// <param name="isPayingUser">true if the user is revenue-generating, false if not</param>
        public static void SetPayingUser(bool isPayingUser)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetPayingUser(isPayingUser);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneSetPayingUser(isPayingUser);
                #endif
                #if UNITY_METRO
                MobileAppTracker.Instance.SetIsPayingUser(isPayingUser);
                #endif
            }
        }
        
        ///<para>
        ///Sets the custom user phone number.
        ///</para>
        ///<param name="phoneNumber">User's phone number</param>
        public static void SetPhoneNumber(string phoneNumber)
        {
            if(!Application.isEditor)
            {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetPhoneNumber(phoneNumber);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneSetPhoneNumber(phoneNumber);
                #endif
                #if UNITY_METRO
                MobileAppTracker.Instance.SetPhoneNumber(phoneNumber);
                #endif
            }
        }
        
        /// <para>
        /// Sets the user ID to associate with Twitter.
        /// </para>
        /// <param name="twitterUserId">Twitter user ID</param>
        public static void SetTwitterUserId(string twitterUserId)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetTwitterUserId(twitterUserId);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneSetTwitterUserId(twitterUserId);
                #endif
                #if UNITY_METRO
                MobileAppTracker.Instance.SetTwitterUserId(twitterUserId);
                #endif
            }
        }

        /// <para>
        /// Sets the custom user email.
        /// </para>
        /// <param name="userEmail">User's email address</param>
        public static void SetUserEmail(string userEmail)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetUserEmail(userEmail);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneSetUserEmail(userEmail);
                #endif
                #if UNITY_METRO
                MobileAppTracker.Instance.SetUserEmail(userEmail);
                #endif
            }
        }

        /// <para>
        /// Sets the custom user ID.
        /// </para>
        /// <param name="userId">the new user ID</param>
        public static void SetUserId(string userId)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetUserId(userId);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneSetUserId(userId);
                #endif
                #if UNITY_METRO
                MobileAppTracker.Instance.SetUserId(userId);
                #endif
            }
        }

        /// <para>
        /// Sets the custom user name.
        /// </para>
        /// <param name="userName">User name</param>
        public static void SetUserName(string userName)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetUserName(userName);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneSetUserName(userName);
                #endif
                #if UNITY_METRO
                MobileAppTracker.Instance.SetUserName(userName);
                #endif
            }
        }

        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        /*--------------------------------Getters----------------------------------*/
        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////

        /// <para>
        /// Gets whether the user is revenue-generating or not.
        /// </para>
        /// <returns>true if the user has produced revenue, false if not</returns>
        public static bool GetIsPayingUser()
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                return MATAndroid.Instance.GetIsPayingUser();
                #endif
                #if UNITY_IPHONE
                return MATExterns.TuneGetIsPayingUser();
                #endif
                #if UNITY_METRO
                return MobileAppTracker.Instance.GetIsPayingUser();
                #endif
            }
            return true;
        }

        /// <para>
        /// Gets the MAT ID generated on install.
        /// </para>
        /// <returns>MAT ID</returns>
        public static string GetMATId()
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                return MATAndroid.Instance.GetMatId();
                #endif
                #if UNITY_IPHONE
                return MATExterns.TuneGetMatId();
                #endif
                #if UNITY_METRO
                return MobileAppTracker.Instance.GetMatId();
                #endif
            }

            return string.Empty;
        }

        /// <para>
        /// Gets the first MAT open log ID.
        /// </para>
        /// <returns>first MAT open log ID</returns>
        public static string GetOpenLogId()
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                return MATAndroid.Instance.GetOpenLogId();
                #endif
                #if UNITY_IPHONE
                return MATExterns.TuneGetOpenLogId();
                #endif
                #if UNITY_METRO
                return MobileAppTracker.Instance.GetOpenLogId();
                #endif
            }

            return string.Empty;
        }

        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        /*--------------------------iOS Specific Features--------------------------*/
        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////

        /// <para>
        /// Sets the Apple Identifier For Advertising -- IDFA.
        /// Does nothing if not an iOS device.
        /// </para>
        /// <param name="advertiserIdentifier">Apple Identifier For Advertising -- IDFA</param>
        /// <param name="trackingEnabled">
        /// A Boolean value that indicates whether the user has limited ad tracking
        /// </param>
        public static void SetAppleAdvertisingIdentifier(string advertiserIdentifier, bool trackingEnabled)
        {
           if(!Application.isEditor)
           {
               #if UNITY_IPHONE
                MATExterns.TuneSetAppleAdvertisingIdentifier(advertiserIdentifier, trackingEnabled);
               #endif
           }
        }

        /// <para>
        /// Set the Apple Vendor Identifier available in iOS 6.
        /// Does nothing if not an iOS device.
        /// </para>
        /// <param name="vendorIdentifier">Apple Vendor Identifier</param>
        public static void SetAppleVendorIdentifier(string vendorIdentifier)
        {
            if (!Application.isEditor) {
                #if UNITY_IPHONE
                MATExterns.TuneSetAppleVendorIdentifier(vendorIdentifier);
                #endif
            }
        }

        /// <para>
        /// Sets the jailbroken device flag.
        /// Does nothing if not an iOS device.
        /// </para>
        /// <param name="isJailbroken">The jailbroken device flag</param>
        public static void SetJailbroken(bool isJailbroken)
        {
            if (!Application.isEditor) {
                #if UNITY_IPHONE
                MATExterns.TuneSetJailbroken(isJailbroken);
                #endif
            }
        }

        /// <para>
        /// Specifies if the sdk should auto detect if the iOS device is jailbroken.
        /// Does nothing if not an iOS device.
        /// </para>
        /// <param name="isAutoDetectJailbroken">
        /// Will detect if the device is jailbroken if set to true. Defaults to true.
        /// </param>
        public static void SetShouldAutoDetectJailbroken(bool isAutoDetectJailbroken)
        {
            if (!Application.isEditor) {
                #if UNITY_IPHONE
                MATExterns.TuneSetShouldAutoDetectJailbroken(isAutoDetectJailbroken);
                #endif
            }
        }

        /// <para>
        /// Specifies if the sdk should auto-collect the Apple Advertising Identfier (IDFA).
        /// Does nothing if not iOS device.
        /// </para>
        /// <param name="shouldAutoCollect">Will auto-collect if true. Defaults to true.</param>
        public static void SetShouldAutoCollectAppleAdvertisingIdentifier(bool shouldAutoCollect)
        {
            if (!Application.isEditor) {
                #if UNITY_IPHONE
                MATExterns.TuneSetShouldAutoCollectAppleAdvertisingIdentifier(shouldAutoCollect);
                #endif
            }
        }

        /// <para>
        /// Specifies if the sdk should auto-collect the geo location of the device.
        /// Does nothing if not iOS device.
        /// </para>
        /// <param name="shouldAutoCollect">Will auto-collect if true. Defaults to true.</param>
        public static void SetShouldAutoCollectDeviceLocation(bool shouldAutoCollect)
        {
            if (!Application.isEditor) {
                #if UNITY_IPHONE
                MATExterns.TuneSetShouldAutoCollectDeviceLocation(shouldAutoCollect);
                #endif
            }
        }

        /// <para>
        /// Specifies if the sdk should pull the Apple Vendor Identifier from the device.
        /// Does nothing if not iOS device.
        /// </para>
        /// <param name="shouldAutoGenerate">True if yes, false if no.</param>
        public static void SetShouldAutoGenerateVendorIdentifier(bool shouldAutoGenerate)
        {
            if (!Application.isEditor) {
                #if UNITY_IPHONE
                MATExterns.TuneSetShouldAutoGenerateAppleVendorIdentifier(shouldAutoGenerate);
                #endif
            }
        }

        /// <para>
        /// Sets the use cookie tracking. Not used by default.
        /// Does nothing if not an iOS device.
        /// </para>
        /// <param name="useCookieTracking">True if yes, false if no.</param>
        public static void SetUseCookieTracking(bool useCookieTracking)
        {
            if (!Application.isEditor) {
                #if UNITY_IPHONE
                MATExterns.TuneSetUseCookieTracking(useCookieTracking);
                #endif
            }
        }

        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        /*-----------------------Android Specific Features-------------------------*/
        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////

        /// <para>
        /// Sets the ANDROID_ID.
        /// Does nothing if not an Android device.
        /// </para>
        /// <param name="androidId">Device ANDROID_ID</param>
        public static void SetAndroidId(string androidId)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetAndroidId(androidId);
                #endif
            }
        }

        /// <para>
        /// Sets the ANDROID_ID MD5 hash.
        /// Does nothing if not an Android device.
        /// </para>
        /// <param name="androidIdMd5">Device ANDROID_ID MD5 hash</param>
        public static void SetAndroidIdMd5(string androidIdMd5)
        {
            if(!Application.isEditor)
            {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetAndroidIdMd5(androidIdMd5);
                #endif
            }
        }

        /// <para>
        /// Sets the ANDROID_ID SHA-1 hash.
        /// Does nothing if not an Android device.
        /// </para>
        /// <param name="androidIdSha1">Device ANDROID_ID SHA-1 hash</param>
        public static void SetAndroidIdSha1(string androidIdSha1)
        {
            if(!Application.isEditor)
            {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetAndroidIdSha1(androidIdSha1);
                #endif
            }
        }

        /// <para>
        /// Sets the ANDROID_ID SHA-256 hash.
        /// Does nothing if not an Android device.
        /// </para>
        /// <param name="androidIdSha256">Device ANDROID_ID SHA-256 hash</param>
        public static void SetAndroidIdSha256(string androidIdSha256)
        {
            if(!Application.isEditor)
            {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetAndroidIdSha256(androidIdSha256);
                #endif
            }
        }

        /// <para>
        /// Sets the device IMEI/MEID.
        /// Does nothing if not an Android device.
        /// </para>
        /// <param name="deviceId">Device IMEI/MEID</param>
        public static void SetDeviceId(string deviceId)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetDeviceId(deviceId);
                #endif
            }
        }

        /// <para>
        /// Enables or disables primary Gmail address collection.
        /// Requires GET_ACCOUNTS permission.
        /// Does nothing if not an Android device.
        /// </para>
        /// <param name="collectEmail">Whether to collect device email address</param>
        public static void SetEmailCollection(bool collectEmail)
        {
            if(!Application.isEditor)
            {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetEmailCollection(collectEmail);
                #endif
            }
        }

        /// <para>
        /// Sets the device MAC address.
        /// Does nothing if not an Android device.
        /// </para>
        /// <param name="macAddress">Device MAC address</param>
        public static void SetMacAddress(string macAddress)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetMacAddress(macAddress);
                #endif
            }
        }

        /// <para>
        /// Sets the Google Play Services Advertising ID.
        /// Does nothing if not an Android device.
        /// </para>
        /// <param name="adId">Google Play advertising ID</param>
        /// <param name="isLATEnabled">
        /// whether user has requested to limit use of the Google ad ID
        /// </param>
        public static void SetGoogleAdvertisingId(string adId, bool isLATEnabled)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetGoogleAdvertisingId(adId, isLATEnabled);
                #endif
            }
        }

        /// <para>
        /// Sets the preloaded app attribution values (publisher information).
        /// Does nothing if not an Android or iOS device.
        /// </para>
        /// <param name="preloadedData">Preloaded app attribution values (publisher information)</param>
        public static void SetPreloadedApp(MATPreloadData preloadData)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetPreloadedApp(preloadData);
                #endif

                #if UNITY_IPHONE
                MATExterns.TuneSetPreloadData(preloadData);
                #endif
            }
        }

        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        /*---------------------Windows Specific Features-------------------*/
        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        #if UNITY_METRO
        /// <para>
        /// Sets the MAT response.
        /// Does nothing if not a Windows device.
        /// </para>
        /// <param name="matResponse">MAT response</param>
        public static void SetMATResponse(MATResponse matResponse)
        {
            if (!Application.isEditor) {
                MobileAppTracker.Instance.SetMATResponse(matResponse);
            }
        }
        #endif

        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        /*----------------Android and iOS Platform-specific Features---------------*/
        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////

        /// <para>
        /// Sets the ISO 4217 currency code.
        /// Does nothing if not Android or iOS.
        /// </para>
        /// <param name="currencyCode">the currency code</param>
        public static void SetCurrencyCode(string currencyCode)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetCurrencyCode(currencyCode);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneSetCurrencyCode(currencyCode);
                #endif
            }
        }

        /// <para>
        /// Sets delegate used by Tune to post success and failure callbacks from the Tune SDK.
        /// Does nothing if not an Android or iOS device.
        /// </para>
        /// <param name="enable">If set to true enable delegate.</param>
        public static void SetDelegate(bool enable)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetDelegate(enable);
                #endif
                #if (UNITY_IPHONE)
                MATExterns.TuneSetDelegate(enable);
                #endif

            }
        }

        /// <para>
        /// Sets the MAT site ID to specify which app to attribute to.
        /// Does nothing if not Android or iOS device.
        /// </para>
        /// <param name="siteId">MAT site ID to attribute to</param>
        /// [Obsolete("SetSiteId is deprecated. Please use SetPackageName instead.")]
        public static void SetSiteId(string siteId)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetSiteId(siteId);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneSetSiteId(siteId);
                #endif
            }
        }

        /// <para>
        /// Sets the TRUSTe ID, should generate via their SDK.
        /// Does nothing if not Android or iOS device.
        /// </para>
        /// <param name="tpid">TRUSTe ID</param>
        public static void SetTRUSTeId(string tpid)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                MATAndroid.Instance.SetTRUSTeId(tpid);
                #endif
                #if UNITY_IPHONE
                MATExterns.TuneSetTRUSTeId(tpid);
                #endif
            }
        }

        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        /*---------------------End of Platform-specific Features-------------------*/
        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
    }
}
