using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "Luck Selection Menu Opening Animation", menuName = "Window Animations/Luck Selection Menu Opening")]
public class LuckSelectionMenuOpeningAnimation : WindowAnimation<UI_SelectionMenu>
{

    public override void ModifySequence(UI_SelectionMenu window, Sequence sequence)
    {
        for (int i = 0; i < window.CurrentButtons.Length; i++)
        {
            var button = window.CurrentButtons[i];

            sequence.Insert(i * 0.05f, button.transform.DOScale(Vector3.one, 0.1f).From(Vector2.zero).SetEase(Ease.OutExpo));
            sequence.Insert(i * 0.05f, button.CanvasGroup.DOFade(1f, 0.1f).From(0f));
        }
    }

}