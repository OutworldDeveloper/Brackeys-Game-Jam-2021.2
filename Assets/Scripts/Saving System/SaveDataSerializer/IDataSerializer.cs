using System.IO;

public interface IDataSerializer
{
    void Serialize<T>(string path, T saveData) where T : DataContainer;
    T Deserialize<T>(FileInfo fileInfo) where T : DataContainer;

}
