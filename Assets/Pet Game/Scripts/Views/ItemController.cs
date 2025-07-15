using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using System.Globalization;

public class ItemController : MonoBehaviour
{
    public GameObject currentItem;
    public GameObject LockImage;
    public bool isLocked;
    public string name;
    public int id;
    public GameObject noInternetConect;

    public GameObject rewardedPanel;

    //void OnValidate()
    //{
    //    //LockImage = gameObject.transform.GetChild(2).gameObject;
    //
    //}
    void OnValidate()
    {
        LockImage = this.gameObject.transform.parent.GetChild(1).gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        unLockedOnStart();
        var prefName = name + id;

        if (PlayerPrefs.GetInt(prefName) == 0)
        {
            isLocked = true;
        }
        else
        {
            isLocked = false;
        }
        //Debug.Log(prefName);
        if (!isLocked)
        {
            
            this.gameObject.GetComponent<ApplicatorListener>().enabled = true;
            LockImage.SetActive(false);

        }
    }

    public void BuyItems(GameObject clickedItem)
    {
        currentItem = clickedItem;
        int itemId = clickedItem.GetComponent<ItemController>().id;
        Debug.Log(itemId);
        bool itemLocked = clickedItem.GetComponent<ItemController>().isLocked;
        string itemName = clickedItem.GetComponent<ItemController>().name;


        if (itemLocked)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                noInternetConect.SetActive(true);
                Invoke("noInternetConectClose", 2.0f);
            } else
            {
                //AdsManager.Instance.CallInterstitialAd(Adspref.GamePause);
                SoundManager.instance.PlaySparkleSound();
                LockImage.SetActive(false);
                this.gameObject.GetComponent<ApplicatorListener>().enabled = true;
                PlayerPrefs.SetInt(name + id, 1);
            }
        }
    }
    void noInternetConectClose()
    {
        noInternetConect.SetActive(false);
    }

    void unLockedOnStart()
    {
        PlayerPrefs.SetInt("CatItem1", 1);
        PlayerPrefs.SetInt("DogItem1", 1);
        PlayerPrefs.SetInt("BunnyItem1", 1);
        PlayerPrefs.SetInt("PandaItem1", 1);
    }

    public void RewarderPanelShow()
    {
        rewardedPanel.SetActive(true);
    }
}
