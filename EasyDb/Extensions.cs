namespace EasyDb
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Extentions for objects
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// The AddRange
        /// </summary>
        /// <typeparam name="T">Range value type</typeparam>
        /// <param name="collection">The collection<see cref="ObservableCollection{T}"/></param>
        /// <param name="enumaration">The enumaration<see cref="IEnumerable{T}"/></param>
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> enumaration)
        {
            foreach (var item in enumaration)
            {
                collection.Add(item);
            }
        }
    }
}