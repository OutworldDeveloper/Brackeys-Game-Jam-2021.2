using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AutoexecManager : ConsoleModule
{

    [Inject] private ConsoleFilesManager _filesManager;
    [Inject] private IConsole _console;

    private void Start()
    {
        _console.Log($"[AutoexecManager] Found {_filesManager.Autoexec.Length} lines. Executing");
        foreach (var line in _filesManager.Autoexec)
        {
            if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                continue;
            if (line.StartsWith("#"))
                continue;
            _console.Submit(line);
        }
    }

}