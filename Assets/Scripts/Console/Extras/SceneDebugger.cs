using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SceneDebugger : ConsoleModule // TODO: ConsoleModuleBehaviour
{

    [Inject] private IConsole _console;

    private void Start()
    {
        _console.RegisterObject(this);
    }

    private void OnDestroy()
    {
        _console.DeregisterObject(this);
    }

    [ConsoleCommand("Debug scene info")]
    public void PrintSceneInfo()
    {
        var roots = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (var gameObject in roots)
        {
            PrintGameObject(gameObject, 0);
        }
    }

    private void PrintGameObject(GameObject gameObject, int depth)
    {
        string prefix = new string('-', depth + 1);
        _console.Log(prefix + gameObject.name);
        foreach (Transform child in gameObject.transform)
        {
            PrintGameObject(child.gameObject, depth + 1);
        }
    }

}

public abstract class ConsoleModule : MonoBehaviour
{

}