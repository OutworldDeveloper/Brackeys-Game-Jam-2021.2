using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SettingsEventFloat : SettingsEventBase
{

    [SerializeField] private FloatEvent settingsUpdated;
    [SerializeField] private Setting_Float targetParameter;

    protected override void SettingsUpdated()
    {
        settingsUpdated.Invoke(targetParameter.GetValue());
    }

}

[System.Serializable]
public class FloatEvent : UnityEvent<float> { }