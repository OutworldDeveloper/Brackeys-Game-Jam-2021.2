using System;
using System.Collections.Generic;

// This is not really possible to use right now
public interface IInputReciver
{
    bool EatEverything { get; set; }
    void BindInputActionPressed(string id, Action action);
    void BindInputActionRelesed(string id, Action action);
    void BindAxis(string id, Action<float> action);

}

public class InputReciver : IInputReciver
{

    public readonly bool ExecuteWhenPaused;
    public readonly string ReciverName;
    public bool EatEverything { get; set; }

    private readonly Dictionary<string, Action> _actionsBindsPressed = new Dictionary<string, Action>();
    private readonly Dictionary<string, Action> _actionsBindsReleased = new Dictionary<string, Action>();
    private readonly Dictionary<string, Action<float>> _axisBinds = new Dictionary<string, Action<float>>();

    public InputReciver(bool executeWhenPaused, string reciverName = "Information is not provided")
    {
        ExecuteWhenPaused = executeWhenPaused;
        ReciverName = reciverName;
    }

    public bool ProcesssActionDown(string key)
    {
        if (_actionsBindsPressed.TryGetValue(key, out Action action))
        {
            action.Invoke();
            return true;
        }
        return false;
    }

    public bool ProcessActionUp(string key)
    {
        if (_actionsBindsReleased.TryGetValue(key, out Action action))
        {
            action.Invoke();
            return true;
        }
        return false;
    }

    public bool ProcessAxis(string key, float value)
    {
        if (_axisBinds.TryGetValue(key, out Action<float> action))
        {
            action.Invoke(value);
            return true;
        }
        return false;
    }

    public void ReleaseAll()
    {
        foreach (var action in _actionsBindsReleased)
        {
            action.Value.Invoke();
        }
    }

    public void ZeroAllAxes()
    {
        foreach (var axis in _axisBinds)
        {
            axis.Value.Invoke(0f);
        }
    }

    public void BindInputActionPressed(string name, Action action)
    {
        _actionsBindsPressed.Add(name, action);
    }

    public void BindInputActionRelesed(string name, Action action)
    {
        _actionsBindsReleased.Add(name, action);
    }

    public void BindAxis(string name, Action<float> action)
    {
        _axisBinds.Add(name, action);
    }

}