using UnityEngine;
using GoogleMobileAds.Api;
using System;
using System.Collections.Generic;
using GoogleMobileAds.Common;
using static AnalyticsManager;

public class AdNetworkAdmob : AdNetworkBase
{
    #region Fields & Properties
    BannerView bannerView, mrecView;
    InterstitialAd interstitialAd;
    RewardedAd rewardedAd;
    AppOpenAd appOpenAd;

    bool m_IsShowingAppOpenAd;

    bool BannerStatus;
    bool LoadingAppOpen;
    //bool BannerEverLoaded;
    [System.NonSerialized] public bool ShowAppOpenOnLoad;
    [System.NonSerialized] public bool IsLoadingCollapsible;
    AdImpressionData bannerImp, mrecImp, interImp, rewardedImp, appOpenImp;

    string interAdUnitId;
    string rewardAdUnitId;

    #endregion

    #region SDK Initialize
    public override void Initialize()
    {
        DebugAds.Log(DebugTag.Admob, "Initializing...");
        SetConfigurations();
        MobileAds.SetiOSAppPauseOnBackground(true);
        MobileAds.RaiseAdEventsOnUnityMainThread = false;
        MobileAds.Initialize(HandleInitCompleteAction);
        
        if(MobileAdsEventExecutor.instance != null)
            Destroy(MobileAdsEventExecutor.instance.gameObject);
    }

    void SetConfigurations()
    {
        MobileAds.SetRequestConfiguration(new RequestConfiguration
        {
            TagForUnderAgeOfConsent = TagForUnderAgeOfConsent.False,
            TagForChildDirectedTreatment = TagForChildDirectedTreatment.False
        });
    }

    private void HandleInitCompleteAction(InitializationStatus status)
    {
        ThreadDispatcher.Enqueue(() =>
        {
            RequestRewardedAd();
            if (!AdConstants.AdsRemoved)
            {
                RequestMREC();
                RequestBanner();
                RequestInterstitial();
                RequestAppOpenAd();
            }
            
            DebugAds.Log(DebugTag.Admob, "Successfully initialized");
            isInitialized = true;
        });
    }

    #endregion

    #region Banner Ad
    public void ShowBanner()
    {
        BannerStatus = true;
        if (!isInitialized) return;

        if (bannerView != null)
            bannerView.Show();
        else
            RequestBanner();
    }

    void RequestBanner()
    {
        IsLoadingCollapsible = AdsManager.Instance.UseAdmobCollapsible;
        string adUnitId = AdsManager.Instance.TestAds ? AdConstants.GetAdmobTestID(IsLoadingCollapsible ? AdFormatX.CollapsibleBanner : AdFormatX.Banner) : AdsManager.Instance.BannerID;
        if (string.IsNullOrEmpty(adUnitId)) return;

        int width = (AdsManager.Instance.BannerSize == BannerWidth.Full) ? AdSize.FullWidth : 320;
        AdSize adSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(width);

        if (bannerView != null)
            bannerView.Destroy();

        bannerImp = new AdImpressionData(adUnitId, AdFormatX.Banner);
        bannerView = new BannerView(adUnitId, adSize, AdsManager.Instance.AdmobBannerPos);

        bannerView.OnAdFullScreenContentClosed += BannerView_OnAdFullScreenContentClosed;
        bannerView.OnBannerAdLoadFailed += BannerView_OnBannerAdLoadFailed;
        bannerView.OnBannerAdLoaded += BannerView_OnBannerAdLoaded;
        bannerView.OnAdClicked += BannerView_OnAdClicked;
        bannerView.OnAdPaid += BannerView_OnAdPaid;

        AdRequest request = CreateAdRequest();

        if (IsLoadingCollapsible)
        {
            AdsManager.Instance.UseAdmobCollapsible = false;
            string pos = AdsManager.Instance.AdmobBannerPos.ToString().Contains("Top") ? "top" : "bottom";
            request.Extras.Add("collapsible", pos);
        }

        bannerView.LoadAd(request);
    }

    private void BannerView_OnBannerAdLoaded()
    {
        ThreadDispatcher.Enqueue(() =>
        {
            if (!BannerStatus)
            {
                HideBanner();
                return;
            }

            if (IsLoadingCollapsible)
            {
                if (bannerView.IsCollapsible() && bannerImp.Format != AdFormatX.CollapsibleBanner)
                    bannerImp.Format = AdFormatX.CollapsibleBanner;
                else
                {
                    IsLoadingCollapsible = false;
                    Invoke(nameof(HideBannerWithDelay), 3f);
                }
            }
        });
    }

    private void BannerView_OnBannerAdLoadFailed(LoadAdError obj)
    {
        if (IsLoadingCollapsible)
        {
            IsLoadingCollapsible = false;
            ThreadDispatcher.Enqueue(() =>
            {
                AdsManager.Instance.ShowBanner();
            });
        }
    }

    private void BannerView_OnAdPaid(AdValue value)
    {
        FirebaseAnalyticsX.SendRevenueAdmob(value, bannerImp);
        ThreadDispatcher.Enqueue(() =>
        {
            AnalyticsManager.ReportRevenue_Admob(value, bannerImp);
        });
    }

    private void BannerView_OnAdClicked()
    {
        AdsManager.Instance.ExtendAppOpenTime();
    }

    private void BannerView_OnAdFullScreenContentClosed()
    {
        if (IsLoadingCollapsible)
        {
            bannerImp.Format = AdFormatX.Banner;
            ThreadDispatcher.Enqueue(() =>
            {
                Invoke(nameof(HideBannerWithDelay), 3f);
            });
        }
        IsLoadingCollapsible = false;
    }

    void HideBannerWithDelay()
    {
        if (AdsManager.Instance.BannerStatus)
        {
            HideBanner();
            AdsManager.Instance.ShowBanner();
        }
    }

    public void HideBanner()
    {
        BannerStatus = false;
        if (bannerView != null)
            bannerView.Hide();
    }

    public void DestroyBanner()
    {
        BannerStatus = false;
        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }
    }

    public void SetPosition(AdPosition pos)
    {
        if (bannerView != null)
            bannerView.SetPosition(pos);
    }

    #endregion

    #region MREC Ad

    public void ShowMREC()
    {
        if (!isInitialized) return;

        if (mrecView != null)
            mrecView.Show();
        else
            RequestMREC();
    }

    void RequestMREC()
    {
        string adUnitId = AdsManager.Instance.TestAds ? AdConstants.GetAdmobTestID(AdFormatX.MRec) : AdsManager.Instance.MrecID;
        if (string.IsNullOrEmpty(adUnitId)) return;

        if (mrecView != null)
            mrecView.Destroy();

        mrecImp = new AdImpressionData(adUnitId, AdFormatX.MRec);
        mrecView = new BannerView(adUnitId, new AdSize(300, 250), AdsManager.Instance.AdmobMrecPos);

        mrecView.OnBannerAdLoadFailed += MrecView_OnBannerAdLoadFailed;
        mrecView.OnBannerAdLoaded += MrecView_OnBannerAdLoaded;
        mrecView.OnAdPaid += MrecView_OnAdPaid;
        mrecView.LoadAd(CreateAdRequest());
    }

    private void MrecView_OnBannerAdLoaded()
    {
        ThreadDispatcher.Enqueue(() =>
        {
            if (!AdsManager.Instance.MrecStatus)
                HideMREC();
        });
    }

    private void MrecView_OnBannerAdLoadFailed(LoadAdError obj)
    {
        ThreadDispatcher.Enqueue(() =>
        {
            AdsManager.Instance.DestroyMREC();
        });
    }

    private void MrecView_OnAdPaid(AdValue value)
    {
        ThreadDispatcher.Enqueue(() =>
        {
            AnalyticsManager.ReportRevenue_Admob(value, mrecImp);
        });
    }

    public void HideMREC()
    {
        if (mrecView != null)
            mrecView.Hide();
    }

    public void DestroyMREC()
    {
        if (mrecView != null)
        {
            mrecView.Destroy();
            mrecView = null;
        }
    }

    #endregion

    #region Interstitial Ad
    public void ShowInterstitial()
    {
        interstitialAd.Show();
    }

    public override bool HasInterstitial(bool doRequest)
    {
        if (!isInitialized) return false;

        if (interstitialAd != null && interstitialAd.CanShowAd())
            return true;
        else
        {
            if (doRequest)
                RequestInterstitial();
            return false;
        }
    }

    void RequestInterstitial()
    {
        interAdUnitId = AdsManager.Instance.TestAds ? AdConstants.GetAdmobTestID(AdFormatX.Interstitial) : AdsManager.Instance.InterstitialID;
        if (string.IsNullOrEmpty(interAdUnitId)) return;

        if (interstitialAd != null)
            interstitialAd.Destroy();

        interImp = new AdImpressionData(interAdUnitId, AdFormatX.Interstitial);
        InterstitialAd.Load(interAdUnitId, CreateAdRequest(), InterstitialLoadCallback);
    }

    void InterstitialLoadCallback(InterstitialAd ad, LoadAdError loadError)
    {
        ThreadDispatcher.Enqueue(() =>
        {
            if (loadError == null && ad != null) // Success
            {
                interstitialAd = ad;
                interstitialAd.OnAdPaid += InterstitialAd_OnAdPaid;
                interstitialAd.OnAdFullScreenContentClosed += InterstitialAd_OnAdFullScreenContentClosed;
            }
            //else Failed 
        });
    }

    private void InterstitialAd_OnAdPaid(AdValue value)
    {
        FirebaseAnalyticsX.SendRevenueAdmob(value, interImp);
        ThreadDispatcher.Enqueue(() =>
        {
            AnalyticsManager.ReportRevenue_Admob(value, interImp);
        });
    }

    private void InterstitialAd_OnAdFullScreenContentClosed()
    {
        ThreadDispatcher.Enqueue(() =>
        {
            RequestInterstitial();
        });
    }

    #endregion

    #region Rewarded Ad

    public void ShowRewardedAd()
    {
        rewardedAd.Show(RewardedAd_OnUserEarnedReward);
    }

    public override bool HasRewarded(bool doRequest)
    {
        if (!isInitialized) return false;

        if (rewardedAd != null && rewardedAd.CanShowAd())
            return true;
        else
        {
            if (doRequest)
                RequestRewardedAd();
            return false;
        }
    }

    public void RequestRewardedAd()
    {
        rewardAdUnitId = AdsManager.Instance.TestAds ? AdConstants.GetAdmobTestID(AdFormatX.Rewarded) : AdsManager.Instance.RewardedID;
        if (string.IsNullOrEmpty(rewardAdUnitId)) return;

        if (rewardedAd != null)
            rewardedAd.Destroy();

        rewardedImp = new AdImpressionData(rewardAdUnitId, AdFormatX.Rewarded);
        RewardedAd.Load(rewardAdUnitId, CreateAdRequest(), RewardedLoadCallback);
    }

    void RewardedLoadCallback(RewardedAd ad, LoadAdError loadError)
    {
        ThreadDispatcher.Enqueue(() =>
        {
            if (loadError == null && ad != null) // Success
            {
                rewardedAd = ad;
                rewardedAd.OnAdPaid += RewardedAd_OnAdPaid;
                rewardedAd.OnAdFullScreenContentClosed += RewardedAd_OnAdFullScreenContentClosed;
            }
            //else Failed
        });
    }

    private void RewardedAd_OnAdPaid(AdValue value)
    {
        FirebaseAnalyticsX.SendRevenueAdmob(value, rewardedImp);
        ThreadDispatcher.Enqueue(() =>
        {
            AnalyticsManager.ReportRevenue_Admob(value, rewardedImp);
        });
    }

    private void RewardedAd_OnAdFullScreenContentClosed()
    {
        ThreadDispatcher.Enqueue(() =>
        {
            RequestRewardedAd();
        });
    }

    private void RewardedAd_OnUserEarnedReward(Reward e)
    {
        ThreadDispatcher.Enqueue(() =>
        {
            AdsManager.Instance.InvokeReward();
        });
    }

    #endregion

    #region AppOpen

    public void RequestAppOpenAd()
    {
        string adUnitId = AdsManager.Instance.TestAds ? AdConstants.GetAdmobTestID(AdFormatX.AppOpen) : AdsManager.Instance.AppOpenID;
        if (string.IsNullOrEmpty(adUnitId)) return;

        LoadingAppOpen = true;
        appOpenImp = new AdImpressionData(adUnitId, AdFormatX.AppOpen);
        if (appOpenAd != null)
            appOpenAd.Destroy();

        AppOpenAd.Load(adUnitId, CreateAdRequest(), AppOpenResponse);
    }

    public void AppOpenResponse(AppOpenAd ad, LoadAdError error)
    {
        ThreadDispatcher.Enqueue(() =>
        {
            LoadingAppOpen = false;
            if (error == null && ad != null)
            {
                appOpenAd = ad;
                appOpenAd.OnAdFullScreenContentOpened += AppOpenAd_OnAdFullScreenContentOpened;
                appOpenAd.OnAdFullScreenContentClosed += AppOpenAd_OnAdFullScreenContentClosed;
                appOpenAd.OnAdFullScreenContentFailed += AppOpenAd_OnAdFullScreenContentFailed;
                appOpenAd.OnAdPaid += AppOpenAd_OnAdPaid;

                if (ShowAppOpenOnLoad)
                {
                    ShowAppOpenOnLoad = false;
                    if (!IsLoadingCollapsible)
                        ShowAppOpen();
                }
            }
            else
                DestroyAppOpen();
        });
    }

    private void AppOpenAd_OnAdFullScreenContentOpened()
    {
        ThreadDispatcher.Enqueue(() =>
        {
            m_IsShowingAppOpenAd = true;
        });
    }

    private void AppOpenAd_OnAdFullScreenContentClosed()
    {
        ThreadDispatcher.Enqueue(() =>
        {
            DestroyAppOpen();
            RequestAppOpenAd();

            AdsManager.Instance.ResumeAllBanners();
        });
    }

    private void AppOpenAd_OnAdFullScreenContentFailed(AdError obj)
    {
        ThreadDispatcher.Enqueue(DestroyAppOpen);
    }

    private void AppOpenAd_OnAdPaid(AdValue value)
    {
        FirebaseAnalyticsX.SendRevenueAdmob(value, appOpenImp);
        ThreadDispatcher.Enqueue(() =>
        {
            AnalyticsManager.ReportRevenue_Admob(value, appOpenImp);
        });
    }

    public void ShowAppOpen()
    {
        if (!isInitialized || m_IsShowingAppOpenAd) return;

        if (appOpenAd != null)
        {
            AdsManager.Instance.HideAllBanners();
            appOpenAd.Show();
        }
        else if (!LoadingAppOpen)
            RequestAppOpenAd();
    }

    void DestroyAppOpen()
    {
        m_IsShowingAppOpenAd = false;

        if (appOpenAd != null)
        {
            appOpenAd.Destroy();
            appOpenAd = null;
        }
    }

    #endregion

    #region Others
    AdRequest CreateAdRequest()
    {
        var req = new AdRequest();
        req.Extras.Add("npa", AdmobConsentController.isPersonalized ? "0" : "1");
        return req;
    }
    #endregion
}