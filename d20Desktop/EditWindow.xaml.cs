using Fiction.GameScreen.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Fiction.GameScreen
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="EditWindow"/>
        /// </summary>
        public EditWindow()
        {
            Loaded += EditWindow_Loaded;
            InitializeComponent();
        }
        #endregion
        #region Methods
        private void OkCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = DataContext is IViewModelCore core && core.IsValid;
        }

        private void OkCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            if (DataContext is IViewModelCore core && core.IsValid)
                DialogResult = true;
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = true;
        }

        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            DialogResult = false;
        }

        private void ViewObjectCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = e.Parameter != null;
        }

        private void ViewObjectCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            if (e.Parameter != null)
            {
                ObjectViewerWindow window = new ObjectViewerWindow(e.Parameter);
                window.Owner = this;
                window.ShowDialog();
            }
        }

        private void EditWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //  Move focus to first item
            MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
        #endregion
    }
}
