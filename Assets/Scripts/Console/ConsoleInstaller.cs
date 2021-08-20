using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ConsoleInstaller : MonoInstaller
{

    [SerializeField] private Console.Settings _settings = default;
    [SerializeField] private ConsoleColors _colors = default;
    [SerializeField] private Canvas _canvas = default;
    [SerializeField] private ConsoleWindow _prefabConsoleWindow = default;
    [SerializeField] private ConsoleLogPresenter _consoleLogPresenter = default;

    public override void InstallBindings()
    {
        Container.Bind<ConsoleColors>().FromInstance(_colors).AsSingle();

        Container.Bind<Console.Settings>().FromInstance(_settings).AsSingle();

        Container.Bind<IParameterConvertor>().
            To(x => x.AllNonAbstractClasses().
            DerivingFrom<IParameterConvertor>()).
            AsCached();

        Container.BindInterfacesAndSelfTo<ConsoleRegistry>().AsSingle();
        Container.BindInterfacesAndSelfTo<Logger>().AsSingle().WithArguments(1024); // shouldn't be here
        Container.BindInterfacesAndSelfTo<CommandsContainer>().AsSingle();
        Container.BindInterfacesAndSelfTo<ParameterConvertors>().AsSingle(); // Test

        Container.Bind<ConsoleParser>().AsSingle();
        Container.Bind<ConsoleProcessor>().AsSingle();

        Container.BindInterfacesTo<BindingsManager>().AsSingle();

        Container.Bind<CommandDescriptionsGenerator>().AsSingle();

        Container.BindFactory<ConsoleWindow, ConsoleWindow.Factory>().
            FromComponentInNewPrefab(_prefabConsoleWindow).
            UnderTransform(_canvas.transform).
            AsSingle();

        Container.BindFactory<TimescaleDebugger, TimescaleDebuggerWindow, TimescaleDebuggerWindow.Factory>().
            FromComponentInNewPrefabResource(nameof(TimescaleDebuggerWindow)).
            UnderTransform(_canvas.transform);

        Container.BindFactory<ConsoleLog, ConsoleLogPresenter, ConsoleLogPresenter.Factory>().
            FromComponentInNewPrefab(_consoleLogPresenter);

        Container.Bind<ConsoleModule>().To(x => x.AllNonAbstractClasses().DerivingFrom<ConsoleModule>()).
            FromNewComponentOnNewGameObject().
            WithGameObjectName("ConsoleModule").
            AsCached().
            NonLazy();

        Container.BindInterfacesAndSelfTo<ConsoleFilesManager>().AsSingle();
    }

}

[System.Serializable]
public class ConsoleColors
{
    public Color SuggestionParameters;
    public Color SuggestionDescription;
    public Color SuccessLog;
    public Color WarningLog;
    public Color ErrorLog;
    public Color UserInput;

    public string SuggestionParametersHEX => ColorUtility.ToHtmlStringRGB(SuggestionParameters);
    public string SuggestionDescriptionHEX => ColorUtility.ToHtmlStringRGB(SuggestionDescription);
    public string SuccessLogHEX => ColorUtility.ToHtmlStringRGB(SuccessLog);
    public string WarningLogHEX => ColorUtility.ToHtmlStringRGB(WarningLog);
    public string ErrorLogHEX => ColorUtility.ToHtmlStringRGB(ErrorLog);
    public string UserInputHEX => ColorUtility.ToHtmlStringRGB(UserInput);

}