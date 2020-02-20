using System;
using System.Runtime.InteropServices;

namespace TuneSDK
{
#if UNITY_IOS

    public class TuneIOS : ITuneNativeBridge
    {
        public TuneIOS(string advertiserId, string conversionKey, string packageName)
        {
            if (packageName != null) {
                TuneIOS.TuneInitWithPackageName(advertiserId, conversionKey, packageName);
            } else {
                TuneIOS.TuneInit(advertiserId, conversionKey);
            }
        }

        public void AutomateIapEventMeasurement(bool automate)
        {
            TuneIOS.TuneAutomateIapEventMeasurement(automate);
        }

        public string GetTuneId()
        {
            return TuneIOS.TuneGetTuneId();
        }

        public string GetOpenLogId()
        {
            return TuneIOS.TuneGetOpenLogId();
        }

        public bool IsPayingUser()
        {
            return TuneIOS.TuneIsPayingUser();
        }

        public bool IsPrivacyProtectedDueToAge()
        {
            return TuneIOS.TuneIsPrivacyProtectedDueToAge();
        }

        public bool IsTuneLink(string appLinkUrl)
        {
            return TuneIOS.TuneIsTuneLink(appLinkUrl);
        }

        public void MeasureEvent(string eventName)
        {
            TuneIOS.TuneMeasureEventName(eventName);
        }

        public void MeasureEvent(TuneEvent tuneEvent)
        {
            int itemCount = null == tuneEvent.eventItems ? 0 : tuneEvent.eventItems.Length;
            TuneEventIos eventIos = new TuneEventIos(tuneEvent);

            byte[] receiptBytes = null == tuneEvent.receipt ? null : System.Convert.FromBase64String(tuneEvent.receipt);
            int receiptByteCount = null == receiptBytes ? 0 : receiptBytes.Length;

            // Convert TuneItem to C-marshallable struct TuneItemIos
            TuneItemIos[] items = new TuneItemIos[itemCount];
            for (int i = 0; i < itemCount; i++) {
                items[i] = new TuneItemIos(tuneEvent.eventItems[i]);
            }

            TuneIOS.TuneMeasureEvent(eventIos, items, itemCount, receiptBytes, receiptByteCount);
        }

        public void MeasureSession()
        {
            TuneIOS.TuneMeasureSession();
        }

        public void RegisterCustomTuneLinkDomain(string domainSuffix)
        {
            TuneIOS.TuneRegisterCustomTuneLinkDomain(domainSuffix);
        }

        public void RegisterDeeplinkListener()
        {
            TuneIOS.TuneRegisterDeeplinkListener();
        }

        public void SetAppAdTracking(bool adTrackingEnabled)
        {
            TuneIOS.TuneSetAppAdTracking(adTrackingEnabled);
        }

        public void SetExistingUser(bool isExistingUser)
        {
            TuneIOS.TuneSetExistingUser(isExistingUser);
        }

        public void SetFacebookEventLogging(bool fbEventLogging, bool limitEventAndDataUsage)
        {
            TuneIOS.TuneSetFacebookEventLogging(fbEventLogging, limitEventAndDataUsage);
        }

        public void SetJailbroken(bool isJailbroken)
        {
            TuneIOS.TuneSetJailbroken(isJailbroken);
        }

        public void SetPayingUser(bool isPayingUser)
        {
            TuneIOS.TuneSetPayingUser(isPayingUser);
        }

        public void SetPreloadedApp(TunePreloadData preloadData)
        {
            TuneIOS.TuneSetPreloadData(preloadData);
        }

        public bool SetPrivacyProtectedDueToAge(bool isPrivacyProtected)
        {
            TuneIOS.TuneSetPrivacyProtectedDueToAge(isPrivacyProtected);
            return true;
        }

        public void UnregisterDeeplinkListener()
        {
            TuneIOS.TuneUnregisterDeeplinkListener();
        }

        // Helper function to convert DateTime to Unix timestamp for iOS
        private string GetDateTimeString(DateTime dateTime)
        {
            // datetime starts in 1970
            DateTime datetime = new DateTime(1970, 1, 1);
            double millis = new TimeSpan(dateTime.Ticks).TotalMilliseconds;
            double millisFrom1970 = millis - (new TimeSpan(datetime.Ticks)).TotalMilliseconds;
            return millisFrom1970.ToString();
        }

        // Helper function to convert string to DateTime for iOS
        private DateTime GetDateTimeFromString(string dateString)
        {
            double unixTimeMillis = Convert.ToDouble(dateString);
            // Add ms from date to ms of 1/1/1970
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(unixTimeMillis);
        }

        // Bridge to Obj-C methods
        [DllImport("__Internal")]
        internal static extern void TuneInit(string advertiserId, string conversionKey);

        [DllImport("__Internal")]
        internal static extern void TuneInitWithPackageName(string advertiserId, string conversionKey, string packageName);

        [DllImport("__Internal")]
        internal static extern void TuneAutomateIapEventMeasurement(bool automate);

        [DllImport("__Internal")]
        internal static extern void TuneSetFacebookEventLogging(bool enable, bool limit);

        [DllImport("__Internal")]
        internal static extern void TuneSetDebugMode(bool enable);

        [DllImport("__Internal")]
        internal static extern void TuneSetAppAdTracking(bool enable);
        [DllImport("__Internal")]
        internal static extern void TuneSetExistingUser(bool isExisting);
        [DllImport("__Internal")]
        internal static extern void TuneSetPayingUser(bool isPaying);
        [DllImport("__Internal")]
        internal static extern void TuneSetJailbroken(bool isJailbroken);

        [DllImport("__Internal")]
        internal static extern void TuneSetPreloadData(TunePreloadData preloadData);

        [DllImport("__Internal")]
        internal static extern void TuneMeasureEventName(string eventName);
        [DllImport("__Internal")]
        internal static extern void TuneMeasureEvent(TuneEventIos tuneEvent, TuneItemIos[] eventItems, int eventItemCount, Byte[] receipt, int receiptByteCount);

        [DllImport("__Internal")]
        internal static extern void TuneRegisterDeeplinkListener();
        [DllImport("__Internal")]
        internal static extern void TuneUnregisterDeeplinkListener();
        [DllImport("__Internal")]
        internal static extern bool TuneIsTuneLink(string linkUrl);
        [DllImport("__Internal")]
        internal static extern void TuneRegisterCustomTuneLinkDomain(string domain);

        [DllImport("__Internal")]
        internal static extern void TuneMeasureSession();

        [DllImport("__Internal")]
        internal static extern string TuneGetTuneId();
        [DllImport("__Internal")]
        internal static extern string TuneGetOpenLogId();
        [DllImport("__Internal")]
        internal static extern bool TuneIsPayingUser();

        [DllImport("__Internal")]
        internal static extern void TuneSetPrivacyProtectedDueToAge(bool isPrivacyProtected);
        [DllImport("__Internal")]
        internal static extern bool TuneIsPrivacyProtectedDueToAge();
    }

    #endif
}
