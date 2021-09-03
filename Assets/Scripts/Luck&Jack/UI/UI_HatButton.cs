using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(CanvasGroup))]
public class UI_HatButton : MonoBehaviour
{

    [SerializeField] private Image _icon;
    [SerializeField] private GameObject _selection;

    public Button Button => GetComponent<Button>();
    public CanvasGroup CanvasGroup => GetComponent<CanvasGroup>();

    public void Init(Sprite sprite, bool enabled)
    {
        _icon.sprite = sprite;
        EnableSelection(false);
        if(!enabled)
        {
            _icon.color = Color.black;
            Button.interactable = false;
        }
    }

    public void EnableSelection(bool b)
    {
        _selection.SetActive(b);
    }

}