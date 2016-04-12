using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TuneSDK
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif

    public class TuneSettings : ScriptableObject
    {
        [MenuItem ("TUNE/Setup", false, 100)] //Open menu for Android configuration
        static void Setup()
        {
            Selection.activeObject = Instance;
        }

        [MenuItem ("TUNE/Plugin Guide", false, 200)] //Open menu for Android configuration
        static void OpenPluginGuide()
        {
            Application.OpenURL("https://developers.mobileapptracking.com/unity-plugin/");
        }

        private static TuneSettings instance;

        static TuneSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = CreateInstance<TuneSettings>();
                }
                return instance;
            }
        }
    }
}