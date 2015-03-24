using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using com.mobileapptracking;

/// <para>
/// MATBinding wraps Android, iOS, and Windows Phone 8 MobileAppTracking SDK 
/// functions to be used within Unity. The functions can be called within any 
/// C# script by typing MATBinding.*.
/// </para>
public class MATBinding : MonoBehaviour
{
    private static GameObject mat;

    public void ReceivedGAID(string gaidAndLAT)
    {
        #if UNITY_ANDROID
        char[] delimiters = { ',' };
        string[] splitStr = gaidAndLAT.Split(delimiters);
        string gaid = splitStr[0];
        bool isLAT;
        if (Boolean.TryParse(splitStr[1], out isLAT)) {
            SetGoogleAdvertisingId(gaid, isLAT);
        }
        string deeplink = MATAndroid.Instance.CheckForDeferredDeeplink(750);
        if (mat != null && deeplink.Length != 0) {
            mat.SendMessage("trackerDidReceiveDeepLink", deeplink);
        }
        #endif
    }

    public void ReceivedAndroidID(string androidId)
    {
        #if UNITY_ANDROID
        SetAndroidId(androidId);
        string deeplink = MATAndroid.Instance.CheckForDeferredDeeplink(750);
        if (mat != null && deeplink.Length != 0) {
            mat.SendMessage("trackerDidReceiveDeepLink", deeplink);
        }
        #endif
    }

    /// <para>
    /// Initializes relevant information about the advertiser and 
    /// conversion key on startup of Unity game.
    /// </para>
    /// <param name="advertiserId">the MAT advertiser ID for the app</param>
    /// <param name="conversionKey">the MAT advertiser key for the app</param>
    public static void Init(string advertiserId, string conversionKey)
    {
        mat = GameObject.Find("MobileAppTracker");
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.Init(advertiserId, conversionKey);
            #endif
            #if UNITY_IPHONE
            MATExterns.initNativeCode(advertiserId, conversionKey);
            MATExterns.setAppleAdvertisingIdentifier(UnityEngine.iPhone.advertisingIdentifier, UnityEngine.iPhone.advertisingTrackingEnabled);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.InitializeValues(advertiserId, conversionKey);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.InitializeValues(advertiserId, conversionKey);
            #endif
        }
    }

    /// <para>
    /// Check if a deferred deep-link is available.
    /// </para>
    /// <param name="timeoutMillis">timeout duration in milliseconds</param>
    public static void CheckForDeferredDeeplinkWithTimeout(double timeoutMillis)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            // Android handles check in Advertising Id response callback
            //MATAndroid.Instance.CheckForDeferredDeeplink(timeoutMillis);
            #endif
            #if UNITY_IPHONE
            MATExterns.checkForDeferredDeeplinkWithTimeout(timeoutMillis);
            #endif
            #if UNITY_WP8
            //MATWP8.MobileAppTracker.Instance.CheckForDeferredDeeplinkWithTimeout(timeoutMillis);
            #endif
            #if UNITY_METRO
            //MATWinStore.MobileAppTracker.Instance.CheckForDeferredDeeplinkWithTimeout(timeoutMillis);
            #endif
        }
    }

    /// <para>
    /// Event measurement function, by event ID or name.
    /// </para>
    /// <param name="action">event name or event ID in MAT system</param>
    public static void MeasureAction(string action)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.MeasureAction(action);
            #endif
            #if UNITY_IPHONE
            MATExterns.measureAction(action);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.MeasureAction(action);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.MeasureAction(action);
            #endif
        }
    }

    /// <para>
    /// Measures the action with event items.
    /// </para>
    /// <param name="action">Action</param>
    /// <param name="items">Items</param>
    /// <param name="eventItemCount">Event item count</param>
    /// <param name="revenue">Revenue</param>
    /// <param name="currency">Currency</param>
    /// <param name="refId">Reference identifier</param>
    /// <param name="transactionState">Transaction state</param>
    /// <param name="receipt">Receipt</param>
    /// <param name="receiptSignature">Receipt signature</param>
    public static void MeasureActionWithEventItems(string action, MATItem[] items, int eventItemCount, double revenue, string currency, string refId, int transactionState, string receipt, string receiptSignature)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.MeasureActionWithEventItems(action, items, eventItemCount, revenue, currency, refId, transactionState, receipt, receiptSignature);
            #endif
            #if UNITY_IPHONE
            MATExterns.measureActionWithEventItems(action, items, eventItemCount, refId, revenue, currency, transactionState, receipt, receiptSignature);
            #endif
            
            #if UNITY_WP8
            //Convert MATItem[] to MATEventItem[]. These are the same things, but must be converted for recognition of
            //MobileAppTracker.cs.
            MATWP8.MATEventItem[] newarr = new MATWP8.MATEventItem[items.Length];
            //Conversion is necessary to prevent the need of referencing a separate class.
            for(int i = 0; i < items.Length; i++)
            {
                newarr[i] = new MATWP8.MATEventItem(items[i].name, items[i].quantity, items[i].unitPrice, 
                                                    items[i].revenue, items[i].attribute1, items[i].attribute2,
                                                    items[i].attribute3, items[i].attribute4, items[i].attribute5);
            }
            
            List<MATWP8.MATEventItem> list =  newarr.ToList();
            MATWP8.MobileAppTracker.Instance.MeasureAction(action, revenue, currency, refId, list);
            #endif

            #if UNITY_METRO
            MATWinStore.MATEventItem[] newarr = new MATWinStore.MATEventItem[items.Length];
            for(int i = 0; i < items.Length; i++)
            {
                newarr[i] = new MATWinStore.MATEventItem(items[i].name, items[i].quantity, items[i].unitPrice, 
                                                      items[i].revenue, items[i].attribute1, items[i].attribute2,
                                                      items[i].attribute3, items[i].attribute4, items[i].attribute5);
            }
            
            List<MATWinStore.MATEventItem> list =  newarr.ToList();
            MATWinStore.MobileAppTracker.Instance.MeasureAction(action, revenue, currency, refId, list);
            #endif
        }
    }

    /// <para>
    /// Measures the action with reference ID. All other event items are set to their default values.
    /// </para>
    /// <param name="action">Action</param>
    /// <param name="refId">Reference ID</param>
    public static void MeasureActionWithRefId(string action, string refId)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.MeasureActionWithRefId(action, refId);
            #endif

            #if UNITY_IPHONE
            MATExterns.measureActionWithRefId(action, refId);
            #endif

            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.MeasureAction(action, 0.0, null, refId, null);
            #endif

            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.MeasureAction(action, 0.0, null, refId, null);
            #endif
        }
    }

    /// <para>
    /// Measures the action with revenue. Other event items are set to their default values.
    /// </para>
    /// <param name="action">Action</param>
    /// <param name="revenue">Revenue</param>
    /// <param name="currencyCode">Currency code</param>
    /// <param name="refId">Reference ID</param>
    public static void MeasureActionWithRevenue(string action, double revenue, string currencyCode, string refId)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.MeasureActionWithRevenue(action, revenue, currencyCode, refId);
            #endif

            #if UNITY_IPHONE
            MATExterns.measureActionWithRevenue(action, revenue, currencyCode, refId);
            #endif

            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.MeasureAction(action, revenue, currencyCode, refId, null);
            #endif

            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.MeasureAction(action, revenue, currencyCode, refId, null);
            #endif
        }
    }

    /// <para>
    /// Main session measurement function; this function should be called at every app open.
    /// </para>
    public static void MeasureSession()
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.MeasureSession();
            #endif

            #if UNITY_IPHONE
            MATExterns.measureSession();
            #endif

            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.MeasureSession();
            #endif

            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.MeasureSession();
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
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetAge(age);
            #endif
            #if UNITY_IPHONE
            MATExterns.setAge(age);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetAge(age);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetAge(age);
            #endif
        }
    }

    /// <para>
    /// Enables acceptance of duplicate installs from this device.
    /// </para>
    /// <param name="allow">whether to allow duplicate installs from device</param>
    public static void SetAllowDuplicates(bool allow)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetAllowDuplicates(allow);
            #endif
            #if UNITY_IPHONE
            MATExterns.setAllowDuplicates(allow);
            #endif
            #if UNITY_WP8 
            MATWP8.MobileAppTracker.Instance.SetAllowDuplicates(allow);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetAllowDuplicates(allow);
            #endif
        }
    }

    ///<para>Sets whether app-level ad tracking is enabled.</para>
    ///<param name="adTrackingEnabled">true if user has opted out of ad tracking at the app-level, false if not</param>
    public static void SetAppAdTracking(bool adTrackingEnabled)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetAppAdTracking(adTrackingEnabled);
            #endif
            #if UNITY_IPHONE
            MATExterns.setAppAdTracking(adTrackingEnabled);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetAppAdTracking(adTrackingEnabled);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetAppAdTracking(adTrackingEnabled);
            #endif
        }
    }

    /// <para>
    /// Turns debug mode on or off, under tag "MobileAppTracker".
    /// </para>
    /// <param name="debug">whether to enable debug output</param>
    public static void SetDebugMode(bool debug)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetDebugMode(debug);
            #endif
            #if UNITY_IPHONE
            MATExterns.tuneSetDebugMode(debug);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetDebugMode(debug);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetDebugMode(debug);
            #endif
        }
    }

    /// <para>
    /// Sets the first attribute associated with an app event.
    /// </para>
    /// <param name="eventAttribute">the attribute</param>
    public static void SetEventAttribute1(string eventAttribute)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetEventAttribute1(eventAttribute);
            #endif
            #if UNITY_IPHONE
            MATExterns.setEventAttribute1(eventAttribute);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetEventAttribute1(eventAttribute);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetEventAttribute1(eventAttribute);
            #endif
        }
    }

    /// <para>
    /// Sets the second attribute associated with an app event.
    /// </para>
    /// <param name="eventAttribute">the attribute</param>
    public static void SetEventAttribute2(string eventAttribute)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetEventAttribute2(eventAttribute);
            #endif
            #if UNITY_IPHONE
            MATExterns.setEventAttribute2(eventAttribute);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetEventAttribute2(eventAttribute);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetEventAttribute2(eventAttribute);
            #endif
        }
    }

    /// <para>
    /// Sets the third attribute associated with an app event.
    /// </para>
    /// <param name="eventAttribute">the attribute</param>
    public static void SetEventAttribute3(string eventAttribute)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetEventAttribute3(eventAttribute);
            #endif
            #if UNITY_IPHONE
            MATExterns.setEventAttribute3(eventAttribute);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetEventAttribute3(eventAttribute);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetEventAttribute3(eventAttribute);
            #endif
        }
    }

    /// <para>
    /// Sets the fourth attribute associated with an app event.
    /// </para>
    /// <param name="eventAttribute">the attribute</param>
    public static void SetEventAttribute4(string eventAttribute)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetEventAttribute4(eventAttribute);
            #endif
            #if UNITY_IPHONE
            MATExterns.setEventAttribute4(eventAttribute);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetEventAttribute4(eventAttribute);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetEventAttribute4(eventAttribute);
            #endif
        }
    }

    /// <para>
    /// Sets the fifth attribute associated with an app event.
    /// </para>
    /// <param name="eventAttribute">the attribute</param>
    public static void SetEventAttribute5(string eventAttribute)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetEventAttribute5(eventAttribute);
            #endif
            #if UNITY_IPHONE
            MATExterns.setEventAttribute5(eventAttribute);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetEventAttribute5(eventAttribute);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetEventAttribute5(eventAttribute);
            #endif
        }
    }

    /// <para>
    /// Sets the content ID associated with an app event.
    /// </para>
    /// <param name="eventContentId">the content ID</param>
    public static void SetEventContentId(string eventContentId)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetEventContentId(eventContentId);
            #endif
            #if UNITY_IPHONE
            MATExterns.setEventContentId(eventContentId);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetEventContentId(eventContentId);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetEventContentId(eventContentId);
            #endif
        }
    }

    /// <para>
    /// Sets the content type associated with an app event.
    /// </para>
    /// <param name="eventContentType">the content type</param>
    public static void SetEventContentType(string eventContentType)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetEventContentType(eventContentType);
            #endif
            #if UNITY_IPHONE
            MATExterns.setEventContentType(eventContentType);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetEventContentType(eventContentType);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetEventContentType(eventContentType);
            #endif
        }
    }

    /// <para>
    /// Sets the first date associated with an app event.
    /// Should be 1/1/1970 and after. 
    /// </para>
    /// <param name="eventDate">the date</param>
    public static void SetEventDate1(DateTime eventDate)
    {
        if(!Application.isEditor)
        {
            #if (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8 || UNITY_METRO)
            double milliseconds = new TimeSpan(eventDate.Ticks).TotalMilliseconds;
            //datetime starts in 1970
            DateTime datetime = new DateTime(1970, 1, 1);
            double millisecondsFrom1970 = milliseconds - (new TimeSpan(datetime.Ticks)).TotalMilliseconds;

            #if UNITY_ANDROID
            MATAndroid.Instance.SetEventDate1(millisecondsFrom1970.ToString());
            #endif
            #if UNITY_IPHONE
            MATExterns.setEventDate1(millisecondsFrom1970.ToString());
            #endif
            
            #if (UNITY_WP8 || UNITY_METRO)
            TimeSpan timeFrom1970 = TimeSpan.FromMilliseconds(millisecondsFrom1970);
            //Update to current time for c#
            datetime = datetime.Add(timeFrom1970);

            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetEventDate1(datetime);
            #endif

            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetEventDate1(datetime);
            #endif

            #endif

            #endif
        }
    }

    /// <para>
    /// Sets the second date associated with an app event.
    /// </para>
    /// <param name="eventDate">the date.</param>
    public static void SetEventDate2(DateTime eventDate)
    {
        if(!Application.isEditor)
        {
            #if (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8 || UNITY_METRO)
            double milliseconds = new TimeSpan(eventDate.Ticks).TotalMilliseconds;
            //datetime starts in 1970
            DateTime datetime = new DateTime(1970, 1, 1);
            double millisecondsFrom1970 = milliseconds - (new TimeSpan(datetime.Ticks)).TotalMilliseconds;
            
            #if UNITY_ANDROID
            MATAndroid.Instance.SetEventDate2(millisecondsFrom1970.ToString());
            #endif
            #if UNITY_IPHONE
            MATExterns.setEventDate2(millisecondsFrom1970.ToString());
            #endif

            #if (UNITY_WP8 || UNITY_METRO)
            TimeSpan timeFrom1970 = TimeSpan.FromMilliseconds(millisecondsFrom1970);
            //Update to current time for c#
            datetime = datetime.Add(timeFrom1970);

            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetEventDate2(datetime);
            #endif

            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetEventDate2(datetime);
            #endif

            #endif

            #endif
        }
    }

    /// <para>
    /// Sets the level associated with an app event.
    /// </para>
    /// <param name="eventLevel">the level</param>
    public static void SetEventLevel(int eventLevel)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetEventLevel(eventLevel);
            #endif
            #if UNITY_IPHONE
            MATExterns.setEventLevel(eventLevel);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetEventLevel(eventLevel);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetEventLevel(eventLevel);
            #endif
        }
    }

    /// <para>
    /// Sets the quantity associated with an app event.
    /// </para>
    /// <param name="eventQuantity">the quantity</param>
    public static void SetEventQuantity(int eventQuantity)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetEventQuantity(eventQuantity);
            #endif
            #if UNITY_IPHONE
            MATExterns.setEventQuantity(eventQuantity);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetEventQuantity(eventQuantity);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetEventQuantity(eventQuantity);
            #endif
        }
    }

    /// <para>
    /// Sets the rating associated with an app event.
    /// </para>
    /// <param name="eventRating">the rating</param>
    public static void SetEventRating(float eventRating)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetEventRating(eventRating);
            #endif
            #if UNITY_IPHONE
            MATExterns.setEventRating(eventRating);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetEventRating(eventRating);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetEventRating(eventRating);
            #endif
        }
    }

    /// <para>
    /// Sets the search string associated with an app event.
    /// </para>
    /// <param name="eventSearchString">the search string</param>
    public static void SetEventSearchString(string eventSearchString)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetEventSearchString(eventSearchString);
            #endif
            #if UNITY_IPHONE
            MATExterns.setEventSearchString(eventSearchString);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetEventSearchString(eventSearchString);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetEventSearchString(eventSearchString);
            #endif
        }
    }

    /// <para>
    /// Sets whether app was previously installed prior to version with MAT SDK.
    /// </para>
    /// <param name="isExistingUser">
    /// existing true if this user already had the app installed prior to updating to MAT version
    /// </param>
    public static void SetExistingUser(bool isExistingUser)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetExistingUser(isExistingUser);
            #endif
            #if UNITY_IPHONE
            MATExterns.setExistingUser(isExistingUser);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetExistingUser(isExistingUser);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetExistingUser(isExistingUser);
            #endif
        }
    }

    /// <para>
    /// Set whether the MAT events should also be logged to the Facebook SDK. This flag is ignored if the Facebook SDK is not present.
    /// </para>
    /// <param name="enable">Whether to send MAT events to FB as well</param>
    /// <param name="limitEventAndDataUsabe">Whether data such as that generated through FBAppEvents and sent to Facebook should be restricted from being used for other than analytics and conversions. Defaults to NO. This value is stored on the device and persists across app launches.</param>
    public static void SetFacebookEventLogging(bool enable, bool limit)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            //MATAndroid.Instance.SetFacebookEventLogging(enable);
            #endif
            #if UNITY_IPHONE
            MATExterns.setFacebookEventLogging(enable, limit);
            #endif
            #if UNITY_WP8
            //MATWP8.MobileAppTracker.Instance.SetFacebookEventLogging(enable, limit);
            #endif
            #if UNITY_METRO
            //MATWinStore.MobileAppTracker.Instance.SetFacebookEventLogging(enable, limit);
            #endif
        }
    }

    /// <para>
    /// Sets the user ID to associate with Facebook.
    /// </para>
    /// <param name="fbUserId">Facebook User ID</param>
    public static void SetFacebookUserId(string fbUserId)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetFacebookUserId(fbUserId);
            #endif
            #if UNITY_IPHONE
            MATExterns.setFacebookUserId(fbUserId);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetFacebookUserId(fbUserId);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetFacebookUserId(fbUserId);
            #endif
        }
    }

    /// <para>
    /// Sets the user gender.
    /// </para>
    /// <param name="gender">use MobileAppTracker.GENDER_MALE, MobileAppTracker.GENDER_FEMALE</param>
    public static void SetGender(int gender)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetGender(gender);
            #endif
            #if UNITY_IPHONE
            MATExterns.setGender(gender);
            #endif

            #if UNITY_WP8
            MATWP8.MATGender gender_temp;
            if(gender == 0)
                gender_temp = MATWP8.MATGender.MALE;
            else if (gender == 1)
                gender_temp = MATWP8.MATGender.FEMALE;
            else
                gender_temp = MATWP8.MATGender.NONE;
            
            MATWP8.MobileAppTracker.Instance.SetGender(gender_temp);
            #endif

            #if UNITY_METRO
            MATWinStore.MATGender gender_temp;
            if(gender == 0)
                gender_temp = MATWinStore.MATGender.MALE;
            else if (gender == 1)
                gender_temp = MATWinStore.MATGender.FEMALE;
            else
                gender_temp = MATWinStore.MATGender.NONE;
            
            MATWinStore.MobileAppTracker.Instance.SetGender(gender_temp);
            #endif
        }
    }

    /// <para>
    /// Sets the user ID to associate with Google.
    /// </para>
    /// <param name="googleUserId">Google user ID.</param>
    public static void SetGoogleUserId(string googleUserId)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetGoogleUserId(googleUserId);
            #endif
            #if UNITY_IPHONE
            MATExterns.setGoogleUserId(googleUserId);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetGoogleUserId(googleUserId);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetGoogleUserId(googleUserId);
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
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetLocation(latitude, longitude, altitude);
            #endif
            #if UNITY_IPHONE
            MATExterns.setLocation(latitude, longitude, altitude);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetLatitude(latitude);
            MATWP8.MobileAppTracker.Instance.SetLongitude(longitude);
            MATWP8.MobileAppTracker.Instance.SetAltitude(altitude);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetLatitude(latitude);
            MATWinStore.MobileAppTracker.Instance.SetLongitude(longitude);
            MATWinStore.MobileAppTracker.Instance.SetAltitude(altitude);
            #endif
        }
    }

    /// <para>
    /// Sets the name of the package.
    /// </para>
    /// <param name="packageName">Package name</param>
    public static void SetPackageName(string packageName)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetPackageName(packageName);
            #endif
            #if UNITY_IPHONE
            MATExterns.setPackageName(packageName);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetPackageName(packageName);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetPackageName(packageName);
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
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetPayingUser(isPayingUser);
            #endif
            #if UNITY_IPHONE
            MATExterns.setPayingUser(isPayingUser);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetIsPayingUser(isPayingUser);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetIsPayingUser(isPayingUser);
            #endif
        }
    }

    ///<para>
    ///Sets the user ID to associate with Twitter.
    ///</para>
    ///<param name="twitterUserId">Twitter user ID</param>
    public static void SetTwitterUserId(string twitterUserId)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetTwitterUserId(twitterUserId);
            #endif
            #if UNITY_IPHONE
            MATExterns.setTwitterUserId(twitterUserId);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetTwitterUserId(twitterUserId);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetTwitterUserId(twitterUserId);
            #endif
        }
    }

    ///<para>
    ///Sets the custom user email.
    ///</para>
    ///<param name="userEmail">User's email address</param>
    public static void SetUserEmail(string userEmail)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetUserEmail(userEmail);
            #endif
            #if UNITY_IPHONE
            MATExterns.setUserEmail(userEmail);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetUserEmail(userEmail);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetUserEmail(userEmail);
            #endif
        }
    }

    /// <para>
    /// Sets the custom user ID.
    /// </para>
    /// <param name="userId">the new user ID</param>
    public static void SetUserId(string userId)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetUserId(userId);
            #endif
            #if UNITY_IPHONE
            MATExterns.setUserId(userId);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetUserId(userId);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetUserId(userId);
            #endif
        }
    }

    ///<para>
    ///Sets the custom user name.
    ///</para>
    ///<param name="userName">User name</param>
    public static void SetUserName(string userName)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetUserName(userName);
            #endif
            #if UNITY_IPHONE
            MATExterns.setUserName(userName);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetUserName(userName);
            #endif
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetUserName(userName);
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
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            return MATAndroid.Instance.GetIsPayingUser();
            #endif
            #if UNITY_IPHONE
            return MATExterns.getIsPayingUser();
            #endif
            #if UNITY_WP8
            return MATWP8.MobileAppTracker.Instance.GetIsPayingUser();
            #endif
            #if UNITY_METRO
            return MATWinStore.MobileAppTracker.Instance.GetIsPayingUser();
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
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            return MATAndroid.Instance.GetMatId();
            #endif
            #if UNITY_IPHONE
            return MATExterns.getMatId();
            #endif
            #if UNITY_WP8
            return MATWP8.MobileAppTracker.Instance.GetMatId();
            #endif
            #if UNITY_METRO
            return MATWinStore.MobileAppTracker.Instance.GetMatId();
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
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            return MATAndroid.Instance.GetOpenLogId();
            #endif
            #if UNITY_IPHONE
            return MATExterns.getOpenLogId();
            #endif
            #if UNITY_WP8
            return MATWP8.MobileAppTracker.Instance.GetOpenLogId();
            #endif
            #if UNITY_METRO
            return MATWinStore.MobileAppTracker.Instance.GetOpenLogId();
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
            MATExterns.setAppleAdvertisingIdentifier(advertiserIdentifier, trackingEnabled);
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
        if(!Application.isEditor)
        {
            #if UNITY_IPHONE
            MATExterns.setAppleVendorIdentifier(vendorIdentifier);
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
        if(!Application.isEditor)
        {
            #if UNITY_IPHONE
            MATExterns.setJailbroken(isJailbroken);
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
        if(!Application.isEditor)
        {
            #if UNITY_IPHONE
            MATExterns.setShouldAutoDetectJailbroken(isAutoDetectJailbroken);
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
        if(!Application.isEditor)
        {
            #if UNITY_IPHONE
            MATExterns.setShouldAutoGenerateAppleVendorIdentifier(shouldAutoGenerate);
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
        if(!Application.isEditor)
        {
            #if UNITY_IPHONE
            MATExterns.setUseCookieTracking(useCookieTracking);
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
        if(!Application.isEditor)
        {
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
        if(!Application.isEditor)
        {
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
        if(!Application.isEditor)
        {
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
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetGoogleAdvertisingId(adId, isLATEnabled);
            #endif
        }
    }

    /// <para>
    /// Sets the publisher ID.
    /// Does nothing if not an Android device.
    /// </para>
    /// <param name="publisherId">MAT publisher ID</param>
    public static void SetPublisherId(string publisherId)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetPublisherId(publisherId);
            #endif
        }
    }

    /// <para>
    /// Sets the offer ID.
    /// Does nothing if not an Android device.
    /// </para>
    /// <param name="offerId">MAT offer ID</param>
    public static void SetOfferId(string offerId)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetOfferId(offerId);
            #endif
        }
    }

    /// <para>
    /// Sets the publisher reference ID.
    /// Does nothing if not an Android device.
    /// </para>
    /// <param name="refId">MAT publisher reference ID</param>
    public static void SetPublisherReferenceId(string refId)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetPublisherReferenceId(refId);
            #endif
        }
    }

    /// <para>
    /// Sets the publisher sub1.
    /// Does nothing if not an Android device.
    /// </para>
    /// <param name="sub1">MAT publisher sub1 value</param>
    public static void SetPublisherSub1(string sub1)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetPublisherSub1(sub1);
            #endif
        }
    }

    /// <para>
    /// Sets the publisher sub2.
    /// Does nothing if not an Android device.
    /// </para>
    /// <param name="sub2">MAT publisher sub2 value</param>
    public static void SetPublisherSub2(string sub2)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetPublisherSub2(sub2);
            #endif
        }
    }

    /// <para>
    /// Sets the publisher sub3.
    /// Does nothing if not an Android device.
    /// </para>
    /// <param name="sub3">MAT publisher sub3 value</param>
    public static void SetPublisherSub3(string sub3)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetPublisherSub3(sub3);
            #endif
        }
    }

    /// <para>
    /// Sets the publisher sub4.
    /// Does nothing if not an Android device.
    /// </para>
    /// <param name="sub4">MAT publisher sub4 value</param>
    public static void SetPublisherSub4(string sub4)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetPublisherSub4(sub4);
            #endif
        }
    }

    /// <para>
    /// Sets the publisher sub5.
    /// Does nothing if not an Android device.
    /// </para>
    /// <param name="sub5">MAT publisher sub5 value</param>
    public static void SetPublisherSub5(string sub5)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetPublisherSub5(sub5);
            #endif
        }
    }

    /// <para>
    /// Sets the publisher sub ad.
    /// Does nothing if not an Android device.
    /// </para>
    /// <param name="subAd">MAT publisher sub ad value</param>
    public static void SetPublisherSubAd(string subAd)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetPublisherSubAd(subAd);
            #endif
        }
    }

    /// <para>
    /// Sets the publisher sub adgroup.
    /// Does nothing if not an Android device.
    /// </para>
    /// <param name="subAdgroup">MAT publisher sub adgroup value</param>
    public static void SetPublisherSubAdgroup(string subAdgroup)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetPublisherSubAdgroup(subAdgroup);
            #endif
        }
    }

    /// <para>
    /// Sets the publisher sub campaign.
    /// Does nothing if not an Android device.
    /// </para>
    /// <param name="subCampaign">MAT publisher sub campaign value</param>
    public static void SetPublisherSubCampaign(string subCampaign)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetPublisherSubCampaign(subCampaign);
            #endif
        }
    }

    /// <para>
    /// Sets the publisher sub keyword.
    /// Does nothing if not an Android device.
    /// </para>
    /// <param name="subKeyword">MAT publisher sub keyword value</param>
    public static void SetPublisherSubKeyword(string subKeyword)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetPublisherSubKeyword(subKeyword);
            #endif
        }
    }

    /// <para>
    /// Sets the publisher sub publisher.
    /// Does nothing if not an Android device.
    /// </para>
    /// <param name="subPublisher">MAT publisher sub publisher value</param>
    public static void SetPublisherSubPublisher(string subPublisher)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetPublisherSubPublisher(subPublisher);
            #endif
        }
    }

    /// <para>
    /// Sets the publisher sub site.
    /// Does nothing if not an Android device.
    /// </para>
    /// <param name="subSite">MAT publisher sub site value</param>
    public static void SetPublisherSubSite(string subSite)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetPublisherSubSite(subSite);
            #endif
        }
    }

    /// <para>
    /// Sets the advertiser sub ad.
    /// Does nothing if not an Android device.
    /// </para>
    /// <param name="subAd">MAT advertiser sub ad value</param>
    public static void SetAdvertiserSubAd(string subAd)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetAdvertiserSubAd(subAd);
            #endif
        }
    }

    /// <para>
    /// Sets the advertiser sub adgroup.
    /// Does nothing if not an Android device.
    /// </para>
    /// <param name="subAdgroup">MAT advertiser sub adgroup value</param>
    public static void SetAdvertiserSubAdgroup(string subAdgroup)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetAdvertiserSubAdgroup(subAdgroup);
            #endif
        }
    }

    /// <para>
    /// Sets the advertiser sub campaign.
    /// Does nothing if not an Android device.
    /// </para>
    /// <param name="subCampaign">MAT advertiser sub campaign value</param>
    public static void SetAdvertiserSubCampaign(string subCampaign)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetAdvertiserSubCampaign(subCampaign);
            #endif
        }
    }

    /// <para>
    /// Sets the advertiser sub keyword.
    /// Does nothing if not an Android device.
    /// </para>
    /// <param name="subKeyword">MAT advertiser sub keyword value</param>
    public static void SetAdvertiserSubKeyword(string subKeyword)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetAdvertiserSubKeyword(subKeyword);
            #endif
        }
    }

    /// <para>
    /// Sets the advertiser sub publisher.
    /// Does nothing if not an Android device.
    /// </para>
    /// <param name="subPublisher">MAT advertiser sub keyword value</param>
    public static void SetAdvertiserSubPublisher(string subPublisher)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetAdvertiserSubPublisher(subPublisher);
            #endif
        }
    }

    /// <para>
    /// Sets the advertiser sub site.
    /// Does nothing if not an Android device.
    /// </para>
    /// <param name="subSite">MAT advertiser sub site value</param>
    public static void SetAdvertiserSubSite(string subSite)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetAdvertiserSubSite(subSite);
            #endif
        }
    }

    /////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////
    /*---------------------Windows Phone 8 Specific Features-------------------*/
    /////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////

    /// <para>
    /// Sets the name of the app.
    /// Does nothing if not a Windows Phone 8 device.
    /// </para>
    /// <param name="appName">App name</param>
    public static void SetAppName(string appName)
    {
        if(!Application.isEditor)
        {
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetAppName(appName);
            #endif
        }
    }


    /// <para>
    /// Sets the app version.
    /// Does nothing if not a Windows Phone 8 device.
    /// </para>
    /// <param name="appVersion">App version</param>
    public static void SetAppVersion(string appVersion)
    {
        if(!Application.isEditor)
        {
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetAppVersion(appVersion);
            #endif
        }
    }

    /// <para>
    /// Sets the last open log ID.
    /// Does nothing if not a Windows Phone 8 device.
    /// </para>
    /// <param name="lastOpenLogId">Last open log ID</param>
    public static void SetLastOpenLogId(string lastOpenLogId)
    {
        if(!Application.isEditor)
        {
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetLastOpenLogId(lastOpenLogId);
            #endif
        }
    }

    /// <para>
    /// Sets the MAT response.
    /// Does nothing if not a Windows Phone 8 device.
    /// </para>
    /// <param name="matResponse">MAT response</param>
    public static void SetMATResponse(MATWP8.MATResponse matResponse)
    {
        if(!Application.isEditor)
        {
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetMATResponse (matResponse);
            #endif
        }
    }

    /// <para>
    /// Sets the MAT response.
    /// Does nothing if not a Windows Store device.
    /// </para>
    /// <param name="matResponse">MAT response</param>
    public static void SetMATResponse(MATWinStore.MATResponse matResponse)
    {
        if(!Application.isEditor)
        {
            #if UNITY_METRO
            MATWinStore.MobileAppTracker.Instance.SetMATResponse(matResponse);
            #endif
        }
    }
    
    /// <para>
    /// Sets the OS version.
    /// Does nothing if not a Windows Phone 8 device.
    /// </para>
    /// <param name="osVersion">OS version</param>
    public static void SetOSVersion(string osVersion)
    {
        if(!Application.isEditor)
        {
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.SetOSVersion(osVersion);
            #endif
        }
    }

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
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetCurrencyCode(currencyCode);
            #endif
            #if UNITY_IPHONE
            MATExterns.setCurrencyCode(currencyCode);
            #endif
        }
    }

    /// <para> 
    /// Sets delegate used by MobileAppTracker to post success and failure callbacks from the MAT SDK.
    /// Does nothing if not an Android or iOS device.
    /// </para>
    /// <param name="enable">If set to true enable delegate.</param>
    public static void SetDelegate(bool enable)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetDelegate(enable);
            #endif
            #if (UNITY_IPHONE)
            MATExterns.setDelegate(enable);
            #endif
            
        }
    }

    /// <para>
    /// Sets the MAT site ID to specify which app to attribute to.
    /// Does nothing if not Android or iOS device.
    /// </para>
    /// <param name="siteId"> MAT site ID to attribute to</param>
    public static void SetSiteId(string siteId)
    {
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetSiteId(siteId);
            #endif
            #if UNITY_IPHONE
            MATExterns.setSiteId(siteId);
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
        if(!Application.isEditor)
        {
            #if UNITY_ANDROID
            MATAndroid.Instance.SetTRUSTeId(tpid);
            #endif
            #if UNITY_IPHONE
            MATExterns.setTRUSTeId(tpid);
            #endif
        }
    }

    /////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////
    /*---------------------End of Platform-specific Features-------------------*/
    /////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////
}