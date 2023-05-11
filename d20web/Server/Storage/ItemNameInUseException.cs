namespace d20Web.Storage
{
    /// <summary>
    /// Exception is thrown when an item cannot be added because it's name is already in use
    /// </summary>
    public sealed class ItemNameInUseException : Exception
    {
        /// <summary>
        /// Constructs a new <see cref="ItemNameInUseException"/>
        /// </summary>
        /// <param name="itemType">Type of item not found</param>
        /// <param name="name">ID of item not found</param>
        public ItemNameInUseException(ItemType itemType, string name)
        {
            ItemType = itemType;
            Name = name;
        }
        /// <summary>
        /// Gets the type of item that was a duplicate
        /// </summary>
        public ItemType ItemType { get; private set; }
        /// <summary>
        /// Gets the name of the item that was a duplicate
        /// </summary>
        public string Name { get; private set; }
    }
}
