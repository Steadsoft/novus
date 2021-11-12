namespace Steadsoft.Novus.Scanner
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
        SemiColon,
        Minus,
        Plus,
        Times,
        Call,
        GoesTo,
        Comma
    }

    public enum Operator
    {
        /// <summary>
        /// ->
        /// </summary>
        PointsTo = TokenType.PointsTo,
        /// <summary>
        /// =>
        /// </summary>
        GoesTo = TokenType.GoesTo,
        /// <summary>
        /// =
        /// </summary>
        Equals = TokenType.Equals,
        /// <summary>
        /// ==
        /// </summary>
        Equality = TokenType.Equals,
        /// <summary>
        /// >>
        /// </summary>
        ShiftRight = TokenType.ShiftRight,
        /// <summary>
        /// <<
        /// </summary>
        ShiftLeft = TokenType.ShiftLeft,
        /// <summary>
        /// -
        /// </summary>
        Minus = TokenType.Minus,
        /// <summary>
        /// +
        /// </summary>
        Plus = TokenType.Plus,
        /// <summary>
        /// *
        /// </summary>
        Times = TokenType.Times
    }
}