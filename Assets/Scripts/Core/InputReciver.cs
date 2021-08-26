using System;
using System.Collections.Generic;

public class InputReciver 
{

    public readonly bool ExecuteWhenPaused;
    public readonly string Information;
    public bool EatEverything { get; set; }
    public IReadOnlyDictionary<string, List<Action>> ActionsBindsPressed => _actionsBindsPressed;
    public IReadOnlyDictionary<string, Action> ActionsBindsReleased => _actionsBindsReleased;
    public IReadOnlyDictionary<string, Action<float>> AxisBinds => _axisBinds;

    private readonly Dictionary<string, List<Action>> _actionsBindsPressed = new Dictionary<string, List<Action>>();
    private readonly Dictionary<string, Action> _actionsBindsReleased = new Dictionary<string, Action>();
    private readonly Dictionary<string, Action<float>> _axisBinds = new Dictionary<string, Action<float>>();

    public InputReciver(bool executeWhenPaused, string information = "Information is not provided")
    {
        ExecuteWhenPaused = executeWhenPaused;
        Information = information;
    }

    public void BindInputActionPressed(string name, params Action[] actions)
    {
        if (_actionsBindsPressed.ContainsKey(name) == false)
        {
            _actionsBindsPressed.Add(name, new List<Action>());
        }
        foreach (var action in actions)
        {
            _actionsBindsPressed[name].Add(action);
        }
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