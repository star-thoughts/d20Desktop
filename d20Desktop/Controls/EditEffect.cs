using Fiction.GameScreen.Combat;
using Fiction.GameScreen.ViewModels;
using System;
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
    /// Control for editing an effect
    /// </summary>
    public class EditEffect : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="EditEffect"/> class
        /// </summary>
        static EditEffect()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EditEffect), new FrameworkPropertyMetadata(typeof(EditEffect)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the view model
        /// </summary>
        public EditEffectViewModel ViewModel
        {
            get { return (EditEffectViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="ViewModel"/>
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(EditEffectViewModel), typeof(EditEffect));
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            CommandBindings.Add(new CommandBinding(Commands.Add, Add_Executed, Add_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Remove, Remove_Executed, Remove_CanExecute));

            Button button = Template.FindName("PART_PickRelated", this) as Button;
            if (button != null)
                button.Click += PickRelated_Clicked;
        }

        private void PickRelated_Clicked(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                IEnumerable<IFilterable> items = ViewModel.Campaign.Spells.Spells.OfType<IFilterable>().Concat(ViewModel.Campaign.EquipmentManager.MagicItems).ToArray();
                SelectCampaignObjectViewModel vm = new SelectCampaignObjectViewModel(items, false);
                EditWindow window = new EditWindow();
                window.Owner = Window.GetWindow(this);
                window.DataContext = vm;

                if (window.ShowDialog() == true)
                    ViewModel.RelatedItem = vm.SelectedItems.FirstOrDefault() as ICampaignObject;
            }
        }

        private void Add_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (ViewModel != null)
                {
                    SelectCombatantsViewModel vm = new SelectCombatantsViewModel(ViewModel.Combat, multiSelect: true);
                    EditWindow window = new EditWindow();
                    window.Owner = Window.GetWindow(this);
                    window.DataContext = vm;

                    if (window.ShowDialog() == true)
                        ViewModel.Targets.Append(vm.SelectedCombatants);
                }
            });
        }

        private void Add_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = ViewModel != null;
        }

        private void Remove_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (ViewModel != null && e.Parameter is ICombatant combatant)
                    ViewModel.Targets.Remove(combatant);
            });
        }

        private void Remove_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = ViewModel != null && e.Parameter is ICombatant;
        }
        #endregion
    }
}
