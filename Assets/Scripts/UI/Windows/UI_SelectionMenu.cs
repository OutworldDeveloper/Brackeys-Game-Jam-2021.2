using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UI_SelectionMenu : UI_BaseWindow
{

    [SerializeField] private UI_SelectionMenuButton _selectionButtonPrefab = default;

    public UI_SelectionMenuButton[] CurrentButtons => _currentButtons.ToArray();

    private readonly List<UI_SelectionMenuButton> _currentButtons = new List<UI_SelectionMenuButton>();

    public void AddSelection(string name, Action action)
    {
        var button = Instantiate(_selectionButtonPrefab, transform);
        button.GetComponentInChildren<Text>().text = name;
        button.Init(name, action);
        _currentButtons.Add(button);
    }

    protected override IWindowAnimation CreateOpeningAnimation()
    {
        return new DefaultSelectionMenuOpeningAnimation(this);
    }

    public class Factory : PlaceholderFactory<UI_SelectionMenu> { }

}
