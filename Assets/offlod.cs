using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class offlod : MonoBehaviour
{
    public GameObject loading;
    void Start()
    {


    }
    private void OnEnable()
    {
        if (storyon.LoadOn==false)
        {
            print("OffStory");
            //storyon.LoadOn = false;
            loading.SetActive(false);
        }
        Invoke("wait1", 1f);
        //else if (story1.activeInHierarchy && story2.activeInHierarchy)
        //{
        //    //print("yessINN");
        //    loading.SetActive(true);
        //}
    }
    // Update is called once per frame
    void wait1()
    {
        storyon.LoadOn = false;
    }
}
