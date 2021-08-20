using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SceneCaptureAndRestorerInstaller : MonoInstaller
{

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<SceneStateCapture>().AsSingle();

        if (Container.HasBinding<DataContainer>() == false)
        {
            Container.Bind<DataContainer>().FromInstance(SavingSystem.CreateSaveData("test123"));
        }
    }

}