using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Closing Animation", menuName = "Window Animations/Default Closing")]
public class DefaultClosingAnimation : GenericWindowAnimation
{

    public override void ModifySequence(Window window, Sequence sequence)
    {
        var alpha = window.CanvasGroup.DOFade(0f, 0.17f);
        var scale = window.RectTransform.DOScale(1.5f, 0.17f);

        sequence.Insert(0, alpha);
        sequence.Insert(0, scale);
    }

}