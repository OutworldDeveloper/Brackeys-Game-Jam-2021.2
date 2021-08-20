using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zenject;

[ConsolePrefix]
public class ConsoleRegistry : IInitializable
{

    private readonly IConsole _console;
    private readonly HashSet<object> _registeredObjects = new HashSet<object>();

    public ConsoleRegistry(IConsole console)
    {
        _console = console;
    }

    public void Initialize()
    {
        RegisterObject(this);
    }

    public void RegisterObject(object target)
    {
        _registeredObjects.Add(target);
    }

    public void DeregisterObject(object target)
    {
        _registeredObjects.Remove(target);
    }

    public object[] GetRegisteredObjectsOfType(Type type)
    {
        var result = new List<object>();

        foreach (var item in _registeredObjects)
        {
            if (item.GetType() == type)
            {
                result.Add(item);
                continue;
            }

            if (item.GetType().IsSubclassOf(type))
            {
                result.Add(item);
            }
        }

        return result.ToArray();
    }

    [ConsoleCommand("Prints a list of currently registered objects")]
    public void Print()
    {
        foreach (var item in _registeredObjects)
        {
            _console.Log(item);
        }
    }

}