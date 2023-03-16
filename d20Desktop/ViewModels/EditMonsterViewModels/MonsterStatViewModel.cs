using Fiction.GameScreen.Monsters;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels.EditMonsterViewModels
{
    /// <summary>
    /// Base view model for a monster stat
    /// </summary>
    /// <typeparam name="T">Type of stat</typeparam>
    public abstract class MonsterStatViewModel<T> : INotifyPropertyChanged, IMonsterStatViewModel
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="MonsterStatViewModel{T}"/>
        /// </summary>
        /// <param name="name">Name of the stat</param>
        /// <param name="monster">Monster this stat is for</param>
        /// <param name="statName">Name of the stat for storage</param>
        public MonsterStatViewModel(string name, string category, Monster monster, string statName)
        {
            Name = name;
            Category = category;
            StatName = statName;
            if (monster != null)
                Value = (T)(monster.Stats[statName]?.Value ?? CreateDefaultValue());
        }
        protected abstract T CreateDefaultValue();
        #endregion
        #region Member Variables
        #endregion
        #region Properties
        /// <summary>
        /// Gets the name of the stat for storage
        /// </summary>
        public string StatName { get; private set; }
        /// <summary>
        /// Gets the stat's category
        /// </summary>
        public string Category { get; private set; }
        /// <summary>
        /// Gets the name of the stat
        /// </summary>
        public string Name { get; private set; }
        private T _value;
        /// <summary>
        /// Gets or sets the value of this stat
        /// </summary>
        public T Value
        {
            get { return _value; }
            set
            {
                if (!ReferenceEquals(_value, value))
                {
                    _value = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets the value of the stat
        /// </summary>
        object IMonsterStatViewModel.Value { get { return this.Value; } }
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
