using System;
using System.Data;
using System.Data.SqlClient;

namespace BoringDotNet.Database.SqlServer
{
    public class SqlServerTransactionFactory : IDbTransactionFactory
    {
        private readonly string _connectionString;

        public SqlServerTransactionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ExecuteInTransaction(Action<IDbTransaction> action)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var txn = conn.BeginTransaction())
                {
                    action(txn);
                }
            }
        }
    }
}
