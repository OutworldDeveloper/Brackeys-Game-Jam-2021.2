using System;
using System.Collections.Generic;


public interface IInputReciverProcessor
{
    bool ExecuteWhenPaused { get; }
    string ReciverName { get; }
    bool EatEverything { get; }
    bool ProcesssActionDown(string key);
    bool ProcessActionUp(string key);
    bool ProcessAxis(string key, float value);
    void ReleaseAll();
    void ZeroAllAxes();

}

public class InputReciver : IInputReciverProcessor
{
    public bool ExecuteWhenPaused { get; }
    public string ReciverName { get; }
    public bool EatEverything { get; set; }

    private readonly Dictionary<string, Action> _actionsBindsPressed = new Dictionary<string, Action>();
    private readonly Dictionary<string, Action> _actionsBindsReleased = new Dictionary<string, Action>();
    private readonly Dictionary<string, Action<float>> _axisBinds = new Dictionary<string, Action<float>>();

    public InputReciver(bool executeWhenPaused, string reciverName = "Information is not provided")
    {
        ExecuteWhenPaused = executeWhenPaused;
        ReciverName = reciverName;
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

    bool IInputReciverProcessor.ProcesssActionDown(string key)
    {
        if (_actionsBindsPressed.TryGetValue(key, out Action action))
        {
            action.Invoke();
            return true;
        }
        return false;
    }

    bool IInputReciverProcessor.ProcessActionUp(string key)
    {
        if (_actionsBindsReleased.TryGetValue(key, out Action action))
        {
            action.Invoke();
            return true;
        }
        return false;
    }

    bool IInputReciverProcessor.ProcessAxis(string key, float value)
    {
        if (_axisBinds.TryGetValue(key, out Action<float> action))
        {
            action.Invoke(value);
            return true;
        }
        return false;
    }

    void IInputReciverProcessor.ReleaseAll()
    {
        foreach (var action in _actionsBindsReleased)
        {
            action.Value.Invoke();
        }
    }

    void IInputReciverProcessor.ZeroAllAxes()
    {
        foreach (var axis in _axisBinds)
        {
            axis.Value.Invoke(0f);
        }
    }

}