using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UI_HatsWindow : UI_BaseWindow<UI_HatsWindow>
{

    [Inject] private UnlockablesManager _unlockablesManager;
    [Inject] private JackCustomizaton _jackCustomizaton;

    [SerializeField] private UI_TweenButton _hatButtonPrefab;
    [SerializeField] private RectTransform _hatsParent;

    protected override Selectable InitialSelection => CloseButton;

    protected override void OnOpened()
    {
        base.OnOpened();

        CreateButton(_jackCustomizaton.DefaultHat, true);

        foreach (var hat in _unlockablesManager.Unlockables)
        {
            var isUnlocked = _unlockablesManager.IsUnlocked(hat);
            CreateButton(hat, isUnlocked);
        }
    }

    private void CreateButton(Hat hat, bool enabled)
    {
        var button = Instantiate(_hatButtonPrefab, _hatsParent);
        button.GetComponent<Image>().sprite = hat.Sprite;

        if (enabled)
        {
            button.onClick.AddListener(() =>
            {
                _jackCustomizaton.Equip(hat);
            });
        }
        else
        {
            button.interactable = _unlockablesManager.IsUnlocked(hat);
            button.GetComponent<Image>().color = Color.black;
        }
    }

    public class Factory : PlaceholderFactory<UI_HatsWindow> { }

}