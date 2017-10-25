using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;

namespace Web.DataSource
{
    static class Util
    {

        internal static DataTable GetRootTable(DataSet dataSet)
        {
            if (dataSet != null)
            {
                foreach (DataTable table in dataSet.Tables)
                {
                    if (table.ParentRelations.Count == 0)
                    {
                        return table;
                    }
                }
            }

            return null;
        }

        internal static TypeCode GetTypeCodeForType(Type type)
        {
            if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                type = type.GetGenericArguments()[0];
            }
            else if (type.IsByRef)
            {
                type = type.GetElementType();
            }
            if (typeof(bool).IsAssignableFrom(type))
            {
                return TypeCode.Boolean;
            }
            if (typeof(byte).IsAssignableFrom(type))
            {
                return TypeCode.Byte;
            }
            if (typeof(char).IsAssignableFrom(type))
            {
                return TypeCode.Char;
            }
            if (typeof(DateTime).IsAssignableFrom(type))
            {
                return TypeCode.DateTime;
            }
            if (typeof(DBNull).IsAssignableFrom(type))
            {
                return TypeCode.DBNull;
            }
            if (typeof(decimal).IsAssignableFrom(type))
            {
                return TypeCode.Decimal;
            }
            if (typeof(double).IsAssignableFrom(type))
            {
                return TypeCode.Double;
            }
            if (typeof(short).IsAssignableFrom(type))
            {
                return TypeCode.Int16;
            }
            if (typeof(int).IsAssignableFrom(type))
            {
                return TypeCode.Int32;
            }
            if (typeof(long).IsAssignableFrom(type))
            {
                return TypeCode.Int64;
            }
            if (typeof(sbyte).IsAssignableFrom(type))
            {
                return TypeCode.SByte;
            }
            if (typeof(float).IsAssignableFrom(type))
            {
                return TypeCode.Single;
            }
            if (typeof(string).IsAssignableFrom(type))
            {
                return TypeCode.String;
            }
            if (typeof(ushort).IsAssignableFrom(type))
            {
                return TypeCode.UInt16;
            }
            if (typeof(uint).IsAssignableFrom(type))
            {
                return TypeCode.UInt32;
            }
            if (typeof(ulong).IsAssignableFrom(type))
            {
                return TypeCode.UInt64;
            }
            return TypeCode.Object;
        }
    }
}
