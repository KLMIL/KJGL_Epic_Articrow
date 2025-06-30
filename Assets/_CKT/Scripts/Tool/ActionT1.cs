using System;
using UnityEngine;

public class ActionT1<T1>
{
    Action<T1> _action = null;

    public void Init()
    {
        _action = null;
    }

    public void Unregister(Action<T1> callback)
    {
        _action -= callback;
    }

    public void Register(Action<T1> callback)
    {
        _action += callback;
    }

    public void SingleRegister(Action<T1> callback)
    {
        _action = null;
        _action += callback;
    }

    public void Trigger(T1 param)
    {
        //safeInvoke(param);
        if (_action == null)
        {
            Debug.Log("[ckt] ActionT1 is null");
        }
        _action?.Invoke(param);
    }

    void safeInvoke(T1 param)
    {
        if (_action == null) return;

        Delegate[] delegates = _action.GetInvocationList();
        _action = null;
        foreach (Delegate del in delegates)
        {
            if (del.Target != null || del.Method.IsStatic)
            {
                _action += (Action<T1>)del;
            }
        }

        _action?.Invoke(param);
    }
}
