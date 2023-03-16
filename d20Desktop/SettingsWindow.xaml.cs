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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="SettingsWindow"/>
        /// </summary>
        public SettingsWindow()
        {
            InitializeComponent();
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the view model that is being edited
        /// </summary>
        public CampaignViewModelCore ViewModel
        {
            get { return (CampaignViewModelCore)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="ViewModel"/>
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(CampaignViewModelCore), typeof(SettingsWindow));
        #endregion

        #region Methods
        private void OkCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = ViewModel != null && ViewModel.IsValid;
        }

        private void OkCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }
        #endregion
    }
}
