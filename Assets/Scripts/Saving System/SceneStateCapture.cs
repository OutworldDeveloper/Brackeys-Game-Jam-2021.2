using System.Collections.Generic;

public class SceneStateCapture
{

    private readonly SavingSystem _savingSystem;
    private readonly IEnumerable<ISavingCallbackReciver> _saveables;
    private readonly DataContainer _saveData;

    public SceneStateCapture(SavingSystem savingSystem, IEnumerable<ISavingCallbackReciver> saveables, DataContainer saveData)
    {
        _savingSystem = savingSystem;
        _saveables = saveables;
        _saveData = saveData;
    }
    
    private void Capture(DataContainer saveData)
    {
        foreach (var saveable in _saveables)
        {
            saveable.OnSaving(saveData);
        }
    }

    /*
    private void SaveGame()
    {
        Capture(_saveData);
        _savingSystem.SaveOverride(_saveData);
    }
    */

}