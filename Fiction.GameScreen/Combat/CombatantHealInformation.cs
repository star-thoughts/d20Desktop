using System.ComponentModel;

namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Information about healing a single combatant
    /// </summary>
    public sealed class CombatantHealInformation : INotifyPropertyChanged
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="CombatantHealInformation"/>
        /// </summary>
        /// <param name="combatant">Combatant this healing information is for</param>
        public CombatantHealInformation(ICombatant combatant)
        {
            Combatant = combatant;
        }
        #endregion
        #region Properties
        public ICombatant Combatant { get; private set; }
        private int _amount;
        /// <summary>
        /// Gets or sets the amount of healing
        /// </summary>
        public int Amount
        {
            get { return _amount; }
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private bool _magical;
        /// <summary>
        /// Gets or sets whether or not this healing is magical
        /// </summary>
        public bool IsMagical
        {
            get { return _magical; }
            set
            {
                if (_magical != value)
                {
                    _magical = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private bool _overheal;
        /// <summary>
        /// Gets or sets whether or not overhealing should become temporary hit points
        /// </summary>
        public bool Overheal
        {
            get { return _overheal; }
            set
            {
                if (_overheal != value)
                {
                    _overheal = value;
                    this.RaisePropertyChanged();
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