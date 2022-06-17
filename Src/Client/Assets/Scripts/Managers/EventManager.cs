using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager> {

    public delegate void DelegateCustomMessage(params object[] param);
    private Dictionary<string, List<DelegateCustomMessage>> events = new Dictionary<string, List<DelegateCustomMessage>>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addEventListener(string eventName, DelegateCustomMessage call)
    {
        if(!events.ContainsKey(eventName))
        {
            events[eventName] = new List<DelegateCustomMessage>();
        }
        events[eventName].Add(call);
    }

    public void removeEventListener(string eventName, DelegateCustomMessage call)
    {
        if (events.ContainsKey(eventName))
        {
            int i = 0;
            List<DelegateCustomMessage> calls = events[eventName];
            while(i < calls.Count)
            {
                if(calls[i] == call)
                {
                    calls.RemoveAt(i);
                }
                else
                {
                    ++i;
                }
            }
        }
    }

    public void removeAllEventListener()
    {
        foreach(var item in events)
        {
            for(int i = item.Value.Count - 1; i >= 0; --i)
            {
                item.Value.RemoveAt(i);
            }
        }
    }

    public void dispatchCustomEvent(string eventName, params object[] param)
    {
        List<DelegateCustomMessage> calls = null;
        events.TryGetValue(eventName, out calls);
        if(calls != null)
        {
            for(int i = 0; i < calls.Count; ++i)
            {
                calls[i](param);
            }
        }
    }
}
