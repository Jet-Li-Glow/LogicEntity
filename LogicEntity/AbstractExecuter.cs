using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
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
            typeof(object)
        };

        static readonly PropertyInfo _ColumnValue = typeof(Column).GetProperty(nameof(Column.Value));
        static readonly MethodInfo _GetBoolean = typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetBoolean));
        static readonly MethodInfo _GetByte = typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetByte));
        static readonly MethodInfo _GetBytes = typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetBytes));
        static readonly MethodInfo _GetChar = typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetChar));
        static readonly MethodInfo _GetChars = typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetChars));
        static readonly MethodInfo _GetDateTime = typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetDateTime));
        static readonly MethodInfo _GetDecimal = typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetDecimal));
        static readonly MethodInfo _GetDouble = typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetDouble));
        static readonly MethodInfo _GetFloat = typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetFloat));
        static readonly MethodInfo _GetGuid = typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetGuid));
        static readonly MethodInfo _GetInt16 = typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetInt16));
        static readonly MethodInfo _GetInt32 = typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetInt32));
        static readonly MethodInfo _GetInt64 = typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetInt64));
        static readonly MethodInfo _GetString = typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetString));
        static readonly MethodInfo _GetValue = typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetValue));
        static readonly MethodInfo _IsDBNull = typeof(IDataRecord).GetMethod(nameof(IDataRecord.IsDBNull));
        static readonly MethodInfo _ChangeType = typeof(Convert).GetMethod(nameof(Convert.ChangeType), new[] { typeof(object), typeof(Type) });
        static readonly MethodInfo _EnumParse = typeof(Enum).GetMethod(nameof(Enum.Parse), new[] { typeof(Type), typeof(string), typeof(bool) });
        static readonly MethodInfo _EnumIsDefined = typeof(Enum).GetMethod(nameof(Enum.IsDefined), new[] { typeof(Type), typeof(object) });
        static readonly ConstructorInfo _InvalidEnumArgumentExceptionConstructor = typeof(InvalidEnumArgumentException).GetConstructor(new[] { typeof(string) });
        static readonly MethodInfo _StringConcat = typeof(string).GetMethod(nameof(string.Concat), new[] { typeof(string), typeof(string), typeof(string) });
        static readonly MethodInfo _ClientReaderInvoke = typeof(Func<object, object>).GetMethod(nameof(Func<object, object>.Invoke));
        static readonly MethodInfo _ClientBytesReaderInvoke = typeof(Func<Func<long, byte[], int, int, long>, object>).GetMethod(nameof(Func<Func<long, byte[], int, int, long>, object>.Invoke));
        static readonly MethodInfo _ClientCharsReaderInvoke = typeof(Func<Func<long, char[], int, int, long>, object>).GetMethod(nameof(Func<Func<long, char[], int, int, long>, object>.Invoke));

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

            Command command = new();

            command.CommandText = sql;

            command.Parameters.AddRange(keyValues);

            return command;
        }

        /// <summary>
        /// 是否是数据库数据类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static bool IsDbBaseType(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;

            return BaseTypes.Contains(type) || type.IsSubclassOf(typeof(Enum));
        }

        /// <summary>
        /// 使用查询操作器查询，并返回 T 类型的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(ISelector selector) where T : new()
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
        public IEnumerable<T> Query<T>(Command command) where T : new()
        {
            return Query<T>(command.CommandText, command.Parameters, command.CommandTimeout, command.Readers, command.BytesReaders, command.CharsReaders);
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
        public IEnumerable<T> Query<T>(string sql, params object[] args) where T : new()
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
        public IEnumerable<T> Query<T>(string sql, IEnumerable<KeyValuePair<string, object>> keyValues) where T : new()
        {
            return Query<T>(sql, keyValues, null, new Dictionary<int, Func<object, object>>(), new Dictionary<int, Func<Func<long, byte[], int, int, long>, object>>(), new Dictionary<int, Func<Func<long, char[], int, int, long>, object>>());
        }

        /// <summary>
        /// 使用SQL语句查询，并返回 T 类型的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="clientReaders"></param>
        /// <param name="clientBytesReaders"></param>
        /// <param name="clientCharsReaders"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int? commandTimeout, Dictionary<int, Func<object, object>> clientReaders, Dictionary<int, Func<Func<long, byte[], int, int, long>, object>> clientBytesReaders, Dictionary<int, Func<Func<long, char[], int, int, long>, object>> clientCharsReaders) where T : new()
        {
            return AbstractQuery<T>(sql, keyValues, commandTimeout, clientReaders, clientBytesReaders, clientCharsReaders);
        }

        /// <summary>
        /// 使用SQL语句查询，并返回 T 类型的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="clientReaders"></param>
        /// <param name="clientBytesReaders"></param>
        /// <param name="clientCharsReaders"></param>
        /// <returns></returns>
        protected internal abstract IEnumerable<T> AbstractQuery<T>(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int? commandTimeout, Dictionary<int, Func<object, object>> clientReaders, Dictionary<int, Func<Func<long, byte[], int, int, long>, object>> clientBytesReaders, Dictionary<int, Func<Func<long, char[], int, int, long>, object>> clientCharsReaders) where T : new();

        /// <summary>
        /// 使用SQL语句查询，并返回 T 类型的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="clientReaders"></param>
        /// <param name="clientBytesReaders"></param>
        /// <param name="clientCharsReaders"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        protected internal IEnumerable<T> Query<T>(IDbConnection connection, string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int? commandTimeout, Dictionary<int, Func<object, object>> clientReaders, Dictionary<int, Func<Func<long, byte[], int, int, long>, object>> clientBytesReaders, Dictionary<int, Func<Func<long, char[], int, int, long>, object>> clientCharsReaders) where T : new()
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

            if (commandTimeout.HasValue)
                command.CommandTimeout = commandTimeout.Value;

            using (IDataReader reader = command.ExecuteReader())
            {
                if (IsDbBaseType(typeof(T)) == false)
                {
                    Action<T> fill = _GetFillAction(reader);

                    while (reader.Read())
                    {
                        T t = new();

                        fill(t);

                        yield return t;
                    }
                }
                else
                {
                    _GetValueExpression(reader, typeof(T), 0, out List<ParameterExpression> variables, out List<Expression> body, out Expression val);

                    Expression bodyExpression = val;

                    if (variables.Any() && body.Any())
                    {
                        body.Add(val);

                        bodyExpression = Expression.Block(variables, body);
                    }

                    Func<T> cellReader = Expression.Lambda<Func<T>>(bodyExpression).Compile();

                    while (reader.Read())
                    {
                        if (reader.IsDBNull(0))
                        {
                            yield return default;

                            continue;
                        }

                        object t = default;

                        t = cellReader();

                        yield return (T)t;
                    }
                }
            }

            Action<T> _GetFillAction(IDataReader reader)
            {
                List<ParameterExpression> variables = new();

                List<Expression> body = new();

                ParameterExpression instance = Expression.Parameter(typeof(T));

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    PropertyInfo property = typeof(T).GetProperty(reader.GetName(i), BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                    if (property is null)
                        continue;

                    if (property.CanWrite == false)
                        continue;

                    Type targetType = property.PropertyType;

                    Expression targetAccess = Expression.Property(instance, property);

                    if (property.PropertyType == typeof(Column))
                    {
                        targetType = typeof(object);

                        targetAccess = Expression.Property(targetAccess, _ColumnValue);
                    }

                    _GetValueExpression(reader, targetType, i, out List<ParameterExpression> fieldVariables, out List<Expression> fieldBody, out Expression val);

                    variables.AddRange(fieldVariables);

                    body.AddRange(fieldBody);

                    body.Add(Expression.Assign(targetAccess, val));
                }

#if DEBUG
                Expression expression = Expression.Lambda<Action<T>>(Expression.Block(variables, body), instance);
#endif

                return Expression.Lambda<Action<T>>(Expression.Block(variables, body), instance).Compile();
            }

            void _GetValueExpression(IDataReader reader, Type targetType, int i, out List<ParameterExpression> variables, out List<Expression> body, out Expression val)
            {
                variables = new();

                body = new();

                val = null;

                Type dataType = Nullable.GetUnderlyingType(targetType) ?? targetType;

                Expression isDBNull = Expression.Call(Expression.Constant(reader), _IsDBNull, Expression.Constant(i));

                if (clientReaders is not null && clientReaders.TryGetValue(i, out Func<object, object> clientReader))
                {
                    ParameterExpression obj = Expression.Parameter(typeof(object));

                    variables.Add(obj);

                    body.Add(Expression.Assign(obj, Expression.Call(Expression.Constant(reader), _GetValue, Expression.Constant(i))));

                    val = Expression.Call(Expression.Constant(clientReader), _ClientReaderInvoke, Expression.Condition(
                        Expression.TypeIs(obj, typeof(DBNull)),
                        Expression.Default(typeof(object)),
                        obj
                        ));

                    if (val.Type != targetType)
                        val = Expression.Convert(val, targetType);
                }
                else if (clientBytesReaders is not null && clientBytesReaders.TryGetValue(i, out Func<Func<long, byte[], int, int, long>, object> clientBytesReader))
                {
                    val = Expression.Call(Expression.Constant(clientBytesReader), _ClientBytesReaderInvoke,
                        Expression.Constant(new Func<long, byte[], int, int, long>((offset, buffer, bufferOffset, length) => reader.GetBytes(i, offset, buffer, bufferOffset, length))));

                    if (val.Type != targetType)
                        val = Expression.Convert(val, targetType);
                }
                else if (clientCharsReaders is not null && clientCharsReaders.TryGetValue(i, out Func<Func<long, char[], int, int, long>, object> clientCharsReader))
                {
                    val = Expression.Call(Expression.Constant(clientCharsReader), _ClientCharsReaderInvoke,
                        Expression.Constant(new Func<long, char[], int, int, long>((offset, buffer, bufferOffset, length) => reader.GetChars(i, offset, buffer, bufferOffset, length))));

                    if (val.Type != targetType)
                        val = Expression.Convert(val, targetType);
                }
                else if (dataType == typeof(bool))
                {
                    val = Expression.Call(Expression.Constant(reader), _GetBoolean, Expression.Constant(i));

                    if (val.Type != targetType)
                        val = Expression.Convert(val, targetType);

                    val = Expression.Condition(isDBNull, Expression.Default(targetType), val);
                }
                else if (dataType == typeof(byte))
                {
                    val = Expression.Call(Expression.Constant(reader), _GetByte, Expression.Constant(i));

                    if (val.Type != targetType)
                        val = Expression.Convert(val, targetType);

                    val = Expression.Condition(isDBNull, Expression.Default(targetType), val);
                }
                else if (dataType == typeof(char))
                {
                    val = Expression.Call(Expression.Constant(reader), _GetChar, Expression.Constant(i));

                    if (val.Type != targetType)
                        val = Expression.Convert(val, targetType);

                    val = Expression.Condition(isDBNull, Expression.Default(targetType), val);
                }
                else if (dataType == typeof(DateTime))
                {
                    val = Expression.Call(Expression.Constant(reader), _GetDateTime, Expression.Constant(i));

                    if (val.Type != targetType)
                        val = Expression.Convert(val, targetType);

                    val = Expression.Condition(isDBNull, Expression.Default(targetType), val);
                }
                else if (dataType == typeof(decimal))
                {
                    val = Expression.Call(Expression.Constant(reader), _GetDecimal, Expression.Constant(i));

                    if (val.Type != targetType)
                        val = Expression.Convert(val, targetType);

                    val = Expression.Condition(isDBNull, Expression.Default(targetType), val);
                }
                else if (dataType == typeof(double))
                {
                    val = Expression.Call(Expression.Constant(reader), _GetDouble, Expression.Constant(i));

                    if (val.Type != targetType)
                        val = Expression.Convert(val, targetType);

                    val = Expression.Condition(isDBNull, Expression.Default(targetType), val);
                }
                else if (dataType == typeof(float))
                {
                    val = Expression.Call(Expression.Constant(reader), _GetFloat, Expression.Constant(i));

                    if (val.Type != targetType)
                        val = Expression.Convert(val, targetType);

                    val = Expression.Condition(isDBNull, Expression.Default(targetType), val);
                }
                else if (dataType == typeof(Guid))
                {
                    val = Expression.Call(Expression.Constant(reader), _GetGuid, Expression.Constant(i));

                    if (val.Type != targetType)
                        val = Expression.Convert(val, targetType);

                    val = Expression.Condition(isDBNull, Expression.Default(targetType), val);
                }
                else if (dataType == typeof(short))
                {
                    val = Expression.Call(Expression.Constant(reader), _GetInt16, Expression.Constant(i));

                    if (val.Type != targetType)
                        val = Expression.Convert(val, targetType);

                    val = Expression.Condition(isDBNull, Expression.Default(targetType), val);
                }
                else if (dataType == typeof(int))
                {
                    val = Expression.Call(Expression.Constant(reader), _GetInt32, Expression.Constant(i));

                    if (val.Type != targetType)
                        val = Expression.Convert(val, targetType);

                    val = Expression.Condition(isDBNull, Expression.Default(targetType), val);
                }
                else if (dataType == typeof(long))
                {
                    val = Expression.Call(Expression.Constant(reader), _GetInt64, Expression.Constant(i));

                    if (val.Type != targetType)
                        val = Expression.Convert(val, targetType);

                    val = Expression.Condition(isDBNull, Expression.Default(targetType), val);
                }
                else if (dataType == typeof(string))
                {
                    val = Expression.Call(Expression.Constant(reader), _GetString, Expression.Constant(i));

                    if (val.Type != targetType)
                        val = Expression.Convert(val, targetType);

                    val = Expression.Condition(isDBNull, Expression.Default(targetType), val);
                }
                else if (dataType == typeof(object))
                {
                    val = Expression.Call(Expression.Constant(reader), _GetValue, Expression.Constant(i));

                    if (val.Type != targetType)
                        val = Expression.Convert(val, targetType);

                    val = Expression.Condition(isDBNull, Expression.Default(targetType), val);
                }
                else if (dataType.IsSubclassOf(typeof(Enum)))
                {
                    ParameterExpression enumValue = Expression.Variable(typeof(object));

                    variables.Add(enumValue);

                    body.Add(Expression.Assign(enumValue, Expression.Convert(Expression.Default(targetType), typeof(object))));

                    Expression assign = Expression.Assign(
                        enumValue,
                        Expression.Call(_EnumParse, Expression.Constant(dataType), Expression.Call(Expression.Constant(reader), _GetString, Expression.Constant(i)), Expression.Constant(true))
                        );

                    Expression exceptionInfo = Expression.Call(_StringConcat,
                        Expression.Constant("尝试将【"),
                        Expression.Call(Expression.Constant(reader), _GetString, Expression.Constant(i)),
                        Expression.Constant("】转换为 " + dataType.FullName)
                        );

                    Expression check = Expression.IfThen(
                        Expression.Not(Expression.Call(_EnumIsDefined, Expression.Constant(dataType), enumValue)),
                        Expression.Throw(Expression.New(_InvalidEnumArgumentExceptionConstructor, exceptionInfo))
                        );

                    body.Add(Expression.IfThen(Expression.Not(isDBNull), Expression.Block(assign, check)));

                    val = Expression.Convert(enumValue, targetType);
                }
                else
                {
                    val = Expression.Call(_ChangeType, Expression.Call(Expression.Constant(reader), _GetValue, Expression.Constant(i)), Expression.Constant(targetType));

                    val = Expression.Convert(val, targetType);

                    val = Expression.Condition(isDBNull, Expression.Default(targetType), val);
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
            return Query(sql, keyValues, null, new Dictionary<int, Func<object, object>>());
        }

        /// <summary>
        /// 使用SQL语句查询，并返回 DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="clientReaders"></param>
        /// <returns></returns>
        public DataTable Query(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int? commandTimeout, Dictionary<int, Func<object, object>> clientReaders)
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
        protected internal abstract DataTable AbstractQuery(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int? commandTimeout, Dictionary<int, Func<object, object>> clientReaders);

        /// <summary>
        /// 使用SQL语句查询，并返回 DataTable
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="clientReaders"></param>
        /// <returns></returns>
        protected internal DataTable Query(IDbConnection connection, string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int? commandTimeout, Dictionary<int, Func<object, object>> clientReaders)
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

            if (commandTimeout.HasValue)
                command.CommandTimeout = commandTimeout.Value;

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
        public int ExecuteNonQuery(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int? commandTimeout)
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
        protected internal abstract int AbstractExecuteNonQuery(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int? commandTimeout);

        /// <summary>
        /// 执行一个SQL语句，并返回受影响的行数
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        protected internal int ExecuteNonQuery(IDbConnection connection, string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int? commandTimeout)
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

            if (commandTimeout.HasValue)
                command.CommandTimeout = commandTimeout.Value;

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
            return ExecuteScalar<T>(sql, keyValues, null, null);
        }

        /// <summary>
        /// 执行SQL语句，并返回查询结果的第一行的第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, IEnumerable<KeyValuePair<string, object>> keyValues)
        {
            return ExecuteScalar(sql, keyValues, null, null);
        }

        /// <summary>
        /// 执行SQL语句，并返回查询结果的第一行的第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int? commandTimeout, Func<object, object> clientReader)
        {
            object obj = ExecuteScalar(sql, keyValues, commandTimeout, clientReader);

            if (obj is T)
                return (T)obj;

            if (obj is null)
                return default(T);

            Type type = typeof(T);

            type = Nullable.GetUnderlyingType(type) ?? type;

            if (type.IsSubclassOf(typeof(Enum)))
            {
                object enumValue = Enum.Parse(type, obj.ToString(), true);

                if (Enum.IsDefined(type, enumValue) == false)
                    throw new InvalidEnumArgumentException($"尝试将【{obj}】转换为{type.FullName}");

                return (T)enumValue;
            }

            return (T)Convert.ChangeType(obj, typeof(T));
        }

        /// <summary>
        /// 执行SQL语句，并返回查询结果的第一行的第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="clientReader"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int? commandTimeout, Func<object, object> clientReader)
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
        protected internal abstract object AbstractExecuteScalar(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int? commandTimeout, Func<object, object> clientReader);

        /// <summary>
        /// 执行SQL语句，并返回查询结果的第一行的第一列
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="clientReader"></param>
        /// <returns></returns>
        protected internal object ExecuteScalar(IDbConnection connection, string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int? commandTimeout, Func<object, object> clientReader)
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

            if (commandTimeout.HasValue)
                command.CommandTimeout = commandTimeout.Value;

            object result = command.ExecuteScalar();

            if (result is DBNull)
                result = null;

            if (clientReader is not null)
                result = clientReader(result);

            return result;
        }
    }
}
