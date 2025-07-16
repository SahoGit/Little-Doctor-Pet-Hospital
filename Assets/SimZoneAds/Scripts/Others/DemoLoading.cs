using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DemoLoading : SplashCallback
{
    [SerializeField] Image LoadingBar;
    [SerializeField] [Range(1, 100)] float LoadingDuration = 7f;
    [SerializeField] [Range(0, 5)] int SceneToLoadIndex;
    private void Awake()
    {
        FirebaseRemote.AddOrUpdateValue("floatvalue", 0.99f);
        FirebaseRemote.AddOrUpdateValue("intvalue", 25);
        FirebaseRemote.AddOrUpdateValue("booleanvalue", true);
        FirebaseRemote.AddOrUpdateValue("stringvalue", "string value specidied");
        Application.targetFrameRate = 60;
    }

    public override void OnPolicyAccepted()
    {
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime / LoadingDuration;
            LoadingBar.fillAmount = timer;
            yield return null;
        }

        SceneManager.LoadScene(SceneToLoadIndex);
    }
}
