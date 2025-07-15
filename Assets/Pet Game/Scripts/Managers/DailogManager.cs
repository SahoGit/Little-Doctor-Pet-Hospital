using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailogManager : MonoBehaviour
{
    public Text JnDailogText;
    public Text SnDailogText;
    public GameObject storyPanel;
    public GameObject LoadingPanel;
    public GameObject careerModePanel;
    public Queue<string> sentences;
    public GameObject nextBtn;
    public GameObject JnDoctor;
    public GameObject SnDoctor;
    public GameObject JnChatBox;
    public GameObject SnChatBox;
    public GameObject SkipButton;

    public Image LoadingFilled;
    //public Animator anim;
    public Sprite[] DoctorImage;

    public int[] SeniorDoctorDailogeNumber;

    public GameObject[] levelArray;
    public int[] levelArrayIds;

    //public BigBannerActivator BigBannerObject;

    // Start is called before the first frame update
    void Start()
    {
        //if (PlayerPrefs.GetInt("ComingFromSplash") == 0)
        //{
        //    //if (PlayerPrefs.GetInt("StartPanelPlayed") == 1)
        //    //{
        //    //    levelArray[PlayerPrefs.GetInt("StoryLevelPlayID")].GetComponent<DailogTrigger>().TriggerDailogLevel(levelArrayIds[PlayerPrefs.GetInt("StoryLevelPlayID")]);
        //    //}
        //    //if (PlayerPrefs.GetInt("NoStoryShow") == 1)
        //    //{
        //    //    levelArray[PlayerPrefs.GetInt("StoryLevelPlayID")].GetComponent<DailogTrigger>().TriggerDailogLevel(levelArrayIds[PlayerPrefs.GetInt("StoryLevelPlayID")]);
        //    //}
        //}
        //PlayerPrefs.SetInt("StoryLevelPlayID", 24);
        if (PlayerPrefs.GetInt("StoryLevelPlayID") == 25)
        {
            //PetCareMode Start here
            PlayerPrefs.SetInt("StoryLevelPlayID", 0);
            PlayerPrefs.SetInt("NoStoryShow", 1);
        }
        if (PlayerPrefs.GetInt("StoryMode") == 1)
        {
            if (PlayerPrefs.GetInt("StoryLevelPlayID") > 19)
            {
                PlayerPrefs.SetInt("CareerMode", 0);
                PlayerPrefs.SetInt("PetCareMode", 1);
            }
            else
            {
                PlayerPrefs.SetInt("CareerMode", 1);
                PlayerPrefs.SetInt("PetCareMode", 0);
            }
            levelArray[PlayerPrefs.GetInt("StoryLevelPlayID")].GetComponent<DailogTrigger>().TriggerDailogLevel(levelArrayIds[PlayerPrefs.GetInt("StoryLevelPlayID")]);
        }
        nextBtn.SetActive(false);
        //anim.SetBool("isUp", false);
        sentences = new Queue<string>();
        
    }


    void skipButtonShow()
    {
        SkipButton.SetActive(true);
    }

    public void StartDailog(Dailog dailog)
    {
        PlayerPrefs.SetInt("StoryPanelActive", 1);
        
        //LoadingPanel.SetActive(true);
        //Invoke("callAds", 1.0f);
        StartCoroutine(FillAction(LoadingFilled));
        StartCoroutine(dailogStart(dailog));

        
    }

    IEnumerator FillAction(Image img)
    {
        if (img.fillAmount < 1)
        {
            img.fillAmount = img.fillAmount + 0.006f;
            yield return new WaitForSeconds(0.02f);
            StartCoroutine(FillAction(img));
        }
        else if (img.color.a >= 1f)
        {
            StopCoroutine(FillAction(img));
        }
    }

    IEnumerator dailogStart(Dailog dailog)
    {
        if (PlayerPrefs.GetInt("ComingFromSplash") == 0)
        {
            if (PlayerPrefs.GetInt("StartPanelPlayed") == 1)
            {
                levelArray[PlayerPrefs.GetInt("StoryLevelPlayID")].GetComponent<DailogTrigger>().TriggerDailogLevel(levelArrayIds[PlayerPrefs.GetInt("StoryLevelPlayID")]);
            }
        }
        else
        {
            yield return new WaitForSeconds(5.0f);
            LoadingPanel.SetActive(false);
            sentences.Clear();

            foreach (string sentence in dailog.sentences)
            {
                sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }
        
        //Invoke("skipButtonShow", 3.0f);
    }

    public void DisplayNextSentence()
    {
        SoundManager.instance.PlayButtonClickSound();
        nextBtn.SetActive(false);
        //anim.SetBool("isUp", false);
        bool isExist = false;

        foreach (int n in SeniorDoctorDailogeNumber)
        {
            if (n == sentences.Count)
            {
                isExist = true;
                break;
            }
        }
        if (isExist)
        {
            SeniorDoctor();
            return;
        }
        if (sentences.Count == 0)
        {
            EndDailog();
            return;
        }
        JuniorDoctor();
    }

    void JuniorDoctor()
    {
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        JnDoctor.SetActive(true);
        SnDoctor.SetActive(false);
        JnChatBox.SetActive(true);
        SnChatBox.SetActive(false);
        nextBtn.transform.position = new Vector2(3.0f, nextBtn.transform.position.y);
        StartCoroutine(JnTypeSentence(sentence));
    }

    IEnumerator JnTypeSentence(string sentence)
    {
        int TypeCounter = 0;
        JnDailogText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            if (TypeCounter == 5)
            {
                SoundManager.instance.PlayTextSound();
                TypeCounter = 0;
            }
            TypeCounter++;
            JnDailogText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
        nextBtn.SetActive(true);
    }

    IEnumerator SnTypeSentence(string sentence)
    {
        int TypeCounter = 0;
        SnDailogText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            if (TypeCounter == 5)
            {
                SoundManager.instance.PlayTextSound();
                TypeCounter = 0;
            }
            TypeCounter++;
            SnDailogText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
        nextBtn.SetActive(true);
    }

     public void EndDailog()
    {
        storyPanel.SetActive(false);
        
        //Destroy(GameObject.Find("MEDIUM_RECTANGLE(Clone)"));
        //Destroy(GameObject.Find("ADAPTIVE(Clone)"));
        PlayerPrefs.SetInt("StartPanelPlayed", 1);
        PlayerPrefs.SetInt("StoryPanelActive", 0);
        LoadingFilled.GetComponent<Image>().fillAmount = 0;
        //careerModePanel.SetActive(true);
        if (PlayerPrefs.GetInt("StoryLevelPlayID") > 19)
        {
            PlayerPrefs.SetInt("CareerMode", 0);
            PlayerPrefs.SetInt("PetCareMode", 1);
        } else
        {
            PlayerPrefs.SetInt("CareerMode", 1);
            PlayerPrefs.SetInt("PetCareMode", 0);
        }
        levelArray[PlayerPrefs.GetInt("StoryLevelPlayID")].GetComponent<DailogTrigger>().TriggerDailogLevel(levelArrayIds[PlayerPrefs.GetInt("StoryLevelPlayID")]);
    }


    void SeniorDoctor()
    {
        string sentence = sentences.Dequeue();
        //nextBtn.transform.position = new Vector2(3.0f, nextBtn.transform.position.y);
        //dailogText.text = sentence.ToString();
        StopAllCoroutines();
        JnDoctor.SetActive(false);
        SnDoctor.SetActive(true);
        JnChatBox.SetActive(false);
        SnChatBox.SetActive(true);
        nextBtn.transform.position = new Vector2(3.0f, nextBtn.transform.position.y);
        StartCoroutine(SnTypeSentence(sentence));
    }

    void callAds()
    {
        AdsManager.Instance.ShowInterstitial("");
    }
}
