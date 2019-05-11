using System;
using UnityEngine;
using TuneSDK;

/// <para>
/// This class demonstrates the basic features of the TUNE Unity Plugin and
/// its ability to have one project work with Android, iOS, and Windows Phone 8.
/// </para>
public class TuneSample : MonoBehaviour {
    string TUNE_ADVERTISER_ID = null;
    string TUNE_CONVERSION_KEY = null;
    string TUNE_PACKAGE_NAME = null;

    static int titleFontSize = 50;
    static bool isTitleBold = true;

    Vector2 scrollPosition = Vector2.zero;

    void Awake()
    {
        TUNE_ADVERTISER_ID = "877";
        TUNE_CONVERSION_KEY = "8c14d6bbe466b65211e781d62e301eec";
        TUNE_PACKAGE_NAME = "com.hasoffers.unitytestapp";

        print(Tune.GetVersion());

        Tune.SetDebugMode(true);
        Tune.Init(TUNE_ADVERTISER_ID, TUNE_CONVERSION_KEY, TUNE_PACKAGE_NAME);

        // throws errors in log
        //Tune.SetFacebookEventLogging(true, false);

        Tune.RegisterDeeplinkListener();
        Tune.AutomateIapEventMeasurement(true);
        Tune.SetPrivacyProtectedDueToAge(true);

        print("Awake called: " + TUNE_ADVERTISER_ID + ", " + TUNE_CONVERSION_KEY);
        return;
    }

    void Update()  // make it scroll with the user's finger
    {
        if (Input.touchCount == 0) return;
        Touch touch = Input.touches[0];
        if (touch.phase == TouchPhase.Moved)
        {
            // the float comparison makes no sense
            float dt = Time.deltaTime / touch.deltaTime;
            if (dt == 0 || float.IsNaN(dt) || float.IsInfinity(dt))
                dt = 1.0f;
            Vector2 glassDelta = touch.deltaPosition * dt;

            scrollPosition.y += glassDelta.y;
        }
    }

    void OnGUI()
    {
        GUIStyle headingLabelStyle = new GUIStyle();
        headingLabelStyle.fontStyle = isTitleBold ? FontStyle.Bold : FontStyle.Normal;
        headingLabelStyle.fontSize = titleFontSize;
        headingLabelStyle.alignment = TextAnchor.MiddleCenter;
        headingLabelStyle.normal.textColor = Color.white;

        GUI.skin.button.fontSize = 40;

        GUI.Label(new Rect(10, 5, Screen.width - 20, Screen.height / 10), "TUNE Unity Test App", headingLabelStyle);

        scrollPosition = GUI.BeginScrollView(new Rect(10, 5 + Screen.height / 10, Screen.width - 20, Screen.height), scrollPosition, new Rect(10, 0, Screen.width - 20, Screen.height * 1.2f), GUIStyle.none, GUIStyle.none);

        if (GUI.Button(new Rect(10, 3 * Screen.height / 10, Screen.width - 20, Screen.height / 10), "Measure Session"))
        {
            print("Measure Session clicked");
            Tune.MeasureSession();
        }

        else if (GUI.Button(new Rect(10, 4 * Screen.height / 10, Screen.width - 20, Screen.height / 10), "Measure Event"))
        {
            print("Measure Event clicked");
            Tune.MeasureEvent("evt11");
        }

        else if (GUI.Button(new Rect(10, 5 * Screen.height / 10, Screen.width - 20, Screen.height / 10), "Measure Event With Event Items"))
        {
            print("Measure Event With Event Items clicked");

            TuneItem item1 = new TuneItem("subitem1");
            item1.unitPrice = 5;
            item1.quantity = 5;
            item1.revenue = 3;
            item1.attribute2 = "attrValue12";
            item1.attribute3 = "attrValue13";
            item1.attribute4 = "attrValue14";
            item1.attribute5 = "attrValue15";

            TuneItem item2 = new TuneItem("subitem2");
            item2.unitPrice = 1;
            item2.quantity = 3;
            item2.revenue = 1.5;
            item2.attribute1 = "attrValue21";
            item2.attribute3 = "attrValue23";
            TuneItem[] eventItems = { item1, item2 };

            TuneEvent tuneEvent = new TuneEvent("purchase");
            tuneEvent.revenue = 10;
            tuneEvent.currencyCode = "AUD";
            tuneEvent.advertiserRefId = "ref222";
            tuneEvent.attribute1 = "test_attribute1";
            tuneEvent.attribute2 = "test_attribute2";
            tuneEvent.attribute3 = "test_attribute3";
            tuneEvent.attribute4 = "test_attribute4";
            tuneEvent.attribute5 = "test_attribute5";
            tuneEvent.contentType = "test_contentType";
            tuneEvent.contentId = "test_contentId";
            tuneEvent.date1 = DateTime.UtcNow;
            tuneEvent.date2 = DateTime.UtcNow.Add(new TimeSpan((new DateTime(2, 1, 1).Ticks)));
            tuneEvent.level = 3;
            tuneEvent.quantity = 2;
            tuneEvent.rating = 4.5;
            tuneEvent.searchString = "test_searchString";
            tuneEvent.eventItems = eventItems;
            // transaction state may be set to the value received from the iOS/Android app store.
            tuneEvent.transactionState = 1;

#if UNITY_IOS
            tuneEvent.receipt = getSampleiTunesIAPReceipt();
#endif

            Tune.MeasureEvent(tuneEvent);
        }

        else if (GUI.Button(new Rect(10, 6 * Screen.height / 10, Screen.width - 20, Screen.height / 10), "Test Setter Methods"))
        {
            print("Test Setter Methods clicked");
            Tune.SetAppAdTracking(true);
            Tune.SetDebugMode(true);
            Tune.SetExistingUser(false);
            Tune.SetPayingUser(true);

            // iOS-specific Features
#if UNITY_IOS
            Tune.SetJailbroken(false);
#endif

            // Android-specific Features
#if UNITY_ANDROID
            // TODO: consider testing emails api
#endif

            TunePreloadData pd = new TunePreloadData("1122334455");
            pd.advertiserSubAd = "some_adv_sub_ad_id";
            pd.publisherSub3 = "some_pub_sub3";
            Tune.SetPreloadedApp(pd);
        }

        else if (GUI.Button(new Rect(10, 7 * Screen.height / 10, Screen.width - 20, Screen.height / 10), "Test Getter Methods"))
        {
            print("Test Getter Methods clicked");
            print("isPayingUser = " + Tune.GetIsPayingUser());
            print("tuneId     = " + Tune.GetTuneId());

            string openLogId = Tune.GetOpenLogId();
            if (openLogId != null) { 
                print("openLogId = " + openLogId);
            }

            print ("privacyProtected: " + Tune.IsPrivacyProtectedDueToAge());
        }

        GUI.EndScrollView();
    }

    public static string getSampleiTunesIAPReceipt ()
    {
        return "dGhpcyBpcyBhIHNhbXBsZSBpb3MgYXBwIHN0b3JlIHJlY2VpcHQ=";
    }
}
