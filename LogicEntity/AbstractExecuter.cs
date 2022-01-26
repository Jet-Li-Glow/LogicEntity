using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Interface;
using LogicEntity.Model;

namespace LogicEntity
{
    public abstract class AbstractExecuter
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
        /// sql转Command
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        static Command ConvertToCommand(string sql, object[] args)
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
        static bool IsDbBaseType(Type type)
        {
            return BaseTypes.Contains(type);
        }

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
            return Query<T>(command.CommandText, command.Parameters, command.CommandTimeout, command.Readers);
        }

        /// <summary>
        /// 使用命令查询，并返回 DataTable
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public DataTable Query(Command command)
        {
            return Query(command.CommandText, command.Parameters, command.CommandTimeout, command.Readers);
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
            return Query<T>(sql, keyValues, 0, new Dictionary<int, Func<object, object>>());
        }

        /// <summary>
        /// 使用SQL语句查询，并返回 T 类型的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="clientReaders"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int commandTimeout, Dictionary<int, Func<object, object>> clientReaders)
        {
            return AbstractQuery<T>(sql, keyValues, commandTimeout, clientReaders);
        }

        /// <summary>
        /// 使用SQL语句查询，并返回 T 类型的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="clientReaders"></param>
        /// <returns></returns>
        protected internal abstract IEnumerable<T> AbstractQuery<T>(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int commandTimeout, Dictionary<int, Func<object, object>> clientReaders);

        /// <summary>
        /// 使用SQL语句查询，并返回 T 类型的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        protected internal IEnumerable<T> Query<T>(IDbConnection connection, string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int commandTimeout, Dictionary<int, Func<object, object>> clientReaders)
        {
            Type type = typeof(T);

            type = Nullable.GetUnderlyingType(type) ?? type;

            IDbCommand command = connection.CreateCommand();

            if (keyValues is not null)
            {
                foreach (KeyValuePair<string, object> kv in keyValues)
                {
                    IDbDataParameter parameter = command.CreateParameter();

                    parameter.ParameterName = kv.Key;

                    parameter.Value = kv.Value;

                    command.Parameters.Add(parameter);
                }
            }

            command.CommandText = sql;

            command.CommandType = CommandType.Text;

            if (commandTimeout > 0)
                command.CommandTimeout = commandTimeout;

            using (IDataReader reader = command.ExecuteReader())
            {
                if (IsDbBaseType(type) == false)
                {
                    List<Action<T, IDataReader>> fillActions = new();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        PropertyInfo property = type.GetProperty(reader.GetName(i), BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                        if (property is null)
                            continue;

                        if (property.CanWrite == false)
                            continue;

                        fillActions.Add(_GetFillAction(property, i));
                    }

                    while (reader.Read())
                    {
                        T t = Activator.CreateInstance<T>();

                        foreach (Action<T, IDataReader> fill in fillActions)
                        {
                            fill(t, reader);
                        }

                        yield return t;
                    }
                }
                else
                {
                    Func<IDataReader, object> cellReader = _GetCellReader(type, 0);

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
                            t = cellReader(reader);
                        }
                        catch (Exception exception)
                        {
                            throw new InvalidCastException("列 " + reader.GetName(0) + " 类型转换异常", exception);
                        }

                        yield return (T)t;
                    }
                }
            }

            Action<T, IDataReader> _GetFillAction(PropertyInfo property, int i)
            {
                if (property.PropertyType == typeof(Column))
                    return (t, reader) =>
                    {
                        (property.GetValue(t) as Column).Value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    };

                Type dataType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                Func<IDataReader, object> cellReader = _GetCellReader(dataType, i);

                if (clientReaders is not null && clientReaders.TryGetValue(i, out Func<object, object> clientReader))
                {
                    cellReader = reader =>
                    {
                        object obj = reader.GetValue(i);

                        if (obj is DBNull)
                            obj = null;

                        return clientReader(obj);
                    };
                }

                return (t, reader) =>
                {
                    if (reader.IsDBNull(i))
                    {
                        property.SetValue(t, null);

                        return;
                    }

                    object cell = null;

                    try
                    {
                        cell = cellReader(reader);
                    }
                    catch (Exception exception)
                    {
                        throw new InvalidCastException("属性 " + property.Name + " 类型转换异常", exception);
                    }

                    property.SetValue(t, cell);
                };
            }

            Func<IDataReader, object> _GetCellReader(Type type, int i)
            {
                if (type == typeof(Column))
                    return (reader) => reader.IsDBNull(i) ? null : reader.GetValue(i);

                if (type == typeof(bool))
                    return (reader) => reader.GetBoolean(i);

                if (type == typeof(byte))
                    return (reader) => reader.GetByte(i);

                if (type == typeof(char))
                    return (reader) => reader.GetChar(i);

                if (type == typeof(DateTime))
                    return (reader) => reader.GetDateTime(i);

                if (type == typeof(decimal))
                    return (reader) => reader.GetDecimal(i);

                if (type == typeof(double))
                    return (reader) => reader.GetDouble(i);

                if (type == typeof(float))
                    return (reader) => reader.GetFloat(i);

                if (type == typeof(Guid))
                    return (reader) => reader.GetGuid(i);

                if (type == typeof(short))
                    return (reader) => reader.GetInt16(i);

                if (type == typeof(int))
                    return (reader) => reader.GetInt32(i);

                if (type == typeof(long))
                    return (reader) => reader.GetInt64(i);

                if (type == typeof(string))
                    return (reader) => reader.GetString(i);

                if (type.IsSubclassOf(typeof(Enum)))
                    return (reader) =>
                    {
                        object enumValue = Enum.Parse(type, reader.GetString(i));

                        if (Enum.IsDefined(type, enumValue) == false)
                            throw new InvalidEnumArgumentException("尝试将【" + reader.GetString(i) + "】转换为 " + type.Name);

                        return enumValue;
                    };

                return (reader) => Convert.ChangeType(reader.GetValue(i), type);
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
            return Query(sql, keyValues, 0, new Dictionary<int, Func<object, object>>());
        }

        /// <summary>
        /// 使用SQL语句查询，并返回 DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="clientReaders"></param>
        /// <returns></returns>
        public DataTable Query(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int commandTimeout, Dictionary<int, Func<object, object>> clientReaders)
        {
            return AbstractQuery(sql, keyValues, commandTimeout, clientReaders);
        }

        /// <summary>
        /// 使用SQL语句查询，并返回 DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="clientReaders"></param>
        /// <returns></returns>
        protected internal abstract DataTable AbstractQuery(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int commandTimeout, Dictionary<int, Func<object, object>> clientReaders);

        /// <summary>
        /// 使用SQL语句查询，并返回 DataTable
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="clientReaders"></param>
        /// <returns></returns>
        protected internal DataTable Query(IDbConnection connection, string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int commandTimeout, Dictionary<int, Func<object, object>> clientReaders)
        {
            IDbCommand command = connection.CreateCommand();

            if (keyValues is not null)
            {
                foreach (KeyValuePair<string, object> kv in keyValues)
                {
                    IDbDataParameter parameter = command.CreateParameter();

                    parameter.ParameterName = kv.Key;

                    parameter.Value = kv.Value;

                    command.Parameters.Add(parameter);
                }
            }

            command.CommandText = sql;

            command.CommandType = CommandType.Text;

            if (commandTimeout > 0)
                command.CommandTimeout = commandTimeout;

            DataTable result = new();

            using (IDataReader reader = command.ExecuteReader())
            {
                result.Load(reader);
            }

            if (clientReaders is not null)
            {
                foreach (KeyValuePair<int, Func<object, object>> clientReader in clientReaders)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        row[clientReader.Key] = clientReader.Value(row[clientReader.Key]);
                    }
                }
            }

            return result;
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
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int commandTimeout)
        {
            return AbstractExecuteNonQuery(sql, keyValues, commandTimeout);
        }

        /// <summary>
        /// 执行一个SQL语句，并返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        protected internal abstract int AbstractExecuteNonQuery(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int commandTimeout);

        /// <summary>
        /// 执行一个SQL语句，并返回受影响的行数
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        protected internal int ExecuteNonQuery(IDbConnection connection, string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int commandTimeout)
        {
            IDbCommand command = connection.CreateCommand();

            if (keyValues is not null)
            {
                foreach (KeyValuePair<string, object> kv in keyValues)
                {
                    IDbDataParameter parameter = command.CreateParameter();

                    parameter.ParameterName = kv.Key;

                    parameter.Value = kv.Value;

                    command.Parameters.Add(parameter);
                }
            }

            command.CommandText = sql;

            command.CommandType = CommandType.Text;

            if (commandTimeout > 0)
                command.CommandTimeout = commandTimeout;

            return command.ExecuteNonQuery();
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
            return ExecuteScalar<T>(command.CommandText, command.Parameters, command.CommandTimeout, command.Readers?.FirstOrDefault().Value);
        }

        /// <summary>
        /// 执行一个命令，并返回查询结果的第一行的第一列
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public object ExecuteScalar(Command command)
        {
            return ExecuteScalar(command.CommandText, command.Parameters, command.CommandTimeout, command.Readers?.FirstOrDefault().Value);
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
            return ExecuteScalar<T>(sql, keyValues, 0, null);
        }

        /// <summary>
        /// 执行SQL语句，并返回查询结果的第一行的第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, IEnumerable<KeyValuePair<string, object>> keyValues)
        {
            return ExecuteScalar(sql, keyValues, 0, null);
        }

        /// <summary>
        /// 执行SQL语句，并返回查询结果的第一行的第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int commandTimeout, Func<object, object> clientReader)
        {
            return (T)Convert.ChangeType(ExecuteScalar(sql, keyValues, commandTimeout, clientReader), typeof(T));
        }

        /// <summary>
        /// 执行SQL语句，并返回查询结果的第一行的第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="clientReader"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int commandTimeout, Func<object, object> clientReader)
        {
            return AbstractExecuteScalar(sql, keyValues, commandTimeout, clientReader);
        }

        /// <summary>
        /// 执行SQL语句，并返回查询结果的第一行的第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="clientReader"></param>
        /// <returns></returns>
        protected internal abstract object AbstractExecuteScalar(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int commandTimeout, Func<object, object> clientReader);

        /// <summary>
        /// 执行SQL语句，并返回查询结果的第一行的第一列
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="clientReader"></param>
        /// <returns></returns>
        protected internal object ExecuteScalar(IDbConnection connection, string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int commandTimeout, Func<object, object> clientReader)
        {
            IDbCommand command = connection.CreateCommand();

            if (keyValues is not null)
            {
                foreach (KeyValuePair<string, object> kv in keyValues)
                {
                    IDbDataParameter parameter = command.CreateParameter();

                    parameter.ParameterName = kv.Key;

                    parameter.Value = kv.Value;

                    command.Parameters.Add(parameter);
                }
            }

            command.CommandText = sql;

            command.CommandType = CommandType.Text;

            if (commandTimeout > 0)
                command.CommandTimeout = commandTimeout;

            object result = command.ExecuteScalar();

            if (clientReader is not null)
                result = clientReader(result);

            return result;
        }
    }
}
