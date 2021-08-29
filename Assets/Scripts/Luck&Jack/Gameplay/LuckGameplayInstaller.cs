using UnityEngine;

public class LuckGameplayInstaller : GameplaySceneInstaller<LuckGameplayBase, PlayerController>
{

    [SerializeField] private PlayerPawn _playerPawnPrefab;
    [SerializeField] private Luck _luckPrefab;
    [SerializeField] private Jack _jackPrefab;
    [SerializeField] private Rat _ratPrefab;
    [SerializeField] private Ghost _ghostPrefab;

    public override void InstallBindings()
    {
        base.InstallBindings();

        Container.BindInterfacesAndSelfTo<PlayerPawn>().
            FromComponentInNewPrefab(_playerPawnPrefab).
            UnderTransform(transform).
            AsSingle();

        Container.Bind<PauseMenu>().
            FromNewComponentOnNewGameObject().
            UnderTransform(transform).
            AsSingle();

        Container.Bind<Luck>().FromComponentInNewPrefab(_luckPrefab).AsSingle();
        Container.Bind<Jack>().FromComponentInNewPrefab(_jackPrefab).AsSingle();

        Container.BindFactory<Rat, Rat.Factory>().FromComponentInNewPrefab(_ratPrefab);
        Container.BindFactory<Ghost.GhostSettings, Ghost, Ghost.Factory>().FromComponentInNewPrefab(_ghostPrefab);
    }

}