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
    /// Provides a view of a combat scenario's settings
    /// </summary>
    public sealed class CombatScenarioView : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="CombatScenarioView"/> class
        /// </summary>
        static CombatScenarioView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CombatScenarioView), new FrameworkPropertyMetadata(typeof(CombatScenarioView)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the combat scenario to view
        /// </summary>
        public CombatScenario Scenario
        {
            get { return (CombatScenario)GetValue(ScenarioProperty); }
            set { SetValue(ScenarioProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="ScenarioProperty"/>
        /// </summary>
        public static readonly DependencyProperty ScenarioProperty = DependencyProperty.Register(nameof(Scenario), typeof(CombatScenario), typeof(CombatScenarioView));
        #endregion
    }
}
