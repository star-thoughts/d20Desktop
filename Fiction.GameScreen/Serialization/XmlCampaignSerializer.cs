
using Fiction.GameScreen.Combat;
using Fiction.GameScreen.Equipment;
using Fiction.GameScreen.Monsters;
using Fiction.GameScreen.Players;
using Fiction.GameScreen.Spells;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;

namespace Fiction.GameScreen.Serialization
{
    /// <summary>
    /// Serializes combat data to XML file
    /// </summary>
    public sealed class XmlCampaignSerializer
    {
        #region Constructors
        public XmlCampaignSerializer(ReaderWriterGenerator generator)
        {
            _generator = generator;
            _campaignItems = new Dictionary<int, ICampaignObject>();
            _idToString = new Dictionary<int, string>();
            _stringToId = new Dictionary<string, int>();
        }
        #endregion
        #region Member Variables
        private ReaderWriterGenerator _generator;
        private Dictionary<int, ICampaignObject> _campaignItems;
        private Dictionary<int, string> _idToString;
        private Dictionary<string, int> _stringToId;
        private int _nextStringId;

        class Keys
        {
            public const string Id = "id";
            public const string Reference = "ref";
            public const string Campaign = "campaign";
            public const string ServerID = "serverid";
            public const string Scenario = "scenario";
            public const string Name = "name";
            public const string Group = "group";
            public const string Details = "details";
            public const string CombatantTemplate = "combTemp";
            public const string Kind = "kind";
            public const string CombatantTemplateKind = "generic";
            public const string HitDice = "hitDice";
            public const string HitDiceStrategy = "hdStrategy";
            public const string InitiativeModifier = "initMod";
            public const string Count = "count";
            public const string DisplayToPlayers = "display";
            public const string DisplayName = "displayName";
            public const string FastHealing = "fastHealing";
            public const string Monster = "monster";
            public const string Value = "value";
            public const string Stats = "stats";
            public const string Source = "source";
            public const string Player = "player";
            public const string IncludeInCombat = "include";
            public const string ActiveCombat = "combat";
            public const string Combatant = "combatant";
            public const string Current = "current";
            public const string Round = "round";
            public const string Ordinal = "ordinal";
            public const string PreparedInfo = "prepared";
            public const string InitiativeOrder = "order";
            public const string HasGoneOnce = "gone";
            public const string Health = "health";
            public const string MaxHealth = "max";
            public const string LethalDamage = "lethal";
            public const string NonlethalDamage = "nonlethal";
            public const string Temporary = "temp";
            public const string DeadAt = "dead";
            public const string UnconsciousAt = "uncon";
            public const string InitiativeRoll = "initRoll";
            public const string InitiativeGroup = "group";
            public const string IsPlayer = "player";
            public const string HitPoints = "hp";
            public const string School = "school";
            public const string Subschool = "subschool";
            public const string CastingTime = "castingTime";
            public const string Level = "level";
            public const string Components = "components";
            public const string CostlyComponents = "costly";
            public const string Range = "range";
            public const string Area = "area";
            public const string Effect = "effect";
            public const string Targets = "targets";
            public const string Dismissible = "dismiss";
            public const string Shapeable = "shape";
            public const string SavingThrow = "save";
            public const string SpellResistance = "sr";
            public const string Description = "desc";
            public const string ShortDesc = "short";
            public const string EffectType = "type";
            public const string String = "string";
            public const string Spell = "spell";
            public const string Duration = "duration";
            public const string DeadAtOption = "deadAtOption";
            public const string MagicItem = "item";
            public const string CasterLevel = "level";
            public const string Slot = "slot";
            public const string Price = "price";
            public const string Weight = "weight";
            public const string Requirement = "req";
            public const string Cost = "cost";
            public const string Intelligence = "int";
            public const string Wisdom = "wis";
            public const string Charisma = "cha";
            public const string Ego = "ego";
            public const string Communication = "comm";
            public const string Senses = "senses";
            public const string Powers = "powers";
            public const string Languages = "languages";
            public const string Descruction = "dest";
            public const string ArtifactLevel = "artLevel";
            public const string Aura = "aura";
            public const string AuraStrength = "strength";
            public const string Mythic = "mythic";
            public const string Alignment = "alignment";
            public const string Condition = "condition";
            public const string DamageReduction = "dr";
            public const string LightRadius = "light";
            public const string Notes = "note";
        }
        #endregion
        #region Methods
        private async Task WriteCachedStringAsync(XmlWriter writer, string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (!_stringToId.TryGetValue(value, out int id))
                {
                    id = _nextStringId++;

                    _idToString[id] = value;
                    _stringToId[value] = id;
                }

                await writer.WriteAttributeStringAsync(key, id.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            }
        }

        private async Task WriteCachedStringsAsync(XmlWriter writer)
        {
            foreach (KeyValuePair<int, string> pair in _idToString)
            {
                await writer.WriteStartElementAsync(Keys.String).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.Id, pair.Key.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
                await writer.WriteStringAsync(pair.Value).ConfigureAwait(false);
                await writer.WriteEndElementAsync().ConfigureAwait(false);
            }
        }

        private void ReadCachedStrings(XElement reader)
        {
            foreach (XElement value in reader.Descendants(Keys.String))
            {
                string s = value.Value;
                int id = value.ReadAttributeInt(Keys.Id);
                if (id != -1)
                {
                    _idToString[id] = s;
                    _stringToId[s] = id;
                }
            }
        }

        private string ReadCachedString(XElement reader, string key)
        {
            int id = reader.ReadAttributeInt(key);
            if (_idToString.TryGetValue(id, out string? result))
                return result;

            return string.Empty;
        }

        private void StoreCampaignObject(ICampaignObject item)
        {
            _campaignItems[item.Id] = item;
        }

        public async Task WriteCampaign(Stream stream, CampaignSettings campaign)
        {
            using (XmlWriter writer = _generator.GetWriter(stream))
                await WriteCampaign(writer, campaign).ConfigureAwait(false);
        }
        public async Task WriteCampaign(XmlWriter writer, CampaignSettings campaign)
        {
            _campaignItems.Clear();
            await writer.WriteStartElementAsync(Keys.Campaign).ConfigureAwait(false);

            await writer.WriteOptionalAttributeStringAsync(Keys.ServerID, campaign.CampaignID).ConfigureAwait(false);

            await writer.WriteAttributeStringAsync(Keys.DeadAtOption, campaign.Options.MonsterDeadAtOption.ToString()).ConfigureAwait(false);
            if (campaign.Options.MonsterDeadAtOption == MonsterDeadAtDefaultOption.SetValue)
                await writer.WriteAttributeStringAsync(Keys.DeadAt, campaign.Options.MonsterDefaultDeadAt.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);

            await WritePlayers(writer, campaign.Players).ConfigureAwait(false);
            await WriteMonsters(writer, campaign.MonsterManager).ConfigureAwait(false);
            await WriteCombatScenariosAsync(writer, campaign.Combat.Scenarios).ConfigureAwait(false);
            await WriteConditionsAsync(writer, campaign.Conditions.Conditions).ConfigureAwait(false);
            await WriteSpellsAsync(writer, campaign.Spells.Spells).ConfigureAwait(false);
            await WriteEquipmentAsync(writer, campaign.EquipmentManager).ConfigureAwait(false);

            //  The following must happen last
            await WriteCachedStringsAsync(writer).ConfigureAwait(false);

            await writer.WriteEndElementAsync().ConfigureAwait(false);
        }

        private async Task WriteConditionsAsync(XmlWriter writer, ObservableCollection<Condition> conditions)
        {
            foreach (Condition condition in conditions)
            {
                await writer.WriteStartElementAsync(Keys.Condition).ConfigureAwait(false);

                await writer.WriteAttributeStringAsync(Keys.Name, condition.Name).ConfigureAwait(false);

                await writer.WriteStartElementAsync(Keys.Description).ConfigureAwait(false);
                await writer.WriteStringAsync(condition.Description).ConfigureAwait(false);
                await writer.WriteEndElementAsync().ConfigureAwait(false);

                await writer.WriteEndElementAsync().ConfigureAwait(false);
            }
        }

        private async Task WriteEquipmentAsync(XmlWriter writer, EquipmentManager equipmentManager)
        {
            foreach (MagicItem item in equipmentManager.MagicItems)
            {
                await writer.WriteStartElementAsync(Keys.MagicItem).ConfigureAwait(false);

                await writer.WriteAttributeStringAsync(Keys.Id, item.Id.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.Name, item.Name).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.CasterLevel, item.CasterLevel.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
                await WriteCachedStringAsync(writer, Keys.Slot, item.Slot ?? string.Empty).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.Price, item.PriceInCopper.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.Cost, item.CostInCopper.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.Weight, item.Weight ?? string.Empty).ConfigureAwait(false);
                await WriteCachedStringAsync(writer, Keys.Group, item.Group ?? string.Empty).ConfigureAwait(false);
                await WriteCachedStringAsync(writer, Keys.Source, item.Source ?? string.Empty).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.Alignment, item.Alignment.ToString()).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.Intelligence, item.Intelligence.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.Wisdom, item.Wisdom.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.Charisma, item.Charisma.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.Ego, item.Ego.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
                await WriteCachedStringAsync(writer, Keys.Communication, item.Communication ?? string.Empty).ConfigureAwait(false);
                await WriteCachedStringAsync(writer, Keys.Senses, item.Senses ?? string.Empty).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.ArtifactLevel, item.ArtifactLevel.ToString()).ConfigureAwait(false);
                await WriteCachedStringAsync(writer, Keys.AuraStrength, item.AuraStrength ?? string.Empty).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.Mythic, item.Mythic.ToString()).ConfigureAwait(false);

                await writer.WriteStartElementAsync(Keys.Description).ConfigureAwait(false);
                await writer.WriteStringAsync(item.Description).ConfigureAwait(false);
                await writer.WriteEndElementAsync().ConfigureAwait(false);

                await writer.WriteStartElementAsync(Keys.Requirement).ConfigureAwait(false);
                await writer.WriteStringAsync(item.Requirements).ConfigureAwait(false);
                await writer.WriteEndElementAsync().ConfigureAwait(false);

                await writer.WriteStartElementAsync(Keys.Powers).ConfigureAwait(false);
                await writer.WriteStringAsync(item.Powers).ConfigureAwait(false);
                await writer.WriteEndElementAsync().ConfigureAwait(false);

                await writer.WriteStartElementAsync(Keys.Languages).ConfigureAwait(false);
                await writer.WriteStringAsync(item.Languages).ConfigureAwait(false);
                await writer.WriteEndElementAsync().ConfigureAwait(false);

                await writer.WriteStartElementAsync(Keys.Descruction).ConfigureAwait(false);
                await writer.WriteStringAsync(item.Descruction).ConfigureAwait(false);
                await writer.WriteEndElementAsync().ConfigureAwait(false);

                foreach (string aura in item.Auras)
                {
                    await writer.WriteStartElementAsync(Keys.Aura).ConfigureAwait(false);
                    await WriteCachedStringAsync(writer, Keys.Aura, aura).ConfigureAwait(false);
                    await writer.WriteEndElementAsync().ConfigureAwait(false);
                }

                await writer.WriteEndElementAsync().ConfigureAwait(false);
            }
        }

        private async Task WriteSpellsAsync(XmlWriter writer, IEnumerable<Spell> spells)
        {
            foreach (Spell spell in spells)
            {
                await writer.WriteStartElementAsync(Keys.Spell).ConfigureAwait(false);

                await writer.WriteAttributeStringAsync(Keys.Id, spell.Id.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.Name, spell.Name ?? string.Empty).ConfigureAwait(false);
                await WriteCachedStringAsync(writer, Keys.School, spell.School ?? string.Empty).ConfigureAwait(false);
                await WriteCachedStringAsync(writer, Keys.Subschool, spell.SubSchool ?? string.Empty).ConfigureAwait(false);
                await WriteCachedStringAsync(writer, Keys.Level, spell.Levels.GetInvariantSpellLevelString()).ConfigureAwait(false);
                await WriteCachedStringAsync(writer, Keys.CastingTime, spell.CastingTime ?? string.Empty).ConfigureAwait(false);
                await WriteCachedStringAsync(writer, Keys.Components, spell.Components ?? string.Empty).ConfigureAwait(false);
                await WriteCachedStringAsync(writer, Keys.CostlyComponents, spell.CostlyComponents ?? string.Empty).ConfigureAwait(false);
                await WriteCachedStringAsync(writer, Keys.Range, spell.Range ?? string.Empty).ConfigureAwait(false);
                await WriteCachedStringAsync(writer, Keys.Area, spell.Area ?? string.Empty).ConfigureAwait(false);
                await WriteCachedStringAsync(writer, Keys.Effect, spell.Effect ?? string.Empty).ConfigureAwait(false);
                await WriteCachedStringAsync(writer, Keys.Targets, spell.Targets ?? string.Empty).ConfigureAwait(false);
                await WriteCachedStringAsync(writer, Keys.Duration, spell.Duration ?? string.Empty).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.Dismissible, spell.Dismissible.ToString()).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.Shapeable, spell.Shapeable.ToString()).ConfigureAwait(false);
                await WriteCachedStringAsync(writer, Keys.SavingThrow, spell.SavingThrow ?? string.Empty).ConfigureAwait(false);
                await WriteCachedStringAsync(writer, Keys.SpellResistance, spell.SpellResistance ?? string.Empty).ConfigureAwait(false);
                await WriteCachedStringAsync(writer, Keys.Source, spell.Source ?? string.Empty).ConfigureAwait(false);

                if (!string.IsNullOrWhiteSpace(spell.Description))
                {
                    await writer.WriteStartElementAsync(Keys.Description).ConfigureAwait(false);
                    await writer.WriteStringAsync(spell.Description).ConfigureAwait(false);
                    await writer.WriteEndElementAsync().ConfigureAwait(false);
                }
                if (!string.IsNullOrWhiteSpace(spell.ShortDescription))
                {
                    await writer.WriteStartElementAsync(Keys.ShortDesc).ConfigureAwait(false);
                    await writer.WriteStringAsync(spell.ShortDescription).ConfigureAwait(false);
                    await writer.WriteEndElementAsync().ConfigureAwait(false);
                }
                foreach (string effectType in spell.EffectTypes)
                {
                    await writer.WriteStartElementAsync(Keys.EffectType).ConfigureAwait(false);
                    await WriteCachedStringAsync(writer, Keys.Id, effectType).ConfigureAwait(false);
                    await writer.WriteEndElementAsync().ConfigureAwait(false);
                }

                await writer.WriteEndElementAsync().ConfigureAwait(false);
            }
        }

        private async Task WriteActiveCombatAsync(XmlWriter writer, ActiveCombat combat)
        {
            await writer.WriteStartElementAsync(Keys.ActiveCombat).ConfigureAwait(false);

            await writer.WriteAttributeStringAsync(Keys.Name, combat.Name).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.Round, combat.Round.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);

            foreach (Combatant combatant in combat.Combatants)
            {
                await WriteCombatantAsync(writer, combatant).ConfigureAwait(false);
            }

            await writer.WriteEndElementAsync().ConfigureAwait(false);
        }

        private async Task WriteCombatantAsync(XmlWriter writer, Combatant combatant)
        {
            await writer.WriteStartElementAsync(Keys.Combatant).ConfigureAwait(false);

            await writer.WriteAttributeStringAsync(Keys.Name, combatant.Name).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.Ordinal, combatant.Ordinal.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.InitiativeOrder, combatant.InitiativeOrder.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.DisplayToPlayers, combatant.DisplayToPlayers.ToString()).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.DisplayName, combatant.DisplayName ?? string.Empty).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.HasGoneOnce, combatant.HasGoneOnce.ToString()).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.IncludeInCombat, combatant.IncludeInCombat.ToString()).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.IsPlayer, combatant.IsPlayer.ToString()).ConfigureAwait(false);

            await WriteCombatantHealthAsync(writer, combatant.Health).ConfigureAwait(false);
            await WriteCombatantPreparedInfo(writer, combatant.PreparedInfo).ConfigureAwait(false);

            await writer.WriteEndElementAsync().ConfigureAwait(false);
        }

        private async Task WriteCombatantHealthAsync(XmlWriter writer, CombatantHealth health)
        {
            await writer.WriteStartElementAsync(Keys.Health).ConfigureAwait(false);

            await writer.WriteAttributeStringAsync(Keys.MaxHealth, health.MaxHealth.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.LethalDamage, health.LethalDamage.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.NonlethalDamage, health.NonlethalDamage.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.Temporary, health.TemporaryHitPoints.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.DeadAt, health.DeadAt.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.UnconsciousAt, health.UnconsciousAt.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.FastHealing, health.FastHealing.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);

            await writer.WriteEndElementAsync().ConfigureAwait(false);
        }

        private async Task WriteCombatantPreparedInfo(XmlWriter writer, CombatantPreparer preparer)
        {
            await writer.WriteStartElementAsync(Keys.PreparedInfo).ConfigureAwait(false);

            if (preparer.Source != null)
                await writer.WriteAttributeStringAsync(Keys.Source, preparer.Source.Id.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.Name, preparer.Name).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.Ordinal, preparer.Ordinal.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.InitiativeRoll, preparer.InitiativeRoll.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.InitiativeModifier, preparer.InitiativeModifier.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.HitPoints, preparer.HitPoints.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.HitDice, preparer.HitDieString).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.HitDiceStrategy, preparer.HitDiceStrategy.ToString()).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.InitiativeOrder, preparer.InitiativeOrder.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.InitiativeGroup, preparer.InitiativeGroup.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.DisplayToPlayers, preparer.DisplayToPlayers.ToString()).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.DisplayName, preparer.DisplayName ?? string.Empty).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.FastHealing, preparer.FastHealing.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);

            await writer.WriteEndElementAsync().ConfigureAwait(false);
        }

        private async Task WritePlayers(XmlWriter writer, PlayerManager players)
        {
            foreach (PlayerCharacter character in players.PlayerCharacters)
            {
                await writer.WriteStartElementAsync(Keys.Player).ConfigureAwait(false);

                await writer.WriteOptionalAttributeStringAsync(Keys.ServerID, character.ServerID?.ToString());
                await writer.WriteAttributeStringAsync(Keys.Id, character.Id.ToString()).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.Name, character.Name ?? string.Empty).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.InitiativeModifier, character.InitiativeModifier.ToString()).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.IncludeInCombat, character.IncludeInCombat.ToString()).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.HitDice, character.HitDieString ?? string.Empty).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.HitDiceStrategy, character.HitDieRollingStrategy.ToString()).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.Player, character.Player ?? string.Empty).ConfigureAwait(false);
                await writer.WriteOptionalAttributeStringAsync(Keys.LightRadius, character.LightRadius.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.Alignment, character.Alignment.ToString()).ConfigureAwait(false);
                await writer.WriteElementStringAsync(Keys.Senses, character.Senses ?? string.Empty).ConfigureAwait(false);
                await writer.WriteElementStringAsync(Keys.Languages, character.Languages ?? string.Empty).ConfigureAwait(false);
                await writer.WriteCollectionElementAsync(Keys.Notes, character.Notes).ConfigureAwait(false);

                await writer.WriteEndElementAsync().ConfigureAwait(false);
            }
        }

        private async Task WriteMonsters(XmlWriter writer, MonsterManager monsterManager)
        {
            foreach (Monster monster in monsterManager.Monsters)
            {
                await writer.WriteStartElementAsync(Keys.Monster).ConfigureAwait(false);

                await writer.WriteOptionalAttributeStringAsync(Keys.ServerID, monster.ServerID).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.Id, monster.Id.ToString()).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.Name, monster.Name).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.InitiativeModifier, monster.InitiativeModifier.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.HitDice, monster.HitDieString ?? string.Empty).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.FastHealing, monster.FastHealing.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
                await WriteCachedStringAsync(writer, Keys.Source, monster.Source ?? string.Empty).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.DeadAt, monster.DeadAt.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
                await writer.WriteAttributeStringAsync(Keys.UnconsciousAt, monster.UnconsciousAt.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);

                foreach (MonsterStat stat in monster.Stats)
                {
                    await writer.WriteStartElementAsync(Keys.Stats).ConfigureAwait(false);
                    await writer.WriteAttributeStringAsync(Keys.Name, stat.Name).ConfigureAwait(false);

                    switch (stat.Value)
                    {
                        case string s:
                            await writer.WriteStringAsync(s).ConfigureAwait(false);
                            break;
                        case Alignment alignment:
                            await writer.WriteStringAsync(alignment.ToString()).ConfigureAwait(false);
                            break;
                        case MonsterSize size:
                            await writer.WriteStringAsync(size.ToString()).ConfigureAwait(false);
                            break;
                        case IEnumerable<string> values:
                            await writer.WriteCollectionElementAsync(Keys.Value, values).ConfigureAwait(false);
                            break;
                    }
                    await writer.WriteEndElementAsync().ConfigureAwait(false);

                }

                await writer.WriteEndElementAsync().ConfigureAwait(false);
            }
        }

        private async Task WriteCombatScenariosAsync(XmlWriter writer, IEnumerable<CombatScenario> scenarios)
        {
            foreach (CombatScenario scenario in scenarios)
                await WriteCombatScenarioAsync(writer, scenario).ConfigureAwait(false);
        }

        private async Task WriteCombatScenarioAsync(XmlWriter writer, CombatScenario scenario)
        {
            await writer.WriteStartElementAsync(Keys.Scenario).ConfigureAwait(false);

            await writer.WriteAttributeStringAsync(Keys.Id, scenario.Id.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.Name, scenario.Name).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.Group, scenario.Group ?? string.Empty).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(scenario.Details))
                await writer.WriteElementStringAsync(null, Keys.Details, null, scenario.Details).ConfigureAwait(false);

            await writer.WriteEndElementAsync().ConfigureAwait(false);

            foreach (ICombatantTemplate combatantTemplate in scenario.Combatants)
                await WriteCombatantTemplateAsync(writer, combatantTemplate, scenario.Id).ConfigureAwait(false);
        }

        private async Task WriteCombatantTemplateAsync(XmlWriter writer, ICombatantTemplate template, int scenarioId)
        {
            if (template is CombatantTemplate combatantTemplate)
                await WriteCombatantTemplateAsync(writer, combatantTemplate, scenarioId).ConfigureAwait(false);
            else
                throw new ArgumentException("Unknown combatant template type " + template.GetType().FullName + ".");
        }

        private async Task WriteCombatantTemplateAsync(XmlWriter writer, CombatantTemplate template, int scenarioId)
        {
            await writer.WriteStartElementAsync(Keys.CombatantTemplate).ConfigureAwait(false);

            await writer.WriteOptionalAttributeStringAsync(Keys.ServerID, template.ServerID);
            await writer.WriteAttributeStringAsync(Keys.Id, template.Id.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.Scenario, scenarioId.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.Kind, Keys.CombatantTemplateKind).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.Name, template.Name).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.HitDice, template.HitDieString).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.HitDiceStrategy, template.HitDieRollingStrategy.ToString()).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.InitiativeModifier, template.InitiativeModifier.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.Count, template.Count).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.DisplayToPlayers, template.DisplayToPlayers.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.DisplayName, template.DisplayName ?? string.Empty).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.FastHealing, template.FastHealing.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.DeadAt, template.DeadAt.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            await writer.WriteAttributeStringAsync(Keys.UnconsciousAt, template.UnconsciousAt.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);

            if (template.Source != null)
                await writer.WriteAttributeStringAsync(Keys.Source, template.Source.Id.ToString()).ConfigureAwait(false);

            await writer.WriteAttributeStringAsync(Keys.DamageReduction, template.DamageReduction.AsDamageReductionString()).ConfigureAwait(false);

            await writer.WriteEndElementAsync().ConfigureAwait(false);
        }

        public async Task<CampaignSettings> ReadCampaignSettings(Stream stream)
        {
            using (XmlReader reader = _generator.GetReader(stream))
                return await ReadCampaignSettings(reader).ConfigureAwait(false);
        }

        /// <summary>
        /// Reads the campaign from an XML reader
        /// </summary>
        /// <param name="reader">Reader to use</param>
        /// <returns>Campaign read in</returns>
        public async Task<CampaignSettings> ReadCampaignSettings(XmlReader reader)
        {
            _campaignItems.Clear();
            return await Task.Run(() =>
            {
                CampaignSettings campaign = new CampaignSettings();
                XElement element = XElement.Load(reader);

                ReadCachedStrings(element);

                campaign.CampaignID = element.ReadAttributeString(Keys.ServerID, string.Empty);
                campaign.Options.MonsterDeadAtOption = element.ReadAttributeEnum(Keys.DeadAtOption, MonsterDeadAtDefaultOption.NegativeConstitution);
                campaign.Options.MonsterDefaultDeadAt = element.ReadAttributeInt(Keys.DeadAt, 0);

                foreach (PlayerCharacter character in ReadPlayerCharacters(element, campaign))
                    campaign.Players.PlayerCharacters.Add(character);

                foreach (Monster monster in ReadMonsters(element, campaign))
                    campaign.MonsterManager.Monsters.Add(monster);
                campaign.MonsterManager.Reconcile();

                foreach (CombatScenario scenario in ReadCombatScenarios(element, campaign))
                    campaign.Combat.Scenarios.Add(scenario);

                foreach (Condition condition in ReadConditions(element, campaign))
                    campaign.Conditions.Conditions.Add(condition);

                XElement? activeCombatNode = element.Descendants(Keys.ActiveCombat).FirstOrDefault();
                if (activeCombatNode != null)
                    campaign.Combat.Active = ReadActiveCombat(activeCombatNode, campaign);

                foreach (Spell spell in ReadSpells(element, campaign))
                    campaign.Spells.Spells.Add(spell);
                campaign.Spells.Reconcile();

                foreach (MagicItem item in ReadMagicItems(element, campaign))
                    campaign.EquipmentManager.MagicItems.Add(item);
                campaign.EquipmentManager.Reconcile();

                if (_campaignItems.Keys.Any())
                    campaign.UpdateNextId(_campaignItems.Keys.Max() + 1);

                campaign.ReconcileSources();

                return campaign;
            });
        }

        private IEnumerable<MagicItem> ReadMagicItems(XElement element, CampaignSettings campaign)
        {
            foreach (XElement itemElement in element.Descendants(Keys.MagicItem))
            {
                int id = itemElement.ReadAttributeInt(Keys.Id);
                string name = itemElement.ReadAttributeString(Keys.Name);
                string group = ReadCachedString(itemElement, Keys.Group);

                MagicItem item = new MagicItem(campaign, name, group, id);
                item.CasterLevel = itemElement.ReadAttributeInt(Keys.CasterLevel);
                item.Slot = ReadCachedString(itemElement, Keys.Slot);
                item.PriceInCopper = itemElement.ReadAttributeInt(Keys.Price, 0);
                item.CostInCopper = itemElement.ReadAttributeInt(Keys.Cost, 0);
                item.Weight = itemElement.ReadAttributeString(Keys.Weight);
                item.Source = ReadCachedString(itemElement, Keys.Source);
                item.Alignment = itemElement.ReadAttributeEnum(Keys.Alignment, Alignment.Unknown);
                item.Intelligence = itemElement.ReadAttributeInt(Keys.Intelligence, 0);
                item.Wisdom = itemElement.ReadAttributeInt(Keys.Wisdom, 0);
                item.Charisma = itemElement.ReadAttributeInt(Keys.Charisma, 0);
                item.Ego = itemElement.ReadAttributeInt(Keys.Ego, 0);
                item.Communication = ReadCachedString(itemElement, Keys.Communication);
                item.Senses = ReadCachedString(itemElement, Keys.Senses);
                item.ArtifactLevel = itemElement.ReadAttributeEnum(Keys.ArtifactLevel, ArtifactLevel.None);
                item.AuraStrength = ReadCachedString(itemElement, Keys.ArtifactLevel);
                item.Mythic = itemElement.ReadAttributeBool(Keys.Mythic);

                item.Description = itemElement.Descendants(Keys.Description).FirstOrDefault()?.Value ?? string.Empty;
                item.Requirements = itemElement.Descendants(Keys.Requirement).FirstOrDefault()?.Value ?? string.Empty;
                item.Powers = itemElement.Descendants(Keys.Powers).FirstOrDefault()?.Value ?? string.Empty;
                item.Languages = itemElement.Descendants(Keys.Languages).FirstOrDefault()?.Value ?? string.Empty;
                item.Descruction = itemElement.Descendants(Keys.Descruction).FirstOrDefault()?.Value ?? string.Empty;

                foreach (string aura in itemElement.Descendants(Keys.Aura).Select(p => ReadCachedString(p, Keys.Aura)))
                    item.Auras.Add(aura);

                yield return item;
            }
        }

        private IEnumerable<Spell> ReadSpells(XElement element, CampaignSettings campaign)
        {
            foreach (XElement spellElement in element.Descendants(Keys.Spell))
            {
                int id = spellElement.ReadAttributeInt(Keys.Id);
                if (id != -1)
                {
                    Spell spell = new Spell(campaign, id);
                    spell.Name = spellElement.ReadAttributeString(Keys.Name);
                    spell.School = ReadCachedString(spellElement, Keys.School);
                    spell.SubSchool = ReadCachedString(spellElement, Keys.Subschool);
                    foreach (SpellLevel level in SpellLevelExtensions.GetLevelsFromInvariantSpellLevelString(ReadCachedString(spellElement, Keys.Level)))
                        spell.Levels.Add(level);
                    spell.CastingTime = ReadCachedString(spellElement, Keys.CastingTime);
                    spell.Components = ReadCachedString(spellElement, Keys.Components);
                    spell.CostlyComponents = ReadCachedString(spellElement, Keys.CostlyComponents);
                    spell.Range = ReadCachedString(spellElement, Keys.Range);
                    spell.Area = ReadCachedString(spellElement, Keys.Area);
                    spell.Effect = ReadCachedString(spellElement, Keys.Effect);
                    spell.Targets = ReadCachedString(spellElement, Keys.Targets);
                    spell.Duration = ReadCachedString(spellElement, Keys.Duration);
                    spell.Dismissible = spellElement.ReadAttributeBool(Keys.Dismissible);
                    spell.Shapeable = spellElement.ReadAttributeBool(Keys.Shapeable);
                    spell.SavingThrow = ReadCachedString(spellElement, Keys.SavingThrow);
                    spell.SpellResistance = ReadCachedString(spellElement, Keys.SpellResistance);
                    spell.Source = ReadCachedString(spellElement, Keys.Source);

                    spell.Description = spellElement.Descendants(Keys.Description).FirstOrDefault()?.Value ?? string.Empty;
                    spell.ShortDescription = spellElement.Descendants(Keys.ShortDesc).FirstOrDefault()?.Value ?? string.Empty;

                    foreach (XElement child in spellElement.Descendants(Keys.EffectType))
                        spell.EffectTypes.Add(ReadCachedString(child, Keys.Id));

                    yield return spell;
                }
            }
        }

        private ActiveCombat ReadActiveCombat(XElement element, CampaignSettings campaign)
        {
            IEnumerable<ICombatant> combatants = element.Descendants(Keys.Combatant).Select(p => ReadCombatant(p, campaign));

            return new ActiveCombat(element.ReadAttributeString(Keys.Name), new XmlActiveCombatSerializer(campaign), combatants);
        }

        private ICombatant ReadCombatant(XElement element, CampaignSettings campaign)
        {
            var preparerElement = element.Descendants(Keys.PreparedInfo).FirstOrDefault();

            if (preparerElement == null)
                throw new InvalidOperationException("Combatant preparer info missing for combatant when reading file.");

            CombatantPreparer preparer = ReadCombatantPreparer(preparerElement);
            Combatant combatant = new Combatant(campaign, preparer);

            combatant.Name = element.ReadAttributeString(Keys.Name);
            combatant.Ordinal = element.ReadAttributeInt(Keys.Ordinal);
            combatant.InitiativeOrder = element.ReadAttributeInt(Keys.InitiativeOrder);
            combatant.DisplayToPlayers = element.ReadAttributeBool(Keys.DisplayToPlayers);
            combatant.DisplayName = element.ReadAttributeString(Keys.DisplayName);
            combatant.HasGoneOnce = element.ReadAttributeBool(Keys.HasGoneOnce);
            combatant.IncludeInCombat = element.ReadAttributeBool(Keys.IncludeInCombat);

            XElement? healthElement = element.Descendants(Keys.Health).FirstOrDefault();

            if (healthElement == null)
                throw new InvalidOperationException("Combatant health info missing for combatant when reading file.");

            combatant.Health.MaxHealth = healthElement.ReadAttributeInt(Keys.MaxHealth);
            combatant.Health.LethalDamage = healthElement.ReadAttributeInt(Keys.LethalDamage);
            combatant.Health.NonlethalDamage = healthElement.ReadAttributeInt(Keys.NonlethalDamage);
            combatant.Health.TemporaryHitPoints = healthElement.ReadAttributeInt(Keys.Temporary);
            combatant.Health.DeadAt = healthElement.ReadAttributeInt(Keys.DeadAt);
            combatant.Health.UnconsciousAt = healthElement.ReadAttributeInt(Keys.UnconsciousAt);
            combatant.Health.FastHealing = healthElement.ReadAttributeInt(Keys.FastHealing);

            return combatant;
        }

        private CombatantPreparer ReadCombatantPreparer(XElement element)
        {
            ICombatantTemplate? source = null;
            int id = element.ReadAttributeInt(Keys.Source);
            if (_campaignItems.TryGetValue(id, out ICampaignObject? temp))
                source = temp as ICombatantTemplate;

            CombatantPreparer preparer = new CombatantPreparer(source);

            preparer.Name = element.ReadAttributeString(Keys.Name);
            preparer.Ordinal = element.ReadAttributeInt(Keys.Ordinal);
            preparer.InitiativeRoll = element.ReadAttributeInt(Keys.InitiativeRoll);
            preparer.InitiativeModifier = element.ReadAttributeInt(Keys.InitiativeModifier);
            preparer.HitPoints = element.ReadAttributeInt(Keys.HitPoints);
            preparer.HitDieString = element.ReadAttributeString(Keys.HitDice);
            preparer.HitDiceStrategy = element.ReadAttributeEnum<RollingStrategy>(Keys.HitDiceStrategy);
            preparer.InitiativeOrder = element.ReadAttributeInt(Keys.InitiativeOrder);
            preparer.InitiativeGroup = element.ReadAttributeInt(Keys.InitiativeGroup);
            preparer.DisplayToPlayers = element.ReadAttributeBool(Keys.DisplayToPlayers);
            preparer.DisplayName = element.ReadAttributeString(Keys.DisplayName);
            preparer.FastHealing = element.ReadAttributeInt(Keys.FastHealing);

            return preparer;
        }

        private IEnumerable<PlayerCharacter> ReadPlayerCharacters(XElement element, CampaignSettings campaign)
        {
            IEnumerable<XElement> characters = element.Descendants(Keys.Player);
            return characters
                .Select(character => ReadCharacter(character, campaign));
        }

        private PlayerCharacter ReadCharacter(XElement character, CampaignSettings campaign)
        {
            return new PlayerCharacter(campaign, character.ReadAttributeInt(Keys.Id))
            {
                ServerID = character.ReadAttributeString(Keys.ServerID),
                Name = character.ReadAttributeString(Keys.Name),
                InitiativeModifier = character.ReadAttributeInt(Keys.InitiativeModifier),
                IncludeInCombat = character.ReadAttributeBool(Keys.IncludeInCombat),
                HitDieString = character.ReadAttributeString(Keys.HitDice),
                HitDieRollingStrategy = character.ReadAttributeEnum(Keys.HitDiceStrategy, RollingStrategy.Standard),
                Player = character.ReadAttributeString(Keys.Player),
                LightRadius = character.ReadAttributeInt(Keys.LightRadius),
                Alignment = character.ReadAttributeEnum<Alignment>(Keys.Alignment),
                Senses = character.ReadElementValue(Keys.Senses),
                Languages = character.ReadElementValue(Keys.Languages),
                Notes = character.ReadCollectionElement(Keys.Notes).ToArray(),
            };
        }

        private Monster[] ReadMonsters(XElement element, CampaignSettings campaign)
        {
            return element.Descendants(Keys.Monster)
                .Select(p => ReadMonster(p, campaign))
                .ToArray();
        }

        private Monster ReadMonster(XElement element, CampaignSettings campaign)
        {
            string serverID = element.ReadAttributeString(Keys.ServerID);
            int id = element.ReadAttributeInt(Keys.Id);
            string name = element.ReadAttributeString(Keys.Name);
            string hitDice = element.ReadAttributeString(Keys.HitDice);
            int initMod = element.ReadAttributeInt(Keys.InitiativeModifier);
            int fastHealing = element.ReadAttributeInt(Keys.FastHealing);
            string source = ReadCachedString(element, Keys.Source);
            int deadAt = element.ReadAttributeInt(Keys.DeadAt, 0);
            int unconsciousAt = element.ReadAttributeInt(Keys.UnconsciousAt, 0);

            IEnumerable<MonsterStat> stats = element.Descendants(Keys.Stats)
                .Select(p => ReadStat(p))
                .ToArray();

            Monster monster = new Monster(campaign, id, name, new MonsterStats(stats))
            {
                ServerID = serverID,
                HitDieString = hitDice,
                InitiativeModifier = initMod,
                FastHealing = fastHealing,
                Source = source,
                DeadAt = deadAt,
                UnconsciousAt = unconsciousAt,
            };

            _campaignItems[monster.Id] = monster;

            return monster;
        }

        private static MonsterStat ReadStat(XElement element)
        {
            string name = element.ReadAttributeString(Keys.Name);
            string value = element.Value;

            switch (name)
            {
                case "alignment":
                    if (Enum.TryParse(value, out Alignment al))
                        return new MonsterStat(name, al);
                    break;
                case "size":
                    if (Enum.TryParse(value, out MonsterSize size))
                        return new MonsterStat(name, size);
                    break;
                case "subType":
                    return new MonsterStat(name, element.Descendants(Keys.Value).Select(p => p.Value).ToObservableCollection());
            }
            return new MonsterStat(name, value);
        }

        /// <summary>
        /// Reads all combat scenarios and combatant templates
        /// </summary>
        /// <param name="reader">XmlReader to use for reading</param>
        /// <returns>Collection of combat scenarios</returns>
        private CombatScenario[] ReadCombatScenarios(XElement element, CampaignSettings campaign)
        {
            CombatScenario[] result = element.Descendants(Keys.Scenario).Select(p => ReadCombatantScenario(p, campaign)).ToArray();

            foreach (XElement templateElement in element.Descendants(Keys.CombatantTemplate))
            {
                ReadCombatantTemplate(templateElement, campaign);
            }

            return result;
        }

        private Condition[] ReadConditions(XElement element, CampaignSettings campaign)
        {
            Condition[] result = element.Descendants(Keys.Condition).Select(p => ReadCondition(p, campaign)).ToArray();

            return result;
        }

        private Condition ReadCondition(XElement element, CampaignSettings campaign)
        {
            string name = element.ReadAttributeString(Keys.Name);
            string? description = element.Descendants(Keys.Description).FirstOrDefault()?.Value;

            return new Condition(name, description);
        }

        private ICombatantTemplate ReadCombatantTemplate(XElement templateElement, CampaignSettings campaign)
        {
            string kind = templateElement.ReadAttributeString(Keys.Kind);
            switch (kind)
            {
                case Keys.CombatantTemplateKind:
                    return ReadGenericCombatantTemplate(templateElement, campaign);
                default:
                    throw new NotImplementedException("Unknown combatant template kind:  " + kind + ".");
            }
        }

        private ICombatantTemplate ReadGenericCombatantTemplate(XElement element, CampaignSettings campaign)
        {
            CombatantTemplate template = new CombatantTemplate(campaign);

            template.ServerID = element.ReadAttributeString(Keys.ServerID);
            template.Count = element.ReadAttributeString(Keys.Count);
            template.DisplayName = element.ReadAttributeString(Keys.DisplayName);
            template.DisplayToPlayers = element.ReadAttributeBool(Keys.DisplayToPlayers);
            template.FastHealing = element.ReadAttributeInt(Keys.FastHealing);
            template.HitDieRollingStrategy = element.ReadAttributeEnum(Keys.HitDiceStrategy, RollingStrategy.Standard);
            template.HitDieString = element.ReadAttributeString(Keys.HitDice);
            template.Id = element.ReadAttributeInt(Keys.Id);
            template.InitiativeModifier = element.ReadAttributeInt(Keys.InitiativeModifier);
            template.Name = element.ReadAttributeString(Keys.Name);
            template.DeadAt = element.ReadAttributeInt(Keys.DeadAt, 0);
            template.UnconsciousAt = element.ReadAttributeInt(Keys.UnconsciousAt, 0);

            //  Attempt to read the source of this combatant in
            int source = element.ReadAttributeInt(Keys.Source, -1);
            if (source != -1 && _campaignItems.TryGetValue(source, out ICampaignObject? value) && value is ICombatantTemplateSource sourceItem)
                template.Source = sourceItem;

            string damageReductionString = element.ReadAttributeString(Keys.DamageReduction);
            IEnumerable<DamageReduction> damageReduction = DamageReduction.Parse(damageReductionString);
            template.DamageReduction.CopyFrom(damageReduction);

            int scenarioId = element.ReadAttributeInt(Keys.Scenario);
            if (_campaignItems.TryGetValue(scenarioId, out ICampaignObject? item) && item is CombatScenario scenario)
                scenario.Combatants.Add(template);

            StoreCampaignObject(template);

            return template;
        }

        private CombatScenario ReadCombatantScenario(XElement scenarioElement, CampaignSettings campaign)
        {
            CombatScenario scenario = new CombatScenario(campaign);
            scenario.Id = scenarioElement.ReadAttributeInt(Keys.Id);
            scenario.Name = scenarioElement.ReadAttributeString(Keys.Name);
            scenario.Group = scenarioElement.ReadAttributeString(Keys.Group);

            scenario.Details = scenarioElement.Descendants().FirstOrDefault(p => p.Name == Keys.Details)?.Value ?? string.Empty;

            StoreCampaignObject(scenario);

            return scenario;
        }
        #endregion
    }
}
