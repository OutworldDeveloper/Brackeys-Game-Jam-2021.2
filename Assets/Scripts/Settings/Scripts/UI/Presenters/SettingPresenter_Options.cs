using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPresenter_Options : SettingPresenter<Setting_Options>
{

    [SerializeField] private Dropdown _dropdown;

    protected override void Present()
    {

    }

    protected override void OnSetup()
    {
        base.OnSetup();
        _dropdown.ClearOptions();
        foreach (var item in Setting.Options.options)
        {
            Dropdown.OptionData optionData = new Dropdown.OptionData();
            optionData.text = item.displayName;
            _dropdown.options.Add(optionData);
        }
        _dropdown.value = Setting.GetValue();
        _dropdown.onValueChanged.AddListener(Setting.SetValue);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _dropdown.onValueChanged.RemoveListener(Setting.SetValue);
    }

}