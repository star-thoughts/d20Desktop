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
    }
}
