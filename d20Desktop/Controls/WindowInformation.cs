using System.Windows;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Contains information about a window
    /// </summary>
    class WindowInformation
    {
        #region Constructors
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the top left of the window
        /// </summary>
        public Point TopLeft { get; set; }
        /// <summary>
        /// Gets or sets the size of the window
        /// </summary>
        public Size Size { get; set; }
        /// <summary>
        /// Gets or sets the state of the window
        /// </summary>
        public WindowState State { get; set; }
        #endregion
    }
}