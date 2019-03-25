namespace Edb.Environment.Model
{
    using System;

    /// <summary>
    /// Defines the <see cref="PackageOperationResult" />
    /// </summary>
    public class PackageOperationResult
    {
        /// <summary>
        /// Defines the SuccessfulCached
        /// </summary>
        public static readonly PackageOperationResult SuccessfulCached = new PackageOperationResult
        {
            Successful = true
        };

        /// <summary>
        /// Gets or sets a value indicating whether Successful
        /// </summary>
        public bool Successful { get; set; }

        /// <summary>
        /// Gets or sets the Messages
        /// </summary>
        public string[] Messages { get; set; } = new string[0];

        /// <summary>
        /// Gets or sets the Exception
        /// </summary>
        public Exception Exception { get; set; }
    }
}
