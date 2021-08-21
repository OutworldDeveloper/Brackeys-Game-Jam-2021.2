using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsGroupPresenter : MonoBehaviour
{

    [SerializeField] private Text text = null;

    public void Setup(SettingsGroup settingsGroup)
    {
        text.text = settingsGroup.DisplayName;
    }

}