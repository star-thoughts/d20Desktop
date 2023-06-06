namespace d20Web.Models.Players
{
    /// <summary>
    /// Contains information about a player's character
    /// </summary>
    public class PlayerCharacter
    {
        #region Properties
        public string? ID { get; set; }
        public string? Name { get; set; }
        public string? Player { get; set; }
        public bool IncludeInCombat { get; set; }
        public string? HitDie { get; set; }
        public RollingStrategy RollingStrategy { get; set; }
        public int InitiativeModifier { get; set; }
        public int LightRadius { get; set; }
        public string? Senses { get; set; }
        public string? Languages { get; set; }
        public Alignment Alignment { get; set; }
        public string[]? Notes { get; set; }
        #endregion
    }
}
