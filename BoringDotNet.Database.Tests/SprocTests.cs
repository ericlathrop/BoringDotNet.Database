using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BoringDotNet.Database
{
    [TestFixture]
    public class SprocTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Query_WithNullTransaction_ThrowsException()
        {
            Sproc.Query(null, "test", IntDeserializer);
        }

        private int IntDeserializer(IDataReader reader)
        {
            return reader.GetInt32(0);
        }

        public class StubDataParameterCollection : List<IDataParameter>, IDataParameterCollection
        {
            public bool Contains(string parameterName)
            {
                return this.Any(p => p.ParameterName == parameterName);
            }

            public int IndexOf(string parameterName)
            {
                return FindIndex(p => p.ParameterName == parameterName);
            }

            public void RemoveAt(string parameterName)
            {
                RemoveAt(IndexOf(parameterName));
            }

            public object this[string parameterName]
            {
                get { return this[IndexOf(parameterName)]; }
                set { this[IndexOf(parameterName)] = (IDataParameter) value; }
            }
        }

        [Test]
        public void NonQuery_WithOutParameters_SetsParameterValues()
        {
            const int expected = 42;
            var txn = BuildOverwriterTransaction(expected);
            var parameters = BuildSampleOutParameters();

            Sproc.NonQuery(txn, "sproc", parameters);

            AssertOutParametersAreExpected(expected, parameters);
        }

        [Test]
        public void NonQuery_WithNullParameters_SetsParameterValues()
        {
            const int expected = 42;
            var txn = BuildOverwriterTransaction(expected);
            Sproc.NonQuery(txn, "sproc", null);
        }

        private static IDbTransaction BuildOverwriterTransaction(int expected)
        {
            var cmd = Substitute.For<IDbCommand>();
            cmd.CreateParameter().Returns(Substitute.For<IDbDataParameter>(), Substitute.For<IDbDataParameter>(), Substitute.For<IDbDataParameter>());
            cmd.ExecuteNonQuery().Returns(x =>
            {
                OverwriteParameterValues(expected, cmd);
                return 1;
            });
            cmd.ExecuteReader().Returns(x =>
            {
                OverwriteParameterValues(expected, cmd);
                return Substitute.For<IDataReader>();
            });

            var dataParameters = new StubDataParameterCollection();
            cmd.Parameters.Returns(dataParameters);

            var conn = Substitute.For<IDbConnection>();
            conn.CreateCommand().Returns(cmd);

            var txn = Substitute.For<IDbTransaction>();
            txn.Connection.Returns(conn);
            return txn;
        }

        private static void OverwriteParameterValues(int expected, IDbCommand cmd)
        {
            foreach (IDbDataParameter param in cmd.Parameters)
            {
                if (param.Direction != ParameterDirection.Input)
                    param.Value = expected;
            }
        }

        private static List<Tuple<string, ParameterDirection, object>> BuildSampleOutParameters()
        {
            var parameters = new List<Tuple<string, ParameterDirection, object>>
            {
                new Tuple<string, ParameterDirection, object>("param1", ParameterDirection.Output, 0),
                new Tuple<string, ParameterDirection, object>("param2", ParameterDirection.InputOutput, 0),
                new Tuple<string, ParameterDirection, object>("param3", ParameterDirection.ReturnValue, 0)
            };
            return parameters;
        }

        private static void AssertOutParametersAreExpected(int expected, List<Tuple<string, ParameterDirection, object>> parameters)
        {
            Assert.AreEqual(expected, parameters[0].Item3);
            Assert.AreEqual(expected, parameters[1].Item3);
            Assert.AreEqual(expected, parameters[2].Item3);
        }

        [Test]
        public void Query_WithOutParameters_SetsParameterValues()
        {
            const int expected = 42;
            var txn = BuildOverwriterTransaction(expected);
            var parameters = BuildSampleOutParameters();

            Sproc.Query(txn, "sproc", IntDeserializer, parameters);

            AssertOutParametersAreExpected(expected, parameters);
        }
    }
}
