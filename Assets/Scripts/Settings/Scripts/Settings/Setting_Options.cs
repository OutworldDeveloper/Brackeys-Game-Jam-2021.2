using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Options Variable", menuName = "Settings/Settings/Options")]
public class Setting_Options : Setting<int>
{

    [SerializeField] private Options _options;

    public Options Options => _options;

    protected override int LoadValue()
    {
        return PlayerPrefs.GetInt(name);
    }

    protected override void SaveValue(int value)
    {
        PlayerPrefs.SetInt(name, value);
    }

}

[System.Serializable]
public struct Options
{

    public Option[] options;

    [System.Serializable]
    public struct Option
    {
        public string displayName;
    }

}