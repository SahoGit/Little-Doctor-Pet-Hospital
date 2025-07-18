﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AdsRemoteSettingsEditor : Editor
{
    //const string AdsRemoteSettingsResDir = "Assets/KZ Monetization/Remote/Resources";
    //const string AdsRemoteSettingsFile = "RemoteSettings";
    //const string AdsRemoteSettingsFileExtension = ".asset";

    //internal static AdsRemoteSettings LoadInstance()
    //{
    //    var instance = Resources.Load<AdsRemoteSettings>(AdsRemoteSettingsFile);

    //    if (instance == null)
    //    {
    //        Directory.CreateDirectory(AdsRemoteSettingsResDir);
    //        instance = ScriptableObject.CreateInstance<AdsRemoteSettings>();
    //        string assetPath = Path.Combine(
    //            AdsRemoteSettingsResDir,
    //            AdsRemoteSettingsFile + AdsRemoteSettingsFileExtension);
    //        AssetDatabase.CreateAsset(instance, assetPath);
    //        AssetDatabase.SaveAssets();
    //    }

    //    return instance;
    //}

    //[MenuItem("KokoZone/Monetization/RemoteSettings/Select", false, 1)]
    //public static void RemoteSettings()
    //{
    //    Selection.activeObject = LoadInstance();
    //}

    [MenuItem("SimZone/Monetization/RemoteSettings/ClipboardKey", false, 1)]
    public static void ClipboardKey()
    {
        GUIUtility.systemCopyBuffer = AdsRemoteSettings.RemoteKey;
        Debug.Log("Create a json with clipboard Key in RemoteConfig");
    }

    [MenuItem("SimZone/Monetization/RemoteSettings/ClipboardJson", false, 1)]
    public static void ClipboardJson()
    {
        GUIUtility.systemCopyBuffer = JsonUtility.ToJson(new AdsRemoteSettings());
        Debug.Log("Now, Paste the json to RemoteConfig");
    }
}