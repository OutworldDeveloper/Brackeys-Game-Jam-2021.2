using DG.Tweening;
using UnityEngine;

public abstract class WindowAnimation<T> : ScriptableObject where T : UI_BaseWindow<T>
{
    public abstract void ModifySequence(T window, Sequence sequence);

}

public abstract class GenericWindowAnimation : ScriptableObject
{
    public abstract void ModifySequence(Window window, Sequence sequence);

}