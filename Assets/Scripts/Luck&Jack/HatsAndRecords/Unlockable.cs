using UnityEngine;

public abstract class Unlockable : ScriptableObject
{

    [SerializeField] private string _displayName;
    [SerializeField] private UnlockRequirements _unlockRequirements;

    public string DisplayName => _displayName;
    public UnlockRequirements UnlockRequirements => _unlockRequirements;

}
