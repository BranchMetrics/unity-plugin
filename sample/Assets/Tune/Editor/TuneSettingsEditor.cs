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
        private static string IosEnableIamKey = "TUNE_EnableIamIos";

        private static string ShowIamFoldoutKey = "TUNE_ShowIam";
        private static string ShowDelegateFoldoutKey = "TUNE_ShowDelegate";

        private static string sOk = "OK";

        private static string advertiserId;
        private static string conversionKey;
        private static string packageName;
        private static string apiLevel;

        public static bool enabledIamIos;

        private static bool showIam;
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
            if (EditorPrefs.HasKey(IosEnableIamKey))
                enabledIamIos = EditorPrefs.GetBool(IosEnableIamKey);

            if (EditorPrefs.HasKey(ShowIamFoldoutKey))
                showIam = EditorPrefs.GetBool(ShowIamFoldoutKey);
            if (EditorPrefs.HasKey(ShowDelegateFoldoutKey))
                showDelegate = EditorPrefs.GetBool(ShowDelegateFoldoutKey);
        }

        private void SaveSettings()
        {
            EditorPrefs.SetString(AdvertiserIdKey, advertiserId);
            EditorPrefs.SetString(ConversionKeyKey, conversionKey);
            EditorPrefs.SetString(PackageNameKey, packageName);
            EditorPrefs.SetString(ApiLevelKey, apiLevel);
            EditorPrefs.SetBool(IosEnableIamKey, enabledIamIos);
            EditorPrefs.SetBool(ShowIamFoldoutKey, showIam);
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

            showIam = EditorGUILayout.Foldout(showIam, "In-App Marketing", myFoldoutStyle);
            if (showIam)
            {
                ShowIamUI();
            }
            showDelegate = EditorGUILayout.Foldout(showDelegate, "Delegate Callbacks", myFoldoutStyle);
            if (showDelegate)
            {
                ShowDelegateCallbacks();
            }
        }

        private void ShowIamUI()
        {
            EditorGUILayout.LabelField("If your app is not using In-App Marketing, you may disregard this section.");
            EditorGUILayout.Space();
            var linkStyle = new GUIStyle(GUI.skin.label);
            linkStyle.normal.textColor = Color.blue;

            if (GUILayout.Button("In-App Marketing Setup Guide", linkStyle))
            {
                Application.OpenURL("https://developers.mobileapptracking.com/enabling-in-app-marketing/");
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            ShowIamAndroid();
            ShowIamIos();
        }

        private void ShowIamAndroid()
        {
            EditorGUILayout.BeginHorizontal();
            advertiserId = EditorGUILayout.TextField("Advertiser Id", advertiserId);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            conversionKey = EditorGUILayout.TextField("Conversion Key", conversionKey);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            packageName = EditorGUILayout.TextField("Package Name", packageName);
            string packagePath = packageName.Replace(".", "/");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Android", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("For Android builds using In-App Marketing, the TUNE plugin requires a custom Application class that extends TuneApplication.\n");

            EditorGUILayout.LabelField("1. Create a custom Application class");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create Application Java File"))
            {
                CreateApplicationJava();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("Creates a file at /Tune/Plugins/Android/" + packagePath + "/MyApplication.java.\n" +
                "Our script will look for this file to generate the JAR.", MessageType.Info);
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("2. Generate a JAR from the Application java file");
            EditorGUILayout.BeginHorizontal();
            apiLevel = EditorGUILayout.TextField("Android API Level", apiLevel);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("Android API level to compile the JAR with.\n" +
                "This will be an int representing a platform version that exists in " + GetAndroidSdkPath() + "/platforms/android-{api_level}.", MessageType.None);
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Generate Application Jar"))
            {
                GenerateApplicationJar();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("Creates a JAR at /Tune/Plugins/Android/MyApplication.jar.\n" +
                "Unity will use this JAR when building your app.", MessageType.Info);
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("3. Add a reference to the Application class in order to use it");
            EditorGUILayout.LabelField("After generating the JAR, modify your AndroidManifest.xml by adding a reference to the MyApplication class:\n\n" +
                "<application\n" +
                "    android:name=\".MyApplication\"");

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        private void ShowIamIos()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("iOS", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("For iOS builds using In-App Marketing, the TUNE plugin requires extra processing of the Xcode project generated by Unity, " +
            "and a custom class which implements Unity's LifeCycleListener.\n");

            EditorGUILayout.LabelField("1. Enable Xcode post-build processing for In-App Marketing");
            EditorGUILayout.LabelField("This includes adding a TuneConfiguration.plist and push permissions to your app.");
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            enabledIamIos = EditorGUILayout.ToggleLeft("Enable iOS In-App Marketing", enabledIamIos);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.HelpBox("Checking this checkbox to add In-App Marketing post-build processing to your Xcode project.", MessageType.None);
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("2. Create a custom LifeCycleListener class");
            EditorGUILayout.LabelField("The TUNE plugin also requires initialization in the iOS didFinishLaunching lifecycle event.\n" +
                "You may generate a custom file that implements Unity's LifeCycleListener here.");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create LifeCycleListener File"))
            {
                CreateLifecycleListener();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("Creates a file at /Plugins/iOS/MyAppDelegate.mm.\n" +
                "Unity will use this file to hook into the iOS application lifecycle to start TUNE.", MessageType.Info);
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
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
