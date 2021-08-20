using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class ScenesDebugger : ConsoleModule
{

    [Inject] private IConsole _console;

    private void Start()
    {
        _console.RegisterObject(this);
    }

    [ConsoleCommand("Forces game to load a scene")]
    public void ForceScene(int index)
    {
        SceneManager.LoadScene(index);
    }

}