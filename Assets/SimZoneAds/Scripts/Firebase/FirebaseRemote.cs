using Firebase.Extensions;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using Firebase.RemoteConfig;
using UnityEngine.UI;

public static class FirebaseRemote
{
    public static event Action OnFetchComplete;
    internal static Dictionary<string, object> defaults = new Dictionary<string, object>(16);
    public static void InitRemoteConfig()
    {
        Debug.Log("Enabling firebase RemoteConfig");
        FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults).ContinueWithOnMainThread(task =>
        {
            FetchDataAsync();
        });
    }

    static Task FetchDataAsync()
    {
        Debug.Log("RemoteConfig Fetching remote data...");
        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    static void FetchComplete(Task fetchTask)
    {
        if (!fetchTask.IsCompleted)
        {
            Debug.LogError("RemoteConfig Retrieval hasn't finished.");
            return;
        }

        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = remoteConfig.Info;
        if (info.LastFetchStatus != LastFetchStatus.Success)
        {
            Debug.LogError($"RemoteConfig FetchComplete was unsuccessful info.LastFetchStatus : {info.LastFetchStatus}");
            return;
        }

        // Fetch successful. Parameter values must be activated to use.
        remoteConfig.ActivateAsync().ContinueWithOnMainThread(
        task =>
        {
            Debug.Log($"RemoteConfig data loaded and ready for use. Last fetch time {info.FetchTime}.");
            ParseData();
            OnFetchComplete?.Invoke();
        });
    }

    static void ParseData()
    {
        string json = GetValueString(AdsRemoteSettings.RemoteKey).Value;
        if (string.IsNullOrEmpty(json)) return;

        if (json.Length > 2)
        {
            AdsManager.RemoteSettings = JsonUtility.FromJson<AdsRemoteSettings>(json);
            AnalyticsManager.ReportAdsRemoteConfig(json);
        }
    }

    public static void AddOrUpdateValue(string key, object value)
    {
        if (!defaults.ContainsKey(key))
        {
            DebugAds.Log(DebugTag.Firebase, $"RemoteKey Added : {key}");
            defaults.Add(key, value);
            return;
        }

        DebugAds.LogWarning(DebugTag.Firebase, $"RemoteKey Updated : {key}");
        defaults[key] = value;
    }

    #region Core
    public static RemoteValue<int> GetValueInteger(string key)
    {
        var result = new RemoteValue<int>(0, ValueSource.StaticValue);
        if (GetValueFromRemote(key, TypeCode.Int32, ref result)) return result;
        if (GetValueFromDefaults(key, ref result)) return result;

        DebugAds.Log(DebugTag.Firebase, $"3rd RemoteValue of key '{key}' is '{result.Value}' with source '{result.Source}'");
        return result;
    }
    public static RemoteValue<bool> GetValueBoolean(string key)
    {
        var result = new RemoteValue<bool>(false, ValueSource.StaticValue);
        if (GetValueFromRemote(key, TypeCode.Boolean, ref result)) return result;
        if (GetValueFromDefaults(key, ref result)) return result;

        DebugAds.Log(DebugTag.Firebase, $"3rd RemoteValue of key '{key}' is '{result.Value}' with source '{result.Source}'");
        return result;
    }
    public static RemoteValue<string> GetValueString(string key)
    {
        var result = new RemoteValue<string>("", ValueSource.StaticValue);
        if (GetValueFromRemote(key, TypeCode.String, ref result)) return result;
        if (GetValueFromDefaults(key, ref result)) return result;

        DebugAds.Log(DebugTag.Firebase, $"3rd RemoteValue of key '{key}' is '{result.Value}' with source '{result.Source}'");
        return result;
    }
    public static RemoteValue<float> GetValueFloat(string key)
    {
        var result = new RemoteValue<float>(0f, ValueSource.StaticValue);
        if (GetValueFromRemote(key, TypeCode.Single, ref result)) return result;
        if (GetValueFromDefaults(key, ref result)) return result;

        DebugAds.Log(DebugTag.Firebase, $"3rd RemoteValue of key '{key}' is '{result.Value}' with source '{result.Source}'");
        return result;
    }

    static bool GetValueFromRemote<T>(string key, TypeCode type, ref RemoteValue<T> value)
    {
        if (Application.isEditor) return false;

        if (FirebaseInitializer.hasInitialized)
        {
            try
            {
                var config = FirebaseRemoteConfig.DefaultInstance.GetValue(key);
                switch (type)
                {
                    case TypeCode.Int32: value = new RemoteValue<T>((T)Convert.ChangeType(config.LongValue, typeof(T)), config.Source); break; // int
                    case TypeCode.Boolean: value = new RemoteValue<T>((T)Convert.ChangeType(config.BooleanValue, typeof(T)), config.Source); break; // bool
                    case TypeCode.String: value = new RemoteValue<T>((T)Convert.ChangeType(config.StringValue, typeof(T)), config.Source); break; // string
                    case TypeCode.Single: value = new RemoteValue<T>((T)Convert.ChangeType(config.DoubleValue, typeof(T)), config.Source); break; // float
                }

                DebugAds.Log(DebugTag.Firebase, $"1st RemoteValue of key '{key}' is '{value.Value}' with source '{value.Source}'");
                return true;
            }
            catch (Exception e)
            {
                DebugAds.LogError(DebugTag.Firebase, $"1st RemoteValue of key '{key}' failed!");
                DebugAds.LogException(DebugTag.Firebase, e);
            }
        }

        return false;
    }

    static bool GetValueFromDefaults<T>(string key, ref RemoteValue<T> value)
    {
        if (defaults.ContainsKey(key))
        {
            value = new RemoteValue<T>((T)defaults[key], ValueSource.DefaultValue);
            DebugAds.Log(DebugTag.Firebase, $"2nd RemoteValue of key '{key}' is '{value.Value}' with source '{value.Source}'");
            return true;
        }

        return false;
    }

    #endregion
}

public struct RemoteValue<T>
{
    public T Value { get; private set; }
    public FetchSource Source { get; private set; }

    public RemoteValue(T value, ValueSource source)
    {
        Value = value;
        Source = (FetchSource)source;
    }
}

public enum FetchSource
{
    StaticValue = 0,
    RemoteValue = 1,
    DefaultValue = 2
}