using Fiction.GameScreen.Combat;
using Fiction.GameScreen.Equipment;
using Fiction.GameScreen.Monsters;
using Fiction.GameScreen.Players;
using Fiction.GameScreen.Spells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Fiction.GameScreen
{
    /// <summary>
    /// Details and information about a specific campaign
    /// </summary>
    public sealed class CampaignSettings : INotifyPropertyChanged
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="CampaignSettings"/>
        /// </summary>
        public CampaignSettings()
        {
            _objects = new Dictionary<int, ICampaignObject>();
            Combat = new CombatManager();
            MonsterManager = new MonsterManager();
            Players = new PlayerManager();
            Sources = new ObservableCollection<string>();
            Spells = new SpellManager();
            EquipmentManager = new EquipmentManager();
            Options = new CampaignOptions();
            Conditions = new ConditionManager();
            _nextId = 1;

            Combat.Scenarios.CollectionChanged += CampaignObject_CollectionChanged;
            MonsterManager.Monsters.CollectionChanged += CampaignObject_CollectionChanged;
            Players.PlayerCharacters.CollectionChanged += CampaignObject_CollectionChanged;
            Spells.Spells.CollectionChanged += CampaignObject_CollectionChanged;
            EquipmentManager.MagicItems.CollectionChanged += CampaignObject_CollectionChanged;

            _sourceManagers = new ISourcedItemManager[]
            {
                MonsterManager, Spells, EquipmentManager
            };
        }
        #endregion
        #region Member Variables
        private int _nextId;
        private Dictionary<int, ICampaignObject> _objects;
        private ISourcedItemManager[] _sourceManagers;
        #endregion
        #region Properties
        /// <summary>
        /// Gets the combat scenarios in the campaign
        /// </summary>
        public CombatManager Combat { get; private set; }
        /// <summary>
        /// Gets the manager used for combat conditions
        /// </summary>
        public ConditionManager Conditions { get; private set; }
        /// <summary>
        /// Gets the monster manager for the campaign
        /// </summary>
        public MonsterManager MonsterManager { get; private set; }
        /// <summary>
        /// Gets the player manager for this campaign
        /// </summary>
        public PlayerManager Players { get; private set; }
        /// <summary>
        /// Gets the spell manager for this campaign
        /// </summary>
        public SpellManager Spells { get; private set; }
        /// <summary>
        /// Gets the manager for equipment and magic items
        /// </summary>
        public EquipmentManager EquipmentManager { get; private set; }
        /// <summary>
        /// Gets a collection of sources in the campaign
        /// </summary>
        public ObservableCollection<string> Sources { get; private set; }
        /// <summary>
        /// Gets the options for this campaign
        /// </summary>
        public CampaignOptions Options { get; private set; }
        private string? _serverUri;
        /// <summary>
        /// Gets or sets the URI to use for connecting to a campaign server
        /// </summary>
        public string? ServerUri
        {
            get { return _serverUri; }
            set
            {
                if (!string.Equals(_serverUri, value))
                {
                    _serverUri = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ServerUri)));
                }
            }
        }
        private string? _campaignID;
        /// <summary>
        /// Gets or sets the ID of the campaign from the campaign server
        /// </summary>
        public string? CampaignID
        {
            get { return _campaignID; }
            set
            {
                if (!string.Equals(_campaignID, value))
                {
                    _campaignID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CampaignID)));
                }
            }
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
        #region Methods
        /// <summary>
        /// Gets the given campaign object
        /// </summary>
        /// <typeparam name="T">Type of object expected</typeparam>
        /// <param name="id">ID of the object to get</param>
        /// <returns>Item requested, or null if item does not exist or is the wrong type</returns>
        public T? GetCampaignObject<T>(int id) where T : class, ICampaignObject
        {
            if (_objects.TryGetValue(id, out ICampaignObject? item) && item is T result)
                return result;
            return null;
        }
        /// <summary>
        /// Resets the next ID to use
        /// </summary>
        internal void UpdateNextId(int nextId)
        {
            _nextId = nextId;
        }
        /// <summary>
        /// Gets the next ID to use for a campaign object
        /// </summary>
        /// <returns>ID to use</returns>
        public int GetNextId()
        {
            return _nextId++;
        }

        private void CampaignObject_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (ICampaignObject o in e.NewItems)
                    _objects[o.Id] = o;
            }
            else if (e.OldItems != null)
            {
                foreach (ICampaignObject o in e.OldItems)
                    _objects.Remove(o.Id);
            }
        }
        /// <summary>
        /// Resets the <see cref="Sources"/> list using all sources in each item in the campaign
        /// </summary>
        public void ReconcileSources()
        {
            string[] sources = Spells.Spells
                .Select(p => p.Source)
                .Concat(MonsterManager.Monsters.Select(p => p.Source))
                .Select(p => p ?? string.Empty)
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .ToArray();

            Sources.Clear();
            foreach (string source in sources)
                Sources.Add(source);
        }
        /// <summary>
        /// Removes the given source, also removing anything from that source
        /// </summary>
        /// <param name="source">Source to remove</param>
        public void RemoveSource(string source)
        {
            Exceptions.ThrowIfArgumentNull(source, nameof(source));

            foreach (ISourcedItemManager manager in _sourceManagers)
                manager.RemoveSource(source);

            Sources.Remove(source);
        }
        /// <summary>
        /// Renames the given source, or moves all items to another source
        /// </summary>
        /// <param name="original">Original source to rename or move from</param>
        /// <param name="source">New source name</param>
        public void RenameSource(string original, string source)
        {
            Exceptions.ThrowIfArgumentNull(original, nameof(original));
            Exceptions.ThrowIfArgumentNullOrEmpty(source, nameof(source));

            if (!string.Equals(original, source, StringComparison.CurrentCulture))
            {
                ISourcedItem[] items = GetAllSourcedItems(original);

                foreach (ISourcedItem item in items)
                    item.Source = source;

                Sources.Remove(original);
                //   Only add if this was a rename and not a merge
                if (!Sources.Contains(source, StringComparer.CurrentCultureIgnoreCase))
                    Sources.Add(source);
            }
        }

        public ISourcedItem[] GetAllSourcedItems(string source)
        {
            return _sourceManagers.SelectMany(p => p.GetItemsFromSource(source))
                .ToArray();
        }
        #endregion
    }
}
