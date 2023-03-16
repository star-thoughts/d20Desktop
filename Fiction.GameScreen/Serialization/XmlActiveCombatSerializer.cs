using Fiction.GameScreen.Combat;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Fiction.GameScreen.Serialization
{
    /// <summary>
    /// Serializer for storing active combat information in XML format for undo/redo purposes
    /// </summary>
    public sealed class XmlActiveCombatSerializer : IXmlActiveCombatSerializer
    {
        #region Constructors
        public XmlActiveCombatSerializer(CampaignSettings campaign)
        {
            _campaign = campaign;
        }
        #endregion
        #region Member Variables
        private CampaignSettings _campaign;

        class Keys
        {
            public const string Combat = "combat";
            public const string Name = "name";
            public const string Current = "current";
            public const string Round = "round";
            public const string Combatant = "combatant";
            public const string Ordinal = "ordinal";
            public const string Prepared = "prepared";
            public const string InitiativeOrder = "order";
            public const string DisplayToPlayers = "display";
            public const string DisplayName = "displayName";
            public const string HasGoneOnce = "gone";
            public const string IncludeInCombat = "include";
            public const string MaxHealth = "max";
            public const string LethalDamage = "lethal";
            public const string NonlethalDamage = "nonlethal";
            public const string Temporary = "temp";
            public const string DeadAt = "deadAt";
            public const string UnconsciousAt = "unconAt";
            public const string FastHealing = "fastHealing";
            public const string Id = "id";
            public const string Effect = "effect";
            public const string Duration = "duration";
            public const string Remaining = "remaining";
            public const string Source = "source";
            public const string InitiativeSource = "init";
            public const string Related = "related";
            public const string Target = "target";
            public const string Condition = "condition";
            public const string DamageReduction = "dr";
        }
        #endregion
        #region Properties
        #endregion
        #region Methods
        public async Task<Stream> Backup(ActiveCombat combat)
        {
            Stream result = new MemoryStream();
            await Serialize(result, combat);
            return result;
        }
        private async Task Serialize(Stream stream, ActiveCombat combat)
        {
            using (XmlWriter writer = ReaderWriterGenerator.DefaultAsync.GetWriter(stream))
            {
                await writer.WriteStartElementAsync(Keys.Combat);

                await writer.WriteAttributeStringAsync(Keys.Name, combat.Name);
                await writer.WriteAttributeStringAsync(Keys.Current, combat.Current.Id.ToString(CultureInfo.InvariantCulture));
                await writer.WriteAttributeStringAsync(Keys.Round, combat.Round.ToString(CultureInfo.InvariantCulture));

                foreach (ICombatant combatant in combat.Combatants)
                {
                    await writer.WriteStartElementAsync(Keys.Combatant);

                    await writer.WriteAttributeStringAsync(Keys.Name, combatant.Name);
                    await writer.WriteAttributeStringAsync(Keys.Id, combatant.Id.ToString(CultureInfo.InvariantCulture));
                    await writer.WriteAttributeStringAsync(Keys.InitiativeOrder, combatant.InitiativeOrder.ToString(CultureInfo.InvariantCulture));
                    await writer.WriteAttributeStringAsync(Keys.DisplayToPlayers, combatant.DisplayToPlayers.ToString());
                    await writer.WriteAttributeStringAsync(Keys.DisplayName, combatant.DisplayName);
                    await writer.WriteAttributeStringAsync(Keys.HasGoneOnce, combatant.HasGoneOnce.ToString());
                    await writer.WriteAttributeStringAsync(Keys.IncludeInCombat, combatant.IncludeInCombat.ToString());
                    await writer.WriteAttributeStringAsync(Keys.MaxHealth, combatant.Health.MaxHealth.ToString(CultureInfo.InvariantCulture));
                    await writer.WriteAttributeStringAsync(Keys.LethalDamage, combatant.Health.LethalDamage.ToString(CultureInfo.InvariantCulture));
                    await writer.WriteAttributeStringAsync(Keys.NonlethalDamage, combatant.Health.NonlethalDamage.ToString(CultureInfo.InvariantCulture));
                    await writer.WriteAttributeStringAsync(Keys.Temporary, combatant.Health.TemporaryHitPoints.ToString(CultureInfo.InvariantCulture));
                    await writer.WriteAttributeStringAsync(Keys.UnconsciousAt, combatant.Health.UnconsciousAt.ToString(CultureInfo.InvariantCulture));
                    await writer.WriteAttributeStringAsync(Keys.DeadAt, combatant.Health.DeadAt.ToString(CultureInfo.InvariantCulture));
                    await writer.WriteAttributeStringAsync(Keys.FastHealing, combatant.Health.FastHealing.ToString(CultureInfo.InvariantCulture));

                    if (combatant.DamageReduction.Any())
                    {
                        await writer.WriteStartElementAsync(Keys.DamageReduction);
                        await writer.WriteStringAsync(combatant.DamageReduction.AsDamageReductionString());
                        await writer.WriteEndElementAsync();
                    }

                    await writer.WriteCollectionElementAsync(Keys.Condition, combatant.Conditions.Select(p => p.Condition.Name));

                    await writer.WriteEndElementAsync();
                }

                foreach(Effect effect in combat.Effects)
                {
                    await writer.WriteStartElementAsync(Keys.Effect);

                    await writer.WriteAttributeStringAsync(Keys.Name, effect.Name);
                    await writer.WriteAttributeStringAsync(Keys.Duration, effect.DurationRounds.ToString(CultureInfo.InvariantCulture));
                    await writer.WriteAttributeStringAsync(Keys.Remaining, effect.RemainingRounds.ToString(CultureInfo.InvariantCulture));
                    await writer.WriteAttributeStringAsync(Keys.Source, effect.Source.Id.ToString(CultureInfo.InvariantCulture));
                    await writer.WriteAttributeStringAsync(Keys.InitiativeSource, effect.InitiativeSource.Id.ToString(CultureInfo.InvariantCulture));
                    await writer.WriteAttributeStringAsync(Keys.Related, effect.RelatedItem?.Id.ToString(CultureInfo.InvariantCulture) ?? string.Empty);
                    
                    foreach(ICombatant combatant in effect.Targets)
                    {
                        await writer.WriteStartElementAsync(Keys.Target);
                        await writer.WriteAttributeStringAsync(Keys.Id, combatant.Id.ToString(CultureInfo.InvariantCulture));
                        await writer.WriteEndElementAsync();
                    }

                    await writer.WriteEndElementAsync();
                }

                await writer.WriteEndElementAsync();
            }
        }

        public void Restore(Stream stream, ActiveCombat combat)
        {
            stream.Position = 0L;
            Deserialize(stream, combat);
        }
        private void Deserialize(Stream stream, ActiveCombat combat)
        {
            using (XmlReader reader = ReaderWriterGenerator.Default.GetReader(stream))
            {
                XElement element = XElement.Load(reader);
                IEnumerable<XElement> combatants = element.Descendants(Keys.Combatant);
                List<ICombatant> updated = new List<ICombatant>(combat.Combatants.Count);

                ICombatant current = combat.Combatants.FirstOrDefault(p => p.Id.Equals(element.ReadAttributeInt(Keys.Current)));
                if (current != null)
                    combat.Current = current;

                combat.Round = element.ReadAttributeInt(Keys.Round);

                foreach (XElement combatantElement in combatants)
                {
                    ICombatant combatant = combat.Combatants.FirstOrDefault(p => p.Id.Equals(combatantElement.ReadAttributeInt(Keys.Id)));
                    if (combatant != null)
                    {
                        updated.Add(combatant);

                        combatant.Name = combatantElement.ReadAttributeString(Keys.Name);
                        combatant.InitiativeOrder = combatantElement.ReadAttributeInt(Keys.InitiativeOrder);
                        combatant.DisplayToPlayers = combatantElement.ReadAttributeBool(Keys.DisplayToPlayers);
                        combatant.DisplayName = combatantElement.ReadAttributeString(Keys.DisplayName);
                        combatant.HasGoneOnce = combatantElement.ReadAttributeBool(Keys.HasGoneOnce);
                        combatant.IncludeInCombat = combatantElement.ReadAttributeBool(Keys.IncludeInCombat);
                        combatant.Health.MaxHealth = combatantElement.ReadAttributeInt(Keys.MaxHealth);
                        combatant.Health.LethalDamage = combatantElement.ReadAttributeInt(Keys.LethalDamage);
                        combatant.Health.NonlethalDamage = combatantElement.ReadAttributeInt(Keys.NonlethalDamage);
                        combatant.Health.TemporaryHitPoints = combatantElement.ReadAttributeInt(Keys.Temporary);
                        combatant.Health.UnconsciousAt = combatantElement.ReadAttributeInt(Keys.UnconsciousAt);
                        combatant.Health.DeadAt = combatantElement.ReadAttributeInt(Keys.DeadAt);
                        combatant.Health.FastHealing = combatantElement.ReadAttributeInt(Keys.FastHealing);

                        XElement damageReduction = combatantElement.Descendants(Keys.DamageReduction)
                            .FirstOrDefault();

                        if (damageReduction != null)
                            combatant.DamageReduction.CopyFrom(DamageReduction.Parse(damageReduction.Value));

                        string[] conditionNames = combatantElement.ReadCollectionElement(Keys.Condition).ToArray();
                        foreach(string conditionName in conditionNames)
                        {
                            Condition condition = _campaign.Conditions.Conditions.FirstOrDefault(p => string.Equals(p.Name, conditionName));
                            if (condition != null)
                                combatant.Conditions.Add(new AppliedCondition(condition));
                        }
                    }
                }

                IEnumerable<XElement> effects = element.Descendants(Keys.Effect);
                combat.Effects.Clear();
                foreach (XElement effectElement in effects)
                {
                    ICombatant source = combat.Combatants.FirstOrDefault(p => p.Id == effectElement.ReadAttributeInt(Keys.Source));
                    int[] targetIds = effectElement.Descendants(Keys.Target).Select(p => p.ReadAttributeInt(Keys.Id)).ToArray();
                    ICombatant[] targets = combat.Combatants.Where(p => targetIds.Contains(p.Id)).ToArray();
                    int relatedId = effectElement.ReadAttributeInt(Keys.Related);

                    if (source != null)
                    {
                        Effect effect = new Effect(source, effectElement.ReadAttributeInt(Keys.Duration), targets);
                        effect.Name = effectElement.ReadAttributeString(Keys.Name);
                        effect.DurationRounds = effectElement.ReadAttributeInt(Keys.Duration);
                        effect.RemainingRounds = effectElement.ReadAttributeInt(Keys.Remaining);
                        effect.RelatedItem = _campaign.Spells.Spells.FirstOrDefault(p => p.Id == relatedId);

                        combat.Effects.Add(effect);
                    }
                }

                ICombatant[] unused = combat.Combatants.Except(updated).ToArray();
                combat.RemoveCombatants(unused);
            }
        }
        #endregion
    }
}
