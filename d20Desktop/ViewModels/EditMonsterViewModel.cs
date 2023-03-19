using Fiction.GameScreen.Monsters;
using Fiction.GameScreen.ViewModels.EditMonsterViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for editing a monster
    /// </summary>
    public sealed class EditMonsterViewModel : ViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="EditMonsterViewModel"/>
        /// </summary>
        /// <param name="campaign">Campaign settings</param>
        /// <param name="monster">Monster to edit</param>
        public EditMonsterViewModel(CampaignSettings campaign, Monster monster)
        {
            Exceptions.ThrowIfArgumentNull(campaign, nameof(campaign));
            Exceptions.ThrowIfArgumentNull(monster, nameof(monster));

            Monster = monster;
            Campaign = campaign;

            _name = monster.Name;
            InitiativeModifier = monster.InitiativeModifier;
            _hitDice = monster.HitDieString;
            _fastHealing = monster.FastHealing;
            _source = monster.Source;
            DeadAt = monster.DeadAt;
            UnconsciousAt = monster.UnconsciousAt;

            CreateStats(monster);
        }
        /// <summary>
        /// Constructs a new <see cref="EditMonsterViewModel"/> with a new monster
        /// </summary>
        /// <param name="campaign">Campaign settings</param>
        public EditMonsterViewModel(CampaignSettings campaign)
        {
            Exceptions.ThrowIfArgumentNull(campaign, nameof(campaign));
            Campaign = campaign;

            Name = string.Empty;
            HitDice = string.Empty;

            CreateStats();
        }
        /// <summary>
        /// Constructs a readonly <see cref="EditMonsterViewModel"/>
        /// </summary>
        /// <param name="monster">Monster to view</param>
        public EditMonsterViewModel(Monster monster)
        {
            Monster = monster;

            Name = monster.Name;
            InitiativeModifier = monster.InitiativeModifier;
            HitDice = monster.HitDieString;
            FastHealing = monster.FastHealing;
            Source = monster.Source;
            DeadAt = monster.DeadAt;
            UnconsciousAt = monster.UnconsciousAt;

            CreateStats(monster);
        }
        #endregion
        #region Properties
        public CampaignSettings? Campaign { get; private set; }
        /// <summary>
        /// Gets a collection of sources to choose from
        /// </summary>
        public IEnumerable<string> Sources
        {
            get
            {
                string[] sources = new string[] { string.Empty }
                    .Concat((IEnumerable<string>?)Campaign?.Sources ?? Array.Empty<string>())
                    .ToArray();
                return sources;
            }
        }
        /// <summary>
        /// Gets a collection of groups to choose from
        /// </summary>
        public IEnumerable<string> Groups
        {
            get
            {
                string[] groups = new string[] { string.Empty }
                    .Concat((IEnumerable<string>?)Campaign?.MonsterManager?.Groups ?? Array.Empty<string>())
                    .ToArray();
                return groups;
            }
        }
        /// <summary>
        /// Gets the monster to edit
        /// </summary>
        public Monster? Monster { get; private set; }
        private string? _name;
        /// <summary>
        /// Gets or sets the name of the monster
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
        private string? _hitDice;
        /// <summary>
        /// Gets or sets the hit dice of the monster
        /// </summary>
        public string? HitDice
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
        /// Gets or sets the initiative modifier for the monster
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
        private int _fastHealing;
        /// <summary>
        /// Gets or sets the fast healing for this monster
        /// </summary>
        public int FastHealing
        {
            get { return _fastHealing; }
            set
            {
                if (_fastHealing != value)
                {
                    _fastHealing = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private int _deadAt;
        /// <summary>
        /// Gets or sets the amount of hit points the combatant is dead at
        /// </summary>
        public int DeadAt
        {
            get { return _deadAt; }
            set
            {
                if (_deadAt != value)
                {
                    _deadAt = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private int _unconsciousAt;
        /// <summary>
        /// Gets or sets the amount of hit points the combatant is unconscious at
        /// </summary>
        public int UnconsciousAt
        {
            get { return _unconsciousAt; }
            set
            {
                if (_unconsciousAt != value)
                {
                    _unconsciousAt = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string? _source;
        /// <summary>
        /// Gets or sets the source of this monster
        /// </summary>
        public string? Source
        {
            get { return _source; }
            set
            {
                if (!string.Equals(_source, value, StringComparison.Ordinal))
                {
                    _source = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets a collection of stats the user can modify
        /// </summary>
        public IReadOnlyCollection<IMonsterStatViewModel>? Stats { get; private set; }
        /// <summary>
        /// Gets whether or not this view model is valid
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Name)
                    && Dice.IsValidString(HitDice);
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Marks this view model as a copy of the monster passed in, so a new monster is created with this one's edited stats when saved
        /// </summary>
        public void MarkAsCopy()
        {
            Monster = null;
        }

        private void CreateStats(Monster monster)
        {
            List<IMonsterStatViewModel> stats = new List<IMonsterStatViewModel>();

            Name = monster.Name;
            InitiativeModifier = monster.InitiativeModifier;
            HitDice = monster.HitDieString;
            FastHealing = monster.FastHealing;

            stats.Add(new StringStatViewModel(Resources.Resources.GroupLabel, Resources.Resources.GeneralLabel, monster, "group", Campaign?.MonsterManager.Groups, true));
            stats.Add(new AlignmentStatViewModel(Resources.Resources.AlignmentLabel, Resources.Resources.GeneralLabel, monster, "alignment"));
            stats.Add(new SizeStatViewModel(Resources.Resources.SizeLabel, Resources.Resources.GeneralLabel, monster, "size"));
            stats.Add(new StringStatViewModel(Resources.Resources.TypeLabel, Resources.Resources.GeneralLabel, monster, "type", Campaign?.MonsterManager.Types, true));
            stats.Add(new CollectionStatViewModel(Resources.Resources.SubtypesLabel, Resources.Resources.GeneralLabel, monster, "subType", Campaign?.MonsterManager.SubTypes, true));
            stats.Add(new StringStatViewModel(Resources.Resources.SensesLabel, Resources.Resources.GeneralLabel, monster, "senses"));
            stats.Add(new StringStatViewModel(Resources.Resources.SpeedLabel, Resources.Resources.GeneralLabel, monster, "speed"));
            stats.Add(new StringStatViewModel(Resources.Resources.SpecialQualitiesLabel, Resources.Resources.GeneralLabel, monster, "specialQualities"));
            stats.Add(new StringStatViewModel(Resources.Resources.FeatsLabel, Resources.Resources.GeneralLabel, monster, "feats"));
            stats.Add(new StringStatViewModel(Resources.Resources.SkillsLabel, Resources.Resources.GeneralLabel, monster, "skills"));
            stats.Add(new StringStatViewModel(Resources.Resources.RacialSkillModifiersLabel, Resources.Resources.GeneralLabel, monster, "racial"));
            stats.Add(new StringStatViewModel(Resources.Resources.LanguagesLabel, Resources.Resources.GeneralLabel, monster, "languages"));
            stats.Add(new StringStatViewModel(Resources.Resources.EnvironmentLabel, Resources.Resources.GeneralLabel, monster, "environment"));
            stats.Add(new StringStatViewModel(Resources.Resources.ChallengeRatingLabel, Resources.Resources.GeneralLabel, monster, "challengeRating"));
            stats.Add(new StringStatViewModel(Resources.Resources.OrganisationLabel, Resources.Resources.GeneralLabel, monster, "org"));
            stats.Add(new StringStatViewModel(Resources.Resources.TreasureLabel, Resources.Resources.GeneralLabel, monster, "treasure"));
            stats.Add(new StringStatViewModel(Resources.Resources.VisualDescriptionLabel, Resources.Resources.GeneralLabel, monster, "visual"));

            stats.Add(new StringStatViewModel(Resources.Resources.StrengthLabel, Resources.Resources.StatisticsLabel, monster, "str"));
            stats.Add(new StringStatViewModel(Resources.Resources.DexterityLabel, Resources.Resources.StatisticsLabel, monster, "dex"));
            stats.Add(new StringStatViewModel(Resources.Resources.ConstitutionLabel, Resources.Resources.StatisticsLabel, monster, "con"));
            stats.Add(new StringStatViewModel(Resources.Resources.IntelligenceLabel, Resources.Resources.StatisticsLabel, monster, "int"));
            stats.Add(new StringStatViewModel(Resources.Resources.WisdomLabel, Resources.Resources.StatisticsLabel, monster, "wis"));
            stats.Add(new StringStatViewModel(Resources.Resources.CharismaLabel, Resources.Resources.StatisticsLabel, monster, "cha"));

            stats.Add(new StringStatViewModel(Resources.Resources.ArmorClassLabel, Resources.Resources.DefenseLabel, monster, "ac"));
            stats.Add(new StringStatViewModel(Resources.Resources.TouchArmorClassLabel, Resources.Resources.DefenseLabel, monster, "touch"));
            stats.Add(new StringStatViewModel(Resources.Resources.FlatFootedArmorClassLabel, Resources.Resources.DefenseLabel, monster, "flat"));
            stats.Add(new StringStatViewModel(Resources.Resources.DamageReductionLabel, Resources.Resources.DefenseLabel, monster, "damageReduction"));
            stats.Add(new StringStatViewModel(Resources.Resources.ImmunitiesLabel, Resources.Resources.DefenseLabel, monster, "immunities"));
            stats.Add(new StringStatViewModel(Resources.Resources.ResistancesLabel, Resources.Resources.DefenseLabel, monster, "resists"));
            stats.Add(new StringStatViewModel(Resources.Resources.VulnerabilitiesLabel, Resources.Resources.DefenseLabel, monster, "vuln"));
            stats.Add(new StringStatViewModel(Resources.Resources.SpellResistanceLabel, Resources.Resources.DefenseLabel, monster, "spellResist"));
            stats.Add(new StringStatViewModel(Resources.Resources.CombatManeuverDefenseLabel, Resources.Resources.DefenseLabel, monster, "cmd"));
            stats.Add(new StringStatViewModel(Resources.Resources.FortitudeLabel, Resources.Resources.DefenseLabel, monster, "fort"));
            stats.Add(new StringStatViewModel(Resources.Resources.ReflexLabel, Resources.Resources.DefenseLabel, monster, "ref"));
            stats.Add(new StringStatViewModel(Resources.Resources.WIllpowerLabel, Resources.Resources.DefenseLabel, monster, "will"));

            stats.Add(new StringStatViewModel(Resources.Resources.BaseAttackBonusLabel, Resources.Resources.OffenseLabel, monster, "bab"));
            stats.Add(new StringStatViewModel(Resources.Resources.MeleeLabel, Resources.Resources.OffenseLabel, monster, "melee"));
            stats.Add(new StringStatViewModel(Resources.Resources.ReachLabel, Resources.Resources.OffenseLabel, monster, "reach"));
            stats.Add(new StringStatViewModel(Resources.Resources.RangedLabel, Resources.Resources.OffenseLabel, monster, "ranged"));
            stats.Add(new StringStatViewModel(Resources.Resources.SpecialAttacksLabel, Resources.Resources.OffenseLabel, monster, "satt"));
            stats.Add(new StringStatViewModel(Resources.Resources.SpellLikeAbilitiesLabel, Resources.Resources.OffenseLabel, monster, "sla"));
            stats.Add(new StringStatViewModel(Resources.Resources.CombatManeuverBonusLabel, Resources.Resources.OffenseLabel, monster, "cmb"));

            stats.Add(new StringStatViewModel(Resources.Resources.DescriptionLabel, Resources.Resources.DescriptionLabel, monster, "desc"));

            Stats = stats;
        }
        private void CreateStats()
        {
            if (Campaign != null)
            {
                Monster monster = new Monster(Campaign, string.Empty, new MonsterStats());
                CreateStats(monster);
            }
        }

        /// <summary>
        /// Saves the information in this view model into the associated monster, adding it to the campaign if necessary
        /// </summary>
        /// <exception cref="InvalidOperationException">Attempt to save a monster that was loaded as readonly</exception>
        public void Save()
        {
            if (Campaign == null)
                throw new InvalidOperationException("Cannot save a readonly monster.");

            if (Stats == null || string.IsNullOrWhiteSpace(Name) || !Dice.IsValidString(HitDice))
                throw new InvalidOperationException("Cannot save a monster without stats assigned.");

            List<IMonsterStat> statList = Stats
                .Select(p => new MonsterStat(p.StatName, p.Value))
                .OfType<IMonsterStat>()
                .ToList();

            MonsterStats stats = new MonsterStats(statList);

            if (Monster == null)
            {
                Monster = new Monster(Campaign, Name, stats);
                Campaign.MonsterManager.Monsters.Add(Monster);
            }
            else
                Monster.Stats = stats;

            Monster.Name = Name;
            Monster.InitiativeModifier = InitiativeModifier;
            Monster.HitDieString = HitDice;
            Monster.FastHealing = FastHealing;
            Monster.DeadAt = DeadAt;
            Monster.UnconsciousAt = UnconsciousAt;
            if (!string.Equals(Monster.Source, Source, StringComparison.CurrentCulture))
            {
                //  If the source changed, force a reconcile of sources
                Monster.Source = Source;
                Campaign.ReconcileSources();
            }

            //  Now let's see if they added a type or subtype
            StringStatViewModel? type = Stats
                .OfType<StringStatViewModel>()
                .FirstOrDefault(p => p.StatName.Equals("type", StringComparison.InvariantCultureIgnoreCase));

            if (!string.IsNullOrWhiteSpace(type?.Value) && !Campaign.MonsterManager.Types.Contains(type.Value, StringComparer.CurrentCultureIgnoreCase))
                Campaign.MonsterManager.Types.Add(type.Value);

            CollectionStatViewModel? subtypesStat = Stats.OfType<CollectionStatViewModel>().FirstOrDefault(p => p.StatName.Equals("subtype", StringComparison.InvariantCultureIgnoreCase));
            string[] subTypes = (subtypesStat?.Value?.ToArray() ?? Array.Empty<string>())
                .Except(Campaign.MonsterManager.SubTypes, StringComparer.CurrentCultureIgnoreCase)
                .ToArray();
            foreach (string subType in subTypes)
                Campaign.MonsterManager.SubTypes.Add(subType);
        }
        #endregion
    }
}
