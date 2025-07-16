using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdmobConsentController : MonoBehaviour
{
    #region Properties

    string TestDeviceID = "77BA4C3218DB8850E1C99D5620704ABC_"; // CoreDev Smartphone :)
    Action<string> m_OnComplete;

    public static bool PrivacyOptionsRequired => ConsentInformation.PrivacyOptionsRequirementStatus ==
                                                 PrivacyOptionsRequirementStatus.Required;

    public bool hasUserConsent
    {
        get { return PlayerPrefs.GetInt("UMP_initializeAds", 0).Equals(1); }
        private set { PlayerPrefs.SetInt("UMP_initializeAds", value ? 1 : 0); }
    }

    public static bool isPersonalized
    {
        get { return PlayerPrefs.GetInt("UMP_isPersonalized", 1).Equals(1); }
        private set { PlayerPrefs.SetInt("UMP_isPersonalized", value ? 1 : 0); }
    }

    public static bool isEurArea
    {
        get { return PlayerPrefs.GetInt("UMP_isEurArea", 0).Equals(1); }
        private set { PlayerPrefs.SetInt("UMP_isEurArea", value ? 1 : 0); }
    }

    #endregion

    private bool consentFlag = false;
    private float lastTimeScale = 0;

    #region GatherConsent

    public void GatherConsent(bool debugConsent, bool targetUnderAge, Action<string> onComplete)
    {
        m_OnComplete = onComplete;
        m_OnComplete += delegate
        {
            hasUserConsent = true;
        };

        ConsentInformation.Update(GetRequestParameters(debugConsent, targetUnderAge), (FormError updateError) =>
        {
            if (updateError != null)
            {
                m_OnComplete("Consent UpdateError : " + updateError.Message);
                return;
            }

            if (ConsentInformation.CanRequestAds())
            {
                m_OnComplete("Consent CanRequestAds");
                return;
            }

            consentFlag = true;
            AdsManager.Instance.SetAppOpenAutoShow(false);
            ConsentForm.LoadAndShowConsentFormIfRequired((FormError showError) =>
            {
                if (showError != null)
                    m_OnComplete("Consent ShowError : " + showError.Message);
                else // Success
                {
                    CheckPurposeConsent();
                    m_OnComplete("Consent Obtained Successfully");
                }
            });
        });
    }

    #endregion

    #region ConsentRequestParameters

    ConsentRequestParameters GetRequestParameters(bool debugConsent, bool targetUnderAge)
    {
        var requestParameters = new ConsentRequestParameters();
        requestParameters.TagForUnderAgeOfConsent = targetUnderAge;
        if (debugConsent) requestParameters.ConsentDebugSettings = GetDebugSettings();

        return requestParameters;
    }

    ConsentDebugSettings GetDebugSettings()
    {
        return new ConsentDebugSettings
        {
            DebugGeography = DebugGeography.EEA,
            TestDeviceHashedIds = new List<string>() { TestDeviceID },
            //TestDeviceHashedIds = new List<string>() { TestDeviceID, AdRequest.TestDeviceSimulator },
        };
    }

    #endregion

    #region PurposeConsents

    void CheckPurposeConsent()
    {
        string purposeConsents = ApplicationPreferences.GetString("IABTCF_PurposeConsents");
        int gdprApplies = ApplicationPreferences.GetInt("IABTCF_gdprApplies");

        DebugAds.Log(DebugTag.Consent, $"PurposeConsents = {purposeConsents}, gdprApplies = {gdprApplies}");

        isEurArea = gdprApplies.Equals(1);

        if (!string.IsNullOrEmpty(purposeConsents) && purposeConsents.Length >= 10)
        {
            char[] aquiredPurpose = new char[7]
            {
                purposeConsents[0], purposeConsents[1], purposeConsents[2], purposeConsents[3], purposeConsents[6],
                purposeConsents[8], purposeConsents[9]
            };
            string targetPurpose = "1111111"; // Requirement of Personalization
            isPersonalized = CompareStringWithChars(targetPurpose, aquiredPurpose);
        }
        else
            isPersonalized = false;
    }

    bool CompareStringWithChars(string consent, char[] aquired)
    {
        if (!consent.Length.Equals(aquired.Length)) return false;

        for (int i = 0; i < consent.Length; i++)
        {
            if (!consent[i].Equals(aquired[i]))
                return false;
        }

        return true;
    }

    #endregion

    #region ConsentPrivacyForm

    public void ShowPrivacyOptionsForm(Action<string> onComplete)
    {
        ConsentForm.ShowPrivacyOptionsForm((FormError showError) =>
        {
            if (showError != null)
                onComplete(showError.Message);
            else // Success
            {
                CheckPurposeConsent();
                onComplete(null);
            }
        });
    }

    #endregion

    private void OnApplicationFocus(bool hasFocus)
    {
        if (consentFlag && !hasFocus)
        {
            consentFlag = false;
            lastTimeScale = Time.timeScale;
            Time.timeScale = 0;
            
            DebugAds.Log(DebugTag.AppCycle, $"TimeScale : 0");
            
            m_OnComplete += delegate
            {
                Time.timeScale = lastTimeScale;
                DebugAds.Log(DebugTag.AppCycle, $"TimeScale : {lastTimeScale}");
            };
        }
    }
}