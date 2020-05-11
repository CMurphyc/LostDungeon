using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading;
public class EventDispatcher
{
    private static EventDispatcher sinstance;
    public static EventDispatcher Instance()
    {
        if (sinstance == null)
        {
            sinstance = new EventDispatcher();
        }
        return sinstance;
    }



    //  object m_objLock = new object();
    //  object m_objLock2 = new object();
    public delegate void EventCallback(EventBase eb);
    private Dictionary<string, List<EventCallback>> registedCallbacks = new Dictionary<string, List<EventCallback>>();
    private Dictionary<string, List<EventCallback>> registedCallbacksPending = new Dictionary<string, List<EventCallback>>();
    private List<EventBase> lPendingEvents = new List<EventBase>();
    public void RegistEventListener(string sEventName, EventCallback eventCallback)
    {
        lock (this)
        {
            if (!registedCallbacks.ContainsKey(sEventName))
            {
                registedCallbacks.Add(sEventName, new List<EventCallback>());
            }
            if (isEnuming)
            {
                if (!registedCallbacksPending.ContainsKey(sEventName))
                {
                    registedCallbacksPending.Add(sEventName, new List<EventCallback>());
                }
                registedCallbacksPending[sEventName].Add(eventCallback);
                return;
            }
            registedCallbacks[sEventName].Add(eventCallback);
        }
    }
    public void UnregistEventListener(string sEventName, EventCallback eventCallback)
    {
        lock (this)
        {
            if (!registedCallbacks.ContainsKey(sEventName))
            {
                return;
            }
            if (isEnuming)
            {
                Debug.Log("Cannot unregist event this moment!");
                return;
            }
            registedCallbacks[sEventName].Remove(eventCallback);
        }
    }
    List<EventBase> lEvents = new List<EventBase>();
    public void DispatchEvent<T>(T eventInstance)
        where T : EventBase
    {
        lock (this)
        {
            if (!registedCallbacks.ContainsKey(eventInstance.sEventName))
            {
                return;
            }
            if (isEnuming)
            {
                lPendingEvents.Add(eventInstance);
                Debug.Log("Cannot dispatch event this moment!");
                return;
            }
            foreach (EventBase eb in lPendingEvents)
            {
                lEvents.Add(eb);
            }
            lPendingEvents.Clear();
            lEvents.Add(eventInstance);
        }
    }
    public void DispatchEvent(string eventName, object eventValue)
    {
        lock (this)
        {
            if (!registedCallbacks.ContainsKey(eventName))
            {
                return;
            }
            if (isEnuming)
            {
                lPendingEvents.Add(new EventBase(eventName, eventValue));
                Debug.Log("Cannot dispatch event this moment!");
                return;
            }

            if (lPendingEvents.Count > 0)
            {
                testPendingEvents();
            }

            lEvents.Add(new EventBase(eventName, eventValue));
        }
    }
    private void testPendingEvents()
    {
        for (int i = 0; i < lPendingEvents.Count;i++ )
        {
            lEvents.Add(lPendingEvents[i]);

        }
        lPendingEvents.Clear();
    }
    public static bool isEnuming = false;
    public void OnTick()
    {
        lock (this)
        {
            if (lEvents.Count == 0)
            {
                foreach (string sEventName in registedCallbacksPending.Keys)
                {
                    foreach (EventCallback ec in registedCallbacksPending[sEventName])
                    {
                        RegistEventListener(sEventName, ec);
                    }
                }
                registedCallbacksPending.Clear();
                testPendingEvents();
                return;
            }

            if (lPendingEvents.Count > 0)
            {
                testPendingEvents();
                return;
            }
            isEnuming = true;
          
            for (int j=0;j< lEvents.Count;j++ )
            {
                for (int i = 0; i < registedCallbacks[lEvents[j].sEventName].Count; i++)// EventCallback ecb in registedCallbacks[eb.sEventName])
                {
                    EventCallback ecb = registedCallbacks[lEvents[j].sEventName][i];
                    if (ecb == null)
                    {
                        continue;
                    }
                    ecb(lEvents[j]);
                }

            }
            lEvents.Clear();
        }
        isEnuming = false;
    }
}
public class EventBase
{
    public object eventValue;
    public string sEventName;
    public EventBase()
    {
        sEventName = this.GetType().FullName;
    }
    public EventBase(string eventName, object ev)
    {
        eventValue = ev;
        sEventName = eventName;
    }
}
public class ChatEvent : EventBase
{
    public int iChannel;
    public string sContent;
    public string sName;
    public ChatEvent()
    {
    }
}

