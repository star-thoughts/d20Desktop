using Fiction.GameScreen.Combat;
using Fiction.GameScreen.ViewModels;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Control for viewing an active combat
    /// </summary>
    public sealed class ActiveCombat : Control
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="ActiveCombat"/>
        /// </summary>
        public ActiveCombat()
        {
        }

        /// <summary>
        /// Initializer the <see cref="ActiveCombat"/> class
        /// </summary>
        static ActiveCombat()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ActiveCombat), new FrameworkPropertyMetadata(typeof(ActiveCombat)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the combat
        /// </summary>
        public ActiveCombatViewModel Combat
        {
            get { return (ActiveCombatViewModel)GetValue(CombatProperty); }
            set { SetValue(CombatProperty, value); }
        }
        /// <summary>
        /// Gets or sets the currently selected combatant
        /// </summary>
        public ICombatant SelectedCombatant
        {
            get { return (ICombatant)GetValue(SelectedCombatantProperty); }
            set { SetValue(SelectedCombatantProperty, value); }
        }
        /// <summary>
        /// Gets whether or not this control is busy
        /// </summary>
        public bool IsIdle
        {
            get { return (bool)GetValue(IsIdleProperty); }
            set { SetValue(IsIdleProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="Combat"/>
        /// </summary>
        public static readonly DependencyProperty CombatProperty = DependencyProperty.Register(nameof(Combat), typeof(ActiveCombatViewModel), typeof(ActiveCombat),
            new FrameworkPropertyMetadata(null, CombatChanged));
        /// <summary>
        /// DependencyProperty for <see cref="SelectedCombatant"/>
        /// </summary>
        public static readonly DependencyProperty SelectedCombatantProperty = DependencyProperty.Register(nameof(SelectedCombatant), typeof(ICombatant), typeof(ActiveCombat));
        /// <summary>
        /// DependencyProperty for <see cref="IsIdle"/>
        /// </summary>
        public static readonly DependencyProperty IsIdleProperty = DependencyProperty.Register(nameof(IsIdle), typeof(bool), typeof(ActiveCombat),
            new FrameworkPropertyMetadata(true));

        private static void CombatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            CommandBindings.Add(new CommandBinding(Commands.NextCombatant, NextCombatant_Executed, NextCombatant_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.DamageCombatants, DamageCombatants_Executed, DamageCombatants_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.HealCombatants, HealCombatants_Executed, HealCombatants_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.ManageCombatants, ManageCombatants_Executed, ManageCombatants_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.MoveBefore, MoveBefore_Executed, MoveBefore_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.MoveAfter, MoveAfter_Executed, MoveAfter_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Undo, Undo_Executed, Undo_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.CreateEffect, CreateEffect_Executed, CreateEffect_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Edit, Edit_Executed, Edit_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.AddCondition, AddCondition_Executed, AddCondition_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.RemoveCondition, RemoveCondition_Executed, RemoveCondition_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.KillCombatant, KillCombatant_Executed, KillCombatant_CanExecute));
        }

        private void KillCombatant_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (Combat != null
                    && e.Parameter is ICombatant combatant)
                {
                    combatant.Health.Kill();
                    combatant.IncludeInCombat = false;
                }
            });
        }

        private void KillCombatant_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Combat != null && e.Parameter is ICombatant;
        }

        private void RemoveCondition_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (SelectedCombatant != null
                    && e.Parameter is AppliedCondition condition)
                {
                    SelectedCombatant.Conditions.Remove(condition);
                }
            });
        }

        private void RemoveCondition_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = e.Parameter is AppliedCondition && SelectedCombatant != null;
        }

        private void AddCondition_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (e.Parameter is Combat.Condition condition
                    && SelectedCombatant != null)
                {
                    SelectedCombatant.Conditions.Add(new AppliedCondition(condition));
                }
            });
        }

        private void AddCondition_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = SelectedCombatant != null && e.Parameter is Combat.Condition;
        }

        private void Edit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                switch (e.Parameter)
                {
                    case Effect effect:
                        EditEffect(effect);
                        break;
                }
            });
        }

        private void EditEffect(Effect effect)
        {
            EditEffectViewModel vm = new EditEffectViewModel(Combat.Factory.Campaign, effect);
            EditWindow window = new EditWindow();
            window.Owner = Window.GetWindow(this);
            window.DataContext = vm;

            if (window.ShowDialog() == true)
                vm.Save();
        }

        private void Edit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = e.Parameter is Effect;
        }

        private void CreateEffect_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (Combat != null && e.Parameter is ICombatant combatant)
                {
                    EditEffectViewModel vm = new EditEffectViewModel(Combat.Factory.Campaign);
                    EditWindow window = new EditWindow();
                    window.Owner = Window.GetWindow(this);
                    window.DataContext = vm;

                    //  Make it count down on the current combatant's initiative
                    vm.InitiativeSource = Combat.Combat.Current;
                    vm.Source = combatant;

                    if (window.ShowDialog() == true)
                        Combat.Combat.Effects.Add(vm.Save());
                }
            });
        }

        private void CreateEffect_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Combat != null && e.Parameter is ICombatant;
        }

        private void Undo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            if (Combat != null && Combat.Combat.CanRestore)
                Combat.Combat.Restore();
        }

        private void Undo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Combat != null && Combat.Combat.CanRestore;
        }

        private void MoveAfter_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;

                Combat.ActiveCombat combat = Combat.Combat;
                if (combat != null)
                {
                    IActiveCombatant? current = combat.Current;
                    if (e.Parameter is IActiveCombatant combatant && current != null && combat.CanMoveAfter(combatant, current))
                        Combat.Combat.MoveAfter(combatant, current);
                }
            });
        }

        private void MoveAfter_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;

            Combat.ActiveCombat combat = Combat.Combat;
            if (combat != null)
            {
                IActiveCombatant? current = combat.Current;
                e.CanExecute = e.Parameter is IActiveCombatant combatant && current != null && combat.CanMoveAfter(combatant, current);
            }
        }

        private void MoveBefore_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;

                Combat.ActiveCombat combat = Combat.Combat;
                if (combat != null)
                {
                    IActiveCombatant? current = combat.Current;
                    if (e.Parameter is IActiveCombatant combatant && current != null && combat.CanMoveBefore(combatant, current))
                        Combat.Combat.MoveBefore(combatant, current);
                }
            });
        }

        private void MoveBefore_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;

            Combat.ActiveCombat combat = Combat.Combat;
            if (combat != null)
            {
                IActiveCombatant? current = combat.Current;
                e.CanExecute = e.Parameter is IActiveCombatant combatant && current != null && combat.CanMoveBefore(combatant, current);
            }
        }

        private void ManageCombatants_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (Combat != null)
                {
                    ManageCombatantsViewModel manager = new ManageCombatantsViewModel(Combat);
                    EditWindow window = new EditWindow();
                    window.Owner = Window.GetWindow(this);
                    window.DataContext = manager;

                    //  It does all the work, don't need to worry about results
                    window.ShowDialog();
                }
            });
        }

        private void ManageCombatants_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Combat != null;
        }

        private void HealCombatants_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                HealCombatantsViewModel heal = new HealCombatantsViewModel(Combat);
                EditWindow window = new EditWindow();
                window.Owner = Window.GetWindow(this);
                window.DataContext = heal;

                if (window.ShowDialog() == true)
                    heal.Apply();
            });
        }

        private void HealCombatants_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Combat != null;
        }

        private void DamageCombatants_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                DamageCombatantViewModel damage = new DamageCombatantViewModel(Combat);
                EditWindow window = new EditWindow();
                window.Owner = Window.GetWindow(this);
                window.DataContext = damage;

                if (window.ShowDialog() == true)
                {
                    Combat.Combat.ApplyDamage(damage.Damage);
                }
            });
        }

        private void DamageCombatants_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Combat != null;
        }

        private void NextCombatant_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Combat != null;
        }

        private async void NextCombatant_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            await Exceptions.FailSafeMethodCall(async () =>
                {
                    e.Handled = true;
                    if (Combat != null)
                    {
                        GotoNextResult result = await Combat.GotoNext();
                        HandleGotoNextResult(result);
                    }
                });
        }

        private void HandleGotoNextResult(GotoNextResult result)
        {
            //  If any of them had expired effects, then show the effects
            GotoNextResultViewModel vm = new GotoNextResultViewModel(result);
            if (vm.ExpiredEffects.Any())
            {
                FeedbackWindow window = new FeedbackWindow();
                window.Owner = Window.GetWindow(this);
                window.DataContext = vm;

                window.ShowDialog();
            }
        }
        #endregion
        #region Filter
        /// <summary>
        /// Gets a filter used to filter out combatants who are marked as inactive (see <see cref="ICombatant.IncludeInCombat"/>
        /// </summary>
        public static FilterEventHandler FilterOutInactiveCombatants { get { return new FilterEventHandler(FilterOutInactiveCombatantsMethod); } }
        private static void FilterOutInactiveCombatantsMethod(object sender, FilterEventArgs e)
        {
            if (e.Item is ICombatant combatant)
                e.Accepted = combatant.IncludeInCombat;
        }
        #endregion
    }
}
