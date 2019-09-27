using UnityEngine;

namespace TuneSDK
{
    /// <para>
    /// Tune.cs wraps Android and iOS TUNE SDK
    /// functions to be used within Unity. The functions can be called within any
    /// C# script by typing Tune.*.
    /// </para>
    public class Tune : MonoBehaviour
    {

        static ITuneNativeBridge instance = null;

        /// <para>
        /// Initializes relevant information about the advertiser and
        /// conversion key on startup of Unity game.
        /// </para>
        /// <param name="advertiserId">the TUNE advertiser ID for your app</param>
        /// <param name="conversionKey">the TUNE conversion key for your app</param>
        public static void Init(string advertiserId, string conversionKey)
        {
            Init(advertiserId, conversionKey, null);
        }

        /// <para>
        /// Initializes relevant information about the advertiser and
        /// conversion key on startup of Unity game.
        /// </para>
        /// <param name="advertiserId">the TUNE advertiser ID for your app</param>
        /// <param name="conversionKey">the TUNE conversion key for your app</param>
        /// <param name="packageName">the package name used when setting up the app in TUNE</param>
        public static void Init(string advertiserId, string conversionKey, string packageName)
        {
            if (!Application.isEditor && instance == null) {
                #if UNITY_ANDROID
                instance = new TuneAndroid(advertiserId, conversionKey, packageName);
                #endif

                #if UNITY_IOS
                instance = new TuneIOS(advertiserId, conversionKey, packageName);
                #endif
            }
        }

        /// <summary>
        /// Returns Tune Unity SDK version
        /// </summary>
        /// <returns>Version Number.</returns>
        public static string GetVersion() {
            return "7.1.0";
        }

        /// <para>
        /// Turns debug mode on or off.
        /// </para>
        /// <param name="debug">whether to enable debug output</param>
        public static void SetDebugMode(bool debug)
        {
            if (!Application.isEditor) {
                #if UNITY_ANDROID
                TuneAndroid.SetDebugMode(debug);
                #endif
                #if UNITY_IOS
                TuneIOS.TuneSetDebugMode(debug);
                #endif
            }
        }

        /// <para>
        /// Enable auto-measurement of successful in-app-purchase (IAP) transactions as "purchase" events in iOS.
        /// </para>
        /// <param name="automate">should auto-measure IAP transactions</param>
        public static void AutomateIapEventMeasurement(bool automate)
        {
            if (!Application.isEditor && instance != null) {
                instance.AutomateIapEventMeasurement(automate);
            }
        }

        /// <para>
        /// Event measurement function, by event name.
        /// </para>
        /// <param name="eventName">event name in TUNE system</param>
        public static void MeasureEvent(string eventName)
        {
            if (!Application.isEditor && instance != null) {
                instance.MeasureEvent(eventName);
            }
        }

        /// <para>
        /// Measures the event with TuneEvent.
        /// </para>
        /// <param name="tuneEvent">TuneEvent object of event values to measure</param>
        public static void MeasureEvent(TuneEvent tuneEvent)
        {
            if (!Application.isEditor && instance != null) {
                instance.MeasureEvent(tuneEvent);
            }
        }

        /// <para>
        /// Registering a deeplink listener will trigger an asynchronous calls to TuneListener
        /// </para>
        public static void RegisterDeeplinkListener()
        {
            if (!Application.isEditor && instance != null) {
                instance.RegisterDeeplinkListener();
            }
        }

        /// <para>
        /// Registering the deeplink listener
        /// </para>
        public static void UnregisterDeeplinkListener()
        {
            if (!Application.isEditor && instance != null) {
                instance.UnregisterDeeplinkListener();
            }
        }

        /// <para>
        /// Test if your custom Tune Link domain is registered with Tune.
        /// </para>
        /// <param name="linkUrl">url to test if it is a Tune Link. Must not be null.</param>
        public static bool IsTuneLink(string linkUrl)
        {
            if (!Application.isEditor && instance != null) {
                return instance.IsTuneLink(linkUrl);
            }

            return false;
        }

        /// <para>
        /// If you have set up a custom domain for use with Tune Links (cname to a *.tlnk.io domain), then register it with this method.
        /// </para>
        /// <param name="domain">domain which you are using for Tune Links. Must not be null.</param>
        public static void RegisterCustomTuneLinkDomain(string domain)
        {
            if (!Application.isEditor && instance != null) {
                instance.RegisterCustomTuneLinkDomain(domain);
            }
        }

        /// <para>
        /// Main session measurement function; this function should be called at every app open.
        /// </para>
        public static void MeasureSession()
        {
            if (!Application.isEditor && instance != null) {
                instance.MeasureSession();
            }
        }

        /// <para>Sets whether app-level ad tracking is enabled.</para>
        /// <param name="adTrackingEnabled">true if user has opted out of ad tracking at the app-level, false if not</param>
        public static void SetAppAdTracking(bool adTrackingEnabled)
        {
            if (!Application.isEditor && instance != null) {
                instance.SetAppAdTracking(adTrackingEnabled);
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
            if (!Application.isEditor && instance != null) {
                instance.SetExistingUser(isExistingUser);
            }
        }

        /// <para>
        /// Set whether the Tune events should also be logged to the Facebook SDK. This flag is ignored if the Facebook SDK is not present.
        /// </para>
        /// <param name="enable">Whether to send Tune events to FB as well</param>
        /// <param name="limit">Whether data such as that generated through FBAppEvents and sent to Facebook should be restricted from being used for other than analytics and conversions. Defaults to NO. This value is stored on the device and persists across app launches.</param>
        public static void SetFacebookEventLogging(bool enable, bool limit)
        {
            if (!Application.isEditor && instance != null) {
                instance.SetFacebookEventLogging(enable, limit);
            }
        }

        /// <para>
        /// Set whether the user is generating revenue for the app or not.
        /// If measureAction is called with a non-zero revenue, this is automatically set to true.
        /// </para>
        /// <param name="isPayingUser">true if the user is revenue-generating, false if not</param>
        public static void SetPayingUser(bool isPayingUser)
        {
            if (!Application.isEditor && instance != null) {
                instance.SetPayingUser(isPayingUser);
            }
        }

        /// <para>
        /// Set this device profile as privacy protected for the purposes of the protection of children
        /// from ad targeting and personal data collection. In the US this is part of the COPPA law.
        /// This method is related to SetAge(int).
        /// </para>
        /// <param name="isPrivacyProtected">True if privacy should be protected for this user.</param>
        /// <returns>True if age requirements are met.  For example, you cannot turn privacy protection "off" for children who meet the COPPA standard. On iOS, always returns true.</returns>
        public static bool SetPrivacyProtectedDueToAge(bool isPrivacyProtected)
        {
            if (!Application.isEditor && instance != null) {
                return instance.SetPrivacyProtectedDueToAge(isPrivacyProtected);
            }
            return false;
        }

        /// <para>
        /// Gets whether the user is revenue-generating or not.
        /// </para>
        /// <returns>true if the user has produced revenue, false if not</returns>
        public static bool GetIsPayingUser()
        {
            if (!Application.isEditor && instance != null) {
                return instance.IsPayingUser();
            }
            return false;
        }

        /// <para>
        /// Gets the TUNE ID generated on install.
        /// </para>
        /// <returns>TUNE ID</returns>
        public static string GetTuneId()
        {
            if (!Application.isEditor && instance != null) {
                return instance.GetTuneId();
            }

            return string.Empty;
        }

        /// <para>
        /// Gets the first TUNE open log ID.
        /// </para>
        /// <returns>first TUNE open log ID</returns>
        public static string GetOpenLogId()
        {
            if (!Application.isEditor && instance != null) {
                return instance.GetOpenLogId();
            }

            return string.Empty;
        }

        /// <para>
        /// Returns whether this device profile is flagged as privacy protected.
        /// </para>
        /// <returns>true if either the age is set to less than 13 or if SetPrivacyProtectedDueToAge(boolean) is set to true.</returns>
        public static bool IsPrivacyProtectedDueToAge()
        {
            if (!Application.isEditor && instance != null) {
                return instance.IsPrivacyProtectedDueToAge();
            }

            return false;
        }

        /*--------------------------iOS Specific Features--------------------------*/

        /// <para>
        /// Sets the jailbroken device flag.
        /// Does nothing if not an iOS device.
        /// </para>
        /// <param name="isJailbroken">The jailbroken device flag</param>
        public static void SetJailbroken(bool isJailbroken)
        {
            if (!Application.isEditor && instance != null) {
                instance.SetJailbroken(isJailbroken);
            }
        }

        /*----------------Android and iOS Platform-specific Features---------------*/

        /// <para>
        /// Sets the preloaded app attribution values (publisher information).
        /// Does nothing if not an Android or iOS device.
        /// </para>
        /// <param name="preloadData">Preloaded app attribution values (publisher information)</param>
        public static void SetPreloadedApp(TunePreloadData preloadData)
        {
            if (!Application.isEditor && instance != null) {
                instance.SetPreloadedApp(preloadData);
            }
        }
    }
}
