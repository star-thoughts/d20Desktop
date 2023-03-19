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
    /// View model for a collection of strings for a stat
    /// </summary>
    public sealed class CollectionStatViewModel : MonsterStatViewModel<ObservableCollection<string>>
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="CollectionStatViewModel"/>
        /// </summary>
        /// <param name="name">Name of the stat</param>
        /// <param name="category">Category for the stat</param>
        /// <param name="monster">Monster this stat is for</param>
        /// <param name="statName">Name of the stat for storage</param>
        /// <param name="source">Source collection to get values from</param>
        /// <param name="canAddNew">Whether not the user can manually enter something to add (which is added to <paramref name="source"/>)</param>
        public CollectionStatViewModel(string name, string category, Monster monster, string statName, ObservableCollection<string>? source, bool canAddNew)
            : base(name, category, monster, statName)
        {
            Source = source;
            CanAddNew = canAddNew;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets whether or not the user can manually enter a new item, when is then added to <see cref="Source"/>
        /// </summary>
        public bool CanAddNew { get; private set; }
        /// <summary>
        /// Gets a collection of source objects to choose from
        /// </summary>
        public ObservableCollection<string>? Source { get; private set; }
        #endregion
        #region Methods
        protected override ObservableCollection<string> CreateDefaultValue()
        {
            return new ObservableCollection<string>();
        }
        #endregion
    }
}
