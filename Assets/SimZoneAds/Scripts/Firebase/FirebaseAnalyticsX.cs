using Firebase.Analytics;
using GoogleMobileAds.Api;
using System.Collections.Generic;
using UnityEngine;
using static AnalyticsManager;

public static class FirebaseAnalyticsX
{
    public static void InitAnalytics()
    {
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        DebugAds.Log(DebugTag.Firebase, $"Analytics collection enabled");
    }

    #region Consent
    public static void UpdateConsentStatus()
    {
        if (FirebaseInitializer.hasInitialized)
        {
            var status = AdmobConsentController.isPersonalized ? ConsentStatus.Granted : ConsentStatus.Denied;
            DebugAds.Log(DebugTag.Firebase, $"Consent status is {status}");
            var consent = new Dictionary<ConsentType, ConsentStatus>() {
                //{ConsentType.AnalyticsStorage, status},
                {ConsentType.AdStorage, status},
                {ConsentType.AdUserData, status},
                {ConsentType.AdPersonalization, status}
            };

            FirebaseAnalytics.SetConsent(consent);
        }
    }
    #endregion

    #region Ad Revenue
    public static void SendRevenueAdmob(AdValue admobAd, AdImpressionData data)
    {
        if (admobAd == null) return;

        double revenue = (admobAd.Value / 1000000f);
        if (FirebaseInitializer.hasInitialized)
        {
            var impressionParameters = new Parameter[6] {
            new Parameter("ad_platform", "Admob"),
            new Parameter("ad_source", "Admob_Native"),
            new Parameter("ad_unit_name", data.AdUnit),
            new Parameter("ad_format", data.Format.ToString()),
            new Parameter("value", revenue),
            new Parameter("currency", admobAd.CurrencyCode)
            };
            FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
#if UNITY_IOS
            FirebaseAnalytics.LogEvent("paid_ad_impression", impressionParameters);
#endif
        }
    }

    public static void SendRevenueApplovin(AdFormatX format)
    {
//        if (maxAd == null) return;

//        double revenue = maxAd.Revenue;
//        if (FirebaseInitializer.hasInitialized)
//        {
//            var impressionParameters = new Parameter[6] {
//            new Parameter("ad_platform", "AppLovin"),
//            new Parameter("ad_source", maxAd.NetworkName),
//            new Parameter("ad_unit_name", maxAd.AdUnitIdentifier),
//            new Parameter("ad_format", format.ToString()),
//            new Parameter("value", revenue),
//            new Parameter("currency", "USD") // All AppLovin revenue is sent in USD
//            };

//            FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
//#if UNITY_IOS
//            FirebaseAnalytics.LogEvent("paid_ad_impression", impressionParameters);// For iOS
//#endif
//        }
    }
    #endregion

    #region IAP Revenue
    public static void LogIAPRevenue(double price, string currency, string itemName)
    {
        if (FirebaseInitializer.hasInitialized)
        {
            var purchaseParam = new Parameter[3]{
                new Parameter(FirebaseAnalytics.ParameterPrice, price),
                new Parameter(FirebaseAnalytics.ParameterCurrency, currency),
                new Parameter(FirebaseAnalytics.ParameterItemName, itemName)
            };
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventPurchase, purchaseParam);
        }
    }
    #endregion
}