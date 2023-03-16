using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for entering a string value
    /// </summary>
    public sealed class EnterValueViewModel : ViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a default emtpy <see cref="EnterValueViewModel"/>
        /// </summary>
        public EnterValueViewModel()
            : this(string.Empty)
        {
        }
        /// <summary>
        /// Constructs a new <see cref="EnterValueViewModel"/>
        /// </summary>
        /// <param name="startingValue">Default value</param>
        public EnterValueViewModel(string startingValue)
        {
            Value = startingValue;
        }
        /// <summary>
        /// Constructs a new <see cref="EnterValueViewModel"/>
        /// </summary>
        /// <param name="startingValue">Default value</param>
        /// <param name="availableValues">Collection of values to choose from</param>
        public EnterValueViewModel(string startingValue, IEnumerable<string> availableValues)
            : this(startingValue)
        {
            Values = availableValues?.ToObservableCollection();
        }
        /// <summary>
        /// Constructs a new <see cref="EnterValueViewModel"/>
        /// </summary>
        /// <param name="startingValue">Default value</param>
        /// <param name="availableValues">Collection of values to choose from</param>
        /// <param name="allowEdit">Whether or not the user can edit the value without picking from the list</param>
        public EnterValueViewModel(string startingValue, IEnumerable<string> availableValues, bool allowEdit)
            : this(startingValue, availableValues)
        {
            AllowEdit = allowEdit;
        }
        #endregion
        #region Properties
        private string _value;
        /// <summary>
        /// Gets or sets the value entered
        /// </summary>
        public string Value
        {
            get { return _value; }
            set
            {
                if (!string.Equals(_value, value, StringComparison.Ordinal))
                {
                    _value = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets a collection of available values
        /// </summary>
        public ObservableCollection<string> Values { get; private set; }
        /// <summary>
        /// Gets or sets whether or not values can be entered that aren't in the list of values
        /// </summary>
        public bool AllowEdit { get; private set; }
        /// <summary>
        /// Gets whether or not this view model is valid
        /// </summary>
        public override bool IsValid => !string.IsNullOrWhiteSpace(Value);
        #endregion
    }
}
