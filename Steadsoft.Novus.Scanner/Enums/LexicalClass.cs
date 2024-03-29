﻿namespace Steadsoft.Novus.Scanner.Enums
{
    public enum LexicalClass : uint
    {
        // We use these unicode characters to represent character 
        // classes that comprise tokens. This means that we can't ever
        // use these chatacters in source code, and that isn't too
        // hard to address if it were needed.

        APL_IBEAM = 0x2336,
        None = 0,
        Any = APL_IBEAM,
        Alpha = APL_IBEAM + 1,
        Digit = APL_IBEAM + 2,
        Group = APL_IBEAM + 3,
        White = APL_IBEAM + 4,
        Separ = APL_IBEAM + 5,
        Punct = APL_IBEAM + 6,
        Symbl = APL_IBEAM + 7,
        Space = APL_IBEAM + 8,
        Hex   = APL_IBEAM + 9,
        CR    = APL_IBEAM + 10,
        LF    = APL_IBEAM + 11,
        NewLine = APL_IBEAM + 12,
    }
}