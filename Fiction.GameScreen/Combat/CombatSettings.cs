using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Contains settings for an active combat
    /// </summary>
    public sealed class CombatSettings : INotifyPropertyChanged
    {
        #region Properties
        private bool _skipDownedCombatants;
        /// <summary>
        /// Gets or sets whether or not to skip unconscious and dead combatants
        /// </summary>
        public bool SkipDownedCombatants
        {
            get { return _skipDownedCombatants; }
            set
            {
                if (_skipDownedCombatants != value)
                {
                    _skipDownedCombatants = value;
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
