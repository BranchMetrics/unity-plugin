using UnityEditor;
class CIBuildScripts
{
    static void BuildiOS()
    {
        string[] scenes = {"Assets/MATSampleScene.unity"};
        BuildPipeline.BuildPlayer(scenes, "ios-buildtest-output", BuildTarget.iPhone, BuildOptions.None);
    }

    static void BuildAndroid()
    {
        string[] scenes = {"Assets/MATSampleScene.unity"};
        BuildPipeline.BuildPlayer(scenes, "android-buildtest-output", BuildTarget.Android, BuildOptions.None);
    }
}