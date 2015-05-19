using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;

namespace MATSDK
{
    // Externs used by the iOS component.
    class MATExterns
    {
        #if UNITY_IPHONE
        // Main initializer method for MAT
        [DllImport ("__Internal")]
        internal static extern void MATInit(string advertiserId, string conversionKey);

        // Method to check if a deferred deep-link is available
        [DllImport ("__Internal")]
        internal static extern void MATCheckForDeferredDeeplinkWithTimeout(double timeoutMillis);

        // Method to enable auto-measurement of successful in-app-purchase (IAP) transactions as "purchase" events
        [DllImport ("__Internal")]
        internal static extern void MATAutomateIapEventMeasurement(bool automate);

        // Method to enable auto-logging of events to Facebook, when Facebook SDK has been included in the project
        [DllImport ("__Internal")]
        internal static extern void MATSetFacebookEventLogging(bool enable, bool limit);

        // Methods to help debugging and testing
        [DllImport ("__Internal")]
        internal static extern void MATSetDebugMode(bool enable);
        [DllImport ("__Internal")]
        internal static extern void MATSetAllowDuplicates(bool allowDuplicateRequests);

        // Method to enable MAT delegate success/failure callbacks
        [DllImport ("__Internal")]
        internal static extern void MATSetDelegate(bool enable);

        // Optional Setter Methods
        [DllImport ("__Internal")]
        internal static extern void MATSetAppAdTracking(bool enable);
        [DllImport ("__Internal")]
        internal static extern void MATSetCurrencyCode(string currencyCode);
        [DllImport ("__Internal")]
        internal static extern void MATSetPackageName(string packageName);
        [DllImport ("__Internal")]
        internal static extern void MATSetPhoneNumber(string phoneNumber);
        [DllImport ("__Internal")]
        internal static extern void MATSetSiteId(string siteId);
        [DllImport ("__Internal")]
        internal static extern void MATSetTRUSTeId(string trusteTPID);
        [DllImport ("__Internal")]
        internal static extern void MATSetUserEmail(string userEmail);
        [DllImport ("__Internal")]
        internal static extern void MATSetUserId(string userId);
        [DllImport ("__Internal")]
        internal static extern void MATSetUserName(string userName);
        [DllImport ("__Internal")]
        internal static extern void MATSetFacebookUserId(string facebookUserId);
        [DllImport ("__Internal")]
        internal static extern void MATSetTwitterUserId(string twitterUserId);
        [DllImport ("__Internal")]
        internal static extern void MATSetGoogleUserId(string googleUserId);
        [DllImport ("__Internal")]
        internal static extern void MATSetExistingUser(bool isExisting);
        [DllImport ("__Internal")]
        internal static extern void MATSetPayingUser(bool isPaying);
        [DllImport ("__Internal")]
        internal static extern void MATSetJailbroken(bool isJailbroken);
        [DllImport ("__Internal")]
        internal static extern void MATSetShouldAutoDetectJailbroken(bool shouldAutoDetect);
        [DllImport ("__Internal")]
        internal static extern void MATSetAge(int age);
        [DllImport ("__Internal")]
        internal static extern void MATSetGender(int gender);
        [DllImport ("__Internal")]
        internal static extern void MATSetLocation(double latitude, double longitude, double altitude);

        // Method to include publisher information in the MAT SDK requests
        [DllImport ("__Internal")]
        internal static extern void MATSetPreloadData(MATPreloadData preloadData);

        // Method to enable cookie based tracking
        [DllImport ("__Internal")]
        internal static extern void MATSetUseCookieTracking(bool useCookieTracking);

        // Methods for setting Apple related id
        [DllImport ("__Internal")]
        internal static extern void MATSetAppleAdvertisingIdentifier(string appleAdvertisingIdentifier, bool trackingEnabled);
        [DllImport ("__Internal")]
        internal static extern void MATSetAppleVendorIdentifier(string appleVendorIdentifier);
        [DllImport ("__Internal")]
        internal static extern void MATSetShouldAutoGenerateAppleVendorIdentifier(bool shouldAutoGenerate);

        // Methods to measure custom in-app events
        [DllImport ("__Internal")]
        internal static extern void MATMeasureEventName(string eventName);
        [DllImport ("__Internal")]
        internal static extern void MATMeasureEventId(int eventId);
        [DllImport ("__Internal")]
        internal static extern void MATMeasureEvent(MATEventIos matEvent, MATItemIos[] items, int itemCount, Byte[] receipt, int receiptLength);
        
        // Method to measure session
        [DllImport ("__Internal")]
        internal static extern void MATMeasureSession();

        // Getter Methods
        [DllImport ("__Internal")]
        internal static extern string MATGetMatId();
        [DllImport ("__Internal")]
        internal static extern string MATGetOpenLogId();
        [DllImport ("__Internal")]
        internal static extern bool MATGetIsPayingUser();

        #endif
    }
}