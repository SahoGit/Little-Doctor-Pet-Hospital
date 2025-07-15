using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReceController : MonoBehaviour
{
    public int id;
    public Button lockBtn;
    public GameObject lockImage;
    public GameObject animalImage;
    //Start is called before the first frame update
    void Start()
    {
        if ((PlayerPrefs.GetInt("AnimalPlayed") >= id) || (PlayerPrefs.GetInt("UnlockAll", 0) == 1) || (PlayerPrefs.GetInt(transform.gameObject.name) == 1))
        {
            lockImage.SetActive(false);
            lockBtn.GetComponent<Button>().interactable = true;
            animalImage.GetComponent<Image>().color = new Color(255, 255, 255, 255);
        }
        else
        {
            lockImage.SetActive(true);
            lockBtn.GetComponent<Button>().interactable = false;

            lockBtn.GetComponent<ActionManager>().enabled = false;
        }
    }
    public void unlockAnimal()
    {
        PlayerPrefs.SetInt(transform.gameObject.name, 1);
        lockImage.SetActive(false);
        lockBtn.GetComponent<Button>().interactable = true;
        animalImage.GetComponent<Image>().color = new Color(255, 255, 255, 255);
    }

    public void onRacingModeGame(int petId)
    {
        PlayerPrefs.SetInt("PetSelectedForRace", petId);
        SceneManager.LoadScene(2);
    }
}
