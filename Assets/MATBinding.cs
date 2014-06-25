using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

/// <para>
/// MATBinding wraps Android and iOS MobileAppTracking SDK 
/// functions to be used within Unity. The functions can be called within any 
/// C# script in Unity by typing MATBinding.*.
/// </para>
public class MATBinding
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
        }
    }
    

    /// <para>
    /// Sets the ISO 4217 currency code.
    /// </para>
    /// <param name="currency_code"> the currency code</param>
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
        }
    }
    

    /// <para>
    /// Sets the MAT site ID to specify which app to attribute to.
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
        }
    }
    
    /// <para>
    /// Start an app-to-app tracking session on the MAT server.
    /// </para>
    /// <param name="targetAppId">The bundle ID of the target app</param>
    /// <param name="advertiserId">The MAT advertiser ID of the target app</param>
    /// <param name="offerId">The MAT offer ID of the target app</param>
    /// <param name="publisherId">The MAT publisher ID of the target app</param>
    /// <param name="shouldRedirect">Should redirect to the download url if the tracking session was 
    ///  successfully created</param>
    public static void StartAppToAppTracking(string targetAppId, string advertiserId, string offerId, string publisherId, bool shouldRedirect) 
    {
        if(!Application.isEditor) 
        {
            #if (UNITY_ANDROID || UNITY_IPHONE)
            startAppToAppTracking(targetAppId, advertiserId, offerId, publisherId, shouldRedirect);
            #endif
        }
    }
    

    /// <para>
    /// Sets whether app-level ad tracking is enabled.
    /// </para>
    /// <param name="adTrackingEnabled">true if user has opted out of ad tracking at the app-level, false if not</param>
    public static void SetAppAdTracking(bool adTrackingEnabled) 
    {
        if(!Application.isEditor) 
        {
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setAppAdTracking(adTrackingEnabled);
            #endif
        }
    }
    
    /// <para>
    /// Sets delegate used by MobileAppTracker to post success and failure callbacks from the MAT SDK.
    /// Does nothing if not iOS device.
    /// </para>
    /// <param name="enable">If set to true enable delegate</param>
    public static void SetDelegate(bool enable) 
    {
        if(!Application.isEditor) 
        {
            #if UNITY_IPHONE
            setDelegate(enable);
            #endif
        }
    }
    
    /// <para>
    /// Sets a url to be used with app-to-app tracking so that
    /// the sdk can open the download (redirect) url. This is
    /// used in conjunction with the setTracking:advertiserId:offerId:publisherId:redirect: method.
    /// </para>
    /// <param name="redirect_url">The string name for the url</param>
    public static void SetRedirectUrl(string redirectUrl) 
    {
        if(!Application.isEditor) 
        {
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setRedirectUrl(redirectUrl);
            #endif
        }
    }
    

    
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
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setGender(gender);
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
            #if UNITY_IPHONE
            setUseCookieTracking(useCookieTracking);
            #endif
        }
    }
    
    /// <para>
    /// Set the Apple Vendor Identifier available in iOS 6.
    /// Does nothing if not an iOS device.
    /// </para>
    /// <param name="vendorIdentifier">Apple Vendor Identifier</param>
    public static void SetVendorIdentifier(string vendorIdentifier) 
    {
        if(!Application.isEditor) 
        {
            #if UNITY_IPHONE
            setAppleVendorIdentifier(vendorIdentifier);
            #endif
        }
    }
    

    /// <para>
    /// Sets the custom user email.
    /// </para>
    /// <param name="user_email">User's email address</param>
    public static void SetUserEmail(string user_email) 
    {
        if(!Application.isEditor) 
        {
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setUserEmail(user_email);
            #endif
        }
    }
    

    /// <para>
    /// Sets the custom user name.
    /// </para>
    /// <param name="user_name">User name</param>
    public static void SetUserName(string user_name) 
    {
        if(!Application.isEditor) 
        {
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setUserName(user_name);
            #endif
        }
    }
    

    /// <para>
    /// Sets the user ID to associate with Facebook.
    /// </para>
    /// <param name="fb_user_id">Facebook User ID</param>
    public static void SetFacebookUserId(string fb_user_id) 
    {
        if(!Application.isEditor) 
        {
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setFacebookUserId(fb_user_id);
            #endif
        }
    }
    

    /// <para>
    /// Sets the user ID to associate with Twitter.
    /// </para>
    /// <param name="twitter_user_id">Twitter user ID</param>
    public static void SetTwitterUserId(string twitter_user_id) 
    {
        if(!Application.isEditor) 
        {
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setTwitterUserId(twitter_user_id);
            #endif
        }
    }
    /// <para>
    /// Sets the user ID to associate with Google.
    /// </para>
    /// <param name="google_user_id">Google user ID</param>
    public static void SetGoogleUserId(string google_user_id) 
    {
        if(!Application.isEditor) 
        {
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setGoogleUserId(google_user_id);
            #endif
        }
    }
    /// <para>
    /// Sets whether app was previously installed prior to version with MAT SDK.
    /// </para>
    /// <param name="isExistingUser">
    /// true if this user already had the app installed prior to updating to MAT version
    /// </param>
    public static void SetExistingUser(bool isExistingUser) 
    {
        if(!Application.isEditor) 
        {
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setExistingUser(isExistingUser);
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
    /// Measures the action with event items.
    /// </para>
    /// <param name="action">Action</param>
    /// <param name="items">Items</param>
    /// <param name="eventItemCount">Event item count</param>
    /// <param name="refId">Reference ID</param>
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
    /// Sets the first date associated with an app event.
    /// </para>
    /// <param name="eventDate">the date</param>
    public static void SetEventDate1(string eventDate) 
    {
        if(!Application.isEditor) 
        {
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setEventDate1(eventDate);
            #endif
        }
    }

    /// <para>
    /// Sets the second date associated with an app event.
    /// </para>
    /// <param name="eventDate">the date</param>
    public static void SetEventDate2(string eventDate) 
    {
        if(!Application.isEditor) 
        {
            #if (UNITY_ANDROID || UNITY_IPHONE)
            setEventDate2(eventDate);
            #endif
        }
    }

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
        }
        
        return string.Empty;
    }
    
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
    private static extern void setAge(int age);
    [DllImport ("mobileapptracker")]
    private static extern void setAppAdTracking(bool enable);
    [DllImport ("mobileapptracker")]
    private static extern void setCurrencyCode(string currencyCode);
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
    private static extern void startAppToAppTracking(string targetAppId, string advertiserId, string offerId, string publisherId, bool shouldRedirect);
    
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
    private static extern void setRedirectUrl(string redirectUrl);
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
    
    // Methods for app-to-app tracking
    [DllImport ("__Internal")]
    private static extern void startAppToAppTracking(string targetAppId, string advertiserId, string offerId, string publisherId, bool shouldRedirect);
    [DllImport ("__Internal")]
    private static extern void setRedirectUrl(string redirectUrl);
    
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

