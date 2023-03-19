using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Information used for healing combatants
    /// </summary>
    public sealed class HealInformation : INotifyPropertyChanged
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="HealInformation"/>
        /// </summary>
        /// <param name="combatants"></param>
        public HealInformation(params ICombatant[] combatants)
        {
            Combatants = combatants
                .Select(p => new CombatantHealInformation(p))
                .ToObservableCollection();
            IsMagical = true;
            Combatants.CollectionChanged += Combatants_CollectionChanged;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets a collection of combatants to heal
        /// </summary>
        public ObservableCollection<CombatantHealInformation> Combatants { get; private set; }
        private int _amount;
        /// <summary>
        /// Gets or sets the amount of healing done
        /// </summary>
        public int Amount
        {
            get { return _amount; }
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    foreach (CombatantHealInformation combatant in Combatants)
                        combatant.Amount = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private bool _magical;
        /// <summary>
        /// Gets or sets whether this healing is from a magical source
        /// </summary>
        public bool IsMagical
        {
            get { return _magical; }
            set
            {
                if (_magical != value)
                {
                    _magical = value;
                    foreach (CombatantHealInformation combatant in Combatants)
                        combatant.IsMagical = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private bool _overheal;
        /// <summary>
        /// Gets or sets whether or not overhealing becomes temporary hit points
        /// </summary>
        public bool Overheal
        {
            get { return _overheal; }
            set
            {
                if (_overheal != value)
                {
                    _overheal = value;
                    foreach (CombatantHealInformation combatant in Combatants)
                        combatant.Overheal = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion
        #region Methods

        private void Combatants_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (CombatantHealInformation heal in e.NewItems)
                {
                    heal.Amount = Amount;
                    heal.IsMagical = IsMagical;
                    heal.Overheal = Overheal;
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
