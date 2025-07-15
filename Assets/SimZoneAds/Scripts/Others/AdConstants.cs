using UnityEngine;
using static AnalyticsManager;

public static class AdConstants
{
#if !UNITY_EDITOR
    public static bool InternetAvailable => Application.internetReachability != NetworkReachability.NotReachable;
#else
    static InternetPanelManager internetManager;
    public static bool InternetAvailable
    {
        get
        {
            if (internetManager == null) internetManager = GameObject.FindObjectOfType<InternetPanelManager>();
            if (internetManager != null) return internetManager.InternetAvailable;

            return false;
        }
    }
#endif
    public static bool AdsRemoved => PlayerPrefs.GetInt("RemoveAds", 0).Equals(1);
    public static bool PolicyAccepted => PlayerPrefs.GetInt("PrivacyPolicy", 0).Equals(1);
    public static bool UseLogs => Application.isEditor || PlayerPrefs.GetInt("UseLogs", 0).Equals(1);
    public static bool FakeInApps => Application.isEditor || PlayerPrefs.GetInt("FakeInApps", 0).Equals(1);

    public static void AcceptPolicy()
    {
        PlayerPrefs.SetInt("PrivacyPolicy", 1);
        PlayerPrefs.Save();
    }

    public static void RemoveAds()
    {
        MobileToast.Show("Ads Removed", false);
        PlayerPrefs.SetInt("RemoveAds", 1);
        PlayerPrefs.Save();
    }

    public static void EnableLogs()
    {
        MobileToast.Show("Logs Enabled", false);
        PlayerPrefs.SetInt("UseLogs", 1);
        PlayerPrefs.Save();
    }

    public static void EnableFakeInApps()
    {
        MobileToast.Show("FakeInApps Enabled", false);
        PlayerPrefs.SetInt("FakeInApps", 1);
        PlayerPrefs.Save();
    }

    public static string GetAdmobTestID(AdFormatX format)
    {
        if (format == AdFormatX.AppOpen && !AdsManager.Instance.UseAdmobAppopen)
            return null;

#if UNITY_ANDROID
        switch (format)
        {
            case AdFormatX.CollapsibleBanner: return "ca-app-pub-3940256099942544/2014213617";
            case AdFormatX.Banner: return "ca-app-pub-3940256099942544/6300978111";
            case AdFormatX.MRec: return "ca-app-pub-3940256099942544/6300978111";
            case AdFormatX.Interstitial: return "ca-app-pub-3940256099942544/1033173712";
            case AdFormatX.Rewarded: return "ca-app-pub-3940256099942544/5224354917";
            case AdFormatX.AppOpen: return "ca-app-pub-3940256099942544/9257395921";
            case AdFormatX.NativeAd: return "ca-app-pub-3940256099942544/2247696110";
            default: return null;
        }
#elif UNITY_IOS
        switch (format)
        {
            case AdFormat.CollapsibleBanner: return "ca-app-pub-3940256099942544/2014213617";
            case AdFormat.Banner: return "ca-app-pub-3940256099942544/2934735716";
            case AdFormat.MRec: return "ca-app-pub-3940256099942544/2934735716";
            case AdFormat.Interstitial: return "ca-app-pub-3940256099942544/4411468910";
            case AdFormat.Rewarded: return "ca-app-pub-3940256099942544/1712485313";
            case AdFormat.AppOpen: return "ca-app-pub-3940256099942544/5575463023";
            case AdFormat.NativeAd: return "ca-app-pub-3940256099942544/3986624511";
            default: return null;
        }
#else
        return "unexpected_platform";
#endif
    }
}
