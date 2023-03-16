﻿using Fiction.GameScreen.Combat;
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
    /// Control used for managing combat scenarios
    /// </summary>
    public class CombatScenarios : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the CombatScenarios class
        /// </summary>
        static CombatScenarios()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CombatScenarios), new FrameworkPropertyMetadata(typeof(CombatScenarios)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the view model factory
        /// </summary>
        public IViewModelFactory Factory
        {
            get { return (IViewModelFactory)GetValue(FactoryProperty); }
            set { SetValue(FactoryProperty, value); }
        }
        /// <summary>
        /// Gets or sets the combat scenarios to edit
        /// </summary>
        public IEnumerable Scenarios
        {
            get { return (IEnumerable)GetValue(ScenariosProperty); }
            set { SetValue(ScenariosProperty, value); }
        }
        /// <summary>
        /// Gets or sets the currently selected combat scenario
        /// </summary>
        public CombatScenario SelectedScenario
        {
            get { return (CombatScenario)GetValue(SelectedScenarioProperty); }
            set { SetValue(SelectedScenarioProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="Factory"/>
        /// </summary>
        public static readonly DependencyProperty FactoryProperty = DependencyProperty.Register(nameof(Factory), typeof(IViewModelFactory), typeof(CombatScenarios));
        /// <summary>
        /// DependencyProperty for <see cref="Scenarios"/>
        /// </summary>
        public static readonly DependencyProperty ScenariosProperty = DependencyProperty.Register(nameof(Scenarios), typeof(IEnumerable), typeof(CombatScenarios));
        /// <summary>
        /// DependencyProperty for <see cref="SelectedScenario"/>
        /// </summary>
        public static readonly DependencyProperty SelectedScenarioProperty = DependencyProperty.Register(nameof(SelectedScenario), typeof(CombatScenario), typeof(CombatScenario));
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            CommandBindings.Add(new CommandBinding(Commands.Add, AddCommand_Executed, AddCommand_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Remove, RemoveCommand_Executed, RemoveCommand_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Edit, EditCommand_Executed, EditCommand_CanExecute));

            ListBox scenarioList = Template.FindName("PART_ScenarioList", this) as ListBox;
            Style containerStyle = scenarioList?.GetOrCreateItemContainerStyle<ListBoxItem>();
            if (containerStyle != null)
                containerStyle.Setters.Add(new EventSetter(Control.MouseDoubleClickEvent, new MouseButtonEventHandler(Scenario_DoubleClick)));
        }

        private void Scenario_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem item)
            {
                if (item.DataContext is CombatScenario scenario)
                    EditScenario(scenario);
            }
        }

        private void EditCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                CombatScenarioEditViewModel viewModel = new CombatScenarioEditViewModel(Factory, SelectedScenario);
                if (EditScenario(viewModel))
                    viewModel.Save();
            });
        }

        private void EditCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = SelectedScenario != null;
        }

        private void RemoveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (SelectedScenario != null && Scenarios is IList scenarios)
                {
                    if (SelectedScenario.Campaign.Combat.CanDeleteScenario(SelectedScenario))
                    {
                        scenarios.Remove(SelectedScenario);
                    }
                    else
                    {
                        string message = GameScreen.Resources.Resources.CannotDeleteCombatScenarioMessage;
                        Window window = Window.GetWindow(this);
                        MessageBox.Show(window, message, window.Title, MessageBoxButton.OK);
                    }
                }
            });
        }

        private void RemoveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = SelectedScenario != null && Scenarios is IList;
        }

        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (Scenarios is IList scenarios)
                {
                    CombatScenarioEditViewModel viewModel = new CombatScenarioEditViewModel(Factory, new CombatScenario(Factory.Campaign));
                    if (EditScenario(viewModel))
                        scenarios.Add(viewModel.Save());
                }
            });
        }

        private bool EditScenario(CombatScenario scenario)
        {
            CombatScenarioEditViewModel viewModel = new CombatScenarioEditViewModel(Factory, scenario);
            if (EditScenario(viewModel))
            {
                viewModel.Save();
                return true;
            }
            return false;
        }

        private bool EditScenario(CombatScenarioEditViewModel viewModel)
        {
            EditWindow window = new EditWindow();
            window.Owner = Window.GetWindow(this);
            window.DataContext = viewModel;

            return window.ShowDialog() == true;
        }

        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Scenarios is IList;
        }
        #endregion
    }
}
