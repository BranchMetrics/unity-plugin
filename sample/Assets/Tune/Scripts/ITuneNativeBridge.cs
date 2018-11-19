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

        void SetAge(int age);

        void SetAppAdTracking(bool adTrackingEnabled);

        void SetExistingUser(bool isExistingUser);

        void SetFacebookEventLogging(bool fbEventLogging, bool limitEventAndDataUsage);

        void SetFacebookUserId(string facebookUserId);

        void SetGender(int gender);

        void SetPayingUser(bool isPayingUser);

        void SetGoogleUserId(string googleUserId);

        void SetPhoneNumber(string phoneNumber);

        bool SetPrivacyProtectedDueToAge(bool isPrivacyProtected);

        void SetTwitterUserId(string twitterUserId);

        void SetUserEmail(string userEmail);

        void SetUserId(string userId);

        void SetUserName(string userName);

        void SetLocation(double latitude, double longitude, double altitude);

        void DisableLocationAutoCollection();

        void SetPreloadedApp(TunePreloadData preloadData);

        // Android methods
        void CollectEmails();

        void ClearEmails();

        // iOS methods
        void AutomateIapEventMeasurement(bool automate);

        void SetJailbroken(bool isJailbroken);
    }
}
