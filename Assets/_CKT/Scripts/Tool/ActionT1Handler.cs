using System;

public class ActionT1Handler<T1>
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
        _action?.Invoke(param);
    }
}
