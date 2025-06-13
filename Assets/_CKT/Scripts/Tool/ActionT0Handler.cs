using System;

public class ActionT0Handler
{
    Action _action = null;

    public void Init()
    {
        _action = null;
    }

    public void Unregister(Action callback)
    {
        _action -= callback;
    }

    public void Register(Action callback)
    {
        _action += callback;
    }

    public void SingleRegister(Action callback)
    {
        _action = null;
        _action += callback;
    }

    public void Trigger()
    {
        _action?.Invoke();
    }
}
