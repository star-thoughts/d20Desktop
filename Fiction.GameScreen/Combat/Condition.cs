using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// View model that contains the definition of a condition
    /// </summary>
    public sealed class Condition : INotifyPropertyChanged
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="Condition"/>
        /// </summary>
        /// <param name="name">Name of the condition</param>
        /// <param name="description">Description and information about the condition</param>
        public Condition(string name, string? description)
        {
            _name = name;
            _description = description;
        }
        #endregion
        #region Properties
        private string _name;
        /// <summary>
        /// Gets or sets the name of the condition
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (!string.Equals(_name, value))
                {
                    _name = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string? _description;
        /// <summary>
        /// Gets or sets the description of this condition
        /// </summary>
        public string? Description
        {
            get { return _description; }
            set
            {
                if (!string.Equals(_description, value))
                {
                    _description = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion
        #region Methods
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
