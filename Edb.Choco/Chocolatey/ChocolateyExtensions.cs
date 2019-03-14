namespace Edb.Environment.Chocolatey
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using chocolatey;
    using chocolatey.infrastructure.results;

    /// <summary>
    /// Defines the <see cref="ChocolateyExtensions" />
    /// </summary>
    public static class ChocolateyExtensions
    {
        /// <summary>
        /// The RunAsync
        /// </summary>
        /// <param name="chocolatey">The chocolatey<see cref="GetChocolatey"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public static Task RunAsync(this GetChocolatey chocolatey)
        {
            return Task.Run(() => chocolatey.Run());
        }

        /// <summary>
        /// The ListAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="chocolatey">The chocolatey<see cref="GetChocolatey"/></param>
        /// <returns>The <see cref="Task{ICollection{T}}"/></returns>
        public static Task<ICollection<T>> ListAsync<T>(this GetChocolatey chocolatey)
        {
            return Task.Run(() => (ICollection<T>)chocolatey.List<T>().ToList());
        }

        /// <summary>
        /// The ListPackagesAsync
        /// </summary>
        /// <param name="chocolatey">The chocolatey<see cref="GetChocolatey"/></param>
        /// <returns>The <see cref="Task{ICollection{PackageResult}}"/></returns>
        public static Task<ICollection<PackageResult>> ListPackagesAsync(this GetChocolatey chocolatey)
        {
            return Task.Run(() => (ICollection<PackageResult>)chocolatey.List<PackageResult>().ToList());
        }
    }
}
