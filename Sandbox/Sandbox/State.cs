namespace Sandbox
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
        INITIAL = 0,
        IDENTIFIER = 1,
        INTEGER = 2,
        SLASH = 3,
        SLASH_SLASH = 4,
        SLASH_STAR = 5,
        STAR = 6,
        SLASH_STAR_STAR = 7
    }
   
}