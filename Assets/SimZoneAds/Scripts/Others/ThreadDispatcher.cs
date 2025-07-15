using System;
using System.Collections.Generic;

public class ThreadDispatcher
{
    static readonly Queue<Action> adEventsQueue = new Queue<Action>();
    static volatile bool adEventsQueueEmpty = true;

    public static void Enqueue(Action action)
    {
        lock (adEventsQueue)
        {
            adEventsQueue.Enqueue(action);
            adEventsQueueEmpty = false;
        }
    }

    public void ExecuteEvents()
    {
        if (adEventsQueueEmpty) return;

        lock (adEventsQueue)
        {
            while (adEventsQueue.Count > 0)
            {
                try
                {
                    adEventsQueue.Dequeue()?.Invoke();
                }
                catch (Exception e)
                {
                    if (AdConstants.UseLogs)
                    {
                        DebugAds.LogException(DebugTag.Dispatcher, e);
                    }
                    else
                    {
                        //AppMetrica.Instance.ReportError(e, "Dispatcher");
#if kz_gameanalytics_enabled
                        string message = $"Dispatcher : Message = {e.Message}";
                        GameAnalyticsSDK.Events.GA_Debug.HandleLog(message, e.StackTrace, LogType.Exception);
#endif
                    }
                }

                adEventsQueueEmpty = true;
            }
        }
    }
}
