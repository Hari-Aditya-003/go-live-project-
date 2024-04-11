using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Manages all types of Game Events. 
/// All the communications should be from this class across the layers 
/// </summary>
public class EventManager
{
    /// <summary>
    /// Base class for all types of Events
    /// </summary>
    [System.Serializable]
    public class GameEvent
    {
        
    }

    /// <summary>
    ///  Is Limiting the Queue Processing with QueueProcessTime
    /// </summary>
    public bool LimitQueueProcesing = false;

    /// <summary>
    /// if LimitQueueProcesing true, how much duration is allowed to process the events in single frame
    /// </summary>
    public float QueueProcessTime = 0.2f;
    //private static EventManager s_Instance = null;
    private Queue mEventQueue = new Queue();

    public delegate void EventDelegate<T>(T e) where T : GameEvent;
    private delegate void EventDelegate(GameEvent e);

    private Dictionary<System.Type, EventDelegate> delegates = new Dictionary<System.Type, EventDelegate>();
    private Dictionary<System.Delegate, EventDelegate> delegateLookup = new Dictionary<System.Delegate, EventDelegate>();
    private Dictionary<System.Delegate, bool> onceLookups = new Dictionary<System.Delegate, bool>();
    private static EventManager _instance = null;

    /// <summary>
    /// Is Instance Destroyed
    /// </summary>
    private static bool _isDestroyed = false;

    // override so we don't have the typecast the object
    public static EventManager Instance
    {
        get
        {
            if (!_isDestroyed && _instance == null)
            {
                _instance = new EventManager();
            }

            return _instance;
        }
    }

    public bool IsDestroyed
    {
        get
        {
            return _isDestroyed;
        }

        set
        {
            _isDestroyed = value;
        }
    }

    /// <summary>
    /// Adds a delgate to a Event
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="del"></param>
    /// <returns></returns>
    private EventDelegate AddDelegate<T>(EventDelegate<T> del) where T : GameEvent
    {
        // Early-out if we've already registered this delegate
        if (delegateLookup.ContainsKey(del))
            return null;

        // Create a new non-generic delegate which calls our generic one.
        // This is the delegate we actually invoke.
        EventDelegate internalDelegate = (e) => del((T)e);
        delegateLookup[del] = internalDelegate;

        EventDelegate tempDel;
        if (delegates.TryGetValue(typeof(T), out tempDel))
        {
            delegates[typeof(T)] = tempDel += internalDelegate;
        }
        else
        {
            delegates[typeof(T)] = internalDelegate;
        }

        return internalDelegate;
    }

    /// <summary>
    /// Registering Liseners
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="del"></param>
    public void AddListener<T>(EventDelegate<T> del) where T : GameEvent
    {
        AddDelegate<T>(del);
        //Debug.LogError("LISTENER ADDEDDD " + del);
    }

    /// <summary>
    /// Add Listener internal function
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="del"></param>
    private void AddListenerOnce<T>(EventDelegate<T> del) where T : GameEvent
    {
        EventDelegate result = AddDelegate<T>(del);

        if (result != null)
        {
            // remember this is only called once
            onceLookups[result] = true;
        }
    }


    /// <summary>
    /// Unregistering the listener
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="del"></param>
    public void RemoveListener<T>(EventDelegate<T> del) where T : GameEvent
    {

        //Debug.LogError("LISTENER REMOVED " + del);
        EventDelegate internalDelegate;
        if (delegateLookup.TryGetValue(del, out internalDelegate))
        {
            EventDelegate tempDel;
            if (delegates.TryGetValue(typeof(T), out tempDel))
            {
                tempDel -= internalDelegate;
                if (tempDel == null)
                {
                    delegates.Remove(typeof(T));
                }
                else
                {
                    delegates[typeof(T)] = tempDel;
                }
            }

            delegateLookup.Remove(del);
        }
    }

    /// <summary>
    /// Removes all listeners
    /// </summary>
    private void RemoveAll()
    {
        delegates.Clear();
        delegateLookup.Clear();
        onceLookups.Clear();
    }

    /// <summary>
    /// Does Listener already Registered or not
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="del"></param>
    /// <returns></returns>
    public bool HasListener<T>(EventDelegate<T> del) where T : GameEvent
    {
        return delegateLookup.ContainsKey(del);
    }

    /// <summary>
    /// Raise Event Syncronously
    /// </summary>
    /// <param name="e"></param>
    public void TriggerEvent(GameEvent e)
    {
        EventDelegate del;
        if (delegates.TryGetValue(e.GetType(), out del))
        {
            del.Invoke(e);
#if LOG_EVENTS
            Debug.Log("<color=green> Event " + e + " </color>");
#endif
            // remove listeners which should only be called once
            foreach (EventDelegate k in delegates[e.GetType()].GetInvocationList())
            {
                if (onceLookups.ContainsKey(k))
                {
                    onceLookups.Remove(k);
                }
            }
        }
        else
        {
            Debug.LogWarning("Event: " + e.GetType() + " has no listeners");
        }
    }

    /// <summary>
    /// Queues the Events to be process Syncronously
    /// </summary>
    /// <param name="evt"></param>
    /// <returns></returns>
    public bool QueueEvent(GameEvent evt)
    {
        if (!delegates.ContainsKey(evt.GetType()))
        {
            //Debug.LogWarning("EventManager: QueueEvent failed due to no listeners for event: " + evt.GetType());
            return false;
        }

        mEventQueue.Enqueue(evt);
        return true;
    }

    /// <summary>
    /// Every update cycle the queue is processed, if the queue processing is limited,
    /// a maximum processing time per update can be set after which the events will have
    /// to be processed next update loop.
    /// </summary>
    public void Update()
    {
        float startTime = Time.timeSinceLevelLoad;
        
        while (mEventQueue.Count > 0)
        {
            if (LimitQueueProcesing)
            {
                if (Time.timeSinceLevelLoad - startTime > QueueProcessTime)
                    return;
            }

            GameEvent evt = mEventQueue.Dequeue() as GameEvent;
            TriggerEvent(evt);
        }
    }

    /// <summary>
    /// Clears all events and queues
    /// </summary>
    public void Release()
    {
        RemoveAll();
        mEventQueue.Clear();
        _isDestroyed = true;
    }

    internal void AddListener<T>()
    {
        throw new NotImplementedException();
    }
}