using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using LogicEntity.Extension;
using LogicEntity.Interface;
using LogicEntity.Model;
using LogicEntity.Operator;

namespace LogicEntity
{
    public abstract partial class AbstractDataBase
    {
        public abstract IDbConnection GetDbConnection();

        public abstract IDataParameter GetDbParameter(string key, object value);

        /// <summary>
        /// 使用查询操作器查询，并返回 T 类型的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(ISelector selector)
        {
            Command command = selector.GetCommand();
            return Query<T>(command.CommandText, command.Parameters, command.CommandTimeout);
        }

        /// <summary>
        /// 使用查询操作器查询，并返回 DataTable
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public DataTable Query(ISelector selector)
        {
            Command command = selector.GetCommand();
            return Query(command.CommandText, command.Parameters, command.CommandTimeout);
        }

        /// <summary>
        /// 使用SQL语句查询，并返回 T 类型的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, params object[] args)
        {
            Command command = ConvertToCommand(sql, args);

            return Query<T>(command.CommandText, command.Parameters);
        }

        /// <summary>
        /// 使用SQL语句查询，并返回 DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public DataTable Query(string sql, params object[] args)
        {
            Command command = ConvertToCommand(sql, args);

            return Query(command.CommandText, command.Parameters);
        }

        /// <summary>
        /// 使用SQL语句查询，并返回 T 类型的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, IEnumerable<KeyValuePair<string, object>> keyValues)
        {
            return Query<T>(sql, keyValues, 0);
        }

        /// <summary>
        /// 使用SQL语句查询，并返回 T 类型的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int commandTimeout)
        {
            Type type = typeof(T);

            TypeCode typeCode = Type.GetTypeCode(type);

            if (typeCode == TypeCode.Empty)
                yield break;

            using (IDbConnection connection = GetDbConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                foreach (KeyValuePair<string, object> kv in keyValues)
                {
                    command.Parameters.Add(GetDbParameter(kv.Key, kv.Value));
                }

                command.CommandText = sql;

                command.CommandType = CommandType.Text;

                if (commandTimeout > 0)
                    command.CommandTimeout = commandTimeout;

                using (IDataReader reader = command.ExecuteReader())
                {
                    if (typeCode == TypeCode.Object)
                    {
                        while (reader.Read())
                        {
                            T t = Activator.CreateInstance<T>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                PropertyInfo  property = type.GetProperty(reader.GetName(i), BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                                if (property is null)
                                    continue;

                                if (reader.IsDBNull(i))
                                {
                                    property.SetValue(t, null);
                                    continue;
                                }

                                Type dataType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                                try
                                {
                                    switch (dataType.Name)
                                    {
                                        case "Boolean":
                                            property.SetValue(t, reader.GetBoolean(i));
                                            break;
                                        case "Byte":
                                            property.SetValue(t, reader.GetByte(i));
                                            break;
                                        case "Char":
                                            property.SetValue(t, reader.GetChar(i));
                                            break;
                                        case "DateTime":
                                            property.SetValue(t, reader.GetDateTime(i));
                                            break;
                                        case "Decimal":
                                            property.SetValue(t, reader.GetDecimal(i));
                                            break;
                                        case "Double":
                                            property.SetValue(t, reader.GetDouble(i));
                                            break;
                                        case "Single":
                                            property.SetValue(t, reader.GetFloat(i));
                                            break;
                                        case "Guid":
                                            property.SetValue(t, reader.GetGuid(i));
                                            break;
                                        case "Int16":
                                            property.SetValue(t, reader.GetInt16(i));
                                            break;
                                        case "Int32":
                                            property.SetValue(t, reader.GetInt32(i));
                                            break;
                                        case "Int64":
                                            property.SetValue(t, reader.GetInt64(i));
                                            break;
                                        case "String":
                                            property.SetValue(t, reader.GetString(i));
                                            break;
                                        default:
                                            property.SetValue(t, Convert.ChangeType(reader.GetValue(i), dataType));
                                            break;
                                    }
                                }
                                catch (Exception exception)
                                {
                                    throw new InvalidCastException("属性 " + property.Name + " 类型转换异常", exception);
                                }
                            }

                            yield return t;
                        }
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0))
                                yield return default;

                            object t = default;

                            try
                            {
                                switch (type.Name)
                                {
                                    case "Boolean":
                                        t = reader.GetBoolean(0);
                                        break;
                                    case "Byte":
                                        t = reader.GetByte(0);
                                        break;
                                    case "Char":
                                        t = reader.GetChar(0);
                                        break;
                                    case "DateTime":
                                        t = reader.GetDateTime(0);
                                        break;
                                    case "Decimal":
                                        t = reader.GetDecimal(0);
                                        break;
                                    case "Double":
                                        t = reader.GetDouble(0);
                                        break;
                                    case "Single":
                                        t = reader.GetFloat(0);
                                        break;
                                    case "Guid":
                                        t = reader.GetGuid(0);
                                        break;
                                    case "Int16":
                                        t = reader.GetInt16(0);
                                        break;
                                    case "Int32":
                                        t = reader.GetInt32(0);
                                        break;
                                    case "Int64":
                                        t = reader.GetInt64(0);
                                        break;
                                    case "String":
                                        t = reader.GetString(0);
                                        break;
                                    default:
                                        t = Convert.ChangeType(reader[0], type);
                                        break;
                                }
                            }
                            catch (Exception exception)
                            {
                                throw new InvalidCastException("列 " + reader.GetName(0) + " 类型转换异常", exception);
                            }

                            yield return (T)t;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 使用SQL语句查询，并返回 DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public DataTable Query(string sql, IEnumerable<KeyValuePair<string, object>> keyValues)
        {
            return Query(sql, keyValues, 0);
        }

        /// <summary>
        /// 使用SQL语句查询，并返回 DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public DataTable Query(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int commandTimeout)
        {
            using (IDbConnection connection = GetDbConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                foreach (KeyValuePair<string, object> kv in keyValues)
                {
                    command.Parameters.Add(GetDbParameter(kv.Key, kv.Value));
                }

                command.CommandText = sql;

                command.CommandType = CommandType.Text;

                if (commandTimeout > 0)
                    command.CommandTimeout = commandTimeout;

                using (IDataReader reader = command.ExecuteReader())
                {
                    DataTable result = new();

                    result.Load(reader);

                    return result;
                }
            }
        }

        /// <summary>
        /// 执行一个数据库操作器，并返回受影响的行数
        /// </summary>
        /// <param name="dbOperator"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(IDbOperator dbOperator)
        {
            Command command = dbOperator.GetCommand();

            return ExecuteNonQuery(command.CommandText, command.Parameters, command.CommandTimeout);
        }

        /// <summary>
        /// 执行一个SQL语句，并返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, params object[] args)
        {
            Command command = ConvertToCommand(sql, args);

            return ExecuteNonQuery(command.CommandText, command.Parameters);
        }

        /// <summary>
        /// 执行一个SQL语句，并返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, IEnumerable<KeyValuePair<string, object>> keyValues)
        {
            return ExecuteNonQuery(sql, keyValues, 0);
        }

        /// <summary>
        /// 执行一个SQL语句，并返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int commandTimeout)
        {
            using (IDbConnection connection = GetDbConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                foreach (KeyValuePair<string, object> kv in keyValues)
                {
                    command.Parameters.Add(GetDbParameter(kv.Key, kv.Value));
                }

                command.CommandText = sql;

                command.CommandType = CommandType.Text;

                if (commandTimeout > 0)
                    command.CommandTimeout = commandTimeout;

                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 执行SQL语句，并返回查询结果的第一行的第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, params object[] args)
        {
            Command command = ConvertToCommand(sql, args);

            return (T)ExecuteScalar(command.CommandText, command.Parameters);
        }

        /// <summary>
        /// 执行数据库操作器，并返回查询结果的第一行的第一列
        /// </summary>
        /// <param name="dbOperator"></param>
        /// <returns></returns>
        public object ExecuteScalar(IDbOperator dbOperator)
        {
            Command command = dbOperator.GetCommand();

            return ExecuteScalar(command.CommandText, command.Parameters, command.CommandTimeout);
        }

        /// <summary>
        /// 执行SQL语句，并返回查询结果的第一行的第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, params object[] args)
        {
            Command command = ConvertToCommand(sql, args);

            return ExecuteScalar(command.CommandText, command.Parameters);
        }

        /// <summary>
        /// 执行SQL语句，并返回查询结果的第一行的第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public object ExecuteScalar<T>(string sql, IEnumerable<KeyValuePair<string, object>> keyValues)
        {
            return (T)ExecuteScalar(sql, keyValues);
        }

        /// <summary>
        /// 执行SQL语句，并返回查询结果的第一行的第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, IEnumerable<KeyValuePair<string, object>> keyValues)
        {
            return ExecuteScalar(sql, keyValues, 0);
        }

        /// <summary>
        /// 执行SQL语句，并返回查询结果的第一行的第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int commandTimeout)
        {
            using (IDbConnection connection = GetDbConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                foreach (KeyValuePair<string, object> kv in keyValues)
                {
                    command.Parameters.Add(GetDbParameter(kv.Key, kv.Value));
                }

                command.CommandText = sql;

                command.CommandType = CommandType.Text;

                if (commandTimeout > 0)
                    command.CommandTimeout = commandTimeout;

                return command.ExecuteScalar();
            }
        }

        private static Command ConvertToCommand(string sql, object[] args)
        {
            List<KeyValuePair<string, object>> keyValues = new();

            int index = 0;

            foreach (object obj in args)
            {
                keyValues.Add(KeyValuePair.Create("param" + index.ToString(), obj));

                index++;
            }

            sql = string.Format(sql, args: keyValues.Select(s => s.Key));

            return new Command() { CommandText = sql, Parameters = keyValues };
        }
    }
}
