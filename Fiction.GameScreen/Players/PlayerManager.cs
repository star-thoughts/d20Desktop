using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Fiction.GameScreen.Players
{
    /// <summary>
    /// Manages players in a campaign
    /// </summary>
    public sealed class PlayerManager
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="PlayerManager"/>
        /// </summary>
        public PlayerManager()
        {
            PlayerCharacters = new ObservableCollection<PlayerCharacter>();
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets a collection of players in the campaign
        /// </summary>
        public ObservableCollection<PlayerCharacter> PlayerCharacters { get; private set; }
        #endregion
    }
}
