using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Randonpop : MonoBehaviour
{
   // public GameObject Rate_us;
    public GameObject special;
    public GameObject fullversion;
    public GameObject petpanal;

    public static int popup_value = 0;
    void OnEnable()
    {
      //  AdsManager.Instance.ShowBanner();
        //Input.multiTouchEnabled = false;
        popup_value++;
       StartCoroutine(Load_PopUp());
    }
    IEnumerator Load_PopUp()
    {
        yield return new WaitForSeconds(2f);
        if (popup_value == 2 && PlayerPrefs.GetInt("UnlockAll") == 0)
        {
            fullversion.SetActive(true);
        }
        //if (popup_value == 3)
        //{
        //    fullversion.SetActive(true);
        //}

        if (popup_value == 3 && PlayerPrefs.GetInt("UnlockAll") == 0)
        {
            petpanal.SetActive(true);
        }
        if (popup_value == 4 && PlayerPrefs.GetInt("UnlockAll") == 0)
        {
            special.SetActive(true);
        }
        if (popup_value == 5)
        {
            //UnityEngine.iOS.Device.RequestStoreReview();
            //Rate_us.SetActive(true);
           // popup_value = 0;
        }
        if (popup_value == 6)
        {
            //UnityEngine.iOS.Device.RequestStoreReview();

            popup_value = 0;
        }

    }


}
