namespace d20Web.Models
{
    /// <summary>
    /// Class for handling conditions applied to a combatant
    /// </summary>
    public class AppliedCondition
    {
        /// <summary>
        /// Name of the condition applied
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Description of the condition applied
        /// </summary>
        public string? Description { get; set; }
    }
}