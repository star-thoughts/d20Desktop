using Fiction.GameScreen.Combat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for editing a collection of damage reductions on a combatant
    /// </summary>
    public sealed class EditDamageReductionsViewModel : ViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="EditDamageReductionsViewModel"/>
        /// </summary>
        /// <param name="damageReductions">Collection of damage reductions to edit</param>
        public EditDamageReductionsViewModel(ICollection<DamageReduction> damageReductions)
        {
            _damageReductions = damageReductions;

            DamageReductions = _damageReductions.Select(p => new EditDamageReductionViewModel(p)).ToObservableCollection();

            _monitor = new CollectionMonitor(DamageReductions);
            _monitor.PropertyChanged += (s, e) => CheckValid(true);
            _monitor.CollectionChanged += (s, e) => CheckValid(true);
        }
        #endregion
        #region Member Variables
        private ICollection<DamageReduction> _damageReductions;
        private CollectionMonitor _monitor;
        #endregion
        #region Properties
        /// <summary>
        /// Gets a collection of damage reduction models to edit
        /// </summary>
        public ObservableCollection<EditDamageReductionViewModel> DamageReductions { get; }
        /// <summary>
        /// Gets whether or not this view model is valid
        /// </summary>
        public override bool IsValid
        {
            get { return DamageReductions.All(p => p.IsValid); }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Saves all changed settings
        /// </summary>
        public void Save()
        {
            _damageReductions.Clear();
            foreach (EditDamageReductionViewModel vm in DamageReductions)
            {
                _damageReductions.Add(vm._dr);
                vm.Save();
            }
            SetClean();
        }
        #endregion
    }
}
