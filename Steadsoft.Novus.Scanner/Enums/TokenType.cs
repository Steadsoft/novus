﻿namespace Steadsoft.Novus.Scanner.Enums
{
    public enum TokenType
    {
        NoMoreTokens,
        Undecided,
        Identifier,
        LineComment,
        BlockComment,
        NumericLiteral,
        Punctuator,
        Symbol,
        PointsTo,
        Equals,
        Equality,
        Greater,
        ShiftRight,
        Lesser,
        ShiftLeft,
        QString,
        AString,
        BraceOpen,
        BraceClose,
        BracketOpen,
        BracketClose,
        ParenOpen,
        ParenClose,
        SemiColon,
        NewLine,
        CR,
        LF,
        Minus,
        Plus,
        Times,
        Call,
        GoesTo,
        Comma,
        Preprocessor
    }
}