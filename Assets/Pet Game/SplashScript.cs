using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SplashScript : MonoBehaviour
{
    public GameObject Loading /*Policy*/;
   
public Image LoadingFilled;
    void Awake()
    {
        PlayerPrefs.SetInt("ComingFromSplash", 1);
        PlayerPrefs.SetInt("StartPanelPlayed", 0);
        int temp = PlayerPrefs.GetInt("PrivacyPolicy", 0);
        if (temp == 1)
        {
            LoadingBgActive();
        }
        ////else
        ////{
        //    LoadingBgActive();
        ////}
        ///
        //AdsManager.Instance.Initialize_Consent();
    }

    public void Accept()
    {
        PlayerPrefs.SetInt("PrivacyPolicy", 1);
        //Policy.SetActive(false);
        LoadingBgActive();
    }

    public void Visit()
    {
        Application.OpenURL("https://bestone-games.webnode.com/privacy-policy/");
        //PlayerPrefs.SetInt("PrivacyPolicy", 1);
        //Policy.SetActive(false);
        //LoadingBgActive();
       
    }
   public void LoadingBgActive(){
		Loading.SetActive (true);
		StartCoroutine (FillAction(LoadingFilled));
		Invoke ("LoadingFull", 5.0f);
	}

	IEnumerator FillAction (Image img){
		if (img.fillAmount < 1) {
			img.fillAmount = img.fillAmount + 0.005f;
			yield return new WaitForSeconds (0.03f);
			StartCoroutine (FillAction (img));
		}  else if (img.color.a >= 1f) {
			StopCoroutine (FillAction (img));
		}
	}

	private void LoadingFull(){
		print ("Loading Completed");
		SceneManager.LoadScene(1);
		//NavigationManager.instance.ReplaceScene (GameScene.CLEANINGVIEW);
	}

    //void AdCalled()
    //{
    //    AdsInitilizer.instance.CallAdsNow();
    //}
}
