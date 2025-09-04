using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SR4BlackDev
{
    public class EventDispatcher : MonoBehaviour {

    // Use this for initialization
    #region initalization
    Dictionary<EventID, List<Action<object, object>>> listActionCallBack 
        = new Dictionary<EventID, List<Action<object, object>>>(); 
    #endregion
    #region singleton
   static EventDispatcher _instance;
    public static EventDispatcher Instance
    {
        get
        {
            if(_instance==null)
            {
                GameObject go = new GameObject();
                go.name = "Event Dispatcher Manager";
                _instance = go.AddComponent<EventDispatcher>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
        private set { }
    }
    void Awake()
    {
        if(_instance!=null&&_instance.GetInstanceID()!=this.GetInstanceID())
        {
            Destroy(gameObject);
        }
      
    }
    void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
    #endregion
    #region main method
    public void registerListener(EventID eventId, Action<object, object> callback)
    {
        if(listActionCallBack.ContainsKey(eventId))
        {
            listActionCallBack[eventId].Add(callback);
            //Debug.Log("Add new action to event name is: " + eventId.ToString());
        }
        else
        {
            var newList = new List<Action<object, object>>();
            newList.Add(callback);
            listActionCallBack.Add(eventId, newList);
           // Debug.Log("Register new Event name is: "+eventId.ToString());
        }
    }
    public void postEvent(EventID eventId, object sender, object param=null)
    {
        List<Action<object, object>> actionList;
        if(listActionCallBack.TryGetValue(eventId, out actionList))
        {
            for(int i=0, amount = actionList.Count; i<amount; i++ )
            {
                try
                {
                    actionList[i](sender, param);
                }
                catch(Exception ex)
                {
                    //Debug.LogError("Error when call event " + eventId.ToString());
                    //Debug.LogException(ex);
                    actionList.RemoveAt(i);
                    if(actionList.Count<0)
                    {
                        listActionCallBack.Remove(eventId);
                    }
                    amount--;
                    i--;
                }
            }
        }
        else
        {
           // Debug.LogError("No listener for event name: " + eventId.ToString());
        }
    }
    public void removeListener(EventID eventId, Action<object, object> callback)
    {
        List<Action<object, object>> actionList;
        if(listActionCallBack.TryGetValue(eventId, out actionList))
        {
            if(actionList.Contains(callback))
            {
                actionList.Remove(callback);
                if(actionList.Count<0)
                {
                    listActionCallBack.Remove(eventId);
                }
            }
            else
            {
                Debug.LogWarning("RemoveListener, event :"+ eventId.ToString()+" no listener found");
            }
        }

    }
    public void removeRedundancies()
    {
        foreach (var keyPairs in listActionCallBack)
        {
            var listenerList = keyPairs.Value;
            for (int amount = listenerList.Count, i = amount - 1; i >= 0; i--)
            {
                var listener = listenerList[i];
                // Use Target.Equal(null) instead of Target == null, it won't work
                if (listener == null || listener.Target.Equals(null))
                {
                    listenerList.RemoveAt(i);
                    if (listenerList.Count == 0)
                    {
                        // no listener remain, then delete this key
                        listActionCallBack.Remove(keyPairs.Key);
                    }
                    i--;
                }
            }
        }
    }
    public void clearAllListener()
    {
        listActionCallBack.Clear();
    }
    #endregion


    public static void PostEvent(EventID eventId, object param)
    {
        Instance.postEvent(eventId, null, param);
    }

    public static void PostEvent(EventID eventId)
    {
        Instance.postEvent(eventId, null, null);
    }
}
public static class EventDispatcherExtensions
{
    public static void RegisterListener(this object sender, EventID eventId, Action<object, object> callback)
    {
        EventDispatcher.Instance.registerListener(eventId, callback);
    }
    public static void PostEvent(this object sender, EventID eventId, object param)
    {
        EventDispatcher.Instance.postEvent(eventId, sender, param);
    }
    public static void PostEvent(this object sender, EventID eventID)
    {
        EventDispatcher.Instance.postEvent(eventID, sender, null);
    }

    
    public static void RemoveListener(this object sender, EventID eventID, Action<object, object> callback)
    {
        EventDispatcher.Instance.removeListener(eventID, callback);
    }
}


}
