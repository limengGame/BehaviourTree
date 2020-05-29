using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class JenkinsAdapter
{
    [MenuItem("Jenkins/JenkinsBuild")]
    public static void Build()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

        List<string> sceneList = new List<string>();
        EditorBuildSettingsScene[] temp = EditorBuildSettings.scenes;
        for (int i = 0, iMax = temp.Length; i < iMax; ++i)
            sceneList.Add(temp[i].path);
        string path = Application.dataPath + "/" + Application.productName + ".apk";
        BuildPipeline.BuildPlayer(sceneList.ToArray(), path, BuildTarget.Android, BuildOptions.None);
    }
}