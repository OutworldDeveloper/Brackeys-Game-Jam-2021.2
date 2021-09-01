using System;
using System.Collections.Generic;
using UnityEngine;

// Рекорды и шапки?
public class PlayerProfile : MonoBehaviour, ISavingCallbackReciver
{
    public void OnSaving(DataContainer data)
    {
        throw new NotImplementedException();
    }

}

public class UnlockablesManager : ISavingCallbackReciver
{

    private List<Unlockable> _unlockedItems = new List<Unlockable>();

    public bool IsUnlocked(Unlockable unlockable)
    {
        return _unlockedItems.Contains(unlockable);
    }

    public void Unlock(Unlockable unlockable)
    {
        _unlockedItems.Add(unlockable);
    }

    public void OnSaving(DataContainer data)
    {
        var unlockedItemsSerialized = new string[_unlockedItems.Count];

        for (int i = 0; i < _unlockedItems.Count; i++)
        {
            unlockedItemsSerialized[i] = _unlockedItems[i].name;
        }

        data.SetData("Unlocked", unlockedItemsSerialized);
    }

}

public abstract class Unlockable : ScriptableObject
{

}

public class Hat : Unlockable
{

    [SerializeField] private string _displayName;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private HatVisuals _prefab;

    public string DisplayName => _displayName;
    public Sprite Sprite => _sprite; 
    public HatVisuals Prefab => _prefab; 

}