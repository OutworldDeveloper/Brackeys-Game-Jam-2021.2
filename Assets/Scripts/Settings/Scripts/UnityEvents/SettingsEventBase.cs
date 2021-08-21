using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class SettingsEventBase : MonoBehaviour
{

    private void OnEnable()
    {
        SettingsManager.OnSettingsChanged += SettingsUpdated;
    }

    private void OnDisable()
    {
        SettingsManager.OnSettingsChanged -= SettingsUpdated;
    }

    private void Awake()
    {
        SettingsUpdated();
    }

    protected abstract void SettingsUpdated();

}