using Fiction.GameScreen.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Control for editing spell levels
    /// </summary>
    public sealed class EditSpellLevels : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="EditSpellLevels"/> class
        /// </summary>
        static EditSpellLevels()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EditSpellLevels), new FrameworkPropertyMetadata(typeof(EditSpellLevels)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the view model
        /// </summary>
        public EditSpellLevelsViewModel ViewModel
        {
            get { return (EditSpellLevelsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="ViewModel"/>
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(EditSpellLevelsViewModel), typeof(EditSpellLevels));
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            Button button = Template.FindName("PART_AddButton", this) as Button;
            if (button != null)
                button.Click += AddButton_Click;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                ViewModel?.AddLevel();
            });
        }
        #endregion
    }
}
