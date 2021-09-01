using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

// Should also install profile and all that stuff
public class LuckAndJackInstaller : MonoInstaller
{

    [Inject] private SavingSystem _savingSystem;
    [SerializeField] private string _saveName = "msave";

    public override void InstallBindings()
    {
        Container.Bind<DataContainer>().FromMethod(() =>
        {
            return _savingSystem.LoadOrCreateSave(0, _saveName);
        }).AsSingle();
    }

}