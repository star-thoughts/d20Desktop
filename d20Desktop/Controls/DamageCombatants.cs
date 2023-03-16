using Fiction.GameScreen.Combat;
using Fiction.GameScreen.ViewModels;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// View to damage combatants during combat
    /// </summary>
    public sealed class DamageCombatants : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="DamageCombatants"/> class
        /// </summary>
        static DamageCombatants()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DamageCombatants), new FrameworkPropertyMetadata(typeof(DamageCombatants)));
        }
        #endregion
        #region Member Variables
        private System.Windows.Controls.Primitives.Selector _damageModifierSelector;
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the damage information
        /// </summary>
        public DamageCombatantViewModel ViewModel
        {
            get { return (DamageCombatantViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        /// <summary>
        /// Gets a collection of damage modifiers
        /// </summary>
        public static IEnumerable DamageModifiers
        {
            get
            {
                return new[]
                {
                    new { Display = GameScreen.Resources.Resources.StandardDamageLabel, Value = DamageModifier.Normal },
                    new { Display = GameScreen.Resources.Resources.NoDamageLabel, Value = DamageModifier.None },
                    new { Display = GameScreen.Resources.Resources.HalfDamageLabel, Value = DamageModifier.Half },
                    new { Display = GameScreen.Resources.Resources.QuarterDamageLabel, Value = DamageModifier.Quarter },
                    new { Display = GameScreen.Resources.Resources.HalfAgainDamageLabel, Value = DamageModifier.ExtraHalf },
                    new { Display = GameScreen.Resources.Resources.DoubleDamageLabel, Value = DamageModifier.Double },
                };
            }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="ViewModel"/>
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(DamageCombatantViewModel), typeof(DamageCombatants),
            new FrameworkPropertyMetadata(null, ViewModelChanged));

        private static void ViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DamageCombatants view)
            {
                DamageCombatantViewModel damage = e.NewValue as DamageCombatantViewModel;
                view.UpdateDamageInformation(damage);
            }
        }
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            System.Windows.Controls.Primitives.Selector selector = Template.FindName("PART_CombatantList", this) as System.Windows.Controls.Primitives.Selector;
            if (selector != null)
                selector.SelectionChanged += Selector_SelectionChanged;

            _damageModifierSelector = Template.FindName("PART_DamageModifier", this) as System.Windows.Controls.Primitives.Selector;
            _damageModifierSelector.SelectionChanged += _damageModifierSelector_SelectionChanged;

            if (ViewModel != null)
                UpdateDamageInformation(ViewModel);
        }

        private void _damageModifierSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic item = e.AddedItems.Cast<object>().FirstOrDefault();
            if (item.Value is IDamageModifiersViewModel vm)
                vm?.Apply();
        }

        private void Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (e.AddedItems != null)
                {
                    foreach (ICombatant combatant in e.AddedItems)
                    {
                        ViewModel.Damage.Combatants.Add(new CombatantDamageInformation(combatant, ViewModel.Damage.DamageTypes));
                    }
                }
                if (e.RemovedItems != null)
                {
                    foreach (ICombatant combatant in e.RemovedItems)
                    {
                        CombatantDamageInformation damage = ViewModel.Damage.Combatants.FirstOrDefault(p => ReferenceEquals(p.Combatant, combatant));
                        if (damage != null)
                            ViewModel.Damage.Combatants.Remove(damage);
                    }
                }
            });
        }

        private void UpdateDamageInformation(DamageCombatantViewModel damage)
        {
            if (_damageModifierSelector != null)
            {
                _damageModifierSelector.Items.Add(new { Value = new BypassDamageModifiersViewModel(damage), Display = GameScreen.Resources.Resources.BypassDamageReduction });
                _damageModifierSelector.Items.Add(new { Value = new ApplyDamageReductionViewModel(damage), Display = GameScreen.Resources.Resources.ApplyDamageReduction });
                _damageModifierSelector.SelectedIndex = 0;
            }
        }
        #endregion
    }
}
