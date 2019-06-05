using System;

namespace TuneSDK
{
    public interface ITuneNativeBridge
    {
        void MeasureSession();

        void MeasureEvent(string eventName);

        void MeasureEvent(TuneEvent tuneEvent);

        void UnregisterDeeplinkListener();

        void RegisterDeeplinkListener();

        void RegisterCustomTuneLinkDomain(string domainSuffix);

        bool IsTuneLink(string appLinkUrl);

        bool IsPayingUser();

        string GetTuneId();

        string GetOpenLogId();

        bool IsPrivacyProtectedDueToAge();

        void SetAppAdTracking(bool adTrackingEnabled);

        void SetExistingUser(bool isExistingUser);

        void SetFacebookEventLogging(bool fbEventLogging, bool limitEventAndDataUsage);

        void SetPayingUser(bool isPayingUser);

        bool SetPrivacyProtectedDueToAge(bool isPrivacyProtected);

        void SetPreloadedApp(TunePreloadData preloadData);

        // iOS methods
        void AutomateIapEventMeasurement(bool automate);

        void SetJailbroken(bool isJailbroken);
    }
}
