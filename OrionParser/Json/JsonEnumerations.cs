using System;

namespace OrionParser.Json
{
    internal enum ParsingState
    {
        None,
        Object,
        Name,
        ValueString,
        Value
    }

}
