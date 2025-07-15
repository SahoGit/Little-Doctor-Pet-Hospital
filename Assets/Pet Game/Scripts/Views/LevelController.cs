using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public int levelId;
    public bool isLocked;
    public GameObject levelNumberText;
    public GameObject lockImage;
    public GameObject lockBtn;
    public Sprite lockedImg;
    public Sprite UnLockedImg;
    public Sprite PlayedCard;
    // Start is called before the first frame update
    void Start()
    {

        //Debug.Log(PlayerPrefs.GetInt("LevelPlayed"));
        if (PlayerPrefs.GetInt("RealLevelPlayed") >= levelId || PlayerPrefs.GetInt("UnlockAll", 0) == 1)
        {
            lockImage.SetActive(false);
            levelNumberText.SetActive(true);
            lockBtn.GetComponent<Image>().sprite = UnLockedImg;
            lockBtn.GetComponent<Button>().interactable = true;
        } else
        {
            lockImage.SetActive(true);
            levelNumberText.SetActive(false);
            lockBtn.GetComponent<Image>().sprite = lockedImg;
            lockBtn.GetComponent<Button>().interactable = false;
            lockBtn.GetComponent<ActionManager>().enabled = false;
        }

        if ((PlayerPrefs.GetInt("RealLevelPlayed") - 1) >= levelId)
        {
            lockBtn.GetComponent<Image>().sprite = PlayedCard;
        }
    }

    public void catLevel(int levelId)
    {
        PlayerPrefs.SetInt("LevelPlayID", levelId);
        NavigationManager.instance.ReplaceScene(GameScene.CATVIEW);
    }

    public void dogLevel(int levelId)
    {
        PlayerPrefs.SetInt("LevelPlayID", levelId);
        NavigationManager.instance.ReplaceScene(GameScene.DOGVIEW);
    }

    public void bunnyLevel(int levelId)
    {
        PlayerPrefs.SetInt("LevelPlayID", levelId);
        NavigationManager.instance.ReplaceScene(GameScene.BUNNYVIEW);
    }

    public void tigerLevel(int levelId)
    {
        PlayerPrefs.SetInt("LevelPlayID", levelId);
        NavigationManager.instance.ReplaceScene(GameScene.TIGERVIEW);
    }

    public void pandaLevel(int levelId)
    {
        PlayerPrefs.SetInt("LevelPlayID", levelId);
        NavigationManager.instance.ReplaceScene(GameScene.PANDAVIEW);
    }

    public void loadLevel(int levelId)
    {
        SoundManager.instance.PlayButtonClickSound();
        PlayerPrefs.SetInt("RealLevelPlayID", levelId);
        if (PlayerPrefs.GetInt("RealLevelPlayID") >= 0 && PlayerPrefs.GetInt("RealLevelPlayID") <= 4)
        {
            NavigationManager.instance.ReplaceScene(GameScene.CATVIEW);
        }
        if (PlayerPrefs.GetInt("RealLevelPlayID") >= 5 && PlayerPrefs.GetInt("RealLevelPlayID") <= 9)
        {
            NavigationManager.instance.ReplaceScene(GameScene.DOGVIEW);
        }
        if (PlayerPrefs.GetInt("RealLevelPlayID") >= 10 && PlayerPrefs.GetInt("RealLevelPlayID") <= 14)
        {
            NavigationManager.instance.ReplaceScene(GameScene.BUNNYVIEW);
        }
        if (PlayerPrefs.GetInt("RealLevelPlayID") >= 15 && PlayerPrefs.GetInt("RealLevelPlayID") <= 16)
        {
            NavigationManager.instance.ReplaceScene(GameScene.TIGERVIEW);
        }
        if (PlayerPrefs.GetInt("RealLevelPlayID") >= 17 && PlayerPrefs.GetInt("RealLevelPlayID") <= 19)
        {
            NavigationManager.instance.ReplaceScene(GameScene.PANDAVIEW);
        }
    }
}
