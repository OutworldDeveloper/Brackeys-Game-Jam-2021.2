using DG.Tweening;

public class DefaultClosingAnimation<T> : WindowAnimation<T> where T : UI_BaseWindow
{
    public DefaultClosingAnimation(T window) : base(window) { }

    public override void ModifySequence(Sequence sequence)
    {
        var alpha = Window.CanvasGroup.DOFade(0f, 0.17f);
        var scale = Window.RectTransform.DOScale(1.5f, 0.17f);

        sequence.Insert(0, alpha);
        sequence.Insert(0, scale);
    }

}