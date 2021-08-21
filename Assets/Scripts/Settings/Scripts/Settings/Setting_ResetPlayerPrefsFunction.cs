using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ResetPlayerPrefs Function", menuName = "Settings/Settings/Reset PlayerPrefs Function")]
public class Setting_ResetPlayerPrefsFunction : Setting_Function
{

    public override void Execute()
    {
        PlayerPrefs.DeleteAll();
    }

}