using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using Firebase.RemoteConfig;

public static class FirebaseInitializer
{
    internal static bool hasInitialized;
    internal static void Initialize(Action onInitialized)
    {
        FirebaseApp.LogLevel = AdConstants.UseLogs ? LogLevel.Info : LogLevel.Error;
        DebugAds.Log(DebugTag.Firebase, $"Initializing...");
        hasInitialized = false;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {            
                DebugAds.Log(DebugTag.Firebase, "Successfully initialized");
                FirebaseAnalyticsX.InitAnalytics();
                FirebaseRemote.InitRemoteConfig();
                hasInitialized = true;
            }
            onInitialized.Invoke();

        });
    }
}