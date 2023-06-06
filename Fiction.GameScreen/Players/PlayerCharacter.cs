using Fiction.GameScreen.Combat;
using Fiction.GameScreen.Monsters;
using System.ComponentModel;

namespace Fiction.GameScreen.Players
{
    /// <summary>
    /// Information about a player character in the group
    /// </summary>
    public sealed class PlayerCharacter : ICombatantSource, ICampaignObject, INotifyPropertyChanged, ICombatantTemplateSource
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="PlayerCharacter"/>
        /// </summary>
        /// <param name="campaign">Campaign the player is in</param>
        public PlayerCharacter(CampaignSettings campaign)
        {
            Campaign = campaign;
            Id = campaign.GetNextId();
            _includeInCombat = true;
            _hitDie = "1";
            _notes = Array.Empty<string>();
        }
        /// <summary>
        /// Constructs a new <see cref="PlayerCharacter"/>
        /// </summary>
        /// <param name="campaign">Campaign the player is in</param>
        /// <param name="id">ID of the player</param>
        public PlayerCharacter(CampaignSettings campaign, int id)
        {
            Campaign = campaign;
            Id = id;
            _includeInCombat = true;
            _hitDie = "1";
            _notes = Array.Empty<string>();
        }
        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets the ID of this character on the server
        /// </summary>
        public string? ServerID { get; set; }

        private string? _name;
        /// <summary>
        /// Gets or sets the name of the character
        /// </summary>
        public string? Name
        {
            get { return _name; }
            set
            {
                if (!string.Equals(_name, value, StringComparison.CurrentCulture))
                {
                    _name = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string? _player;
        /// <summary>
        /// Gets or sets the name of the player
        /// </summary>
        public string? Player
        {
            get { return _player; }
            set
            {
                if (!string.Equals(_player, value, StringComparison.CurrentCulture))
                {
                    _player = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private bool _includeInCombat;
        /// <summary>
        /// Gets or sets whether or not to automatically add this character to combat
        /// </summary>
        public bool IncludeInCombat
        {
            get { return _includeInCombat; }
            set
            {
                if (_includeInCombat != value)
                {
                    _includeInCombat = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets the campaign the player is in
        /// </summary>
        public CampaignSettings Campaign { get; private set; }
        /// <summary>
        /// Gets the characters ID
        /// </summary>
        public int Id { get; private set; }
        private string _hitDie;
        /// <summary>
        /// Gets or sets the hit dice for this combatant
        /// </summary>
        public string HitDieString
        {
            get { return _hitDie; }
            set
            {
                if (!string.Equals(_hitDie, value, StringComparison.CurrentCulture))
                {
                    _hitDie = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private RollingStrategy _rollingStrategy;
        /// <summary>
        /// Gets or sets the rolling strategy to use for hit dice
        /// </summary>
        public RollingStrategy HitDieRollingStrategy
        {
            get { return _rollingStrategy; }
            set
            {
                if (_rollingStrategy != value)
                {
                    _rollingStrategy = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private int _initiativeModifier;
        /// <summary>
        /// Gets or sets the initiative modifier for combat
        /// </summary>
        public int InitiativeModifier
        {
            get { return _initiativeModifier; }
            set
            {
                if (_initiativeModifier != value)
                {
                    _initiativeModifier = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        int ICombatantTemplateSource.FastHealing
        {
            get { return 0; }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets whether or not this is a player character
        /// </summary>
        public bool IsPlayer { get { return true; } }
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Player)
                    && !string.IsNullOrWhiteSpace(Name)
                    && !string.IsNullOrWhiteSpace(HitDieString);
            }
        }
        /// <summary>
        /// Gets the amount of hit points the player's character is dead at
        /// </summary>
        public int DeadAt => 0;
        /// <summary>
        /// Gets the amount of hit points the player's character is unconscious at
        /// </summary>
        public int UnconsciousAt => 0;
        private int _lightRadius;
        /// <summary>
        /// Gets or sets the amount of light given off by this character
        /// </summary>
        public int LightRadius
        {
            get { return _lightRadius; }
            set
            {
                if (_lightRadius != value)
                {
                    _lightRadius = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string? _senses;
        /// <summary>
        /// Gets or sets a string containing senses
        /// </summary>
        public string? Senses
        {
            get { return _senses; }
            set
            {
                if (!string.Equals(_senses, value, StringComparison.Ordinal))
                {
                    _senses = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string? _languages;
        /// <summary>
        /// Gets or sets the languages the character can speak
        /// </summary>
        public string? Languages
        {
            get { return _languages; }
            set
            {
                if (!string.Equals(_languages, value, StringComparison.Ordinal))
                {
                    _languages = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private Alignment _alignment;
        /// <summary>
        /// Gets or sets the characters alignment
        /// </summary>
        public Alignment Alignment
        {
            get { return _alignment; }
            set
            {
                if (_alignment != value)
                {
                    _alignment = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string[] _notes;
        /// <summary>
        /// Gets or sets notes for this character
        /// </summary>
        public string[] Notes
        {
            get { return _notes; }
            set
            {
                if (!ReferenceEquals(_notes, value))
                {
                    _notes = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Creates a combatant from this player
        /// </summary>
        /// <param name="preparer">Combat preparer to add this to</param>
        /// <returns>Combatant created</returns>
        public CombatantPreparer[] Prepare(CombatPreparer preparer)
        {
            CombatantPreparer combatant = new CombatantPreparer(CreatePreparer());
            return new CombatantPreparer[] { combatant };

            CombatantTemplate CreatePreparer()
            {
                CombatantTemplate template = new CombatantTemplate(Campaign);
                template.Count = "1";
                template.FastHealing = 0;
                template.HitDieRollingStrategy = RollingStrategy.Standard;
                template.HitDieString = HitDieString ?? string.Empty;
                template.InitiativeModifier = InitiativeModifier;
                template.Name = Name ?? string.Empty;
                template.Source = this;

                return template;
            }
        }

        /// <summary>
        /// Creates a server representation of this character
        /// </summary>
        /// <returns></returns>
        public d20Web.Models.Players.PlayerCharacter ToServerCharacter()
        {
            return new d20Web.Models.Players.PlayerCharacter()
            {
                HitDie = HitDieString,
                ID = ServerID,
                Alignment = (d20Web.Models.Alignment)Alignment,
                IncludeInCombat = IncludeInCombat,
                InitiativeModifier = InitiativeModifier,
                Languages = Languages,
                LightRadius = LightRadius,
                Name = Name,
                Notes = Notes,
                Player = Player,
                RollingStrategy = (d20Web.Models.RollingStrategy)HitDieRollingStrategy,
                Senses = Senses,
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
