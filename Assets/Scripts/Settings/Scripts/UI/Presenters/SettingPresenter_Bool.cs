using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SettingPresenter_Bool : SettingPresenter<Setting_Bool>
{

    [SerializeField] private Button _switchButton;

    protected override void Present()
    {
        _switchButton.GetComponentInChildren<Text>().text = Setting.GetValue() ? "enabled" : "disabled";
    }

    protected override void OnSetup()
    {
        base.OnSetup();
        _switchButton.onClick.AddListener(OnButtonPressed);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _switchButton.onClick.RemoveListener(OnButtonPressed);
    }

    public void OnButtonPressed()
    {
        Setting.SetValue(!Setting.GetValue());
        _switchButton.GetComponentInChildren<Text>().text = Setting.GetValue() ? "enabled" : "disabled";
    }

}