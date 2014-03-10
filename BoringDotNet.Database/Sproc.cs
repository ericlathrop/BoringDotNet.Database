using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BoringDotNet.Database
{
    public class Sproc
    {
        public static void NonQuery(IDbTransaction txn, string sproc, IList<Tuple<string, ParameterDirection, object>> parameters)
        {
            using (var cmd = BuildCommand(txn, sproc, parameters))
            {
                cmd.ExecuteNonQuery();
                CopyOutputValuesToParameters(parameters, cmd);
            }
        }

        private static void CopyOutputValuesToParameters(IEnumerable<Tuple<string, ParameterDirection, object>> parameters, IDbCommand cmd)
        {
            if (parameters == null)
                return;
            foreach (var parameter in parameters)
            {
                if (parameter.Item2 != ParameterDirection.Input)
                    parameter.Item3 = ((IDataParameter)cmd.Parameters[parameter.Item1]).Value;
            }
        }

        public static IList<T> Query<T>(IDbTransaction txn, string sproc, Func<IDataReader, T> deserializer, IList<Tuple<string, ParameterDirection, object>> parameters = null)
        {
            if (txn == null)
                throw new ArgumentNullException("txn");

            using (var cmd = BuildCommand(txn, sproc, parameters))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    var rows = new List<T>();
                    while (reader.Read())
                    {
                        rows.Add(deserializer(reader));
                    }
                    CopyOutputValuesToParameters(parameters, cmd);
                    return rows;
                }
            }
        }

        private static IDbCommand BuildCommand(IDbTransaction txn, string sproc, IEnumerable<Tuple<string, ParameterDirection, object>> parameters)
        {
            var cmd = txn.Connection.CreateCommand();
            cmd.CommandText = sproc;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = txn;

            if (parameters != null)
            {
                foreach (var dbParam in MakeParameters(parameters, cmd))
                {
                    cmd.Parameters.Add(dbParam);
                }
            }
            return cmd;
        }

        private static IEnumerable<IDbDataParameter> MakeParameters(IEnumerable<Tuple<string, ParameterDirection, object>> parameters, IDbCommand cmd)
        {
            return parameters.Select(tuple =>
                                     {
                                         var param1 = cmd.CreateParameter();
                                         param1.ParameterName = tuple.Item1;
                                         param1.Direction = tuple.Item2;
                                         param1.Value = tuple.Item3;
                                         return param1;
                                     });
        }
    }
}