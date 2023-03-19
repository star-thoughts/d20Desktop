using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for selecting damage types for damage reduction
    /// </summary>
    public sealed class ApplyDamageReductionViewModel : ViewModelCore, IDamageModifiersViewModel
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="ApplyDamageReductionViewModel"/>
        /// </summary>
        /// <param name="damage">Damage view model to apply damage reduction to</param>
        public ApplyDamageReductionViewModel(DamageCombatantViewModel damage)
        {
            Damage = damage;
            Damage.Damage.Combatants.CollectionChanged += Combatants_CollectionChanged;
            Types = new ObservableCollection<DamageTypeSelectionViewModel>();
            UpdateAvailableDamageTypes();

            _monitor = new CollectionMonitor(Types);
            _monitor.PropertyChanged += _monitor_PropertyChanged;
        }
        #endregion
        #region Member Variables
        private CollectionMonitor _monitor;
        #endregion
        #region Properties
        /// <summary>
        /// Gets the damage view model
        /// </summary>
        public DamageCombatantViewModel Damage { get; }
        /// <summary>
        /// Gets a collection of damage types that can be applied
        /// </summary>
        public ObservableCollection<DamageTypeSelectionViewModel> Types { get; private set; }
        /// <summary>
        /// Gets whether or not the values in this view model are valid
        /// </summary>
        public override bool IsValid => true;
        #endregion
        #region Methods
        /// <summary>
        /// Applies this damage modifier to the damage information
        /// </summary>
        public void Apply()
        {
            Damage.Damage.BypassDamageReduction = false;
        }

        private void Combatants_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateAvailableDamageTypes();
        }

        private void UpdateAvailableDamageTypes()
        {
            //  Get all damage types as strings
            string[] types = Damage.Damage.Combatants.SelectMany(p => p.Combatant.DamageReduction.SelectMany(i => i.Types))
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .ToArray();

            foreach (string type in types)
            {
                if (!Types.Any(p => p.Type.Equals(type, StringComparison.CurrentCultureIgnoreCase)))
                    Types.Add(new DamageTypeSelectionViewModel(type));
            }

            List<DamageTypeSelectionViewModel> current = Types.ToList();
            foreach (DamageTypeSelectionViewModel type in current)
            {
                if (!types.Contains(type.Type, StringComparer.CurrentCultureIgnoreCase))
                    Types.Remove(type);
            }

            UpdateDamageTypes();
        }

        private void UpdateDamageTypes()
        {
            Damage.Damage.DamageTypes.Clear();

            foreach (DamageTypeSelectionViewModel type in Types.Where(p => p.Selected))
            {
                Damage.Damage.DamageTypes.Add(type.Type);
            }
        }

        private void _monitor_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateDamageTypes();
        }
        #endregion
    }
}
