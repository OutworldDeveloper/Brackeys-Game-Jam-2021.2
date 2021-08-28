using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(UI_WindowReferences))]
public abstract class UI_BaseWindow : MonoBehaviour // Maybe add Esc button to close the window
{

    [Inject] private UI_WindowsManager _windowsManager;
    [Inject] private CursorManager _cursorManager;
    [Inject] private TimescaleManager _timescaleManager;
    [Inject] private InputSystem _inputSystem;

    protected UI_WindowReferences References;
    protected readonly InputReciver InputReciver = new InputReciver(true);
    private Sequence _currentSequence;
    private bool _isClosing;

    private void Awake()
    {
        References = GetComponent<UI_WindowReferences>();
    }

    private void Start()
    {
        References.CanvasGroup.alpha = 0f;
        References.RectTransform.localScale = new Vector3(0, 0f, 1f);

        _currentSequence = CreateOpeningSequence();
        _currentSequence.SetUpdate(true);

        References.CloseButton.onClick.AddListener(() => CloseThenDestroy());

        _windowsManager.AddWindow(this);

        _cursorManager.Show(this);
        _timescaleManager.Pause(this);

        _inputSystem.AddReciver(InputReciver);

        InputReciver.BindInputActionPressed("pause", CloseThenDestroy);

        OnOpened();
    }

    public void Show(bool b)
    {
        gameObject.SetActive(b);
    }

    public void CloseThenDestroy()
    {
        if (_isClosing)
            return;

        _isClosing = true;

        _currentSequence?.Kill();
        _currentSequence = CreateClosingSequence();
        _currentSequence.SetUpdate(true).OnComplete(() => Destroy(gameObject));

        _windowsManager.RemoveWindow(this);

        _cursorManager.Hide(this);
        _timescaleManager.Unpause(this);

        _inputSystem.RemoveReciver(InputReciver);

        OnClosed();
    }

    public void SetTitle(string title)
    {
        References.TitleText.text = title;
    }

    public void AddCloseButtonCallback(Action action)
    {
        References.CloseButton.gameObject.SetActive(true);
        References.CloseButton.onClick.AddListener(() => 
        {
            if (_isClosing == false)
            {
                action.Invoke();
            }
        });
    }

    public void DisableCloseButton()
    {
        References.CloseButton.gameObject.SetActive(false);
    }

    protected virtual void OnOpened() { }
    protected virtual void OnClosed() { }

    protected virtual Sequence CreateOpeningSequence()
    {
        var newSequence = DOTween.Sequence();

        var alpha = References.CanvasGroup.DOFade(1f, 0.25f);
        var scaleX = References.RectTransform.DOScaleX(1f, 0.16f).SetEase(Ease.OutBack);
        var scaleY = References.RectTransform.DOScaleY(1f, 0.25f).SetEase(Ease.OutBack);

        newSequence.Insert(0, alpha);
        newSequence.Insert(0, scaleX);
        newSequence.Insert(0, scaleY);

        return newSequence;
    }

    protected virtual Sequence CreateClosingSequence()
    {
        var newSequence = DOTween.Sequence();

        var alpha = References.CanvasGroup.DOFade(0f, 0.17f);
        var scaleX = References.RectTransform.DOScaleX(1.5f, 0.17f);
        var scaleY = References.RectTransform.DOScaleY(1.5f, 0.17f);

        newSequence.Insert(0, alpha);
        newSequence.Insert(0, scaleX);
        newSequence.Insert(0, scaleY);

        return newSequence;
    }

}