using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetController : MonoBehaviour
{
    public int id;
    public Button lockBtn;
    public GameObject lockImage;
    public GameObject animalImage;

    public GameObject loadingPanel;
    public Image LoadingFilled;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("AnimalPlayed") >= id || (PlayerPrefs.GetInt("UnlockAll", 0) == 1) || (PlayerPrefs.GetInt(transform.gameObject.name)==1))
        {
            lockImage.SetActive(false);
            lockBtn.GetComponent<Button>().interactable = true;
            animalImage.GetComponent<Image>().color = new Color(255, 255, 255, 255);
        }
        else
        {
            lockBtn.GetComponent<ActionManager>().enabled = false;
        }
    }

    public void onPlayGame(int petId)
    {
        SoundManager.instance.PlayButtonClickSound();
        PlayerPrefs.SetInt("PetSelected", petId);
        loadingPanel.SetActive(true);
        LoadingFilled.GetComponent<Image>().fillAmount = 0;
        StartCoroutine(FillAction(LoadingFilled));
        Invoke("callAds", 2.0f);
        Invoke("playLevel", 4.0f);
    }

    void playLevel()
    {
        NavigationManager.instance.ReplaceScene(GameScene.RECEPTIONView);
    }
   public void unlockAnimal()
    {
        PlayerPrefs.SetInt(transform.gameObject.name, 1);
        lockImage.SetActive(false);
        lockBtn.GetComponent<Button>().interactable = true;
        animalImage.GetComponent<Image>().color = new Color(255, 255, 255, 255);
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

    void callAds()
    {
        AdsManager.Instance.ShowInterstitial("");
    }
}
