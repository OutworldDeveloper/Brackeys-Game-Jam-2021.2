using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class DataContainer
{
    [JsonProperty("Data")]
    private Dictionary<string, object> _content = new Dictionary<string, object>();

    public T GetData<T>(string id)
    {
        if (TryGetData(id, out T result))
        {
            return result;
        }
        return default;
    }

    public bool TryGetData<T>(string id, out T data)
    {
        if (_content.TryGetValue(id, out object result))
        {
            data = (T)result;
            return true;
        }

        data = default;
        return false;
    }

    public void SetData<T>(string id, T data)
    {
        if (_content.ContainsKey(id))
        {
            _content[id] = data;
            return;
        }

        _content.Add(id, data);
    }

}