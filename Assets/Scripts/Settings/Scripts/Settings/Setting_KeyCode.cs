using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New KeyCode Setting", menuName = "Settings/Settings/KeyCode")]
public class Setting_KeyCode : Setting_Enum<KeyCode> 
{

    public bool IsDown()
    {
        return Input.GetKeyDown(GetValue());
    }

    public bool IsUp()
    {
        return Input.GetKeyUp(GetValue());
    }

    public bool IsBeingHolded()
    {
        return Input.GetKey(GetValue());
    }

}