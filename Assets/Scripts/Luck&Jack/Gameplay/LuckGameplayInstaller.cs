using UnityEngine;

public class LuckGameplayInstaller : GameplaySceneInstaller<LuckGameplayBase, PlayerController>
{

    [SerializeField] private PlayerPawn _playerPawnPrefab;
    [SerializeField] private UI_LuckHud _hudPrefab;
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

        Container.BindFactory<PlayerPawn, UI_LuckHud, UI_LuckHud.Factory>().
            FromComponentInNewPrefab(_hudPrefab);

        Container.Bind<PauseMenu>().
            FromNewComponentOnNewGameObject().
            UnderTransform(transform).
            AsSingle();

        Container.Bind<Luck>().FromComponentInNewPrefab(_luckPrefab).AsSingle();
        Container.Bind<Jack>().FromComponentInNewPrefab(_jackPrefab).AsSingle();

        Container.BindFactory<Rat, Rat.Factory>().FromComponentInNewPrefab(_ratPrefab);
        Container.BindFactory<Ghost.GhostSettings, Ghost, Ghost.Factory>().FromComponentInNewPrefab(_ghostPrefab);

        //Container.BindInterfacesAndSelfTo<StatsSaver>().AsSingle();
    }

}