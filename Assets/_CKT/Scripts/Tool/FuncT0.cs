using System;
using UnityEngine;

/// <summary>
/// FuncT0은 하나만 구독, SingleRegister만 존재
/// </summary>
/// <typeparam name="TResult"></typeparam>
public class FuncT0<TResult>
{
    Func<TResult> _func = null;

    public void Init()
    {
        _func = null;
    }

    public void Unregister(Func<TResult> callback)
    {
        _func -= callback;
    }

    public void SingleRegister(Func<TResult> callback)
    {
        _func = null;
        _func += callback;
    }

    public TResult Trigger()
    {
        if (_func == null)
        {
            Debug.LogError("_func is null. Check class FuncT0<TResult>");
            return default(TResult);
        }
        else
        {
            return _func.Invoke();
        }
    }
}
