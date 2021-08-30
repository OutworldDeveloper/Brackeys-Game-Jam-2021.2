using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPresenter_Options : SettingPresenter<Setting_Options>
{

    [SerializeField] private Button _button;

    protected override void Present() 
    {
        var value = Setting.GetValue();
        _button.GetComponentInChildren<Text>().text = Setting.Options[value];
    }

    protected override void OnSetup()
    {
        base.OnSetup();
        _button.onClick.AddListener(Next);
    }

    private void Next()
    {
        var value = Setting.GetValue();
        if (value + 1 > Setting.Options.Length - 1)
        {
            Setting.SetValue(0);
        }
        else
        {
            Setting.SetValue(value + 1);
        }
    }

}