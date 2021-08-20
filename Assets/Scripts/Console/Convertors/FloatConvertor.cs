public class FloatConvertor : ParameterConvertorBase<float>
{
    protected override bool TryConvert(string input, out float result)
    {
        if (input.EndsWith("f") == false)
        {
            result = default;
            return false;
        }

        input = input.Remove(input.Length - 1);


        if (float.TryParse(input, out float parsedResult) == false)
        {
            result = default;
            return false;
        }

        result = parsedResult;
        return true;
    }

}
