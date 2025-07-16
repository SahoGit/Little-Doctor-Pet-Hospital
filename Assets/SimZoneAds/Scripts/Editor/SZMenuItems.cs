using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

static class SZMenuItems
{
#if UNITY_ANDROID && UNITY_EDITOR
#if !UNITY_IOS
    [MenuItem("SimZone/Monetization/Firebase/Debug/Start", false, 4)]
    public static void Start()
    {
        string DebugCommand = $"/c adb shell setprop debug.firebase.analytics.app {Application.identifier}";

        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = @DebugCommand,
            WorkingDirectory = @"C:\Users\" + Environment.UserName
        };
        process.StartInfo = startInfo;
        process.Start();

        UnityEngine.Debug.Log("Please Wait... The prompt will close automatically");
    }

    [MenuItem("SimZone/Monetization/Firebase/Debug/Stop", false, 4)]
    public static void Stop()
    {
        string DebugCommand = $"/c adb shell setprop debug.firebase.analytics.app .none.";

        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = @DebugCommand,
            WorkingDirectory = @"C:\Users\" + Environment.UserName
        };
        process.StartInfo = startInfo;
        process.Start();

        UnityEngine.Debug.Log("Please Wait... The prompt will close automatically");
    }
    [MenuItem("SimZone/Monetization/Firebase/Debug/Documentation", false, 4)]
    public static void FirebaseDoc()
    {
        Application.OpenURL("https://firebase.google.com/docs/analytics/debugview");
    }
#endif

    [MenuItem("SimZone/Monetization/Editor/Ads/Enable", false, 5)]
    public static void DisableAdsNo()
    {
        EditorUtility.DisplayDialog("Congratulations!", "Ads are enabled inside editor.", "OK");
        PlayerPrefs.SetInt("RemoveAds", 0);
    }

    [MenuItem("SimZone/Monetization/Editor/Ads/Disable", false, 5)]
    public static void DisableAdsYes()
    {
        EditorUtility.DisplayDialog("Congratulations!", "Ads are disabled inside editor.", "OK");
        AdConstants.RemoveAds();
    }

#endif

    #region Environment
    //[MenuItem("KokoZone/Monetization/Environment/PRODUCTION")]
    //private static void Env_Prod()
    //{
    //    SetEnabled("ADS_SANDBOX", false);
    //}


    //[MenuItem("KokoZone/Monetization/Environment/PRODUCTION", true)]
    //private static bool Env_ProdValidate()
    //{
    //    var defines = GetDefinesList(BuildTargetGroup.Android);
    //    return defines.Contains("ADS_SANDBOX");
    //}


    //[MenuItem("KokoZone/Monetization/Environment/SANDBOX")]
    //private static void Env_Sand()
    //{
    //    SetEnabled("ADS_SANDBOX", true);
    //}


    //[MenuItem("KokoZone/Monetization/Environment/SANDBOX", true)]
    //private static bool Env_SandValidate()
    //{
    //    var defines = GetDefinesList(BuildTargetGroup.Android);
    //    return !defines.Contains("ADS_SANDBOX");
    //}
    #endregion

    #region GameAnalytics

    [MenuItem("SimZone/Monetization/Extensions/GameAnalytics/Import", false, 3)]
    private static void GA_Import()
    {
        if (EditorUtility.DisplayDialog("Confirmation!", "Do you want to import GameAnalytics in your project?", "Yes", "No"))
        {
            var package = "Assets/SimZoneAds/Plugins/GameAnalytics_v7.6.1.unitypackage";
            AssetDatabase.ImportPackage(package, false);
            SetEnabled("kz_gameanalytics_enabled", true);
        }
    }

    [MenuItem("SimZone/Monetization/Extensions/GameAnalytics/Import", true, 3)]
    private static bool GA_ImportValidate()
    {
        PluginImporter importer = AssetImporter.GetAtPath("Assets/GameAnalytics/Plugins/GameAnalytics.dll") as PluginImporter;
        return importer == null;
    }

    [MenuItem("SimZone/Monetization/Extensions/GameAnalytics/Remove", false, 3)]
    private static void GA_Remove()
    {
        if (EditorUtility.DisplayDialog("Confirmation!", "Do you really want to remove GameAnalytics?", "Yes", "No"))
        {
            var package = "Assets/GameAnalytics";
            AssetDatabase.DeleteAsset(package);
            SetEnabled("kz_gameanalytics_enabled", false);
        }
    }

    [MenuItem("SimZone/Monetization/Extensions/GameAnalytics/Remove", true, 3)]
    private static bool GA_RemoveValidate()
    {
        PluginImporter importer = AssetImporter.GetAtPath("Assets/GameAnalytics/Plugins/GameAnalytics.dll") as PluginImporter;
        return importer != null;
    }

    //[MenuItem("KokoZone/Monetization/GameAnalytics/ILRD/Enable", false, 2)]
    //private static void GA_ILRD_Enable()
    //{
    //    SetEnabled("gameanalytics_admob_enabled", true);
    //    SetEnabled("gameanalytics_max_enabled", true);
    //}


    //[MenuItem("KokoZone/Monetization/GameAnalytics/ILRD/Enable", true, 2)]
    //private static bool GA_ILRD_EnableValidate()
    //{
    //    var defines = GetDefinesList(BuildTargetGroup.Android);
    //    return !defines.Contains("gameanalytics_admob_enabled") || !defines.Contains("gameanalytics_max_enabled");
    //}


    //[MenuItem("KokoZone/Monetization/GameAnalytics/ILRD/Disable", false, 2)]
    //private static void GA_ILRD_Disable()
    //{
    //    SetEnabled("gameanalytics_admob_enabled", false);
    //    SetEnabled("gameanalytics_max_enabled", false);
    //}


    //[MenuItem("KokoZone/Monetization/GameAnalytics/ILRD/Disable", true, 2)]
    //private static bool GA_ILRD_DisableValidate()
    //{
    //    var defines = GetDefinesList(BuildTargetGroup.Android);
    //    return defines.Contains("gameanalytics_admob_enabled") || defines.Contains("gameanalytics_max_enabled");
    //}

    #endregion

    #region Define Symbols
    private static BuildTargetGroup[] buildTargetGroups = new BuildTargetGroup[]
    {
            BuildTargetGroup.Standalone,
            BuildTargetGroup.Android,
            BuildTargetGroup.iOS
    };

    private static void SetEnabled(string defineName, bool enable)
    {
        for (int i = 0; i < buildTargetGroups.Length; i++)
        {
            var group = buildTargetGroups[i];
            var defines = GetDefinesList(group);
            if (enable)
            {
                if (defines.Contains(defineName))
                {
                    continue;
                }
                defines.Add(defineName);
            }
            else
            {
                if (!defines.Contains(defineName))
                {
                    continue;
                }
                while (defines.Contains(defineName))
                {
                    defines.Remove(defineName);
                }
            }
            string definesString = string.Join(";", defines.ToArray());
            PlayerSettings.SetScriptingDefineSymbolsForGroup(group, definesString);
        }
    }
    private static List<string> GetDefinesList(BuildTargetGroup group)
    {
        return new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';'));
    }
    #endregion
}