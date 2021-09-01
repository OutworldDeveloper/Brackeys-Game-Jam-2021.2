using System.Collections.Generic;
using Zenject;

public class SceneStateCapture
{

    private readonly SavingSystem _savingSystem;
    private readonly IEnumerable<ISavingCallbackReciver> _saveables;
    private readonly DataContainer _saveData;

    public SceneStateCapture(
        SavingSystem savingSystem, 
        IEnumerable<ISavingCallbackReciver> saveables, 
        DataContainer saveData)
    {
        _savingSystem = savingSystem;
        _saveables = saveables;
        _saveData = saveData;
    }

    public void SaveGame()
    {
        Capture(_saveData);
        _savingSystem.SaveOverride(_saveData);
    }

    private void Capture(DataContainer saveData)
    {
        foreach (var saveable in _saveables)
        {
            saveable.OnSaving(saveData);
        }
    }

    // Recreate it when needed so it can recive all the callback recivers
    public class Factory : PlaceholderFactory<SceneStateCapture> { }

}