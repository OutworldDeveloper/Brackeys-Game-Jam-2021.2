using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ConsoleWindow : BaseConsoleWindow<ConsoleWindow>
{

    [SerializeField] private Text _prefab_ConsoleText;
    [SerializeField] private RectTransform _consoleTextParent;
    [SerializeField] private ScrollRect _consoleScrollRect;
    [SerializeField] protected InputField _inputField;
    [Inject] private Logger _logger;
    [Inject] private ConsoleLogPresenter.Factory _logsFactory;
    
    protected override string WindowDisplayName => "Debug Console";
    protected override string WindowId => "DebugConsole";

    protected override void Start()
    {
        base.Start();

        _logger.LogEvent += LogEvent;
        _logger.ClearedEvent += ClearedEvent;

        foreach (var log in _logger.History)
        {
            LogEvent(this, log);
        }
    }

    private void Update()
    {
        InputReciver.EatEverything = _inputField.isFocused;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _logger.LogEvent -= LogEvent;
        _logger.ClearedEvent -= ClearedEvent;
    }

    private void LogEvent(object sender, ConsoleLog log)
    {
        if (_consoleTextParent.childCount + 1 > _logger.HistoryCapacity)
        {
            var toDelete = _consoleTextParent.GetChild(0);
            Destroy(toDelete.gameObject);
        }
        var logPresenter = _logsFactory.Create(log);
        logPresenter.transform.SetParent(_consoleTextParent, false);
    }

    private void ClearedEvent()
    {
        foreach (Transform item in _consoleTextParent)
        {
            Destroy(item.gameObject);
        }
    }

    public class Factory : PlaceholderFactory<ConsoleWindow> { }

}