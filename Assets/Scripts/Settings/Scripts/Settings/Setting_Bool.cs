using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bool Setting", menuName = "Settings/Settings/Bool")]
public class Setting_Bool : Setting<bool>
{

    protected override bool LoadValue()
    {
        return PlayerPrefs.GetInt(name) != 0;
    }

    protected override void SaveValue(bool value)
    {
        PlayerPrefs.SetInt(name, (value ? 1 : 0));
    }

}