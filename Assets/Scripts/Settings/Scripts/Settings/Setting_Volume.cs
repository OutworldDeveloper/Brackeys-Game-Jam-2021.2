using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "New Volume Setting", menuName = "Settings/Settings/Volume")]
public class Setting_Volume : Setting_Float
{

    [SerializeField] private AudioMixer _targetMixer;
    [SerializeField] private string _targetParameter;

    public override float MinValue => 0.001f;
    public override float MaxValue => 1f;
    protected override float DefaultValue => 1f;

    public override void OnGameStarted()
    {
        ApplyParameter();
    }

    protected override void OnValueChanged()
    {
        ApplyParameter();
    }

    private void ApplyParameter()
    {
        _targetMixer.SetFloat(_targetParameter, Mathf.Log10(GetValue()) * 20);
    }

}