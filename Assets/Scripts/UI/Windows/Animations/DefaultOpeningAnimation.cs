using DG.Tweening;

public class DefaultOpeningAnimation<T> : WindowAnimation<T> where T : UI_BaseWindow
{
    public DefaultOpeningAnimation(T window) : base(window) { }

    public override void ModifySequence(Sequence sequence)
    {
        var alpha = Window.CanvasGroup.DOFade(1f, 0.25f);
        var scaleX = Window.RectTransform.DOScaleX(1f, 0.16f).SetEase(Ease.OutBack);
        var scaleY = Window.RectTransform.DOScaleY(1f, 0.25f).SetEase(Ease.OutBack);

        sequence.Insert(0, alpha);
        sequence.Insert(0, scaleX);
        sequence.Insert(0, scaleY);
    }

}
