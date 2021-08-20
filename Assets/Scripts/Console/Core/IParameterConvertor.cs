using System;

public interface IParameterConvertor
{
    Type ConvertorType { get; }
    bool TryConvert(string input, out object result);

}
