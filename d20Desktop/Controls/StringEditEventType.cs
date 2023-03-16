namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Enumeration of event types that can occur when editing a string in a <see cref="StringListEditor"/>
    /// </summary>
    public enum StringEditEventType
    {
        /// <summary>
        /// String was added
        /// </summary>
        Added,
        /// <summary>
        /// String was changed
        /// </summary>
        Changed,
        /// <summary>
        /// String was removed
        /// </summary>
        Removed,
    }
}