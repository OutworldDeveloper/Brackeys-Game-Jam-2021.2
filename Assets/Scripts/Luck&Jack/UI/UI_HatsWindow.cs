using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UI_HatsWindow : UI_BaseWindow<UI_HatsWindow>
{

    [Inject] private UnlockablesManager _unlockablesManager;
    [Inject] private JackCustomizaton _jackCustomizaton;

    [SerializeField] private UI_HatButton _hatButtonPrefab;
    [SerializeField] private RectTransform _hatsParent;

    public IReadOnlyList<UI_HatButton> HatButtons;
    protected override Selectable InitialSelection => CloseButton;

    private UI_HatButton _selectedButton;

    protected override void OnOpened()
    {
        base.OnOpened();

        var hatButtons = new List<UI_HatButton>(_unlockablesManager.Unlockables.Count + 1);

        {
            var button = CreateButton(_jackCustomizaton.DefaultHat, true);
            hatButtons.Add(button);
        }

        foreach (var hat in _unlockablesManager.Unlockables)
        {
            var isUnlocked = _unlockablesManager.IsUnlocked(hat);
            var button = CreateButton(hat, isUnlocked);
            hatButtons.Add(button);
        }

        HatButtons = hatButtons;
    }

    private UI_HatButton CreateButton(Hat hat, bool enabled)
    {
        bool selected = _jackCustomizaton.EquipedHat == hat;

        var button = Instantiate(_hatButtonPrefab, _hatsParent);
        button.Init(hat.Sprite, enabled);

        if (selected)
        {
            _selectedButton = button;
            button.EnableSelection(true);
        }

        if (enabled)
        {
            button.Button.onClick.AddListener(() =>
            {
                _selectedButton?.EnableSelection(false);
                _jackCustomizaton.Equip(hat);
                _selectedButton = button;
                button.EnableSelection(true);
            });
        }

        return button;
    }

    public class Factory : PlaceholderFactory<UI_HatsWindow> { }

}
