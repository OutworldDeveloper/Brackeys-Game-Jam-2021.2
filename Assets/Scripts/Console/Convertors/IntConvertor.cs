using System.Collections;
using System.Collections.Generic;

public class IntConvertor : ParameterConvertorBase<int>
{
    protected override bool TryConvert(string input, out int result)
    {
        if (int.TryParse(input, out int parseResult))
        {
            result = parseResult;
            return true;
        }
        result = default;
        return false;
    }

}
