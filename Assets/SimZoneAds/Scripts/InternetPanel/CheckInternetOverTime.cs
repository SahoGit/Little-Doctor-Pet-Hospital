using System;
using UnityEngine;

public class CheckInternetOverTime
{
    Action<bool> statusChanged;
    float timer = -1;
    bool status = true;

    public void CheckInternet()
    {
        timer += Time.unscaledDeltaTime;
        if (timer > 2)
        {
            timer = 0;

            if (!status.Equals(AdConstants.InternetAvailable))
            {
                status = AdConstants.InternetAvailable;
                statusChanged?.Invoke(status);
            }
        }
    }

    public void SetCallback(Action<bool> onStatusChange)
    {
        this.statusChanged = onStatusChange;
    }
}
