namespace Steadsoft.Novus.Scanner
{
    /// <summary>
    /// Represents the distinct states that arise as character are read.
    /// </summary>
    /// <remarks>
    /// Multi character tokens that are only distinguished by their 2nd or later characters
    /// are represented as states named after the characters read so far.
    /// </remarks>
    public enum State
    {
        INITIAL,
        IDENTIFIER,
        INTEGER,
        SLASH,
        SLASH_SLASH,
        SLASH_STAR,
        STAR,
        SLASH_STAR_STAR,
        HYPHEN,
        HYPHEN_ARROW,
        EQUALS,
        EQUALITY,
        GREATER,
        SHIFT_RIGHT,
        LESSER,
        SHIFT_LEFT,
        QUOTATION,
        APOSTROPHE,
    }
}