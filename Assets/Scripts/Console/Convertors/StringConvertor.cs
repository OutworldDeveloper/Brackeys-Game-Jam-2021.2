public sealed class StringConvertor : ParameterConvertorBase<string>
{

    private const char STRING_SPLITTING_CHARACTER = '"';

    protected override bool TryConvert(string input, out string result)
    {
        if (input[0] != STRING_SPLITTING_CHARACTER || input[input.Length - 1] != STRING_SPLITTING_CHARACTER)
        {
            result = default;
            return false;
        }

        input = input.Trim(STRING_SPLITTING_CHARACTER);

        result = input;
        return true;
    }

}