using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailogManagerLevel : MonoBehaviour
{
    public Text DocText;
    public Text PetText;
    public GameObject storyPanel;
    public GameObject LoadingPanel;
    //public GameObject careerModePanel;
    public Queue<string> sentencesLevel;
    public GameObject nextBtn;
    public GameObject SkipButton;
    public GameObject PetOwner;
    public GameObject JnDoctor;
    public GameObject PetOwnerChatBox;
    public GameObject JnChatBox;
    public Sprite DoctorImage;
    public Image PetImage;
    public Image PetOwnerImage;
    public Image DoctorImageDisplay;

    public Image LoadingFilled;

    private Sprite CoustumerImageDisplay;
    private Sprite PetImageDisplay;

    public int[] JuniorDoctorDailogeNumber;
    //public BigBannerActivator BigBannerObject;

    // Start is called before the first frame update
    void Start()
    {
        nextBtn.SetActive(false);
        sentencesLevel = new Queue<string>();
    }

    void skipButtonShow()
    {
        SkipButton.SetActive(true);
    }

 
    public void StartDailog(DailogLevel dailogLevel)
    {
        PlayerPrefs.SetInt("StoryPanelActive", 1);
        //Destroy(GameObject.Find("MEDIUM_RECTANGLE(Clone)"));
        //Destroy(GameObject.Find("ADAPTIVE(Clone)"));
       


        LoadingFilled.GetComponent<Image>().fillAmount = 0;
        LoadingPanel.SetActive(true);
        Invoke("callAds", 1.0f);
        StartCoroutine(FillAction(LoadingFilled));
        //Invoke("LoadingFull", 4.0f);

        StartCoroutine(dailogStart(dailogLevel));

        
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

    IEnumerator dailogStart(DailogLevel dailogLevel)
    {

        if (storyon.LoadOn == false)
        {
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(5f);
        }
        LoadingPanel.SetActive(false);
        sentencesLevel.Clear();

        foreach (string sentence in dailogLevel.sentencesLevel)
        {
            sentencesLevel.Enqueue(sentence);
        }
        CoustumerImageDisplay = dailogLevel.CoustumerImage;
        PetImageDisplay = dailogLevel.Pet;
        JuniorDoctorDailogeNumber = dailogLevel.DoctorDailogeNumber;
        DisplayNextSentence();
        //Invoke("skipButtonShow", 3.0f);
    }

    public void DisplayNextSentence()
    {
        Debug.Log(sentencesLevel.Count);
        SoundManager.instance.PlayButtonClickSound();
        nextBtn.SetActive(false);
        //anim.SetBool("isUp", false);
        bool isExist = false;

        foreach (int n in JuniorDoctorDailogeNumber)
        {
            if (n == sentencesLevel.Count)
            {
                isExist = true;
                break;
            }
        }
        if (isExist)
        {
            Doctor();
            return;
        }
        if (sentencesLevel.Count == 0)
        {
            EndDailog();
            return;
        }
        Coustumer();
    }

    void Coustumer()
    {
        string sentence = sentencesLevel.Dequeue();
        StopAllCoroutines();
        PetImage.GetComponent<Image>().enabled = true;
        PetOwner.SetActive(true);
        JnDoctor.SetActive(false);
        PetOwnerChatBox.SetActive(true);
        JnChatBox.SetActive(false);
        nextBtn.transform.position = new Vector2(5.20f, nextBtn.transform.position.y);
        StartCoroutine(PetTypeSentence(sentence));
        PetOwnerImage.GetComponent<Image>().sprite = CoustumerImageDisplay;
        PetOwnerImage.GetComponent<Image>().SetNativeSize();
        PetImage.GetComponent<Image>().sprite = PetImageDisplay;
        PetImage.GetComponent<Image>().SetNativeSize();
        //StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator PetTypeSentence(string sentence)
    {
        int TypeCounter = 0;
        PetText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            if (TypeCounter == 5)
            {
                SoundManager.instance.PlayTextSound();
                TypeCounter = 0;
            }
            TypeCounter++;
            PetText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
        nextBtn.SetActive(true);
    }

    IEnumerator JnTypeSentence(string sentence)
    {
        int TypeCounter = 0;
        DocText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            if (TypeCounter == 5)
            {
                SoundManager.instance.PlayTextSound();
                TypeCounter = 0;
            }
            TypeCounter++;
            DocText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
        nextBtn.SetActive(true);
    }

    public void EndDailog()
    {
        storyPanel.SetActive(false);
        //BigBannerObject.HideBanner();
        //Destroy(GameObject.Find("MEDIUM_RECTANGLE(Clone)"));
        //Destroy(GameObject.Find("ADAPTIVE(Clone)"));
        PlayerPrefs.SetInt("StoryPanelActive", 0);
        LoadingFilled.GetComponent<Image>().fillAmount = 0;
        //careerModePanel.SetActive(true);
        if (PlayerPrefs.GetInt("CareerMode") == 1)
        {
            //Debug.Log(PlayerPrefs.GetInt("LevelPlayID"));
            if (PlayerPrefs.GetInt("LevelPlayID") >= 0 && PlayerPrefs.GetInt("LevelPlayID") <= 4)
            {
                NavigationManager.instance.ReplaceScene(GameScene.CATVIEW);
            }
            if (PlayerPrefs.GetInt("LevelPlayID") >= 5 && PlayerPrefs.GetInt("LevelPlayID") <= 9)
            {
                NavigationManager.instance.ReplaceScene(GameScene.DOGVIEW);
            }
            if (PlayerPrefs.GetInt("LevelPlayID") >= 10 && PlayerPrefs.GetInt("LevelPlayID") <= 14)
            {
                NavigationManager.instance.ReplaceScene(GameScene.BUNNYVIEW);
            }
            if (PlayerPrefs.GetInt("LevelPlayID") >= 15 && PlayerPrefs.GetInt("LevelPlayID") <= 16)
            {
                NavigationManager.instance.ReplaceScene(GameScene.TIGERVIEW);
            }
            if (PlayerPrefs.GetInt("LevelPlayID") >= 17 && PlayerPrefs.GetInt("LevelPlayID") <= 19)
            {
                NavigationManager.instance.ReplaceScene(GameScene.PANDAVIEW);
            }
            

        }
        if (PlayerPrefs.GetInt("PetCareMode") == 1)
        {
            if (PlayerPrefs.GetInt("LevelPlayID") == 20)
            {
                PlayerPrefs.SetInt("PetCareMode", 1);
                NavigationManager.instance.ReplaceScene(GameScene.CATVIEW);
            }
            if (PlayerPrefs.GetInt("LevelPlayID") == 21)
            {
                PlayerPrefs.SetInt("PetCareMode", 1);
                NavigationManager.instance.ReplaceScene(GameScene.DOGVIEW);
            }
            if (PlayerPrefs.GetInt("LevelPlayID") == 22)
            {
                PlayerPrefs.SetInt("PetCareMode", 1);
                NavigationManager.instance.ReplaceScene(GameScene.BUNNYVIEW);
            }
            if (PlayerPrefs.GetInt("LevelPlayID") == 23)
            {
                PlayerPrefs.SetInt("PetCareMode", 1);
                NavigationManager.instance.ReplaceScene(GameScene.TIGERVIEW);
            }
            if (PlayerPrefs.GetInt("LevelPlayID") == 24)
            {
                PlayerPrefs.SetInt("PetCareMode", 1);
                NavigationManager.instance.ReplaceScene(GameScene.PANDAVIEW);
            }
            if (PlayerPrefs.GetInt("LevelPlayID") == 25)
            {
                PlayerPrefs.SetInt("PetCareMode", 1);
                NavigationManager.instance.ReplaceScene(GameScene.UNICORNVIEW);
            }
        }
    }


    void Doctor()
    {
        PetImage.GetComponent<Image>().enabled = false;
        string sentence = sentencesLevel.Dequeue();
        StopAllCoroutines();
        PetOwner.SetActive(false);
        JnDoctor.SetActive(true);
        PetOwnerChatBox.SetActive(false);
        JnChatBox.SetActive(true);
        nextBtn.transform.position = new Vector2(3.46f, nextBtn.transform.position.y);
        StartCoroutine(JnTypeSentence(sentence));
        //DoctorImageDisplay.GetComponent<Image>().sprite = DoctorImage;
        //DoctorImageDisplay.GetComponent<Image>().SetNativeSize();
        //StartCoroutine(TypeSentence(sentence));
    }

    void callAds()
    {
        AdsManager.Instance.ShowInterstitial("");
    }
}
