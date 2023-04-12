namespace d20Web.Storage
{
    /// <summary>
    /// Exception is thrown when an item cannot be found
    /// </summary>
    public sealed class ItemNotFoundException : Exception
    {
        /// <summary>
        /// Constructs a new <see cref="ItemNotFoundException"/>
        /// </summary>
        /// <param name="itemType">Type of item not found</param>
        /// <param name="itemID">ID of item not found</param>
        public ItemNotFoundException(ItemType itemType, string itemID)
        {
            ItemType = itemType;
            ItemID = itemID;
        }
        /// <summary>
        /// Gets the type of item that was not found
        /// </summary>
        public ItemType ItemType { get; private set; }
        /// <summary>
        /// Gets the ID of the item that was not found
        /// </summary>
        public string ItemID { get; private set; }
    }
}
