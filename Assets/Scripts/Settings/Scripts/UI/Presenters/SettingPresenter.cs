using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class SettingPresenter<T> : BaseSettingPresenter where T : BaseSetting
{

    [SerializeField] private Text _titleText;

    public override Type TargetType => typeof(T);

    protected T Setting;

    public override void Setup(BaseSetting setting)
    {
        Setting = setting as T;
        _titleText.text = GetDisplayName();
        OnSetup();
        Present();
    }

    protected virtual string GetDisplayName()
    {
        return Setting.DisplayName;
    }

    protected void UpdateName()
    {
        _titleText.text = GetDisplayName();
    }

    protected virtual void OnSetup()
    {
        SettingsManager.OnSettingsChanged += OnSettingsChanged;
    }

    protected virtual void OnDestroy()
    {
        SettingsManager.OnSettingsChanged -= OnSettingsChanged;
    }

    private void OnSettingsChanged()
    {
        Present();
    }

    protected abstract void Present();

}