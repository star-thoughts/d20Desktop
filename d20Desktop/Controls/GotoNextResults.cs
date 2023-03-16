using Fiction.GameScreen.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Control for displaying the results of going to the next combatant
    /// </summary>
    public sealed class GotoNextResults : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="GotoNextResults"/> class
        /// </summary>
        static GotoNextResults()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GotoNextResults), new FrameworkPropertyMetadata(typeof(GotoNextResults)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the results to display
        /// </summary>
        public GotoNextResultViewModel ViewModel
        {
            get { return (GotoNextResultViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="ViewModel"/>
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(GotoNextResultViewModel), typeof(GotoNextResults));
        #endregion
        #region Methods
        #endregion
    }
}
