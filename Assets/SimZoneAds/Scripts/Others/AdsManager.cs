
using System;
using System.Collections;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

public class AdsManager : MonoBehaviour
{
    public const string Version = "4.4.0 Production";
    public const string Release_Date = "February 25, 2025";

    #region Instance

    public static AdsManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //Debug.unityLogger.logEnabled = AdConstants.UseLogs;
        }
        else
            Destroy(gameObject);
    }

    #endregion

    #region Fields

    [Header("Settings")][SerializeField] bool ShowAppopenOnLoad = true;
    public bool TestAds = false;
    bool Initialized = false;

    [Header("References")][SerializeField] AdNetworkAdmob Admob;

    
    [SerializeField] InternetPanelManager InternetPanel;
    public AdmobConsentController AdmobConsent;


    [Header("Admob IDs")] public string AppID;
    public string BannerID, MrecID, InterstitialID, RewardedID, AppOpenID;

    [Header("AppLovin IDs")]
    [SerializeField]
    //string MaxSDKKey;

    //public string MaxBanner;
    //public string MaxMrec;
    //public string MaxInterstitial;
    //public string MaxRewarded;
    //public string MaxAppopen;

    [Header("Banner Settings")] public BannerWidth BannerSize = BannerWidth.Full;
    public AdPosition BannerPos;
    public AdPosition MrecPos;

    [Header("Links")][SerializeField] string PrivacyPolicy;

    [Header("Remote Configuration")]
    [SerializeField]
    AdsRemoteSettings DefaultAdsSettings;

    bool BannerWasActive;
    bool MrecWasActive;

    public bool IsLoadingCollapsible => Admob.IsLoadingCollapsible;
    [HideInInspector] public bool UseAdmobCollapsible;
    [HideInInspector] public bool UseAdmobAppopen;
    public bool UseAdmobSDK;
    CheckInternetOverTime InternetChecker = new CheckInternetOverTime();
    ThreadDispatcher Dispatcher = new ThreadDispatcher();
    MethodInvoker invoker = new MethodInvoker();

    #endregion

    #region Properties

    public bool isTablet => false;

    public int GetBannerHeight => 100;

    //public string RateUsLink => "https://play.google.com/store/apps/details?id=" + Application.identifier;
    public bool HasInterstitial =>
        (Admob.HasInterstitial(false)) && ReadyForNextInterstitial;

    public bool HasRewarded =>Admob.HasRewarded(false);

    public bool BannerStatus { get; private set; }
    public bool MrecStatus { get; private set; }

    //[HideInInspector] public bool MrecStatus = false;
    public Action OnMaxBannerLoaded { get; private set; }
    public Action OnMaxBannerFailed { get; private set; }

    Action OnRewardComplete;

    static AdsRemoteSettings m_Remote;

    public static AdsRemoteSettings RemoteSettings
    {
        get
        {
            if (m_Remote == null)
                m_Remote = JsonUtility.FromJson<AdsRemoteSettings>(FirebaseRemote
                    .GetValueString(AdsRemoteSettings.RemoteKey).Value);
            return m_Remote;
        }
        set { m_Remote = value; }
    }

    public AdPosition AdmobBannerPos
    {
        get
        {
            switch (BannerPos)
            {
                case AdPosition.TopLeft: return AdPosition.TopLeft;
                case AdPosition.Top: return AdPosition.Top;
                case AdPosition.TopRight: return AdPosition.TopRight;
                case AdPosition.BottomLeft: return AdPosition.BottomLeft;
                case AdPosition.BottomRight: return AdPosition.BottomRight;
                default: return AdPosition.Center;
            }
        }
    }

    public AdPosition AdmobMrecPos
    {
        get
        {
            switch (MrecPos)
            {
                case AdPosition.TopLeft: return AdPosition.TopLeft;
                case AdPosition.Top: return AdPosition.Top;
                case AdPosition.TopRight: return AdPosition.TopRight;
                case AdPosition.BottomLeft: return AdPosition.BottomLeft;
                case AdPosition.BottomRight: return AdPosition.BottomRight;
                default: return AdPosition.Center;
            }
        }
    }

    #endregion

    #region Delegates

    //public delegate void OnAdnetworkInit();
    //public static OnAdnetworkInit OnAdmobInitSuccess;
    //public static OnAdnetworkInit OnApplovinInitSuccess;

    #endregion

    #region Initialization

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        FirebaseRemote.AddOrUpdateValue(AdsRemoteSettings.RemoteKey, JsonUtility.ToJson(DefaultAdsSettings));
        SetAppOpenAutoShow(ShowAppopenOnLoad);
    }

    public void Initialize()
    {
        if (!Initialized)
        {
            Debug.Log($"Initializing KZMonetization v{Version}");
            Initialized = true;
            InternetPanel.Initialize();
            FirebaseInitializer.Initialize(GatherConsent);
            MethodInvoker.InvokeDelayed(delegate { OnConsentReceived("Consent Skipped"); }, 30, false);

#if kz_gameanalytics_enabled
        GameAnalyticsSDK.GameAnalytics.Initialize();
#endif
        }
        Admob.Initialize();
    }

    void GatherConsent()
    {
        AdmobConsent.GatherConsent(false, false, OnConsentReceived);
    }

    private bool recevied = false;

    void OnConsentReceived(string message)
    {
        if (recevied) return;

        ThreadDispatcher.Enqueue(() =>
        {
            recevied = true;
            DebugAds.Log(DebugTag.Consent, message);
            InitializeMax();
        });
    }


    void ReportAnalytics()
    {
        bool hasReported = PlayerPrefs.GetString("AppVersion", "").Equals(Application.version);
        if (!hasReported)
        {
            PlayerPrefs.SetString("AppVersion", Application.version);

            //int DeviceRam = Mathf.Clamp(Mathf.CeilToInt(Ram()), 0, 8);
            //string RamInGB = $"{DeviceRam}GB";

            //if (ConsentManager.isEurArea)
            //AnalyticsManager.ReportCustomEvent(AnalyticsType.Extras, "Consent", ConsentManager.isPersonalized ? "Accepted" : "Denied");

            //GameAnalyticsSDK.GameAnalytics.NewDesignEvent($"{AnalyticsType.Extras.ToString()}:Ram:{RamInGB}");

            AnalyticsManager.ReportCustomEvent(AnalyticsType.Monetization, "Version", Version, Release_Date);
            //GameAnalyticsSDK.GameAnalytics.SetCustomDimension01(RamInGB);
            //GameAnalyticsSDK.GameAnalytics.SetCustomDimension02(ConsentInformation.ConsentStatus.ToString());
        }
    }

    #region Admob + Applovin

    public void InitializeAdmob()
    {
        UseAdmobCollapsible = RemoteSettings.UseAdmobCollapsible;
        UseAdmobAppopen = RemoteSettings.UseAdmobAppopen;
        UseAdmobSDK = RemoteSettings.UseAdmobSDK;

        //if (UseAdmobAppopen)
        //    MaxAppopen = null;
        //else
            AppOpenID = null;

        if (!UseAdmobSDK)
        {
            InterstitialID = null;
            RewardedID = null;
            if (!UseAdmobCollapsible)
                BannerID = null;
        }

        //if (UseAdmobSDK || UseAdmobAppopen || UseAdmobCollapsible)
        //    Admob.Initialize();

    }

    private bool flag = false;

    void InitializeMax()
    {
        if (flag) return;

        flag = true;
        if (RemoteSettings.DisableMaxOn2GBDevices)
        {
            if (SystemInfo.systemMemorySize < 2048)
            {
                FirebaseAnalyticsX.UpdateConsentStatus();
                InitializeAdmob();
                DebugAds.LogWarning(DebugTag.Applovin, "Failed to initialize MAX because of 2GB device!");
                return;
            }
        }

        OnMaxBannerLoaded = delegate
        {
            if (IsLoadingCollapsible) return;

            if (BannerStatus)
                Admob.ShowBanner();
            else
                Admob.HideBanner();
        };

        OnMaxBannerFailed = delegate
        {
            if (IsLoadingCollapsible) return;

            Admob.HideBanner(); // Max banner does not hides when it fails to load new one!
            if (BannerStatus)
                Admob.ShowBanner();
            else
                Admob.HideBanner();
        };

       
    }

   

    private void LateUpdate()
    {
        invoker.ExecuteDelayedActions();
        Dispatcher.ExecuteEvents();
        InternetChecker.CheckInternet();
    }

    public void SetInternetCallback(Action<bool> onStatusChange)
    {
        InternetChecker.SetCallback(onStatusChange);
    }

    #endregion

    #endregion

    #region Banner

    public void ShowBanner()
    {
        if (!AdConstants.AdsRemoved)
        {
            BannerStatus = true;
            if (UseAdmobCollapsible)
                Admob.ShowBanner();
            else
            {
                    Admob.ShowBanner();
            }
        }
    }

    public void HideBanner()
    {
        BannerStatus = false;
        Admob.HideBanner();
    }

    public void DestroyBanner()
    {
        BannerStatus = false;
        Admob.DestroyBanner();
    }

    //public void RepositionBanner(MaxSdkBase.BannerPosition pos)
    //{
    //    if (BannerPos != pos)
    //    {
    //        BannerPos = pos;
    //        Applovin.UpdateBannerPosition(BannerPos);
    //        Admob.SetPosition(AdmobBannerPos);
    //    }
    //}

    #endregion

    #region MREC

    public void ShowMREC()
    {
        if (!AdConstants.AdsRemoved)
        {
            Admob.ShowMREC();
        }
    }

    public void HideMREC()
    {
        Admob.HideMREC();
    }

    public void DestroyMREC()
    {
        Admob.DestroyMREC();
    }

    //public void RepositionMREC(MaxSdkBase.AdViewPosition pos)
    //{
    //    if (MrecPos != pos)
    //    {
    //        MrecPos = pos;
    //        Applovin.UpdateMrecPosition(pos);
    //    }
    //}

    #endregion

    #region Banners Status

    public void HideAllBanners()
    {
        BannerWasActive = BannerStatus;
        MrecWasActive = false;
        HideBanner();
        HideMREC();
    }

    public void ResumeAllBanners()
    {
        if (BannerWasActive)
            ShowBanner();

        if (MrecWasActive)
            ShowMREC();
    }

    #endregion

    #region Interstital

    public void ShowInterstitial(string placementName, bool staticAd = false)
    {
        AnalyticsManager.PlacementName = placementName;
        if (!AdConstants.AdsRemoved && ReadyForNextInterstitial)
        {
            if (!staticAd && Admob.HasInterstitial(true))
            {
                ExtendOtherAdFormatsTime();
                Admob.ShowInterstitial();
            }
            else if (Admob.HasInterstitial(true))
            {
                ExtendOtherAdFormatsTime();
                Admob.ShowInterstitial();
            }
        }
    }

    float InterstitialTimer = 0;
    bool ReadyForNextInterstitial => Time.time > InterstitialTimer;

    public void ExtendInterstitialTime()
    {
        InterstitialTimer = Time.time + RemoteSettings.NextInterstitialDelay;
    }

    public void ResetInterstitialTime()
    {
        InterstitialTimer = 0;
    }

    #endregion

    #region Rewarded

    public bool ShowRewarded(Action UserReward, string placementName)
    {
        if (!AdConstants.InternetAvailable)
        {
            MobileToast.Show("Sorry, No Internet Connection!", true);
            return false;
        }

        if (ReadyForNextRewarded) // Used to avoid invalid rewarded clicks for 2 seconds.
        {
            ExtendRewardedTime();
            OnRewardComplete = UserReward;
            AnalyticsManager.PlacementName = placementName;

            if (Admob.HasRewarded(true))
            {
                ExtendOtherAdFormatsTime();
                Admob.ShowRewardedAd();
                return true;
            }
            else if (Admob.HasRewarded(true))
            {
                ExtendOtherAdFormatsTime();
                Admob.ShowRewardedAd();
                return true;
            }

            MobileToast.Show("Video Ad not Available!", false);
        }

        return false;
    }

    public void InvokeReward()
    {
        if (OnRewardComplete != null)
            OnRewardComplete.Invoke();
        OnRewardComplete = null;
    }

    float RewardedTimer = 0;
    bool ReadyForNextRewarded => Time.unscaledTime > RewardedTimer;

    public void ExtendRewardedTime()
    {
        RewardedTimer = Time.unscaledTime + 1.5f;
    }

    #endregion

    #region AppOpen

    float AppOpenTimer = 3;
    int NextAppOpenDelay = 5;

    public void SetAppOpenAutoShow(bool value)
    {
        if (Application.isEditor) return;
        if (!AdmobConsent.hasUserConsent) return;

        if (!value)
        {
            Admob.ShowAppOpenOnLoad = false;
            return;
        }

        if (RemoteSettings.UseAdmobAppopen)
            Admob.ShowAppOpenOnLoad = true;
        else
            Admob.ShowAppOpenOnLoad = true;
    }

    public void ExtendAppOpenTime()
    {
        AppOpenTimer = Time.time + NextAppOpenDelay;
    }

    private void OnApplicationPause(bool pause)
    {
        DebugAds.Log(DebugTag.AppCycle, $"OnApplicationPause : {pause}");
        if (!pause)
            ShowAppOpen();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        DebugAds.Log(DebugTag.AppCycle, $"OnApplicationFocus : {hasFocus}");
    }

    public void ShowAppOpen()
    {
        if (Time.time > AppOpenTimer && !AdConstants.AdsRemoved)
        {
            ThreadDispatcher.Enqueue(() =>
            {
                ExtendAppOpenTime();
                if (UseAdmobAppopen)
                    Admob.ShowAppOpen();
                //MobileToast.Show_DevMode($"AppOpen Left : {(AppOpenTimer - Time.time).ToString("F2")}s");
            });
        }
    }

    #endregion

    #region Misc

    void ExtendOtherAdFormatsTime()
    {
        ExtendAppOpenTime();
        ExtendInterstitialTime();
    }

    public void VisitPrivacyPolicy()
    {
        ExtendAppOpenTime();

        if (string.IsNullOrEmpty(PrivacyPolicy)) MobileToast.Show("PrivacyPolicy is missing!", false);
        else Application.OpenURL(PrivacyPolicy);
    }

    public void ShowInternetPanel(bool value = true)
    {
        InternetPanel.Hide(!value);
    }

    #endregion

    #region GAID

    public string GetAdvertisingID()
    {

        try
        {
            StartCoroutine(GetAdId());
        }
        catch (System.Exception)
        {

        }
        return null;
    }

    

    IEnumerator GetAdId()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        var advertisingIdClient = new AndroidJavaClass("com.google.android.gms.ads.identifier.AdvertisingIdClient");
        var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
                        .GetStatic<AndroidJavaObject>("currentActivity");
        var adInfo = advertisingIdClient.CallStatic<AndroidJavaObject>("getAdvertisingIdInfo", activity);
        string adId = adInfo.Call<string>("getId");
        bool limitAdTracking = adInfo.Call<bool>("isLimitAdTrackingEnabled");

        Debug.Log("Ad ID: " + adId);
        Debug.Log("Limit Ad Tracking Enabled: " + limitAdTracking);
#else
        Debug.Log("Advertiser ID can only be fetched on an actual Android device.");
#endif
        yield return null;
    }
    #endregion

    #region OnDestroy

    private void OnDestroy()
    {
        m_Remote = null;
    }

    #endregion

#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        FirebaseRemote.defaults = new System.Collections.Generic.Dictionary<string, object>();
        Instance = null;
    }
#endif
}

#region Enums

public enum BannerWidth
{
    Full,
    Half
}

#endregion