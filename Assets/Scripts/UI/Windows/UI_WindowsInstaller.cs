using UnityEngine;
using Zenject;

public class UI_WindowsInstaller : MonoInstaller
{

    [SerializeField] private Canvas _windowsCanvas = default;
    [SerializeField] private UI_YesNoWindow _prefabYesNoWindow = default;
    [SerializeField] private UI_SelectionMenu _prefabSelectionMenu = default;
    [SerializeField] private UI_LuckSettingsMenu _settingsMenu;

    public override void InstallBindings()
    {
        Container.BindFactory<UI_YesNoWindow, UI_YesNoWindow.Factory>().
            FromComponentInNewPrefab(_prefabYesNoWindow.gameObject).
            UnderTransform(_windowsCanvas.transform).
            AsSingle();

        Container.BindFactory<UI_SelectionMenu, UI_SelectionMenu.Factory>().
           FromComponentInNewPrefab(_prefabSelectionMenu.gameObject).
           UnderTransform(_windowsCanvas.transform).
           AsSingle();

        Container.BindFactory<UI_LuckSettingsMenu, UI_LuckSettingsMenu.Factory>().
            FromComponentInNewPrefab(_settingsMenu).
            UnderTransform(_windowsCanvas.transform).
            AsSingle();
    }

}