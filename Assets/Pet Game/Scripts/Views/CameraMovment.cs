using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMovment : MonoBehaviour
{
    public float movespeed = 0;



    // Update is called once per frame
    void FixedUpdate()
    {
        //if (movespeed < 20)
        //{
        //    movespeed += 0.1f * Time.deltaTime;
        //}
        transform.Translate(new Vector3(1, 0, 0) * movespeed * Time.deltaTime);
        //transform.Translate(new Vector2(1f,0f) * movespeed * Time.deltaTime);
    }

    public void mainMenu()
    {

        SceneManager.LoadScene(1);
    }
}
