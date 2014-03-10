using System;
using System.Data;

namespace BoringDotNet.Database
{
    public static class IDataReaderExtensions
    {
        public static bool GetBoolean(this IDataReader reader, string column)
        {
            if (column == null)
                throw new ArgumentNullException("column");
            return reader.GetBoolean(reader.GetOrdinal(column));
        }

        public static bool? GetBooleanNullable(this IDataReader reader, string column)
        {
            if (column == null)
                throw new ArgumentNullException("column");
            var ordinal = reader.GetOrdinal(column);
            if (reader.IsDBNull(ordinal))
                return null;
            return reader.GetBoolean(ordinal);
        }

        public static int GetInt32(this IDataReader reader, string column)
        {
            if (column == null)
                throw new ArgumentNullException("column");
            return reader.GetInt32(reader.GetOrdinal(column));
        }

        public static int? GetInt32Nullable(this IDataReader reader, string column)
        {
            if (column == null)
                throw new ArgumentNullException("column");
            var ordinal = reader.GetOrdinal(column);
            if (reader.IsDBNull(ordinal))
                return null;
            return reader.GetInt32(ordinal);
        }

        public static string GetString(this IDataReader reader, string column)
        {
            if (column == null)
                throw new ArgumentNullException("column");
            return reader.GetString(reader.GetOrdinal(column));
        }

        public static string GetStringNullable(this IDataReader reader, string column)
        {
            if (column == null)
                throw new ArgumentNullException("column");
            var ordinal = reader.GetOrdinal(column);
            if (reader.IsDBNull(ordinal))
                return null;
            return reader.GetString(ordinal);
        }
    }
}
