namespace Steadsoft.Novus.Scanner
{
    public enum TokenDefinition
    {
        /// <summary>
        /// A pathname of a token defintion file.
        /// </summary>
        Pathname,
        /// <summary>
        /// Resource name of an embedded token defintion file.
        /// </summary>
        Resource,
        /// <summary>
        /// A plain text token defintion.
        /// </summary>
        Text
    }

    public enum SourceOrigin
    {
        /// <summary>
        /// A pathname of a source file.
        /// </summary>
        Pathname,
        /// <summary>
        /// Plain text source code.
        /// </summary>
        Text
    }
}
