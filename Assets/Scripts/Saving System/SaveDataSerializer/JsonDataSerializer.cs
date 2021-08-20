using System.IO;
using Newtonsoft.Json;

public sealed class JsonDataSerializer : IDataSerializer
{

    public void Serialize<T>(string path, T saveData) where T : DataContainer
    {
        using (StreamWriter streamWriter = new StreamWriter(path))
        {
            using (JsonWriter writer = new JsonTextWriter(streamWriter))
            {
                var jsonSerializer = new JsonSerializer();

                jsonSerializer.NullValueHandling = NullValueHandling.Ignore;
                jsonSerializer.TypeNameHandling = TypeNameHandling.Auto;
                jsonSerializer.Formatting = Formatting.Indented;
                jsonSerializer.MissingMemberHandling = MissingMemberHandling.Error;

                jsonSerializer.Serialize(writer, saveData);
            }
        }
    }

    public T Deserialize<T>(FileInfo fileInfo) where T : DataContainer
    {       
        using (var streamReader = new StreamReader(fileInfo.Open(FileMode.Open)))
        {
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var jsonSerializer = new JsonSerializer();

                jsonSerializer.NullValueHandling = NullValueHandling.Ignore;
                jsonSerializer.TypeNameHandling = TypeNameHandling.Auto;
                jsonSerializer.Formatting = Formatting.Indented;
                jsonSerializer.MissingMemberHandling = MissingMemberHandling.Error;
                jsonSerializer.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;

                return jsonSerializer.Deserialize<T>(jsonReader);             
            }
        }
    }

}
