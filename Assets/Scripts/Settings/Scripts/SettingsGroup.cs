using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsGroup
{

    [SerializeField] private string _displayName = "Name";
    [SerializeField] private List<BaseSetting> _settings = new List<BaseSetting>();
    public string DisplayName => _displayName;
    public List<BaseSetting> Settings => _settings;

}