namespace System.ComponentModel
{
    /// <summary>
    /// Exception thrown when a property is not found
    /// </summary>
    public sealed class PropertyNotFoundException : Exception
    {
        #region Constructors
        public PropertyNotFoundException() : base("Could not find property.") { }
        public PropertyNotFoundException(string message, Exception innerException) : base(message, innerException) { }
        public PropertyNotFoundException(string property) : base(property)
        {
        }
        #endregion
    }
}
