namespace Steadsoft.Novus.Scanner.Enums
{
    public enum Operators
    {
        IsNotAnOperator = 0,
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
        Equals = TokenType.Equal,
        /// <summary>
        /// ==
        /// </summary>
        Equality = TokenType.Equal,
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