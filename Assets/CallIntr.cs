using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallIntr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }
    private void OnEnable()
    {
        AdsManager.Instance.ShowInterstitial("");

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
