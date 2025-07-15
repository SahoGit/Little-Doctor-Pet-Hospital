using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SplashPolicyPanel : MonoBehaviour
{
    [SerializeField] Button m_AcceptBtn;
    [SerializeField] Button m_PolicyBtn;
    [SerializeField] Canvas m_Canvas;

    public void SetListeners(UnityAction onAccept, UnityAction onVisit)
    {
        m_AcceptBtn.onClick.AddListener(onAccept);
        m_PolicyBtn.onClick.AddListener(onVisit);
    }

    public void HideCanvas()
    {
        m_Canvas.enabled = false;
    }
}
