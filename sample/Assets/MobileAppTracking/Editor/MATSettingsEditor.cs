using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEditor;

namespace MATSDK
{
    [CustomEditor(typeof(MATSettings))]
    public class MATSettingsEditor : Editor {
        // Minimum version of Google Play Services required for Google Advertising Id collection
        private long MinGPSVersion = 4030530;

        private string sOk = "OK";
        private string sCancel = "Cancel";
        private string sSuccess = "Success";
        private string sWarning = "Warning";

        public override void OnInspectorGUI()
        {
            SetupUI();
        }

        private void SetupUI()
        {
            GUI.skin.label.wordWrap = true;
            GUI.skin.button.wordWrap = true;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Android", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.HelpBox("For Android builds, the MAT plugin requires a copy of the Google Play Services 4.0+ library.\n",
                MessageType.Info);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Import Google Play Services"))
            {
                ImportGooglePlayServices();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Delegate Callbacks", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("You may debug server requests with the MATDelegate.cs functions by enabling delegate callbacks: \n\n" +
                "MATBinding.SetDelegate(true); \n\n" +
                "The MATDelegate.cs script must be attached to a GameObject named \"MobileAppTracker\" in order to receive the callbacks.\n");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create MobileAppTracker GameObject"))
            {
                if (GameObject.Find("MobileAppTracker") == null)
                {
                    var obj = new GameObject("MobileAppTracker");
                    obj.AddComponent<MATDelegate>();
                }
                else
                {
                    EditorUtility.DisplayDialog("MobileAppTracker exists", "A MobileAppTracker GameObject already exists", sOk);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void ImportGooglePlayServices()
        {
            string sdkPath = GetAndroidSdkPath();
            string gpsLibPath = FixSlashes(sdkPath) + FixSlashes("/extras/google/google_play_services/libproject/google-play-services_lib");
            string gpsLibVersion = gpsLibPath + FixSlashes("/res/values/version.xml");
            string gpsLibDestDir = FixSlashes("Assets/Plugins/Android/google-play-services_lib");
    
            // Check that Android SDK is there
            if (!HasAndroidSdk())
            {
                Debug.LogError("Android SDK not found.");
                EditorUtility.DisplayDialog("Android SDK not found",
                    "The Android SDK path was not found. Please configure it in Unity > Edit > Preferences > External Tools.",
                    sOk);
                return;
            }
            
            // Check that the Google Play Services lib project is there
            if (!System.IO.Directory.Exists(gpsLibPath) || !System.IO.File.Exists(gpsLibVersion))
            {
                Debug.LogError("Google Play Services lib project not found at: " + gpsLibPath);
                EditorUtility.DisplayDialog("Google Play Services library not found",
                    "Google Play Services could not be found in your Android SDK installation.\n" +
                    "Install from the SDK Manager under Extras > Google Play Services.", sOk);
                return;
            }
            
            // Check GPS lib version for 4.0+ to support Advertising Id
            if (!CheckForLibVersion(gpsLibVersion))
            {
                return;
            }
            
            // Create Assets/Plugins and Assets/Plugins/Android if not existing
            CheckDirExists("Assets/Plugins");
            CheckDirExists("Assets/Plugins/Android");

            // Delete any existing google_play_services_lib destination directory
            DeleteDirIfExists(gpsLibDestDir);
            
            // Copy Google Play Services library
            FileUtil.CopyFileOrDirectory(gpsLibPath, gpsLibDestDir);
            
            // Refresh assets, and we're done
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog(sSuccess,
                "Google Play Services imported successfully to Assets/Plugins/Android.", sOk);
        }
        
        private bool CheckForLibVersion(string gpsLibVersionFile)
        {
            var root = new XmlDocument();
            root.Load(gpsLibVersionFile);

            // Read the version number from the res/values/version.xml
            var versionNode = root.SelectSingleNode("resources/integer[@name='google_play_services_version']");
            if (versionNode != null)
            {
                var version = versionNode.InnerText;
                if (version == null || version == "")
                {
                    Debug.LogError("Google Play Services lib version could not be read from: " + gpsLibVersionFile);
                    return EditorUtility.DisplayDialog(sWarning,
                        string.Format(
                            "The version of your Google Play Services could not be determined. Please make sure it is " +
                            "at least version {0}. Continue?",
                            MinGPSVersion),
                        sOk, sCancel);
                } 
                else
                {
                    // Convert version to long and compare to min version
                    long versionNum = System.Convert.ToInt64(version);
                    if (versionNum < MinGPSVersion)
                    {
                        return EditorUtility.DisplayDialog(sWarning,
                            string.Format(
                                "Your version of Google Play Services does not support Google Advertising Id." +
                                "Please update your Google Play Services to 4.0+." +
                                "Your version: {0}; required version: {1}). Proceed anyway?",
                                versionNum,
                                MinGPSVersion),
                            sOk, sCancel);
                    }
                }
            }
            return true;
        }
        
        private string GetAndroidSdkPath()
        {
            string sdkPath = EditorPrefs.GetString("AndroidSdkRoot");
            // Remove trailing slash if exists
            if (sdkPath != null && (sdkPath.EndsWith("/") || sdkPath.EndsWith("\\")))
            {
                sdkPath = sdkPath.Substring(0, sdkPath.Length - 1);
            }
            return sdkPath;
        }
        
        private bool HasAndroidSdk()
        {
            string sdkPath = GetAndroidSdkPath();
            return sdkPath != null && sdkPath.Trim() != "" && System.IO.Directory.Exists(sdkPath);
        }

        private void CheckDirExists(string dir)
        {
            dir = dir.Replace("/", System.IO.Path.DirectorySeparatorChar.ToString());
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
        }
        
        private void DeleteDirIfExists(string dir)
        {
            if (System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.Delete(dir, true);
            }
        }
        
        private string FixSlashes(string path)
        {
            return path.Replace("/", System.IO.Path.DirectorySeparatorChar.ToString());
        }
    }
}