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

public class DynamicKeyCodeAction : IInputAction
{

    private readonly Setting_KeyCode _keyCode;

    public DynamicKeyCodeAction(Setting_KeyCode keyCode)
    {
        _keyCode = keyCode;
    }

    public bool IsDown()
    {
        return Input.GetKeyDown(_keyCode.GetValue());
    }

    public bool IsUp()
    {
        return Input.GetKeyUp(_keyCode.GetValue());
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

public class DynamicTwoKeysAxis : IInputAxis
{

    private readonly Setting_KeyCode _keyCodeA;
    private readonly Setting_KeyCode _keyCodeB;

    public DynamicTwoKeysAxis(Setting_KeyCode keyCodeA, Setting_KeyCode keyCodeB)
    {
        _keyCodeA = keyCodeA;
        _keyCodeB = keyCodeB;
    }

    public float GetValue()
    {
        int value = 0;

        if (Input.GetKey(_keyCodeA.GetValue()))
            value++;
        if (Input.GetKey(_keyCodeB.GetValue()))
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

public class InputSystem : MonoBehaviour
{

    [Inject] private IConsole _console;
    [Inject] private TimescaleManager _timescaleManager;
    [SerializeField] private InputActionBlueprint[] _actionsBlueprints;
    [SerializeField] private InputAxisBlueprint[] _axesBlueprints;

    private readonly Dictionary<string, IInputAction> _inputActions = new Dictionary<string, IInputAction>()
    {
        { "mouse0", new MouseAction(0) },
        { "mouse1", new MouseAction(1) },
        { "mouse2", new MouseAction(2) },
        { "pause", new KeyCodeAction(KeyCode.Escape) },
        { "submit", new KeyCodeAction(KeyCode.Return) },
    };

    private readonly Dictionary<string, IInputAxis> _inputAxes = new Dictionary<string, IInputAxis>()
    {
        { "mouseX", new UnityAxis("Mouse X") },
        { "mouseY", new UnityAxis("Mouse Y") },
        { "scroll", new MouseScrollAxis() },
        { "moveForward", new TwoKeysAxis(KeyCode.W, KeyCode.S) },
        { "moveRight", new TwoKeysAxis(KeyCode.D, KeyCode.A) },
        { "qe", new TwoKeysAxis(KeyCode.E, KeyCode.Q) },
    };
    
    private readonly List<InputReciver> _inputRecivers = new List<InputReciver>();

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

    private void Start()
    {
        _console.RegisterObject(this);
        _timescaleManager.OnGamePaused += OnGamePaused;

        foreach (var blueprint in _actionsBlueprints)
        {
            var action = new DynamicKeyCodeAction(blueprint.Key);
            _inputActions.Add(blueprint.Id, action);
        }

        foreach (var blueprint in _axesBlueprints)
        {
            var axis = new DynamicTwoKeysAxis(blueprint.PositiveKey, blueprint.NegativeKey);
            _inputAxes.Add(blueprint.Id, axis);
        }
    }

    // This is not needed because InputSystem is only destroyed 
    // when the application is closed.
    private void OnDestroy()
    {
        _console.DeregisterObject(this);
        _timescaleManager.OnGamePaused -= OnGamePaused;
    }

    private void Update()
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
                    OnGamePaused(); // ?
                    break;
                }
            }
        }
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
            foreach (var axis in reciver.AxisBinds.Values)
            {
                axis.Invoke(0f);
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

[Serializable]
public class InputActionBlueprint
{
    public string Id;
    public Setting_KeyCode Key;

}

[Serializable]
public class InputAxisBlueprint
{
    public string Id;
    public Setting_KeyCode PositiveKey;
    public Setting_KeyCode NegativeKey;

}