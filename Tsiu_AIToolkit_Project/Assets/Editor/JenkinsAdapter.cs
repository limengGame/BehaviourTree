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

        if(CommandLineTool.HasCommandArgs(CommandArgsName.Channel))
            Debug.Log("JenkinsCommandParams: " + CommandLineTool.GetEnvironmentVariable(CommandArgsName.Channel));


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
        string str = "";
        foreach (string arg in Environment.GetCommandLineArgs())
        {
            if (!string.IsNullOrEmpty(arg))
            {
                str += arg;
            }
        }
        return str;
    }

    enum CommandArgsName
    {
        Version,
        Channel,
        BuildType,
    }
    class CommandLineTool
    {
        private static Dictionary<CommandArgsName, string> dicCommandArgsName = new Dictionary<CommandArgsName, string>();
        public static string GetEnvironmentVariable(CommandArgsName commandArgsName)
        {
            return dicCommandArgsName.ContainsKey(commandArgsName) ? dicCommandArgsName[commandArgsName] :
             System.Environment.GetEnvironmentVariable(commandArgsName.ToString()) ?? string.Empty;
        }

        public static bool HasCommandArgs(CommandArgsName commandArgsName)
        {
            var value = GetEnvironmentVariable(commandArgsName);
            return !(string.IsNullOrEmpty(value) || string.Compare(value, "false", true) == 0);
        }
    }


}