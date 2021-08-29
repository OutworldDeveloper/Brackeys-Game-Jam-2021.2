using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UI_SelectionMenu : UI_BaseWindow<UI_SelectionMenu>
{

    [SerializeField] private UI_SelectionMenuButton _selectionButtonPrefab;
    [SerializeField] private RectTransform _selectionsParent;

    public UI_SelectionMenuButton[] CurrentButtons => _currentButtons.ToArray();
    protected override Selectable InitialSelection => _currentButtons[0]?.Button;

    private readonly List<UI_SelectionMenuButton> _currentButtons = new List<UI_SelectionMenuButton>();

    public void AddSelection(string name, Action action)
    {
        var button = GameObject.Instantiate(_selectionButtonPrefab, _selectionsParent);
        button.GetComponentInChildren<Text>().text = name;
        button.Init(name, action);
        _currentButtons.Add(button);
    }

    public class Factory : PlaceholderFactory<UI_SelectionMenu> { }

}
