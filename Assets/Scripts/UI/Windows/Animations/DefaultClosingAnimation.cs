using DG.Tweening;

public class DefaultClosingAnimation<T> : WindowAnimation<T> where T : UI_BaseWindow
{
    public DefaultClosingAnimation(T window) : base(window) { }

    public override Sequence CreateSequence()
    {
        var sequence = DOTween.Sequence();

        var alpha = Window.CanvasGroup.DOFade(0f, 0.17f);
        var scaleX = Window.RectTransform.DOScaleX(1.5f, 0.17f);
        var scaleY = Window.RectTransform.DOScaleY(1.5f, 0.17f);

        sequence.Insert(0, alpha);
        sequence.Insert(0, scaleX);
        sequence.Insert(0, scaleY);

        return sequence;
    }

}