﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PandaView : MonoBehaviour {
	#region Variables, Constants & Initializers

	// Use this for initialization

	private bool timeflag=false;
	private int boneCounter = 0;
	private int dragCounter = 0;
	private int eatingCounter = 0;
    public GameObject sideTable;
    public GameObject sideTable1;
    public GameObject panda;
	public RectTransform bottomTableEndPoint, sideTableEndPoint, sideTableStartPoint, pandaEndPoint;
	public GameObject XrayMachine,originalXrayMachine;
	private Vector3 startXrayPosition;
	private Vector3 startXrayMachinePosition;
	public RectTransform xrayMachineStartPoint, xrayMachineEndPoint, xrayMachineCrackPoint; 
	public GameObject xrayimage;
	public GameObject warningImage;
	public GameObject bonesPopup;
	public GameObject headBandage, ArmBandage,headBandage1, ArmBandage1;
	public RectTransform bandageEndPoint, bandageStartPoint, headBandageOutPoint, armBandageOutPoint;
	public GameObject headBandageHand, armHand, eatingHand;
	public RectTransform headHandEndPOint, armHandEndPoint, eatingHandEndPoint;
	public GameObject medicineTray, medicine;
	public RectTransform medicineTrayEndPoint, medicineTrayStartPoint, medicineMouthPoint;
	public GameObject plate, spoon;
    public GameObject itemPlate;
    public Sprite [] spoonSprites;
    public RectTransform plateEndPoint, itemStartPoint, itemEndPoint, plateStartPoint, spoonEndPoint, spoonMouthPoint, eatingPoint;
    public GameObject pandaFace, mouth;
	public GameObject nextButton;
	public GameObject lastpopup, treamentText;
	public RectTransform treatmentTextEndPoint;
	public Image popupImage;
	public GameObject originalPopup;
	public GameObject headSwell, armSwell;
	public Image blackScreen;
	public GameObject newBg;
	public GameObject newPanda;
	public RectTransform newPandaEndPoint;
	public GameObject frame, cameraButton, lastNextButton;
	public GameObject levelEndParticles;
	public Image expressions;
	public Sprite [] faceImages;
    public GameObject levelComplete;

    public GameObject homeButton;
    public GameObject backButton;
    public GameObject menuButton;

    public int levelCount;

    public Text XPText;

    public Image[] EatingItemsImages;

    public Sprite[] EatingItemsSprite;

    public GameObject taskDonePanel,EatingContent,Loadingbg;
    public Image FillImage;
    #endregion

    #region Lifecycle Methods
    void Awake () {
		GameManager.instance.currentScene = GameUtils.PANDA_VIEW_SCENE;
	}
	// Use this for initialization
	void Start () {
        AdsManager.Instance.ShowBanner();
        PlayerPrefs.SetInt("ComingFromSplash", 0);
        //GameManager.instance.currentScene = GameUtils.PANDA_VIEW_SCENE;
        Invoke("SetViewContents", 0.1f);
        Invoke("WaitGrid", 0.5f);
        if (PlayerPrefs.GetInt("LevelPlayID") >= 19)
        {
            headSwell.SetActive(false);
            armSwell.SetActive(false);
        }
        //Invoke("SetViewContents", 0.1f);
        //if (PlayerPrefs.GetInt("RealLevelPlayID") >= 19)
        //{
        //    headSwell.SetActive(false);
        //    armSwell.SetActive(false);
        //}
        eatingItemsSetting();
    }


    #endregion

    #region Utility Methods
    private void WaitGrid()
    {
        EatingContent.gameObject.SetActive(true);
    }
    private void SetViewContents() {

		startXrayPosition = xrayimage.GetComponent<RectTransform> ().localPosition;
		startXrayMachinePosition = XrayMachine.GetComponent<RectTransform> ().localPosition;
		//xrayMachineComesInn ();
		animalFaceMovement ();
		pandaPositioning ();
        //		bonesPopupActive();
        //headBandageComesInn ();
        //	medicineTrayComesInn();
        //plateComesInn ();
    }

    void eatingItemsSetting()
    {
        for (int t = 0; t < EatingItemsSprite.Length; t++)
        {
            Sprite tmp = EatingItemsSprite[t];
            int r = Random.Range(t, EatingItemsSprite.Length);
            EatingItemsSprite[t] = EatingItemsSprite[r];
            EatingItemsSprite[r] = tmp;
        }
        for (int i = 0; i < EatingItemsImages.Length; i++)
        {
            EatingItemsImages[i].sprite = EatingItemsSprite[i];
            //EatingItemsImages[i].GetComponent<Image>().SetNativeSize();
            //Debug.Log(EatingItemsSprite[i].name);
        }
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

	private void ShakeAction(GameObject obj) {
		Hashtable tweenParams = new Hashtable();
		tweenParams.Add ("amount", new Vector3 (0.02f, 0.02f, 0.02f));
		tweenParams.Add ("time", 1.0f);
		tweenParams.Add ("easetype", iTween.EaseType.easeInCubic);
		tweenParams.Add ("looptype", iTween.LoopType.pingPong);
		iTween.ShakePosition(obj, tweenParams);
	}

	private void animalFaceMovement(){
		RotateAction (pandaFace, -4.0f, 3.0f, iTween.EaseType.linear, iTween.LoopType.pingPong);
	}

	private void colorIncreases(Image img, float val){
		if (img.color.a < 1) {
			img.color = new Vector4 (img.color.r,img.color.g,img.color.b, img.color.a + val);
		}
	}

	private void colorDecreases(Image img , float value){
		if (img.color.a > 0) {
			img.color = new Vector4 (img.color.r,img.color.g,img.color.b, img.color.a - value);
		}
	}
		
	private void pandaPositioning(){
		SoundManager.instance.PlayToolComesSound ();
		MoveAction (panda, pandaEndPoint, 0.8f, iTween.EaseType.easeInBounce, iTween.LoopType.none);
		Invoke ("sideTablePositioning", 0.9f);
	}

	private void sideTablePositioning(){
        if (PlayerPrefs.GetInt("CareerMode") == 1)
        {
            if (PlayerPrefs.GetInt("LevelPlayID") == 17)
            {
                MoveAction(sideTable, sideTableEndPoint, 0.5f, iTween.EaseType.easeInBounce, iTween.LoopType.none);
            }
            else if (PlayerPrefs.GetInt("LevelPlayID") == 18)
            {
                MoveAction(sideTable1, sideTableEndPoint, 0.5f, iTween.EaseType.easeInBounce, iTween.LoopType.none);
                Invoke("headBandageComesInn", 0.7f);
            }
            else if (PlayerPrefs.GetInt("LevelPlayID") == 19)
            {
                MoveAction(sideTable, sideTableStartPoint, 0.5f, iTween.EaseType.easeInBounce, iTween.LoopType.none);
                Invoke("eatingItemsComesInn", 0.7f);
            }
        }
        else if (PlayerPrefs.GetInt("RealCareerMode") == 1)
        {
            if (PlayerPrefs.GetInt("RealLevelPlayID") == 17)
            {
                MoveAction(sideTable, sideTableEndPoint, 0.5f, iTween.EaseType.easeInBounce, iTween.LoopType.none);
            }
            else if (PlayerPrefs.GetInt("RealLevelPlayID") == 18)
            {
                MoveAction(sideTable1, sideTableEndPoint, 0.5f, iTween.EaseType.easeInBounce, iTween.LoopType.none);
                Invoke("headBandageComesInn", 0.7f);
            }
            else if (PlayerPrefs.GetInt("RealLevelPlayID") == 19)
            {
                MoveAction(sideTable, sideTableStartPoint, 0.5f, iTween.EaseType.easeInBounce, iTween.LoopType.none);
                Invoke("eatingItemsComesInn", 0.7f);
            }
        } 
		else
        {
            MoveAction(sideTable, sideTableEndPoint, 0.5f, iTween.EaseType.easeInBounce, iTween.LoopType.none);
            //MoveAction(sideTable, sideTableStartPoint, 0.5f, iTween.EaseType.easeInBounce, iTween.LoopType.none);
            //Invoke("eatingItemsComesInn", 0.7f);
        }
    }

	private void xrayMachineComesInn(){
		SoundManager.instance.PlayToolComesSound ();
		XrayMachine.SetActive (true);
		MoveAction (XrayMachine, xrayMachineEndPoint, 0.5f, iTween.EaseType.linear, iTween.LoopType.none);
		Invoke ("xrayMachineListenerOn", 0.6f);
	}

	private void xrayMachineListenerOn(){
		XrayMachine.GetComponent<ApplicatorListener> ().enabled = true;
	}

	private void xrayMachineGoOutside(){
		MoveAction (XrayMachine, xrayMachineStartPoint, 0.5f, iTween.EaseType.easeInBack  , iTween.LoopType.none);
	}

	private void xrayMachineShake(){
		ShakeAction (XrayMachine.gameObject);
		iTween.Pause (XrayMachine.gameObject);
		xrayMachineGoOutside ();
		Invoke ("bonesPopupActive", 0.3f);
	}
	private void bonesPopupActive(){
		
		warningImage.SetActive (true);
		SoundManager.instance.playWarningSound ();
	}

	private void bonesPopupClose(){
		SoundManager.instance.PlayPopupCloseSound ();
		bonesPopup.SetActive (false);
		expressions.sprite = faceImages [0];
		ParticleManger.instance.showPointingParticle (warningImage.gameObject);
		Invoke ("headBandageComesInn", 0.5f);
        Debug.Log("Level1 Done");
        if (PlayerPrefs.GetInt("CareerMode") == 1)
        {
            levelCompletePanel(17, 17);
        }
        if (PlayerPrefs.GetInt("RealCareerMode") == 1)
        {
            levelCompleteCareerPanel(17, 17);
        }
        if (PlayerPrefs.GetInt("PetCareMode") == 1 || PlayerPrefs.GetInt("RandomMode") == 1)
        {
            taskDonePanel.SetActive(true);
        }
    }

	private void headBandageComesInn(){
		expressions.sprite = faceImages [0];
		SoundManager.instance.PlayToolComesSound ();
		headBandage.SetActive (true);
		MoveAction (headBandage, bandageEndPoint, 0.5f, iTween.EaseType.linear, iTween.LoopType.none);
		Invoke ("headBandageListenerOn", 0.5f);
	}

	private void headBandageListenerOn(){
		headBandageHand.SetActive (true);
		MoveAction (headBandageHand, headHandEndPOint, 1.0f, iTween.EaseType.linear, iTween.LoopType.loop);
		headBandage.GetComponent<ApplicatorListener> ().enabled = true;
	}

	private void armBandageComesInn(){
		SoundManager.instance.PlayToolComesSound ();
		ArmBandage.SetActive (true);
		MoveAction (ArmBandage, bandageEndPoint, 0.5f, iTween.EaseType.linear, iTween.LoopType.none);
		Invoke ("ArmBandageListenerOn", 0.5f);
	}

	private void ArmBandageListenerOn(){
		expressions.sprite = faceImages [0];
		armHand.SetActive (true);
		MoveAction (armHand, armHandEndPoint, 1.0f, iTween.EaseType.linear, iTween.LoopType.loop);
		ArmBandage.GetComponent<ApplicatorListener> ().enabled = true;
	}
		
	private void medicineTrayComesInn(){
		SoundManager.instance.PlayToolComesSound ();
		medicineTray.SetActive (true);
		MoveAction (medicineTray, medicineTrayEndPoint, 0.5f, iTween.EaseType.linear, iTween.LoopType.none);
		Invoke ("makingMedicineListenerOn", 0.5f);
	}

	private void makingMedicineListenerOn(){
		medicine.GetComponent<ApplicatorListener> ().enabled = true;
	}

	private void scaleOutMedicine(){
		ScaleAction (medicine, 0.0f, 1.0f,iTween.EaseType.linear, iTween.LoopType.none);
		Invoke ("medicineTrayGoesout", 1.0f);
	}

	private void medicineTrayGoesout(){
		ParticleManger.instance.showPointingParticle (pandaFace.gameObject);
		MoveAction (medicineTray, medicineTrayStartPoint, 0.5f, iTween.EaseType.easeInBack, iTween.LoopType.none);
        MoveAction(sideTable, sideTableStartPoint, 0.5f, iTween.EaseType.easeInBounce, iTween.LoopType.none);
        //Invoke("plateComesInn", 0.7f);
        Invoke("eatingItemsComesInn", 0.7f);
        Debug.Log("Level2 Done");
        if (PlayerPrefs.GetInt("CareerMode") == 1)
        {
            levelCompletePanel(18, 10);
        }
        if (PlayerPrefs.GetInt("RealCareerMode") == 1)
        {
            levelCompleteCareerPanel(18, 10);
        }
        if (PlayerPrefs.GetInt("PetCareMode") == 1 || PlayerPrefs.GetInt("RandomMode") == 1)
        {
            taskDonePanel.SetActive(true);
        }
    }

	private void plateComesInn(){
		SoundManager.instance.PlayToolComesSound ();
		plate.SetActive (true);
		MoveAction (plate, plateEndPoint, 0.5f, iTween.EaseType.linear, iTween.LoopType.none);
		Invoke ("spoonListenerOn", 0.6f);
    }

    private void eatingItemsComesInn()
    {
        SoundManager.instance.PlayToolComesSound();
        MoveAction(itemPlate, itemEndPoint, 0.5f, iTween.EaseType.linear, iTween.LoopType.none);
		Invoke("eatingItemHand", 0.7f);
    }

	private void eatingItemHand()
	{

        eatingHand.SetActive(true);
        MoveAction(eatingHand, eatingHandEndPoint, 1.0f, iTween.EaseType.linear, iTween.LoopType.loop);
    }

    private void spoonListenerOn(){
		spoon.GetComponent<ApplicatorListener> ().enabled = true;
	}

	private void platesGoOutside(){
		MoveAction (plate, plateStartPoint, 0.5f, iTween.EaseType.easeInBack  , iTween.LoopType.none);
		Invoke ("toolsCompleted", 0.6f);
	}

	private void toolsCompleted(){
		ParticleManger.instance.showPointingParticle (pandaFace.gameObject);
        if (PlayerPrefs.GetInt("PetCareMode") == 1)
        {
            if (PlayerPrefs.GetInt("AnimalPlayed") == 4)
            {
                PlayerPrefs.SetInt("AnimalPlayed", 5);
            }
        }
        //Invoke ("popupActive", 2.0f);
        Debug.Log("Level3 Done");
        if (PlayerPrefs.GetInt("CareerMode") == 1)
        {
            levelCompletePanel(19, 1);
        }
        else if (PlayerPrefs.GetInt("PetCareMode") == 1)
        {
            levelCompletePetCare();
        }
        else if (PlayerPrefs.GetInt("RandomMode") == 1)
        {
            levelCompleteRandom();
        }
        else if (PlayerPrefs.GetInt("RealCareerMode") == 1)
        {
            levelCompleteCareerPanel(19, 1);
        }
        else
        {
            Invoke("popupActive", 2.0f);
        }
    }

	private void popupActive(){
		lastpopup.SetActive (true);
		ScaleAction (lastpopup.transform.GetChild(0).gameObject, 1.0f, 0.5f, iTween.EaseType.linear, iTween.LoopType.none);
		SoundManager.instance.PlayPopupCloseSound ();
		Invoke ("treatmentTextAppear", 1.2f);
	}

	private void treatmentTextAppear(){
		SoundManager.instance.PlayGirlSound ();
		ScaleAction (treamentText, 1.0f, 0.3f, iTween.EaseType.linear, iTween.LoopType.none);
		Invoke ("treatMentStamp", 0.3f);
	}

	private void treatMentStamp(){
		SoundManager.instance.playstampSound ();
		MoveAction (treamentText, treatmentTextEndPoint, 0.3f, iTween.EaseType.easeInBounce, iTween.LoopType.none);
		Invoke ("nextButtonActive", 0.7f);
		Invoke ("treamentParticleAppear", 0.3f);
	}

	private void treamentParticleAppear(){
		ParticleManger.instance.showStarParticle (treamentText);
       
    }


	private void nextButtonActive(){
		nextButton.SetActive (true);
	}


	private void popUpClose(){
		StartCoroutine (FadeOutAction(popupImage));
		ScaleAction (originalPopup, 0.0f, 0.5f, iTween.EaseType.linear, iTween.LoopType.none);
		Invoke ("pandaBandgeGoesOut", 1.2f);

	}

	private void pandaBandgeGoesOut(){
		SoundManager.instance.PlayToolGoesSound ();
		MoveAction (headBandage1, headBandageOutPoint, 0.5f, iTween.EaseType.linear, iTween.LoopType.none);
		MoveAction (ArmBandage1, armBandageOutPoint, 0.5f, iTween.EaseType.linear, iTween.LoopType.none);
		Invoke ("blackScreenActive", 1.5f);
	}

	private void blackScreenActive(){
		blackScreen.gameObject.SetActive (true);
		newBg.SetActive (true);
		StartCoroutine (FadeOutAction(blackScreen));
		Invoke ("newPandaComes", 2.0f);
	}

	private void newPandaComes(){
		blackScreen.gameObject.SetActive (false);
		newPanda.SetActive (true);
		MoveAction (newPanda, newPandaEndPoint, 0.5f, iTween.EaseType.linear, iTween.LoopType.none);
		Invoke ("cameraButtonActive", 0.7f);
	}

	private void cameraButtonActive(){
		cameraButton.SetActive (true);
        if (PlayerPrefs.GetInt("RandomMode") == 1)
        {
            if (PlayerPrefs.GetInt("RaceModeModeAvailable") == 0)
            {
                PlayerPrefs.SetInt("RaceModeModeAvailable", 1);
            }
        }
    }

	private void photoFrameComes(){
		frame.SetActive (true);
		ScaleAction (frame, 1.0f, 0.3f, iTween.EaseType.easeInBounce, iTween.LoopType.none);
		Invoke ("lastNextButtonActive", 0.7f);
	}

	private void lastNextButtonActive(){
		SoundManager.instance.playLevelCompletedSound ();
		levelEndParticles.SetActive (true);
        if (PlayerPrefs.GetInt("PetCareMode") == 1)
        {
            if (PlayerPrefs.GetInt("AnimalPlayed") == 4)
            {
                PlayerPrefs.SetInt("AnimalPlayed", 5);
            }
        }
		lastNextButton.SetActive (true);

	}

	#endregion

	#region Utility Methods
	public void xrayMachineBeginDrag(){
		//iTween.Pause (XrayMachine);
		XrayMachine.transform.GetChild(0).gameObject.SetActive(true);
		XrayMachine.GetComponent<Image> ().enabled = false;

	}
	public void xrayMachineEndDrag(){
		XrayMachine.transform.GetChild(0).gameObject.SetActive(false);
		XrayMachine.GetComponent<Image> ().enabled = true;

	}

	public void xrayMachineDrag(){
		expressions.sprite = faceImages [1];
		Vector3 difference = startXrayMachinePosition - XrayMachine.GetComponent<RectTransform> ().localPosition;
		Vector3 localPosition = xrayimage.GetComponent<RectTransform> ().localPosition;
		xrayimage.GetComponent<RectTransform>().localPosition = new Vector3(startXrayPosition.x - 370 + difference.x, startXrayPosition.y + 180 + difference.y, localPosition.z);
//		print ("DragCouter"+dragCounter++);
		dragCounter++;
		if(dragCounter >= 200){
			SoundManager.instance.PlayscanningLoop (false);
			XrayMachine.GetComponent<ApplicatorListener> ().enabled = false;
			//MoveAction (XrayMachine, xrayMachineCrackPoint, 0.3f, iTween.EaseType.linear, iTween.LoopType.none);
			Invoke("xrayMachineShake", 0.3f);
		}
	}

	public void onCollisionWithXrayMachine(){

	}
		
	public void OnCollisionWithBones(){
		boneCounter++;
		if(boneCounter >= 5){
			Invoke ("bonesPopupClose", 1.5f);

		}
	}

	public void onCollisionWithHadBandage(){
		expressions.sprite = faceImages [1];
		headBandageHand.SetActive (false);
		headSwell.SetActive (false);
		Invoke("armBandageComesInn", 1.0f);
	}

	public void onCollisionWithArmBandage(){
		expressions.sprite = faceImages [1];
		armHand.SetActive (false);
		armSwell.SetActive (false);
		Invoke("medicineTrayComesInn", 1.0f);

        Debug.Log("Level2 Done");
    }

	public void onCollisionOfMedicine(){
		expressions.sprite = faceImages [1];
		SoundManager.instance.playmedicineSound ();
		MoveAction (medicine, medicineMouthPoint, 0.5f, iTween.EaseType.linear, iTween.LoopType.none);
		Invoke ("scaleOutMedicine", 0.5f);
	}

	public void onCollisionSpoonWithMouth(){
		expressions.sprite = faceImages [0];
		if (GameManager.instance.currentItem == "mouth") {
			MoveAction (spoon, spoonMouthPoint, 0.5f, iTween.EaseType.linear, iTween.LoopType.none);
			Invoke ("spoonEmpty", 0.8f);
		}
		else if (GameManager.instance.currentItem == "Plate") {
			spoon.GetComponent<Image>().sprite = spoonSprites [1];
		}
    }

    public void OnCollisionWithEatingItems(GameObject eatingItem)
    {
        SoundManager.instance.PlayBiteSound();
        MoveAction(eatingItem, eatingPoint, 0.5f, iTween.EaseType.linear, iTween.LoopType.none);
        ScaleAction(eatingItem, 0.0f, 2.0f, iTween.EaseType.linear, iTween.LoopType.none);
		Destroy(eatingItem.transform.parent.gameObject, 2.0f);
        eatingHand.SetActive(false);
        Invoke("eatingItemGoesBack", 2.0f);
        Invoke("toolsCompleted", 2.0f);
    }

    void eatingItemGoesBack()
    {
        //MoveAction(itemPlate, itemStartPoint, 0.5f, iTween.EaseType.linear, iTween.LoopType.none);
    }

    private void spoonEmpty(){
		SoundManager.instance.PlayBiteSound ();
		spoon.GetComponent<Image>().sprite = spoonSprites [0];
		Invoke ("smoothStartPoint", 0.3f);
	}

	private void smoothStartPoint(){
		MoveAction (spoon, spoonEndPoint, 0.5f, iTween.EaseType.linear, iTween.LoopType.none);
		Invoke("spoonAgainActive", 0.7f);
	}

	private void spoonAgainActive(){

		//		 {
		spoon.GetComponent<ApplicatorListener> ().enabled = true;
		spoon.GetComponent<BoxCollider2D> ().enabled = true;
		plate.GetComponent<BoxCollider2D> ().enabled = true;
		mouth.GetComponent<BoxCollider2D> ().enabled = true;
		print ("eatingCounter"+ ++eatingCounter);
		if (eatingCounter > 3) {
			spoon.GetComponent<ApplicatorListener> ().enabled = false;
			spoon.GetComponent<BoxCollider2D> ().enabled = false;
			plate.GetComponent<BoxCollider2D> ().enabled = false;
			mouth.GetComponent<BoxCollider2D> ().enabled = false;
			Invoke("platesGoOutside", 0.3f);
		}
	}


	IEnumerator FadeOutAction (Image img)
	{
		if (img.color.a >0) {
			img.color = new Vector4 (img.color.r,img.color.g,img.color.b, img.color.a - 0.03f);
			yield return new WaitForSeconds (0.05f);
			StartCoroutine (FadeOutAction (img));
		}  else if (img.color.a < 0) {
			StopCoroutine (FadeOutAction (img));
		}
	}

	public void onCliockWarningButton(){
		SoundManager.instance.PlayPopupCloseSound ();
		warningImage.SetActive (false);
		bonesPopup.SetActive (true);

	}

	public void onClickNextButton(){
		SoundManager.instance.PlayButtonClickSound ();
		nextButton.SetActive (false);
		popUpClose ();


	}

	public void OnClickCameraButton(){
		SoundManager.instance.playcameraSound ();
		cameraButton.SetActive (false);
		photoFrameComes ();
	}

	public void OnClickLastNext(){
		SoundManager.instance.PlayButtonClickSound ();
        NavigationManager.instance.ReplaceScene(GameScene.MAINMENU);
        //if (PlayerPrefs.GetInt("RandomMode") == 1)
        //{
        //    NavigationManager.instance.ReplaceScene(GameScene.MAINMENU);
        //}
        //else if(PlayerPrefs.GetInt("PetCareMode") == 1)
        //{
        //    NavigationManager.instance.ReplaceScene(GameScene.MAINMENU);
        //}
        //else
        //{
        //    NavigationManager.instance.ReplaceScene(GameScene.RECEPTIONView);
        //}

    }

    public void homeBtn()
    {
        SoundManager.instance.PlayButtonClickSound();
        if (PlayerPrefs.GetInt("CareerMode") == 1)
        {
            PlayerPrefs.SetInt("StoryLevelPlayID", PlayerPrefs.GetInt("StoryLevelPlayID") + 1);
            PlayerPrefs.SetInt("StoryMode", 0);
        }
        if (PlayerPrefs.GetInt("RealCareerMode") == 1)
        {

        }
        levelComplete.SetActive(false);
        Loadingbg.SetActive(true);
        StartCoroutine(GameManager.instance.FillLoading(FillImage));
        Invoke("callAds", 2.0f);
        Invoke("callScene", 3.7f);
    }

    public void nextBtn()
    {
        SoundManager.instance.PlayButtonClickSound();
        if (PlayerPrefs.GetInt("CareerMode") == 1)
        {
            PlayerPrefs.SetInt("StoryLevelPlayID", PlayerPrefs.GetInt("StoryLevelPlayID") + 1);
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
            PlayerPrefs.SetInt("StoryMode", 1);
        }
        if (PlayerPrefs.GetInt("RealCareerMode") == 1)
        {

        }

        
        //if (levelCount == 19)
        //{
        //    PlayerPrefs.SetInt("LevelPlayID", 0);
        //    NavigationManager.instance.ReplaceScene(GameScene.CATVIEW);
        //}
        //if (levelCount == 18)
        //{
        //    PlayerPrefs.SetInt("LevelPlayID", 1);
        //    NavigationManager.instance.ReplaceScene(GameScene.CATVIEW);
        //}
        //if (levelCount == 17)
        //{
        //    PlayerPrefs.SetInt("LevelPlayID", 13);
        //    NavigationManager.instance.ReplaceScene(GameScene.BUNNYVIEW);
        //}
        levelComplete.SetActive(false);
        Loadingbg.SetActive(true);
        StartCoroutine(GameManager.instance.FillLoading(FillImage));
        Invoke("callAds", 2.0f);
        Invoke("callScene", 3.7f);
    }
    void callAds()
    {
        AdsManager.Instance.ShowInterstitial("");
    }
    void callScene()
    {
        Loadingbg.SetActive(false);
        NavigationManager.instance.ReplaceScene(GameScene.MAINMENU);
    }
    void levelCompletePanel(int levelId, int levelPlayingID)
    {
        int levelPlayID = levelPlayingID - 1;

        //Debug.Log(PlayerPrefs.GetInt("StoryLevelPlayID") + "StoryLevelPlayIDFarrukh");
        if (PlayerPrefs.GetInt("CareerMode") == 1)
        {
            ParticleManger.instance.showPointingParticle(pandaFace.gameObject);
            if (PlayerPrefs.GetInt("LevelPlayed") == levelPlayID)
            {
                PlayerPrefs.SetInt("LevelPlayed", levelPlayID + 1);

            }
        }
        levelCount = levelId;
        //Destroy(GameObject.Find("MEDIUM_RECTANGLE(Clone)"));
        levelComplete.SetActive(true);
        PlayerPrefs.SetInt("PlayerCash", PlayerPrefs.GetInt("PlayerCash") + 100);
        //PlayerPrefs.SetInt("StoryLevelPlayID", PlayerPrefs.GetInt("StoryLevelPlayID") + 1);
        Invoke("starSound", 1.10f);
        SoundManager.instance.PlayButtonClickSound();
    }

    void levelCompleteCareerPanel(int levelId, int levelPlayingID)
    {
        int levelPlayID = levelPlayingID - 1;

        //Debug.Log(PlayerPrefs.GetInt("StoryLevelPlayID") + "StoryLevelPlayIDFarrukh");
        if (PlayerPrefs.GetInt("RealCareerMode") == 1)
        {
            ParticleManger.instance.showPointingParticle(pandaFace.gameObject);
            if (PlayerPrefs.GetInt("RealLevelPlayed") == levelPlayID)
            {
                PlayerPrefs.SetInt("RealLevelPlayed", levelPlayID + 1);

            }
        }
        levelCount = levelId;
        //Destroy(GameObject.Find("MEDIUM_RECTANGLE(Clone)"));
        levelComplete.SetActive(true);
        PlayerPrefs.SetInt("PlayerCash", PlayerPrefs.GetInt("PlayerCash") + 100);
        //PlayerPrefs.SetInt("StoryLevelPlayID", PlayerPrefs.GetInt("StoryLevelPlayID") + 1);
        Invoke("starSound", 1.10f);
        SoundManager.instance.PlayButtonClickSound();
    }

    public void closePanel(GameObject parent)
	{
		parent.SetActive(false);
        SoundManager.instance.PlayButtonClickSound();
    }

    void levelCompletePetCare()
    {
        XPText.text = "XP 1000+";
        PlayerPrefs.SetInt("PlayerCash", PlayerPrefs.GetInt("PlayerCash") + 1000);
        levelComplete.SetActive(true);
        Invoke("starSound", 1.10f);
    }

    void levelCompleteRandom()
    {
        XPText.text = "XP 2000+";
        PlayerPrefs.SetInt("PlayerCash", PlayerPrefs.GetInt("PlayerCash") + 2000);
        levelComplete.SetActive(true);
        Invoke("starSound", 1.10f);
    }

    public void doubleReward()
    {
        if (PlayerPrefs.GetInt("CareerMode") == 1)
        {
            PlayerPrefs.SetInt("PlayerCash", PlayerPrefs.GetInt("PlayerCash") + 200);
        }
        else if (PlayerPrefs.GetInt("RealCareerMode") == 1)
        {
            PlayerPrefs.SetInt("PlayerCash", PlayerPrefs.GetInt("PlayerCash") + 200);
        }
        else if (PlayerPrefs.GetInt("PetCareMode") == 1)
        {
            PlayerPrefs.SetInt("PlayerCash", PlayerPrefs.GetInt("PlayerCash") + 1000);
        }
        else if (PlayerPrefs.GetInt("RandomMode") == 1)
        {
            PlayerPrefs.SetInt("PlayerCash", PlayerPrefs.GetInt("PlayerCash") + 2000);
        }
        nextBtn();
    }

    public void starSound()
    {
        SoundManager.instance.PlayStarSound();
        Invoke("starSound1", 0.3f);
    }

    public void starSound1()
    {
        SoundManager.instance.PlayStarSound();
        Invoke("starSound2", 0.3f);
    }

    public void starSound2()
    {
        SoundManager.instance.PlayStarSound();
    }

    public void taskPanelClose(GameObject panel)
    {
        SoundManager.instance.PlayButtonClickSound();
        Loadingbg.SetActive(true);
        StartCoroutine(GameManager.instance.FillLoading(FillImage));
        Invoke("callAds", 2.0f);
        Invoke("OffLoading", 3.7f);
        panel.SetActive(false);
    }
    void OffLoading()
    {
        Loadingbg.SetActive(false);
    }

    public void taskHomeBtn()
    {
        SoundManager.instance.PlayButtonClickSound();
        Loadingbg.SetActive(true);
        StartCoroutine(GameManager.instance.FillLoading(FillImage));
        Invoke("callAds", 2.0f);
        Invoke("callScene", 3.7f);
    }

    #endregion
}