using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Used to start combat, resolve initiatives and roll hit points
    /// </summary>
    public class CombatPreparer : INotifyPropertyChanged, IInitiativeCollection
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="CombatPreparer"/>
        /// </summary>
        /// <param name="scenario">Combat scenario to use to start the combat</param>
        public CombatPreparer(CombatScenario scenario)
        {
            Exceptions.ThrowIfArgumentNull(scenario, nameof(scenario));
            _campaign = scenario.Campaign;

            Combatants = new ObservableCollection<CombatantPreparer>();
            AddCombatants(scenario);
        }
        /// <summary>
        /// Constructs a new <see cref="CombatPreparer"/>
        /// </summary>
        /// <param name="campaign">Campaign this combat is for</param>
        public CombatPreparer(CampaignSettings campaign)
        {
            _campaign = campaign;
            Combatants = new ObservableCollection<CombatantPreparer>();
            AddPlayers();
        }

        /// <summary>
        /// Constructs a new <see cref="CombatPreparer"/> for adding to an existing combat
        /// </summary>
        /// <param name="campaign">Campaign this combat is taking place in</param>
        /// <param name="combat">Combat to add to</param>
        public CombatPreparer(CampaignSettings campaign, ActiveCombat combat)
        {
            Exceptions.ThrowIfArgumentNull(combat, nameof(combat));

            _campaign = campaign;

            SourceCombat = combat;
            Combatants = new ObservableCollection<CombatantPreparer>();
        }
        #endregion
        #region Member Variables
        private CampaignSettings _campaign;
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the combat that this preparation is for
        /// </summary>
        public ActiveCombat? SourceCombat { get; private set; }
        /// <summary>
        /// Gets a collection of combatants for the battle
        /// </summary>
        public ObservableCollection<CombatantPreparer> Combatants { get; private set; }
        /// <summary>
        /// Gets a collection of combatants for the battle
        /// </summary>
        IEnumerable<IActiveCombatant> IInitiativeCollection.Combatants { get { return this.Combatants; } }
        #endregion
        #region Methods
        public void Reset()
        {
            //  Remove all combatants
            Combatants.Clear();

            //  If this is a new combat, then add the players back
            if (SourceCombat == null)
                AddPlayers();
        }

        private void AddPlayers()
        {
            IEnumerable<CombatantPreparer> preparers = _campaign.Players.PlayerCharacters
               .Where(p => p.IncludeInCombat)
               .SelectMany(p => p.Prepare(this));

            foreach (CombatantPreparer preparer in preparers)
            {
                preparer.RollHitPoints();
                Combatants.Add(preparer);
            }
        }

        /// <summary>
        /// Adds the combatants from the given scenario
        /// </summary>
        /// <param name="source">Source to add combatants from</param>
        /// <returns>Combatants added to combat</returns>
        public CombatantPreparer[] AddCombatants(ICombatantSource source)
        {
            CombatantPreparer[] newCombatants = source.Prepare(this).ToArray();

            foreach (CombatantPreparer preparer in newCombatants)
            {
                preparer.RollInitiative();
                preparer.RollHitPoints();
                Combatants.Add(preparer);
            }

            ResolveOrdinals();
            return newCombatants;
        }

        /// <summary>
        /// Resolves the ordinals of all combatants
        /// </summary>
        public void ResolveOrdinals()
        {
            CombatantPreparer[] original = SourceCombat?.Combatants?.Select(p => p.PreparedInfo).ToArray()
                ?? Array.Empty<CombatantPreparer>();

            Dictionary<string, CombatantPreparer[]> byName = original.Concat(Combatants)
                .GroupBy(p => p.Name)
                .ToDictionary(p => p.Key, p => p.ToArray());

            foreach (string name in byName.Keys)
            {
                CombatantPreparer[] combatants = byName[name];
                //  If the only one, it has a 0 ordinal
                if (combatants.Length == 1)
                {
                    combatants[0].Ordinal = 0;
                }
                else
                {
                    int ordinal = combatants.Max(p => p.Ordinal) + 1;
                    //  Now take all combatants with a 0 ordinal and set it to a proper ordinal
                    foreach (CombatantPreparer preparer in combatants.Where(p => p.Ordinal == 0))
                        preparer.Ordinal = ordinal++;
                }
            }
        }

        /// <summary>
        /// Resolves initiative orders and applies them to the combatants
        /// </summary>
        /// <returns>Whether or not there are any ties</returns>
        public bool ResolveInitiatives()
        {
            int[] rolls = Combatants.Select(p => p.InitiativeTotal)
                .Distinct()
                .OrderByDescending(p => p)
                .ToArray();

            int order = 1;
            bool hasTie = false;
            int group = 0;

            if (SourceCombat != null && SourceCombat.Combatants.Any())
                order = SourceCombat.Combatants.Max(p => p.InitiativeOrder) + 1;

            foreach (int roll in rolls)
            {
                CombatantPreparer[] combatants = Combatants
                    .Where(p => p.InitiativeTotal == roll)
                    .OrderByDescending(p => p.InitiativeModifier)
                    .ToArray();

                int lastModifier = Int32.MinValue;

                foreach (CombatantPreparer preparer in combatants)
                {
                    if (lastModifier == preparer.InitiativeModifier)
                    {
                        hasTie = true;
                        preparer.InitiativeOrder = order;
                        preparer.InitiativeGroup = group;
                    }
                    else
                    {
                        preparer.InitiativeOrder = order;
                        preparer.InitiativeGroup = ++group;
                    }
                    order++;
                    lastModifier = preparer.InitiativeModifier;
                }
            }

            return hasTie;
        }
        /// <summary>
        /// Resolves initiative order and creates combatants in order
        /// </summary>
        /// <returns>Combatants to add to an active combat</returns>
        public ICombatant[] CreateCombatants()
        {
            return Combatants
                .Select(p => p.Source?.CreateCombatant(p))
                .OfType<Combatant>()
                .ToArray();
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
