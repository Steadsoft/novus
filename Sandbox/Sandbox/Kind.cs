namespace Sandbox
{
    /// <summary>
    /// Represents characters by their "kind" or "type"
    /// </summary>
    public enum Kind
    {
        Whitespace,
        Punctuation,
        Digit,
        Letter,
        Symbol,
        Parenthesis,
        Bracket,
        Brace,
        Arrow,
        CR,
        LF,
        Uncategorized,
        AnythingElse
    }
}