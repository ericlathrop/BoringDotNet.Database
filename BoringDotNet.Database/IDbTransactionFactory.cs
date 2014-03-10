using System;
using System.Data;

namespace BoringDotNet.Database
{
    public interface IDbTransactionFactory
    {
        void ExecuteInTransaction(Action<IDbTransaction> action);
    }
}