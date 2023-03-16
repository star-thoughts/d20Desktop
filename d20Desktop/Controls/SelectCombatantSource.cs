using Fiction.GameScreen.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Control to select source for adding combatants to combat
    /// </summary>
    public sealed class SelectCombatantSource : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="SelectCombatantSource"/> class
        /// </summary>
        static SelectCombatantSource()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SelectCombatantSource), new FrameworkPropertyMetadata(typeof(SelectCombatantSource)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the source of combatants selected
        /// </summary>
        public SelectCombatantSourceViewModel ViewModel
        {
            get { return (SelectCombatantSourceViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="ViewModel"/>
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(SelectCombatantSourceViewModel), typeof(SelectCombatantSource));
        #endregion
    }
}
