using Fiction.GameScreen.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Control used to choose a source for a combatant template
    /// </summary>
    public sealed class ChooseCombatantTemplateSource : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="ChooseCombatantTemplateSource"/> class
        /// </summary>
        static ChooseCombatantTemplateSource()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChooseCombatantTemplateSource), new FrameworkPropertyMetadata(typeof(ChooseCombatantTemplateSource)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the view model for this view
        /// </summary>
        public ChooseCombatantTemplateSourceViewModel ViewModel
        {
            get { return (ChooseCombatantTemplateSourceViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="ViewModel"/>
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ChooseCombatantTemplateSourceViewModel), typeof(ChooseCombatantTemplateSource));
        #endregion
        #region Methods
        #endregion
    }
}
