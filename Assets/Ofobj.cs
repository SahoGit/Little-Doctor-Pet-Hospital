using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ofobj : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }
    private void OnEnable()
    {
        Invoke("callAdsText", 1.9f);

        gameObject.transform.GetChild(2).gameObject.SetActive(true);
    }
    // Update is called once per frame
    void callAdsText()
    {
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
    }
}
