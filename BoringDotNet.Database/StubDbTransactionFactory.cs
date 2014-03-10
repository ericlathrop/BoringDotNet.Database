using System;
using System.Data;

namespace BoringDotNet.Database
{
    public class StubDbTransactionFactory : IDbTransactionFactory
    {
        private readonly IDbTransaction _txn;

        public StubDbTransactionFactory(IDbTransaction txn)
        {
            _txn = txn;
        }

        public void ExecuteInTransaction(Action<IDbTransaction> action)
        {
            action(_txn);
        }
    }
}
