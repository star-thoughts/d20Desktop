using Fiction.GameScreen.Combat;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model used for selecting one or more combatants
    /// </summary>
    public sealed class SelectCombatantsViewModel : ViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="SelectCombatantsViewModel"/>
        /// </summary>
        /// <param name="combat">Combat to select combatants from</param>
        /// <param name="multiSelect">Whether or not to select multiple combatants</param>
        public SelectCombatantsViewModel(ActiveCombat combat, bool multiSelect)
            : this(combat, multiSelect, Array.Empty<ICombatant>())
        {
        }
        /// <summary>
        /// Constructs a new <see cref="SelectCombatantsViewModel"/>
        /// </summary>
        /// <param name="combat">Combat to select combatants from</param>
        /// <param name="multiSelect">Whether or not to select multiple combatants</param>
        public SelectCombatantsViewModel(ActiveCombat combat, bool multiSelect, params ICombatant[] combatants)
        {
            Exceptions.ThrowIfArgumentNull(combat, nameof(combat));
            Exceptions.ThrowIfArgumentNull(combatants, nameof(combatants));

            IncludeNonPlayers = true;
            IncludePlayers = true;

            Combat = combat;
            MultiSelect = multiSelect;
            SelectedCombatants = combatants.ToObservableCollection();
            SelectedCombatants.CollectionChanged += Combatants_CollectionChanged;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the combat to select from
        /// </summary>
        public ActiveCombat Combat { get; private set; }
        /// <summary>
        /// Gets whether or not the user can select multiple combatants
        /// </summary>
        public bool MultiSelect { get; private set; }
        private bool _includePlayers;
        /// <summary>
        /// Gets or sets whether to include player characters in the list of combatants
        /// </summary>
        public bool IncludePlayers
        {
            get { return _includePlayers; }
            set
            {
                if (_includePlayers != value)
                {
                    _includePlayers = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private bool _includeNonPlayers;
        /// <summary>
        /// Gets or sets whether to include non-player characters in the list of combatants
        /// </summary>
        public bool IncludeNonPlayers
        {
            get { return _includeNonPlayers; }
            set
            {
                if (_includeNonPlayers != value)
                {
                    _includeNonPlayers = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets the selected combatants
        /// </summary>
        public ObservableCollection<ICombatant> SelectedCombatants { get; private set; }
        /// <summary>
        /// Gets whether or not this view model's state is valid
        /// </summary>
        public override bool IsValid => SelectedCombatants.Any();
        #endregion
        #region Methods

        private void Combatants_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.RaisePropertyChanged(nameof(IsValid));
        }
        #endregion
    }
}
