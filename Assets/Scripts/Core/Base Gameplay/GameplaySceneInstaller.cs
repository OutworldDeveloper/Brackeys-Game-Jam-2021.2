using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class GameplaySceneInstaller<TGameplayController, TPlayerController> : MonoInstaller 
    where TGameplayController : GameplayController
    where TPlayerController : PlayerController
{

    [SerializeField] private Camera _mainCamera = default;

    public override void InstallBindings()
    {
        Container.Bind<Camera>().
            FromInstance(_mainCamera).
            AsSingle().
            WhenInjectedInto<TPlayerController>();

        Container.Bind<FreeCamera>().
            FromNewComponentOnNewGameObject().
            UnderTransform(transform).
            AsSingle();

        BindGameplayController();
        BindPlayerController();
    }

    private void BindGameplayController()
    {
        var type = typeof(TGameplayController);
        var typesToBind = new List<Type>();

        foreach (var item in type.GetInterfaces())
        {
            typesToBind.Add(item);
        }

        typesToBind.Add(type);

        if (type != typeof(GameplayController))
        {
            typesToBind.Add(typeof(GameplayController));
        }

        Container.Bind(typesToBind).To<TGameplayController>().AsSingle();
    }

    private void BindPlayerController()
    {
        var type = typeof(TPlayerController);
        var typesToBind = new List<Type>();

        foreach (var item in type.GetInterfaces())
        {
            typesToBind.Add(item);
        }

        typesToBind.Add(type);

        if (type != typeof(PlayerController))
        {
            typesToBind.Add(typeof(PlayerController));
        }

        Container.Bind(typesToBind).To<TPlayerController>().AsSingle();
    }

}