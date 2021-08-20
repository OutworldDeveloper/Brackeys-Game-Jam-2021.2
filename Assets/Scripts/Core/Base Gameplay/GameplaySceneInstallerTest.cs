using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class GameplaySceneInstallerTest<TGameplayController, TPlayerController> : MonoInstaller 
    where TGameplayController : GameplayController
    where TPlayerController : PlayerController
{

    [SerializeField] private Camera _mainCamera = default;

    public override void InstallBindings()
    {
        // Camera

        Container.Bind<Camera>().
            FromInstance(_mainCamera).
            AsSingle().
            WhenInjectedInto<TPlayerController>();

        // Player Controller

        var playerControllerType = typeof(TPlayerController);
        var playerControllerToBind = new List<Type>();

        foreach (var item in playerControllerType.GetInterfaces())
        {
            playerControllerToBind.Add(item);
        }

        playerControllerToBind.Add(playerControllerType);
        playerControllerToBind.Add(typeof(PlayerController));

        Container.Bind(playerControllerToBind).To<TPlayerController>().AsSingle();

        // GameplayController

        var gameplayControllerType = typeof(TGameplayController);
        var gameplayControllerToBind = new List<Type>();

        foreach (var item in gameplayControllerType.GetInterfaces())
        {
            gameplayControllerToBind.Add(item);
        }

        gameplayControllerToBind.Add(gameplayControllerType);
        gameplayControllerToBind.Add(typeof(GameplayController));

        Container.Bind(gameplayControllerToBind).To<TGameplayController>().AsSingle();

        // FreeCamera

        Container.Bind<FreeCamera>().
            FromNewComponentOnNewGameObject().
            UnderTransform(transform).
            AsSingle();
    }

}