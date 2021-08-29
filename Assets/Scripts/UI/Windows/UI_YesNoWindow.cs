using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UI_YesNoWindow : UI_BaseWindow<UI_YesNoWindow>
{ 

    [SerializeField] private Button _yesButton = default;
    [SerializeField] private Button _noButton = default;

    protected override Selectable InitialSelection => _noButton;

    public void SetYesCallback(Action action)
    {
        _yesButton.onClick.AddListener(action.Invoke);
    }

    public void SetNoCallback(Action action)
    {
        _noButton.onClick.AddListener(action.Invoke);
    }

    public class Factory : PlaceholderFactory<UI_YesNoWindow> { }

}