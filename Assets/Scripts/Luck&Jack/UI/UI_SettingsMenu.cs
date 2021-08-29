using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UI_SettingsMenu : UI_BaseWindow<UI_SettingsMenu>
{
    protected override Selectable InitialSelection => CloseButton;

    public class Factory : PlaceholderFactory<UI_SettingsMenu> { }

}