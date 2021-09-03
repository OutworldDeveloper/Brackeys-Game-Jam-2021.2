using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HatsWindowInstaller : MonoInstaller
{

    [SerializeField] private UI_HatButton _hatButtonPrefab;

    public override void InstallBindings()
    {
        Container.BindFactory<Hat, bool, UI_HatButton, UI_HatButton.Factory>().
            FromComponentInNewPrefab(_hatButtonPrefab).
            AsSingle();
    }

}