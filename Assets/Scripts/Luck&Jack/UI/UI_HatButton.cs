using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(TextTooltipTrigger))]
public class UI_HatButton : MonoBehaviour
{

    [SerializeField] private Image _icon;
    [SerializeField] private GameObject _selection;

    public Button Button => GetComponent<Button>();
    public CanvasGroup CanvasGroup => GetComponent<CanvasGroup>();

    [Inject]
    public void Init(Hat hat, bool unlocked)
    {
        _icon.sprite = hat.Sprite;
        EnableSelection(false);

        var trigger = GetComponent<TextTooltipTrigger>();
        trigger.Target.Title = hat.DisplayName;

        if (unlocked)
        {
            trigger.Target.Text = hat.Description;
        }
        else
        {
            _icon.color = Color.black;
            Button.interactable = false;
            trigger.Target.Text = hat.UnlockRequirements.Text;
        }
    }

    public void EnableSelection(bool b)
    {
        _selection.SetActive(b);
    }

    public class Factory : PlaceholderFactory<Hat, bool, UI_HatButton> { }

}