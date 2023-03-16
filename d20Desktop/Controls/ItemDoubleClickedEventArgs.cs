using System.Windows;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Event handler for ItemDoubleClicked event
    /// </summary>
    /// <param name="sender">Control that caught the event</param>
    /// <param name="e">Event args</param>
    public delegate void ItemDoubleClickedEventHandler(object sender, ItemDoubleClickedEventArgs e);

    /// <summary>
    /// Event arguments for an item being double clicked in a <see cref="FilterableList"/>
    /// </summary>
    public class ItemDoubleClickedEventArgs : RoutedEventArgs
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="ItemDoubleClickedEventArgs"/>
        /// </summary>
        /// <param name="item">Item that was double clicked</param>
        /// <param name="source">Source of the event</param>
        public ItemDoubleClickedEventArgs(object item, object source)
            : base(FilterableList.ItemDoubleClickedEvent, source)
        {
            Item = item;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the item that was double clicked
        /// </summary>
        public object Item { get; private set; }
        #endregion
    }
}