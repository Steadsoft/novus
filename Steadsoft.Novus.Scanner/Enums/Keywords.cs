﻿namespace Steadsoft.Novus.Scanner.Enums
{

    // Consider moving these to a text file and then creating per-language variants...

    public enum Keywords
    {
        IsNotAKeyword = 0,
        Argument,
        Binary,
        Builtin,
        By,
        Call,
        Coroutine,
        Decimal,
        Declare,
        End,
        Enum,
        Fixed,
        Float,
        Function,
        Goto,
        Internal,
        Interrupt,
        Language,
        Loop,
        Namespace,
        Out,
        Private,
        Procedure,
        Public,
        Readonly,
        Ref,
        Return,
        Returnon,
        Singlet,
        Static,
        String,
        Structure,
        To,
        Type,
        Until,
        Using,
        While,
        Varying,
        Yield,
        Arg = Argument + AbbreviationShift,
        Bin = Binary + AbbreviationShift,
        Dcl = Declare + AbbreviationShift,
        Dec = Decimal + AbbreviationShift,
        Func = Function + AbbreviationShift,
        Proc = Procedure + AbbreviationShift,
        Struct = Structure + AbbreviationShift,
        AbbreviationShift = 1000
 }
}
