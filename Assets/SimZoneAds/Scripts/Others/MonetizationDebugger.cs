using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MonetizationDebugger : MonoBehaviour
{
    int logsCount, adsCount, inAppsCount;
    float width = .2f;
    float height = .25f;

    Rect topMid, bottomLeft, bottomRight;
    Color transparent = new Color(1f, 1f, 1f, 0f);

    public MonetizationDebugger()
    {
        var ScreenSize = new Vector2(Screen.width, Screen.height);
        topMid = new Rect((ScreenSize.x / 2f) - (ScreenSize.x * width * .5f), 0, ScreenSize.x * width, ScreenSize.y * height);
        bottomLeft = new Rect(0, ScreenSize.y * (1f - height), ScreenSize.x * width, ScreenSize.y * height);
        bottomRight = new Rect(ScreenSize.x * (1 - width), ScreenSize.y * (1f - height), ScreenSize.x * width, ScreenSize.y * height);
    }

    private void Start()
    {
        Destroy(gameObject, 10);
    }

    void OnGUI()
    {
        GUI.color = transparent;
        if (GUI.Button(bottomRight, "Use Logs"))
        {
            logsCount++;
            if (logsCount.Equals(5))
            {
                AdConstants.EnableLogs();
                Debug.unityLogger.logEnabled = true;
            }
        }

        //if (GUI.Button(topMid, "Remove Ads"))
        //{
        //    adsCount++;
        //    if (adsCount.Equals(5))
        //        AdConstants.RemoveAds();
        //}

        //if (GUI.Button(bottomLeft, "Fake InApps"))
        //{
        //    inAppsCount++;
        //    if (inAppsCount.Equals(5))
        //        AdConstants.EnableFakeInApps();
        //}
    }
}
