namespace Fiction.GameScreen
{
    /// <summary>
    /// Enumeration of possible default values for monster's dead at attribute when importing monsters
    /// </summary>
    public enum MonsterDeadAtDefaultOption
    {
        /// <summary>
        /// Nothing is done to the monster's "dead at" value
        /// </summary>
        None,
        /// <summary>
        /// Monster's dead at value is set to it's constitution * -1
        /// </summary>
        NegativeConstitution,
        /// <summary>
        /// Monster's dead at value is set to a specific value specified by <see cref="CampaignOptions.MonsterDefaultDeadAt"/>
        /// </summary>
        SetValue,
    }
}