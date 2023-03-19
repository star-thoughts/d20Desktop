using System.Collections.ObjectModel;
using Fiction;

namespace System.Linq
{
    /// <summary>
    /// Extension methods for linq
    /// </summary>
    public static class LinqExtensions
    {
        public static T? AfterOrDefault<T>(this IEnumerable<T> source, T after) where T : class
        {
            Exceptions.ThrowIfArgumentNull(source, nameof(source));

            return source
                .SkipWhile(p => !ReferenceEquals(after, p))
                .Skip(1)
                .FirstOrDefault();
        }
        public static T? AfterOrFirstOrDefault<T>(this IEnumerable<T> source, T after) where T : class
        {
            Exceptions.ThrowIfArgumentNull(source, nameof(source));

            T? result = source.AfterOrDefault(after)
                ?? source.FirstOrDefault();

            if (ReferenceEquals(result, after))
                return null;
            return result;
        }
        public static T? PrevOrDefault<T>(this IEnumerable<T> source, T after) where T : class
        {
            Exceptions.ThrowIfArgumentNull(source, nameof(source));
            return source.Reverse().AfterOrDefault(after);
        }
        public static T? PrevOrLastOrDefault<T>(this IEnumerable<T> source, T after) where T : class
        {
            Exceptions.ThrowIfArgumentNull(source, nameof(source));
            T? result = source.Reverse().AfterOrFirstOrDefault(after);


            if (ReferenceEquals(result, after))
                return null;
            return result;
        }

        /// <summary>
        /// Creates an ObservableCollection from a source enumerable
        /// </summary>
        /// <typeparam name="T">Type of ObservableCollection</typeparam>
        /// <param name="source">Source enumerable</param>
        /// <returns>ObservableCollection</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            Exceptions.ThrowIfArgumentNull(source, nameof(source));
            return new ObservableCollection<T>(source);
        }

        /// <summary>
        /// Returns true if there are more than the given number of items in the collection
        /// </summary>
        /// <typeparam name="T">Type of item</typeparam>
        /// <param name="source">Source collection</param>
        /// <param name="count">Number of items that meets the condition</param>
        /// <param name="comparison">Comparison method</param>
        /// <returns>Whether or not at least count items in the source collection match using the comparison</returns>
        public static bool MoreThan<T>(this IEnumerable<T> source, int count, Func<T, bool> comparison)
        {
            Exceptions.ThrowIfArgumentNull(source, nameof(source));
            Exceptions.ThrowIfArgumentNull(comparison, nameof(comparison));

            int current = 0;
            foreach (T item in source)
            {
                if (comparison(item))
                    current++;
                if (current > count)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the index of the given item
        /// </summary>
        /// <typeparam name="T">Type of item to search for</typeparam>
        /// <param name="source">Source enumerable to search</param>
        /// <param name="item">Item to search for</param>
        /// <returns>Index of the item in the list</returns>
        public static int IndexOf<T>(this IEnumerable<T> source, T item)
        {
            Exceptions.ThrowIfArgumentNull(source, nameof(source));

            IList<T>? list = source as IList<T>;

            if (list != null)
                return list.IndexOf(item);

            int index = -1;
            foreach (T t in source)
            {
                index++;
                if (item != null && item.Equals(t))
                    return index;
            }
            return -1;
        }


        /// <summary>
        /// Gets the index of the given item
        /// </summary>
        /// <typeparam name="T">Type of item to search for</typeparam>
        /// <param name="source">Source enumerable to search</param>
        /// <param name="item">Item to search for</param>
        /// <param name="comparer">IEqualityComparer to use to do comparison</param>
        /// <returns>Index of the item in the list</returns>
        public static int IndexOf<T>(this IEnumerable<T> source, T item, IEqualityComparer<T> comparer)
        {
            Exceptions.ThrowIfArgumentNull(source, nameof(source));
            Exceptions.ThrowIfArgumentNull(comparer, nameof(comparer));

            IList<T>? list = source as IList<T>;

            if (list != null)
                return list.IndexOf(item, comparer);

            int index = -1;
            foreach (T t in source)
            {
                index++;
                if (comparer.Equals(t, item))
                    return index;
            }
            return -1;
        }

        public static T WithMax<T>(this IEnumerable<T> source, Func<T, int> value)
        {
            T max = source.First();
            int maxValue = value(max);

            foreach (T item in source)
            {
                if (value(item) > maxValue)
                    max = item;
            }

            return max;
        }

        /// <summary>
        /// Determines whether or not a specified number of items meet a specified criteria
        /// </summary>
        /// <typeparam name="T">Type of collection to test against</typeparam>
        /// <param name="source">Sourc collection to test against</param>
        /// <param name="count">Exact number of items expected</param>
        /// <param name="evaluator">Evaluator to test each item</param>
        /// <returns>Whether or not the exact given number of items was found</returns>
        public static bool HasExactly<T>(this IEnumerable<T> source, int count, Func<T, bool> evaluator)
        {
            return source.Where(evaluator).Take(count + 1).Count() == count;
        }
        /// <summary>
        /// Determines whether or not a maximum specified number of items meet a specified criteria
        /// </summary>
        /// <typeparam name="T">Type of collection to test against</typeparam>
        /// <param name="source">Sourc collection to test against</param>
        /// <param name="count">Maximum number of items expected</param>
        /// <param name="evaluator">Evaluator to test each item</param>
        /// <returns>Whether or not at least given number of items was found</returns>
        public static bool HasAtMost<T>(this IEnumerable<T> source, int count, Func<T, bool> evaluator)
        {
            return source.Where(evaluator).Take(count + 1).Count() <= count;
        }

        /// <summary>
        /// Gets the index of the first item that matches the evaluator
        /// </summary>
        /// <typeparam name="T">Type of items to evaluate</typeparam>
        /// <param name="source">Source collection to evaluate</param>
        /// <param name="evaluator">Function to use to evaluate the collection</param>
        /// <param name="defaultIndex">The default value to return if no items match the evaluator</param>
        /// <returns>Index of the first item that matches, or <paramref name="defaultIndex"/> if no item matches</returns>
        public static int IndexOfFirst<T>(this IEnumerable<T> source, Func<T, bool> evaluator, int defaultIndex)
        {
            int i = 0;
            foreach (T item in source)
            {
                if (evaluator(item))
                    return i;
                i++;
            }
            return defaultIndex;
        }
        /// <summary>
        /// Gets the index of the first item that matches the evaluator
        /// </summary>
        /// <typeparam name="T">Type of items to evaluate</typeparam>
        /// <param name="source">Source collection to evaluate</param>
        /// <param name="evaluator">Function to use to evaluate the collection</param>
        /// <returns>Index of the first item that matches, or -1 if no item matches</returns>
        public static int IndexOfFirst<T>(this IEnumerable<T> source, Func<T, bool> evaluator)
        {
            return source.IndexOfFirst(evaluator, -1);
        }
    }
}
