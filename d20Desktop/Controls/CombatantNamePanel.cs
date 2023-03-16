using Fiction.GameScreen.Combat;
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
    /// Control that displays a combatant's name in a consistant way
    /// </summary>
    public class CombatantNamePanel : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="CombatantNamePanel"/> class
        /// </summary>
        static CombatantNamePanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CombatantNamePanel), new FrameworkPropertyMetadata(typeof(CombatantNamePanel)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the combatant to display
        /// </summary>
        public ICombatantBase Combatant
        {
            get { return (ICombatantBase)GetValue(CombatantProperty); }
            set { SetValue(CombatantProperty, value);}
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="Combatant"/>
        /// </summary>
        public static readonly DependencyProperty CombatantProperty = DependencyProperty.Register(nameof(Combatant), typeof(ICombatantBase), typeof(CombatantNamePanel));
        #endregion
    }
}
