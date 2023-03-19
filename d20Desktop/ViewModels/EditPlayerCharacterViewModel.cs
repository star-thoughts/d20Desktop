using Fiction.GameScreen.Monsters;
using Fiction.GameScreen.Players;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;

namespace Fiction.GameScreen.ViewModels
{
    public sealed class EditPlayerCharacterViewModel : ViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="EditPlayerCharacterViewModel"/> for creating a character
        /// </summary>
        /// <param name="campaign">Campaign to make the character in</param>
        public EditPlayerCharacterViewModel(CampaignSettings campaign)
        {
            Exceptions.ThrowIfArgumentNull(campaign, nameof(campaign));

            Campaign = campaign;
            IncludeInCombat = true;
            _hitDice = "1";
            Notes = new ObservableCollection<string>();
        }
        /// <summary>
        /// Constructs a new <see cref="EditPlayerCharacterViewModel"/> for editing an existing character
        /// </summary>
        /// <param name="character"></param>
        public EditPlayerCharacterViewModel(PlayerCharacter character)
        {
            Character = character;

            Name = character.Name;
            Player = character.Player;
            InitiativeModifier = character.InitiativeModifier;
            IncludeInCombat = character.IncludeInCombat;
            _hitDice = character.HitDieString;
            Languages = character.Languages;
            Senses = character.Senses;
            LightRadius = character.LightRadius;
            Alignment = character.Alignment;
            Notes = new ObservableCollection<string>(character.Notes ?? Array.Empty<string>());
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the campaign the character is in
        /// </summary>
        public CampaignSettings? Campaign { get; private set; }
        /// <summary>
        /// Gets the character being edited, or null if adding a character
        /// </summary>
        public PlayerCharacter? Character { get; private set; }
        private string? _name;
        /// <summary>
        /// Gets or sets the name of the character
        /// </summary>
        public string? Name
        {
            get { return _name; }
            set
            {
                if (!string.Equals(_name, value, StringComparison.Ordinal))
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
                if (!string.Equals(_player, value, StringComparison.Ordinal))
                {
                    _player = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _hitDice;
        /// <summary>
        /// Gets or sets the hit dice for this character
        /// </summary>
        public string HitDice
        {
            get { return _hitDice; }
            set
            {
                if (!string.Equals(_hitDice, value, StringComparison.Ordinal))
                {
                    _hitDice = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private int _initiativeModifier;
        /// <summary>
        /// Gets or sets the initiative modifier
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
        private bool _includeInCombat;
        /// <summary>
        /// Gets or sets whether to automatically add to combat
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
        private int _lightRadius;
        /// <summary>
        /// Gets or sets the radius of light emitted by the character
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
        /// Gets or sets the senses for the character
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
        /// Gets or sets the languages the character speaks
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
        /// Gets or sets the alignment of the character
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
        /// <summary>
        /// Gets a collection of notes for this player character
        /// </summary>
        public ObservableCollection<string> Notes { get; }
        /// <summary>
        /// Gets whether or not this view model is valid
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Name)
                    && !string.IsNullOrWhiteSpace(Player);
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Saves or creates the player character
        /// </summary>
        public void Save()
        {
            if (Campaign == null)
                throw new InvalidOperationException("Cannot save a PC without a campaign.");

            if (Character == null)
            {
                Character = new PlayerCharacter(Campaign);
                Campaign.Players.PlayerCharacters.Add(Character);
            }

            Character.Name = Name;
            Character.Player = Player;
            Character.InitiativeModifier = InitiativeModifier;
            Character.IncludeInCombat = IncludeInCombat;
            Character.HitDieString = HitDice;
            Character.LightRadius = LightRadius;
            Character.Senses = Senses;
            Character.Languages = Languages;
            Character.Alignment = Alignment;
            Character.Notes = Notes.ToArray();

            Character.HitDieRollingStrategy = RollingStrategy.Standard;
        }
        #endregion
    }
}
