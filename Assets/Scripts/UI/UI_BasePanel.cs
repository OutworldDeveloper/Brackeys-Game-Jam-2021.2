using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
public abstract class UI_BasePanel<T> : MonoBehaviour, IPanel where T : UI_BasePanel<T>
{

    [Inject] private UI_WindowsManager _windowsManager;
    [Inject] private InputSystem _inputSystem;

    [SerializeField] private GenericWindowAnimation _openingAnimation;
    [SerializeField] private WindowAnimation<T>[] _openingAnimations;
    [SerializeField] private GenericWindowAnimation _closingAnimation;
    [SerializeField] private WindowAnimation<T>[] _closingAnimations;

    public CanvasGroup CanvasGroup { get; private set; }
    public RectTransform RectTransform { get; private set; }
    public abstract bool HideWindowsUnderneath { get; }
    public abstract bool HideBackground { get; }

    private readonly InputReciver InputReciver = new InputReciver(true);

    private Sequence _currentSequence;
    protected bool IsClosing;

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        RectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        SetupInput(InputReciver);
        OnOpening();

        CanvasGroup.blocksRaycasts = false;

        _currentSequence = CreateOpeningSequence();

        _currentSequence.OnComplete(() =>
        {
            OnOpened();
            CanvasGroup.blocksRaycasts = true;
        });

        _inputSystem.AddReciver(InputReciver);
        _windowsManager.AddWindow(this);
    }

    public void CloseThenDestroy()
    {
        if (IsClosing)
        {
            return;
        }

        IsClosing = true;

        OnClosing();

        _currentSequence?.Kill();
        _currentSequence = CreateClosingSequence();      

        _currentSequence.OnComplete(() =>
        {
            OnClosed();
            Destroy(gameObject);
        });

        _inputSystem.RemoveReciver(InputReciver);
        _windowsManager.RemoveWindow(this);
    }

    public virtual void Show() 
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual void Select() 
    {
        CanvasGroup.interactable = true;
    }

    public virtual void Deselect() 
    {
        CanvasGroup.interactable = false;
    }

    protected virtual void SetupInput(InputReciver inputReciver) { }
    protected virtual void OnOpening() { }
    protected virtual void OnClosing() { }
    protected virtual void OnOpened() { }
    protected virtual void OnClosed() { }

    private Sequence CreateOpeningSequence()
    {
        return CreateSequence(_openingAnimation, _openingAnimations);
    }

    private Sequence CreateClosingSequence()
    {
        return CreateSequence(_closingAnimation, _closingAnimations);
    }

    private Sequence CreateSequence(GenericWindowAnimation genericAnimation, IEnumerable<WindowAnimation<T>> animations)
    {
        var sequence = DOTween.Sequence();

        genericAnimation?.ModifySequence(this, sequence);

        foreach (var animation in animations)
        {
            animation.ModifySequence(this as T, sequence);
        }

        sequence.SetUpdate(true);

        return sequence;
    }

}

public interface IPanel
{
    CanvasGroup CanvasGroup { get; }
    RectTransform RectTransform { get; }
    bool HideWindowsUnderneath { get; }
    bool HideBackground { get; }
    void Show();
    void Hide();
    void Select();
    void Deselect();

}