using System;
using UnityEditor;
// using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildScript
{
    public static void Build()
    {
        // TODO: may get option from JSON written by pipeline
        var args = Environment.GetCommandLineArgs();
        var buildOutputPath = GetCommandLineArgumentValue(args, "BuildOutputPath");

        var developmentBuild = false;
        var devbuildParseResult = Boolean.TryParse(GetCommandLineArgumentValue(args, "DevelopmentBuild"), out developmentBuild);

        var allActiveScenes = GetAllActivatedScenes();
        var activeTarget = EditorUserBuildSettings.activeBuildTarget;
        var selectedtargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        var outputPath = AddBuildTargetOutputFileExtension(buildOutputPath);

        var dbg = "Aquired build options:\n\tBuild scenes (" + allActiveScenes.Length + "):\n";
        for (int i = 0; i < allActiveScenes.Length; i++)
        {
            dbg += "\t\t- " + allActiveScenes[i] + "\n";
        }
        dbg += "\tTarget: " + activeTarget + " | " + selectedtargetGroup;
        dbg += "\nOutput path: " + buildOutputPath;
        dbg += "\nOutput with extension: " + outputPath;
        dbg += "\nDevemlopment Build: " + developmentBuild;

        Debug.Log(dbg);

        // These options make append to project and auto play + build
        var playerOptions = BuildOptions.AcceptExternalModificationsToPlayer | BuildOptions.AutoRunPlayer;

        // if dev build also enable script debugging
        if (developmentBuild) playerOptions = playerOptions | BuildOptions.Development | BuildOptions.AllowDebugging;

        var buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = allActiveScenes,
            target = activeTarget,
            targetGroup = selectedtargetGroup,
            locationPathName = outputPath,
            options = playerOptions,
        };

//        AddressableAssetSettings.BuildPlayerContent();

        var buildReport = default(BuildReport);
        buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);

        Debug.Log(string.Format("Build {0} in {1} seconds", buildReport.summary.result.ToString(), buildReport.summary.totalTime.Seconds));

        if (buildReport.summary.result == BuildResult.Succeeded)
        {
            EditorApplication.Exit(0);
        }

        // else
        EditorApplication.Exit(1);
    }

    private static void ListAllArguments(string[] allArguments)
    {
        var dbg = "All arguments:\n";

        for (int i = 0; i < allArguments.Length; i++)
        {
            dbg += "\t(" + i + ") " + allArguments[i];
        }
    }

    private static string GetCommandLineArgumentValue(string[] allArguments, string argumentName)
    {
        if (!argumentName.StartsWith("--"))
        {
            argumentName = ("--" + argumentName);
        }

        for (int i = 0; i < allArguments.Length; i++)
        {
            if (allArguments[i] == argumentName)
            {
                return allArguments[i + 1];
            }
        }

        throw new Exception("The argument '" + argumentName + "' could not be found");
    }

    private static string[] GetAllActivatedScenes()
    {
        EditorBuildSettingsScene[] buildSettingsScenes = EditorBuildSettings.scenes;
        var resultScenes = new string[buildSettingsScenes.Length];

        for (int i = 0; i < buildSettingsScenes.Length; i++)
        {
            if (buildSettingsScenes[i].enabled)
            {
                resultScenes[i] = buildSettingsScenes[i].path;
            }
        }

        return resultScenes;
    }

    private static string AddBuildTargetOutputFileExtension(string outputFileName)
    {
        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.Android:
                return string.Format("{0}.apk", outputFileName);
            case BuildTarget.StandaloneWindows64:
            case BuildTarget.StandaloneWindows:
                return string.Format("{0}.exe", outputFileName);
#if UNITY_2018_1_OR_NEWER
            case BuildTarget.StandaloneOSX:
#endif
#if !UNITY_2017_3_OR_NEWER
            case BuildTarget.StandaloneOSXIntel:
            case BuildTarget.StandaloneOSXIntel64:
#endif
                return string.Format("{0}.app", outputFileName);
            case BuildTarget.iOS:
            case BuildTarget.tvOS:
            case BuildTarget.WebGL:
            case BuildTarget.WSAPlayer:
            case BuildTarget.StandaloneLinux64:
#if !UNITY_2018_3_OR_NEWER
            case BuildTarget.PSP2:    
#endif
            case BuildTarget.PS4:
            case BuildTarget.XboxOne:
#if !UNITY_2017_3_OR_NEWER
            case BuildTarget.SamsungTV:
#endif
#if !UNITY_2018_1_OR_NEWER
            case BuildTarget.N3DS:
            case BuildTarget.WiiU:
#endif
            case BuildTarget.Switch:
            case BuildTarget.NoTarget:
            default:
                return outputFileName;
        }
    }
}

