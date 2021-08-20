using System;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class ParameterConvertors : IInitializable
{

    private readonly Dictionary<Type, IParameterConvertor> _convertors = new Dictionary<Type, IParameterConvertor>();

    public void Initialize()
    {
        var convertorsFinder = new ParametorConvertorsFinder();

        var convertors = convertorsFinder.FindParameterConvertors();

        foreach (var convertor in convertors)
        {
            var targetType = convertor.ConvertorType;
            _convertors.Add(targetType, convertor);
        }
    }

    public void AddConvertor(IParameterConvertor convertor)
    {
        var targetType = convertor.ConvertorType;
        _convertors.Add(targetType, convertor);
    }

    public bool TryFindConvertor(Type targetType, out IParameterConvertor result)
    {
        return _convertors.TryGetValue(targetType, out result);
    }

}