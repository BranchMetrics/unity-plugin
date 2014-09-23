using UnityEngine;
using UnityEditor;
using System;

public class MATMenu : EditorWindow 
{
    [MenuItem ("MobileAppTracker/Create MATDelegate Object", false, 100)] //Create MATDelegate
    static void InitMATDelegate () 
    {
        EditorWindow window = EditorWindow.GetWindow (typeof (MATDelegateMenu));
        int winWidth, winHeight;
        winWidth = 450;
        winHeight = 350;
        window.minSize = new Vector2(winWidth, winHeight);
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