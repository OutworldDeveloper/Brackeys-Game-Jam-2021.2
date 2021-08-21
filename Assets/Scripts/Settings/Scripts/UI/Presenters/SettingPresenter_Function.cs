using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPresenter_Function : SettingPresenter<Setting_Function>
{

    [SerializeField] private Button _executeButton;
    
    protected override void Present() { }

    protected override void OnSetup()
    {
        base.OnSetup();
        _executeButton.GetComponentInChildren<Text>().text = Setting.ButtonText;
        _executeButton.onClick.AddListener(Setting.Execute);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _executeButton.onClick.RemoveListener(Setting.Execute);
    }

}