using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ExitingManager : IInitializable
{

    private readonly UI_YesNoWindow.Factory _dialogueWindowFactory;
    private bool _isActuallyQuitting;
    private UI_YesNoWindow _currentWindow;

    public ExitingManager(UI_YesNoWindow.Factory dialogueWindowFactory)
    {
        _dialogueWindowFactory = dialogueWindowFactory;
    }

    public void Initialize()
    {
        Application.wantsToQuit += WantsToQuit;
    }

    private bool WantsToQuit()
    {
        if (_currentWindow)
        {
            return true;
        }

        _currentWindow = _dialogueWindowFactory.Create();
        _currentWindow.SetTitle("Are you sure?");
        _currentWindow.SetDescription("Any not saved data will be lost.");

        _currentWindow.SetYesCallback(() => 
        { 
            _isActuallyQuitting = true;
            Application.Quit();
        });

        _currentWindow.SetNoCallback(() =>
        {
            _currentWindow.CloseThenDestroy();
            _currentWindow = null;
        });

        _currentWindow.AddCloseButtonCallback(() =>
        {
            _currentWindow.CloseThenDestroy();
            _currentWindow = null;
        });

        return _isActuallyQuitting;
    }

}