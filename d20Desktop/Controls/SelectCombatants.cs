using Fiction.GameScreen.Combat;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Control for selecting a combatant
    /// </summary>
    public sealed class SelectCombatants : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="SelectCombatants"/> class
        /// </summary>
        static SelectCombatants()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SelectCombatants), new FrameworkPropertyMetadata(typeof(SelectCombatants)));
        }
        #endregion
        #region Member Variables
        private ListBox _combatantList;
        private bool _updatingSelection;
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the combatants to choose from
        /// </summary>
        public IEnumerable<ICombatant> Combatants
        {
            get { return (IEnumerable<ICombatant>)GetValue(CombatantsProperty); }
            set { SetValue(CombatantsProperty, value); }
        }
        /// <summary>
        /// Gets or sets the selected combatants
        /// </summary>
        public ObservableCollection<ICombatant> SelectedCombatants
        {
            get { return (ObservableCollection<ICombatant>)GetValue(SelectedCombatantsProperty); }
            set { SetValue(SelectedCombatantsProperty, value); }
        }
        /// <summary>
        /// Gets or sets whether or not to include player characters in the list
        /// </summary>
        public bool IncludePlayers
        {
            get { return (bool)GetValue(IncludePlayersProperty); }
            set { SetValue(IncludePlayersProperty, value); }
        }
        /// <summary>
        /// Gets or sets whether or not to include non-player characters in the list
        /// </summary>
        public bool IncludeNonPlayers
        {
            get { return (bool)GetValue(IncludeNonPlayersProperty); }
            set { SetValue(IncludeNonPlayersProperty, value); }
        }
        /// <summary>
        /// Gets or sets whether or not to allow the user to select multiple combatants
        /// </summary>
        public bool MultiSelect
        {
            get { return (bool)GetValue(MultiSelectProperty); }
            set { SetValue(MultiSelectProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="Combatants"/>
        /// </summary>
        public static readonly DependencyProperty CombatantsProperty = DependencyProperty.Register(nameof(Combatants), typeof(IEnumerable<ICombatant>), typeof(SelectCombatants));
        /// <summary>
        /// DependencyProperty for <see cref="SelectedCombatants"/>
        /// </summary>
        public static readonly DependencyProperty SelectedCombatantsProperty = DependencyProperty.Register(nameof(SelectedCombatants), typeof(ObservableCollection<ICombatant>), typeof(SelectCombatants),
            new FrameworkPropertyMetadata(null, SelectedCombatantsChanged));
        /// <summary>
        /// DependencyProperty for <see cref="IncludePlayers"/>
        /// </summary>
        public static readonly DependencyProperty IncludePlayersProperty = DependencyProperty.Register(nameof(IncludePlayers), typeof(bool), typeof(SelectCombatants));
        /// <summary>
        /// DependencyProperty for <see cref="IncludeNonPlayers"/>
        /// </summary>
        public static readonly DependencyProperty IncludeNonPlayersProperty = DependencyProperty.Register(nameof(IncludeNonPlayers), typeof(bool), typeof(SelectCombatants));
        /// <summary>
        /// DependencyProperty for <see cref="MultiSelect"/>
        /// </summary>
        public static readonly DependencyProperty MultiSelectProperty = DependencyProperty.Register(nameof(MultiSelect), typeof(bool), typeof(SelectCombatants));

        private static void SelectedCombatantsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (d is SelectCombatants view)
                    view?.UpdateCombatantSelection(e.NewValue as ObservableCollection<ICombatant>);
            });
        }
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            Panel panel = Template.FindName("RootGrid", this) as Panel;
            CollectionViewSource collection = panel?.Resources["CombatantsCollection"] as CollectionViewSource;
            if (collection != null)
                collection.Filter += Collection_Filter;

            _combatantList = Template.FindName("PART_CombatantList", this) as ListBox;
            if (_combatantList != null)
            {
                _combatantList.SelectionChanged += ListBox_SelectionChanged;
                if (MultiSelect)
                    _combatantList.SelectionMode = SelectionMode.Extended;
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (SelectedCombatants != null && !_updatingSelection)
                {
                    if (e.AddedItems != null)
                    {
                        foreach (ICombatant combatant in e.AddedItems)
                            SelectedCombatants.Add(combatant);
                    }
                    if (e.RemovedItems != null)
                    {
                        foreach (ICombatant combatant in e.RemovedItems)
                            SelectedCombatants.Remove(combatant);
                    }
                }
            });
        }

        private async Task UpdateCombatantSelection(ObservableCollection<ICombatant> combatants)
        {
            if (!IsLoaded)
                await Dispatcher.InvokeAsync(() => InnerUpdateCombatantSelection(combatants), System.Windows.Threading.DispatcherPriority.Render);
            else
                InnerUpdateCombatantSelection(combatants);
        }

        private void InnerUpdateCombatantSelection(ObservableCollection<ICombatant> combatants)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (_combatantList != null)
                {
                    _updatingSelection = true;
                    try
                    {
                        _combatantList.SelectedItems.Clear();
                        if (combatants != null)
                        {
                            foreach (ICombatant combatant in combatants)
                                _combatantList.SelectedItems.Add(combatant);
                        }
                    }
                    finally
                    {
                        _updatingSelection = false;
                    }
                }
            });
        }

        private void Collection_Filter(object sender, FilterEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (e.Item is ICombatant combatant)
                {
                    e.Accepted = (IncludePlayers && combatant.IsPlayer)
                        || (IncludeNonPlayers && !combatant.IsPlayer);
                }
                else
                    e.Accepted = false;
            });
        }
        #endregion
    }
}
