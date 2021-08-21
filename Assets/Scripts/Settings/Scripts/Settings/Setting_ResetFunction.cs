using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Reset Function", menuName = "Settings/Settings/Reset Function")]
public class Setting_ResetFunction : Setting_Function
{

    public override void Execute()
    {
        SettingsManager.ResetSettings();
    }

}