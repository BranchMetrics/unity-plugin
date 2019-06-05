using UnityEditor;

class CIBuildScripts
{
    static string PACKAGE_NAME = "com.hasoffers.unitytestapp";

    static void BuildiOS()
    {
        PlayerSettings.applicationIdentifier = PACKAGE_NAME;
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
        string[] scenes = {"Assets/Tune/Scenes/TUNESampleScene.unity"};
        BuildPipeline.BuildPlayer(scenes, "ios-buildtest-output", BuildTarget.iOS, BuildOptions.None);
    }

    static void BuildAndroid()
    {
        PlayerSettings.applicationIdentifier = PACKAGE_NAME;
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        string[] scenes = {"Assets/Tune/Scenes/TUNESampleScene.unity"};
        BuildPipeline.BuildPlayer(scenes, "android-buildtest-output.apk", BuildTarget.Android, BuildOptions.None);
    }
}
