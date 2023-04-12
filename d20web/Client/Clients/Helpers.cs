using System.Text.Json;

namespace d20Web.Clients
{
    /// <summary>
    /// Helpers for connections to a server
    /// </summary>
    public static class Helpers
    {
        public static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
    }
}
