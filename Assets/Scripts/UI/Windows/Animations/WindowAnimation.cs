using DG.Tweening;

public abstract class WindowAnimation<T> : IWindowAnimation where T : UI_BaseWindow
{

    protected readonly T Window;

    public WindowAnimation(T window)
    {
        Window = window;
    }

    public abstract Sequence CreateSequence();

}