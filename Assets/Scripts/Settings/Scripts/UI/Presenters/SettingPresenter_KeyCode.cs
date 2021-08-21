using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class SettingPresenter_KeyCode : SettingPresenter<Setting_KeyCode>
{

    [SerializeField] private Text _keyCodeText;
    [SerializeField] private Button _inputButton;

    private bool _isListening;

    protected override void Present()
    {
        _keyCodeText.text = Setting.GetValue().ToString();
    }

    protected override void OnSetup()
    {
        base.OnSetup();
        _inputButton.onClick.AddListener(StartListeningForInput);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _inputButton.onClick.RemoveListener(StartListeningForInput);
    }

    private void Update()
    {
        if (!_isListening) 
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndListeningForInput();
            return;
        }

        if (Input.anyKeyDown)
        {
            foreach (KeyCode item in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(item))
                {
                    Setting.SetValue(item);
                    EndListeningForInput();
                    _isListening = false;
                    return;
                }
            }
        }
    }

    private void StartListeningForInput()
    {
        _isListening = true;
        _keyCodeText.text = "Press any key";
    }

    private void EndListeningForInput()
    {
        _keyCodeText.text = Setting.GetValue().ToString();
        _isListening = false;
    }

}