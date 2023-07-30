namespace d20Web.Identity
{
    public class LoginResponse
    {
        public string? Token { get; set; }
        public string? UserName { get; set; }
        public Dictionary<string, string>? RoleMap { get; set; }
    }
}
