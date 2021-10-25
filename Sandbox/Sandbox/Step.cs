namespace Sandbox
{
    public enum Step
    {
        /// <summary>
        /// Append the just-read char to the buffer and continue building the token.
        /// </summary>
        AppendResume,
        /// <summary>
        /// Append the just-read char to the buffer and finish building the token.
        /// </summary>
        AppendHalt,
        /// <summary>
        /// Discard the just-read char and resume reading
        /// </summary>
        DiscardResume,
        /// <summary>
        /// Return the just-read char so it will be read again then continue building the token.
        /// </summary>
        //RestoreResume, // Likely not needed as it will just loop forever.
        /// <summary>
        /// Return the just-read char so it will be read again and finish building the token
        /// </summary>
        RestoreHalt
    }
}