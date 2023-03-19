using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fiction.Windows
{
    /// <summary>
    /// Object used to initialize special WPF windows handlers
    /// </summary>
    public static class CommonWindows
    {
        #region Constructors
        static CommonWindows()
        {
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.PreviewMouseLeftButtonDownEvent,
              new MouseButtonEventHandler(SelectivelyHandleMouseButton), true);
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.GotKeyboardFocusEvent,
              new RoutedEventHandler(SelectAllText), true);
        }
        #endregion
        #region Member Variables
        #endregion
        #region Properties
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for AutoSelectAll
        /// </summary>
        public static readonly DependencyProperty AutoSelectAllProperty = DependencyProperty.RegisterAttached("AutoSelectAll", typeof(bool), typeof(CommonWindows),
            new FrameworkPropertyMetadata(true));

        /// <summary>
        /// Sets whether or not to auto select all text in a TextBox when focus enters
        /// </summary>
        /// <param name="textBox">TextBox to set the property for</param>
        /// <param name="value">Whether or not to select all text</param>
        public static void SetAutoSelectAll(TextBox textBox, bool value)
        {
            Exceptions.ThrowIfArgumentNull(textBox, nameof(textBox));

            textBox.SetValue(AutoSelectAllProperty, value);
        }
        /// <summary>
        /// Gets whether or not to auto select all text in a TextBox when focus enters
        /// </summary>
        /// <param name="textBox">TextBox to set the property for</param>
        /// <returns>Whether or not to select all text</returns>
        public static bool GetAutoSelectAll(TextBox textBox)
        {
            Exceptions.ThrowIfArgumentNull(textBox, nameof(textBox));

            return (bool)textBox.GetValue(AutoSelectAllProperty);
        }
        #endregion
        #region Methods
        private static void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                Exceptions.RaiseIgnoredException(e.Exception);
            }
            catch (Exception exc)
            {
                Exceptions.RaiseIgnoredException(exc);
            }
        }

        public static void Initialize()
        {
            Application.Current.DispatcherUnhandledException += Dispatcher_UnhandledException;
        }
        #endregion
        #region Event Handlers
        private static void SelectAllText(object sender, RoutedEventArgs e)
        {
            if (GetAutoSelectAll((TextBox)sender))
            {
                TextBox? textBox = e.OriginalSource as TextBox;
                if (textBox != null)
                    textBox.SelectAll();
            }
        }

        private static void SelectivelyHandleMouseButton(object sender, MouseButtonEventArgs e)
        {
            TextBox? textbox = sender as TextBox;
            if (GetAutoSelectAll((TextBox)sender))
            {
                if (textbox != null && !textbox.IsKeyboardFocusWithin)
                {
                    if (e.OriginalSource.GetType().Name == "TextBoxView")
                    {
                        e.Handled = true;
                        textbox.Focus();
                    }
                }
            }
        }
        #endregion
    }
}
