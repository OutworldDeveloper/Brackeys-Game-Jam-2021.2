using System.Collections;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Options Variable", menuName = "Settings/Settings/Options")]
public class Setting_Options : Setting<int>
{

    [SerializeField] private string[] _options;
    public string[] Options => _options;

    protected override int LoadValue()
    {
        return PlayerPrefs.GetInt(name);
    }

    protected override void SaveValue(int value)
    {
        PlayerPrefs.SetInt(name, value);
    }

}