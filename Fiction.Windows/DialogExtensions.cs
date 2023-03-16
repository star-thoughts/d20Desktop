using System.Windows;

namespace Fiction.Windows
{
    /// <summary>
    /// Extensions for windows that allows it to act like a dialog
    /// </summary>
    public static class DialogExtensions
    {
        /// <summary>
        /// DependencyProperty for IsDialog
        /// </summary>
        public static readonly DependencyProperty IsDialogProperty =
            DependencyProperty.RegisterAttached("IsDialog", typeof(bool), typeof(DialogExtensions), new FrameworkPropertyMetadata(false, IsDialogChanged));

        /// <summary>
        /// Sets whether or not the given window is a dialog
        /// </summary>
        /// <param name="window">Window to set to a dialog</param>
        /// <param name="value">Whether or not to consider the window a dialog</param>
        public static void SetIsDialog(Window window, bool value)
        {
            Exceptions.ThrowIfArgumentNull(window, nameof(window));
            window.SetValue(IsDialogProperty, value);
        }

        /// <summary>
        /// Gets whether or not the window is considered a dialog
        /// </summary>
        /// <param name="window">Window to test for whether or not it is a dialog</param>
        /// <returns>Whether or not the window is a dialog</returns>
        public static bool GetIsDialog(Window window)
        {
            Exceptions.ThrowIfArgumentNull(window, nameof(window));
            return (bool)window.GetValue(IsDialogProperty);
        }

        private static void IsDialogChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Window? window = d as Window;
            if (window != null)
            {
                if ((bool)e.NewValue)
                {
                    window.Loaded += Window_Loaded;
                    window.ShowInTaskbar = false;
                }
                else
                {
                    window.Loaded -= Window_Loaded;
                }
            }
        }

        private static void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Window? window = sender as Window;
            if (window != null)
            {
                window.MoveFocus(new System.Windows.Input.TraversalRequest(System.Windows.Input.FocusNavigationDirection.Next));
            }
        }
    }
}
