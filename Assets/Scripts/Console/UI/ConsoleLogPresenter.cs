using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ConsoleLogPresenter : MonoBehaviour
{

    [SerializeField] private Text _text;
    
    [Inject]
    public void Construct(ConsoleLog log, ConsoleColors colors)
    {
        _text.text = log.Message;

        switch (log.LogType)
        {
            case LogType.Message:
                break;
            case LogType.Warning:
                _text.color = colors.WarningLog;
                break;
            case LogType.Error:
                _text.color = colors.ErrorLog;
                break;
            case LogType.Success:
                _text.color = colors.SuccessLog;
                break;
        }
    }

    public class Factory : PlaceholderFactory<ConsoleLog, ConsoleLogPresenter> { }

}