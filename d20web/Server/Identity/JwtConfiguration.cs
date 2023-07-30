namespace d20Web.Identity
{
    /// <summary>
    /// Configuration information for JWT
    /// </summary>
    public class JwtConfiguration
    {
        public string? Key { get; set; }
        public string? Issuer { get; set; }
        public int ExpireDays { get; set; }
    }
}
