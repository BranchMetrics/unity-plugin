using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEditor;

namespace TuneSDK
{
    [InitializeOnLoad]
    [CustomEditor(typeof(TuneSettings))]
    public class TuneSettingsEditor : Editor {
        private static string AdvertiserIdKey = "TUNE_AdvertiserId";
        private static string ConversionKeyKey = "TUNE_ConversionKey";
        private static string PackageNameKey = "TUNE_PackageName";
        private static string ApiLevelKey = "TUNE_ApiLevel";

        private static string ShowDelegateFoldoutKey = "TUNE_ShowDelegate";

        private static string sOk = "OK";

        private static string advertiserId;
        private static string conversionKey;
        private static string packageName;
        private static string apiLevel;

        private static bool showDelegate;

        static TuneSettingsEditor()
        {
            LoadSettings();
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            LoadSettings();
            SetupUI();
            if (EditorGUI.EndChangeCheck())
                SaveSettings();
        }

        void OnDestroy()
        {
            SaveSettings();
        }

        private static void LoadSettings()
        {
            packageName = PlayerSettings.applicationIdentifier;
            if (EditorPrefs.HasKey(AdvertiserIdKey))
                advertiserId = EditorPrefs.GetString(AdvertiserIdKey);
            if (EditorPrefs.HasKey(ConversionKeyKey))
                conversionKey = EditorPrefs.GetString(ConversionKeyKey);
            if (EditorPrefs.HasKey(PackageNameKey))
                packageName = EditorPrefs.GetString(PackageNameKey);
            if (EditorPrefs.HasKey(ApiLevelKey))
                apiLevel = EditorPrefs.GetString(ApiLevelKey);
            if (EditorPrefs.HasKey(ShowDelegateFoldoutKey))
                showDelegate = EditorPrefs.GetBool(ShowDelegateFoldoutKey);
        }

        private void SaveSettings()
        {
            EditorPrefs.SetString(AdvertiserIdKey, advertiserId);
            EditorPrefs.SetString(ConversionKeyKey, conversionKey);
            EditorPrefs.SetString(PackageNameKey, packageName);
            EditorPrefs.SetString(ApiLevelKey, apiLevel);
            EditorPrefs.SetBool(ShowDelegateFoldoutKey, showDelegate);
        }

        private void SetupUI()
        {
            GUI.skin.label.wordWrap = true;
            GUI.skin.button.wordWrap = true;

            EditorStyles.textField.wordWrap = true;
            EditorStyles.label.wordWrap = true;

            GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);
            myFoldoutStyle.fontStyle = FontStyle.Bold;

            showDelegate = EditorGUILayout.Foldout(showDelegate, "Delegate Callbacks", myFoldoutStyle);
            if (showDelegate)
            {
                ShowDelegateCallbacks();
            }
        }


        private void ShowDelegateCallbacks()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("You may debug server requests with the TuneListener.cs functions by enabling delegate callbacks: \n\n" +
                "Tune.SetDelegate(true); \n\n" +
                "The TuneListener.cs script must be attached to a GameObject named \"TuneListener\" in order to receive the callbacks.\n");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create TuneListener GameObject"))
            {
                if (GameObject.Find("TuneListener") == null)
                {
                    var obj = new GameObject("TuneListener");
                    obj.AddComponent<TuneListener>();
                }
                else
                {
                    EditorUtility.DisplayDialog("TuneListener exists", "A TuneListener GameObject already exists", sOk);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void CreateLifecycleListener()
        {
            if (advertiserId == null || advertiserId == "")
            {
                UnityEngine.Debug.LogError("Advertiser Id not set.");
                EditorUtility.DisplayDialog("Advertiser Id not set",
                    "The advertiser id was not set. Please set this in order to generate your LifecycleListener.",
                    sOk);
                return;
            }
            if (conversionKey == null || conversionKey == "")
            {
                UnityEngine.Debug.LogError("Conversion key not set.");
                EditorUtility.DisplayDialog("Conversion key level not set",
                    "The conversion key was not set. Please set this in order to generate your LifecycleListener.",
                    sOk);
                return;
            }

            Process p = new Process();
            p.StartInfo.FileName = "python";
            p.StartInfo.Arguments = "create_native_init.py" +
                " -p ios" +
                " -a " + advertiserId +
                " -c " + conversionKey +
                " -pn " + packageName;

            // Pipe the output to itself
            p.StartInfo.RedirectStandardError=true;
            p.StartInfo.RedirectStandardOutput=true;
            p.StartInfo.CreateNoWindow = true;

            p.StartInfo.WorkingDirectory = Application.dataPath+"/Tune";
            p.StartInfo.UseShellExecute = false;
            p.Start();
            // Read the output - this will show as a single entry in the console
            UnityEngine.Debug.Log( p.StandardOutput.ReadToEnd() );
            p.WaitForExit();
            p.Close();
        }

        private void CreateApplicationJava()
        {
            if (advertiserId == null || advertiserId == "")
            {
                UnityEngine.Debug.LogError("Advertiser Id not set.");
                EditorUtility.DisplayDialog("Advertiser Id not set",
                    "The advertiser id was not set. Please set this in order to generate your Application java.",
                    sOk);
                return;
            }
            if (conversionKey == null || conversionKey == "")
            {
                UnityEngine.Debug.LogError("Conversion key not set.");
                EditorUtility.DisplayDialog("Conversion key level not set",
                    "The conversion key was not set. Please set this in order to generate your Application java.",
                    sOk);
                return;
            }

            Process p = new Process();
            p.StartInfo.FileName = "python";
            p.StartInfo.Arguments = "create_native_init.py" +
                " -p android" +
                " -a " + advertiserId +
                " -c " + conversionKey +
                " -pn " + packageName;

            // Pipe the output to itself
            p.StartInfo.RedirectStandardError=true;
            p.StartInfo.RedirectStandardOutput=true;
            p.StartInfo.CreateNoWindow = true;

            p.StartInfo.WorkingDirectory = Application.dataPath+"/Tune";
            p.StartInfo.UseShellExecute = false;
            p.Start();
            // Read the output - this will show as a single entry in the console
            UnityEngine.Debug.Log( p.StandardOutput.ReadToEnd() );
            p.WaitForExit();
            p.Close();
        }

        private void GenerateApplicationJar()
        {
            if (packageName == null || packageName == "")
            {
                UnityEngine.Debug.LogError("Package name not set.");
                EditorUtility.DisplayDialog("Package name not set",
                    "The package name was not set. Please set this in order to generate your Application jar.",
                    sOk);
                return;
            }
            if (apiLevel == null || apiLevel == "")
            {
                UnityEngine.Debug.LogError("Android API level not set.");
                EditorUtility.DisplayDialog("Android API level not set",
                    "The Android API level was not set. Please set this in order to generate your Application jar.",
                    sOk);
                return;
            }

            Process p = new Process();
            p.StartInfo.FileName = "python";
            p.StartInfo.Arguments = "compile_application_jar.py" +
                " -p " + packageName +
                " -s " + GetAndroidSdkPath() +
                " -l " + apiLevel;

            // Pipe the output to itself
            p.StartInfo.RedirectStandardError=true;
            p.StartInfo.RedirectStandardOutput=true;
            p.StartInfo.CreateNoWindow = true;

            p.StartInfo.WorkingDirectory = Application.dataPath+"/Tune";
            p.StartInfo.UseShellExecute = false;
            p.Start();
            // Read the output - this will show as a single entry in the console
            UnityEngine.Debug.Log( p.StandardOutput.ReadToEnd() );
            p.WaitForExit();
            p.Close();
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
    }
}
