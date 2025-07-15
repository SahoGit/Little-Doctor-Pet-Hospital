using System;
using UnityEngine;

public static class DebugAds
{

    public static void Log(DebugTag tag, string message)
    {
        Debug.Log($"[{tag}] {message}");
    }
    public static void LogWarning(DebugTag tag, string message)
    {
        Debug.LogWarning($"[{tag}] {message}");
    }
    public static void LogError(DebugTag tag, string message)
    {
        Debug.LogError($"[{tag}] {message}");
    }
    public static void LogException(DebugTag tag, Exception e)
    {
        LogError(tag, $"Exception > Message : {e.Message}, StackTrace : {e.StackTrace}");
    }
}

public enum DebugTag { Admob, Applovin, Consent, Firebase,Adjust, Appmetrica, GameAnalytics, CustomEvent, InAppPurchasing, InAppReview, Dispatcher,AppCycle }