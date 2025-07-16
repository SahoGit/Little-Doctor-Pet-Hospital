﻿using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventSystem))]
[RequireComponent(typeof(StandaloneInputModule))]
class EventSystemEnabler : MonoBehaviour
{
    [SerializeField] EventSystem m_EventSystem;
    [SerializeField] StandaloneInputModule m_InputModule;

    private void OnEnable()
    {
        if (EventSystem.current == null)
            m_EventSystem.enabled = m_InputModule.enabled = true;
        else
            m_EventSystem.enabled = m_InputModule.enabled = false;
    
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        m_EventSystem = GetComponent<EventSystem>();
        m_InputModule = GetComponent<StandaloneInputModule>();
        m_EventSystem.enabled = m_InputModule.enabled = false;
    }
#endif
}