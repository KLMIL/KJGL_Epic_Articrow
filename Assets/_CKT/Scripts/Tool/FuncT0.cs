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

    public void SingleSubscribe(Func<TResult> callback)
    {
        _func = null;
        _func += callback;
    }

    public TResult Publish()
    {
        if (_func == null)
        {
            Debug.LogError("_func is null. Check class FuncT0<TResult>");
            return default;
        }
        else
        {
            return _func.Invoke();
        }
    }
}
