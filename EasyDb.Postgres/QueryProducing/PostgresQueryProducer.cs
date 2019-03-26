using EDb.Interfaces;
using EDb.Interfaces.iface;

namespace EasyDb.Postgres.QueryProducing
{
    public class PostgresQueryProducer : IEdbDataSourceQueryProducer
    {
        public string GetTestConnectionQuery()
        {
            return "select 1";
        }
    }
}
