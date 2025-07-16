using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestAds : MonoBehaviour
{
    [SerializeField] Text RewardCounter;
    int rewardCount;

    public void ShowInterstitial()
    {
        AdsManager.Instance.ShowInterstitial("test_ads");
    }

    public void ShowRewarded()
    {
        AdsManager.Instance.ShowRewarded(() =>
        {
            rewardCount++;
            RewardCounter.text = "Reward Count : " + rewardCount.ToString();
        }, "TestAds_Count");
    }

    public void ShowAppOpen()
    {
        AdsManager.Instance.ShowAppOpen();
    }

    public void BannerShow()
    {
        AdsManager.Instance.ShowBanner();
    }

    public void BannerHide()
    {
        AdsManager.Instance.HideBanner();
    }

    public void BannerDestroy()
    {
        AdsManager.Instance.DestroyBanner();
    }

    public void MrecShow()
    {
        AdsManager.Instance.ShowMREC();
    }

    public void MrecHide()
    {
        AdsManager.Instance.HideMREC();
    }
    public void MrecDestroy()
    {
        AdsManager.Instance.DestroyMREC();
    }

    public void ShowPrivacyPolicy()
    {
        AdsManager.Instance.VisitPrivacyPolicy();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
