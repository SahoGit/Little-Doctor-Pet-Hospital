using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class MainView : MonoBehaviour {

	#region Variables, Constants & Initializers
	private bool quitFlag = true;
    public GameObject careerBtn, petCareBtn, randomBtn, racingBtn;
    public GameObject title, playButton, dailogPlayButton, moreGamesButton, rateUsButton, privacyButton, shopButton;
    public RectTransform titleEndPoint , playButtonEndPoint, moreGamesButtonEndPoint, rateUsButtonEndPoint, privacyButtonEndPoint, shopButtonEndPoint, removeAdsButtonEndPoint, restorePurchaseButtonEndPoint;
	public GameObject doctor;
	public RectTransform doctorEndPoint;
	public GameObject quitPopup;
	public GameObject catFace, catTate, pandaFace, dogFace;
	public GameObject titleParticles;
	public GameObject burgerGameButton, pizzaMakingButton;

    public GameObject modeSelection, careerSelection, petCareSelection, petCareRacingSelection, restorePurchasePanel,  storyPanel, storyPanelLevel, loadingPanel, loading2, watchAdPanel; //shopPanel;

	public Sprite[] gamesIcon;
	static int index = 0;

    //public GameObject[] packages;
    public Button BuyBtn;

    public Image LoadingFilled, LoadingFilled2;
    public Text WatchAdCounterText;
    public GameObject[] LockImage;

    public Image[] LockModeImages;
    public GameObject[] outLine;
    public GameObject[] petCareOutLine;
    public GameObject[] raceingOutLine;

    public Text scoreText;
    public GameObject addCashPanel;

    public ScrollRect petcareModeScroll;
    public ScrollRect careerModeScroll;

    public Animator petCareContentAnim;
    public Animator racingContentAnim;

    #endregion

    #region Lifecycle Methods

    // Use this for initialization
    void Start ()
    {
        Debug.Log(PlayerPrefs.GetInt("AnimalPlayed") + "hello");
        //PlayerPrefs.SetInt("RealLevelPlayed", 16);
        outlineManage();
        petCareOutlineManage();
        RacingModeOutlineManage();

        scrollSetting();
        carrerScrollSetting();
        PlayerPrefs.SetInt("PetCareModeUnlock", 1);
        PlayerPrefs.SetInt("RandomModeUnlock", 1);
        PlayerPrefs.SetInt("RacingModeUnlock", 1);
        //PlayerPrefs.SetInt("AnimalPlayed", 5);
        scoreText.text = PlayerPrefs.GetInt("PlayerCash").ToString();
        //PlayerPrefs.SetInt("CareerMode", 0);
        //Debug.Log(PlayerPrefs.GetInt("LevelPlayed"));
        if (PlayerPrefs.GetInt("PetCareModeUnlock") == 1 || PlayerPrefs.GetInt("LevelPlayed") >= 19)
        {
            LockImage[0].SetActive(false);
            LockModeImages[0].GetComponent<Image>().color += new Color(0f, 0f, 0f, 1f);
        }
        if (PlayerPrefs.GetInt("RandomModeUnlock") == 1 || PlayerPrefs.GetInt("AnimalPlayed") >= 4)
        {
            LockImage[1].SetActive(false);
            LockModeImages[1].GetComponent<Image>().color += new Color(0f, 0f, 0f, 1f);
        }
        if (PlayerPrefs.GetInt("RacingModeUnlock") == 1 || PlayerPrefs.GetInt("RaceModeModeAvailable") == 1)
        {
            LockImage[2].SetActive(false);
            LockModeImages[2].GetComponent<Image>().color += new Color(0f, 0f, 0f, 1f);
        }
        if (PlayerPrefs.GetInt("ComingFromSplash") == 0)
		{
            PlayerPrefs.SetInt("StartPanelPlayed", 1);
            if (PlayerPrefs.GetInt("RandomMode") == 1)
            {
                modeSelection.SetActive(true);
            }
            if (PlayerPrefs.GetInt("RealCareerMode") == 1)
            {
                modeSelection.SetActive(true);
                careerSelection.SetActive(true);
            }
            if (PlayerPrefs.GetInt("PetCareMode") == 1)
            {
                modeSelection.SetActive(true);
                petCareSelection.SetActive(true);
            }
        } else
        {
            PlayerPrefs.SetInt("StoryMode", 0);
        }

        //if (PlayerPrefs.GetInt("LevelPlayed") > 25)
        //{
        //    petCareBtn.GetComponent<Button>().interactable = true;
        //}
        //if (PlayerPrefs.GetInt("AnimalPlayed") > 5)
        //{
        //    randomBtn.GetComponent<Button>().interactable = true;
        //}
        //if (PlayerPrefs.GetInt("RaceModeModeAvailable") == 1)
        //{
        //    racingBtn.GetComponent<Button>().interactable = true;
        //}

        //PlayerPrefs.SetInt("CareerMode", 0);
        //PlayerPrefs.SetInt("PetCareMode", 0);
        PlayerPrefs.SetInt("RandomMode", 0);
        PlayerPrefs.SetInt("RacingMode", 0);
        Invoke("SetViewContents", 0.1f);
        checkBuyBtn();
       // AdsManager.Instance.ShowBanner();
        AdsManager.Instance.ShowBanner();
    }

    public void scrollSetting()
    {
        if (PlayerPrefs.GetInt("AnimalPlayed") == 1)
        {
            petcareModeScroll.normalizedPosition = new Vector2(0.4f, 0.0f);
        }
        if (PlayerPrefs.GetInt("AnimalPlayed") >= 2)
        {
            petcareModeScroll.normalizedPosition = new Vector2(0.8f, 0.0f);
        }
        if (PlayerPrefs.GetInt("AnimalPlayed") >= 3)
        {
            petcareModeScroll.normalizedPosition = new Vector2(1f, 0.0f);
        }
    }

    public void carrerScrollSetting()
    {
        if (PlayerPrefs.GetInt("RealLevelPlayed") >= 5 && PlayerPrefs.GetInt("RealLevelPlayed") <= 9)
        {
            careerModeScroll.normalizedPosition = new Vector2(0.0f, 0.4f);
        }
        if (PlayerPrefs.GetInt("RealLevelPlayed") >= 10 && PlayerPrefs.GetInt("RealLevelPlayed") <= 19)
        {
            careerModeScroll.normalizedPosition = new Vector2(0.0f, 0.0f);
        }
    }

    public void outlineManage()
    {
        for (int i = 0; i < outLine.Length; i++)
        {
            outLine[i].SetActive(false);
        }
        if (PlayerPrefs.GetInt("CareerMode") == 1)
        {
            outLine[0].SetActive(true);
        }
        else if (PlayerPrefs.GetInt("PetCareMode") == 1)
        {
            outLine[1].SetActive(true);
        }
        else if (PlayerPrefs.GetInt("RandomMode") == 1)
        {
            outLine[2].SetActive(true);
        }
        else if (PlayerPrefs.GetInt("RacingMode") == 1)
        {
            outLine[3].SetActive(true);
        }
        else if (PlayerPrefs.GetInt("RealCareerMode") == 1)
        {
            outLine[4].SetActive(true);
        }
    }

    public void petCareOutlineManage()
    {
        for (int i = 0; i < petCareOutLine.Length; i++)
        {
            petCareOutLine[i].SetActive(false);
        }
        if (PlayerPrefs.GetInt("AnimalPlayed") == 5)
        {
            petCareOutLine[PlayerPrefs.GetInt("AnimalPlayed") - 1].SetActive(true);
        }
        else
        {
            petCareOutLine[PlayerPrefs.GetInt("AnimalPlayed")].SetActive(true);
        }

    }

    public void RacingModeOutlineManage()
    {
        for (int i = 0; i < raceingOutLine.Length; i++)
        {
            raceingOutLine[i].SetActive(false);
        }
        raceingOutLine[PlayerPrefs.GetInt("PetSelectedForRace")].SetActive(true);

    }

    // Update is called once per frame
    void Update () {
#if UNITY_ANDROID || UNITY_WP8
		if (Input.GetKeyDown(KeyCode.Escape)) 
		{ 
	
			if(PlayerPrefs.GetInt("StoryPanelActive") == 1){
			    storyPanel.SetActive(false);
			    storyPanelLevel.SetActive(false);
			    loadingPanel.SetActive(false);
        Invoke("callAds", 2.0f);
                PlayerPrefs.SetInt("StoryPanelActive", 0);
                PlayerPrefs.SetInt("StoryMode", 0);
                LoadingFilled.GetComponent<Image>().fillAmount = 0;
			} else {
                if (quitPopup != null) {
			    	titleParticles.SetActive(false);
			    	if(quitPopup.GetComponent<RectTransform>().localScale != Vector3.one) {
			    		Hashtable tweenParams = new Hashtable();
			    		tweenParams.Add ("scale", Vector3.one);
			    		tweenParams.Add ("time", 0.5f);
			    		//tweenParams.Add ("oncompletetarget", gameObject);
			    		//tweenParams.Add ("oncomplete", "HideCartFullIndication");
			    		iTween.ScaleTo(quitPopup.gameObject, tweenParams);
			    	}
			    }
			    else {
			    	OnQuitYesButtonClicked();
			    }
            }
			
		}

#endif
    }

    void Destroy() {
		iTween.Stop ();
	}

	#endregion

	#region Callback Methods

	private void SetViewContents() {
		buttonActive ();
		animalFaceMovement ();
		doctorComesInn ();
	}

    private void buttonActive()
    {
        int index = PlayerPrefs.GetInt("ButtonActive");
        Debug.Log("value is"+ index);
        index = index % 2;
        if (index == 0)
        {
            pizzaMakingButton.SetActive(false);
            burgerGameButton.SetActive(false);
            ScaleAction(pizzaMakingButton, 1.1f, 0.5f, iTween.EaseType.easeInBounce, iTween.LoopType.pingPong);
        }
        else
        {
            pizzaMakingButton.SetActive(false);
            burgerGameButton.SetActive(false);
            ScaleAction(burgerGameButton, 1.1f, 0.5f, iTween.EaseType.easeInBounce, iTween.LoopType.pingPong);
        }

        PlayerPrefs.SetInt("ButtonActive", PlayerPrefs.GetInt("ButtonActive") + 1);

    }
    private void ScaleAction(GameObject obj,float scaleval,float time,iTween.EaseType type,iTween.LoopType loopType) {
		Hashtable tweenParams = new Hashtable();
		tweenParams.Add ("scale", new Vector3 (scaleval,scaleval, 0));
		tweenParams.Add ("time", time);
		tweenParams.Add ("easetype", type);
		tweenParams.Add ("looptype", loopType);
		iTween.ScaleTo(obj, tweenParams);
	}


	private void MoveAction(GameObject obj,RectTransform pos,float time,iTween.EaseType actionType,iTween.LoopType loopType){
		Hashtable tweenParams = new Hashtable();
		tweenParams.Add ("x", pos.position.x);
		tweenParams.Add ("y", pos.position.y);
		tweenParams.Add ("time", time);
		tweenParams.Add ("easetype", actionType);
		tweenParams.Add ("looptype", loopType);
		iTween.MoveTo (obj, tweenParams);
	}

	private void RotateAction(GameObject obj,float roatationamount,float t,iTween.EaseType actionType,iTween.LoopType loopType){
		Hashtable tweenParams = new Hashtable ();
		tweenParams.Add ("z", roatationamount);
		tweenParams.Add ("time", t);
		tweenParams.Add ("easetype", actionType);
		tweenParams.Add ("looptype", loopType);
		iTween.RotateTo (obj, tweenParams);
	}

	private void animalFaceMovement(){
		RotateAction (catFace, -3.0f, 2.0f, iTween.EaseType.linear, iTween.LoopType.pingPong);
		RotateAction (catTate, 4.0f, 2.0f, iTween.EaseType.linear, iTween.LoopType.pingPong);
		RotateAction (pandaFace, -5.0f, 3.0f, iTween.EaseType.linear, iTween.LoopType.pingPong);
		RotateAction (dogFace, 5.0f, 2.5f, iTween.EaseType.linear, iTween.LoopType.pingPong);
	}

	private void doctorComesInn(){
		MoveAction (doctor, doctorEndPoint, 0.5f, iTween.EaseType.linear, iTween.LoopType.none);
		Invoke ("titleComesInn", 0.5f);

	}

	private void titleComesInn()
    {
        SoundManager.instance.PlayTitleDropSound();
        MoveAction (title, titleEndPoint, 0.5f, iTween.EaseType.linear, iTween.LoopType.none);
		Invoke ("ButtonsComesInn", 0.3f);
	}

	private void ButtonsComesInn(){
        SoundManager.instance.PlaySideBtnSound();
        MoveAction(moreGamesButton, moreGamesButtonEndPoint, 0.5f, iTween.EaseType.easeInBounce, iTween.LoopType.none);
        Invoke ("rateUsComesInn", 0.3f);
	}

    private void rateUsComesInn()
    {
        SoundManager.instance.PlaySideBtnSound();
        MoveAction(rateUsButton, rateUsButtonEndPoint, 0.5f, iTween.EaseType.easeInBounce, iTween.LoopType.none);
        Invoke("privacyPolicyComesInn", 0.3f);
    }

    private void privacyPolicyComesInn()
    {
        SoundManager.instance.PlaySideBtnSound();
        MoveAction(privacyButton, privacyButtonEndPoint, 0.5f, iTween.EaseType.easeInBounce, iTween.LoopType.none);
        //Invoke("shopComesInn", 0.3f);
        Invoke("restorePurchaseComesInn", 0.3f);
    }

    private void shopComesInn()
    {
        SoundManager.instance.PlaySideBtnSound();
        MoveAction(shopButton, shopButtonEndPoint, 0.5f, iTween.EaseType.easeInBounce, iTween.LoopType.none);
        Invoke("removeAdsComesInn", 0.3f);
    }

    private void removeAdsComesInn()
    {
        //SoundManager.instance.PlaySideBtnSound();
        //MoveAction(removeAdsButton, removeAdsButtonEndPoint, 0.5f, iTween.EaseType.easeInBounce, iTween.LoopType.none);
        //Invoke("restorePurchaseComesInn", 0.3f);
    }

    private void restorePurchaseComesInn()
    {
        //SoundManager.instance.PlaySideBtnSound();
        //MoveAction(restorePurchaseButton, restorePurchaseButtonEndPoint, 0.5f, iTween.EaseType.easeInBounce, iTween.LoopType.none);
        //Invoke("playButtonComesInn", 0.3f);
    }


    private void playButtonComesInn()
    {
        SoundManager.instance.PlayPlayBtnSound();
        //dailogPlayButton.SetActive (true);
        //ScaleAction (dailogPlayButton, 1.0f, 0.5f, iTween.EaseType.linear, iTween.LoopType.none);
        playButton.SetActive (true);
        ScaleAction (playButton, 1.0f, 0.5f, iTween.EaseType.linear, iTween.LoopType.none);
        Invoke("buttonScaleUp", 0.6f);
		//MoveAction (playButton, playButtonEndPoint, 0.5f, iTween.EaseType.easeInBounce, iTween.LoopType.none);
	}

	private void buttonScaleUp()
    {
        //dailogPlayButton.GetComponent<ActionManager>().enabled = true;
        playButton.GetComponent<ActionManager>().enabled = true;

    }

	public void OnPlayButtonClicked() {
		GameManager.instance.LogDebug ("Play Clicked");
		SoundManager.instance.PlayButtonClickSound ();
        loading2.SetActive(true);
        StartCoroutine(FillAction(LoadingFilled2));
        Invoke("modeSelectionShow", 4.0f);
        //Invoke("callAds", 2.0f);
        //NavigationManager.instance.ReplaceScene (GameScene.RECEPTIONView);
    }

    void modeSelectionShow()
    {
        LoadingFilled.GetComponent<Image>().fillAmount = 0;
        loading2.SetActive(false);
        modeSelection.SetActive(true);
    }

    public void OnPanelClosed(GameObject parent)
    {

        SoundManager.instance.PlayButtonClickSound();
        parent.SetActive(false);
    }

    public void OnModeShowPanelClosed(GameObject parent)
    {
        modeSelection.SetActive(true);
        SoundManager.instance.PlayButtonClickSound();
        parent.SetActive(false);
    }

    public void OnCareerModeButtonClicked()
    {
        SoundManager.instance.PlayButtonClickSound();
        PlayerPrefs.SetInt("CareerMode", 0);
        PlayerPrefs.SetInt("PetCareMode", 0);
        PlayerPrefs.SetInt("RandomMode", 0);
        PlayerPrefs.SetInt("RacingMode", 0);
        PlayerPrefs.SetInt("StoryMode", 0);
        PlayerPrefs.SetInt("RealCareerMode", 1);

        loadingPanel.SetActive(true);
        StartCoroutine(FillAction(LoadingFilled));
        Invoke("callAds", 2.0f);
        Invoke("carrerModePlay", 4.0f);
    }

    void carrerModePlay()
    {
        LoadingFilled.GetComponent<Image>().fillAmount = 0;
        loadingPanel.SetActive(false);
        careerSelection.SetActive(true);
        modeSelection.SetActive(false);
    }

    public void OnPetCareModeButtonClicked()
    {
        petCareContentAnim.enabled = true;
        SoundManager.instance.PlayButtonClickSound();
        PlayerPrefs.SetInt("CareerMode", 0);
        PlayerPrefs.SetInt("PetCareMode", 1);
        PlayerPrefs.SetInt("RandomMode", 0);
        PlayerPrefs.SetInt("RacingMode", 0);
        PlayerPrefs.SetInt("StoryMode", 0);
        PlayerPrefs.SetInt("RealCareerMode", 0);
        outlineManage();
        if (PlayerPrefs.GetInt("PetCareModeUnlock") == 1)
        {
            loadingPanel.SetActive(true);
            StartCoroutine(FillAction(LoadingFilled));
            Invoke("callAds", 2.0f);
            Invoke("petCareModePlay", 4.0f);
        }
        else
        {
            WatchAdCounterText.text = "Watch Ad " + PlayerPrefs.GetInt("PetCareUnlockCounter") + "/3";
            watchAdPanel.SetActive(true);
        }
        //NavigationManager.instance.ReplaceScene(GameScene.RECEPTIONView);
    }

    void petCareModePlay()
    {
        LoadingFilled.GetComponent<Image>().fillAmount = 0;
        loadingPanel.SetActive(false);
        petCareSelection.SetActive(true);
        modeSelection.SetActive(false);
        Invoke("petCareModeAnim", 1.0f);
    }

    void petCareModeAnim()
    {
        petCareContentAnim.enabled = false;
    }

    public void OnRandomButtonClicked()
    {
        
        SoundManager.instance.PlayButtonClickSound();
        PlayerPrefs.SetInt("CareerMode", 0);
        PlayerPrefs.SetInt("PetCareMode", 0);
        PlayerPrefs.SetInt("RandomMode", 1);
        PlayerPrefs.SetInt("RacingMode", 0);
        PlayerPrefs.SetInt("StoryMode", 0);
        PlayerPrefs.SetInt("RealCareerMode", 0);
        outlineManage();
        if (PlayerPrefs.GetInt("RandomModeUnlock") == 1)
        {
            loadingPanel.SetActive(true);
            StartCoroutine(FillAction(LoadingFilled));
            Invoke("callAds", 2.0f);
            Invoke("randomModePlay", 4.0f);
        }
        else
        {
            WatchAdCounterText.text = "Watch Ad " + PlayerPrefs.GetInt("RandomUnlockCounter") + "/4";
            watchAdPanel.SetActive(true);
        }
    }

    void randomModePlay()
    {
        LoadingFilled.GetComponent<Image>().fillAmount = 0;
        NavigationManager.instance.ReplaceScene(GameScene.RECEPTIONView);
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

    public void OnRacingButtonClicked()
    {
        racingContentAnim.enabled = true;
        SoundManager.instance.PlayButtonClickSound();
        PlayerPrefs.SetInt("CareerMode", 0);
        PlayerPrefs.SetInt("PetCareMode", 0);
        PlayerPrefs.SetInt("RandomMode", 0);
        PlayerPrefs.SetInt("RacingMode", 1);
        PlayerPrefs.SetInt("StoryMode", 0);
        PlayerPrefs.SetInt("RealCareerMode", 0);

        outlineManage();
        if (PlayerPrefs.GetInt("RacingModeUnlock") == 1)
        {

            loadingPanel.SetActive(true);
            StartCoroutine(FillAction(LoadingFilled));
            Invoke("callAds", 2.0f);
            Invoke("racingModePlay", 4.0f);
        }
        else
        {
            WatchAdCounterText.text = "Watch Ad " + PlayerPrefs.GetInt("RacingUnlockCounter") + "/5";
            watchAdPanel.SetActive(true);
        }
        //SceneManager.LoadScene(2);
        //NavigationManager.instance.ReplaceScene(GameScene.RECEPTIONView);
    }


    void racingModePlay()
    {
        LoadingFilled.GetComponent<Image>().fillAmount = 0;
        loadingPanel.SetActive(false);
        petCareRacingSelection.SetActive(true);
        modeSelection.SetActive(false);
        Invoke("racingModeAnim", 1.0f);
    }

    void racingModeAnim()
    {
        racingContentAnim.enabled = false;
    }

    public void OnRateUsButtonClicked() {
		GameManager.instance.LogDebug ("RateUs Clicked");
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.bestone.apps.and.games.mylittle.pet.doctor.furry.hospital.game2022");
        SoundManager.instance.PlayButtonClickSound ();
	}

	public void OnMoreFunButtonClicked() {
		GameManager.instance.LogDebug ("MoreFun Clicked");
        Application.OpenURL("https://play.google.com/store/apps/dev?id=4826365601331502275");
        SoundManager.instance.PlayButtonClickSound();
    }

    public void OnPrivacyPolicyButtonClicked()
    {
        GameManager.instance.LogDebug("Privacy policy");
        Application.OpenURL("https://petdragoninc.com/privacy-policy/");
        SoundManager.instance.PlayButtonClickSound();
    }

    public void OnShopButtonClicked()
    {
        SoundManager.instance.PlayButtonClickSound();
      //  shopPanel.SetActive(true);
    }

    public void OnRemoveAdsButtonClicked()
    {
        SoundManager.instance.PlayButtonClickSound();
    //    shopPanel.SetActive(true);
    }

    public void OnReStorePurchaseButtonClicked()
    {
        SoundManager.instance.PlayButtonClickSound();
        restorePurchasePanel.SetActive(true);
    }

    public void OnBurgerGameClicked() {
		SoundManager.instance.PlayButtonClickSound ();
		Application.OpenURL ("https://play.google.com/store/apps/details?id=com.bestonegames.yummyburger.homemade.cooking");
	}

    public void OnPizzaMakingClicked()
    {
        SoundManager.instance.PlayButtonClickSound();
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.bestonegames.mypizzashop.goodpizza.business.makergame");
    }

    public void OnQuitNoButtonClicked()
    {
        SoundManager.instance.PlayButtonClickSound();
        GameManager.instance.LogDebug ("QuitNo Clicked");
		quitPopup.GetComponent<RectTransform> ().localScale = Vector3.zero;
		titleParticles.SetActive(true);
    }

    public void OnQuitYesButtonClicked()
    {
        SoundManager.instance.PlayButtonClickSound();
        GameManager.instance.LogDebug("QuitYes Clicked");
        Application.Quit();
    }

    public void restorePurchase()
    {
        SoundManager.instance.PlayButtonClickSound();
        Destroy(GameObject.Find("GameManager"));
        Destroy(GameObject.Find("NavigationManager"));
        Destroy(GameObject.Find("AdsManagerSplash"));
        Destroy(GameObject.Find("AdCallingCanvas"));
        Destroy(GameObject.Find("SoundManager"));
        Destroy(GameObject.Find("ParticleManager"));
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }

    public void nextPackage()
    {
        SoundManager.instance.PlayButtonClickSound();
       // if (PlayerPrefs.GetInt("SelectedPackage") == packages.Length - 1)
        {
            PlayerPrefs.SetInt("SelectedPackage", 0);
        }
        //else
        {
            PlayerPrefs.SetInt("SelectedPackage", PlayerPrefs.GetInt("SelectedPackage") + 1);
        }
       // showSelectedPackage(PlayerPrefs.GetInt("SelectedPackage"));
        checkBuyBtn();
    }

    public void previousPackage()
    {
        SoundManager.instance.PlayButtonClickSound();
        if (PlayerPrefs.GetInt("SelectedPackage") == 0)
        {
         //   PlayerPrefs.SetInt("SelectedPackage", packages.Length - 1);
        }
        else
        {
            PlayerPrefs.SetInt("SelectedPackage", PlayerPrefs.GetInt("SelectedPackage") - 1);
        }
       // showSelectedPackage(PlayerPrefs.GetInt("SelectedPackage"));
        checkBuyBtn();
    }

    //void showSelectedPackage(int packageID)
    //{
    //    //for (int i = 0; i < packages.Length; i++)
    //    //{
    //    //    if (i == packageID)
    //    //    {
    //    //        packages[i].SetActive(true);
    //    //    }
    //    //    else
    //    //    {
    //    //        packages[i].SetActive(false);
    //    //    }
    //    //}

    //}

    public void buyPackage()
    {
        if (PlayerPrefs.GetInt("SelectedPackage") == 0)
        {
            package0();
        }
        else if (PlayerPrefs.GetInt("SelectedPackage") == 1)
        {
            package1();
        }
        else if (PlayerPrefs.GetInt("SelectedPackage") == 2)
        {
            package2();
        }
    }

    private void checkBuyBtn()
    {
        if (PlayerPrefs.GetInt("SelectedPackage") == 0 && PlayerPrefs.GetInt("PurchasedPackage1") == 1)
        {
            BuyBtn.GetComponent<Button>().interactable = false;
        } else if (PlayerPrefs.GetInt("SelectedPackage") == 1 && PlayerPrefs.GetInt("PurchasedPackage2") == 1)
        {
            BuyBtn.GetComponent<Button>().interactable = false;
        }
        else if (PlayerPrefs.GetInt("SelectedPackage") == 2 && PlayerPrefs.GetInt("PurchasedPackage3") == 1)
        {
            BuyBtn.GetComponent<Button>().interactable = false;
        } else
        {
            BuyBtn.GetComponent<Button>().interactable = true;
        }


    }

    private void package0()
    {
        //BUY CODE START HERE

        //BUY CODE END HERE
        PlayerPrefs.SetInt("AnimalPlayed", 5);
        PlayerPrefs.SetInt("PurchasedPackage1", 1);
        Destroy(GameObject.Find("GameManager"));
        Destroy(GameObject.Find("NavigationManager"));
        Destroy(GameObject.Find("AdsManagerSplash"));
        Destroy(GameObject.Find("AdCallingCanvas"));
        Destroy(GameObject.Find("SoundManager"));
        Destroy(GameObject.Find("ParticleManager"));
        SceneManager.LoadScene(0);
    }

    private void package1()
    {
        //BUY CODE START HERE

        //BUY CODE END HERE
        PlayerPrefs.SetInt("RemoveAds", 1);
        PlayerPrefs.SetInt("PurchasedPackage2", 1);
        PlayerPrefs.SetInt("RaceModeModeAvailable", 1);
        PlayerPrefs.SetInt("AnimalPlayed", 5);
        PlayerPrefs.SetInt("LevelPlayed", 19);
        Destroy(GameObject.Find("GameManager"));
        Destroy(GameObject.Find("NavigationManager"));
        Destroy(GameObject.Find("AdsManagerSplash"));
        Destroy(GameObject.Find("AdCallingCanvas"));
        Destroy(GameObject.Find("SoundManager"));
        Destroy(GameObject.Find("ParticleManager"));
        SceneManager.LoadScene(0);

    }

    private void package2()
    {
        //BUY CODE START HERE

        //BUY CODE END HERE
        PlayerPrefs.SetInt("RemoveAds", 1);
        PlayerPrefs.SetInt("PurchasedPackage3", 1);
        PlayerPrefs.SetInt("AnimalPlayed", 5);
        PlayerPrefs.SetInt("LevelPlayed", 19);
        PlayerPrefs.SetInt("RaceModeModeAvailable", 1);
        Destroy(GameObject.Find("GameManager"));
        Destroy(GameObject.Find("NavigationManager"));
        Destroy(GameObject.Find("AdsManagerSplash"));
        Destroy(GameObject.Find("AdCallingCanvas"));
        Destroy(GameObject.Find("SoundManager"));
        Destroy(GameObject.Find("ParticleManager"));
        SceneManager.LoadScene(0);

    }

    public void unlockWithWatchAd()
    {
        SoundManager.instance.PlayButtonClickSound();
        if (PlayerPrefs.GetInt("PetCareMode") == 1)
        {
            PlayerPrefs.SetInt("PetCareUnlockCounter", PlayerPrefs.GetInt("PetCareUnlockCounter") + 1);
            WatchAdCounterText.text = "Watch Ad " + PlayerPrefs.GetInt("PetCareUnlockCounter") + "/3";
            if (PlayerPrefs.GetInt("PetCareUnlockCounter") == 3)
            {
                LockModeImages[0].GetComponent<Image>().color += new Color(0f, 0f, 0f, 1f);
                PlayerPrefs.SetInt("PetCareModeUnlock", 1);
                LockImage[0].SetActive(false);
                watchAdPanel.SetActive(false);
            }
        }
        if (PlayerPrefs.GetInt("RandomMode") == 1)
        {
            PlayerPrefs.SetInt("RandomUnlockCounter", PlayerPrefs.GetInt("RandomUnlockCounter") + 1);
            WatchAdCounterText.text = "Watch Ad " + PlayerPrefs.GetInt("RandomUnlockCounter") + "/4";
            if (PlayerPrefs.GetInt("RandomUnlockCounter") == 4)
            {
                LockModeImages[1].GetComponent<Image>().color += new Color(0f, 0f, 0f, 1f);
                PlayerPrefs.SetInt("RandomModeUnlock", 1);
                LockImage[1].SetActive(false);
                watchAdPanel.SetActive(false);
            }
        }
        if (PlayerPrefs.GetInt("RacingMode") == 1)
        {
            PlayerPrefs.SetInt("RacingUnlockCounter", PlayerPrefs.GetInt("RacingUnlockCounter") + 1);
            WatchAdCounterText.text = "Watch Ad " + PlayerPrefs.GetInt("RacingUnlockCounter") + "/5";
            if (PlayerPrefs.GetInt("RacingUnlockCounter") == 5)
            {
                LockModeImages[2].GetComponent<Image>().color += new Color(0f, 0f, 0f, 1f);
                PlayerPrefs.SetInt("RacingModeUnlock", 1);
                LockImage[2].SetActive(false);
                watchAdPanel.SetActive(false);
            }
        }
    }

    public void CashAdd()
    {
        SoundManager.instance.PlayButtonClickSound();
        PlayerPrefs.SetInt("PlayerCash", PlayerPrefs.GetInt("PlayerCash") + 1000);
        scoreText.text = PlayerPrefs.GetInt("PlayerCash").ToString();
        addCashPanel.SetActive(false);
    }

    public void cashAddPanel()
    {
        SoundManager.instance.PlayButtonClickSound();
        addCashPanel.SetActive(true);
    }

    public void sound()
    {
        SoundManager.instance.PlayButtonClickSound();
    }

    void callAds()
    {
        AdsManager.Instance.ShowInterstitial("");
    }
    #endregion
}
