using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Opening Animation", menuName = "Window Animations/Default Opening")]
public class DefaultOpeningAnimation : GenericWindowAnimation
{

    public override void ModifySequence(IPanel window, Sequence sequence)
    {
        var alpha = window.CanvasGroup.DOFade(1f, 0.25f);
        var scaleX = window.RectTransform.DOScaleX(1f, 0.16f).SetEase(Ease.OutBack);
        var scaleY = window.RectTransform.DOScaleY(1f, 0.25f).SetEase(Ease.OutBack);

        sequence.Insert(0, alpha);
        sequence.Insert(0, scaleX);
        sequence.Insert(0, scaleY);
    }

}