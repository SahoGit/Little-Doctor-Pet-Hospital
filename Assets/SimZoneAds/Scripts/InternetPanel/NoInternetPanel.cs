using UnityEngine;

class NoInternetPanel : MonoBehaviour
{
    [SerializeField] bool PauseTimeScale = true;
    [SerializeField] bool MuteAudioListner = true;

    private void Start()
    {
        PauseBackground(true);
        AdsManager.Instance.HideAllBanners();
        AnalyticsManager.ReportCustomEvent(AnalyticsType.Monetization, "InternetPopup", "Show");
    }

    private void OnDestroy()
    {
        PauseBackground(false);
        AdsManager.Instance.ResumeAllBanners();
        AnalyticsManager.ReportCustomEvent(AnalyticsType.Monetization, "InternetPopup", "Hide");
    }

    public void Panel_RetryBtn()
    {
        if (AdConstants.InternetAvailable)
            AdsManager.Instance.ShowInternetPanel(false);
        else
            ShowToast();
    }

    void PauseBackground(bool value)
    {
        if (PauseTimeScale) Time.timeScale = value ? 0 : 1;
        if (MuteAudioListner) AudioListener.pause = value;
    }

    #region Toast or Settings
    void ShowToast()
    {
        string currentLanguage = Application.systemLanguage.ToString();
        if (currentLanguage.Equals("English"))
            MobileToast.Show("No Internet Connection!", false);
        else
            OpenWifiSettings();
    }

    void OpenWifiSettings()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");

            AndroidJavaClass wifiManager = new AndroidJavaClass("android.net.wifi.WifiManager");
            AndroidJavaObject wifiService = context.Call<AndroidJavaObject>("getSystemService", "wifi");

            AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent");
            intent.Call<AndroidJavaObject>("setAction", wifiManager.GetStatic<string>("ACTION_PICK_WIFI_NETWORK"));

            currentActivity.Call("startActivity", intent);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            MobileToast.Show("No Internet Connection!", false);
        }
#endif
    }

#endregion
}