using System;
using System.ComponentModel;

namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Health information for a combatant
    /// </summary>
    public sealed class CombatantHealth : INotifyPropertyChanged
    {
        #region Properties
        private int _maxHealth;
        /// <summary>
        /// Gets or sets the maximum health for this combatant
        /// </summary>
        public int MaxHealth
        {
            get { return _maxHealth; }
            set
            {
                if (_maxHealth != value)
                {
                    _maxHealth = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertiesChanged(nameof(IsDead), nameof(IsUnconscious), nameof(CurrentHitPoints), nameof(IsDown));
                }
            }
        }
        private int _lethalDamage;
        /// <summary>
        /// Gets or sets the amount of lethal damage for this combatant
        /// </summary>
        public int LethalDamage
        {
            get { return _lethalDamage; }
            set
            {
                if (_lethalDamage != value)
                {
                    _lethalDamage = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertiesChanged(nameof(IsDead), nameof(IsUnconscious), nameof(CurrentHitPoints), nameof(IsDown));
                }
            }
        }
        private int _nonlethalDamage;
        /// <summary>
        /// Gets or sets the amount of non-lethal damage for this combatant
        /// </summary>
        public int NonlethalDamage
        {
            get { return _nonlethalDamage; }
            set
            {
                if (_nonlethalDamage != value)
                {
                    _nonlethalDamage = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertiesChanged(nameof(IsDead), nameof(IsUnconscious), nameof(IsDown));
                }
            }
        }
        private int _temporaryHitPoints;
        /// <summary>
        /// Gets or sets the amount of temporary hit points the combatant has
        /// </summary>
        public int TemporaryHitPoints
        {
            get { return _temporaryHitPoints; }
            set
            {
                if (_temporaryHitPoints != value)
                {
                    _temporaryHitPoints = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertiesChanged(nameof(IsDead), nameof(IsUnconscious), nameof(CurrentHitPoints), nameof(IsDown));
                }
            }
        }
        private int _deadAt;
        /// <summary>
        /// Gets or sets the current life the character has to be at to be dead
        /// </summary>
        public int DeadAt
        {
            get { return _deadAt; }
            set
            {
                if (_deadAt != value)
                {
                    _deadAt = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertiesChanged(nameof(IsDead), nameof(IsDown), nameof(IsUnconscious));
                }
            }
        }
        private int _unconsciousAt;
        /// <summary>
        /// Gets or sets the current life the character has to be at to be unconscious
        /// </summary>
        public int UnconsciousAt
        {
            get { return _unconsciousAt; }
            set
            {
                if (_unconsciousAt != value)
                {
                    _unconsciousAt = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertiesChanged(nameof(IsUnconscious), nameof(IsDown), nameof(IsDead));
                }
            }
        }
        /// <summary>
        /// Gets the combatant's current health
        /// </summary>
        public int CurrentHitPoints
        {
            get { return MaxHealth + TemporaryHitPoints - LethalDamage; }
        }
        /// <summary>
        /// Gets whether or not the combatant is unconscious
        /// </summary>
        public bool IsUnconscious
        {
            get
            {
                return !IsDead && ((CurrentHitPoints - NonlethalDamage) < UnconsciousAt);
            }
        }
        /// <summary>
        /// Gets whether or not the combatant is dead
        /// </summary>
        public bool IsDead
        {
            get
            {
                return CurrentHitPoints <= DeadAt;
            }
        }
        private int _fastHealing;
        /// <summary>
        /// Gets or sets the amount of fast healing the combatant has
        /// </summary>
        public int FastHealing
        {
            get { return _fastHealing; }
            set
            {
                if (_fastHealing != value)
                {
                    _fastHealing = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets whether or not the combatant is dead or unconscious
        /// </summary>
        public bool IsDown { get { return IsDead || IsUnconscious; } }
        #endregion
        #region Methods
        /// <summary>
        /// Applies lethal damage to the combatant
        /// </summary>
        /// <param name="amount">Amount of damage to apply</param>
        public void ApplyLethalDamage(int amount)
        {
            int overflow = 0;
            TemporaryHitPoints -= amount;

            if (TemporaryHitPoints < 0)
                overflow = Math.Abs(TemporaryHitPoints);

            TemporaryHitPoints += overflow;
            LethalDamage += overflow;
        }
        /// <summary>
        /// Applies non-lethal damage to the combatant
        /// </summary>
        /// <param name="amount">Amount of damage to apply</param>
        public void ApplyNonlethalDamage(int amount)
        {
            int overflow = 0;
            TemporaryHitPoints -= amount;

            if (TemporaryHitPoints < 0)
                overflow = Math.Abs(TemporaryHitPoints);

            TemporaryHitPoints += overflow;
            int maxHp = MaxHealth + TemporaryHitPoints;
            int totalNonlethal = NonlethalDamage + overflow;

            NonlethalDamage = Math.Min(maxHp, totalNonlethal);

            //  If any of the nonlethal damage did not get applied, send it to lethal damage instead
            int difference = totalNonlethal - NonlethalDamage;
            if (difference > 0)
                ApplyLethalDamage(difference);
        }
        /// <summary>
        /// Heals the combatant
        /// </summary>
        /// <param name="amount">Amount of healing</param>
        /// <param name="overheal">Whether or not overhealing becomes temporary hit points</param>
        public void ApplyHealing(int amount, bool overheal)
        {
            if (overheal)
                TemporaryHitPoints += Math.Max(0, amount - LethalDamage);
            NonlethalDamage = Math.Max(0, NonlethalDamage - amount);
            LethalDamage = Math.Max(0, LethalDamage - amount);
        }
        /// <summary>
        /// Places this combatant at the point of death
        /// </summary>
        public void Kill()
        {
            TemporaryHitPoints = 0;
            LethalDamage = MaxHealth - DeadAt;
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