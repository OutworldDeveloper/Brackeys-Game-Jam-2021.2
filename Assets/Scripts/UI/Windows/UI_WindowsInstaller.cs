using UnityEngine;
using Zenject;

public class UI_WindowsInstaller : MonoInstaller
{

    [SerializeField] private Canvas _windowsCanvas = default;
    [SerializeField] private UI_YesNoWindow _prefabYesNoWindow = default;
    [SerializeField] private UI_SelectionMenu _prefabSelectionMenu = default;

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
    }

}