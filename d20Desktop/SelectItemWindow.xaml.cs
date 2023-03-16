using System.Collections;
using System.Windows;
using System.Windows.Input;

namespace Fiction.GameScreen
{
    /// <summary>
    /// Interaction logic for SelectItemWindow.xaml
    /// </summary>
    public partial class SelectItemWindow : Window
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="SelectItemWindow"/>
        /// </summary>
        public SelectItemWindow()
        {
            InitializeComponent();
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the items to select from
        /// </summary>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        /// <summary>
        /// Gets or sets the item the user selected
        /// </summary>
        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        /// <summary>
        /// Gets or sets the member to display for each item
        /// </summary>
        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="ItemsSource"/>
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(SelectItemWindow));
        /// <summary>
        /// DependencyProperty for <see cref="SelectedItem"/>
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(SelectItemWindow));
        /// <summary>
        /// DependencyProperty for <see cref="DisplayMemberPath"/>
        /// </summary>
        public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register(nameof(DisplayMemberPath), typeof(string), typeof(SelectItemWindow));
        #endregion
        #region Methods
        private void OkCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = SelectedItem != null;
        }

        private void OkCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            DialogResult = SelectedItem != null;
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
        #endregion
    }
}
