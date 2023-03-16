using Fiction.GameScreen.Spells;
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
    /// View model for editing a spell's level information
    /// </summary>
    public sealed class EditSpellLevelsViewModel : ViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="EditSpellLevelsViewModel"/>
        /// </summary>
        /// <param name="campaign">Campaign the spell is in</param>
        /// <param name="spell">Spell to edit the levels for</param>
        public EditSpellLevelsViewModel(CampaignSettings campaign, ObservableCollection<SpellLevel> levels)
        {
            _campaign = campaign;
            _levels = levels;
            Levels = levels.Select(p => new SpellLevel() { Class = p.Class, Level = p.Level }).ToObservableCollection();
        }
        #endregion
        #region Member Variables
        private CampaignSettings _campaign;
        private ObservableCollection<SpellLevel> _levels;
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the spell to edit
        /// </summary>
        public ObservableCollection<SpellLevel> Levels
        {
            get { return _levels; }
            private set
            {
                if (!ReferenceEquals(_levels, value))
                {
                    _levels = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets all of the classes in all spells in the campaign
        /// </summary>
        public string[] Classes
        {
            get
            {
                return _campaign.Spells.Spells
                    .SelectMany(p => p.Levels.Select(i => i.Class)).
                    Distinct(StringComparer.CurrentCultureIgnoreCase)
                    .ToArray();
            }
        }
        /// <summary>
        /// Gets whether or not this view model is valid
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return Levels.All(p => IsLevelValid(p));
            }
        }
        #endregion
        #region Methods

        private bool IsLevelValid(SpellLevel level)
        {
            return !string.IsNullOrWhiteSpace(level.Class) && level.Level >= 0;
        }

        /// <summary>
        /// Saves the levels to the spell
        /// </summary>
        public void Save()
        {
            _levels.Clear();
            foreach (SpellLevel level in Levels)
                _levels.Add(level);
        }
        /// <summary>
        /// Adds a spell level
        /// </summary>
        public void AddLevel()
        {
            Levels.Add(new SpellLevel());
        }
        #endregion
    }
}
