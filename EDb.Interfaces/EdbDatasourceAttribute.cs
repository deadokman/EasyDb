namespace EDb.Interfaces
{
    using System;

    /// <summary>
    /// Defines the <see cref="EdbDatasourceAttribute" />
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EdbDatasourceAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EdbDatasourceAttribute"/> class.
        /// </summary>
        /// <param name="guid">The guid<see cref="string"/></param>
        /// <param name="version">The version<see cref="string"/></param>
        public EdbDatasourceAttribute(string guid, string version)
        {
            SourceGuid = new Guid(guid);
            Version = new Version(version);
        }

        /// <summary>
        /// Gets the SourceGuid
        /// </summary>
        public Guid SourceGuid { get; private set; }

        /// <summary>
        /// Gets or sets the Version
        /// </summary>
        public Version Version { get; set; }
    }
}
