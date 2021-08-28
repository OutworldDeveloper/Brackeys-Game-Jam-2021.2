using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LuckSelectionMenu : UI_SelectionMenu
{

    protected override IWindowAnimation CreateOpeningAnimation()
    {
        return new LuckSelectionMenuOpeningAnimation(this);
    }

}

public class LuckOpeningAnimation<T> : WindowAnimation<T> where T : UI_BaseWindow
{
    public LuckOpeningAnimation(T window) : base(window) { }

    public override Sequence CreateSequence()
    {
        var sequence = DOTween.Sequence();

        var alpha = Window.CanvasGroup.DOFade(1f, 0.25f);
        var scale = Window.RectTransform.DOScale(1f, 0.2f).From(1.5f).SetEase(Ease.OutExpo); 

        sequence.Insert(0, alpha);
        sequence.Insert(0, scale);

        return sequence;
    }

}

public class LuckSelectionMenuOpeningAnimation : LuckOpeningAnimation<UI_SelectionMenu>
{
    public LuckSelectionMenuOpeningAnimation(UI_SelectionMenu window) : base(window) { }

    public override Sequence CreateSequence()
    {
        var sequence = base.CreateSequence();

        for (int i = 0; i < Window.CurrentButtons.Length; i++)
        {
            var button = Window.CurrentButtons[i];

            sequence.Insert(i * 0.05f, button.transform.DOScale(Vector3.one, 0.1f).From(Vector2.zero).SetEase(Ease.OutExpo));
            sequence.Insert(i * 0.05f, button.CanvasGroup.DOFade(1f, 0.1f).From(0f));
        }

        return sequence;
    }

}