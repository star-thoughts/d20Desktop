using Fiction.GameScreen.Monsters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for filtering monsters
    /// </summary>
    public sealed class MonsterFilterViewModel : FilterViewModelCore<Monster>
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="MonsterFilterViewModel"/>
        /// </summary>
        public MonsterFilterViewModel(CampaignSettings campaign)
        {
            Campaign = campaign;

            ChallengeRatings = Campaign.MonsterManager.Monsters
                    .Select(p => p.Stats["challengeRating"]?.Value)
                    .OfType<string>()
                    .Where(p => !string.IsNullOrWhiteSpace(p))
                    .Distinct(StringComparer.CurrentCultureIgnoreCase)
                    .ToArray();
            SubTypes = new ObservableCollection<string>();
            SubTypes.CollectionChanged += SubTypes_CollectionChanged;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the campaign
        /// </summary>
        public CampaignSettings Campaign { get; private set; }
        /// <summary>
        /// Gets the groups that can be filtered by
        /// </summary>
        public IEnumerable<string> GroupOptions { get { return Campaign.MonsterManager.Groups; } }
        /// <summary>
        /// Gets the types that can be filtered by
        /// </summary>
        public IEnumerable<string> TypeOptions { get { return Campaign.MonsterManager.Types; } }
        /// <summary>
        /// Gets the subtypes that can be filtered by
        /// </summary>
        public IEnumerable<string> SubTypeOptions { get { return Campaign.MonsterManager.SubTypes; } }
        /// <summary>
        /// Gets a collection of challenge ratings
        /// </summary>
        public IEnumerable<string> ChallengeRatings { get; private set; }
        private string? _name;
        /// <summary>
        /// Gets or sets the name filter
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
        private string? _group;
        /// <summary>
        /// Gets or sets the group to filter by
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
        private string? _type;
        /// <summary>
        /// Gets or sets the type to filter by
        /// </summary>
        public string? Type
        {
            get { return _type; }
            set
            {
                if (!string.Equals(_type, value, StringComparison.Ordinal))
                {
                    _type = value;
                    this.RaisePropertiesChanged(nameof(Type), nameof(HasFilter), nameof(IsValid));
                }
            }
        }
        /// <summary>
        /// Gets or sets the sub types to filter by
        /// </summary>
        public ObservableCollection<string> SubTypes { get; private set; }
        /// <summary>
        /// Gets whether or not this is a valid view model
        /// </summary>
        public override bool IsValid => HasFilter;
        /// <summary>
        /// Gets whether or not this filter has data in it
        /// </summary>
        public override bool HasFilter
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Name)
                    || !string.IsNullOrWhiteSpace(Group)
                    || !string.IsNullOrWhiteSpace(Type)
                    || SubTypes.Any();
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Determines whether or not a monster matches the filter
        /// </summary>
        /// <param name="monster">Monster to test</param>
        /// <returns>Whether or not the monster matches the filter</returns>
        public override bool Matches(Monster monster)
        {
            bool matches = true;
            if (!string.IsNullOrWhiteSpace(Name))
                matches &= IFilterableExtensions.MatchesFilter(Name, monster.Name);
            if (!string.IsNullOrWhiteSpace(Group))
                matches &= string.Equals(Group, monster.Stats["group"]?.Value as string, StringComparison.CurrentCultureIgnoreCase);
            if (!string.IsNullOrWhiteSpace(Type))
                matches &= string.Equals(Type, monster.Stats["type"]?.Value as string, StringComparison.CurrentCultureIgnoreCase);
            if (SubTypes.Any())
            {
                if (monster.Stats["subType"]?.Value is IEnumerable<string> subTypes)
                    matches &= SubTypes.All(p => subTypes.Contains(p));
                else
                    matches = false;
            }

            return matches;
        }

        /// <summary>
        /// Resets this view model back to default
        /// </summary>
        public override void Reset()
        {
            Name = string.Empty;
            Group = string.Empty;
            Type = string.Empty;
            SubTypes.Clear();
        }

        private void SubTypes_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.RaisePropertiesChanged(nameof(IsValid), nameof(HasFilter));
        }
        #endregion
    }
}