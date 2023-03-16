namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Contains information about a condition that was applied to a combatant
    /// </summary>
    public class AppliedCondition
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="AppliedCondition"/>
        /// </summary>
        /// <param name="condition">Condition that is applied</param>
        public AppliedCondition(Condition condition)
        {
            Condition = condition;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the condition that was applied
        /// </summary>
        public Condition Condition { get; set; }
        #endregion
    }
}