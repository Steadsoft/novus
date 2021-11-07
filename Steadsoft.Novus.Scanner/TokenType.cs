﻿namespace Steadsoft.Novus.Scanner
{
    public enum TokenType
    {
        NoMoreTokens,
        Undecided,
        Identifier,
        LineComment,
        BlockComment,
        Integer,
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
        LBrace,
        RBrace,
        LBrack,
        RBrack,
        LPar,
        RPar,
        SemiColon
    }
}