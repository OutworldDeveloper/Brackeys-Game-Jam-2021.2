using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class GameplaySceneInstaller<TGameplayController, TPlayerController> : MonoInstaller
    where TGameplayController : GameplayController
    where TPlayerController : PlayerController
{

    [SerializeField] private Camera _mainCamera;
    [SerializeField] private TGameplayController _gameplayControllerPrefab;

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
        var typesToBind = new List<Type>();

        var desiredType = typeof(TGameplayController);
        var actuallType = _gameplayControllerPrefab.GetType();

        typesToBind.Add(desiredType);

        if (actuallType != desiredType)
        {
            typesToBind.Add(actuallType);
        }     

        if (desiredType != typeof(GameplayController))
        {
            typesToBind.Add(typeof(GameplayController));
        }

        Container.Bind(typesToBind).FromComponentInNewPrefab(_gameplayControllerPrefab).UnderTransform(transform).AsSingle().NonLazy();
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

        Container.Bind(typesToBind).To<TPlayerController>().AsSingle().NonLazy();
    }

}