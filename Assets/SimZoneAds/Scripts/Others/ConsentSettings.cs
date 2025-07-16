using UnityEngine;
using UnityEngine.UI;

class ConsentSettings : MonoBehaviour
{
    [SerializeField] GameObject SettingsBtn;
    bool isLoading = false;
    private void Start()
    {
        bool value = AdmobConsentController.PrivacyOptionsRequired;
        SettingsBtn.gameObject.SetActive(value);
    }

    public void OpenSettings() // Btn Listener
    {
        if (isLoading) return;

        isLoading = true;
        MobileToast.Show("Loading Consent!", false);
        // MaxSdk.CmpService.ShowCmpForExistingUser(error =>
        // {
        //     ThreadDispatcher.Enqueue(() =>
        //     {
        //         isLoading = false;
        //         if (error == null)
        //         {
        //
        //
        //             DebugAds.Log(DebugTag.Applovin, $"Successfully updated for existing user!");
        //         }
        //         else
        //             DebugAds.LogError(DebugTag.Applovin, $"Failed to show consent privacy settings with error.Message : {error.Message} and error.CmpMessage : {error.CmpMessage}");
        //     });
        // });
        
        AdsManager.Instance.AdmobConsent.ShowPrivacyOptionsForm((error) =>
        {
            ThreadDispatcher.Enqueue(() =>
            {
                Start();
                isLoading = false;
                if (error != null)
                    DebugAds.LogError(DebugTag.Consent,$"Failed to show privacy options with error : {error}");
                else
                    ApplyConsentSettings();
            });
        });
    }

    void ApplyConsentSettings()
    {
        FirebaseAnalyticsX.UpdateConsentStatus();
       

        //MaxSdk.SetDoNotSell(!AdmobConsentController.isPersonalized);
        //MaxSdk.SetHasUserConsent(AdmobConsentController.isPersonalized);
    }
}