using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public interface IInputAction
{
    bool IsUp();
    bool IsDown();

}

public class KeyCodeAction : IInputAction
{

    private readonly KeyCode _keyCode;

    public KeyCodeAction(KeyCode keyCode)
    {
        _keyCode = keyCode;
    }

    public bool IsDown()
    {
        return Input.GetKeyDown(_keyCode);
    }

    public bool IsUp()
    {
        return Input.GetKeyUp(_keyCode);
    }

}

public class MouseAction : IInputAction
{

    private readonly int _button;
    private bool _isPressed;

    public MouseAction(int button)
    {
        _button = button;
    }

    public bool IsDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            _isPressed = false;
            return false;
        }

        if (Input.GetMouseButtonDown(_button))
        {
            _isPressed = true;
            return true;
        }

        return false;
    }

    public bool IsUp()
    {
        if (_isPressed && EventSystem.current.IsPointerOverGameObject())
        {
            _isPressed = false;
            return true;
        }
        _isPressed = !Input.GetMouseButtonUp(_button);
        return Input.GetMouseButtonUp(_button);
    }

}

public interface IInputAxis
{
    float GetValue();

}

public class TwoKeysAxis : IInputAxis
{

    private readonly KeyCode _keyCodeA;
    private readonly KeyCode _keyCodeB;

    public TwoKeysAxis(KeyCode keyCodeA, KeyCode keyCodeB)
    {
        _keyCodeA = keyCodeA;
        _keyCodeB = keyCodeB;
    }

    public float GetValue()
    {
        int value = 0;

        if (Input.GetKey(_keyCodeA))
            value++;
        if (Input.GetKey(_keyCodeB))
            value--;

        return value;
    }

}

public class UnityAxis : IInputAxis
{

    private readonly string _unityAxisName;

    public UnityAxis(string unityAxisName)
    {
        _unityAxisName = unityAxisName;
    }

    public float GetValue()
    {
        return Input.GetAxis(_unityAxisName);
    }

}

public class MouseScrollAxis : IInputAxis
{

    public float GetValue()
    {
        return Input.mouseScrollDelta.y;
    }

}

public class InputSystem : IInitializable, ITickable, IDisposable
{

    private readonly Dictionary<string, IInputAction> _inputActions = new Dictionary<string, IInputAction>()
    {
        { "mouse0", new MouseAction(0) },
        { "mouse1", new MouseAction(1) },
        { "mouse2", new MouseAction(2) },
        { "fire", new KeyCodeAction(KeyCode.Mouse0) },
        { "jump", new KeyCodeAction(KeyCode.Space) },
        { "pause", new KeyCodeAction(KeyCode.Escape) },
        { "submit", new KeyCodeAction(KeyCode.Return) },
    };

    private readonly Dictionary<string, IInputAxis> _inputAxes = new Dictionary<string, IInputAxis>()
    {
        { "moveForward", new TwoKeysAxis(KeyCode.W, KeyCode.S) },
        { "moveRight", new TwoKeysAxis(KeyCode.D, KeyCode.A) },
        { "qe", new TwoKeysAxis(KeyCode.E, KeyCode.Q) },
        { "mouseX", new UnityAxis("Mouse X") },
        { "mouseY", new UnityAxis("Mouse Y") },
        { "scroll", new MouseScrollAxis() },
    };

    private readonly IConsole _console;
    private readonly TimescaleManager _timescaleManager;
    private readonly List<InputReciver> _inputRecivers = new List<InputReciver>();

    public InputSystem(IConsole console, TimescaleManager timescaleManager)
    {
        _console = console;
        _timescaleManager = timescaleManager;
    }

    public void Initialize()
    {
        _console.RegisterObject(this);
        _timescaleManager.OnGamePaused += OnGamePaused;
    }

    public void Dispose()
    {
        _console.DeregisterObject(this);
        _timescaleManager.OnGamePaused -= OnGamePaused;
    }

    public void Tick()
    {
        foreach (var inputAction in _inputActions)
        {
            var actionName = inputAction.Key;
            var action = inputAction.Value;

            if (action.IsDown())
            {
                foreach (var reciver in _inputRecivers)
                {
                    if (_timescaleManager.IsGamePaused && reciver.ExecuteWhenPaused == false)
                        continue;
                    if (reciver.ActionsBindsPressed.ContainsKey(actionName))
                    {
                        foreach (var item in reciver.ActionsBindsPressed[actionName])
                        {
                            item.Invoke();
                        }
                        break;
                    }

                    if (reciver.EatEverything)
                        break;
                }
            }

            if (action.IsUp())
            {
                foreach (var reciver in _inputRecivers)
                {
                    if (_timescaleManager.IsGamePaused && reciver.ExecuteWhenPaused == false) 
                        continue;

                    if (reciver.ActionsBindsReleased.ContainsKey(actionName))
                    {
                        reciver.ActionsBindsReleased[actionName].Invoke();
                        break;
                    }

                    if (reciver.EatEverything)
                        break;
                }
            }
        }

        foreach (var inputAxis in _inputAxes)
        {
            var axisName = inputAxis.Key;
            var axis = inputAxis.Value;

            foreach (var reciver in _inputRecivers)
            {
                if (_timescaleManager.IsGamePaused && reciver.ExecuteWhenPaused == false)
                    continue;

                if (reciver.AxisBinds.ContainsKey(axisName))
                {
                    float value = axis.GetValue();
                    reciver.AxisBinds[axisName].Invoke(value);
                    break;
                }

                if (reciver.EatEverything)
                {
                    OnGamePaused();
                    break;
                }
            }
        }
    }

    public void AddReciver(InputReciver reciver) 
    {
        _inputRecivers.Insert(0, reciver);
    }

    public void AddReciverToTheBottomOfStack(InputReciver reciver)
    {
        _inputRecivers.Add(reciver);
    }

    public void RemoveReciver(InputReciver reciver)
    {
        _inputRecivers.Remove(reciver);
    }

    private void OnGamePaused()
    {
        foreach (var reciver in _inputRecivers)
        {
            if (reciver.ExecuteWhenPaused)
                continue;
            foreach (var action in reciver.ActionsBindsReleased.Values)
            {
                action.Invoke();
            }
        }
    }

    [ConsoleCommand("Prints registred input recivers")]
    public void PrintInputRecivers()
    {
        _console.Log($"{_inputRecivers.Count} active input recivers:");
        foreach (var reciver in _inputRecivers)
        {
            _console.Log($"{reciver.Information} (ExecuteWhenPaused: {reciver.ExecuteWhenPaused})");
        }
    }

}

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