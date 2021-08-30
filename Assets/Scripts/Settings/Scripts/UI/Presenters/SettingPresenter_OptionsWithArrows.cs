using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPresenter_OptionsWithArrows : SettingPresenter<Setting_Options>
{

    [SerializeField] private Button _prevButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Text _text;

    protected override void Present()
    {
        var value = Setting.GetValue();
        _text.GetComponentInChildren<Text>().text = Setting.Options[value];
    }

    protected override void OnSetup()
    {
        base.OnSetup();
        _prevButton.onClick.AddListener(Prev);
        _nextButton.onClick.AddListener(Next);
    }

    private void Prev()
    {
        var value = Setting.GetValue();
        if (value - 1 < 0)
        {
            Setting.SetValue(Setting.Options.Length - 1);
        }
        else
        {
            Setting.SetValue(value - 1);
        }
    }

    private void Next()
    {
        var value = Setting.GetValue();
        if (value + 1 > Setting.Options.Length - 1)
        {
            Setting.SetValue(0);
        }
        else
        {
            Setting.SetValue(value + 1);
        }
    }

}