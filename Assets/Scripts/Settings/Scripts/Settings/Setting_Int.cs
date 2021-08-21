using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting_Int : Setting<int>
{

    [SerializeField] private int minValue;
    [SerializeField] private int maxValue;

    public virtual float MinValue => minValue;
    public virtual float MaxValue => maxValue;

    protected override int LoadValue()
    {
        return PlayerPrefs.GetInt(name);
    }

    protected override void SaveValue(int value)
    {
        PlayerPrefs.SetInt(name, value);
    }

}