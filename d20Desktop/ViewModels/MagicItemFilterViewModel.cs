using Fiction.GameScreen.Equipment;
using System;
using System.Globalization;
using System.Linq;
using System.ComponentModel;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// Filter settings for magic items
    /// </summary>
    public sealed class MagicItemFilterViewModel : FilterViewModelCore<MagicItem>
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="MagicItemFilterViewModel"/>
        /// </summary>
        /// <param name="campaign">Campaign the filter is for</param>
        public MagicItemFilterViewModel(CampaignSettings campaign)
        {
            SlotOptions = campaign.EquipmentManager.MagicItems
                .Select(p => p.Slot)
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .OfType<string>()
                .ToArray();

            GroupOptions = campaign.EquipmentManager.MagicItems
                .Select(p => p.Group)
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .OfType<string>()
                .ToArray();
        }
        #endregion
        #region Properties
        private string? _name;
        /// <summary>
        /// Gets or sets the name to filter by
        /// </summary>
        public string? Name
        {
            get { return _name; }
            set
            {
                if (!string.Equals(_name, value, StringComparison.Ordinal))
                {
                    _name = value;
                    this.RaisePropertiesChanged(nameof(Name), nameof(HasFilter), nameof(IsValid));
                }
            }
        }
        private string? _casterLevel;
        /// <summary>
        /// Gets or sets the expected caster level
        /// </summary>
        public string? CasterLevel
        {
            get { return _casterLevel; }
            set
            {
                if (!string.Equals(_casterLevel, value, StringComparison.Ordinal))
                {
                    _casterLevel = value;
                    this.RaisePropertiesChanged(nameof(CasterLevel), nameof(HasFilter), nameof(IsValid));
                }
            }
        }
        private string? _slot;
        /// <summary>
        /// Gets or sets the expected slot
        /// </summary>
        public string? Slot
        {
            get { return _slot; }
            set
            {
                if (!string.Equals(_slot, value, StringComparison.Ordinal))
                {
                    _slot = value;
                    this.RaisePropertiesChanged(nameof(Slot), nameof(HasFilter), nameof(IsValid));
                }
            }
        }
        /// <summary>
        /// Gets a collection of options for <see cref="Slot"/>
        /// </summary>
        public string[] SlotOptions { get; }
        private int _minimumPrice;
        /// <summary>
        /// Gets or sets the minimum price to expect
        /// </summary>
        public int MinimumPrice
        {
            get { return _minimumPrice; }
            private set
            {
                if (_minimumPrice != value)
                {
                    _minimumPrice = value;
                    this.RaisePropertiesChanged(nameof(MinimumPrice), nameof(HasFilter), nameof(IsValid));
                }
            }
        }
        private int _maximumPrice;
        /// <summary>
        /// Gets or sets the maximum price to expect
        /// </summary>
        public int MaximumPrice
        {
            get { return _maximumPrice; }
            private set
            {
                if (_maximumPrice != value)
                {
                    _maximumPrice = value;
                    this.RaisePropertiesChanged(nameof(MaximumPrice), nameof(HasFilter), nameof(IsValid));
                }
            }
        }
        private string? _group;
        /// <summary>
        /// Gets or sets the group to expect
        /// </summary>
        public string? Group
        {
            get { return _group; }
            set
            {
                if (!string.Equals(_group, value, StringComparison.Ordinal))
                {
                    _group = value;
                    this.RaisePropertiesChanged(nameof(Group), nameof(HasFilter), nameof(IsValid));
                }
            }
        }
        /// <summary>
        /// Gets or sets a collection of options for <see cref="Group"/>
        /// </summary>
        public string[]? GroupOptions { get; }
        private bool? _isIntelligent;
        /// <summary>
        /// Gets or sets whether or not to filter by intelligence
        /// </summary>
        public bool? IsIntelligent
        {
            get { return _isIntelligent; }
            private set
            {
                if (_isIntelligent != value)
                {
                    _isIntelligent = value;
                    this.RaisePropertiesChanged(nameof(IsIntelligent), nameof(HasFilter), nameof(IsValid));
                }
            }
        }
        private ArtifactLevel _artifactLevel;
        /// <summary>
        /// Gets or sets whether or not to filter by artifact level
        /// </summary>
        public ArtifactLevel ArtifactLevel
        {
            get { return _artifactLevel; }
            private set
            {
                if (_artifactLevel != value)
                {
                    _artifactLevel = value;
                    this.RaisePropertiesChanged(nameof(ArtifactLevel), nameof(HasFilter), nameof(IsValid));
                }
            }
        }
        private bool? _mythic;
        /// <summary>
        /// Gets or sets whether to filter by mythic
        /// </summary>
        public bool? Mythic
        {
            get { return _mythic; }
            private set
            {
                if (_mythic != value)
                {
                    _mythic = value;
                    this.RaisePropertiesChanged(nameof(Mythic), nameof(HasFilter), nameof(IsValid));
                }
            }
        }
        /// <summary>
        /// Gets whether or not this filter is set
        /// </summary>
        public override bool HasFilter
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Name)
                    || !string.IsNullOrWhiteSpace(CasterLevel)
                    || !string.IsNullOrWhiteSpace(Slot)
                    || MinimumPrice != 0
                    || MaximumPrice != 0
                    || !string.IsNullOrWhiteSpace(Group)
                    || IsIntelligent.HasValue
                    || Mythic.HasValue
                    || ArtifactLevel != ArtifactLevel.None;
            }
        }
        /// <summary>
        /// Gets or sets whether or not this filter is valid
        /// </summary>
        public override bool IsValid => HasFilter;
        #endregion
        #region Methods
        /// <summary>
        /// Gets whether or not the given item matches the filter
        /// </summary>
        /// <param name="item">Item to check for filter</param>
        /// <returns>Whether or not the item is a match</returns>
        public override bool Matches(MagicItem item)
        {
            bool matches = true;

            if (!string.IsNullOrWhiteSpace(Name))
                matches &= IFilterableExtensions.MatchesFilter(Name, item.Name);
            if (!string.IsNullOrWhiteSpace(CasterLevel) && Int32.TryParse(CasterLevel, NumberStyles.Integer, CultureInfo.CurrentCulture, out int level))
                matches &= level == item.CasterLevel;
            if (!string.IsNullOrWhiteSpace(Slot))
                matches &= string.Equals(Slot, item.Slot, StringComparison.CurrentCultureIgnoreCase);
            if (MinimumPrice != 0)
                matches &= item.PriceInCopper >= MinimumPrice;
            if (MaximumPrice != 0)
                matches &= item.PriceInCopper <= MaximumPrice;
            if (!string.IsNullOrWhiteSpace(Group))
                matches &= string.Equals(Group, item.Group, StringComparison.CurrentCultureIgnoreCase);
            if (ArtifactLevel != ArtifactLevel.None)
                matches &= item.ArtifactLevel == ArtifactLevel;
            if (IsIntelligent.HasValue)
                matches &= item.IsIntelligent == IsIntelligent.Value;
            if (Mythic.HasValue)
                matches &= item.Mythic == Mythic.Value;

            return matches;
        }
        /// <summary>
        /// Resets the filter back to default
        /// </summary>
        public override void Reset()
        {
            Name = string.Empty;
            CasterLevel = string.Empty;
            Slot = string.Empty;
            MinimumPrice = 0;
            MaximumPrice = 0;
            Group = string.Empty;
            ArtifactLevel = ArtifactLevel.None;
            IsIntelligent = null;
            Mythic = null;
        }
        #endregion
    }
}
