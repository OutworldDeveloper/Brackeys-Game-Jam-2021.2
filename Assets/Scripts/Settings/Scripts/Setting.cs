using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Setting<T> : BaseSetting
{

    [SerializeField] private T _defaultValue;
    
    protected virtual T DefaultValue => _defaultValue;

    private T _cashedValue;
    private bool _isCashed;

    public void SetValue(T value)
    {
        _isCashed = false;
        SaveValue(value);
        OnValueChanged();
        SettingsManager.SettingsChanged();
    }

    public T GetValue()
    {
        if (_isCashed)
            return _cashedValue;

        if (PlayerPrefs.HasKey(name))
        {
            _cashedValue = LoadValue();
            _isCashed = true;
            return _cashedValue;
        }

        return DefaultValue;
    }

    public override void Reset()
    {
        SetValue(DefaultValue);
    }

    protected abstract T LoadValue();
    protected abstract void SaveValue(T value);
    protected virtual void OnValueChanged() { }

}