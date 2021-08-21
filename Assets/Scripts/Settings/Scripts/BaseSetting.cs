using UnityEngine;

public abstract class BaseSetting : ScriptableObject
{

    [SerializeField] private string _displayName;

    public string DisplayName => _displayName;

    public virtual void Reset() { }
    public virtual void OnGameStarted() { }

}