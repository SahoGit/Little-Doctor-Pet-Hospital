
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InternetPanelManager : MonoBehaviour
{
    [SerializeField] PopupStatus Visibility = PopupStatus.OnSceneChange;
    [SerializeField] string resourcePath = "Prefabs/InternetPanel";
#if UNITY_EDITOR
    [Header("Editor Only")]
    public bool InternetAvailable = true;
#endif

    GameObject Panel;

    public void Initialize()
    {
        if (Visibility.Equals(PopupStatus.OnDemand) || AdConstants.AdsRemoved) return;
        if (Visibility.Equals(PopupStatus.OnUpdateLoop))
            AdsManager.Instance.SetInternetCallback(Hide);
    }

    void OnEnable()
    {
        if (Visibility.Equals(PopupStatus.OnSceneChange)) SceneManager.activeSceneChanged += ActiveSceneChanged;
    }

    void OnDisable()
    {
        if (Visibility.Equals(PopupStatus.OnSceneChange)) SceneManager.activeSceneChanged -= ActiveSceneChanged;
    }

    void ActiveSceneChanged(Scene arg0, Scene arg1)
    {
        Hide(AdConstants.InternetAvailable);
    }

    public void Hide(bool internetStatus)
    {
        if (internetStatus) // Hide Panel
        {
            if (Panel != null)
            {
                Destroy(Panel);
                Panel = null;
            }
        }
        else // Show Panel
        {
            if (!AdConstants.AdsRemoved && AdsManager.RemoteSettings.ShowInternetPopup)
            {
                if (Panel == null)
                {
                    Panel = Instantiate(Resources.Load(resourcePath) as GameObject);
                    DontDestroyOnLoad(Panel);
                }
            }
        }
    }

    public enum PopupStatus { OnDemand, OnUpdateLoop, OnSceneChange }
}