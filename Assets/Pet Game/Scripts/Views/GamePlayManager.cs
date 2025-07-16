using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePlayManager : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject SpikeGenrator;
    public GameObject AnimalGenrator;

    public GameObject StartGamePanel;
    public GameObject HowToPlayPanel;
    public GameObject RacingModePanel;
    public GameObject EndGamePanel;
    public GameObject PausedPanel;
    public GameObject PauseBtn;

    public GameObject Cat;
    public GameObject Dog;
    public GameObject Tiger;
    public GameObject Panda;

    public GameObject[] Animals;

    public Text StartText;

    public Image HealthImage;
    // Start is called before the first frame update
    void Start()
    {
        StartGamePanel.SetActive(true);
        StartText.text = "Ready";
        MainCamera.GetComponent<CameraMovment>().enabled = false;
        SpikeGenrator.GetComponent<SpikeGenrator>().enabled = false;
        AnimalGenrator.GetComponent<AnimalGenrator>().enabled = false;
    }

    void two()
    {
        StartText.text = "Set";
        Invoke("one", 0.35f);
    }

    void one()
    {
        StartText.text = "Go";
        Invoke("zero", 0.35f);
    }

    void zero()
    {
        PauseBtn.SetActive(true);
        StartText.gameObject.SetActive(false);
        MainCamera.GetComponent<CameraMovment>().enabled = true;
        SpikeGenrator.GetComponent<SpikeGenrator>().enabled = true;
        AnimalGenrator.GetComponent<AnimalGenrator>().enabled = true;
        Animals[PlayerPrefs.GetInt("PetSelectedForRace")].GetComponent<Animator>().enabled = true;
        Animals[PlayerPrefs.GetInt("PetSelectedForRace")].GetComponent<PlayerScript>().isGameStart = true;
    }




    public void StartGamePlay()
    {
        SoundManager.instance.PlayButtonClickSound();
        SoundManager.instance.PlayGetSetGoSound();
        Animals[PlayerPrefs.GetInt("PetSelectedForRace")].SetActive(true);
        Invoke("two", 0.5f);
        StartText.gameObject.SetActive(true);
        StartGamePanel.SetActive(false);
    }

    public void SecondGamePlay()
    {
        SoundManager.instance.PlayButtonClickSound();
        //StartGamePanel.SetActive(true);
        MainCamera.GetComponent<CameraMovment>().enabled = true;
        SpikeGenrator.GetComponent<SpikeGenrator>().enabled = true;
        SpikeGenrator.GetComponent<SpikeGenrator>().isGameStoped = true;
        AnimalGenrator.GetComponent<AnimalGenrator>().enabled = true;
        AnimalGenrator.GetComponent<AnimalGenrator>().isGameStoped = true;
        Animals[PlayerPrefs.GetInt("PetSelectedForRace")].GetComponent<Animator>().enabled = true;
        HealthImage.fillAmount = 1;
        EndGamePanel.SetActive(false);
    }



    public void HowToPlay()
    {
        SoundManager.instance.PlayButtonClickSound();
        HowToPlayPanel.SetActive(true);
    }

    public void StopGamePlay()
    {
        SoundManager.instance.PlayButtonClickSound();
        //StartGamePanel.SetActive(true);
        MainCamera.GetComponent<CameraMovment>().enabled = false;
        SpikeGenrator.GetComponent<SpikeGenrator>().enabled = false;
        SpikeGenrator.GetComponent<SpikeGenrator>().isGameStoped = false;
        AnimalGenrator.GetComponent<AnimalGenrator>().enabled = false;
        AnimalGenrator.GetComponent<AnimalGenrator>().isGameStoped = false;
        Animals[PlayerPrefs.GetInt("PetSelectedForRace")].GetComponent<Animator>().enabled = false;

    }

    public void closePanel(GameObject parent)
    {
        SoundManager.instance.PlayButtonClickSound();
        parent.SetActive(false);
    }

    public void RestartGame()
    {
        SoundManager.instance.PlayButtonClickSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SoundManager.instance.PlayButtonClickSound();
        Destroy(GameObject.Find("GameManager"));
        Destroy(GameObject.Find("NavigationManager"));
        Destroy(GameObject.Find("AdsManagerSplash"));
        Destroy(GameObject.Find("AdCallingCanvas"));
        //Destroy(GameObject.Find("NotificationManager"));
        //Destroy(GameObject.Find("AndroidReceivedNotificationMainThreadDispatcher"));
        Destroy(GameObject.Find("SoundManager"));
        Destroy(GameObject.Find("ParticleManager"));
        SceneManager.LoadScene(1);
    }


    public void pauseGame()
    {
        if (Animals[PlayerPrefs.GetInt("PetSelectedForRace")].GetComponent<PlayerScript>().isGameStart)
        {
            SoundManager.instance.PlayButtonClickSound();
            PausedPanel.SetActive(true);
            MainCamera.GetComponent<CameraMovment>().enabled = false;
            SpikeGenrator.GetComponent<SpikeGenrator>().enabled = false;
            SpikeGenrator.GetComponent<SpikeGenrator>().isGameStoped = false;
            AnimalGenrator.GetComponent<AnimalGenrator>().enabled = false;
            AnimalGenrator.GetComponent<AnimalGenrator>().isGameStoped = false;
            Animals[PlayerPrefs.GetInt("PetSelectedForRace")].GetComponent<PlayerScript>().isGameStart = false;
            Animals[PlayerPrefs.GetInt("PetSelectedForRace")].GetComponent<Animator>().enabled = false;
        } 
        else
        {
            SoundManager.instance.PlayButtonClickSound();
            PausedPanel.SetActive(false);
            MainCamera.GetComponent<CameraMovment>().enabled = true;
            SpikeGenrator.GetComponent<SpikeGenrator>().enabled = true;
            SpikeGenrator.GetComponent<SpikeGenrator>().isGameStoped = true;
            AnimalGenrator.GetComponent<AnimalGenrator>().enabled = true;
            AnimalGenrator.GetComponent<AnimalGenrator>().isGameStoped = true;
            Animals[PlayerPrefs.GetInt("PetSelectedForRace")].GetComponent<PlayerScript>().isGameStart = true;
            Animals[PlayerPrefs.GetInt("PetSelectedForRace")].GetComponent<Animator>().enabled = true;
        }
        AdsManager.Instance.ShowInterstitial("");
        AdsManager.Instance.ShowInterstitial("");
    }



    public void jumpPlayer()
    {
        if (Animals[PlayerPrefs.GetInt("PetSelectedForRace")].GetComponent<PlayerScript>().isGameStart)
        {
            if (Animals[PlayerPrefs.GetInt("PetSelectedForRace")].GetComponent<PlayerScript>().isGrounded == true)
            {
                Animals[PlayerPrefs.GetInt("PetSelectedForRace")].GetComponent<Rigidbody2D>().AddForce(Vector2.up * Animals[PlayerPrefs.GetInt("PetSelectedForRace")].GetComponent<PlayerScript>().JumpForce);
                Animals[PlayerPrefs.GetInt("PetSelectedForRace")].GetComponent<Animator>().SetBool("Jump", true);
                Animals[PlayerPrefs.GetInt("PetSelectedForRace")].GetComponent<PlayerScript>().isGrounded = false;
                SoundManager.instance.PlayJumpSound();
            }
        }
    }

    public void chooseAnimalPanel()
    {
        RacingModePanel.SetActive(true);
        SoundManager.instance.PlayButtonClickSound();
    }

    public void chooseAnimal(int petId)
    {
        SoundManager.instance.PlayButtonClickSound();
        PlayerPrefs.SetInt("PetSelectedForRace", petId);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
