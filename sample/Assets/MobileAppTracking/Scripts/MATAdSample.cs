using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MATSDK;

/// <para>
/// This class demonstrates the basic features of the MAT Unity Plugin and
/// its ability to have one project work with Android, iOS, and Windows Phone 8.
/// </para>
public class MATAdSample : MonoBehaviour {
    string MAT_ADVERTISER_ID = null;
    string MAT_CONVERSION_KEY = null;
    string MAT_PACKAGE_NAME = null;

    Vector2 scrollPosition = Vector2.zero;

    int numButtons = 0;

    int topMargin = 5;
    int bottomMargin = 5;

    void Awake ()
    {
        MAT_ADVERTISER_ID = "877";
        MAT_CONVERSION_KEY = "40c19f41ef0ec2d433f595f0880d39b9";
        MAT_PACKAGE_NAME = "edu.self.AtomicDodgeBallLite";

        print ("Awake called: " + MAT_ADVERTISER_ID + ", " + MAT_CONVERSION_KEY);

        return;
    }

    void OnGUI ()
    {
        GUIStyle headingLabelStyle = new GUIStyle();
        headingLabelStyle.fontStyle = FontStyle.Bold;
        headingLabelStyle.fontSize = 50;
        headingLabelStyle.alignment = TextAnchor.MiddleCenter;
        headingLabelStyle.normal.textColor = Color.white;
        
        float titleHeight = Screen.height / 10;
        GUI.Label(new Rect(10, topMargin, Screen.width - 20, titleHeight), "MAT Unity Test App", headingLabelStyle);

        GUI.skin.button.fontSize = 40;

        GUI.skin.verticalScrollbar.fixedWidth = Screen.width * 0.05f;
        GUI.skin.verticalScrollbarThumb.fixedWidth = Screen.width * 0.05f;

        float scrollViewContentHeight = (float)(numButtons * 0.125) * Screen.height;

        scrollPosition = GUI.BeginScrollView(
            new Rect(0.1f * Screen.width, 0.15f * Screen.height, Screen.width - 0.1f * Screen.width, Screen.height - 0.15f * Screen.height - topMargin - bottomMargin), 
            scrollPosition, 
            new Rect(0.1f * Screen.width, 0.15f * Screen.height, Screen.width - 0.1f * Screen.width, scrollViewContentHeight));

        int buttonIndex = 0;
        float yPos = (float)(0.15f + buttonIndex * 0.125) * Screen.height;
        Rect rect = new Rect(0.1f * Screen.width, yPos,
            0.8f * Screen.width, 0.1f * Screen.height);

        if (GUI.Button(rect, "Start MAT SDK"))
        {
            print ("Start MAT SDK clicked");
            MATBinding.Init(MAT_ADVERTISER_ID, MAT_CONVERSION_KEY);
            MATBinding.SetPackageName(MAT_PACKAGE_NAME);
        }

        ++buttonIndex;
        yPos = (float)(0.15f + buttonIndex * 0.125) * Screen.height;
        rect = new Rect(0.1f * Screen.width, yPos,
            0.8f * Screen.width, 0.1f * Screen.height);
        if (GUI.Button(rect, "Set Delegate"))
        {
            print ("Set Delegate clicked");
            #if (UNITY_ANDROID || UNITY_IPHONE)
            MATBinding.SetDelegate(true);
            #endif
            #if UNITY_METRO
            MATBinding.SetMATResponse(new SampleWinMATResponse());
            #endif
        }

        ++buttonIndex;
        yPos = (float)(0.15f + buttonIndex * 0.125) * Screen.height;
        rect = new Rect(0.1f * Screen.width, yPos,
            0.8f * Screen.width, 0.1f * Screen.height);
        if (GUI.Button(rect, "Show Banner"))
        {
            print ("Show Banner");
            MATBinding.ShowBanner("place1");
        }

        ++buttonIndex;
        yPos = (float)(0.15f + buttonIndex * 0.125) * Screen.height;
        rect = new Rect(0.1f * Screen.width, yPos,
            0.8f * Screen.width, 0.1f * Screen.height);
        if (GUI.Button(rect, "Hide Banner"))
        {
            print ("Hide Banner");
            MATBinding.HideBanner();
        }

        ++buttonIndex;
        yPos = (float)(0.15f + buttonIndex * 0.125) * Screen.height;
        rect = new Rect(0.1f * Screen.width, yPos,
            0.8f * Screen.width, 0.1f * Screen.height);
        if (GUI.Button(rect, "Destroy Banner"))
        {
            print ("Destroy Banner");
            MATBinding.DestroyBanner();
        }

        ++buttonIndex;
        yPos = (float)(0.15f + buttonIndex * 0.125) * Screen.height;
        rect = new Rect(0.1f * Screen.width, yPos,
            0.8f * Screen.width, 0.1f * Screen.height);
        if (GUI.Button(rect, "Cache Interstitial"))
        {
            MATAdMetadata metadata = new MATAdMetadata();
            metadata.setBirthDate(DateTime.Today.AddYears(-45));
            metadata.setGender(MATAdGender.FEMALE);
            metadata.setLocation(120.8f, 234.5f, 579.6f);
            metadata.setDebugMode(true);

            HashSet<string> keywords = new HashSet<string>();
            keywords.Add("pro");
            keywords.Add("evening");
            metadata.setKeywords(keywords);

            Dictionary<string, string> customTargets = new Dictionary<string, string>();
            customTargets.Add("type", "game");
            customTargets.Add("subtype1", "adventure");
            customTargets.Add("subtype2", "action");
            metadata.setCustomTargets(customTargets);

            MATBinding.CacheInterstitial("place1", metadata);
        }

        ++buttonIndex;
        yPos = (float)(0.15f + buttonIndex * 0.125) * Screen.height;
        rect = new Rect(0.1f * Screen.width, yPos,
            0.8f * Screen.width, 0.1f * Screen.height);
        if (GUI.Button(rect, "Show Interstitial"))
        {
            print ("Show Interstitial");
            MATBinding.ShowInterstitial("place1");
        }

        ++buttonIndex;
        yPos = (float)(0.15f + buttonIndex * 0.125) * Screen.height;
        rect = new Rect(0.1f * Screen.width, yPos,
            0.8f * Screen.width, 0.1f * Screen.height);
        if (GUI.Button(rect, "Destroy Interstitial"))
        {
            print ("Destroy Interstitial");
            MATBinding.DestroyInterstitial();
        }

        GUI.EndScrollView();

        numButtons = buttonIndex + 1;

        // resize banner ad for current orientation 
        MATBinding.LayoutBanner();
    }
}
