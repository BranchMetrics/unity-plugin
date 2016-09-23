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

namespace TuneSDK
{
    /// <para>
    /// Tune.cs wraps Android, iOS, and Windows TUNE SDK
    /// functions to be used within Unity. The functions can be called within any
    /// C# script by typing Tune.*.
    /// </para>
    public class Tune : MonoBehaviour
    {
        /// <para>
        /// Initializes relevant information about the advertiser and
        /// conversion key on startup of Unity game.
        /// </para>
        /// <param name="advertiserId">the TUNE advertiser ID for your app</param>
        /// <param name="conversionKey">the TUNE conversion key for your app</param>
        public static void Init(string advertiserId, string conversionKey)
        {
            if(!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.Init(advertiserId, conversionKey);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneInit(advertiserId, conversionKey);
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
        /// <param name="advertiserId">the TUNE advertiser ID for your app</param>
        /// <param name="conversionKey">the TUNE conversion key for your app</param>
        /// <param name="turnOnTMA">enables In-App Marketing for Android</param>
        public static void Init(string advertiserId, string conversionKey, bool turnOnTMA)
        {
            if(!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.Init(advertiserId, conversionKey, turnOnTMA);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneInit(advertiserId, conversionKey);
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
        /// <param name="advertiserId">the TUNE advertiser ID for your app</param>
        /// <param name="conversionKey">the TUNE conversion key for your app</param>
        /// <param name="packageName">the package name used when setting up the app in TUNE</param>
        /// <param name="wearable">should be set to YES when being initialized in a WatchKit extension, defaults to NO</param>
        public static void Init(string advertiserId, string conversionKey, string packageName, bool wearable)
        {
            if(!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.Init(advertiserId, conversionKey);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneInitForWearable(advertiserId, conversionKey, packageName, wearable);
                #endif
                #if UNITY_METRO
                MobileAppTracker.InitializeValues(advertiserId, conversionKey);
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
                TuneAndroid.Instance.CheckForDeferredDeeplink();
                #endif
                #if UNITY_IOS
                TuneExterns.TuneCheckForDeferredDeeplink();
                #endif
                #if UNITY_METRO
                //MATWinStore.MobileAppTracker.Instance.CheckForDeferredDeeplink();
                #endif
            }
        }

        /// <para>
        /// Enable auto-measurement of successful in-app-purchase (IAP) transactions as "purchase" events in iOS.
        /// </para>
        /// <param name="automate">should auto-measure IAP transactions</param>
        public static void AutomateIapEventMeasurement(bool automate)
        {
            if(!Application.isEditor)
            {
                #if UNITY_IOS
                TuneExterns.TuneAutomateIapEventMeasurement(automate);
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
                TuneAndroid.Instance.SetDeepLink(deepLinkUrl);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetDeepLink(deepLinkUrl);
                #endif
            }
        }

        /// <para>
        /// Event measurement function, by event name.
        /// </para>
        /// <param name="eventName">event name in TUNE system</param>
        public static void MeasureEvent(string eventName)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.MeasureEvent(eventName);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneMeasureEventName(eventName);
                #endif
                #if UNITY_METRO
                MobileAppTracker.Instance.MeasureAction(eventName);
                #endif
            }
        }

        /// <para>
        /// Measures the event with TuneEvent.
        /// </para>
        /// <param name="tuneEvent">TuneEvent object of event values to measure</param>
        public static void MeasureEvent(TuneEvent tuneEvent)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.MeasureEvent(tuneEvent);
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

                #if UNITY_IOS
                int itemCount = null == tuneEvent.eventItems ? 0 : tuneEvent.eventItems.Length;
                TuneEventIos eventIos = new TuneEventIos(tuneEvent);

                byte[] receiptBytes = null == tuneEvent.receipt ? null : System.Convert.FromBase64String(tuneEvent.receipt);
                int receiptByteCount = null == receiptBytes ? 0 : receiptBytes.Length;

                // Convert TuneItem to C-marshallable struct TuneItemIos
                TuneItemIos[] items = new TuneItemIos[itemCount];
                for (int i = 0; i < itemCount; i++)
                {
                    items[i] = new TuneItemIos(tuneEvent.eventItems[i]);
                }

                TuneExterns.TuneMeasureEvent(eventIos, items, itemCount, receiptBytes, receiptByteCount);
                #endif

                #if UNITY_METRO
                int itemCount = null == tuneEvent.eventItems ? 0 : tuneEvent.eventItems.Length;
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
                TuneAndroid.Instance.MeasureSession();
                #endif
                #if UNITY_IOS
                TuneExterns.TuneMeasureSession();
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
        /// <param name="age">User age to track in TUNE</param>
        public static void SetAge(int age)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                TuneAndroid.Instance.SetAge(age);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetAge(age);
                #endif
                #if UNITY_METRO
                MobileAppTracker.Instance.SetAge(age);
                #endif
            }
        }

        /// <para>Sets whether app-level ad tracking is enabled.</para>
        /// <param name="adTrackingEnabled">true if user has opted out of ad tracking at the app-level, false if not</param>
        public static void SetAppAdTracking(bool adTrackingEnabled)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                TuneAndroid.Instance.SetAppAdTracking(adTrackingEnabled);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetAppAdTracking(adTrackingEnabled);
                #endif
                #if UNITY_METRO
                MobileAppTracker.Instance.SetAppAdTracking(adTrackingEnabled);
                #endif
            }
        }

        /// <para>
        /// Turns debug mode on or off.
        /// </para>
        /// <param name="debug">whether to enable debug output</param>
        public static void SetDebugMode(bool debug)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                TuneAndroid.Instance.SetDebugMode(debug);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetDebugMode(debug);
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
                TuneAndroid.Instance.SetExistingUser(isExistingUser);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetExistingUser(isExistingUser);
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
                TuneAndroid.Instance.SetFacebookEventLogging(enable, limit);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetFacebookEventLogging(enable, limit);
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
                TuneAndroid.Instance.SetFacebookUserId(fbUserId);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetFacebookUserId(fbUserId);
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
                TuneAndroid.Instance.SetGender(gender);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetGender(gender);
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
                TuneAndroid.Instance.SetGoogleUserId(googleUserId);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetGoogleUserId(googleUserId);
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
                TuneAndroid.Instance.SetLocation(latitude, longitude, altitude);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetLocation(latitude, longitude, altitude);
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
                TuneAndroid.Instance.SetPackageName(packageName);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetPackageName(packageName);
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
                TuneAndroid.Instance.SetPayingUser(isPayingUser);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetPayingUser(isPayingUser);
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
                TuneAndroid.Instance.SetPhoneNumber(phoneNumber);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetPhoneNumber(phoneNumber);
                #endif
                #if UNITY_METRO
                MobileAppTracker.Instance.SetPhoneNumber(phoneNumber);
                #endif
            }
        }

        /// <para>
        /// Specifies if the sdk should auto-collect the geo location of the device.
        /// </para>
        /// <param name="shouldAutoCollect">Will auto-collect if true. Defaults to true.</param>
        public static void SetShouldAutoCollectDeviceLocation(bool shouldAutoCollect)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                TuneAndroid.Instance.SetShouldAutoCollectDeviceLocation(shouldAutoCollect);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetShouldAutoCollectDeviceLocation(shouldAutoCollect);
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
                TuneAndroid.Instance.SetTwitterUserId(twitterUserId);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetTwitterUserId(twitterUserId);
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
                TuneAndroid.Instance.SetUserEmail(userEmail);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetUserEmail(userEmail);
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
                TuneAndroid.Instance.SetUserId(userId);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetUserId(userId);
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
                TuneAndroid.Instance.SetUserName(userName);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetUserName(userName);
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
                return TuneAndroid.Instance.GetIsPayingUser();
                #endif
                #if UNITY_IOS
                return TuneExterns.TuneGetIsPayingUser();
                #endif
                #if UNITY_METRO
                return MobileAppTracker.Instance.GetIsPayingUser();
                #endif
            }
            return false;
        }

        /// <para>
        /// Gets the MAT ID generated on install.
        /// </para>
        /// <returns>MAT ID</returns>
        /// [Obsolete("GetMATId is deprecated. Please use GetTuneId instead.")]
        public static string GetMATId()
        {
            return GetTuneId();
        }

        /// <para>
        /// Gets the TUNE ID generated on install.
        /// </para>
        /// <returns>TUNE ID</returns>
        public static string GetTuneId()
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                return TuneAndroid.Instance.GetMatId();
                #endif
                #if UNITY_IOS
                return TuneExterns.TuneGetTuneId();
                #endif
                #if UNITY_METRO
                return MobileAppTracker.Instance.GetMatId();
                #endif
            }

            return string.Empty;
        }

        /// <para>
        /// Gets the first TUNE open log ID.
        /// </para>
        /// <returns>first TUNE open log ID</returns>
        public static string GetOpenLogId()
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                return TuneAndroid.Instance.GetOpenLogId();
                #endif
                #if UNITY_IOS
                return TuneExterns.TuneGetOpenLogId();
                #endif
                #if UNITY_METRO
                return MobileAppTracker.Instance.GetOpenLogId();
                #endif
            }

            return string.Empty;
        }

        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        /*-----------------------------In-App Marketing----------------------------*/
        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Registers a custom profile string.
        /// </summary>
        /// <param name="name="variableName">Variable name.</param>">
        public static void RegisterCustomProfileString(string variableName)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.RegisterCustomProfileString(variableName);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneRegisterCustomProfileString(variableName);
                #endif
            }
        }

        /// <summary>
        /// Registers the custom profile string.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        /// <param name="defaultValue">Default value.</param>
        public static void RegisterCustomProfileString(string variableName, string defaultValue)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.RegisterCustomProfileString(variableName, defaultValue);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneRegisterCustomProfileStringWithDefault(variableName, defaultValue);
                #endif
            }
        }

        /// <summary>
        /// Registers the custom profile date.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        public static void RegisterCustomProfileDate(string variableName)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.RegisterCustomProfileDate(variableName);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneRegisterCustomProfileDate(variableName);
                #endif
            }
        }

        /// <summary>
        /// Registers the custom profile date.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        /// <param name="defaultValue">Default value.</param>
        public static void RegisterCustomProfileDate(string variableName, DateTime defaultValue)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.RegisterCustomProfileDate(variableName, defaultValue);
                #endif
                #if UNITY_IOS
                // Convert DateTime to string to pass to bridge
                string dateTimeStr = GetDateTimeString(defaultValue);
                TuneExterns.TuneRegisterCustomProfileDateWithDefault(variableName, dateTimeStr);
                #endif
            }
        }

        /// <summary>
        /// Registers the custom profile number.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        public static void RegisterCustomProfileNumber(string variableName)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.RegisterCustomProfileNumber(variableName);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneRegisterCustomProfileNumber(variableName);
                #endif
            }
        }

        /// <summary>
        /// Registers the custom profile number.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        /// <param name="defaultValue">Default value.</param>
        public static void RegisterCustomProfileNumber(string variableName, int defaultValue)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.RegisterCustomProfileNumber(variableName, defaultValue);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneRegisterCustomProfileNumberWithDefaultInt(variableName, defaultValue);
                #endif
            }
        }

        /// <summary>
        /// Registers the custom profile number.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        /// <param name="defaultValue">Default value.</param>
        public static void RegisterCustomProfileNumber(string variableName, double defaultValue)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.RegisterCustomProfileNumber(variableName, defaultValue);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneRegisterCustomProfileNumberWithDefaultDouble(variableName, defaultValue);
                #endif
            }
        }

        /// <summary>
        /// Registers the custom profile number.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        /// <param name="defaultValue">Default value.</param>
        public static void RegisterCustomProfileNumber(string variableName, float defaultValue)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.RegisterCustomProfileNumber(variableName, defaultValue);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneRegisterCustomProfileNumberWithDefaultFloat(variableName, defaultValue);
                #endif
            }
        }

        /// <summary>
        /// Registers the custom profile geo location.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        public static void RegisterCustomProfileGeoLocation(string variableName)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.RegisterCustomProfileGeoLocation(variableName);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneRegisterCustomProfileGeolocation(variableName);
                #endif
            }
        }

        /// <summary>
        /// Registers the custom profile geo location.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        /// <param name="defaultValue">Default value.</param>
        public static void RegisterCustomProfileGeoLocation(string variableName, TuneLocation defaultValue)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.RegisterCustomProfileGeoLocation(variableName, defaultValue);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneRegisterCustomProfileGeolocationWithDefault(variableName, defaultValue.longitude, defaultValue.latitude);
                #endif
            }
        }

        /// <summary>
        /// Sets the custom profile string.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        /// <param name="value">Value.</param>
        public static void SetCustomProfileString(string variableName, string value)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.SetCustomProfileString(variableName, value);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetCustomProfileString(variableName, value);
                #endif
            }
        }

        /// <summary>
        /// Sets the custom profile date.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        /// <param name="value">Value.</param>
        public static void SetCustomProfileDate(string variableName, DateTime value)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.SetCustomProfileDate(variableName, value);
                #endif
                #if UNITY_IOS
                // Convert DateTime to string to pass to bridge
                string dateTimeStr = GetDateTimeString(value);
                TuneExterns.TuneSetCustomProfileDate(variableName, dateTimeStr);
                #endif
            }
        }

        /// <summary>
        /// Sets the custom profile number.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        /// <param name="value">Value.</param>
        public static void SetCustomProfileNumber(string variableName, int value)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.SetCustomProfileNumber(variableName, value);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetCustomProfileNumberWithInt(variableName, value);
                #endif
            }
        }

        /// <summary>
        /// Sets the custom profile number.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        /// <param name="value">Value.</param>
        public static void SetCustomProfileNumber(string variableName, double value)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.SetCustomProfileNumber(variableName, value);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetCustomProfileNumberWithDouble(variableName, value);
                #endif
            }
        }

        /// <summary>
        /// Sets the custom profile number.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        /// <param name="value">Value.</param>
        public static void SetCustomProfileNumber(string variableName, float value)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.SetCustomProfileNumber(variableName, value);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetCustomProfileNumberWithFloat(variableName, value);
                #endif
            }
        }

        /// <summary>
        /// Sets the custom profile geolocation.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        /// <param name="value">Value.</param>
        public static void SetCustomProfileGeolocation(string variableName, TuneLocation value)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.SetCustomProfileGeolocation(variableName, value);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetCustomProfileGeolocation(variableName, value.longitude, value.latitude);
                #endif
            }
        }

        /// <summary>
        /// Gets the custom profile string.
        /// </summary>
        /// <returns>The custom profile string.</returns>
        /// <param name="variableName">Variable name.</param>
        public static string GetCustomProfileString(string variableName)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                return TuneAndroid.Instance.GetCustomProfileString(variableName);
                #endif
                #if UNITY_IOS
                return TuneExterns.TuneGetCustomProfileString(variableName);
                #endif
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the custom profile date.
        /// </summary>
        /// <returns>The custom profile date.</returns>
        /// <param name="variableName">Variable name.</param>
        public static DateTime GetCustomProfileDate(string variableName)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                return TuneAndroid.Instance.GetCustomProfileDate(variableName);
                #endif
                #if UNITY_IOS
                return GetDateTimeFromString(TuneExterns.TuneGetCustomProfileDate(variableName));
                #endif
            }
            return new DateTime();
        }

        /// <summary>
        /// Gets the custom profile number.
        /// </summary>
        /// <returns>The custom profile number.</returns>
        /// <param name="variableName">Variable name.</param>
        public static double GetCustomProfileNumber(string variableName)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                return TuneAndroid.Instance.GetCustomProfileNumber(variableName);
                #endif
                #if UNITY_IOS
                return Convert.ToDouble(TuneExterns.TuneGetCustomProfileNumber(variableName));
                #endif
            }
            return -1;
        }

        /// <summary>
        /// Gets the custom profile geolocation.
        /// </summary>
        /// <returns>The custom profile geolocation.</returns>
        /// <param name="variableName">Variable name.</param>
        public static TuneLocation GetCustomProfileGeolocation(string variableName)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                return TuneAndroid.Instance.GetCustomProfileGeolocation(variableName);
                #endif
                #if UNITY_IOS
                // Parse string as "latitude,longitude"
                string[] locationArray = TuneExterns.TuneGetCustomProfileGeolocation(variableName).Split(',');
                if (locationArray.Length == 2)
                {
                    TuneLocation location = new TuneLocation();
                    location.latitude = Double.Parse(locationArray[0]);
                    location.longitude = Double.Parse(locationArray[1]);
                    return location;
                }
                return null;
                #endif
            }
            return null;
        }

        /// <summary>
        /// Clears the custom profile variable.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        public static void ClearCustomProfileVariable(string variableName)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.ClearCustomProfileVariable(variableName);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneClearCustomProfileVariable(variableName);
                #endif
            }
        }

        /// <summary>
        /// Clears all custom profile variables.
        /// </summary>
        public static void ClearAllCustomProfileVariables()
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.ClearAllCustomProfileVariables();
                #endif
                #if UNITY_IOS
                TuneExterns.TuneClearAllCustomProfileVariables();
                #endif
            }
        }

        /// <summary>
        /// Registers a power hook.
        /// </summary>
        /// <param name="hookId">Hook identifier.</param>
        /// <param name="friendlyName">Friendly name.</param>
        /// <param name="defaultValue">Default value.</param>
        public static void RegisterPowerHook(string hookId, string friendlyName, string defaultValue)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.RegisterPowerHook(hookId, friendlyName, defaultValue);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneRegisterHookWithId(hookId, friendlyName, defaultValue);
                #endif
            }
        }

        /// <summary>
        /// Gets the value for a power hook by identifier.
        /// </summary>
        /// <returns>The value for hook by identifier.</returns>
        /// <param name="hookId">Hook identifier.</param>
        public static string GetValueForHookById(string hookId)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                return TuneAndroid.Instance.GetValueForHookById(hookId);
                #endif
                #if UNITY_IOS
                return TuneExterns.TuneGetValueForHookById(hookId);
                #endif
            }
            return string.Empty;
        }

        /// <summary>
        /// Sets the value for a power hook.
        /// </summary>
        /// <param name="hookId">Hook identifier.</param>
        /// <param name="value">Value.</param>
        public static void SetValueForHookById(string hookId, string value)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.SetValueForHookById(hookId, value);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetValueForHookById(hookId, value);
                #endif
            }
        }

        /// <summary>
        /// Enables listener for the power hooks changed event.
        /// </summary>
        /// <param name="listenForPowerHooksChanged">If set to <c>true</c> listen for power hooks changed.</param>
        public static void OnPowerHooksChanged(bool listenForPowerHooksChanged)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.OnPowerHooksChanged(listenForPowerHooksChanged);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneOnPowerHooksChanged(listenForPowerHooksChanged);
                #endif
            }
        }

        /// <summary>
        /// Gets the power hook experiment details.
        /// </summary>
        /// <returns>The power hook experiment details.</returns>
        public static Dictionary<string, TunePowerHookExperimentDetails> GetPowerHookExperimentDetails()
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                return TuneAndroid.Instance.GetPowerHookExperimentDetails();
                #endif
                #if UNITY_IOS
                string dictStr = TuneExterns.TuneGetPowerHookVariableExperimentDetails();
                TuneSerializablePowerHookDictionaryIos dict = JsonUtility.FromJson<TuneSerializablePowerHookDictionaryIos>(dictStr);
                Dictionary<string, TunePowerHookExperimentDetails> outputDict = new Dictionary<string, TunePowerHookExperimentDetails>();

                // Convert TuneSerializablePowerHookDictionaryIos to Dictionary<string, TunePowerHookExperimentDetails>
                if (dict != null)
                {
                    if (dict.keys.Length != dict.values.Length)
                    {
                        throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization.", dict.keys.Length, dict.values.Length));
                    }

                    // Iterate through keys and construct Dictionary with corresponding values
                    for (int i = 0; i < dict.keys.Length; i++)
                    {
                        // Construct C# TunePowerHookExperimentDetails from each TunePowerHookExperimentDetailsIos
                        TunePowerHookExperimentDetails detail = new TunePowerHookExperimentDetails(dict.values[i]);
                        outputDict.Add(dict.keys[i], detail);
                    }
                }
                return outputDict;
                #endif
            }
            return null;
        }

        /// <summary>
        /// Gets the in app message experiment details.
        /// </summary>
        /// <returns>The in app message experiment details.</returns>
        public static Dictionary<string, TuneInAppMessageExperimentDetails> GetInAppMessageExperimentDetails()
        {
            if (!Application.isEditor)
            {
                #if UNITY_IOS
                string dictStr = TuneExterns.TuneGetInAppMessageExperimentDetails();
                TuneSerializableInAppMessageDictionaryIos dict = JsonUtility.FromJson<TuneSerializableInAppMessageDictionaryIos>(dictStr);
                Dictionary<string, TuneInAppMessageExperimentDetails> outputDict = new Dictionary<string, TuneInAppMessageExperimentDetails>();

                // Convert TuneSerializableInAppMessageDictionaryIos to Dictionary<string, TuneInAppMessageExperimentDetails>
                if (dict != null)
                {
                    if (dict.keys.Length != dict.values.Length)
                    {
                        throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization.", dict.keys.Length, dict.values.Length));
                    }

                    // Iterate through keys and construct Dictionary with corresponding values
                    for (int i = 0; i < dict.keys.Length; i++)
                    {
                        // Construct C# TuneInAppMessageExperimentDetails from each TuneInAppMessageExperimentDetailsIos
                        TuneInAppMessageExperimentDetails detail = new TuneInAppMessageExperimentDetails(dict.values[i]);
                        outputDict.Add(dict.keys[i], detail);
                    }
                }
                return outputDict;
                #endif
            }
            return null;
        }

        /// <summary>
        /// Enables listener for the first playlist downloaded event.
        /// </summary>
        /// <param name="listenForFirstPlaylist">If set to <c>true</c> listen for first playlist.</param>
        public static void OnFirstPlaylistDownloaded(bool listenForFirstPlaylist)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.OnFirstPlaylistDownloaded(listenForFirstPlaylist);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneOnFirstPlaylistDownloaded(listenForFirstPlaylist);
                #endif
            }
        }

        /// <summary>
        /// Enables listener for the first playlist downloaded event.
        /// </summary>
        /// <param name="listenForFirstPlaylist">If set to <c>true</c> listen for first playlist.</param>
        /// <param name="timeout">Timeout.</param>
        public static void OnFirstPlaylistDownloaded(bool listenForFirstPlaylist, long timeout)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.OnFirstPlaylistDownloaded(listenForFirstPlaylist, timeout);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneOnFirstPlaylistDownloadedWithTimeout(listenForFirstPlaylist, timeout);
                #endif
            }
        }

        /// <summary>
        /// Sets the push notification sender identifier.
        /// </summary>
        /// <param name="pushSenderId">Push sender identifier.</param>
        public static void SetPushNotificationSenderId(string pushSenderId)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.SetPushNotificationSenderId(pushSenderId);
                #endif
            }
        }

        /// <summary>
        /// Sets the push notification registration identifier.
        /// </summary>
        /// <param name="registrationId">Registration identifier.</param>
        public static void SetPushNotificationRegistrationId(string registrationId)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.SetPushNotificationRegistrationId(registrationId);
                #endif
            }
        }

        /// <summary>
        /// Sets whether user opted out of push.
        /// </summary>
        /// <param name="optedOutOfPush">If set to <c>true</c> opted out of push.</param>
        public static void SetOptedOutOfPush(bool optedOutOfPush)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.SetOptedOutOfPush(optedOutOfPush);
                #endif
            }
        }

        /// <summary>
        /// Gets the push registration device token.
        /// </summary>
        /// <returns>The device token.</returns>
        public static string GetDeviceToken()
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                return TuneAndroid.Instance.GetDeviceToken();
                #endif
            }
            return string.Empty;
        }

        /// <summary>
        /// Returns whether the user manually disabled push for the app at the Android system level.
        /// </summary>
        /// <returns><c>true</c>, if user manually disabled push, <c>false</c> otherwise.</returns>
        public static bool DidUserManuallyDisablePush()
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                return TuneAndroid.Instance.DidUserManuallyDisablePush();
                #endif
            }
            return false;
        }

        /// <summary>
        /// Returns whether the current session was caused by a TUNE push notification.
        /// </summary>
        /// <returns><c>true</c>, if session was opened via TUNE push notification, <c>false</c> otherwise.</returns>
        public static bool DidSessionStartFromTunePush()
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                return TuneAndroid.Instance.DidSessionStartFromTunePush();
                #endif
                #if UNITY_IOS
                return TuneExterns.TuneDidSessionStartFromTunePush();
                #endif
            }
            return false;
        }

        /// <summary>
        /// Returns TUNE push information if session was opened by a TUNE push notification.
        /// </summary>
        /// <returns>TunePushInfo object of push information. On iOS, the extrasPayload field will be empty.</returns>
        public static TunePushInfo GetTunePushInfoForSession()
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                return TuneAndroid.Instance.GetTunePushInfoForSession();
                #endif
                #if UNITY_IOS
                string campaignId = TuneExterns.TuneGetTuneCampaignIdForSession();
                string pushId = TuneExterns.TuneGetTunePushIdForSession();
                // TODO: convert Objective-C NSDictionary to C# Dictionary for extrasPayload
                return new TunePushInfo(campaignId, pushId, new Dictionary<string, string>());
                #endif
            }
            return null;
        }

        /// <summary>
        /// Returns whether the user belongs to the given segment.
        /// </summary>
        /// <param name="segmentId">Segment ID to check for a match.</param>
        /// <returns>Whether the user is in the given segment.</returns>
        public static bool IsUserInSegmentId(string segmentId)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                return TuneAndroid.Instance.IsUserInSegmentId(segmentId);
                #endif
                #if UNITY_IOS
                return TuneExterns.TuneIsUserInSegmentId(segmentId);
                #endif
            }
            return false;
        }

        /// <summary>
        /// Returns whether the user belongs to any of the given segments.
        /// </summary>
        /// <param name="segmentId">Segment IDs to check for a match.</param>
        /// <returns>Whether the user is in any of the given segments.</returns>
        public static bool IsUserInAnySegmentIds(string[] segmentIds)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                return TuneAndroid.Instance.IsUserInAnySegmentIds(segmentIds);
                #endif
                #if UNITY_IOS
                return TuneExterns.TuneIsUserInAnySegmentIds(segmentIds, segmentIds.Length);
                #endif
            }
            return false;
        }

        /// <summary>
        /// Sets the status of the user in the given segment, for testing IsUserInSegmentId or IsUserInAnySegmentIds.
        /// Only affects segment status locally, for testing. Does not update segments server-side.
        /// This call should be removed prior to shipping to production.
        /// </summary>
        /// <param name="segmentId">Segment to modify status for.</param>
        /// <param name="isInSegment">Status to modify of whether user is in the segment.</param>
        public static void ForceSetUserInSegmentId(string segmentId, bool isInSegment)
        {
            if (!Application.isEditor)
            {
                #if UNITY_ANDROID
                TuneAndroid.Instance.ForceSetUserInSegmentId(segmentId, isInSegment);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneForceSetUserInSegmentId(segmentId, isInSegment);
                #endif
            }
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
               #if UNITY_IOS
                TuneExterns.TuneSetAppleAdvertisingIdentifier(advertiserIdentifier, trackingEnabled);
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
                #if UNITY_IOS
                TuneExterns.TuneSetAppleVendorIdentifier(vendorIdentifier);
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
                #if UNITY_IOS
                TuneExterns.TuneSetJailbroken(isJailbroken);
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
                #if UNITY_IOS
                TuneExterns.TuneSetShouldAutoDetectJailbroken(isAutoDetectJailbroken);
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
                #if UNITY_IOS
                TuneExterns.TuneSetShouldAutoCollectAppleAdvertisingIdentifier(shouldAutoCollect);
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
                #if UNITY_IOS
                TuneExterns.TuneSetShouldAutoGenerateAppleVendorIdentifier(shouldAutoGenerate);
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
                #if UNITY_IOS
                TuneExterns.TuneSetUseCookieTracking(useCookieTracking);
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
                TuneAndroid.Instance.SetAndroidId(androidId);
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
                TuneAndroid.Instance.SetAndroidIdMd5(androidIdMd5);
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
                TuneAndroid.Instance.SetAndroidIdSha1(androidIdSha1);
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
                TuneAndroid.Instance.SetAndroidIdSha256(androidIdSha256);
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
                TuneAndroid.Instance.SetDeviceId(deviceId);
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
                TuneAndroid.Instance.SetEmailCollection(collectEmail);
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
                TuneAndroid.Instance.SetMacAddress(macAddress);
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
                TuneAndroid.Instance.SetGoogleAdvertisingId(adId, isLATEnabled);
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
                TuneAndroid.Instance.SetCurrencyCode(currencyCode);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetCurrencyCode(currencyCode);
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
                TuneAndroid.Instance.SetDelegate(enable);
                #endif
                #if (UNITY_IOS)
                TuneExterns.TuneSetDelegate(enable);
                #endif

            }
        }

        /// <para>
        /// Sets the preloaded app attribution values (publisher information).
        /// Does nothing if not an Android or iOS device.
        /// </para>
        /// <param name="preloadedData">Preloaded app attribution values (publisher information)</param>
        public static void SetPreloadedApp(TunePreloadData preloadData)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                TuneAndroid.Instance.SetPreloadedApp(preloadData);
                #endif

                #if UNITY_IOS
                TuneExterns.TuneSetPreloadData(preloadData);
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
                TuneAndroid.Instance.SetTRUSTeId(tpid);
                #endif
                #if UNITY_IOS
                TuneExterns.TuneSetTRUSTeId(tpid);
                #endif
            }
        }

        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        /*---------------------End of Platform-specific Features-------------------*/
        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////

        // Helper function to convert DateTime to Unix timestamp for iOS
        private static string GetDateTimeString(DateTime dateTime)
        {
            // datetime starts in 1970
            DateTime datetime = new DateTime(1970, 1, 1);
            double millis = new TimeSpan(dateTime.Ticks).TotalMilliseconds;
            double millisFrom1970 = millis - (new TimeSpan(datetime.Ticks)).TotalMilliseconds;
            return millisFrom1970.ToString();
        }

        // Helper function to convert string to DateTime for iOS
        private static DateTime GetDateTimeFromString(string dateString)
        {
            double unixTimeMillis = Convert.ToDouble(dateString);
            // Add ms from date to ms of 1/1/1970
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(unixTimeMillis);
        }
    }
}
