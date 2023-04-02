namespace Fiction.GameScreen.Server
{
    /// <summary>
    /// Class to handle communications with the combat server
    /// </summary>
    public sealed class CombatManagement : ICombatManagement
    {
        /// <summary>
        /// Constructs a new <see cref="CombatManagement"/> class
        /// </summary>
        /// <param name="client">HTTP Client to use for communicating with the server</param>
        public CombatManagement(HttpClient client)
        {
            _client = client;
        }

        private HttpClient _client;

        public Task<string> CreateCombat(string name)
        {
            throw new NotImplementedException();
        }
    }
}
