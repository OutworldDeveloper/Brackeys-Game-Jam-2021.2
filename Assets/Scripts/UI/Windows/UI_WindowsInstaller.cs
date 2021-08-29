using UnityEngine;
using Zenject;

public class UI_WindowsInstaller : MonoInstaller
{

    [SerializeField] private Canvas _windowsCanvas;
    [SerializeField] private UI_YesNoWindow _yesNoWindowPrefab;
    [SerializeField] private UI_SelectionMenu _selectionMenuWindowPrefab;
    [SerializeField] private UI_SettingsMenu _settingsMenuWindowPrefab;

    public override void InstallBindings()
    {
        Container.BindFactory<UI_YesNoWindow, UI_YesNoWindow.Factory>().
            FromComponentInNewPrefab(_yesNoWindowPrefab).
            UnderTransform(_windowsCanvas.transform);

        Container.BindFactory<UI_SelectionMenu, UI_SelectionMenu.Factory>().
           FromComponentInNewPrefab(_selectionMenuWindowPrefab).
           UnderTransform(_windowsCanvas.transform);

        Container.BindFactory<UI_SettingsMenu, UI_SettingsMenu.Factory>().
            FromComponentInNewPrefab(_settingsMenuWindowPrefab).
            UnderTransform(_windowsCanvas.transform);
    }

}