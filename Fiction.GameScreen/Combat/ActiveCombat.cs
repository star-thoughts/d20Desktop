using Fiction.GameScreen.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// A currently active combat
    /// </summary>
    public class ActiveCombat : INotifyPropertyChanged, IInitiativeCollection
    {
        #region Constructors
        /// <summary>
        /// Creates a new <see cref="ActiveCombat"/> from prepared combat
        /// </summary>
        /// <param name="name">Name of the combat</param>
        /// <param name="preparer">CombatPreparer used to set up the combat</param>
        /// <param name="serializer">Serializer to use for backing up and restoring combat for undo operations</param>
        public ActiveCombat(string name, CombatPreparer preparer, IXmlActiveCombatSerializer serializer)
        {
            Exceptions.ThrowIfArgumentNullOrEmpty(name, nameof(name));
            Exceptions.ThrowIfArgumentNull(preparer, nameof(preparer));

            _name = name;
            preparer.ResolveOrdinals();
            _serializer = serializer;

            AddCombatants(preparer);

            Initialize();
        }

        /// <summary>
        /// Creates a new <see cref="ActiveCombat"/> from a collection of combatants
        /// </summary>
        /// <param name="name">Name of the combat</param>
        /// <param name="combatants">Collection of combatants</param>
        public ActiveCombat(string name, IXmlActiveCombatSerializer serializer, IEnumerable<ICombatant> combatants)
        {
            Exceptions.ThrowIfArgumentNullOrEmpty(name, nameof(name));
            Exceptions.ThrowIfArgumentNull(combatants, nameof(combatants));

            _name = name;
            _combatants = combatants.ToObservableCollection();
            Combatants = new ReadOnlyObservableCollection<ICombatant>(_combatants);
            _serializer = serializer;

            Initialize();
        }

        [MemberNotNull(nameof(Settings)), MemberNotNull(nameof(Effects)), MemberNotNull(nameof(_combatantsMonitor)),
            MemberNotNull(nameof(_goneThisTurn)), MemberNotNull(nameof(_backups))]
        private void Initialize()
        {
            Settings = new CombatSettings();
            Effects = new ObservableCollection<Effect>();

            _combatantsMonitor = new CollectionMonitor(Combatants);
            _combatantsMonitor.PropertyChanged += _combatantsMonitor_PropertyChanged;

            _goneThisTurn = new List<ICombatant>();

            //  Set the initiative to the first combatant
            Current = GetCombatantAtInitiativeOrder(1);
            //  We do the beginning of the round stuff for the combatant, but we always start at 1 regardless
            Current?.TryBeginTurn(Settings);

            Round = 1;

            _backups = new Stack<Stream>();
        }
        #endregion
        #region Member Variables
        private ObservableCollection<ICombatant> _combatants;
        private CollectionMonitor _combatantsMonitor;
        private List<ICombatant> _goneThisTurn;
        private Stack<Stream> _backups;
        private IXmlActiveCombatSerializer _serializer;
        #endregion
        #region Properties
        /// <summary>
        /// Gets the combat settings for this combat
        /// </summary>
        public CombatSettings Settings { get; private set; }
        private string _name;
        /// <summary>
        /// Gets or sets the name of this combat
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (!string.Equals(_name, value))
                {
                    _name = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private ICombatant? _current;
        /// <summary>
        /// Gets or sets the current combatant
        /// </summary>
        public ICombatant? Current
        {
            get { return _current; }
            set
            {
                if (!ReferenceEquals(_current, value))
                {
                    _current = value;
                    if (Current != null)
                        Current.IsCurrent = true;
                    this.RaisePropertyChanged();
                }
            }
        }
        private int _round;
        /// <summary>
        /// Gets or sets the current round of combat
        /// </summary>
        public int Round
        {
            get { return _round; }
            set
            {
                if (_round != value)
                {
                    _round = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets a collection of combatants in this combat
        /// </summary>
        public ReadOnlyObservableCollection<ICombatant> Combatants { get; private set; }
        IEnumerable<IActiveCombatant> IInitiativeCollection.Combatants { get { return _combatants; } }
        /// <summary>
        /// Gets whether or not the user can restore from a backup for this combat
        /// </summary>
        public bool CanRestore { get { return _backups.Any(); } }
        /// <summary>
        /// Gets the effects going in combat
        /// </summary>
        public ObservableCollection<Effect> Effects { get; private set; }
        #endregion
        #region Methods
        /// <summary>
        /// Stores the current state of the combat for undo operations
        /// </summary>
        /// <returns>Task for asynchronous completion</returns>
        public async Task Backup()
        {
            _backups.Push(await _serializer.Backup(this));
        }
        /// <summary>
        /// Restores from the last backup that was made
        /// </summary>
        public void Restore()
        {
            if (_backups.Any())
            {
                Stream backup = _backups.Pop();
                _serializer.Restore(backup, this);
            }
        }
        /// <summary>
        /// Removes the given combatants from the combat
        /// </summary>
        /// <param name="combatants">Combatants to remove</param>
        /// <remarks>
        /// Generally combatants should not be removed from combat, they should be hidden by setting
        /// <see cref="ICombatant.IncludeInCombat"/> to false instead.
        /// </remarks>
        internal void RemoveCombatants(params ICombatant[] combatants)
        {
            foreach (ICombatant combatant in combatants)
                _combatants.Remove(combatant);
        }
        /// <summary>
        /// Adds combatants from the <see cref="CombatPreparer"/> passed in
        /// </summary>
        /// <param name="preparer">Prepared combat information to add to this active combat</param>
        [MemberNotNull(nameof(_combatants)), MemberNotNull(nameof(Combatants))]
        public void AddCombatants(CombatPreparer preparer)
        {
            ThrowIfCantAddPreparer(preparer);

            preparer.ResolveOrdinals();

            if (_combatants == null || Combatants == null)
            {
                _combatants = preparer.CreateCombatants().ToObservableCollection();
                Combatants = new ReadOnlyObservableCollection<ICombatant>(_combatants);
            }
            else
            {
                foreach (ICombatant combatant in preparer.CreateCombatants())
                    _combatants.Add(combatant);
            }
        }

        private void ThrowIfCantAddPreparer(CombatPreparer preparer)
        {
            if (Combatants != null)
            {
                if (!ReferenceEquals(preparer.SourceCombat, this))
                    throw new ArgumentException("CombatPreparer must be for this combat.", "preparer");
            }
        }

        /// <summary>
        /// Moves the current initiative to the next combatant in order
        /// </summary>
        /// <returns>The new current combatant, or null if no new combatant could be found</returns>
        public GotoNextResult GotoNext()
        {
            //  Only iterate through combatants once, if can't find one then we return null
            ICombatant? start = Current;
            bool beganTurn = false;
            List<GotoNextCombatant> combatants = new List<GotoNextCombatant>();
            do
            {
                int nextOrder = Current?.InitiativeOrder + 1 ?? 1;
                ICombatant? next = GetCombatantAtInitiativeOrder(nextOrder);

                if (next == null)
                {
                    next = GetCombatantAtInitiativeOrder(1);
                    //  Reset the list of combatants that have gone this turn and increment the round
                    _goneThisTurn.Clear();
                    Round++;
                }

                Current = next;
                (bool result, Effect[] effects) result = TryBeginTurn(Current);
                beganTurn = result.result;
                combatants.Add(new GotoNextCombatant(Current, result.effects));
            }
            while (!beganTurn && !ReferenceEquals(Current, start));

            return new GotoNextResult(ReferenceEquals(Current, start), combatants.ToArray());
        }

        /// <summary>
        /// Attempts to begin the turn for the combatant and returns whether the combatant can go this turn normally
        /// </summary>
        /// <param name="combatant">Combatant to test</param>
        /// <returns>Whether or not this character should go in initiative</returns>
        private (bool result, Effect[] effects) TryBeginTurn(ICombatant? combatant)
        {
            bool result = true;
            Effect[] effects = ResolveEffects(combatant);

            if (combatant != null)
            {
                if (!_goneThisTurn.Contains(combatant))
                {
                    result = combatant.TryBeginTurn(Settings);
                }
                _goneThisTurn.Add(combatant);
            }
            return (result, effects);
        }

        private Effect[] ResolveEffects(ICombatant? combatant)
        {
            List<Effect> completed = new List<Effect>();

            if (combatant != null)
            {
                foreach (Effect effect in Effects.Where(p => ReferenceEquals(p.InitiativeSource, combatant)))
                {
                    effect.RemainingRounds--;
                    if (effect.RemainingRounds <= 0)
                    {
                        effect.RemainingRounds = 0;
                        completed.Add(effect);
                    }
                }

                foreach (Effect effect in completed)
                    Effects.Remove(effect);
            }

            return completed.ToArray();
        }

        private ICombatant? GetCombatantAtInitiativeOrder(int order)
        {
            foreach (ICombatant combatant in Combatants)
            {
                if (combatant.InitiativeOrder == order)
                    return combatant;
            }
            return null;
        }

        private void _combatantsMonitor_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            //  If we set a combatant's IsCurrent to true, then update all previous current combatants
            if (e.IsProperty(nameof(Combatant.IsCurrent)) && sender is ICombatant combatant && combatant.IsCurrent == true)
                UnflagCurrent(combatant);
        }

        private void UnflagCurrent(ICombatant actualCurrent)
        {
            foreach (Combatant combatant in Combatants)
            {
                if (!ReferenceEquals(combatant, actualCurrent))
                    combatant.IsCurrent = false;
            }
        }
        /// <summary>
        /// Applies healing to combatants in the combat
        /// </summary>
        /// <param name="info">Info about the healing to apply</param>
        public void ApplyHealing(HealInformation info)
        {
            Exceptions.ThrowIfArgumentNull(info, nameof(info));

            foreach (CombatantHealInformation combatant in info.Combatants)
            {
                combatant.Combatant.Health.ApplyHealing(info.Amount, info.Overheal);
            }
        }
        /// <summary>
        /// Applies damage to combatants in the combat
        /// </summary>
        /// <param name="info">Info about the damage to apply</param>
        public void ApplyDamage(DamageInformation info)
        {
            Exceptions.ThrowIfArgumentNull(info, nameof(info));

            foreach (CombatantDamageInformation damageInfo in info.Combatants)
            {
                int amount = damageInfo.ActualAmount;

                if (damageInfo.IsLethal)
                    damageInfo.Combatant.Health.ApplyLethalDamage(amount);
                else
                    damageInfo.Combatant.Health.ApplyNonlethalDamage(amount);
            }
        }
        #endregion
        #region Events
#pragma warning disable 67
        /// <summary>
        /// Event that is triggered when a property changes
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore 67
        #endregion
    }
}
