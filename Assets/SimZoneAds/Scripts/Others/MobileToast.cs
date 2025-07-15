﻿using UnityEngine;
using System.Runtime.InteropServices;

public static class MobileToast
{
    public static void Show(string message, bool isLong)
    {
        try
        {
#if UNITY_EDITOR
            Debug.Log("Toast : " + message);

#elif UNITY_ANDROID
            using (var toastClass = new AndroidJavaClass("android.widget.Toast"))
            {
                var context = GetUnityActivity();
                if (context != null)
                {
                    var javaToastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", context, message, toastClass.GetStatic<int>(isLong ? "LENGTH_LONG" : "LENGTH_SHORT"));
                    javaToastObject.Call("show");
                }
            }

#elif UNITY_IOS
        showToast(message, isLong);
#endif
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }

#if UNITY_ANDROID
    static AndroidJavaObject GetUnityActivity()
    {
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            return unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        }
    }
#endif

#if UNITY_IOS
        [DllImport ("__Internal")]
		private static extern void showToast(string msg, bool isLong);
#endif
}