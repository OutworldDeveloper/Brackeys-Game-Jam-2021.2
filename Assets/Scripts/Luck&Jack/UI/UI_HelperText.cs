using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UI_HelperText : MonoBehaviour
{

    [SerializeField] private Text _text;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeDuration = 1f;
    [Inject] private IConsole _console;

    private float _endTime;

    private void Start()
    {
        _console.RegisterObject(this);
        _canvasGroup.alpha = 0f;
    }

    private void OnDestroy()
    {
        _console.DeregisterObject(this);
    }

    private void Update()
    {
        if (Time.time < _endTime)
        {
            if (_canvasGroup.alpha < 1f)
                _canvasGroup.alpha += _fadeDuration * Time.unscaledDeltaTime;
        }
        else
        {
            if (_canvasGroup.alpha > 0f)
                _canvasGroup.alpha -= _fadeDuration * Time.unscaledDeltaTime;
        }
    }

    [ConsoleCommand("Shows a message on screen")]
    public void Show(string text, float duration)
    {
        _text.text = text;
        _endTime = Time.time + duration;
    }

}