using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

public class MATObjectMenu : EditorWindow 
{
    void OnGUI()
    {
        GUI.skin.label.wordWrap = true;
        GUI.skin.button.wordWrap = true;

        GUILayout.Label("\nA GameObject named \"MobileAppTracker\" with MATDelegate.cs and MATBinding.cs is " +
            "required in order for MAT to receive server callbacks and Google Advertising Id, respectively.\n\n" +
            "After calls to MATBinding.MeasureSession or any of the MATBinding.MeasureAction functions, you may debug server requests with the MATDelegateScript functions.\n\n" +
            "Before receiving information from the server, you must enable delegates and enable debug mode. This can be done with the following " +
            "lines of code: \n\n" +
            "MATBinding.SetDelegate(true); \n" +
            "MATBinding.SetDebugMode(true); \n\n" +
            "When using Windows Phone 8, the following code is also required: \n\n" +
            "#if UNITY_WP8 \n" +
            "MATBinding.SetMATResponse(new SampleMATResponse());\n" +
            "#endif" +
            "\n\nPlease read the instructions in MATDelegate.cs for more information about setting up a WP8 SampleMATResponse object. \n\n");

        if(GUILayout.Button("Create MobileAppTracker GameObject"))
        {
            if(GameObject.Find("MobileAppTracker") == null)
            {
                var obj = new GameObject("MobileAppTracker");
                obj.AddComponent("MATDelegate");
                obj.AddComponent("MATBinding");
            }
        }
    }
}