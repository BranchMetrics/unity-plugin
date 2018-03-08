using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace TuneSDK
{
    public static class TunePostBuildTrigger
    {
        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
        {
            if (buildTarget == BuildTarget.iOS)
            {
                UnityEngine.Debug.Log("Running TUNE iOS post build script.");

                // Go get pbxproj file
                string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

                // PBXProject class represents a project build settings file,
                // here is how to read that in.
                PBXProject proj = new PBXProject ();
                proj.ReadFromFile (projPath);

                // This is the Xcode target in the generated project
                string target = proj.TargetGuidByName ("Unity-iPhone");

                // Add required frameworks
                UnityEngine.Debug.Log("Adding frameworks");
                proj.AddFrameworkToProject(target, "AdSupport.framework", false);
                proj.AddFrameworkToProject(target, "CoreSpotlight.framework", false);
                proj.AddFrameworkToProject(target, "CoreTelephony.framework", false);
                proj.AddFrameworkToProject(target, "iAd.framework", false);
                proj.AddFrameworkToProject(target, "MobileCoreServices.framework", false);
                proj.AddFrameworkToProject(target, "Security.framework", false);
                proj.AddFrameworkToProject(target, "StoreKit.framework", false);
                proj.AddFrameworkToProject(target, "SystemConfiguration.framework", false);
                proj.AddFrameworkToProject(target, "UserNotifications.framework", true);

                // Add libz.dylib
                proj.AddFileToBuild(target, proj.AddFile("usr/lib/libz.dylib", "Frameworks/libz.dylib", PBXSourceTree.Sdk));

                // Set 'Enable Modules' to YES to prevent errors using @import syntax
                UnityEngine.Debug.Log("Enabling modules: CLANG_ENABLE_MODULES = YES");
                proj.AddBuildProperty(target, "CLANG_ENABLE_MODULES", "YES");

                // Add compiler flags
                UnityEngine.Debug.Log("Adding compiler flags: -ObjC -lz");
                proj.AddBuildProperty(target, "OTHER_LDFLAGS", "-ObjC -lz");

                // If IAM is enabled in the Editor, copy TuneConfiguration.plist
                if (TuneSettingsEditor.enabledIamIos)
                {
                    // Add TuneConfiguration.plist
                    UnityEngine.Debug.Log("Adding TuneConfiguration.plist");
                    // Copy TuneConfiguration.plist from the project folder to the build folder
                    FileUtil.ReplaceFile("Assets/Tune/Plugins/iOS/TuneConfiguration.plist", path + "/TuneConfiguration.plist");
                    proj.AddFileToBuild(target, proj.AddFile("TuneConfiguration.plist", "TuneConfiguration.plist"));
                }

                // If IAM is enabled in the Editor
                if (TuneSettingsEditor.enabledIamIos)
                {
                    // Add remote notifications to Background Modes in Info.plist
                    // Get plist
                    string plistPath = path + "/Info.plist";
                    PlistDocument plist = new PlistDocument();
                    plist.ReadFromString(File.ReadAllText(plistPath));

                    // Get root
                    PlistElementDict rootDict = plist.root;

                    // Add remote-notification to UIBackgroundModes in Xcode plist
                    var buildKey = "UIBackgroundModes";
                    rootDict.CreateArray(buildKey).AddString("remote-notification");

                    // Write to file
                    File.WriteAllText(plistPath, plist.WriteToString());

                    // Enable push capabilities in the project
                    proj.AddCapability(target, PBXCapabilityType.PushNotifications);
                }

                // Write PBXProject object back to the file
                proj.WriteToFile (projPath);

                UnityEngine.Debug.Log("Finished TUNE iOS post build script.");
            }
        }
    }
}
