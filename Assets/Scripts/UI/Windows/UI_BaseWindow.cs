using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(UI_WindowReferences))]
public abstract class UI_BaseWindow<T> : MonoBehaviour, IWindow where T : UI_BaseWindow<T>
{

    [Inject] private UI_WindowsManager _windowsManager;
    [Inject] private CursorManager _cursorManager;
    [Inject] private TimescaleManager _timescaleManager;
    [Inject] private InputSystem _inputSystem;

    [SerializeField] private bool _hideMenusUnderneath = false;
    [SerializeField] private WindowAnimation<T>[] _openingAnimations;
    [SerializeField] private WindowAnimation<T>[] _closingAnimations;

    public CanvasGroup CanvasGroup => _references.CanvasGroup;
    public RectTransform RectTransform => _references.RectTransform;
    public Text TitleText => _references.TitleText;
    public Button CloseButton => _references.CloseButton;
    public bool HideWindowsUnderneath => _hideMenusUnderneath;
    protected abstract Selectable InitialSelection { get; }
    protected readonly InputReciver InputReciver = new InputReciver(true);

    private UI_WindowReferences _references;
    private Sequence _currentSequence;
    private bool _isClosing;
    private bool _isClosingDisabled;

    private void Awake()
    {
        _references = GetComponent<UI_WindowReferences>();
    }

    private void Start()
    {
        CanvasGroup.alpha = 0f;
        RectTransform.localScale = new Vector3(0, 0f, 1f);

        _currentSequence = DOTween.Sequence();

        _references.GenericOpeningAnimation.ModifySequence(this, _currentSequence);

        foreach (var animation in _openingAnimations)
        {
            animation.ModifySequence((T)this, _currentSequence);
        }

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

        //InitialSelection.Select();

        OnOpened();
    }

    public void Select()
    {
        InitialSelection.Select();
        CanvasGroup.interactable = true;
    }

    public void Deselect()
    {
        CanvasGroup.interactable = false;
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

        _references.GenericClosingAnimation.ModifySequence(this, _currentSequence);

        foreach (var animation in _closingAnimations)
        {
            animation.ModifySequence((T)this, _currentSequence);
        }

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

    public void DisableCloseButton()
    {
        CloseButton.gameObject.SetActive(false);
    }

    public void DisableClosing()
    {
        _isClosingDisabled = true;
        DisableCloseButton();
    }

    protected virtual void OnOpened() { }
    protected virtual void OnClosed() { }

}

public interface IWindow
{
    CanvasGroup CanvasGroup { get; }
    RectTransform RectTransform { get; }
    bool HideWindowsUnderneath { get; }
    void Show();
    void Hide();
    void Select();
    void Deselect();

}