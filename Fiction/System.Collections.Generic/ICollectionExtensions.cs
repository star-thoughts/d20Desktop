using Fiction;
using Fiction.Resources;

namespace System.Collections.Generic
{
    /// <summary>
    /// Extension methods for collections, lists, etc
    /// </summary>
    public static class ICollectionExtensions
    {
        /// <summary>
        /// Appends a collection of items to the collection
        /// </summary>
        /// <typeparam name="T">Type of items</typeparam>
        /// <param name="collection">Collection to append to</param>
        /// <param name="appending">Items to append</param>
        public static void Append<T>(this ICollection<T> collection, IEnumerable<T> appending)
        {
            Exceptions.ThrowIfArgumentNull(appending, nameof(appending));
            Exceptions.ThrowIfArgumentNull(collection, nameof(collection));

            foreach (T item in appending)
                collection.Add(item);
        }
        /// <summary>
        /// Inserts an item after the given item
        /// </summary>
        /// <typeparam name="T">Type of items in the list</typeparam>
        /// <param name="collection">List to add the item to</param>
        /// <param name="itemToInsert">Item to be inserted into the list</param>
        /// <param name="after">Item to insert after</param>
        public static void InsertAfter<T>(this IList<T> collection, T itemToInsert, T after)
        {
            Exceptions.ThrowIfArgumentNull(collection, nameof(collection));

            int index = collection.IndexOf(after);
            if (index == -1)
                Exceptions.ThrowArgumentException(CommonResources.ItemNotInCollectionException, nameof(after));

            collection.Insert(index + 1, itemToInsert);
        }
        /// <summary>
        /// Inserts an item before the given item
        /// </summary>
        /// <typeparam name="T">Type of items in the list</typeparam>
        /// <param name="collection">List to add the item to</param>
        /// <param name="itemToInsert">Item to be inserted into the list</param>
        /// <param name="after">Item to insert before</param>
        public static void InsertBefore<T>(this IList<T> collection, T itemToInsert, T before)
        {
            Exceptions.ThrowIfArgumentNull(collection, nameof(collection));

            int index = collection.IndexOf(before);
            if (index == -1)
                Exceptions.ThrowArgumentException(CommonResources.ItemNotInCollectionException, nameof(before));

            collection.Insert(index, itemToInsert);
        }
        /// <summary>
        /// Removes all instances of the given value from the dictionary
        /// </summary>
        /// <typeparam name="T1">First type in the dictionary</typeparam>
        /// <typeparam name="T2">Second type in the dictionary</typeparam>
        /// <param name="dictionary">Dictionary to remove the value from</param>
        /// <param name="value">Value to remove from teh library</param>
        public static void RemoveValue<T1, T2>(this Dictionary<T1, T2> dictionary, T2 value) where T1 : notnull
        {
            Exceptions.ThrowIfArgumentNull(dictionary, nameof(dictionary));

            IEnumerable<KeyValuePair<T1, T2>>? where = null;

            if (value != null)
                where = dictionary
                    .Where(p => value.Equals(p.Value));
            else
                where = dictionary
                    .Where(p => p.Value == null);

            List<T1> toRemove = where.Select(p => p.Key).ToList();

            foreach (T1 item in toRemove)
                dictionary.Remove(item);
        }
    }
}
