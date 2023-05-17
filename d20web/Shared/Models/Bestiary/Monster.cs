using System.ComponentModel.DataAnnotations;

namespace d20Web.Models.Bestiary
{
    /// <summary>
    /// Template for storing combatant information
    /// </summary>
    public sealed class Monster
    {
        /// <summary>
        /// Gets the ID of the monster in the campaign
        /// </summary>
        public string? ID { get; set; }
        /// <summary>
        /// Gets or sets the name of this monster
        /// </summary>
        [Required]
        public string? Name { get; set; }
        /// <summary>
        /// Gets or sets the stats for the monster
        /// </summary>
        [Required]
        public MonsterStat[]? Stats { get; set; }
        /// <summary>
        /// Gets or sets the hit dice of the monster
        /// </summary>
        [Required]
        public string? HitDieString { get; set; }
        /// <summary>
        /// Gets or sets the initiative modifier for the monster
        /// </summary>
        [Required]
        public int InitiativeModifier { get; set; }
        /// <summary>
        /// Gets or sets the fast healing for the monster
        /// </summary>
        [Required]
        public int FastHealing { get; set; }
        /// <summary>
        /// Gets or sets the current HP the monster is considered dead at
        /// </summary>
        [Required]
        public int DeadAt { get; set; }
        /// <summary>
        /// Gets or sets the current HP the monster is considered unconscious at
        /// </summary>
        [Required]
        public int UnconsciousAt { get; set; }
        /// <summary>
        /// Gets or sets the source of this monster
        /// </summary>
        public string? Source { get; set; }
    }
}
