using Fiction.GameScreen.Combat;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for selecting a source for adding combatants to a combat
    /// </summary>
    public sealed class SelectCombatantSourceViewModel : ViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="SelectCombatantSourceViewModel"/>
        /// </summary>
        /// <param name="factory">Factory to use to create choices</param>
        public SelectCombatantSourceViewModel(IViewModelFactory factory)
        {
            Factory = factory;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the view model factory
        /// </summary>
        public IViewModelFactory Factory { get; private set; }
        private ICombatantSource _selectedSource;
        /// <summary>
        /// Gets or sets the currently selected combatant source
        /// </summary>
        public ICombatantSource SelectedSource
        {
            get { return _selectedSource; }
            set
            {
                if (_selectedSource!= value)
                {
                    _selectedSource = value;
                    this.RaisePropertiesChanged(nameof(SelectedSource), nameof(IsValid));
                }
            }
        }
        /// <summary>
        /// Gets whether or not the selection is valid
        /// </summary>
        public override bool IsValid => SelectedSource != null;
        #endregion
        #region Methods
        #endregion
    }
}
