using Fiction.GameScreen.Combat;
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
    /// Control for selecting a combat scenario
    /// </summary>
    public sealed class SelectCombatScenario : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="SelectCombatScenario"/> class
        /// </summary>
        static SelectCombatScenario()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SelectCombatScenario), new FrameworkPropertyMetadata(typeof(SelectCombatScenario)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the scenarios in the campaign
        /// </summary>
        public CombatScenariosViewModel Scenarios
        {
            get { return (CombatScenariosViewModel)GetValue(ScenariosProperty); }
            set { SetValue(ScenariosProperty, value); }
        }
        /// <summary>
        /// Gets or sets the selected scenario
        /// </summary>
        public CombatScenario SelectedScenario
        {
            get { return (CombatScenario)GetValue(SelectedScenarioProperty); }
            set { SetValue(SelectedScenarioProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="ScenariosProperty"/>
        /// </summary>
        public static readonly DependencyProperty ScenariosProperty = DependencyProperty.Register(nameof(Scenarios), typeof(CombatScenariosViewModel), typeof(SelectCombatScenario));
        /// <summary>
        /// DependencyProperty for <see cref="SelectedScenario"/>
        /// </summary>
        public static readonly DependencyProperty SelectedScenarioProperty = DependencyProperty.Register(nameof(SelectedScenario), typeof(CombatScenario), typeof(SelectCombatScenario),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion
    }
}
