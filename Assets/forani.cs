using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forani : MonoBehaviour
{
    // Start is called before the first frame update
   public void Stopani()
    {
        GetComponent<Animator>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
