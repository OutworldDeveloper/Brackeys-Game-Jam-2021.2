using UnityEngine.SceneManagement;
using Zenject;

public class CoreInstaller : MonoInstaller
{

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<CursorManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<TimescaleManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<ExitingManager>().AsSingle();
        Container.Bind<InputSystem>().FromComponentInHierarchy().AsSingle();
        Container.Bind<SceneLoader>().FromComponentInHierarchy().AsSingle();
    }

}