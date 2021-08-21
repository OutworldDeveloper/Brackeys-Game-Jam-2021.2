using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Float Setting", menuName = "Settings/Settings/Float")]
public class Setting_Float : Setting<float>
{

    [SerializeField] private float _minValue;
    [SerializeField] private float _maxValue;
    [SerializeField] private FloatPresentation _presentation;

    public virtual float MinValue => _minValue;
    public virtual float MaxValue => _maxValue;
    public FloatPresentation Presentation => _presentation;

    protected override float LoadValue()
    {
        return PlayerPrefs.GetFloat(name);
    }

    protected override void SaveValue(float value)
    {
        PlayerPrefs.SetFloat(name, value);
    }

}

public enum FloatPresentation
{
    Percentage,
    ValueRounded,
    Value,
    NormalizedValue,
}