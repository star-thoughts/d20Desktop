namespace d20Web.Models
{
    /// <summary>
    /// Contains information describing damage reduction for a combatant
    /// </summary>
    public sealed class DamageReduction
    {
        /// <summary>
        /// Gets or sets the amount of reduction
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// Gets or sets the types that can bypass this reduction
        /// </summary>
        public IEnumerable<string>? Types { get; set; }
        /// <summary>
        /// Gets or sets whether or not all types must be present to bypass damage reduction
        /// </summary>
        public bool RequiresAllTypes { get; set; }
    }
}