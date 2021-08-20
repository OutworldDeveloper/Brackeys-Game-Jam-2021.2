using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;

// Data Manager
public class ConsoleFilesManager : IInitializable, IDisposable
{

    public DataContainer DataContainer { get; private set; }
    public string[] Autoexec { get; private set; }
    private readonly IDataSerializer _dataSerializer;

    public ConsoleFilesManager(IDataSerializer dataSerializer)
    {
        _dataSerializer = dataSerializer;
    }

    public void Initialize()
    {
        var autoexecPath = Path.Combine(GetPath(), "Autoexec.txt");
        if (File.Exists(autoexecPath))
        {
            Autoexec = File.ReadAllLines(autoexecPath);
        }
        else
        {
            File.Create(autoexecPath);
        }

        var filePath = $"{GetPath()}/ConsoleData.txt";
        if (File.Exists(filePath))
        {
            DataContainer = _dataSerializer.Deserialize<DataContainer>(new FileInfo(filePath));
            return;
        }
        DataContainer = new DataContainer();
    }

    public void Dispose()
    {
        var filePath = $"{GetPath()}/ConsoleData.txt";
        _dataSerializer.Serialize(filePath, DataContainer);
    }

    // FIXME: Shouldn't be here. 
    private string GetPath()
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        path += $"/{Application.companyName}/{Application.productName}";
        Directory.CreateDirectory(path);
        return path;
    }

}