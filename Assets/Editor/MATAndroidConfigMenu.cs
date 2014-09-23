using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

public class MATAndroidConfigMenu : EditorWindow 
{
    string google_play_services_path = "";
    string google_play_services_folder_name = "";

    void OnGUI()
    {
        GUI.skin.label.wordWrap = true;
        GUI.skin.button.wordWrap = true;

        GUILayout.Label("\nA reference to your google-play-services_lib folder is required for Android builds that use the MAT Plugin."); 
        if(GUILayout.Button("import google-play-services_lib"))
        {
            google_play_services_path = EditorUtility.OpenFolderPanel("select google-play-services_lib folder","","");
            google_play_services_path = google_play_services_path.Replace('/', '\\');
            string[] folders = google_play_services_path.Split(new char[] {'\\'});

            if(folders.Length > 0)
                google_play_services_folder_name = folders[folders.Length - 1]; 
            
            if(google_play_services_path.Length > 0 && File.Exists(google_play_services_path + "\\libs\\google-play-services.jar"))
            { 
                if(Directory.Exists(Application.dataPath + "\\Plugins\\Android"))
                {
                    FileUtil.CopyFileOrDirectory(
                        google_play_services_path,
                        Application.dataPath + "\\Plugins\\Android\\" + google_play_services_folder_name);

                    AssetDatabase.Refresh();
                    this.Close();
                }
                else
                {
                    EditorUtility.DisplayDialog("Error:", "Cannot find Assets/Plugins/Android. Please make sure that you're using the latest version of the MAT Unity plugin.", "OK");
                }
            }
            else if(google_play_services_path.Length != 0 && !File.Exists(google_play_services_path + "\\libs\\google-play-services.jar"))
            {
                EditorUtility.DisplayDialog("Error:", google_play_services_path + " does not contain the necessary files. Please ensure that your directory contains the following: google-play-services_lib/libs/google-play-services.jar", "OK");
            }
        }
    }
}