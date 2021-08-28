using DG.Tweening;
using UnityEngine;

public class DefaultSelectionMenuOpeningAnimation : DefaultOpeningAnimation<UI_SelectionMenu>
{
    public DefaultSelectionMenuOpeningAnimation(UI_SelectionMenu window) : base(window) { }

    public override void ModifySequence(Sequence sequence)
    {
        base.ModifySequence(sequence);

        for (int i = 0; i < Window.CurrentButtons.Length; i++)
        {
            var button = Window.CurrentButtons[i];

            sequence.Insert(i * 0.05f, button.transform.DOScale(Vector3.one, 0.1f).From(Vector2.zero));
            sequence.Insert(i * 0.05f, button.CanvasGroup.DOFade(1f, 0.1f).From(0f));
        }
    }

}