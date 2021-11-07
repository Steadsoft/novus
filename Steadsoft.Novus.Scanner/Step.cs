namespace Steadsoft.Novus.Scanner
{
    public enum Step
    {
        /// <summary>
        /// Append the just-read char to the buffer and continue building the token.
        /// </summary>
        AppendContinue,
        /// <summary>
        /// Append the just-read char to the buffer and retunr the built token.
        /// </summary>
        AppendReturn,
        /// <summary>
        /// Discard the just-read char but continue building the token.
        /// </summary>
        DiscardContinue,
        /// <summary>
        /// Return the just-read char so it will be read again but return the currently built token
        /// </summary>
        RewindReturn
    }
}