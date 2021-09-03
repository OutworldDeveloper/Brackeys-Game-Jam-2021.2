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
    [Inject] private UI_HatsWindow.Factory _hatWindowFactory;
    [Inject] private Camera _camera;

    [Inject] private UnlockablesManager _unlockablesManager;

    [Inject] private UI_SelectionMenu.Factory _selectionMenuFactory;

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
        gameObject.SetActive(false);

        var menu = _selectionMenuFactory.Create();
        menu.SetTitle("Select Level");
        menu.AddSelection("Tutorial", () =>
        {
            _sceneLoader.LoadGameplayScene(_tutorialScene);
            menu.CloseThenDestroy();
        });

        var maps = _unlockablesManager.GetUnlockablesOfType<LuckMap>(true);
        foreach (var map in maps)
        {
            menu.AddSelection(map.DisplayName, () =>
            {
                _sceneLoader.LoadGameplayScene(map.GameplayScene);
                menu.CloseThenDestroy();
            });
        }

        menu.AddClosingCallback(() => gameObject.SetActive(true));
    }

    public void OpenSettings()
    {
        gameObject.SetActive(false);

        var menu = _settingsWindowFactory.Create();

        menu.AddClosingCallback(() => gameObject.SetActive(true));
    }

    public void OpenHats()
    {
        gameObject.SetActive(false);

        var menu = _hatWindowFactory.Create();

        menu.AddClosingCallback(() => gameObject.SetActive(true));
    }

}