[System.Serializable]
public class AdsRemoteSettings
{
    public const string RemoteKey = "AdsSettings";
    public bool ShowInternetPopup = true;
    public bool UseAdmobSDK = true;
    public bool UseAdmobAppopen = true;
    public bool UseAdmobCollapsible = true;
    public bool DisableMaxOn2GBDevices = true;
    public int NextInterstitialDelay = 10;
    public int BannerRefreshRate = 10;
    public int MRecRefreshRate = 10;
}