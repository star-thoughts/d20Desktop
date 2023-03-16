namespace Fiction.GameScreen.Combat
{
    public interface ICombatantSource
    {
        /// <summary>
        /// Creates a preparer from this template
        /// </summary>
        /// <returns>Combatant created</returns>
        CombatantPreparer[] Prepare(CombatPreparer preparer);
    }
}