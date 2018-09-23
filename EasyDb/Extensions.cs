using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EasyDb
{
    /// <summary>
    /// Extentions for objects
    /// </summary>
    public static class Extensions
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> enumaration)
        {
            foreach (var item in enumaration)
            {
                collection.Add(item);
            }
        }
    }
}
