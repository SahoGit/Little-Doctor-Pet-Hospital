using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageController : MonoBehaviour
{
    public int packageId;
    // Start is called before the first frame update
    void Start()
    {
        PackageSelecter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PackageSelecter()
    {
        if (PlayerPrefs.GetInt("SelectedPackage") != packageId)
        {
            this.gameObject.SetActive(false);
        }
    }
}
