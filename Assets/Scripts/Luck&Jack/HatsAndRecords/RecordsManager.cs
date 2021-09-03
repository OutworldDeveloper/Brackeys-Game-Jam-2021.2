using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RecordsManager : MonoBehaviour
{

    [Inject] private SavingSystem _savingSystem;
    [Inject] private DataContainer _saveData;

    public StatsCapture.Stats GetRecords(GameplayScene gameplayScene)
    {
        Debug.Log(gameplayScene);
        return _saveData.GetData<StatsCapture.Stats>(gameplayScene.name);
    }

    public void UpdateRecords(GameplayScene gameplayScene, StatsCapture.Stats stats)
    {
        var key = gameplayScene.name;
        var beforeStats = _saveData.GetData<StatsCapture.Stats>(key);

        if (stats.Win)
        {
            beforeStats.Win = true;
        }

        if (stats.GravesSaved > beforeStats.GravesSaved)
        {
            beforeStats.GravesSaved = stats.GravesSaved;
        }

        if (stats.RatsKilled > beforeStats.RatsKilled)
        {
            beforeStats.RatsKilled = stats.RatsKilled;
        }

        _saveData.SetData(key, beforeStats);
        _savingSystem.SaveOverride(_saveData);
    }

}