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

        // Main initializer method for Tune
        [DllImport ("__Internal")]
        internal static extern void TuneInit(string advertiserId, string conversionKey);

        [DllImport ("__Internal")]
        internal static extern void TuneInitForWearable(string advertiserId, string conversionKey, string packageName, bool wearable);

        // Method to check if a deferred deep-link is available
        [DllImport ("__Internal")]
        internal static extern void TuneCheckForDeferredDeeplink();

        // Method to enable auto-measurement of successful in-app-purchase (IAP) transactions as "purchase" events
        [DllImport ("__Internal")]
        internal static extern void TuneAutomateIapEventMeasurement(bool automate);

        // Method to enable auto-logging of events to Facebook, when Facebook SDK has been included in the project
        [DllImport ("__Internal")]
        internal static extern void TuneSetFacebookEventLogging(bool enable, bool limit);

        // Methods to help debugging and testing
        [DllImport ("__Internal")]
        internal static extern void TuneSetDebugMode(bool enable);
        [DllImport ("__Internal")]
        internal static extern void TuneSetAllowDuplicates(bool allowDuplicateRequests);

        // Method to enable Tune delegate success/failure callbacks
        [DllImport ("__Internal")]
        internal static extern void TuneSetDelegate(bool enable);

        // Optional Setter Methods
        [DllImport ("__Internal")]
        internal static extern void TuneSetAppAdTracking(bool enable);
        [DllImport ("__Internal")]
        internal static extern void TuneSetCurrencyCode(string currencyCode);
        [DllImport ("__Internal")]
        internal static extern void TuneSetPackageName(string packageName);
        [DllImport ("__Internal")]
        internal static extern void TuneSetPhoneNumber(string phoneNumber);
        [DllImport ("__Internal")]
        internal static extern void TuneSetSiteId(string siteId);
        [DllImport ("__Internal")]
        internal static extern void TuneSetTRUSTeId(string trusteTPID);
        [DllImport ("__Internal")]
        internal static extern void TuneSetUserEmail(string userEmail);
        [DllImport ("__Internal")]
        internal static extern void TuneSetUserId(string userId);
        [DllImport ("__Internal")]
        internal static extern void TuneSetUserName(string userName);
        [DllImport ("__Internal")]
        internal static extern void TuneSetFacebookUserId(string facebookUserId);
        [DllImport ("__Internal")]
        internal static extern void TuneSetTwitterUserId(string twitterUserId);
        [DllImport ("__Internal")]
        internal static extern void TuneSetGoogleUserId(string googleUserId);
        [DllImport ("__Internal")]
        internal static extern void TuneSetExistingUser(bool isExisting);
        [DllImport ("__Internal")]
        internal static extern void TuneSetPayingUser(bool isPaying);
        [DllImport ("__Internal")]
        internal static extern void TuneSetJailbroken(bool isJailbroken);
        [DllImport ("__Internal")]
        internal static extern void TuneSetShouldAutoCollectDeviceLocation(bool shouldAutoCollect);
        [DllImport ("__Internal")]
        internal static extern void TuneSetShouldAutoDetectJailbroken(bool shouldAutoDetect);
        [DllImport ("__Internal")]
        internal static extern void TuneSetAge(int age);
        [DllImport ("__Internal")]
        internal static extern void TuneSetGender(int gender);
        [DllImport ("__Internal")]
        internal static extern void TuneSetLocation(double latitude, double longitude, double altitude);
        
        // Method to include publisher information in the Tune SDK requests
        [DllImport ("__Internal")]
        internal static extern void TuneSetPreloadData(MATPreloadData preloadData);
        
        // Method to enable cookie based tracking
        [DllImport ("__Internal")]
        internal static extern void TuneSetUseCookieTracking(bool useCookieTracking);

        // Methods for setting Apple related id
        [DllImport ("__Internal")]
        internal static extern void TuneSetAppleAdvertisingIdentifier(string appleAdvertisingIdentifier, bool trackingEnabled);
        [DllImport ("__Internal")]
        internal static extern void TuneSetAppleVendorIdentifier(string appleVendorIdentifier);
        [DllImport ("__Internal")]
        internal static extern void TuneSetShouldAutoGenerateAppleVendorIdentifier(bool shouldAutoGenerate);
        [DllImport ("__Internal")]
        internal static extern void TuneSetShouldAutoCollectAppleAdvertisingIdentifier(bool shouldAutoCollect);

        // Methods to measure custom in-app events
        [DllImport ("__Internal")]
        internal static extern void TuneMeasureEventName(string eventName);
        [DllImport ("__Internal")]
        internal static extern void TuneMeasureEventId(int eventId);
        [DllImport ("__Internal")]
        internal static extern void TuneMeasureEvent(MATEventIos tuneEvent, MATItemIos[] items, int itemCount, Byte[] receipt, int receiptLength);

        // Method to measure session
        [DllImport ("__Internal")]
        internal static extern void TuneMeasureSession();

        // Getter Methods
        [DllImport ("__Internal")]
        internal static extern string TuneGetMatId();
        [DllImport ("__Internal")]
        internal static extern string TuneGetOpenLogId();
        [DllImport ("__Internal")]
        internal static extern bool TuneGetIsPayingUser();

        /////////////////////
        /// iOS Ad Methods //
        /////////////////////

        [DllImport("__Internal")]
        internal static extern IntPtr TuneCreateMetadata();
        [DllImport("__Internal")]
        internal static extern void TuneAddKeyword(IntPtr metadata, string keyword);
        [DllImport("__Internal")]
        internal static extern void TuneSetBirthDate(IntPtr metadata, int year, int month, int day);
        [DllImport("__Internal")]
        internal static extern void TuneAdSetDebugMode(IntPtr metadata, bool debugMode);
        [DllImport("__Internal")]
        internal static extern void TuneAdSetGender(IntPtr metadata, int genderCode);
        [DllImport("__Internal")]
        internal static extern void TuneSetLatitude(IntPtr metadata, double latitude);
        [DllImport("__Internal")]
        internal static extern void TuneSetLongitude(IntPtr metadata, double longitude);
        [DllImport("__Internal")]
        internal static extern void TuneSetAltitude(IntPtr metadata, double altitude);
        [DllImport("__Internal")]
        internal static extern void TuneSetCustomTargets(IntPtr metadata, string key, string value);
        [DllImport("__Internal")]
        internal static extern void TuneRelease(IntPtr obj);


        [DllImport ("__Internal")]
        internal static extern void TuneCacheInterstitial(string placement);
        [DllImport ("__Internal")]
        internal static extern void TuneCacheInterstitialWithMetadata(string placement, IntPtr metadata);
        [DllImport ("__Internal")]
        internal static extern void TuneShowInterstitial(string placement);
        [DllImport ("__Internal")]
        internal static extern void TuneShowInterstitialWithMetadata(string placement, IntPtr metadata);
        [DllImport ("__Internal")]
        internal static extern void TuneDestroyInterstitial();


        [DllImport ("__Internal")]
        internal static extern void TuneShowBanner(string placement);
        [DllImport ("__Internal")]
        internal static extern void TuneShowBannerWithMetadata(string placement, IntPtr request);
        [DllImport ("__Internal")]
        internal static extern void TuneShowBannerWithPosition(string placement, IntPtr request, MATBannerPosition position);
        [DllImport ("__Internal")]
        internal static extern void TuneHideBanner();
        [DllImport ("__Internal")]
        internal static extern void TuneLayoutBanner();
        [DllImport ("__Internal")]
        internal static extern void TuneDestroyBanner();

        #endif
    }
}
