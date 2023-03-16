using Fiction.GameScreen.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Editor for magic items
    /// </summary>
    public sealed class MagicItemEditor : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="MagicItemEditor"/> class
        /// </summary>
        static MagicItemEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MagicItemEditor), new FrameworkPropertyMetadata(typeof(MagicItemEditor)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the view model for editing magic items
        /// </summary>
        public EditMagicItemViewModel ViewModel
        {
            get { return (EditMagicItemViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="ViewModel"/>
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(EditMagicItemViewModel), typeof(MagicItemEditor));
        #endregion
    }
}
