using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Need to find a way to make it more generic. 
// It should be able to save data into different folders
public sealed class SavingSystem
{

    // Generics here are usless for now
    // Maybe it's not needed actually
    public static DataContainer CreateSaveData<T>(string saveName) where T : DataContainer, new()
    {
        var dataContainer = new T();

        var saveInfo = new SaveInfo()
        {
            FileName = Guid.NewGuid().ToString(),
            SaveName = saveName,
            CreationTime = DateTime.Now,
        };

        dataContainer.SetData("SaveInfo", saveInfo);

        return dataContainer;
    }

    private readonly IDataSerializer _dataSerializer;

    public SavingSystem(IDataSerializer dataSerializer)
    {
        _dataSerializer = dataSerializer;
    }

    public DataContainer[] LoadSaves()
    {
        var saveDatas = new List<DataContainer>();
        var saveFolder = new DirectoryInfo(GetPath());

        foreach (var fileInfo in saveFolder.GetFiles())
        {
            var result = _dataSerializer.Deserialize<DataContainer>(fileInfo);
            saveDatas.Add(result);
        }

        return saveDatas.ToArray();
    }

    public DataContainer LoadOrCreateSave(int index, string saveName)
    {
        var saveDatas = LoadSaves();

        if (saveDatas.Length > 0)
        {
            return saveDatas[index];
        }

        return CreateSaveData<DataContainer>(saveName);
    }

    public void SaveOverride(DataContainer saveData)
    {
        var fileName = saveData.GetData<SaveInfo>("SaveInfo").FileName;
        var savePath = $"{GetPath()}/{fileName}.txt";
        _dataSerializer.Serialize(savePath, saveData);
    }

    // FIXME: Shouldn't be here. 
    private string GetPath()
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        path += $"/{Application.companyName}/{Application.productName}/Saves";
        Directory.CreateDirectory(path);
        return path;
    }

}

[Serializable]
public struct SaveInfo
{
    public string FileName;
    public string SaveName;
    public DateTime CreationTime;

}