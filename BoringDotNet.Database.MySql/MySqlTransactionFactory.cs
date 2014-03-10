using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace BoringDotNet.Database.MySql
{
    public class MySqlTransactionFactory : IDbTransactionFactory
    {
        private readonly string _connectionString;

        public MySqlTransactionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ExecuteInTransaction(Action<IDbTransaction> action)
        {
            using (var conn = new MySqlConnection(_connectionString))
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