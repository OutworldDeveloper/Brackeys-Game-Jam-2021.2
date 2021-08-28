using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TweenButton : Button, IPointerClickHandler
{

    [SerializeField] private ScaleAction[] scaleActions = default;
    [SerializeField] private ColorAction[] colorActions = default;
    [SerializeField] private FillAmountAction[] fieldAmountActions = default;
    //[SerializeField] private UnityEvent onClick;
    //public UnityEvent OnClick => onClick;

    private List<ITweenSetting> tweenSettings = new List<ITweenSetting>();
    private List<Tween> activeTweens = new List<Tween>();

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);
        KillCurrentTweens();
        foreach (var tweenSetting in tweenSettings)
            activeTweens.Add(tweenSetting.DoStateTransition(state).SetUpdate(true));
    }

    protected override void Awake()
    {
        base.Awake();
        if (Application.isPlaying == false)
            return;
        foreach (var action in scaleActions) tweenSettings.Add(action);
        foreach (var action in fieldAmountActions) tweenSettings.Add(action);
        foreach (var action in colorActions) tweenSettings.Add(action);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        KillCurrentTweens();
    }

#if UNITY_EDITOR
    
    protected override void Reset()
    {
        base.Reset();
        KillCurrentTweens();
    }

#endif

    private void KillCurrentTweens()
    {
        foreach (var tween in activeTweens)
            tween.Kill();
    }

    [System.Serializable]
    private class ScaleAction : TweenSettings<RectTransform, Vector2>
    {

        protected override Tween DoHighlighted(float duration)
        {
            return Target.DOScale(highlightedState, duration);
        }

        protected override Tween DoNormal(float duration)
        {
            return Target.DOScale(normalState, duration);
        }

        protected override Tween DoPressed(float duration)
        {
            return Target.DOScale(pressedState, duration);
        }

        protected override Tween DoSelected(float duration)
        {
            return Target.DOScale(selectedState, duration);
        }
    }

    [System.Serializable]
    private class FillAmountAction : TweenSettings<Image, float>
    {

        protected override Tween DoHighlighted(float duration)
        {
            return Target.DOFillAmount(highlightedState, duration);
        }

        protected override Tween DoNormal(float duration)
        {
            return Target.DOFillAmount(normalState, duration);
        }

        protected override Tween DoPressed(float duration)
        {
            return Target.DOFillAmount(pressedState, duration);
        }

        protected override Tween DoSelected(float duration)
        {
            return Target.DOFillAmount(selectedState, duration);
        }

    }

    [System.Serializable]
    private class ColorAction : TweenSettings<Graphic, Color>
    {
        protected override Tween DoHighlighted(float duration)
        {
            return Target.DOColor(highlightedState, duration);
        }

        protected override Tween DoNormal(float duration)
        {
            return Target.DOColor(normalState, duration);
        }

        protected override Tween DoPressed(float duration)
        {
            return Target.DOColor(pressedState, duration);
        }

        protected override Tween DoSelected(float duration)
        {
            return Target.DOColor(selectedState, duration);
        }

    }

    private interface ITweenSetting
    {
        Tween DoStateTransition(SelectionState state);

    }

    private abstract class TweenSettings<T1, T2> : ITweenSetting where T1 : Component where T2: struct
    {

        public T1 Target;
        public float Duration;
        public T2 normalState;
        public T2 highlightedState;
        public T2 pressedState;
        public T2 selectedState;

        public Tween DoStateTransition(SelectionState state)
        {
            switch (state)
            {
                case SelectionState.Normal: return DoNormal(Duration);
                case SelectionState.Highlighted: return DoHighlighted(Duration);
                case SelectionState.Pressed: return DoPressed(Duration);
                case SelectionState.Selected: return DoSelected(Duration);
            }
            return default;
        }

        protected abstract Tween DoNormal(float duration);
        protected abstract Tween DoHighlighted(float duration);
        protected abstract Tween DoPressed(float duration);
        protected abstract Tween DoSelected(float duration);

    }

}