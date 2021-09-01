using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class CoreInstaller : MonoInstaller
{

    [SerializeField] private GameplayScene _initialScene;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<CursorManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<TimescaleManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<ExitingManager>().AsSingle();
        Container.Bind<InputSystem>().FromComponentInHierarchy().AsSingle();
        Container.Bind<SceneLoader>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IInitializable>().To<Bootstrap>().AsSingle().WithArguments(_initialScene);
    }

}

// Глупо быть и в ProjectContext, потому что нельзя выгружать все сцены,
// глупо и в контексте ниже, так как это вроде как во всех проектах полезно...
public class Bootstrap : IInitializable
{

    private readonly SceneLoader _sceneLoader;
    private readonly GameplayScene _gameplayScene;

    public Bootstrap(SceneLoader sceneLoader, GameplayScene gameplayScene)
    {
        _sceneLoader = sceneLoader;
        _gameplayScene = gameplayScene;
    }

    public void Initialize()
    {
        _sceneLoader.LoadGameplayScene(_gameplayScene);
    }

}