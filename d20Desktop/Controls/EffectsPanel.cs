using Fiction.GameScreen.Combat;
using Fiction.GameScreen.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Control for displaying and managing effects during combat
    /// </summary>
    public class EffectsPanel : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="EffectsPanel"/> class
        /// </summary>
        static EffectsPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EffectsPanel), new FrameworkPropertyMetadata(typeof(EffectsPanel)));
        }
        #endregion
        #region Member Variables
        private CollectionViewSource? _effectsCollection;
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the current combatant
        /// </summary>
        public ICombatant Combatant
        {
            get { return (ICombatant)GetValue(CombatantProperty); }
            set { SetValue(CombatantProperty, value); }
        }
        /// <summary>
        /// Gets or sets the active combat
        /// </summary>
        public Combat.ActiveCombat Combat
        {
            get { return (Combat.ActiveCombat)GetValue(CombatProperty); }
            set { SetValue(CombatProperty, value);}
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="Combatant"/>
        /// </summary>
        public static readonly DependencyProperty CombatantProperty = DependencyProperty.Register(nameof(Combatant), typeof(ICombatant), typeof(EffectsPanel),
            new FrameworkPropertyMetadata(null, CombatantChanged));
        /// <summary>
        /// DependencyProperty for <see cref="Combat"/>
        /// </summary>
        public static readonly DependencyProperty CombatProperty = DependencyProperty.Register(nameof(Combat), typeof(Combat.ActiveCombat), typeof(EffectsPanel),
            new FrameworkPropertyMetadata(null, CombatChanged));

        private static void CombatantChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                (d as EffectsPanel)?.RefreshCollections();
            });
        }

        private static void CombatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (d is EffectsPanel view)
                    view._effectsCollection?.View?.Refresh();
            });
        }
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Panel? panel = Template.FindName("PART_RootGrid", this) as Panel;

            _effectsCollection = panel?.Resources["EffectsCollection"] as CollectionViewSource;
            if (_effectsCollection != null)
                _effectsCollection.Filter += EffectsCollection_Filter;

            CommandBindings.Add(new CommandBinding(Commands.Add, AddEffect_Executed, AddEffect_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Remove, RemoveEffect_Executed, RemoveEffect_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.RemoveTarget, RemoveTarget_Executed, RemoveTarget_CanExecute));
        }

        private void RemoveTarget_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (Combatant != null && e.Parameter is Effect effect)
                {
                    effect.Targets.Remove(Combatant);
                    RefreshCollections();
                }
            });
        }

        private void RefreshCollections()
        {
            _effectsCollection?.View?.Refresh();
        }

        private void RemoveTarget_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                e.CanExecute = e.Parameter is Effect && Combatant != null;
            });
        }

        private void RemoveEffect_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (e.Parameter is Effect effect)
                    Combat.Effects.Remove(effect);
                RefreshCollections();
            });
        }

        private void RemoveEffect_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Combat != null && e.Parameter is Effect;
        }

        private void AddEffect_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (Combatant.Campaign != null)
                {
                    EditEffectViewModel vm = new EditEffectViewModel(Combatant.Campaign);
                    vm.Source = Combatant;
                    vm.InitiativeSource = Combatant;

                    EditWindow window = new EditWindow();
                    window.Owner = Window.GetWindow(this);
                    window.DataContext = vm;

                    if (window.ShowDialog() == true)
                        Combat.Effects.Add(vm.Save());
                    RefreshCollections();
                }
            });
        }

        private void AddEffect_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Combat != null;
        }

        private void EffectsCollection_Filter(object sender, FilterEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (e.Item is Effect effect)
                    e.Accepted = ReferenceEquals(Combatant, effect.Source) || effect.Targets.Contains(Combatant);
                else
                    e.Accepted = false;
            });
        }
        #endregion
    }
}
