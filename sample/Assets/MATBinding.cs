using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq; //for ToList function

/// <para>
/// MATBinding wraps Android, iOS, and Windows Phone 8 MobileAppTracking SDK 
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            initNativeCode(advertiserId, conversionKey);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.initializeValues(advertiserId, conversionKey);
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            measureAction(action);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.MeasureAction(action);
            #endif
        }
    }

    /// <para>
    /// Measures the action with event items.
    /// </para>
    /// <param name="action">Action</param>
    /// <param name="items">Items</param>
    /// <param name="eventItemCount">Event item count</param>
    /// <param name="refId">Reference identifier</param>
    /// <param name="revenue">Revenue</param>
    /// <param name="currency">Currency</param>
    /// <param name="transactionState">Transaction state</param>
    /// <param name="receipt">Receipt</param>
    /// <param name="receiptSignature">Receipt signature</param>
    public static void MeasureActionWithEventItems(string action, MATItem[] items, int eventItemCount, string refId, double revenue, string currency, int transactionState, string receipt, string receiptSignature)
    {
        if(!Application.isEditor)
        {
            #if (UNITY_ANDROID || UNITY_IPHONE)
            measureActionWithEventItems(action, items, eventItemCount, refId, revenue, currency, transactionState, receipt, receiptSignature);
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            measureActionWithRefId(action, refId);
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            measureActionWithRevenue(action, revenue, currencyCode, refId);
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
            #if UNITY_IPHONE
            setAppleAdvertisingIdentifier(UnityEngine.iPhone.advertisingIdentifier, UnityEngine.iPhone.advertisingTrackingEnabled);
            #endif
            
            #if (UNITY_ANDROID || UNITY_IPHONE)
            measureSession();
            #endif
            
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.MeasureSession();
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setAge(age);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.Age = age;
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setAllowDuplicates(allow);
            #endif
            #if UNITY_WP8 
            MATWP8.MobileAppTracker.Instance.AllowDuplicates = allow;
            #endif
        }
    }

    ///<para>Sets whether app-level ad tracking is enabled.</para>
    ///<param name="adTrackingEnabled">true if user has opted out of ad tracking at the app-level, false if not</param>
    public static void SetAppAdTracking(bool adTrackingEnabled)
    {
        if(!Application.isEditor)
        {
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setAppAdTracking(adTrackingEnabled);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.AppAdTracking = adTrackingEnabled;
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setDebugMode(debug);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.DebugMode = debug;
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setEventAttribute1(eventAttribute);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.EventAttribute1 = eventAttribute;
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setEventAttribute2(eventAttribute);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.EventAttribute2 = eventAttribute;
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setEventAttribute3(eventAttribute);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.EventAttribute3 = eventAttribute;
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setEventAttribute4(eventAttribute);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.EventAttribute4 = eventAttribute;
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setEventAttribute5(eventAttribute);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.EventAttribute5 = eventAttribute;
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setEventContentId(eventContentId);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.EventContentId = eventContentId;
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setEventContentType(eventContentType);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.EventContentType = eventContentType;
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
            #if (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8)
            double milliseconds = new TimeSpan(eventDate.Ticks).TotalMilliseconds;
            //datetime starts in 1970
            DateTime datetime = new DateTime(1970, 1, 1);
            double millisecondsFrom1970 = milliseconds - (new TimeSpan(datetime.Ticks)).TotalMilliseconds;

            #if (UNITY_ANDROID || UNITY_IPHONE)
            setEventDate1(millisecondsFrom1970.ToString());
            #endif
            
            #if UNITY_WP8
            TimeSpan timeFrom1970 = TimeSpan.FromMilliseconds(millisecondsFrom1970);
            //Update to current time for c#
            datetime = datetime.Add(timeFrom1970);
            MATWP8.MobileAppTracker.Instance.EventDate1 = datetime;
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
            #if (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8)
            double milliseconds = new TimeSpan(eventDate.Ticks).TotalMilliseconds;
            //datetime starts in 1970
            DateTime datetime = new DateTime(1970, 1, 1);
            double millisecondsFrom1970 = milliseconds - (new TimeSpan(datetime.Ticks)).TotalMilliseconds;
            
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setEventDate2(millisecondsFrom1970.ToString());
            #endif
            
            #if UNITY_WP8
            TimeSpan timeFrom1970 = TimeSpan.FromMilliseconds(millisecondsFrom1970);
            //Update to current time for c#
            datetime = datetime.Add(timeFrom1970);
            MATWP8.MobileAppTracker.Instance.EventDate2 = datetime;
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setEventLevel(eventLevel);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.EventLevel = eventLevel;
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setEventQuantity(eventQuantity);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.EventQuantity = eventQuantity;
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setEventRating(eventRating);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.EventRating = eventRating;
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setEventSearchString(eventSearchString);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.EventSearchString = eventSearchString;
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setExistingUser(isExistingUser);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.ExistingUser = isExistingUser;
            #endif
        }
    }

    ///<para>
    ///Sets the user ID to associate with Facebook.
    ///</para>
    ///<param name="fb_user_id">Facebook User ID</param>
    public static void SetFacebookUserId(string fb_user_id)
    {
        if(!Application.isEditor)
        {
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setFacebookUserId(fb_user_id);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.FacebookUserId = fb_user_id;
            #endif
        }
    }

    ///<para>
    ///Sets the user gender.
    ///</para>
    ///<param name="gender">use MobileAppTracker.GENDER_MALE, MobileAppTracker.GENDER_FEMALE</param>
    public static void SetGender(int gender)
    {
        if(!Application.isEditor)
        {
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setGender(gender);
            #endif
            #if UNITY_WP8
            MATWP8.MATGender gender_temp;
            if(gender == 0)
                gender_temp = MATWP8.MATGender.MALE;
            else if (gender == 1)
                gender_temp = MATWP8.MATGender.FEMALE;
            else
                gender_temp = MATWP8.MATGender.NONE;
            
            MATWP8.MobileAppTracker.Instance.Gender = gender_temp;
            #endif
        }
    }

    /// <para>
    /// Sets the user ID to associate with Google.
    /// </para>
    /// <param name="google_user_id">Google user ID.</param>
    public static void SetGoogleUserId(string google_user_id)
    {
        if(!Application.isEditor)
        {
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setGoogleUserId(google_user_id);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.GoogleUserId = google_user_id;
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setLocation(latitude, longitude, altitude);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.Latitude = latitude;
            MATWP8.MobileAppTracker.Instance.Longitude = longitude;
            MATWP8.MobileAppTracker.Instance.Altitude = altitude;
            #endif
        }
    }

    /// <para>
    /// Sets the name of the package.
    /// </para>
    /// <param name="package_name">Package name</param>
    public static void SetPackageName(string package_name)
    {
        if(!Application.isEditor)
        {
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setPackageName(package_name);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.PackageName = package_name;
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setPayingUser(isPayingUser);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.IsPayingUser = isPayingUser;
            #endif
        }
    }

    ///<para>
    ///Sets the user ID to associate with Twitter.
    ///</para>
    ///<param name="twitter_user_id">Twitter user ID</param>
    public static void SetTwitterUserId(string twitter_user_id)
    {
        if(!Application.isEditor)
        {
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setTwitterUserId(twitter_user_id);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.TwitterUserId = twitter_user_id;
            #endif
        }
    }

    ///<para>
    ///Sets the custom user email.
    ///</para>
    ///<param name="user_email">User's email address</param>
    public static void SetUserEmail(string user_email)
    {
        if(!Application.isEditor)
        {
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setUserEmail(user_email);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.UserEmail = user_email;
            #endif
        }
    }

    /// <para>
    /// Sets the custom user ID.
    /// </para>
    /// <param name="user_id">the new user ID</param>
    public static void SetUserId(string user_id)
    {
        if(!Application.isEditor)
        {
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setUserId(user_id);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.UserId = user_id;
            #endif
        }
    }

    ///<para>
    ///Sets the custom user name.
    ///</para>
    ///<param name="user_name">User name</param>
    public static void SetUserName(string user_name)
    {
        if(!Application.isEditor)
        {
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setUserName(user_name);
            #endif
            #if UNITY_WP8
            MATWP8.MobileAppTracker.Instance.UserName = user_name;
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            return getIsPayingUser();
            #endif
            #if UNITY_WP8
            return MATWP8.MobileAppTracker.Instance.IsPayingUser;
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            return getMatId();
            #endif
            #if UNITY_WP8
            return MATWP8.MobileAppTracker.Instance.MatId;
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            return getOpenLogId();
            #endif
            #if UNITY_WP8
            return MATWP8.MobileAppTracker.Instance.OpenLogId;
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
           setAppleAdvertisingIdentifier(advertiserIdentifier, trackingEnabled);
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
            setAppleVendorIdentifier(vendorIdentifier);
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
            setJailbroken(isJailbroken);
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
            setShouldAutoDetectJailbroken(isAutoDetectJailbroken);
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setShouldAutoGenerateAppleVendorIdentifier(shouldAutoGenerate);
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setUseCookieTracking(useCookieTracking);
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
            setAndroidId(androidId);
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
            setDeviceId(deviceId);
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
            setMacAddress(macAddress);
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
            setGoogleAdvertisingId(adId, isLATEnabled);
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
            setPublisherId(publisherId);
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
            setOfferId(offerId);
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
            setPublisherReferenceId(refId);
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
            setPublisherSub1(sub1);
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
            setPublisherSub2(sub2);
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
            setPublisherSub3(sub3);
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
            setPublisherSub4(sub4);
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
            setPublisherSub5(sub5);
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
            setPublisherSubAd(subAd);
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
            setPublisherSubAdgroup(subAdgroup);
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
            setPublisherSubCampaign(subCampaign);
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
            setPublisherSubKeyword(subKeyword);
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
            setPublisherSubPublisher(subPublisher);
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
            setPublisherSubSite(subSite);
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
            setAdvertiserSubAd(subAd);
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
            setAdvertiserSubAdgroup(subAdgroup);
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
            setAdvertiserSubCampaign(subCampaign);
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
            setAdvertiserSubKeyword(subKeyword);
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
            setAdvertiserSubPublisher(subPublisher);
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
            setAdvertiserSubSite(subSite);
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
        #if UNITY_WP8
        MATWP8.MobileAppTracker.Instance.AppName = appName;
        #endif
    }


    /// <para>
    /// Sets the app version.
    /// Does nothing if not a Windows Phone 8 device.
    /// </para>
    /// <param name="appVersion">App version</param>
    public static void SetAppVersion(string appVersion)
    {
        #if UNITY_WP8
        MATWP8.MobileAppTracker.Instance.AppVersion = appVersion;
        #endif
    }

    /// <para>
    /// Sets the device brand.
    /// Does nothing if not a Windows Phone 8 device.
    /// </para>
    /// <param name="deviceBrand">Device brand</param>
    public static void SetDeviceBrand(string deviceBrand)
    {
        #if UNITY_WP8
        MATWP8.MobileAppTracker.Instance.DeviceBrand = deviceBrand;
        #endif
    }

    /// <para>
    /// Sets the device carrier.
    /// Does nothing if not a Windows Phone 8 device.
    /// </para>
    /// <param name="deviceCarrier">Device carrier</param>
    public static void SetDeviceCarrier(string deviceCarrier)
    {
        #if UNITY_WP8
        MATWP8.MobileAppTracker.Instance.DeviceCarrier = deviceCarrier;
        #endif
    }

    /// <para>
    /// Sets the device model.
    /// Does nothing if not a Windows Phone 8 device.
    /// </para>
    /// <param name="deviceModel">Device model</param>
    public static void SetDeviceModel(string deviceModel)
    {
        #if UNITY_WP8
        MATWP8.MobileAppTracker.Instance.DeviceModel = deviceModel;
        #endif
    }

    /// <para>
    /// Sets the device unique ID.
    /// Does nothing if not a Windows Phone 8 device.
    /// </para>
    /// <param name="deviceUniqueId">Device unique ID</param>
    public static void SetDeviceUniqueId(string deviceUniqueId)
    {
        #if UNITY_WP8
        MATWP8.MobileAppTracker.Instance.DeviceUniqueId = deviceUniqueId;
        #endif
    }

    /// <para>
    /// Sets the last open log ID.
    /// Does nothing if not a Windows Phone 8 device.
    /// </para>
    /// <param name="lastOpenLogId">Last open log ID</param>
    public static void SetLastOpenLogId(string lastOpenLogId)
    {
        #if UNITY_WP8
        MATWP8.MobileAppTracker.Instance.LastOpenLogId = lastOpenLogId;
        #endif
    }

    /// <para>
    /// Sets the MAT response.
    /// Does nothing if not a Windows Phone 8 device.
    /// </para>
    /// <param name="matResponse">MAT response</param>
    public static void SetMATResponse(MATWP8.MATResponse matResponse)
    {
        #if UNITY_WP8
        MATWP8.MobileAppTracker.Instance.SetMATResponse (matResponse);
        #endif
    }
        
    /// <para>
    /// Sets the OS version.
    /// Does nothing if not a Windows Phone 8 device.
    /// </para>
    /// <param name="osVersion">OS version</param>
    public static void SetOSVersion(string osVersion)
    {
        #if UNITY_WP8
        MATWP8.MobileAppTracker.Instance.OSVersion = osVersion;
        #endif
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
    /// <param name="currency_code">the currency code</param>
    public static void SetCurrencyCode(string currency_code)
    {
        if(!Application.isEditor)
        {
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setCurrencyCode(currency_code);
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
            #if (UNITY_IPHONE)
            setDelegate(enable);
            #endif
            #if UNITY_ANDROID
            setMATResponse();
            #endif
        }
    }

    /// <para>
    /// Sets the MAT site ID to specify which app to attribute to.
    /// Does nothing if not Android or iOS device.
    /// </para>
    /// <param name="site_id"> MAT site ID to attribute to</param>
    public static void SetSiteId(string site_id)
    {
        if(!Application.isEditor)
        {
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setSiteId(site_id);
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setTRUSTeId(tpid);
            #endif
        }
    }

    /////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////
    /*---------------------End of Platform-specific Features-------------------*/
    /////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////

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
    private static extern void setMATResponse();
    
    [DllImport ("mobileapptracker")]
    private static extern void setAge(int age);
    [DllImport ("mobileapptracker")]
    private static extern void setAndroidId(string androidId);
    [DllImport ("mobileapptracker")]
    private static extern void setAppAdTracking(bool enable);
    [DllImport ("mobileapptracker")]
    private static extern void setCurrencyCode(string currencyCode);
    [DllImport ("mobileapptracker")]
    private static extern void setDeviceId(string deviceId);
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
    private static extern void setEventContentType(string contentType);
    [DllImport ("mobileapptracker")]
    private static extern void setEventContentId(string contentId);
    [DllImport ("mobileapptracker")]
    private static extern void setEventDate1(string dateString);
    [DllImport ("mobileapptracker")]
    private static extern void setEventDate2(string dateString);
    [DllImport ("mobileapptracker")]
    private static extern void setEventLevel(int level);
    [DllImport ("mobileapptracker")]
    private static extern void setEventQuantity(int quantity);
    [DllImport ("mobileapptracker")]
    private static extern void setEventRating(float rating);
    [DllImport ("mobileapptracker")]
    private static extern void setEventSearchString(string searchString);
    
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
    private static extern void setMacAddress(string macAddress);
    [DllImport ("mobileapptracker")]
    private static extern void setPackageName(string packageName);
    [DllImport ("mobileapptracker")]
    private static extern void setPayingUser(bool isPaying);
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
    private static extern void setPublisherId(string publisherId);
    [DllImport ("mobileapptracker")]
    private static extern void setOfferId(string offerId);
    [DllImport ("mobileapptracker")]
    private static extern void setPublisherReferenceId(string publisherRefId);
    [DllImport ("mobileapptracker")]
    private static extern void setPublisherSub1(string sub1);
    [DllImport ("mobileapptracker")]
    private static extern void setPublisherSub2(string sub2);
    [DllImport ("mobileapptracker")]
    private static extern void setPublisherSub3(string sub3);
    [DllImport ("mobileapptracker")]
    private static extern void setPublisherSub4(string sub4);
    [DllImport ("mobileapptracker")]
    private static extern void setPublisherSub5(string sub5);
    [DllImport ("mobileapptracker")]
    private static extern void setPublisherSubAd(string subAd);
    [DllImport ("mobileapptracker")]
    private static extern void setPublisherSubAdgroup(string subAdgroup);
    [DllImport ("mobileapptracker")]
    private static extern void setPublisherSubCampaign(string subCampaign);
    [DllImport ("mobileapptracker")]
    private static extern void setPublisherSubKeyword(string subKeyword);
    [DllImport ("mobileapptracker")]
    private static extern void setPublisherSubPublisher(string subPublisher);
    [DllImport ("mobileapptracker")]
    private static extern void setPublisherSubSite(string subSite);
    [DllImport ("mobileapptracker")]
    private static extern void setAdvertiserSubAd(string subAd);
    [DllImport ("mobileapptracker")]
    private static extern void setAdvertiserSubAdgroup(string subAdgroup);
    [DllImport ("mobileapptracker")]
    private static extern void setAdvertiserSubCampaign(string subCampaign);
    [DllImport ("mobileapptracker")]
    private static extern void setAdvertiserSubKeyword(string subKeyword);
    [DllImport ("mobileapptracker")]
    private static extern void setAdvertiserSubPublisher(string subPublisher);
    [DllImport ("mobileapptracker")]
    private static extern void setAdvertiserSubSite(string subSite);

    // Tracking functions
    [DllImport ("mobileapptracker")]
    private static extern void measureAction(string action);
    [DllImport ("mobileapptracker")]
    private static extern void measureActionWithRefId(string action, string refId);
    [DllImport ("mobileapptracker")]
    private static extern void measureActionWithRevenue(string action, double revenue, string currencyCode, string refId);
    [DllImport ("mobileapptracker")]
    private static extern void measureActionWithEventItems(string action, MATItem[] items, int eventItemCount, string refId, double revenue, string currency, int transactionState, string receipt, string receiptSignature);
    
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
    private static extern void setPayingUser(bool isPaying);
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
    
    [DllImport ("__Internal")]
    private static extern void setEventContentType(string contentType);
    [DllImport ("__Internal")]
    private static extern void setEventContentId(string contentId);
    [DllImport ("__Internal")]
    private static extern void setEventDate1(string dateString);
    [DllImport ("__Internal")]
    private static extern void setEventDate2(string dateString);
    [DllImport ("__Internal")]
    private static extern void setEventLevel(int level);
    [DllImport ("__Internal")]
    private static extern void setEventQuantity(int quantity);
    [DllImport ("__Internal")]
    private static extern void setEventRating(float rating);
    [DllImport ("__Internal")]
    private static extern void setEventSearchString(string searchString);
    
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
    
    // Methods to measure custom in-app events
    [DllImport ("__Internal")]
    private static extern void measureAction(string action);
    [DllImport ("__Internal")]
    private static extern void measureActionWithRefId(string action, string refId);
    [DllImport ("__Internal")]
    private static extern void measureActionWithRevenue(string action, double revenue, string currencyCode, string refId);
    [DllImport ("__Internal")]
    private static extern void measureActionWithEventItems(string action, MATItem[] items, int eventItemCount, string refId, double revenue, string currency, int transactionState, string receipt, string receiptSignature);
    
    // Method to measure session
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
}

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