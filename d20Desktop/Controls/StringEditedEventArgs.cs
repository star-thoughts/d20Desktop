using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Event handler for editing or removing a string
    /// </summary>
    /// <param name="sender">Control that caught the event</param>
    /// <param name="e">Event arguments</param>
    public delegate void StringEditedEventHandler(object sender, StringEditedEventArgs e);
    /// <summary>
    /// Event args for a string being edited or removed from a string list
    /// </summary>
    public sealed class StringEditedEventArgs : RoutedEventArgs
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="StringEditedEventArgs"/> for an added or removed string
        /// </summary>
        /// <param name="source">Source of the event</param>
        /// <param name="eventType">Type of editing that occurred</param>
        /// <param name="string">String that was edited</param>
        public StringEditedEventArgs(object source, StringEditEventType eventType, string @string)
            : base(StringListEditor.StringEditedEvent, source)
        {
            if (eventType == StringEditEventType.Changed)
                throw new InvalidOperationException("This constructor can only be called for adds or removes.");

            String = @string;
            EventType = eventType;
        }
        /// <summary>
        /// Constructs a new <see cref="StringEditedEventArgs"/> for a changed string
        /// </summary>
        /// <param name="source">Source of the event</param>
        /// <param name="oldValue">Original value of the string</param>
        /// <param name="newValue">New value for the string</param>
        public StringEditedEventArgs(object source, string oldValue, string newValue)
            : base(StringListEditor.StringEditedEvent, source)
        {
            String = oldValue;
            NewValue = newValue;
            EventType = StringEditEventType.Changed;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the string associated with this event
        /// </summary>
        public string String { get; private set; }
        /// <summary>
        /// Gets the new value of the string if it is edited
        /// </summary>
        public string NewValue { get; private set; }
        /// <summary>
        /// Gets the type of event that occurred
        /// </summary>
        public StringEditEventType EventType { get; private set; }
        #endregion
    }
}
