using Fiction.GameScreen.Monsters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels.EditMonsterViewModels
{
    /// <summary>
    /// View model for a string based stat
    /// </summary>
    public sealed class StringStatViewModel : MonsterStatViewModel<string>
    {
        #region Cosntructors
        /// <summary>
        /// Constructs a new <see cref="StringStatViewModel"/>
        /// </summary>
        /// <param name="name">Name of the stat</param>
        /// <param name="category">Category for the stat</param>
        /// <param name="monster">Monster this stat is for</param>
        /// <param name="statName">Name of the stat for storage</param>
        public StringStatViewModel(string name, string category, Monster monster, string statName)
            : base(name, category, monster, statName)
        {
        }
        /// <summary>
        /// Constructs a new <see cref="StringStatViewModel"/>
        /// </summary>
        /// <param name="name">Name of the stat</param>
        /// <param name="category">Category for the stat</param>
        /// <param name="monster">Monster this stat is for</param>
        /// <param name="statName">Name of the stat for storage</param>
        /// <param name="source">Source collection to get values from</param>
        /// <param name="canAddNew">Whether not the user can manually enter something to add (which is added to <paramref name="source"/>)</param>
        public StringStatViewModel(string name, string category, Monster monster, string statName, ObservableCollection<string>? source, bool canAddNew)
            : base(name, category, monster, statName)
        {
            Source = source;
            CanAddNew = canAddNew;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets a collection of strings that can be used to set the value, or null if no options
        /// </summary>
        public ObservableCollection<string>? Source { get; private set; }
        /// <summary>
        /// Gets whether or not the user can add strings to the source
        /// </summary>
        public bool CanAddNew { get; private set; }
        #endregion
        #region Methods
        protected override string CreateDefaultValue()
        {
            return string.Empty;
        }
        #endregion
    }
}
