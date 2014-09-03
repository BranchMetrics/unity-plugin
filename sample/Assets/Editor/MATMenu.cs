using UnityEngine;
using UnityEditor;
using System;

public class MATMenu : EditorWindow 
{
    [MenuItem ("MobileAppTracker/Create MATDelegate Object", false, 100)] //Create MATDelegate
    static void InitMATDelegate () 
    {
        if(GameObject.Find("MobileAppTracker") == null)
        {
            var obj = new GameObject("MobileAppTracker");
            obj.AddComponent("MATDelegateScript");
        }
    }

    [MenuItem ("MobileAppTracker/Configure Android", false, 101)] //Open menu for Android configuration
    static void InitConfigForAndroid () 
    {
        EditorWindow.GetWindow (typeof (MATAndroidConfigMenu));
    }

    [MenuItem ("MobileAppTracker/Plugin Guide", false, 200)] //Open menu for Android configuration
    static void InitPluginGuide () 
    {
        Application.OpenURL("https://developers.mobileapptracking.com/unity-plugin/");
    }
}