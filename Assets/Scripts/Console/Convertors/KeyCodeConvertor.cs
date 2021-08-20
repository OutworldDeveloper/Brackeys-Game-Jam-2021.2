using System;
using UnityEngine;

public class KeyCodeConvertor : ParameterConvertorBase<KeyCode>
{
    protected override bool TryConvert(string input, out KeyCode result)
    {
        if (Enum.TryParse(input, out KeyCode parseResult))
        {
            result = parseResult;
            return true;
        }
        result = default;
        return false;
    }

}
