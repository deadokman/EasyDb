namespace EDb.Interfaces.iface
{
    /// <summary>
    /// Produces querys for datasource module
    /// </summary>
    public interface IEdbDataSourceQueryProducer
    {
        /// <summary>
        /// Returns connection test query for datasource module
        /// </summary>
        /// <returns>Returns connection test query for datasource module</returns>
        string GetTestConnectionQuery();
    }
}
