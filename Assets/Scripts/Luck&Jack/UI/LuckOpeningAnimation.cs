using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "Luck Opening Animation", menuName = "Window Animations/Luck Opening")]
public class LuckOpeningAnimation : GenericWindowAnimation
{
    public override void ModifySequence(IWindow window, Sequence sequence)
    {
        var alpha = window.CanvasGroup.DOFade(1f, 0.25f);
        var scale = window.RectTransform.DOScale(1f, 0.2f).From(1.5f).SetEase(Ease.OutExpo); 

        sequence.Insert(0, alpha);
        sequence.Insert(0, scale);
    }

}