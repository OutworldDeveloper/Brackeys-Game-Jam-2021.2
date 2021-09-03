using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PauseMenu : MonoBehaviour
{

    [Inject] private UI_YesNoWindow.Factory _yesNoWindowFactory;
    [Inject] private UI_SelectionMenu.Factory _selectionWindowFactory;
    [Inject] private UI_SettingsMenu.Factory _settingsMenuFactory;
    [Inject] private UI_HatsWindow.Factory _hatsWindowFactory;
    [Inject] private SceneLoader _sceneLoader;

    [SerializeField] private GameplayScene _menuScene;

    public void Show()
    {
        var pauseWindow = _selectionWindowFactory.Create();

        pauseWindow.SetTitle("Pause");
        pauseWindow.DisableCloseButton();

        pauseWindow.AddSelection("Resume", pauseWindow.CloseThenDestroy);
        pauseWindow.AddSelection("Hats", OpenHatsWindoow);
        pauseWindow.AddSelection("Settings", OpenSettingsMenu);
        pauseWindow.AddSelection("Main Menu", () => SubMenu(pauseWindow));
    }

    private void OpenHatsWindoow()
    {
        _hatsWindowFactory.Create();
    }

    private void OpenSettingsMenu()
    {
        _settingsMenuFactory.Create();
    }

    private void SubMenu(UI_SelectionMenu parent)
    {
        var window = _yesNoWindowFactory.Create();

        window.SetTitle("Main Menu?");
        window.SetDescription("Progress won't be saved!");
        window.SetYesCallback(() =>
        {
            parent.CloseThenDestroy();
            _sceneLoader.LoadGameplayScene(_menuScene);
        });
    }

}