using Fiction.GameScreen.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Editor used for adding/editing/copying spells
    /// </summary>
    public sealed class SpellEditor : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="SpellEditor"/> class
        /// </summary>
        static SpellEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SpellEditor), new FrameworkPropertyMetadata(typeof(SpellEditor)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the view model for this control
        /// </summary>
        public EditSpellViewModel ViewModel
        {
            get { return (EditSpellViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="ViewModel"/>
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(EditSpellViewModel), typeof(SpellEditor));
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            Button? button = Template.FindName("PART_EditButton", this) as Button;
            if (button != null)
                button.Click += EditLevels_Click;
        }

        private void EditLevels_Click(object sender, RoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                EditSpellLevelsViewModel levels = new EditSpellLevelsViewModel(ViewModel.Campaign, ViewModel.SpellLevels);
                EditWindow window = new EditWindow();
                window.Owner = Window.GetWindow(this);
                window.DataContext = levels;
                window.Width /= 2;

                if (window.ShowDialog() == true)
                    levels.Save();
            });
        }
        #endregion
    }
}
