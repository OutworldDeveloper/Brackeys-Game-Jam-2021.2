using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckGameplayInstaller : GameplaySceneInstaller<LuckGameplayBase, PlayerController>
{

    [SerializeField] private Luck _luckPrefab;
    [SerializeField] private Jack _jackPrefab;
    [SerializeField] private Rat _ratPrefab;

    public override void InstallBindings()
    {
        base.InstallBindings();

        Container.BindInterfacesAndSelfTo<PlayerPawn>().
            FromNewComponentOnNewGameObject().
            UnderTransform(transform).
            AsSingle();

        Container.Bind<Luck>().FromComponentInNewPrefab(_luckPrefab).AsSingle();
        Container.Bind<Jack>().FromComponentInNewPrefab(_jackPrefab).AsSingle();

        Container.BindFactory<Rat, Rat.Factory>().FromComponentInNewPrefab(_ratPrefab);
    }

}