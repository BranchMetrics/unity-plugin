using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;

namespace com.mobileapptracking
{
    // Externs used by the iOS component.
    class MATExterns
    {
        #if UNITY_IPHONE
        // Main initializer method for MAT
        [DllImport ("__Internal")]
        internal static extern void initNativeCode(string advertiserId, string conversionKey);

        // Method to check if a deferred deep-link is available
        [DllImport ("__Internal")]
        internal static extern void checkForDeferredDeeplinkWithTimeout(double timeoutMillis);

        // Methods to help debugging and testing
        [DllImport ("__Internal")]
        internal static extern void tuneSetDebugMode(bool enable);
        [DllImport ("__Internal")]
        internal static extern void setAllowDuplicates(bool allowDuplicateRequests);

        // Method to enable MAT delegate success/failure callbacks
        [DllImport ("__Internal")]
        internal static extern void setDelegate(bool enable);

        // Optional Setter Methods
        [DllImport ("__Internal")]
        internal static extern void setAppAdTracking(bool enable);
        [DllImport ("__Internal")]
        internal static extern void setCurrencyCode(string currencyCode);
        [DllImport ("__Internal")]
        internal static extern void setPackageName(string packageName);
        [DllImport ("__Internal")]
        internal static extern void setSiteId(string siteId);
        [DllImport ("__Internal")]
        internal static extern void setTRUSTeId(string trusteTPID);
        [DllImport ("__Internal")]
        internal static extern void setUserEmail(string userEmail);
        [DllImport ("__Internal")]
        internal static extern void setUserId(string userId);
        [DllImport ("__Internal")]
        internal static extern void setUserName(string userName);
        [DllImport ("__Internal")]
        internal static extern void setFacebookEventLogging(bool enable, bool limit);
        [DllImport ("__Internal")]
        internal static extern void setFacebookUserId(string facebookUserId);
        [DllImport ("__Internal")]
        internal static extern void setTwitterUserId(string twitterUserId);
        [DllImport ("__Internal")]
        internal static extern void setGoogleUserId(string googleUserId);
        [DllImport ("__Internal")]
        internal static extern bool getIsPayingUser();
        [DllImport ("__Internal")]
        internal static extern void setExistingUser(bool isExisting);
        [DllImport ("__Internal")]
        internal static extern void setPayingUser(bool isPaying);
        [DllImport ("__Internal")]
        internal static extern void setJailbroken(bool isJailbroken);
        [DllImport ("__Internal")]
        internal static extern void setShouldAutoDetectJailbroken(bool shouldAutoDetect);
        [DllImport ("__Internal")]
        internal static extern void setAge(int age);
        [DllImport ("__Internal")]
        internal static extern void setGender(int gender);
        [DllImport ("__Internal")]
        internal static extern void setLocation(double latitude, double longitude, double altitude);
        [DllImport ("__Internal")]
        internal static extern void setEventAttribute1(string value);
        [DllImport ("__Internal")]
        internal static extern void setEventAttribute2(string value);
        [DllImport ("__Internal")]
        internal static extern void setEventAttribute3(string value);
        [DllImport ("__Internal")]
        internal static extern void setEventAttribute4(string value);
        [DllImport ("__Internal")]
        internal static extern void setEventAttribute5(string value);

        [DllImport ("__Internal")]
        internal static extern void setEventContentType(string contentType);
        [DllImport ("__Internal")]
        internal static extern void setEventContentId(string contentId);
        [DllImport ("__Internal")]
        internal static extern void setEventDate1(string dateString);
        [DllImport ("__Internal")]
        internal static extern void setEventDate2(string dateString);
        [DllImport ("__Internal")]
        internal static extern void setEventLevel(int level);
        [DllImport ("__Internal")]
        internal static extern void setEventQuantity(int quantity);
        [DllImport ("__Internal")]
        internal static extern void setEventRating(float rating);
        [DllImport ("__Internal")]
        internal static extern void setEventSearchString(string searchString);

        // Method to enable cookie based tracking
        [DllImport ("__Internal")]
        internal static extern void setUseCookieTracking(bool useCookieTracking);

        // Methods for setting Apple related id
        [DllImport ("__Internal")]
        internal static extern void setAppleAdvertisingIdentifier(string appleAdvertisingIdentifier, bool trackingEnabled);
        [DllImport ("__Internal")]
        internal static extern void setAppleVendorIdentifier(string appleVendorIdentifier);
        [DllImport ("__Internal")]
        internal static extern void setShouldAutoGenerateAppleVendorIdentifier(bool shouldAutoGenerate);

        // Methods to measure custom in-app events
        [DllImport ("__Internal")]
        internal static extern void measureAction(string action);
        [DllImport ("__Internal")]
        internal static extern void measureActionWithRefId(string action, string refId);
        [DllImport ("__Internal")]
        internal static extern void measureActionWithRevenue(string action, double revenue, string currencyCode, string refId);
        [DllImport ("__Internal")]
        internal static extern void measureActionWithEventItems(string action, MATItem[] items, int eventItemCount, string refId, double revenue, string currency, int transactionState, string receipt, string receiptSignature);

        // Method to measure session
        [DllImport ("__Internal")]
        internal static extern void measureSession();

        [DllImport ("__Internal")]
        internal static extern string getMatId();
        [DllImport ("__Internal")]
        internal static extern string getOpenLogId();

        // Android-only methods
        [DllImport ("__Internal")]
        internal static extern void setGoogleAdvertisingId(string advertisingId, bool limitAdTracking);

        #endif
    }
}