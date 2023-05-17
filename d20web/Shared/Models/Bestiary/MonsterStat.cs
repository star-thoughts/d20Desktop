using System.ComponentModel.DataAnnotations;

namespace d20Web.Models.Bestiary
{
    /// <summary>
    /// Contains a stat for a monster
    /// </summary>
    public sealed class MonsterStat
    {
        /// <summary>
        /// Gets or sets the name of the stat
        /// </summary>
        [Required]
        public string? Name { get; set; }
        /// <summary>
        /// Gets or sets the value of the stat
        /// </summary>
        public string? Value { get; set; }
        /// <summary>
        /// Gets or sets an array of values for this
        /// </summary>
        public IEnumerable<string>? Values { get; set; }
    }
}