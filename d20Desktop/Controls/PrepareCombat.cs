using Fiction.GameScreen.Combat;
using Fiction.GameScreen.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Initiates combat by gathering required combat data
    /// </summary>
    public sealed class PrepareCombat : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="PrepareCombat"/> class
        /// </summary>
        static PrepareCombat()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PrepareCombat), new FrameworkPropertyMetadata(typeof(PrepareCombat)));
        }
        #endregion
        #region Member Variables
        private ListBox _combatantList;
        private Panel _inputPanel;
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
        /// Gets or sets the view model for preparing combat
        /// </summary>
        public PrepareCombatViewModel Preparer
        {
            get { return (PrepareCombatViewModel)GetValue(PreparerProperty); }
            set { SetValue(PreparerProperty, value); }
        }
        /// <summary>
        /// Gets or sets the currently selected combatant
        /// </summary>
        public CombatantPreparer SelectedCombatant
        {
            get { return (CombatantPreparer)GetValue(SelectedCombatantProperty); }
            set { SetValue(SelectedCombatantProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="Factory"/>
        /// </summary>
        public static readonly DependencyProperty FactoryProperty = DependencyProperty.Register(nameof(Factory), typeof(IViewModelFactory), typeof(PrepareCombat));
        /// <summary>
        /// DependencyProperty for <see cref="Preparer"/>
        /// </summary>
        public static readonly DependencyProperty PreparerProperty = DependencyProperty.Register(nameof(Preparer), typeof(PrepareCombatViewModel), typeof(PrepareCombat),
            new FrameworkPropertyMetadata(null, PreparerChanged));
        /// <summary>
        /// DependencyProperty for <see cref="SelectedCombatant"/>
        /// </summary>
        public static readonly DependencyProperty SelectedCombatantProperty = DependencyProperty.Register(nameof(SelectedCombatant), typeof(CombatantPreparer), typeof(PrepareCombat));

        private static void PreparerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
        #endregion
        #region Methods
        public override async void OnApplyTemplate()
        {
            CommandBindings.Add(new CommandBinding(Commands.Add, Add_Executed, Add_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Remove, Remove_Executed, Remove_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Roll, Roll_Executed, Roll_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Reset, Reset_Executed, Reset_CanExecute));

            _combatantList = Template.FindName("PART_CombatantList", this) as ListBox;

            Loaded += PrepareCombat_Loaded;

            _inputPanel = Template.FindName("PART_InformationPanel", this) as Panel;

            //  This isn't loaded until a combatant becomes selected, so wait for it to happen first
            await Dispatcher.InvokeAsync(() =>
            {
                if (_inputPanel != null)
                {
                    foreach (TextBox textBox in VisualTreeHelperEx.GetAllChildren<TextBox>(_inputPanel))
                        textBox.PreviewKeyDown += TextBox_PreviewKeyDown;
                }
            });
            await DispatchHighlightTextBox(null);
        }

        private async void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            await Exceptions.FailSafeMethodCall(async () =>
             {
                 switch (e.Key)
                 {
                     case Key.Up:
                         e.Handled = true;
                         await MoveUp(sender as TextBox);
                         break;
                     case Key.Down:
                         e.Handled = true;
                         await MoveDown(sender as TextBox);
                         break;
                 }
             });
        }

        private async Task MoveUp(TextBox textBox)
        {
            if (_combatantList != null && !ReferenceEquals(SelectedCombatant, _combatantList.Items.OfType<object>().FirstOrDefault()))
            {
                SelectedCombatant = _combatantList.Items.OfType<CombatantPreparer>().PrevOrDefault(SelectedCombatant);
                await DispatchHighlightTextBox(textBox);
            }
        }

        private async Task DispatchHighlightTextBox(TextBox textBox)
        {
            await Dispatcher.InvokeAsync(() => HighlightTextBox(textBox), DispatcherPriority.Loaded);
        }

        private void HighlightTextBox(TextBox textBox)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (textBox == null && _inputPanel != null)
                    textBox = VisualTreeHelperEx.GetAllChildren<TextBox>(_inputPanel).FirstOrDefault(p => p.Name == "PART_InitiativeTotal");

                textBox.Focus();
                textBox.SelectAll();
            });
        }

        private async Task MoveDown(TextBox textBox)
        {
            if (_combatantList != null && !ReferenceEquals(SelectedCombatant, _combatantList.Items.OfType<object>().LastOrDefault()))
            {
                SelectedCombatant = _combatantList.Items.OfType<CombatantPreparer>().AfterOrDefault(SelectedCombatant);
                await DispatchHighlightTextBox(textBox);
            }
        }

        private void Reset_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (e.Parameter is PrepareCombatViewModel vm)
                {
                    vm.Reset();
                    _combatantList.SelectedIndex = 0;
                }
            });
        }

        private void Reset_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = e.Parameter is PrepareCombatViewModel;
        }

        private void Roll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (SelectedCombatant != null && Dice.IsValidString(SelectedCombatant.HitDieString))
                    SelectedCombatant.RollHitPoints();
            });
        }

        private void Roll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = SelectedCombatant != null && Dice.IsValidString(SelectedCombatant.HitDieString);
        }

        private async void PrepareCombat_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedCombatant = Preparer?.Combatants.FirstOrDefault();
        }

        private void EndSession(PrepareCombatViewModel preparer)
        {
        }

        private void Remove_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (SelectedCombatant != null)
                {
                    e.Handled = true;
                    CombatantPreparer next = Preparer.Combatants.AfterOrDefault(SelectedCombatant);
                    Preparer.Combatants.Remove(SelectedCombatant);
                    if (!ReferenceEquals(next, SelectedCombatant))
                        SelectedCombatant = next;
                }
            });
        }

        private void Remove_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = SelectedCombatant != null;
        }

        private async void Add_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            await Exceptions.FailSafeMethodCall(async () =>
             {
                 if (Preparer != null)
                 {
                     e.Handled = true;
                     SelectCombatantSourceViewModel vm = new SelectCombatantSourceViewModel(Factory);
                     EditWindow window = new EditWindow();
                     window.Owner = Window.GetWindow(this);
                     window.DataContext = vm;

                     if (window.ShowDialog() == true)
                     {
                         CombatantPreparer first = Preparer.Preparer.AddCombatants(vm.SelectedSource).FirstOrDefault();

                         if (first != null)
                         {
                             SelectedCombatant = first;

                             await DispatchHighlightTextBox(null);
                         }
                     }
                 }
             });
        }

        private void Add_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Preparer != null;
        }

        #endregion
    }
}
