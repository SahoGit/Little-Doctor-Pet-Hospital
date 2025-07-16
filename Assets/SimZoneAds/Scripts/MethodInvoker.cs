using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MethodInvoker
{
    private static List<DelayedActionData> _delayedActions = new List<DelayedActionData>(8);
    private static int m_Count = 0;

    public void ExecuteDelayedActions()
    {
        if (m_Count > 0)
        {
            for (int i = 0; i < m_Count; i++)
            {
                bool executed = _delayedActions[i].ExecuteAfterDelay();
                if (!executed) continue;

                _delayedActions.RemoveAt(i);
                m_Count = _delayedActions.Count;
                i--;
            }
        }
    }

    public static void InvokeDelayed(Action action, float delay, bool ignoreTimeScale = true)
    {
        _delayedActions.Add(new DelayedActionData(action, delay, ignoreTimeScale));
        m_Count = _delayedActions.Count;
    }

    private class DelayedActionData
    {
        Action action;
        float delay;
        bool ignoreTimeScale;

        public DelayedActionData(Action action, float delay, bool ignoreTimeScale)
        {
            this.action = action;
            this.delay = delay;
            this.ignoreTimeScale = ignoreTimeScale;
        }

        public bool ExecuteAfterDelay()
        {
            if (ignoreTimeScale)
            {
                delay -= Time.unscaledDeltaTime;
            }
            else
            {
                delay -= Time.deltaTime;
            }

            if (delay <= 0)
            {
                action.Invoke();
                return true;
            }

            return false;
        }
    }
}