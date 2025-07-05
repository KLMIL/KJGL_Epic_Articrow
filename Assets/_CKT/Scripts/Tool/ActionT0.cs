using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionT0
{
    Dictionary<Action, (WeakReference weak, string name)> _targetDict = new();

    public void Init()
    {
        _targetDict.Clear();
    }

    public void Unsubscribe(Action callback)
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

    public void Subscribe(Action callback)
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

    public void SingleSubscribe(Action callback)
    {
        Init();
        Subscribe(callback);
    }

    public void Publish()
    {
        if (_targetDict.Count == 0)
        {
            //Debug.LogWarning($"{this} subscriber is null");
            return;
        }

        List<Action> removeList = new();
        foreach (Action action in _targetDict.Keys)
        {
            WeakReference weak = _targetDict[action].weak;
            if (weak.IsAlive && weak.Target is MonoBehaviour mono && (mono != null))
            {
                action?.Invoke();
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
