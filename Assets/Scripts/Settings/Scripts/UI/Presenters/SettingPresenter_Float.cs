using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPresenter_Float : SettingPresenter<Setting_Float>
{

    private static readonly IFloatPresenter[] FloatPresenters = new IFloatPresenter[]
    {
        new PercentagePresenter(),
        new ValuePresenter(),
        new ValueRoundedPresenter(),
    };

    [SerializeField] private Slider _slider;

    protected override void Present()
    {
        var value = Setting.GetValue();
        _slider.SetValueWithoutNotify(value);
    }

    protected override void OnSetup()
    {
        base.OnSetup();
        _slider.minValue = Setting.MinValue;
        _slider.maxValue = Setting.MaxValue;
        _slider.onValueChanged.AddListener(OnValueChanged);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _slider.onValueChanged.RemoveListener(OnValueChanged);
    }

    public void OnValueChanged(float value)
    {
        Setting.SetValue(value);
        UpdateName();
    }

    protected override string GetDisplayName()
    {
        foreach (var presenter in FloatPresenters)
        {
            if (presenter.TargetPresentation == Setting.Presentation)
                return presenter.Present(Setting);
        }
        return base.GetDisplayName();
    }

}

public interface IFloatPresenter
{
    FloatPresentation TargetPresentation { get; }
    string Present(Setting_Float setting);

}

public class PercentagePresenter : IFloatPresenter
{
    public FloatPresentation TargetPresentation => FloatPresentation.Percentage;

    public string Present(Setting_Float setting)
    {
        float difference = setting.MaxValue - setting.MinValue;
        float percentage = (setting.GetValue() - setting.MinValue) / difference * 100f;

        return $"{setting.DisplayName} {percentage.ToString("0")}%";
    }

}

public class ValuePresenter : IFloatPresenter
{
    public FloatPresentation TargetPresentation => FloatPresentation.Value;

    public string Present(Setting_Float setting)
    {
        return $"{setting.DisplayName} {setting.GetValue()}";
    }

}

public class ValueRoundedPresenter : IFloatPresenter
{
    public FloatPresentation TargetPresentation => FloatPresentation.ValueRounded;

    public string Present(Setting_Float setting)
    {
        return $"{setting.DisplayName} {(int)setting.GetValue()}";
    }

}