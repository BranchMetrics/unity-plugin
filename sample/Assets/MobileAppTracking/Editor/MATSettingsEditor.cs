/*
* Copyright (C) 2013 Google Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
* 
* Modified for use with the Chartboost Unity plugin
*/
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace MATSDK {
    [CustomEditor(typeof(MATSettings))]
    public class MATSettingsEditor : Editor {
        private string sOk = "OK";
        private string google_play_services_path = "";
        private string google_play_services_folder_name = "";

        public override void OnInspectorGUI() {
            SetupUI();
        }

        private void SetupUI() {
            GUI.skin.label.wordWrap = true;
            GUI.skin.button.wordWrap = true;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("MobileAppTracker", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox("A GameObject named \"MobileAppTracker\" with MATDelegate.cs and MATBinding.cs attached is " +
            "required in order for MAT to receive server callbacks and Google Advertising Id, respectively.", MessageType.Info);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create MobileAppTracker GameObject"))
            {
                if (GameObject.Find("MobileAppTracker") == null)
                {
                    var obj = new GameObject("MobileAppTracker");
                    obj.AddComponent("MATDelegate");
                    obj.AddComponent("MATBinding");
                } else {
                    EditorUtility.DisplayDialog("MobileAppTracker exists", "A MobileAppTracker GameObject already exists", sOk);
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("You may debug server requests with the MATDelegate.cs functions by enabling " +
            "delegates and debug mode. This can be done with the following lines of code: \n\n" +
            "MATBinding.SetDelegate(true); \n" +
            "MATBinding.SetDebugMode(true); \n\n" +
            "When using Windows Phone 8, the following code is also required: \n\n" +
            "#if UNITY_WP8 \n" +
            "MATBinding.SetMATResponse(new SampleMATResponse());\n" +
            "#endif");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Android", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.HelpBox("\nA reference to your google-play-services_lib folder is required for Android builds that use the MAT Plugin.", MessageType.Info);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Import Google Play Services"))
            {
                ImportGooglePlayServices();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void ImportGooglePlayServices() {
            google_play_services_path = EditorUtility.OpenFolderPanel("select google-play-services_lib folder","","");
            google_play_services_path = google_play_services_path.Replace('/', '\\');
            string[] folders = google_play_services_path.Split(new char[] {'\\'});

            if (folders.Length > 0)
                google_play_services_folder_name = folders[folders.Length - 1]; 
            
            if (google_play_services_path.Length > 0 && File.Exists(google_play_services_path + "\\libs\\google-play-services.jar"))
            { 
                if (Directory.Exists(Application.dataPath + "\\Plugins\\Android"))
                {
                    FileUtil.CopyFileOrDirectory(
                        google_play_services_path,
                        Application.dataPath + "\\Plugins\\Android\\" + google_play_services_folder_name);

                    AssetDatabase.Refresh();
                }
                else
                {
                    EditorUtility.DisplayDialog("Error:", "Cannot find Assets/Plugins/Android. Please make sure that you're using the latest version of the MAT Unity plugin.", "OK");
                }
            }
            else if (google_play_services_path.Length != 0 && !File.Exists(google_play_services_path + "\\libs\\google-play-services.jar"))
            {
                EditorUtility.DisplayDialog("Error:", google_play_services_path + " does not contain the necessary files. Please ensure that your directory contains the following: google-play-services_lib/libs/google-play-services.jar", "OK");
            }
        }
    }
}