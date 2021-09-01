using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

// Main Menu could be written as a window
public class MainMenu : MonoBehaviour
{

    [Inject] private CursorManager _cursorManager;
    [Inject] private SceneLoader _sceneLoader;
    [Inject] private UI_SettingsMenu.Factory _settingsWindowFactory;
    [Inject] private Camera _camera;

    [SerializeField] private GameplayScene _tutorialScene;


    private void Start()
    {
        _camera.transform.position = transform.position;
        _camera.transform.rotation = transform.rotation;

        _cursorManager.Show(this);
    }

    private void OnDestroy()
    {
        _cursorManager.Hide(this);
    }

    public void LoadTutorial()
    {
        _sceneLoader.LoadGameplayScene(_tutorialScene);
    }

    public void OpenSettings()
    {
        _settingsWindowFactory.Create();
    }

}