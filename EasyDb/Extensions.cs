namespace EasyDb
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using EDb.Interfaces;

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

        /// <summary>
        /// Returns correct ODBC driver name from EDB module for 32 and 63 system architecture
        /// </summary>
        /// <param name="module">Edb module</param>
        /// <returns>Driver name</returns>
        public static string GetCorrectDriverName(this EdbDatasourceModule module)
        {
            return IntPtr.Size == 8 ? module?.OdbcSystemDriverName : module?.OdbcSystem32DriverName;
        }
    }
}