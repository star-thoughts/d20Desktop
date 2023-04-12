namespace d20Web.Storage
{
    /// <summary>
    /// Gets or sets settings used for storage
    /// </summary>
    public sealed class StorageSettings
    {
        /// <summary>
        /// Gets or sets the type of storage
        /// </summary>
        public StorageType Type { get; set; }
        /// <summary>
        /// Gets or sets the connection string or URL
        /// </summary>
        public string? ConnectionString { get; set; }
        /// <summary>
        /// Gets or sets the default database name to use
        /// </summary>
        public string? DatabaseName { get; set; }
    }
}
