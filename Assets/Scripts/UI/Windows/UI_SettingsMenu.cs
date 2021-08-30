using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UI_SettingsMenu : UI_BaseWindow<UI_SettingsMenu>
{

    [SerializeField] private SettingsGroupPresenter _groupPresenterPrefab;
    [SerializeField] private BaseSettingPresenter[] _presentersPrefabs;
    [SerializeField] private Transform _parent;

    protected override Selectable InitialSelection => CloseButton;

    protected override void OnOpened()
    {
        var helper = new UISettingsHelper(_groupPresenterPrefab, _presentersPrefabs);
        helper.Populate(_parent);
    }

    public class Factory : PlaceholderFactory<UI_SettingsMenu> { }

}