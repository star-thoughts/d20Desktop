using System.ComponentModel;
using System.Windows.Markup;

namespace Fiction.GameScreen.Monsters
{
    /// <summary>
    /// Information about a stat
    /// </summary>
    public sealed class MonsterStat : INotifyPropertyChanged, IMonsterStat
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="MonsterStat"/>
        /// </summary>
        /// <param name="name">Name of the stat</param>
        /// <param name="value">Value for the stat</param>
        public MonsterStat(string name, object? value)
        {
            Name = name;
            Value = value;
        }
        /// <summary>
        /// Constructs a new <see cref="MonsterStat"/> from server representation
        /// </summary>
        /// <param name="stat">Server representation of a monster's stat</param>
        public MonsterStat(d20Web.Models.Bestiary.MonsterStat stat)
        {
            Name = stat.Name ?? string.Empty;
            Value = stat.Value;
            Value ??= stat.Values;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the name of the stat
        /// </summary>
        public string Name { get; private set; }
        private object? _value;
        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public object? Value
        {
            get { return _value; }
            set
            {
                if (_value == null && value != null)
                {
                    _value = value;
                    this.RaisePropertyChanged();
                }
                else if (_value == null || !_value.Equals(value))
                {
                    _value = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Copies these stats to a server representation of stats
        /// </summary>
        /// <returns>Server representation</returns>
        public d20Web.Models.Bestiary.MonsterStat ToServerMonsterStat()
        {
            d20Web.Models.Bestiary.MonsterStat result = new d20Web.Models.Bestiary.MonsterStat()
            {
                Name = Name,
                Values = Value as IEnumerable<string>
            };
            if (result.Values == null)
                result.Value = Value?.ToString();

            return result;
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
