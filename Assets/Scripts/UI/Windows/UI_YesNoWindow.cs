using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UI_YesNoWindow : UI_BaseWindow
{ 

    [SerializeField] private Text _descriptionText = default;
    [SerializeField] private Button _yesButton = default;
    [SerializeField] private Button _noButton = default;

    public UI_YesNoWindow SetDescription(string description)
    {
        _descriptionText.text = description;
        return this;
    }

    public UI_YesNoWindow SetYesCallback(Action action)
    {
        _yesButton.onClick.AddListener(action.Invoke);
        return this;
    }

    public UI_YesNoWindow SetNoCallback(Action action)
    {
        _noButton.onClick.AddListener(action.Invoke);
        return this;
    }

    public class Factory : PlaceholderFactory<UI_YesNoWindow> { }

}