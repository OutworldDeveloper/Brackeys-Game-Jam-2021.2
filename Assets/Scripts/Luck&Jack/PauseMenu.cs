using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PauseMenu : MonoBehaviour
{

    [Inject] private UI_YesNoWindow.Factory _yesNoWindowFactory;
    [Inject] private UI_SelectionMenu.Factory _selectionWindowFactory;

    public void Show()
    {
        var pauseWindow = _selectionWindowFactory.Create();

        pauseWindow.SetTitle("Pause");

        pauseWindow.AddSelection("Resume", pauseWindow.CloseThenDestroy);
        pauseWindow.AddSelection("Settings", () => Debug.Log("Goga :("));
        pauseWindow.AddSelection("Main Menu", () => SubMenu(pauseWindow));
    }

    private void SubMenu(UI_SelectionMenu parent)
    {
        var window = _yesNoWindowFactory.Create();

        window.SetTitle("You sure?");
        window.SetDescription("Нажми нижнию кнопку что бы удалить");

        window.SetYesCallback(() => Debug.Log("кого"));
        window.SetNoCallback(() =>
        {
            parent.CloseThenDestroy();
        });
        parent.AddChildWindow(window);
    }

}