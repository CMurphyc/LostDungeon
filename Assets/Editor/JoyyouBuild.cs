using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;


public class JoyyouBuild
{
    [MenuItem("JoyYouSDK/AndroidBuild")]
	public static void AndroidBuild()
    {
        PlayerSettings.Android.keystoreName = "./dungeon_demo.keystore";
        PlayerSettings.Android.keystorePass = "com.tencent.tmgp.dungeon_demo";
        PlayerSettings.Android.keyaliasName = "dungeon_demoawake";
        PlayerSettings.Android.keyaliasPass = "com.tencent.tmgp.dungeon_demo";

        Build("./android.apk", BuildTarget.Android, BuildOptions.None);
    }

    [MenuItem("JoyYouSDK/AndroidBuild", true)]
    static bool DirectBuild_Android()
    {
        Debug.Log("Current plat = " + EditorUserBuildSettings.activeBuildTarget);
        return (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android);
    }

    [MenuItem("JoyYouSDK/IOSBuild")]
    public static void IosBuild()
	{
        string destination = "/Users/kedlly/Desktop/JoyYouSDK/Xcode_Project/BuildTest3";
        Build(destination, BuildTarget.iOS, BuildOptions.None);
    }

    [MenuItem("JoyYouSDK/IOSBuild", true)]
    static bool DirectBuild_IOS()
    {
        Debug.Log("Current plat = " + EditorUserBuildSettings.activeBuildTarget);
        return (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS);
    }

    private static void Build(string target_dir, BuildTarget build_target, BuildOptions build_options)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);

        string res = BuildPipeline.BuildPlayer(FindEnabledEditorScenes(), target_dir, build_target, build_options).ToString();
        if (res.Length > 0)
        {
            throw new Exception("BuildPlayer failure: " + res);
        }
    }

    //获取可用场景列表
    public static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();

        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;

            EditorScenes.Add(scene.path);
        }

        return EditorScenes.ToArray();
    }
}
