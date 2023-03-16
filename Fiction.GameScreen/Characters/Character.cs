using Fiction.GameScreen.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Represents a character (player or non-player)
    /// </summary>
    public class Character : IAttributeContainer, IModifierContainer
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="Character"/> using a campaign
        /// </summary>
        /// <param name="serializer">Serializer to use for character information</param>
        /// <param name="campaign">Campaign the character is in</param>
        public Character(ICampaignSerializer serializer)
        {
            Exceptions.ThrowIfArgumentNull(serializer, nameof(serializer));

            Initialize(serializer, new AttributeManager(serializer, new AttributeModifierManager(), this), new SpecialsManager());
        }

        /// <summary>
        /// Constructs a new <see cref="Character"/> using an attribute manager
        /// </summary>
        /// <param name="serializer">Serializer to use for character information</param>
        /// <param name="attributeManager">Attribute manager to use</param>
        /// <param name="qualityManager">Special quality manager to use</param>
        public Character(ICampaignSerializer serializer, IAttributeManager attributeManager, ISpecialsManager qualityManager)
        {
            Initialize(serializer, attributeManager, qualityManager);
        }

        private void Initialize(ICampaignSerializer serializer, IAttributeManager attributeManager, ISpecialsManager qualityManager)
        {
            _serializer = serializer;

            Modifiers = ImmutableArray<AttributeModifier>.Empty;
            Attributes = new AttributeCollection(serializer);
            AttributeManager = attributeManager;
            ClassLevels = ImmutableArray<SelectedLevel>.Empty;
            SpecialQualityManager = qualityManager;
            SpecialQualities = new SpecialQualityCollection();
        }
        #endregion
        #region Member Variables
        private ICampaignSerializer _serializer;
        #endregion
        #region Properties
        /// <summary>
        /// Gets the race for this character
        /// </summary>
        public CharacterRace Race { get; set; }
        /// <summary>
        /// Gets or sets the levels the character has taken in classes
        /// </summary>
        public ImmutableArray<SelectedLevel> ClassLevels { get; private set; }
        /// <summary>
        /// Gets the <see cref="AttributeManager"/> used to calculate attributes
        /// </summary>
        public IAttributeManager AttributeManager { get; private set; }
        /// <summary>
        /// Gets the attributes on this <see cref="IAttributeContainer"/>
        /// </summary>
        public AttributeCollection Attributes { get; private set; }
        /// <summary>
        /// Gets the special qualities on this character
        /// </summary>
        public SpecialQualityCollection SpecialQualities { get; private set; }
        /// <summary>
        /// Gets the <see cref="SpecialQualityManager"/> used to maintain what special qualities are applied to this character
        /// </summary>
        public ISpecialsManager SpecialQualityManager { get; private set; }
        /// <summary>
        /// Gets or sets a collection of attribute modifiers
        /// </summary>
        public ImmutableArray<AttributeModifier> Modifiers { get; set; }
        #endregion
        #region Methods
        /// <summary>
        /// Adds the given selected class level to the character
        /// </summary>
        /// <param name="level">Information about the level to add</param>
        public void AddClassLevel(SelectedLevel level)
        {
            ClassLevels = ClassLevels.Add(level);
        }
        /// <summary>
        /// Removes the highest class level
        /// </summary>
        public void RemoveClassLevel()
        {
            if (ClassLevels.Any())
                ClassLevels = ClassLevels.Remove(ClassLevels.Last());
        }
        /// <summary>
        /// Gets the final modified information for an attribute
        /// </summary>
        /// <param name="definition">Definition of the attribute to get</param>
        /// <returns>Calculated information about an attribute</returns>
        public async Task<CalculatedAttribute> GetAttributeAsync(AttributeDefinition definition)
        {
            await ResolveAllSpecialQualities();

            return await InnerGetAttributeAsync(definition);
        }

        internal async Task<CalculatedAttribute> InnerGetAttributeAsync(AttributeDefinition definition)
        {
            IModifierContainer[] containers = GetModifierContainers();

            CalculatedAttribute result = await AttributeManager.GetOrCalculateAttributeAsync(definition, containers);

            return result;
        }

        /// <summary>
        /// Resolves special qualities to make sure the proper ones are applied
        /// </summary>
        /// <returns></returns>
        public async Task ResolveAllSpecialQualities()
        {
            SelectedSpecialQuality[] specialQualities = GetAllSelectedSpecialQualities();
            CalculatedSpecial[] calculated = specialQualities.Select(p => new CalculatedSpecial(p))
                .ToArray();
            SpecialQualities.Qualities = ImmutableArray.Create(calculated);

            await SpecialQualityManager.ReexamineQualities(SpecialQualities.Qualities.ToArray(), this);
        }

        private SelectedSpecialQuality[] GetAllSelectedSpecialQualities()
        {
            List<ISelectedItem> qualitySources = new List<ISelectedItem>();

            if (Race != null)
                qualitySources.Add(Race);
            qualitySources.AddRange(ClassLevels);

            return qualitySources.SelectMany(p => p.SpecialQualities)
                .ToArray();
        }

        private IModifierContainer[] GetModifierContainers()
        {
            List<IModifierContainer> containers = new List<IModifierContainer>();

            containers.Add(this);
            if (Race != null)
                containers.Add(Race);

            foreach (SelectedLevel level in ClassLevels)
                containers.Add(level);

            containers.AddRange(SpecialQualities.Qualities);

            return containers.ToArray();
        }

        /// <summary>
        /// Gets the base modifiers on this character, usually includes things such as starting ability scores
        /// </summary>
        /// <param name="data">Information about the calculations being done</param>
        /// <returns>Modifiers used for calculation of the given attribute</returns>
        public AttributeModifier[] GetAttributeModifiers(AttributeCalculationData data)
        {
            return Modifiers.ToArray();
        }
        #endregion
    }
}
