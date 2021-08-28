using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(UI_WindowReferences))]
public abstract class UI_BaseWindow : MonoBehaviour
{

    // An idea: Add ShouldHideMenusUnderneath for different menus

    [Inject] private UI_WindowsManager _windowsManager;
    [Inject] private CursorManager _cursorManager;
    [Inject] private TimescaleManager _timescaleManager;
    [Inject] private InputSystem _inputSystem;

    public CanvasGroup CanvasGroup { get; private set; }
    public RectTransform RectTransform { get; private set; }
    public Text TitleText { get; private set; }
    public Button CloseButton { get; private set; }

    protected readonly InputReciver InputReciver = new InputReciver(true);

    private IWindowAnimation _openingAnimation;
    private IWindowAnimation _closingAnimation;

    private Sequence _currentSequence;
    private bool _isClosing;
    private bool _isClosingDisabled;

    private void Awake()
    {
        var references = GetComponent<UI_WindowReferences>();
        CanvasGroup = references.CanvasGroup;
        RectTransform = references.RectTransform;
        TitleText = references.TitleText;
        CloseButton = references.CloseButton;
    }

    private void Start()
    {
        _openingAnimation = CreateOpeningAnimation();
        _closingAnimation = CreateClosingAnimation();

        CanvasGroup.alpha = 0f;
        RectTransform.localScale = new Vector3(0, 0f, 1f);

        _currentSequence = DOTween.Sequence();
        _openingAnimation.ModifySequence(_currentSequence);

        _currentSequence.SetUpdate(true);

        CloseButton.onClick.AddListener(() =>
        {
            if (!_isClosingDisabled && !_isClosing)
                CloseThenDestroy();
        });

        _windowsManager.AddWindow(this);
        _cursorManager.Show(this);
        _timescaleManager.Pause(this);

        _inputSystem.AddReciver(InputReciver);

        InputReciver.BindInputActionPressed("pause", () =>
        {
            if (!_isClosingDisabled && !_isClosing)
                CloseThenDestroy();
        });

        OnOpened();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void CloseThenDestroy()
    {
        if (_isClosing)
            return;

        _isClosing = true;

        _currentSequence?.Kill();

        _currentSequence = DOTween.Sequence();
        _closingAnimation.ModifySequence(_currentSequence);
        _currentSequence.SetUpdate(true).OnComplete(() => Destroy(gameObject));

        _windowsManager.RemoveWindow(this);

        _cursorManager.Hide(this);
        _timescaleManager.Unpause(this);

        _inputSystem.RemoveReciver(InputReciver);

        OnClosed();
    }

    public void SetTitle(string title)
    {
        TitleText.text = title;
    }

    public void AddCloseButtonCallback(Action action)
    {
        CloseButton.gameObject.SetActive(true);
        CloseButton.onClick.AddListener(() => 
        {
            if (!_isClosingDisabled && !_isClosing)
            {
                action.Invoke();
            }
        });
    }

    public void DisableClosing()
    {
        _isClosingDisabled = true;
        CloseButton.gameObject.SetActive(false);
    }

    protected virtual IWindowAnimation CreateOpeningAnimation()
    {
        return new DefaultOpeningAnimation<UI_BaseWindow>(this);
    }

    protected virtual IWindowAnimation CreateClosingAnimation()
    {
        return new DefaultClosingAnimation<UI_BaseWindow>(this);
    }

    protected virtual void OnOpened() { }
    protected virtual void OnClosed() { }

}