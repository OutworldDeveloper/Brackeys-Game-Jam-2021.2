using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(UI_WindowReferences))]
public abstract class UI_BaseWindow<T> : UI_BasePanel<T>, IPanel where T : UI_BaseWindow<T>
{

    [Inject] private CursorManager _cursorManager;
    [Inject] private TimescaleManager _timescaleManager;

    [SerializeField] private bool _hideMenusUnderneath = false;

    public Text TitleText => _references.TitleText;
    public Button CloseButton => _references.CloseButton;

    public override bool HideWindowsUnderneath => _hideMenusUnderneath;
    public override bool HideBackground => true;

    protected virtual Selectable InitialSelection => null;

    private UI_WindowReferences _references => GetComponent<UI_WindowReferences>();
    private bool _isClosingNotAllowed;
    private Action _closingCallback;

    protected override void SetupInput(InputReciver inputReciver)
    {
        inputReciver.BindInputActionPressed("pause", () =>
        {
            if (!_isClosingNotAllowed && !IsClosing)
                CloseThenDestroy();
        });
    }

    // Maybe WindowsManager should manage input and cursor
    protected override void OnOpening()
    {
        _cursorManager.Show(this);
        _timescaleManager.Pause(this);
    }

    protected override void OnClosing()
    {
        _cursorManager.Hide(this);
        _timescaleManager.Unpause(this);
        _closingCallback?.Invoke();
    }

    public void SetTitle(string title)
    {
        TitleText.text = title;
        TitleText.gameObject.SetActive(true);
    }

    public void SetDescription(string description)
    {
        _references.DescriptionText.text = description;
        _references.DescriptionText.gameObject.SetActive(true);
    }

    public void EnableCloseButton(Action action)
    {
        CloseButton.gameObject.SetActive(true);
        CloseButton.onClick.AddListener(() => 
        {
            if (!_isClosingNotAllowed && !IsClosing)
            {
                action.Invoke();
            }
        });
    }

    public void DisableCloseButton()
    {
        CloseButton.gameObject.SetActive(false);
    }

    public void AddClosingCallback(Action action)
    {
        _closingCallback += action;
    }

    public void DisallowClosing()
    {
        _isClosingNotAllowed = true;
        DisableCloseButton();
    }

}