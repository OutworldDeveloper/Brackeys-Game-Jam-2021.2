using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SettingsEventInt : SettingsEventBase
{

    [SerializeField] private IntEvent settingsUpdated;
    [SerializeField] private Setting_Int targetParameter;

    protected override void SettingsUpdated()
    {
        settingsUpdated.Invoke(targetParameter.GetValue());
    }

}

[System.Serializable]
public class IntEvent : UnityEvent<int> { }