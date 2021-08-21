using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SettingsEventBool : SettingsEventBase
{

    [SerializeField] private BoolEvent settingsUpdated;
    [SerializeField] private Setting_Bool targetParameter;
    [SerializeField] private bool invert;

    protected override void SettingsUpdated()
    {
        bool value = invert ? !targetParameter.GetValue() : targetParameter.GetValue();
        settingsUpdated.Invoke(value);
    }

}

[System.Serializable]
public class BoolEvent : UnityEvent<bool> { }