using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalGenrator : MonoBehaviour
{
    public GameObject[] spike;
    public bool isGameStoped;
    private GameObject script;
    private int petNumber = 0;
    int previousPet;
    // Start is called before the first frame update
    void Start()
    {
        isGameStoped = true;
        script = GameObject.Find("AnimalsSpawn");
        Invoke("Spawn", 15.0f);
    }

    private Quaternion newRotation;
    void Spawn()
    {
        
        


        if (isGameStoped)
        {
            if (petNumber == 0 || petNumber == 1)
            {
                Quaternion newRotation = Quaternion.Euler(new Vector3(0, 180, 0));
                Instantiate(spike[petNumber], new Vector3(script.transform.position.x, script.transform.position.y, 0), newRotation);
            }
            else
            {
                Quaternion newRotation = Quaternion.identity;
                Instantiate(spike[petNumber], new Vector3(script.transform.position.x, script.transform.position.y, 0), newRotation);
            }
            if (petNumber < spike.Length - 1)
            {
                petNumber++;
            } else
            {
                petNumber = 0;
            }
        }
        Invoke("Spawn", Random.Range(10.0f, 12.0f));
    }
}
