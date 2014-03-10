using NSubstitute;
using NUnit.Framework;
using System;
using System.Data;

namespace BoringDotNet.Database
{
    [TestFixture]
    public class IDataReaderExtensionsTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetBoolean_WithNullColumn_ThrowsException()
        {
            var dataReader = Substitute.For<IDataReader>();
            dataReader.GetBoolean(null);
        }

        [Test]
        public void GetBoolean_WithColumn_ReturnsData()
        {
            var dataReader = Substitute.For<IDataReader>();
            dataReader.GetOrdinal("column").Returns(3);
            dataReader.GetBoolean(3).Returns(true);

            var actual = dataReader.GetBoolean("column");

            Assert.IsTrue(actual);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetBooleanNullable_WithNullColumn_ThrowsException()
        {
            var dataReader = Substitute.For<IDataReader>();
            dataReader.GetBooleanNullable(null);
        }

        [Test]
        public void GetBooleanNullable_WithColumnAndNullData_ReturnsNull()
        {
            var dataReader = Substitute.For<IDataReader>();
            dataReader.GetOrdinal("column").Returns(3);
            dataReader.IsDBNull(3).Returns(true);

            var actual = dataReader.GetBooleanNullable("column");

            Assert.IsNull(actual);
        }

        [Test]
        public void GetBooleanNullable_WithColumnAndData_ReturnsData()
        {
            var dataReader = Substitute.For<IDataReader>();
            dataReader.GetOrdinal("column").Returns(3);
            dataReader.IsDBNull(3).Returns(false);
            dataReader.GetBoolean(3).Returns(true);

            var actual = dataReader.GetBooleanNullable("column");

            Assert.AreEqual(true, actual);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetInt32_WithNullColumn_ThrowsException()
        {
            var dataReader = Substitute.For<IDataReader>();
            dataReader.GetInt32(null);
        }

        [Test]
        public void GetInt32_WithColumn_ReturnsData()
        {
            var dataReader = Substitute.For<IDataReader>();
            dataReader.GetOrdinal("column").Returns(3);
            dataReader.GetInt32(3).Returns(42);

            var actual = dataReader.GetInt32("column");

            Assert.AreEqual(42, actual);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetInt32Nullable_WithNullColumn_ThrowsException()
        {
            var dataReader = Substitute.For<IDataReader>();
            dataReader.GetInt32Nullable(null);
        }

        [Test]
        public void GetInt32Nullable_WithColumnAndNullData_ReturnsNull()
        {
            var dataReader = Substitute.For<IDataReader>();
            dataReader.GetOrdinal("column").Returns(3);
            dataReader.IsDBNull(3).Returns(true);

            var actual = dataReader.GetInt32Nullable("column");

            Assert.IsNull(actual);
        }

        [Test]
        public void GetInt32Nullable_WithColumnAndData_ReturnsData()
        {
            var dataReader = Substitute.For<IDataReader>();
            dataReader.GetOrdinal("column").Returns(3);
            dataReader.IsDBNull(3).Returns(false);
            dataReader.GetInt32(3).Returns(42);

            var actual = dataReader.GetInt32Nullable("column");

            Assert.AreEqual(42, actual);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetString_WithNullColumn_ThrowsException()
        {
            var dataReader = Substitute.For<IDataReader>();
            dataReader.GetString(null);
        }

        [Test]
        public void GetString_WithColumn_ReturnsData()
        {
            var dataReader = Substitute.For<IDataReader>();
            dataReader.GetOrdinal("column").Returns(3);
            dataReader.GetString(3).Returns("yolo");

            var actual = dataReader.GetString("column");

            Assert.AreEqual("yolo", actual);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetStringNullable_WithNullColumn_ThrowsException()
        {
            var dataReader = Substitute.For<IDataReader>();
            dataReader.GetStringNullable(null);
        }

        [Test]
        public void GetStringNullable_WithColumnAndNullData_ReturnsNull()
        {
            var dataReader = Substitute.For<IDataReader>();
            dataReader.GetOrdinal("column").Returns(3);
            dataReader.IsDBNull(3).Returns(true);

            var actual = dataReader.GetStringNullable("column");

            Assert.IsNull(actual);
        }

        [Test]
        public void GetStringNullable_WithColumnAndData_ReturnsData()
        {
            var dataReader = Substitute.For<IDataReader>();
            dataReader.GetOrdinal("column").Returns(3);
            dataReader.IsDBNull(3).Returns(false);
            dataReader.GetString(3).Returns("yolo");

            var actual = dataReader.GetStringNullable("column");

            Assert.AreEqual("yolo", actual);
        }
    }
}
