using Fiction.GameScreen.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for choosing a source for a combatant template
    /// </summary>
    public sealed class ChooseCombatantTemplateSourceViewModel : ViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="ChooseCombatantTemplateSourceViewModel"/>
        /// </summary>
        public ChooseCombatantTemplateSourceViewModel(CampaignSettings campaign)
        {
            Campaign = campaign;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the campaign
        /// </summary>
        public CampaignSettings Campaign { get; private set; }
        private ICombatantTemplateSource? _selectedSource;
        /// <summary>
        /// Gets or sets the chosen source
        /// </summary>
        public ICombatantTemplateSource? SelectedSource
        {
            get { return _selectedSource; }
            set
            {
                if (!ReferenceEquals(_selectedSource, value))
                {
                    _selectedSource = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets whether or not this view model's data is valid
        /// </summary>
        public override bool IsValid => SelectedSource != null;
        #endregion
    }
}
