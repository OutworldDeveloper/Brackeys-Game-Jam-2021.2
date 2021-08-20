public sealed class BoolConvertor : ParameterConvertorBase<bool>
{
    protected override bool TryConvert(string parameter, out bool result)
    {
        if (bool.TryParse(parameter, out bool parseResult))
        {
            result = parseResult;
            return true;
        }
        result = default;
        return false;
    }

}