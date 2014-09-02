using UnityEngine;
using UnityEditor;
using System;

public class MATMenu : EditorWindow 
{
    [MenuItem ("MobileAppTracker/Create MATDelegate Object")] //Create MATDelegate
    static void InitMATDelegate () 
    {
        if(GameObject.Find("MobileAppTracker") == null)
        {
            var obj = new GameObject("MobileAppTracker");
            obj.AddComponent("MATDelegateScript");
        }
    }

    [MenuItem ("MobileAppTracker/Configure Android")] //Open menu for Android configuration
    static void InitConfigForAndroid () 
    {
        var window = EditorWindow.GetWindow (typeof (MATAndroidConfigMenu));
        window.position = new Rect(100,40,250,200);
    }
}