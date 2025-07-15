using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashManager : MonoBehaviour
{
    [SerializeField] SplashCallback Callback;
    [SerializeField] string resourcePath = "Prefabs/PolicyPanel (Landscape)";
    SplashPolicyPanel PolicyPanel;

    void Start()
    {
        if (AdConstants.PolicyAccepted)
            OnPolicyAccepted();
        else
        {
            PolicyPanel = Instantiate(Resources.Load(resourcePath) as GameObject).GetComponent<SplashPolicyPanel>();
            PolicyPanel.SetListeners(Accept, VisitWebsite);
            new GameObject("MonetizationDebugger", typeof(MonetizationDebugger));
        }
    }

    void Accept()
    {
        OnPolicyAccepted();
        AdConstants.AcceptPolicy();
        PolicyPanel.HideCanvas();
    }

    void VisitWebsite()
    {
        AdsManager.Instance.VisitPrivacyPolicy();
    }

    void OnPolicyAccepted()
    {
        AdsManager.Instance.Initialize();
        Callback.OnPolicyAccepted();
    }
}
