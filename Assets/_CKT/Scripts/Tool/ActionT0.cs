using System;

public class ActionT0
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
        SafeInvoke();
    }

    void SafeInvoke()
    {
        if (_action == null) return;

        Delegate[] delegates = _action.GetInvocationList();
        _action = null;
        foreach (Delegate del in delegates)
        {
            if (del.Target != null || del.Method.IsStatic)
            {
                _action += (Action)del;
            }
        }

        _action?.Invoke();
    }
}
