using UnityEngine;
using Zenject;

public class SavingSystemInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IDataSerializer>().To<JsonDataSerializer>().AsSingle();
        Container.Bind<SavingSystem>().AsSingle();
    }
}