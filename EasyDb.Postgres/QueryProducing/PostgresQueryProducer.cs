using EDb.Interfaces;

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
