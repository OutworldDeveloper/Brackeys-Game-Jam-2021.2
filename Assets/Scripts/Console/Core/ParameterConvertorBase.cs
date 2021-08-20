using System;

public abstract class ParameterConvertorBase<T> : IParameterConvertor
{
    public Type ConvertorType => typeof(T);

    public bool TryConvert(string input, out object result)
    {
        if (TryConvert(input, out T convertionResult))
        {
            result = convertionResult;
            return true;
        }
        result = default;
        return false;
    }

    protected abstract bool TryConvert(string input, out T result);

}