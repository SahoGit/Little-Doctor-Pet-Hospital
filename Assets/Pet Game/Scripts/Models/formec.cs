using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class formec : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        AdsManager.Instance.ShowMREC();
    }
    private void OnDisable()
    {
        AdsManager.Instance.HideMREC();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
