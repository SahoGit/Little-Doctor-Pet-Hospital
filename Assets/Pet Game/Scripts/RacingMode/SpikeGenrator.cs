using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpikeGenrator : MonoBehaviour
{
    public GameObject[] spike;
    public bool isGameStoped;
    private GameObject script;
    // Start is called before the first frame update
    void Start()
    {
        isGameStoped = true;
        script = GameObject.Find("EatingItemSpawn");
        Invoke("Spawn", 1.0f);
    }

    void Spawn()
    {
        if (isGameStoped)
        {
            Instantiate(spike[Random.Range(0, spike.Length - 1)], new Vector3(script.transform.position.x, script.transform.position.y, 0), Quaternion.identity);
        }
        Invoke("Spawn", Random.Range(2.0f, 4.0f));
    }
}
