using System;
using System.Collections;
using System.Text;
using UnityEngine;
using Firebase.Analytics;
using System.Collections.Generic;
using GoogleMobileAds.Api;

public static class AnalyticsManager
{
    static StringBuilder stringBuilder = new StringBuilder();
    public static string PlacementName = "none";

    #region PaidEvents - Firebase, Admob, Adjust & Appmetrica

    public static void ReportRevenue_Admob(AdValue admobAd, AdImpressionData data)
    {
        if (admobAd == null) return;
        double revenue = (admobAd.Value / 1000000f);

      
        //Rev Event for Appmetrica

        IDictionary<string, string> payload = new Dictionary<string, string>();
        //payload.Add($"Ads | {AdsManager.Version}", "Admob");

        //YandexAppMetricaAdRevenue rev = new YandexAppMetricaAdRevenue(revenue, "USD");
        //rev.AdType = GetAdType(data.Format);
        //rev.AdUnitId = data.AdUnit;
        //rev.AdNetwork = "Admob_Native";
        //rev.Payload = payload;
        //if (GetPlacementForMetrica(data.Format, out string placement))
        //    rev.AdPlacementName = placement.ToLower();
        //AppMetrica.Instance.ReportAdRevenue(rev);

        //Rev Event for Appsflyer
        //Dictionary<string, string> dic = new Dictionary<string, string>();
        //dic.Add("ad_format", "admob_" + data.Format.ToString());
        //AppsFlyerAdRevenue.logAdRevenue("simple_admob", AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeGoogleAdMob, revenue, "USD", dic);

        //if (data.Format == AdFormat.Interstitial || data.Format == AdFormat.Rewarded)
        //    SendAppsFlyerEvents();
    }

    public static void ReportRevenue_Applovin(AdValue maxAd, AdFormatX format)
    {
        if (maxAd == null) return;
        double revenue = maxAd.Value;

        //Rev Event for Appmetrica

        IDictionary<string, string> payload = new Dictionary<string, string>();
        //payload.Add($"Ads | {AdsManager.Version}", "Applovin");

        //YandexAppMetricaAdRevenue rev = new YandexAppMetricaAdRevenue(revenue, "USD");
        //rev.AdType = GetAdType(format);
        //rev.AdNetwork = maxAd.NetworkName;
        //rev.AdUnitId = maxAd.AdUnitIdentifier;
        //rev.Payload = payload;
        //if (GetPlacementForMetrica(format, out string placement))
        //    rev.AdPlacementName = placement.ToLower();
        //AppMetrica.Instance.ReportAdRevenue(rev);

        //Rev Event for Appsflyer
        //Dictionary<string, string> dic = new Dictionary<string, string>();
        //dic.Add("ad_unit_name", maxAd.AdUnitIdentifier);
        //dic.Add("ad_format", "applovin_" + format.ToString());
        //AppsFlyerAdRevenue.logAdRevenue(maxAd.NetworkName, AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeApplovinMax, revenue, "USD", dic);

        //if (format == AdFormat.Interstitial || format == AdFormat.Rewarded)
        //    SendAppsFlyerEvents();
    }

    //static YandexAppMetricaAdRevenue.AdTypeEnum GetAdType(AdFormatX format)
    //{
    //    switch (format)
    //    {
    //        case AdFormatX.Banner: return YandexAppMetricaAdRevenue.AdTypeEnum.Banner;
    //        case AdFormatX.Interstitial: return YandexAppMetricaAdRevenue.AdTypeEnum.Interstitial;
    //        case AdFormatX.Rewarded: return YandexAppMetricaAdRevenue.AdTypeEnum.Rewarded;
    //        case AdFormatX.AppOpen: return YandexAppMetricaAdRevenue.AdTypeEnum.Other;
    //        case AdFormatX.MRec: return YandexAppMetricaAdRevenue.AdTypeEnum.Mrec;
    //        case AdFormatX.NativeAd: return YandexAppMetricaAdRevenue.AdTypeEnum.Native;
    //        case AdFormatX.CollapsibleBanner: return YandexAppMetricaAdRevenue.AdTypeEnum.Banner;
    //        default: return YandexAppMetricaAdRevenue.AdTypeEnum.Other;
    //    }
    //}

    static bool GetPlacementForMetrica(AdFormatX format, out string placement)
    {
        placement = null;
        switch (format)
        {
            case AdFormatX.Interstitial: placement = PlacementName; return true;
            case AdFormatX.Rewarded: placement = PlacementName; return true;
            case AdFormatX.Banner: placement = "adaptive"; return true;
            case AdFormatX.CollapsibleBanner: placement = "collapsible"; return true;
            default: return false;
        }
    }

    #endregion

    #region CustomEvents - Appmetrica

    public static void ReportCustomEvent(AnalyticsType type, params string[] data)
    {
        //if (!data.IsNullOrEmpty())
            //AppMetrica.Instance.ReportEvent(type.ToString(), GetJsonFromParams(data));
    }

    #endregion

    #region RemoteConfig - Appmetrica
    public static void ReportAdsRemoteConfig(string adsJson)
    {
        //AppMetrica.Instance.ReportEvent(nameof(AnalyticsType.Monetization), $"{{\"RemoteSettings\":{adsJson}}}");
    }
    #endregion

    #region Extensions
    static bool IsNullOrEmpty(this string[] value)
    {
        if (value != null)
        {
            return value.Length == 0;
        }
        return true;
    }

    static string GetJsonFromParams(params string[] data)
    {
        stringBuilder.Clear();
        int dataLength = data.Length;
        for (int i = 0; i < dataLength; i++)
        {
            if (i == dataLength - 1)
            {
                stringBuilder.Append($"\"{data[i]}\"");
                break;
            }
            stringBuilder.Append($"{{\"{data[i]}\":");
        }
        stringBuilder.Append('}', data.Length - 1);
        return stringBuilder.ToString();
    }
    #endregion

    #region Appsflyer
    //static int ImpressionCount
    //{
    //    get
    //    {
    //        return PlayerPrefs.GetInt("ImpressionCount", 0);
    //    }
    //    set
    //    {
    //        PlayerPrefs.SetInt("ImpressionCount", value);
    //    }
    //}


    //static TimeSpan TimeSpan()
    //{
    //    DateTime FirstOpen, CurrentDate;
    //    if (!PlayerPrefs.HasKey("FirstOpenDate"))
    //        PlayerPrefs.SetString("FirstOpenDate", DateTime.Now.ToBinary().ToString());

    //    long temp = Convert.ToInt64(PlayerPrefs.GetString("FirstOpenDate"));

    //    FirstOpen = DateTime.FromBinary(temp);
    //    CurrentDate = System.DateTime.Now;
    //    return CurrentDate.Subtract(FirstOpen);
    //}
    //static void SendAppsFlyerEvents()
    //{
    //    if (TimeSpan().TotalDays < 1)
    //    {
    //        ImpressionCount++;
    //        if (ImpressionCount > 5 && ImpressionCount <= 20)
    //            AppsFlyer.sendEvent("af_ad_watched_x" + ImpressionCount.ToString(), null);
    //    }
    //}
    #endregion

    public enum AdFormatX { Banner, MRec, Interstitial, Rewarded, AppOpen, NativeAd, CollapsibleBanner }
}

public enum AnalyticsType { Extras, GameData, Monetization }