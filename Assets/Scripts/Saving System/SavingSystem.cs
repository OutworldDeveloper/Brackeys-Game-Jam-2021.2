using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SavingSystem
{

    public static DataContainer CreateSaveData(string saveName)
    {
        var dataContainer = new DataContainer();
        dataContainer.SetData("FileName", DateTime.Now.ToString("yyyyMMddTHHmmss"));
        dataContainer.SetData("SaveName", saveName);
        dataContainer.SetData("CreationTime", DateTime.Now);
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

    public void SaveOverride(DataContainer saveData)
    {
        var fileName = saveData.GetData<string>("FileName");
        var savePath = GetPath() + $"/{fileName}.txt";
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