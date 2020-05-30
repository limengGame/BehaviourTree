using System;
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
        string path = GetBuildPathAndroid();
        Debug.Log("StartBuild!");
        Debug.Log("JenkinsParams: " + GetJenkinsParameter());
        BuildPipeline.BuildPlayer(sceneList.ToArray(), path, BuildTarget.Android, BuildOptions.None);
    }

    static string GetBuildPathAndroid()
    {
        //string dirPath = Application.dataPath.Replace("/Assets", "") + "/../build";
        string dirPath = "C:/Resource";
        if (!System.IO.Directory.Exists(dirPath))
        {
            System.IO.Directory.CreateDirectory(dirPath);
        }
        return dirPath + "/behaviour.apk";
    }

    static string GetJenkinsParameter()
    {
        foreach (string arg in Environment.GetCommandLineArgs())
        {
            if (!string.IsNullOrEmpty(arg))
            {
                return (arg.Split('-') != null && arg.Split('-').Length > 0) ? arg.Split('-')[0] : "";
            }
        }
        return "";
    }

}