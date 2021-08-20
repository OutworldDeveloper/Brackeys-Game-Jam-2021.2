using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UI_YesNoWindow : UI_BaseWindow // TODO: Base class or even better a component so we don't have to set settings on each panel
{                                              // There is no settings though! (It does (close button))

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
        _yesButton.onClick.AddListener(() => action.Invoke());
        return this;
    }

    public UI_YesNoWindow SetNoCallback(Action action)
    {
        _noButton.onClick.AddListener(() => action.Invoke());
        return this;
    }

    public class Factory : PlaceholderFactory<UI_YesNoWindow> { }

}