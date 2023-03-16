using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Contains information about damage to apply to a combatant
    /// </summary>
    public class CombatantDamageInformation : INotifyPropertyChanged
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="CombatantDamageInformation"/>
        /// </summary>
        /// <param name="combatant">Combatant this damage information is for</param>
        /// <param name="damageTypes">Types of damage being applied</param>
        public CombatantDamageInformation(ICombatant combatant, ObservableCollection<string> damageTypes)
        {
            Exceptions.ThrowIfArgumentNull(damageTypes, nameof(damageTypes));

            Combatant = combatant;
            DamageTypes = damageTypes;
            damageTypes.CollectionChanged += DamageTypes_CollectionChanged;
            IsLethal = true;
        }
        #endregion
        #region Properties
        public ICombatant Combatant { get; private set; }
        private string _amount;
        /// <summary>
        /// Gets or sets the amount of damage
        /// </summary>
        public string Amount
        {
            get { return _amount; }
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    this.RaisePropertiesChanged(nameof(Amount), nameof(ActualAmount));
                }
            }
        }
        /// <summary>
        /// Gets the actual amount of damage to deal
        /// </summary>
        public int ActualAmount
        {
            get
            {
                DamageReduction dr = !BypassDamageReduction ? Combatant.DamageReduction.GetDamageReduction(DamageTypes.ToArray())
                    : null;

                if (DamageParser.TryParse(_amount, dr?.Amount ?? 0, !ApplyDamageReductionToTotal, out int amount))
                {
                    switch (Modifier)
                    {
                        case DamageModifier.Double:
                            return amount * 2;
                        case DamageModifier.ExtraHalf:
                            return Convert.ToInt32(Math.Floor(amount * 1.5));
                        case DamageModifier.Half:
                            return Convert.ToInt32(Math.Floor(amount * 0.5));
                        case DamageModifier.None:
                            return 0;
                        case DamageModifier.Quarter:
                            return Convert.ToInt32(Math.Floor(amount * 0.25));
                        default:
                            return amount;
                    }
                }
                return 0;
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
                    this.RaisePropertyChanged();
                }
            }
        }
        private DamageModifier _modifier;
        /// <summary>
        /// Gets or sets the modifier for this combatant
        /// </summary>
        public DamageModifier Modifier
        {
            get { return _modifier; }
            set
            {
                if (_modifier != value)
                {
                    _modifier = value;
                    this.RaisePropertiesChanged(nameof(Modifier), nameof(ActualAmount));
                }
            }
        }
        private bool _bypassDamageReduction;
        /// <summary>
        /// Gets or sets whether or not to bypass damage reduction
        /// </summary>
        public bool BypassDamageReduction
        {
            get { return _bypassDamageReduction; }
            set
            {
                if (_bypassDamageReduction != value)
                {
                    _bypassDamageReduction = value;
                    this.RaisePropertiesChanged(nameof(BypassDamageReduction), nameof(ActualAmount));
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
                    this.RaisePropertiesChanged(nameof(ApplyDamageReductionToTotal), nameof(ActualAmount));
                }
            }
        }
        /// <summary>
        /// Gets a collection of damage types
        /// </summary>
        public IEnumerable<string> DamageTypes { get; private set; }

        private void DamageTypes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.RaisePropertyChanged(nameof(ActualAmount));
        }
        #endregion
        #region Events
#pragma warning disable 67
        /// <summary>
        /// Event that is triggered when a property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67
        #endregion
    }
}