using UnityEngine;
using Zenject;

public class UI_WindowsInstaller : MonoInstaller
{

    [SerializeField] private UI_YesNoWindow _yesNoWindowPrefab;
    [SerializeField] private UI_SelectionMenu _selectionMenuWindowPrefab;
    [SerializeField] private UI_SettingsMenu _settingsMenuWindowPrefab;

    public override void InstallBindings()
    {
        Container.BindFactory<UI_YesNoWindow, UI_YesNoWindow.Factory>().
            FromComponentInNewPrefab(_yesNoWindowPrefab);

        Container.BindFactory<UI_SelectionMenu, UI_SelectionMenu.Factory>().
           FromComponentInNewPrefab(_selectionMenuWindowPrefab);

        Container.BindFactory<UI_SettingsMenu, UI_SettingsMenu.Factory>().
            FromComponentInNewPrefab(_settingsMenuWindowPrefab);
    }

}