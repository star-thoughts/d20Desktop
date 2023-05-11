using d20Web.Models.Combat;

namespace d20Web.Storage.MongoDB.Models
{
    internal class MongoCombatPrepWithCombatants : MongoCombatPrep
    {
        public IEnumerable<MongoCombatantPrep>? Combatants { get; set; }

        public override CombatPrep ToCombatPrep()
        {
            return new CombatPrep()
            {
                ID = ID.ToString(),
                Combatants = Combatants?.Select(p => p.ToCombatantPreparer()).ToArray() ?? Enumerable.Empty<CombatantPreparer>(),
            };
        }
    }
}
