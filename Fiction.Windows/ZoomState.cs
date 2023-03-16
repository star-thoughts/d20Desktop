namespace Fiction.Windows
{
    /// <summary>
    /// Gets the current state of the zoom usage
    /// </summary>
    public enum ZoomState
    {
        /// <summary>
        /// The zoom level is set to an unknown value
        /// </summary>
        Custom = 0,
        /// <summary>
        /// The zoom value is set to fit the entire contents on the screen
        /// </summary>
        Fit = 1,
        /// <summary>
        /// The zoom value is set to fit the height of the contents on the screen
        /// </summary>
        FitToHeight = 2,
        /// <summary>
        /// The zoom value is set to fit the width of the contents on the screen
        /// </summary>
        FitToWidth = 3,
    }
}