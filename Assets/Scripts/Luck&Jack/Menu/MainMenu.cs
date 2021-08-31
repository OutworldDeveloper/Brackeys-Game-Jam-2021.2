using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

// Main Menu could be written as a window
public class MainMenu : MonoBehaviour
{

    [Inject] private CursorManager _cursorManager;
    [Inject] private SceneLoader _sceneLoader;

    private void Start()
    {
        _cursorManager.Show(this);
    }

    private void OnDestroy()
    {
        _cursorManager.Hide(this);
    }

    public void LoadTutorial()
    {
        _sceneLoader.LoadScene(1);
    }

}