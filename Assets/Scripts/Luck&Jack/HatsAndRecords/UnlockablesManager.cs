using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UnlockablesManager : MonoBehaviour
{

    [SerializeField] private Unlockable[] _unlockables;

    [Inject] private DataContainer _saveData;
    [Inject] private SavingSystem _savingSystem;
    [Inject] private RecordsManager _recordsManager;

    public IReadOnlyCollection<Unlockable> Unlockables => _unlockables;

    public T GetHatByName<T>(string name) where T : Unlockable
    {
        foreach (var unlockable in _unlockables)
        {
            if (unlockable is T)
            {
                if (unlockable.name == name)
                {
                    return unlockable as T;
                }
            }
        }
        return null;
    }

    public IEnumerable<T> GetUnlockablesOfType<T>(bool onlyUnlocked) where T : Unlockable
    {
        foreach (var unlockable in _unlockables)
        {
            if (onlyUnlocked && IsUnlocked(unlockable) == false)
            {
                continue;
            }

            if (unlockable is T)
            {
                yield return unlockable as T;
            }
        }
    }

    public bool IsUnlocked(Unlockable unlockable)
    {
        return _saveData.TryGetData($"{unlockable.name}_unlocked", out bool result);
    }

    public void Unlock(Unlockable unlockable)
    {
        _saveData.SetData($"{unlockable.name}_unlocked", true);
        _savingSystem.SaveOverride(_saveData);
    }

    public void TryUnlockAll()
    {
        foreach (var unlockable in _unlockables)
        {
            var scene = unlockable.UnlockRequirements.GameplayScene;
            var records = _recordsManager.GetRecords(scene);

            if (unlockable.UnlockRequirements.ShouldWin && !records.Win)
            {
                continue;
            }

            if (unlockable.UnlockRequirements.MinGravesSaved > records.GravesSaved)
            {
                continue;
            }

            Unlock(unlockable);
        }
    }

}