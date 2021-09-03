using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public sealed class BinaryDataSerializer : IDataSerializer
{

    public void Serialize<T>(string path, T saveData) where T : DataContainer
    {
        using (var stream = File.Open(path, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, saveData);
        }
    }

    public T Deserialize<T>(FileInfo fileInfo) where T : DataContainer
    {
        using (FileStream stream = fileInfo.Open(FileMode.Open))
        {
            var binaryFormatter = new BinaryFormatter();
            var result = binaryFormatter.Deserialize(stream);
            return result as T;
        }
    }

}