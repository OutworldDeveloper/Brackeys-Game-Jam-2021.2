using System;

public class TypeConvertor : ParameterConvertorBase<Type>
{
    protected override bool TryConvert(string input, out Type result)
    {
        if (input.StartsWith("$") == false)
        {
            result = null;
            return false;
        }

        input = input.Remove(0, 1);

        result = Type.GetType(input);
        return result != null;
    }

}