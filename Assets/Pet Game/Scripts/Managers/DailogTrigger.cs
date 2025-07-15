using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailogTrigger : MonoBehaviour
{
    public GameObject storyPanel;
    public Dailog dailog;
    public DailogLevel dailogLevel;
    //public BigBannerActivator BigBannerObject;
    public void TriggerDailog()
    {
        PlayerPrefs.SetInt("CareerMode", 1);
        PlayerPrefs.SetInt("PetCareMode", 0);
        PlayerPrefs.SetInt("RandomMode", 0);
        PlayerPrefs.SetInt("RacingMode", 0);
        PlayerPrefs.SetInt("StoryMode", 0);
        PlayerPrefs.SetInt("RealCareerMode", 0);
        SoundManager.instance.PlayButtonClickSound();
        storyPanel.SetActive(true);
        //BigBannerObject.ShowBanner();
        FindObjectOfType<DailogManager>().StartDailog(dailog);
    }

    public void TriggerDailogLevel(int levelId)
    {
        PlayerPrefs.SetInt("CareerMode", 1);
        PlayerPrefs.SetInt("PetCareMode", 0);
        PlayerPrefs.SetInt("RandomMode", 0);
        PlayerPrefs.SetInt("RacingMode", 0);
        PlayerPrefs.SetInt("StoryMode", 0);
        PlayerPrefs.SetInt("RealCareerMode", 0);
        SoundManager.instance.PlayButtonClickSound();
        PlayerPrefs.SetInt("LevelPlayID", levelId);
        Debug.Log(PlayerPrefs.GetInt("LevelPlayID"));
        storyPanel.SetActive(true);
        //BigBannerObject.ShowBanner();
        FindObjectOfType<DailogManagerLevel>().StartDailog(dailogLevel);
    }
    public void LoadOn()
    {
        storyon.LoadOn = true;
    }
}
