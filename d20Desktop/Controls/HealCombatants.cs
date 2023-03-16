using Fiction.GameScreen.Combat;
using Fiction.GameScreen.ViewModels;
using Fiction.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Shows options for healing combatants
    /// </summary>
    public sealed class HealCombatants : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="HealCombatants"/> class
        /// </summary>
        static HealCombatants()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HealCombatants), new FrameworkPropertyMetadata(typeof(HealCombatants)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the view model associated with this healing
        /// </summary>
        public HealCombatantsViewModel ViewModel
        {
            get { return (HealCombatantsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="ViewModel"/>
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(HealCombatantsViewModel), typeof(HealCombatants));
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            System.Windows.Controls.Primitives.Selector selector = Template.FindName("PART_CombatantList", this) as System.Windows.Controls.Primitives.Selector;
            if (selector != null)
                selector.SelectionChanged += Selector_SelectionChanged;
        }

        private void Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (e.AddedItems != null)
                {
                    foreach (ICombatant combatant in e.AddedItems)
                        ViewModel.Healing.Combatants.Add(new CombatantHealInformation(combatant));
                }
                if (e.RemovedItems != null)
                {
                    foreach (ICombatant combatant in e.RemovedItems)
                    {
                        CombatantHealInformation heal = ViewModel.Healing.Combatants.FirstOrDefault(p => ReferenceEquals(p.Combatant, combatant));
                        if (heal != null)
                            ViewModel.Healing.Combatants.Remove(heal);
                    }
                }
            });
        }
        #endregion
    }
}
