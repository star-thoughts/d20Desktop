using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Information about damage to apply to one or more combatants
    /// </summary>
    public sealed class DamageInformation : INotifyPropertyChanged
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="DamageInformation"/>
        /// </summary>
        /// <param name="combatants"></param>
        public DamageInformation(params ICombatant[] combatants)
        {
            DamageTypes = new ObservableCollection<string>();
            Combatants = combatants
                .Select(p => new CombatantDamageInformation(p, DamageTypes))
                .ToObservableCollection();
            _isLethal = true;
            Combatants.CollectionChanged += Combatants_CollectionChanged;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets a collection of combatants to damage
        /// </summary>
        public ObservableCollection<CombatantDamageInformation> Combatants { get; }
        private string? _amount;
        /// <summary>
        /// Gets or sets the amount of damage done
        /// </summary>
        public string? Amount
        {
            get { return _amount; }
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    foreach (CombatantDamageInformation combatant in Combatants)
                        combatant.Amount = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private bool _isLethal;
        /// <summary>
        /// Gets or sets whether or not this damage is lethal
        /// </summary>
        public bool IsLethal
        {
            get { return _isLethal; }
            set
            {
                if (_isLethal != value)
                {
                    _isLethal = value;
                    foreach (CombatantDamageInformation combatant in Combatants)
                        combatant.IsLethal = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets a collection of types of damage that are being applied
        /// </summary>
        public ObservableCollection<string> DamageTypes { get; }
        private bool _bypassDamageReduction;
        /// <summary>
        /// Gets or sets whether or not to bypass damage reduction for this damage
        /// </summary>
        public bool BypassDamageReduction
        {
            get { return _bypassDamageReduction; }
            set
            {
                if (_bypassDamageReduction != value)
                {
                    _bypassDamageReduction = value;
                    foreach (CombatantDamageInformation combatant in Combatants)
                        combatant.BypassDamageReduction = _bypassDamageReduction;
                }
            }
        }
        private bool _applyDamageReductionToTotal;
        /// <summary>
        /// Gets or sets whether to apply the damage reduction to the total damage, or each damage
        /// </summary>
        public bool ApplyDamageReductionToTotal
        {
            get { return _applyDamageReductionToTotal; }
            set
            {
                if (_applyDamageReductionToTotal != value)
                {
                    _applyDamageReductionToTotal = value;
                    foreach (CombatantDamageInformation combatant in Combatants)
                        combatant.ApplyDamageReductionToTotal = _applyDamageReductionToTotal;
                }
            }
        }
        #endregion
        #region Methods

        private void Combatants_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (CombatantDamageInformation info in e.NewItems)
                {
                    if (info.Amount == null)
                    {
                        info.Amount = Amount;
                        info.BypassDamageReduction = BypassDamageReduction;
                        info.IsLethal = IsLethal;
                    }
                }
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