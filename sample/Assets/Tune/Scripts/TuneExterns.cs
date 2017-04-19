using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;

namespace TuneSDK
{
    // Externs used by the iOS component.
    class TuneExterns
    {
        #if UNITY_IOS

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
        [DllImport ("__Internal")]
        internal static extern void TuneSetDeepLink(string deepLinkUrl);

        // Method to include publisher information in the Tune SDK requests
        [DllImport ("__Internal")]
        internal static extern void TuneSetPreloadData(TunePreloadData preloadData);

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
        internal static extern void TuneMeasureEvent(TuneEventIos tuneEvent, TuneItemIos[] eventItems, int eventItemCount, Byte[] receipt, int receiptByteCount);

        // Methods for deeplinking
        [DllImport ("__Internal")]
        internal static extern void TuneRegisterDeeplinkListener();
        [DllImport ("__Internal")]
        internal static extern void TuneUnregisterDeeplinkListener();
        [DllImport ("__Internal")]
        internal static extern bool TuneIsTuneLink(string linkUrl);
        [DllImport ("__Internal")]
        internal static extern void TuneRegisterCustomTuneLinkDomain(string domain);

        // Method to measure session
        [DllImport ("__Internal")]
        internal static extern void TuneMeasureSession();

        // Getter Methods
        [DllImport ("__Internal")]
        internal static extern string TuneGetTuneId();
        [DllImport ("__Internal")]
        internal static extern string TuneGetOpenLogId();
        [DllImport ("__Internal")]
        internal static extern bool TuneGetIsPayingUser();

        /* In-App Marketing functions */

        // Profile API
        [DllImport ("__Internal")]
        internal static extern void TuneRegisterCustomProfileString(string variableName);
        [DllImport ("__Internal")]
        internal static extern void TuneRegisterCustomProfileStringWithDefault(string variableName, string defaultValue);
        [DllImport ("__Internal")]
        internal static extern void TuneRegisterCustomProfileDate(string variableName);
        [DllImport ("__Internal")]
        internal static extern void TuneRegisterCustomProfileDateWithDefault(string variableName, string defaultValue);
        [DllImport ("__Internal")]
        internal static extern void TuneRegisterCustomProfileNumber(string variableName);
        [DllImport ("__Internal")]
        internal static extern void TuneRegisterCustomProfileNumberWithDefaultInt(string variableName, int defaultValue);
        [DllImport ("__Internal")]
        internal static extern void TuneRegisterCustomProfileNumberWithDefaultDouble(string variableName, double defaultValue);
        [DllImport ("__Internal")]
        internal static extern void TuneRegisterCustomProfileNumberWithDefaultFloat(string variableName, float defaultValue);
        [DllImport ("__Internal")]
        internal static extern void TuneRegisterCustomProfileGeolocation(string variableName);
        [DllImport ("__Internal")]
        internal static extern void TuneRegisterCustomProfileGeolocationWithDefault(string variableName, double defaultLongitude, double defaultLatitude);

        [DllImport ("__Internal")]
        internal static extern void TuneSetCustomProfileString(string variableName, string value);
        [DllImport ("__Internal")]
        internal static extern void TuneSetCustomProfileDate(string variableName, string value);
        [DllImport ("__Internal")]
        internal static extern void TuneSetCustomProfileNumberWithInt(string variableName, int value);
        [DllImport ("__Internal")]
        internal static extern void TuneSetCustomProfileNumberWithDouble(string variableName, double value);
        [DllImport ("__Internal")]
        internal static extern void TuneSetCustomProfileNumberWithFloat(string variableName, float value);
        [DllImport ("__Internal")]
        internal static extern void TuneSetCustomProfileGeolocation(string variableName, double longitude, double latitude);

        [DllImport ("__Internal")]
        internal static extern string TuneGetCustomProfileString(string variableName);
        [DllImport ("__Internal")]
        internal static extern string TuneGetCustomProfileDate(string variableName);
        [DllImport ("__Internal")]
        internal static extern string TuneGetCustomProfileNumber(string variableName);
        [DllImport ("__Internal")]
        internal static extern string TuneGetCustomProfileGeolocation(string variableName);

        [DllImport ("__Internal")]
        internal static extern void TuneClearCustomProfileVariable(string variableName);
        [DllImport ("__Internal")]
        internal static extern void TuneClearAllCustomProfileVariables();

        // Power Hook API
        [DllImport ("__Internal")]
        internal static extern void TuneRegisterHookWithId(string hookId, string friendlyName, string defaultValue);
        [DllImport ("__Internal")]
        internal static extern string TuneGetValueForHookById(string hookId);
        [DllImport ("__Internal")]
        internal static extern void TuneSetValueForHookById(string hookId, string value);
        [DllImport ("__Internal")]
        internal static extern void TuneOnPowerHooksChanged(bool listenForPowerHooksChanged);

        // Experiment Details API
        [DllImport ("__Internal")]
        internal static extern string TuneGetPowerHookVariableExperimentDetails();
        [DllImport ("__Internal")]
        internal static extern string TuneGetInAppMessageExperimentDetails();

        // On First Playlist Downloaded API
        [DllImport ("__Internal")]
        internal static extern void TuneOnFirstPlaylistDownloaded(bool listenForFirstPlaylist);
        [DllImport ("__Internal")]
        internal static extern void TuneOnFirstPlaylistDownloadedWithTimeout(bool listenForFirstPlaylist, long timeout);

        // Push API
        [DllImport ("__Internal")]
        internal static extern bool TuneDidSessionStartFromTunePush();
        [DllImport ("__Internal")]
        internal static extern string TuneGetTuneCampaignIdForSession();
        [DllImport ("__Internal")]
        internal static extern string TuneGetTunePushIdForSession();

        // Segment API
        [DllImport ("__Internal")]
        internal static extern bool TuneIsUserInSegmentId(string segmentId);
        [DllImport ("__Internal")]
        internal static extern bool TuneIsUserInAnySegmentIds(string[] segmentIds, int segmentCount);
        [DllImport ("__Internal")]
        internal static extern void TuneForceSetUserInSegmentId(string segmentId, bool isInSegment);

        #endif
    }
}
