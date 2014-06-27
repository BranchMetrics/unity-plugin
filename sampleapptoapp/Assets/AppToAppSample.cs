using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class AppToAppSample : MonoBehaviour {

    void Awake () {
        //initNativeCode("877", "5afe3bc434184096023e3d8b2ae27e1c"); // Android
        MATBinding.Init("877", "8c14d6bbe466b65211e781d62e301eec"); // iOS
        MATBinding.SetDebugMode(true);
        MATBinding.SetDelegate(true);

        #if UNITY_IPHONE
        MATBinding.SetAppleAdvertisingIdentifier(iPhone.advertisingIdentifier, iPhone.advertisingTrackingEnabled);
        #endif

        return;
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 350, 100), "App-to-App Tracking Click"))
        {
            print("App-to-App Tracking button clicked");

            string targetAppId = "com.hasoffers.MATUnitySample";
            string advertiserId = "877";
            string offerId = "244760";
            //string offerId = "245086"; // Android
            string publisherId = "10852";
            bool shouldRedirect = true;

            MATBinding.StartAppToAppTracking(targetAppId, advertiserId, offerId, publisherId, shouldRedirect);
        }
    }
}