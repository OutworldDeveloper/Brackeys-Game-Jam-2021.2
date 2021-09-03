using DG.Tweening;
using UnityEngine;

public abstract class WindowAnimation<T> : ScriptableObject where T : UI_BasePanel<T>
{
    public abstract void ModifySequence(T window, Sequence sequence);

}

public abstract class GenericWindowAnimation : ScriptableObject
{
    public abstract void ModifySequence(IPanel window, Sequence sequence);

}