namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Extensions for the <see cref="ActiveCombat"/> class
    /// </summary>
    public static class ActiveCombatExtensions
    {
        /// <summary>
        /// Converts an <see cref="ActiveCombat"/> into a <see cref="d20Web.Models.Combat"/> for uploading to server
        /// </summary>
        /// <param name="activeCombat">Combat to convert</param>
        /// <returns>Combat for uploading</returns>
        public static d20Web.Models.Combat ToServerCombat(this ActiveCombat activeCombat)
        {
            return new d20Web.Models.Combat
            {
                ID = activeCombat.ID,
                Name = activeCombat.Name,
                Round = activeCombat.Round,
            };
        }

        public static d20Web.Models.Combatant ToServerCombatant(this ICombatant combatant)
        {
            return new d20Web.Models.Combatant()
            {
                DamageReduction = combatant.DamageReduction.ToServerDamageReduction(),
                DisplayName = combatant.DisplayName,
                DisplayToPlayers = combatant.DisplayToPlayers,
                ID = combatant.ServerID,
                AppliedConditions = combatant.Conditions.ToServerConditions(),
                HasGoneOnce = combatant.HasGoneOnce,
                Health = combatant.Health.ToServerHealth(),
                IncludeInCombat = combatant.IncludeInCombat,
                InitiativeOrder = combatant.InitiativeOrder,
                IsCurrent = combatant.IsCurrent,
                IsPlayer = combatant.IsPlayer,
                Name = combatant.Name,
                Ordinal = combatant.Ordinal,
            };
        }

        public static IEnumerable<d20Web.Models.DamageReduction> ToServerDamageReduction(this IEnumerable<DamageReduction> damageReduction)
        {
            return damageReduction.Select(p => new d20Web.Models.DamageReduction()
            {
                Amount = p.Amount,
                RequiresAllTypes = p.RequiresAllTypes,
                Types = p.Types.ToArray(),
            }).ToArray();
        }

        public static IEnumerable<d20Web.Models.AppliedCondition> ToServerConditions(this IEnumerable<AppliedCondition> conditions)
        {
            return conditions.Select(p => new d20Web.Models.AppliedCondition()
            {
                Description = p.Condition.Description,
                Name = p.Condition.Name,
            }).ToArray();
        }

        public static d20Web.Models.CombatantHealth ToServerHealth(this CombatantHealth combatantHealth)
        {
            return new d20Web.Models.CombatantHealth()
            {
                DeadAt = combatantHealth.DeadAt,
                LethalDamage = combatantHealth.LethalDamage,
                NonLethalDamage = combatantHealth.NonlethalDamage,
                MaxHealth = combatantHealth.MaxHealth,
                TemporaryHitPoints = combatantHealth.TemporaryHitPoints,
                UnconsciousAt = combatantHealth.UnconsciousAt,
            };
        }
    }
}
