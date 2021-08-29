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
    [Inject] private UI_HelperText _helperText;

    public void Show()
    {
        var pauseWindow = _selectionWindowFactory.Create();

        pauseWindow.SetTitle("Pause");
        pauseWindow.DisableCloseButton();

        pauseWindow.AddSelection("Resume", pauseWindow.CloseThenDestroy);
        pauseWindow.AddSelection("Hats", () => Debug.Log("Hats :("));
        pauseWindow.AddSelection("Settings", OpenPauseMenu);
        pauseWindow.AddSelection("Main Menu", () => SubMenu(pauseWindow));

        //pauseWindow.AddCloseButtonCallback(_helperText.RequestHidding); Should be closing callback, but whatever

        //_helperText.RequestHidding();
    }

    private void OpenPauseMenu()
    {
        _settingsMenuFactory.Create();
    }

    private void SubMenu(UI_SelectionMenu parent)
    {
        var window = _yesNoWindowFactory.Create();

        window.SetTitle("Main Menu?");
        window.SetDescription("Progress won't be saved!");

        window.SetYesCallback(() => Debug.Log("кого"));
        window.SetNoCallback(window.CloseThenDestroy);
    }

}