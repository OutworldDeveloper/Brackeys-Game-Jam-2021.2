using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BindingsManager : IInitializable, ITickable
{

    private readonly IConsole _console;
    private readonly ConsoleProcessor _processor;
    private readonly Dictionary<KeyCode, List<IBinding>> _bindings = new Dictionary<KeyCode, List<IBinding>>();

    public BindingsManager(IConsole console, ConsoleProcessor processor)
    {
        _console = console;
        _processor = processor;
    }

    public void Initialize()
    {
        _console.RegisterObject(this);
    }

    public void Tick()
    {
        foreach (var binding in _bindings)
        {
            var targetKey = binding.Key;
            if (Input.GetKeyDown(targetKey))
            {
                foreach (var bindin in binding.Value)
                {
                    var toExecute = bindin.GetConsoleInput();
                    _processor.TryProcess(toExecute);
                }
            }
        }
    }

    [ConsoleCommand("Binds a command to a key")]
    [NonBindable]
    public void Bind(KeyCode keyCode, ConsoleInput input)
    {
        if (input.Command.IsBindable == false)
        {
            _console.Log("The target command is not bindable", LogType.Error);
            return;
        }

        if (_bindings.ContainsKey(keyCode) == false)
        {
            _bindings.Add(keyCode, new List<IBinding>());
        }

        var newBinding = new StandartBinding(input);
        _bindings[keyCode].Add(newBinding);
    }

    [ConsoleCommand("Binds two command to a key. The commands will toggle between each other")]
    [NonBindable]
    public void Bind(KeyCode keyCode, ConsoleInput inputA, ConsoleInput inputB)
    {
        if (inputA.Command.IsBindable == false || inputB.Command.IsBindable == false)
        {
            _console.Log("The target command is not bindable", LogType.Error);
            return;
        }

        if (_bindings.ContainsKey(keyCode) == false)
        {
            _bindings.Add(keyCode, new List<IBinding>());
        }

        var newBinding = new ToggleBinding(inputA, inputB);
        _bindings[keyCode].Add(newBinding);
    }

    [ConsoleCommand("Prints all bindings associated with a key")]
    public void PrintBinds(KeyCode keyCode)
    {
        if (_bindings.TryGetValue(keyCode, out List<IBinding> bindings))
        {
            foreach (var binding in bindings)
            {
                _console.Log(binding, LogType.Message);
            }
            return;
        }
        _console.Log($"No bindings associated with the key {keyCode} were found.", LogType.Message);
    }

}

public interface IBinding
{
    ConsoleInput GetConsoleInput();

}

public class StandartBinding : IBinding
{

    private readonly ConsoleInput _consoleInput;

    public StandartBinding(ConsoleInput consoleInput)
    {
        _consoleInput = consoleInput;
    }

    public ConsoleInput GetConsoleInput()
    {
        return _consoleInput;
    }

}

public class ToggleBinding : IBinding
{

    private readonly ConsoleInput _a;
    private readonly ConsoleInput _b;
    private bool _toggle;

    public ToggleBinding(ConsoleInput a, ConsoleInput b)
    {
        _a = a;
        _b = b;
    }

    public ConsoleInput GetConsoleInput()
    {
        _toggle = !_toggle;
        return _toggle ? _a : _b;
    }

}