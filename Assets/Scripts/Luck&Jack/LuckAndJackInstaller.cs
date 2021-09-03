using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

// Should also install profile and all that stuff
public class LuckAndJackInstaller : MonoInstaller
{

    [Inject] private SavingSystem _savingSystem;
    [SerializeField] private string _saveName = "msave";
    [SerializeField] private UI_HatsWindow _hatsWindowPrefab;

    public override void InstallBindings()
    {
        Container.Bind<DataContainer>().
            FromMethod(() => _savingSystem.LoadOrCreateSave(0, _saveName)).
            AsSingle().
            WhenInjectedInto(typeof(JackCustomizaton), typeof(UnlockablesManager), typeof(RecordsManager));

        Container.BindInterfacesAndSelfTo<JackCustomizaton>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<UnlockablesManager>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<RecordsManager>().FromComponentInHierarchy().AsSingle().NonLazy();

        Container.BindFactory<UI_HatsWindow, UI_HatsWindow.Factory>().
            FromComponentInNewPrefab(_hatsWindowPrefab);
    }

}