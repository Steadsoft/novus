namespace Steadsoft.Novus.Scanner.Enums
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
        DELIMITER0,
        DELIMITER1,
        DELIMITER2,
        DELIMITER3,
        DELIMITER4,
        DELIMITER5,
        DELIMITER6,
        DELIMITER7,
        DELIMITER8,
        DELIMITER9,
        INITIAL,
        IDENTIFIER,
        DIGIT_0,
        DECIMAL,
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
        PREPROC,
        PREPROC2,
        HEX_0,
        HEX_1, 
        HEX_2, 
        HEX_3
    }
}