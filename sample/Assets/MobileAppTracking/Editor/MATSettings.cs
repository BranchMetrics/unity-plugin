using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MATSDK {

#if UNITY_EDITOR
    [InitializeOnLoad]
#endif

    public class MATSettings : ScriptableObject
    {
        [MenuItem ("MobileAppTracking/Setup", false, 100)] //Open menu for Android configuration
        static void Setup() 
        {
            Selection.activeObject = Instance;
        }

        [MenuItem ("MobileAppTracking/Plugin Guide", false, 200)] //Open menu for Android configuration
        static void OpenPluginGuide() 
        {
            Application.OpenURL("https://developers.mobileapptracking.com/unity-plugin/");
        }

        private static MATSettings instance;

        static MATSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = CreateInstance<MATSettings>();
                }
                return instance;
            }
        }
    }
}