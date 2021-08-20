using System.Collections;
using UnityEngine;
using Zenject;

public class ConsoleProcessor 
{

    private readonly Logger _logger;
    private readonly ConsoleRegistry _regestry;

    public ConsoleProcessor(Logger logger, ConsoleRegistry regestry)
    {
        _logger = logger;
        _regestry = regestry;
    }

    // What about the return object when there's more than one target?
    public bool TryProcess(ConsoleInput input) 
    {
        var targetType = input.Command.TargetType; 
        var targets = _regestry.GetRegisteredObjectsOfType(targetType);

        if (targets.Length == 0)
        {
            _logger.Log($"No objects of type {targetType} found. Command won't be executed.", LogType.Error);
            return false;
        }

        foreach (var target in targets)
        {
            object result = input.Command.Execute(target, input.Parameters);
            if (result != null)
            {
                _logger.Log(result, LogType.Message);
            }
        }

        return true;
    }

}