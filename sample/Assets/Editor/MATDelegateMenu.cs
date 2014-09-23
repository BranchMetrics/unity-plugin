using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

public class MATDelegateMenu : EditorWindow 
{
    void OnGUI()
    {
        GUI.skin.label.wordWrap = true;
        GUI.skin.button.wordWrap = true;

        GUILayout.Label("\nCreate and use a MobileAppTracker object to track information that is received from the server " +
            "after calls to MATBinding.MeasureSession or any of the MATBinding.MeasureAction functions. This can be used to debug server requests.\n\n" +
            "Before receiving information from the server, you must allow delegates and enable debug mode. This can be done with the following " +
            "lines of code: \n\n" +
            "MATBinding.SetDelegate(true); \n" +
            "MATBinding.SetDebugMode(true); \n\n" +
            "When using Windows Phone 8, the following code is also required: \n\n" +
            "#if UNITY_WP8 \n" +
            "MATBinding.SetMATResponse(new SampleMATResponse());\n" +
            "#endif" +
            "\n\nPlease read the instructions in MATDelegateScript for more information about setting up a WP8 SampleMATResponse object. \n\n\n\n");

        if(GUILayout.Button("create MobileAppTracker"))
        {
            if(GameObject.Find("MobileAppTracker") == null)
            {
                var obj = new GameObject("MobileAppTracker");
                obj.AddComponent("MATDelegateScript");
            }
        }
    }
}