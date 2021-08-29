using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UI_SettingsMenu : UI_BaseWindow
{

    protected override Selectable InitialSelection => CloseButton;

    protected override IWindowAnimation CreateOpeningAnimation()
    {
        return new LuckOpeningAnimation<UI_SettingsMenu>(this);
    }

    public class Factory : PlaceholderFactory<UI_SettingsMenu> { }

}