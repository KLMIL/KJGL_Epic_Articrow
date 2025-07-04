using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionT1<T1>
{
    Dictionary<Action<T1>, (WeakReference weak, string name)> _targetDict = new();

    public void Init()
    {
        _targetDict.Clear();
    }

    public void Unsubscribe(Action<T1> callback)
    {
        if (_targetDict.ContainsKey(callback))
        {
            _targetDict.Remove(callback);
        }
        /*else
        {
            string name = (callback.Target is MonoBehaviour mono && (mono != null)) ? mono.gameObject.name : "UnKnown";
            Debug.LogWarning($"{name}'s Action does not contain");
        }*/
    }

    public void Subscribe(Action<T1> callback)
    {
        if (callback.Target is MonoBehaviour mono && (mono != null))
        {
            _targetDict[callback] = (new WeakReference(mono), mono.gameObject.name);
        }
        /*else if (name == null)
        {
            Debug.LogError("Subscriber is null");
        }
        else if (_targetDict.ContainsValue(name))
        {
            Debug.LogError("Subscriber already contains");
        }*/
    }

    public void SingleSubscribe(Action<T1> callback)
    {
        Init();
        Subscribe(callback);
    }

    public void Publish(T1 param1)
    {
        if (_targetDict.Count == 0)
        {
            //Debug.LogWarning($"{this} subscriber is null");
            return;
        }

        List<Action<T1>> removeList = new();
        foreach (Action<T1> action in _targetDict.Keys)
        {
            WeakReference weak = _targetDict[action].weak;
            if (weak.IsAlive && weak.Target is MonoBehaviour mono && (mono != null))
            {
                action?.Invoke(param1);
            }
            else
            {
                //Debug.LogError($"{_targetDict[action]} invoke action try failed");
                removeList.Add(action);
            }
        }
        for (int i = 0; i < removeList.Count; i++)
        {
            Unsubscribe(removeList[i]);
        }
    }
}
