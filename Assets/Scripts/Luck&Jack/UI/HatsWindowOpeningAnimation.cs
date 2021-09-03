using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "Hats Window Opening Animation", menuName = "Window Animations/Hats Window Opening")]
public class HatsWindowOpeningAnimation : WindowAnimation<UI_HatsWindow>
{
    public override void ModifySequence(UI_HatsWindow window, Sequence sequence)
    {
        for (int i = 0; i < window.HatButtons.Count; i++)
        {
            var button = window.HatButtons[i];
            sequence.Insert(0.2f * (1 + i), button.CanvasGroup.DOFade(1f, 0.1f).From(0f));
            sequence.Insert(0.2f * (1 + i), button.transform.DOScale(Vector3.one, 0.2f).From(Vector3.one * 1.25f));
        }
    }

}