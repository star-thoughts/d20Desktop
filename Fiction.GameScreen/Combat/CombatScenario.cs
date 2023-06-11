using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Represents a combat in the campaign
    /// </summary>
    public class CombatScenario : INotifyPropertyChanged, ICampaignObject, ICombatantSource
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="CombatScenario"/>
        /// </summary>
        public CombatScenario(CampaignSettings campaign)
        {
            Exceptions.ThrowIfArgumentNull(campaign, nameof(campaign));

            Initialize(campaign);
        }

        [MemberNotNull(nameof(_combatants)), MemberNotNull(nameof(_name)), MemberNotNull(nameof(Campaign))]
        private void Initialize(CampaignSettings campaign)
        {
            _combatants = new ObservableCollection<ICombatantTemplate>();
            _name = string.Empty;
            Campaign = campaign;
            Id = campaign.GetNextId();
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the campaign for this scenario
        /// </summary>
        public CampaignSettings Campaign { get; internal set; }
        /// <summary>
        /// Gets the ID of this scenario
        /// </summary>
        public int Id { get; internal set; }
        /// <summary>
        /// Gets or sets the ID of this scenario on the server
        /// </summary>
        public string? ServerID { get; set; }
        private string _name;
        /// <summary>
        /// Gets or sets the name of this combat scenario
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (!string.Equals(_name, value))
                {
                    _name = value;
                    this.RaisePropertyChanged(nameof(Name));
                }
            }
        }

        private string? _group;
        /// <summary>
        /// Gets or sets the grouping for this scenario
        /// </summary>
        public string? Group
        {
            get { return _group; }
            set
            {
                if (!string.Equals(_group, value))
                {
                    _group = value ?? string.Empty;
                    this.RaisePropertyChanged(nameof(Group));
                }
            }
        }
        private string? _details;
        /// <summary>
        /// Gets or sets any extra details for this combat scenario
        /// </summary>
        public string? Details
        {
            get { return _details; }
            set
            {
                if (!string.Equals(_details, value))
                {
                    _details = value ?? string.Empty;
                    this.RaisePropertiesChanged(nameof(Details));
                }
            }
        }
        private ObservableCollection<ICombatantTemplate> _combatants;
        /// <summary>
        /// Gets a collection of combatants in this scenario
        /// </summary>
        public ObservableCollection<ICombatantTemplate> Combatants
        {
            get { return _combatants; }
            set
            {
                if (!ReferenceEquals(_combatants, value))
                {
                    _combatants = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets whether or not this combat is currently valid
        /// </summary>
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(Name)
                    && Combatants.Any();
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Sets the combatants in this scenario
        /// </summary>
        /// <param name="combatants">Combatants to use</param>
        public void SetCombatants(IEnumerable<ICombatantTemplate> combatants)
        {
            Combatants = combatants.ToObservableCollection();
        }

        /// <summary>
        /// Creates combatants to add to combat
        /// </summary>
        /// <param name="preparer">Combat preparer to add to</param>
        /// <returns>Information about the combatants to add</returns>
        public CombatantPreparer[] Prepare(CombatPreparer preparer)
        {
            return Combatants.SelectMany(p => p.Prepare(preparer))
                .ToArray();
        }

        public d20Web.Models.Combat.CombatScenario ToServerScenario()
        {
            return new d20Web.Models.Combat.CombatScenario()
            {
                ID = ServerID,
                Details = Details,
                Group = Group,
                Name = Name,
            };
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
