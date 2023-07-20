This repo is deprecated

# Legacy TUNE Unity Plugin v7.2.0

[Get the Branch Unity SDK here!](https://github.com/BranchMetrics/unity-branch-deep-linking-attribution)

The TUNE plugin for Unity provides application session and event logging functionality via the TUNE native SDKs.

* TUNE Android SDK 6.1.2
* TUNE iOS SDK 6.2.0

This plugin was built and tested with `Unity version 2018.2.11f1 LTS`.

## Dependencies

Unity 2018+

[Google Unity Jar Resolver](https://github.com/googlesamples/unity-jar-resolver).  This plugin handles Android and iOS dependency management for the TUNE plugin.

For iOS builds:

* Xcode
* [Cocoapods](https://cocoapods.org/)

For Android builds:

* Android Studio

## Install Tune Unity Plugin

Download `Tune.unitypackage` from the releases section.

While your Unity project is open, double click the unitypackage.  It should install.  Otherwise, you can import it via the menu, `Assets` -> `Import Package` -> `Custom Package`.

![Import Tune plugin](/images/ImportTune.png)

Create a new Tune script and attach it to an object.

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TuneSDK;

public class TuneScript : MonoBehaviour {

    private void Awake()
    {
    	// verbose debug messages
        //Tune.SetDebugMode(true);
        
        Tune.Init("your_advertiser_id", "your_conversion_key");
        Tune.MeasureSession();
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause) {
            Tune.MeasureSession();
        }
    }

    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
        
    }
}
```

Sample script in an example project:

![Sample script in Assets](/images/TuneScriptInAssets.png)

Sample script attached to an object:

![Sample script attached to an Object](/images/TuneScriptAttachedToCube.png)

## Build and export project

Switch platform to **Android** or **iOS**. Go into **Player Settings** and change the default Android package name or iOS bundle identifier to something other than `com.Company.ProductName`.

Sample Android build settings:

![Sample build for Android](/images/BuildMenu.png)

Sample Android package name change:

![Sample change package name](/images/ChangingPackageName.png)

For Android, Unity will export an apk file.

For iOS, Unity will export a folder with all the project files.

## Running Android

From a terminal, use `adb install` to install the apk file to a device.

[adb documentation](https://developer.android.com/studio/command-line/adb)

If you enabled debug, you can see Tune calls in the logs by using `adb logcat`.

## Running iOS 

Find and open the `Unity-iPhone.xcworkspace` file.

In the `Unity-iPhone` target, set the Apple developer to your developer account.

![Set Apple developer](/images/SetAppleDeveloper.png)

Debug on a device.  Unity projects do not support simulator.

## Using the bundled Sample App

Open **/sample/Assets/Tune/Scenes/TUNESampleScene.unity** in Unity.

Build and export as you would your own project.  The TUNE sample app displays a menu that uses the TUNE Unity plugin's various functionalities. To see samples of how to use the plugin in your own code, see [our sample script](sdk-unity/sample/Assets/Tune/Scripts/TuneSample.cs).


