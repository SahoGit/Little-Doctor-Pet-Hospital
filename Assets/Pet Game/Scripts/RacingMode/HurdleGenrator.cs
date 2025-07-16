using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class HurdleGenrator : MonoBehaviour
{
    public GameObject[] hurdle;
    private GameObject script;
    // Start is called before the first frame update
    void Start()
    {
        script = GameObject.Find("HurdleSpawn");
        Invoke("Spawn", 3);
    }

    void Spawn()
    {
        Instantiate(hurdle[Random.Range(0, hurdle.Length - 1)], new Vector3(script.transform.position.x, script.transform.position.y, 0), Quaternion.identity);

        Invoke("Spawn", 3);
    }
}
