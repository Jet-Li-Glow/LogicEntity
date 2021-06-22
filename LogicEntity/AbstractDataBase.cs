using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
        /// <summary>
        /// 数据库数据类型
        /// </summary>
        static readonly Type[] BaseTypes = new Type[] {
            typeof(bool),
            typeof(byte),
            typeof(byte[]),
            typeof(char),
            typeof(char[]),
            typeof(DateTime),
            typeof(decimal),
            typeof(double),
            typeof(float),
            typeof(Guid),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(string),
        };

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns></returns>
        public abstract IDbConnection GetDbConnection();

        /// <summary>
        /// 使用查询操作器查询，并返回 T 类型的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(ISelector selector)
        {
            return Query<T>(selector.GetCommand());
        }

        /// <summary>
        /// 使用查询操作器查询，并返回 DataTable
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public DataTable Query(ISelector selector)
        {
            return Query(selector.GetCommand());
        }

        /// <summary>
        /// 使用命令查询，并返回 T 类型的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(Command command)
        {
            return Query<T>(command.CommandText, command.Parameters, command.CommandTimeout);
        }

        /// <summary>
        /// 使用命令查询，并返回 DataTable
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public DataTable Query(Command command)
        {
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

            type = Nullable.GetUnderlyingType(type) ?? type;

            using (IDbConnection connection = GetDbConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                foreach (KeyValuePair<string, object> kv in keyValues)
                {
                    IDbDataParameter parameter = command.CreateParameter();

                    parameter.ParameterName = kv.Key;

                    parameter.Value = kv.Value;

                    command.Parameters.Add(parameter);
                }

                command.CommandText = sql;

                command.CommandType = CommandType.Text;

                if (commandTimeout > 0)
                    command.CommandTimeout = commandTimeout;

                using (IDataReader reader = command.ExecuteReader())
                {
                    if (IsDbBaseType(type) == false)
                    {
                        while (reader.Read())
                        {
                            T t = Activator.CreateInstance<T>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                PropertyInfo property = type.GetProperty(reader.GetName(i), BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                                if (property is null)
                                    continue;

                                if (property.CanWrite == false)
                                    continue;

                                if (property.PropertyType == typeof(Column))
                                {
                                    (property.GetValue(t) as Column).Value = reader.IsDBNull(i) ? null : reader.GetValue(i);

                                    continue;
                                }

                                if (reader.IsDBNull(i))
                                {
                                    property.SetValue(t, null);

                                    continue;
                                }

                                Type dataType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                                try
                                {
                                    switch (dataType.FullName)
                                    {
                                        case "System.Boolean":
                                            property.SetValue(t, reader.GetBoolean(i));
                                            break;
                                        case "System.Byte":
                                            property.SetValue(t, reader.GetByte(i));
                                            break;
                                        case "System.Char":
                                            property.SetValue(t, reader.GetChar(i));
                                            break;
                                        case "System.DateTime":
                                            property.SetValue(t, reader.GetDateTime(i));
                                            break;
                                        case "System.Decimal":
                                            property.SetValue(t, reader.GetDecimal(i));
                                            break;
                                        case "System.Double":
                                            property.SetValue(t, reader.GetDouble(i));
                                            break;
                                        case "System.Single":
                                            property.SetValue(t, reader.GetFloat(i));
                                            break;
                                        case "System.Guid":
                                            property.SetValue(t, reader.GetGuid(i));
                                            break;
                                        case "System.Int16":
                                            property.SetValue(t, reader.GetInt16(i));
                                            break;
                                        case "System.Int32":
                                            property.SetValue(t, reader.GetInt32(i));
                                            break;
                                        case "System.Int64":
                                            property.SetValue(t, reader.GetInt64(i));
                                            break;
                                        case "System.String":
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
                            {
                                yield return default;

                                continue;
                            }

                            object t = default;

                            try
                            {
                                switch (type.FullName)
                                {
                                    case "System.Boolean":
                                        t = reader.GetBoolean(0);
                                        break;
                                    case "System.Byte":
                                        t = reader.GetByte(0);
                                        break;
                                    case "System.Char":
                                        t = reader.GetChar(0);
                                        break;
                                    case "System.DateTime":
                                        t = reader.GetDateTime(0);
                                        break;
                                    case "System.Decimal":
                                        t = reader.GetDecimal(0);
                                        break;
                                    case "System.Double":
                                        t = reader.GetDouble(0);
                                        break;
                                    case "System.Single":
                                        t = reader.GetFloat(0);
                                        break;
                                    case "System.Guid":
                                        t = reader.GetGuid(0);
                                        break;
                                    case "System.Int16":
                                        t = reader.GetInt16(0);
                                        break;
                                    case "System.Int32":
                                        t = reader.GetInt32(0);
                                        break;
                                    case "System.Int64":
                                        t = reader.GetInt64(0);
                                        break;
                                    case "System.String":
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
                    IDbDataParameter parameter = command.CreateParameter();

                    parameter.ParameterName = kv.Key;

                    parameter.Value = kv.Value;

                    command.Parameters.Add(parameter);
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
        /// <param name="dbOperator">数据库操作器</param>
        /// <returns></returns>
        public int ExecuteNonQuery(IDbOperator dbOperator)
        {
            return ExecuteNonQuery(dbOperator.GetCommand());
        }

        /// <summary>
        /// 执行一个命令，并返回受影响的行数
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns></returns>
        public int ExecuteNonQuery(Command command)
        {
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
                    IDbDataParameter parameter = command.CreateParameter();

                    parameter.ParameterName = kv.Key;

                    parameter.Value = kv.Value;

                    command.Parameters.Add(parameter);
                }

                command.CommandText = sql;

                command.CommandType = CommandType.Text;

                if (commandTimeout > 0)
                    command.CommandTimeout = commandTimeout;

                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 执行数据库操作器，并返回查询结果的第一行的第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbOperator"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(IDbOperator dbOperator)
        {
            return ExecuteScalar<T>(dbOperator.GetCommand());
        }

        /// <summary>
        /// 执行数据库操作器，并返回查询结果的第一行的第一列
        /// </summary>
        /// <param name="dbOperator"></param>
        /// <returns></returns>
        public object ExecuteScalar(IDbOperator dbOperator)
        {
            return ExecuteScalar(dbOperator.GetCommand());
        }

        /// <summary>
        /// 执行一个命令，并返回查询结果的第一行的第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(Command command)
        {
            return ExecuteScalar<T>(command.CommandText, command.Parameters, command.CommandTimeout);
        }

        /// <summary>
        /// 执行一个命令，并返回查询结果的第一行的第一列
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public object ExecuteScalar(Command command)
        {
            return ExecuteScalar(command.CommandText, command.Parameters, command.CommandTimeout);
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

            return ExecuteScalar<T>(command.CommandText, command.Parameters);
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
        public T ExecuteScalar<T>(string sql, IEnumerable<KeyValuePair<string, object>> keyValues)
        {
            return ExecuteScalar<T>(sql, keyValues, 0);
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
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int commandTimeout)
        {
            return (T)Convert.ChangeType(ExecuteScalar(sql, keyValues, commandTimeout), typeof(T));
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
                    IDbDataParameter parameter = command.CreateParameter();

                    parameter.ParameterName = kv.Key;

                    parameter.Value = kv.Value;

                    command.Parameters.Add(parameter);
                }

                command.CommandText = sql;

                command.CommandType = CommandType.Text;

                if (commandTimeout > 0)
                    command.CommandTimeout = commandTimeout;

                return command.ExecuteScalar();
            }
        }

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="dbOperators">数据库操作器</param>
        /// <returns>事务是否执行成功</returns>
        public bool ExecuteTransaction(params IDbOperator[] dbOperators)
        {
            if (dbOperators is null)
                return false;

            return ExecuteTransaction(dbOperators, out int affected, out Exception exception);
        }

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="dbOperators">数据库操作器</param>
        /// <param name="affected">受影响的行数</param>
        /// <param name="exception">事务执行失败时的异常</param>
        /// <returns>事务是否执行成功</returns>
        public bool ExecuteTransaction(IEnumerable<IDbOperator> dbOperators, out int affected, out Exception exception)
        {
            return ExecuteTransaction(dbOperators.Select(o => o.GetCommand()), out affected, out exception);
        }

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="commands">命令</param>
        /// <returns></returns>
        public bool ExecuteTransaction(params Command[] commands)
        {
            if (commands is null)
                return false;

            return ExecuteTransaction(commands, out int affected, out Exception exception);
        }

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="commands">命令</param>
        /// <param name="affected">受影响的行数</param>
        /// <param name="exception">事务执行失败时的异常</param>
        /// <returns>事务是否执行成功</returns>
        public bool ExecuteTransaction(IEnumerable<Command> commands, out int affected, out Exception exception)
        {
            affected = 0;

            exception = null;

            using (IDbConnection connection = GetDbConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                command.Transaction = connection.BeginTransaction();

                try
                {
                    foreach (Command cmd in commands)
                    {
                        command.CommandText = cmd.CommandText;

                        command.CommandType = CommandType.Text;

                        command.Parameters.Clear();

                        foreach (KeyValuePair<string, object> kv in cmd.Parameters)
                        {
                            IDbDataParameter parameter = command.CreateParameter();

                            parameter.ParameterName = kv.Key;

                            parameter.Value = kv.Value;

                            command.Parameters.Add(parameter);
                        }

                        command.CommandTimeout = 30;

                        if (cmd.CommandTimeout > 0)
                            command.CommandTimeout = cmd.CommandTimeout;

                        affected += command.ExecuteNonQuery();
                    }

                    command.Transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    try
                    {
                        command.Transaction.Rollback();
                    }
                    catch
                    {
                    }

                    affected = 0;

                    exception = ex;

                    return false;
                }
            }
        }

        /// <summary>
        /// 创建事务
        /// </summary>
        /// <returns></returns>
        public DbTransaction BeginTransaction()
        {
            return new DbTransaction(this);
        }

        internal static Command ConvertToCommand(string sql, object[] args)
        {
            List<KeyValuePair<string, object>> keyValues = new();

            int index = 0;

            foreach (object obj in args)
            {
                keyValues.Add(KeyValuePair.Create("@param" + index.ToString(), obj));

                index++;
            }

            sql = string.Format(sql, keyValues.Select(s => s.Key).ToArray());

            return new Command() { CommandText = sql, Parameters = keyValues };
        }

        /// <summary>
        /// 是否是数据库数据类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool IsDbBaseType(Type type)
        {
            return BaseTypes.Contains(type);
        }
    }
}
