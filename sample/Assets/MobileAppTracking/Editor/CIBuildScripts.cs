using UnityEditor;
class CIBuildScripts
{
    static void BuildiOS()
    {
        string[] scenes = {"Assets/MobileAppTracking/Scenes/MATSampleScene.unity"};
        #if UNITY_5
        BuildPipeline.BuildPlayer(scenes, "ios-buildtest-output", BuildTarget.iOS, BuildOptions.None);
        #else
        BuildPipeline.BuildPlayer(scenes, "ios-buildtest-output", BuildTarget.iPhone, BuildOptions.None);
        #endif
    }

    static void BuildAndroid()
    {
        string[] scenes = {"Assets/MATSampleScene.unity"};
        BuildPipeline.BuildPlayer(scenes, "android-buildtest-output", BuildTarget.Android, BuildOptions.None);
    }

    static void BuildWP8()
    {
        string[] scenes = {"Assets/MATSampleScene.unity"};
        BuildPipeline.BuildPlayer(scenes, "WP8-buildtest-output", BuildTarget.WP8Player, BuildOptions.None);
    }
}