using Fiction.GameScreen.Combat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for editing damage reduction information for a combatant
    /// </summary>
    public sealed class EditDamageReductionViewModel : ViewModelCore, IDamageReduction
    {
        #region Constructors
        /// <summary>
        /// Constructs a new, empty <see cref="EditDamageReductionViewModel"/>
        /// </summary>
        public EditDamageReductionViewModel()
        {
            _dr = new DamageReduction(0, false, Array.Empty<string>());
            Types = new ObservableCollection<string>();

            PropertyChanged += (s, e) =>
            {
                if (!e.IsProperty(nameof(IsValid)) && !e.IsProperty(nameof(IsDirty)))
                    CheckValid(true);
            };
            Types.CollectionChanged += (s, e) => CheckValid(true);
        }
        /// <summary>
        /// Constructs a new <see cref="EditDamageReductionViewModel"/>
        /// </summary>
        /// <param name="dr">Damge reduction to edit</param>
        public EditDamageReductionViewModel(DamageReduction dr)
        {
            _dr = dr;
            Amount = dr.Amount;
            Types = dr.Types.ToObservableCollection();
            RequiresAllTypes = dr.RequiresAllTypes;

            PropertyChanged += (s, e) =>
            {
                if (!e.IsProperty(nameof(IsValid)) && !e.IsProperty(nameof(IsDirty)))
                    CheckValid(true);
            };
            Types.CollectionChanged += (s, e) => CheckValid(true);
        }
        #endregion
        #region Member Variables
        internal DamageReduction _dr;
        #endregion
        #region Properties
        private int _amount;
        /// <summary>
        /// Gets or sets the amount of damage reduction
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
        /// <summary>
        /// Gets a collection of types that can overcome the damage reduction
        /// </summary>
        public ObservableCollection<string> Types { get; }
        private bool _requiresAllTypes;
        /// <summary>
        /// Gets or sets whether all types are required to overcome damage reduction
        /// </summary>
        public bool RequiresAllTypes
        {
            get { return _requiresAllTypes; }
            set
            {
                if (_requiresAllTypes != value)
                {
                    _requiresAllTypes = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets whether or not this view model is valid
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return Amount > 0
                    && Types.All(p => !string.IsNullOrWhiteSpace(p));
            }
        }

        public void Save()
        {
            _dr.Amount = Amount;
            _dr.Types.Clear();
            foreach (string type in Types)
                _dr.Types.Add(type);
            _dr.RequiresAllTypes = RequiresAllTypes;
            SetClean();
        }
        #endregion
    }
}
