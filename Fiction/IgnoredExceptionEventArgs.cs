namespace Fiction
{
    /// <summary>
    /// Event handler for an ignored exception event
    /// </summary>
    /// <param name="sender">Object that triggered the event</param>
    /// <param name="e">Arguments for the event</param>
    public delegate void IgnoredExceptionEventHandler(object sender, IgnoredExceptionEventArgs e);

    /// <summary>
    /// Event arguments for an ignored exception event
    /// </summary>
    public class IgnoredExceptionEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Constructs an <see cref="IgnoredExceptionEventArgs"/> object
        /// </summary>
        /// <param name="exception">Exception that was ignored</param>
        public IgnoredExceptionEventArgs(Exception exception)
        {
            Exception = exception;
        }
        /// <summary>
        /// Constructs an <see cref="IgnoredExceptionEventArgs"/> object
        /// </summary>
        /// <param name="exception">Exception that was ignored</param>
        /// <param name="extraInfo">Extra information about the exception</param>
        public IgnoredExceptionEventArgs(Exception exception, string extraInfo)
        {
            Exception = exception;
            ExtraInfo = extraInfo;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the exception that was ignored
        /// </summary>
        public Exception Exception { get; private set; }
        /// <summary>
        /// Gets extra information associated with this exception
        /// </summary>
        public string? ExtraInfo { get; private set; }
        #endregion
    }
}
