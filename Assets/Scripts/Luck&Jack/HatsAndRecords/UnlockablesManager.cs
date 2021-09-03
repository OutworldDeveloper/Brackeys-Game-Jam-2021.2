using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UnlockablesManager : MonoBehaviour
{

    [SerializeField] private Hat[] _unlockables;

    [Inject] private DataContainer _saveData;
    [Inject] private SavingSystem _savingSystem;
    [Inject] private RecordsManager _recordsManager;

    public IReadOnlyCollection<Hat> Unlockables => _unlockables;

    public Hat GetHatByName(string name)
    {
        foreach (var hat in _unlockables)
        {
            if (hat.name == name)
            {
                return hat;
            }
        }
        return null;
    }

    public bool IsUnlocked(Hat unlockable)
    {
        return _saveData.TryGetData($"{unlockable.name}_unlocked", out bool result);
    }

    public void Unlock(Hat unlockable)
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