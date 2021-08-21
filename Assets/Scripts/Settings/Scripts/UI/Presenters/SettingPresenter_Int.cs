using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPresenter_Int : SettingPresenter<Setting_Int>
{

    [SerializeField] private Slider _slider;

    protected override void Present()
    {
        _slider.value = Setting.GetValue();
    }

    protected override void OnSetup()
    {
        base.OnSetup();
        _slider.minValue = Setting.MinValue;
        _slider.maxValue = Setting.MaxValue;
        _slider.onValueChanged.AddListener(OnValueChanged);
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
        _slider.onValueChanged.RemoveListener(OnValueChanged);
    }

    public void OnValueChanged(float value)
    {
        Setting.SetValue((int)value);
        UpdateName();
    }

    protected override string GetDisplayName()
    {
        return $"{Setting.DisplayName} {Setting.GetValue()}";
    }

}