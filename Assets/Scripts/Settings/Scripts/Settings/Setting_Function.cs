using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Setting_Function : BaseSetting
{

    [SerializeField] private string buttonText;

    public string ButtonText => buttonText;

    public abstract void Execute();

}