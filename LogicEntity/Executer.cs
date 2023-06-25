using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using LogicEntity;
using LogicEntity.Json;
using LogicEntity.Model;

namespace LogicEntity
{
    internal static class Executer
    {
        /// <summary>
        /// 数据库数据类型
        /// </summary>
        static readonly Type[] DbDataTypes = new Type[] {
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
            typeof(object),
            typeof(TimeSpan)
        };

        static readonly MethodInfo _GetName = typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetName));
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
        static readonly ConstructorInfo _InvalidEnumArgumentExceptionConstructor = typeof(InvalidEnumArgumentException).GetConstructor(new[] { typeof(string), typeof(int), typeof(Type) });
        static readonly MethodInfo _DataRowIndexSet = typeof(DataRow).GetProperties().Single(p => { ParameterInfo[] parameterInfos = p.GetIndexParameters(); return parameterInfos.Length == 1 && parameterInfos[0].ParameterType == typeof(int); }).SetMethod;

        /// <summary>
        /// 是否是数据库数据类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static bool IsDbDataType(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;

            return DbDataTypes.Contains(type) || type.IsSubclassOf(typeof(Enum));
        }

        /// <summary>
        /// 获取ClientReader的参数类型
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        /// <exception cref="UnSupportedClientReaderException"></exception>
        static Type GetReaderParameterType(Delegate reader)
        {
            Type type = reader.GetType();

            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();

                if (genericTypeDefinition == typeof(Func<,>))
                    return type.GetGenericArguments()[0];
            }

            throw new UnSupportedClientReaderException(reader);
        }

        /// <summary>
        /// 创建Command
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        static IDbCommand CreateCommand(this IDbConnection connection, IDbTransaction transaction, string commandText, IEnumerable<KeyValuePair<string, object>> parameters, int? commandTimeout)
        {
            IDbCommand dbCommand = connection.CreateCommand();

            if (transaction is not null)
                dbCommand.Transaction = transaction;

            foreach (KeyValuePair<string, object> kv in parameters)
            {
                IDbDataParameter parameter = dbCommand.CreateParameter();

                parameter.ParameterName = kv.Key;

                parameter.Value = kv.Value;

                dbCommand.Parameters.Add(parameter);
            }

            dbCommand.CommandText = commandText;

            dbCommand.CommandType = CommandType.Text;

            if (commandTimeout.HasValue)
                dbCommand.CommandTimeout = commandTimeout.Value;

            return dbCommand;
        }

        /// <summary>
        /// 使用SQL语句查询，并返回所有的结果集
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<IEnumerable> Query(this IDbConnection connection, IDbTransaction transaction, Command command)
        {
            if (command is null)
                throw new ArgumentNullException(nameof(command));

            using (IDbCommand dbCommand = connection.CreateCommand(transaction, command.CommandText, command.Parameters, command.CommandTimeout))
            {
                CommandResult resultInfo = null;

                using (IDataReader reader = dbCommand.ExecuteReader())
                {
                    int i = 0;

                    do
                    {
                        if (i >= command.Results.Count)
                            yield break;

                        resultInfo = command.Results[i];

                        if (resultInfo is null)
                            yield break;

                        yield return ReadTable(reader, resultInfo.Type, resultInfo.Constructors, resultInfo.Readers);

                        i++;
                    } while (reader.NextResult());
                }
            }

            IEnumerable ReadTable(IDataReader reader, Type entityType, Dictionary<string, ConstructorInfo> constructors, Dictionary<int, Delegate> clientReaders)
            {
                clientReaders.TryGetValue(0, out var clientReader);

                Type returnType = clientReader?.Method.ReturnType;

                if (IsDbDataType(entityType) || (reader.FieldCount == 1 && constructors.Count == 0 && returnType == entityType))
                {
                    ParameterExpression record = Expression.Parameter(typeof(IDataRecord));

                    Expression val = GetValueExpression(record, entityType, 0, clientReader);

                    if (val.Type != typeof(object))
                        val = Expression.Convert(val, typeof(object));

                    Func<IDataRecord, object> readCell = Expression.Lambda<Func<IDataRecord, object>>(val, record).Compile();

                    while (reader.Read())
                    {
                        yield return readCell(reader);
                    }
                }
                else
                {
                    Func<IDataRecord, object> readRow = _GetRowReader(reader, entityType, constructors, clientReaders);

                    while (reader.Read())
                    {
                        yield return readRow(reader);
                    }
                }
            }

            Func<IDataRecord, object> _GetRowReader(IDataReader reader, Type entityType, Dictionary<string, ConstructorInfo> constructors, Dictionary<int, Delegate> clientReaders)
            {
                ParameterExpression record = Expression.Parameter(typeof(IDataRecord));

                List<ParameterExpression> bodyVariables = new();

                List<Expression> bodyExpressions = new();

                List<ColumnInfo> columns = new();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    columns.Add(new() { Index = i, ColumnName = JsonPathParser.Parse(reader.GetName(i)) });
                }

                bodyExpressions.Add(_CreateInstance(entityType, columns, 0));

                Expression _CreateInstance(Type instanceType, List<ColumnInfo> instanceColumns, int level)
                {
                    List<string> properties = instanceColumns.Select(c => c.ColumnName[level].MemberName).ToList();

                    ConstructorParameters constructor = null;

                    string path = string.Join(".", instanceColumns.First().ColumnName.Take(level));

                    if (constructors.TryGetValue(path, out ConstructorInfo constructorInfo))
                    {
                        constructor = new()
                        {
                            Constructor = constructorInfo,
                            Parameters = constructorInfo.GetParameters()
                        };
                    }
                    else if (instanceType.IsArray)
                    {
                        constructor = new() { Constructor = instanceType.GetConstructors()[0] };

                        constructor.Parameters = constructor.Constructor.GetParameters();
                    }
                    else
                    {
                        constructor = instanceType.GetConstructors().Select(m => new ConstructorParameters()
                        {
                            Constructor = m,
                            Parameters = m.GetParameters()
                        })
                       .FirstOrDefault(m => m.Parameters.Length == 0 || m.Parameters.All(p => properties.Contains(p.Name)));
                    }

                    if (constructor is null)
                        throw new Exception($"No suitable constructor found for type {entityType.FullName}");

                    if (constructor.Constructor.DeclaringType.IsArray)
                    {
                        List<Expression> initializers = new();

                        Type elementType = constructor.Constructor.DeclaringType.GetElementType();

                        foreach (var item in instanceColumns.GroupBy(c => c.ColumnName[level].MemberName))
                        {
                            if (int.TryParse(item.Key, out int index) == false)
                                throw new InvalidJsonPathException(item.Key);

                            initializers.Add(_GetPropertyValue(elementType, item.ToList(), level));
                        }

                        return Expression.NewArrayInit(elementType, initializers);
                    }
                    else
                    {
                        List<Expression> parameterExpressions = constructor.Parameters.GroupJoin(instanceColumns, a => a.Name, b => b.ColumnName[level].MemberName, (a, bs) =>
                        {
                            List<ColumnInfo> columnInfos = bs.ToList();

                            instanceColumns = instanceColumns.Except(columnInfos).ToList();

                            return _GetPropertyValue(a.ParameterType, columnInfos, level);
                        }).ToList();

                        Expression result = Expression.New(constructor.Constructor, parameterExpressions);

                        var propertyBindings = constructor.Constructor.DeclaringType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty)
                            .GroupJoin(instanceColumns, a => a.Name, b => b.ColumnName[level].MemberName, (a, bs) => new { Property = a, ColumnInfos = bs.ToList() })
                            .Where(s => s.ColumnInfos.Count > 0).ToList();

                        if (propertyBindings.Any())
                        {
                            ParameterExpression propertyInstance = Expression.Parameter(result.Type);

                            bodyVariables.Add(propertyInstance);

                            bodyExpressions.Add(Expression.Assign(propertyInstance, result));

                            result = propertyInstance;

                            propertyBindings.ForEach(s =>
                            {
                                bodyExpressions.Add(
                                    Expression.Call(
                                        propertyInstance,
                                        s.Property.SetMethod,
                                        _GetPropertyValue(s.Property.PropertyType, s.ColumnInfos, level)
                                    )
                                    );
                            });
                        }

                        return result;
                    }
                }

                Expression _GetPropertyValue(Type propertyType, List<ColumnInfo> columnInfos, int level)
                {
                    List<ColumnInfo> nextLevel = columnInfos.Where(c => c.ColumnName.Length > (level + 1)).ToList();

                    if (nextLevel.Any())
                        return _CreateInstance(propertyType, nextLevel, level + 1);

                    int i = columnInfos[0].Index;

                    clientReaders.TryGetValue(i, out var clientReader);

                    return GetValueExpression(record, propertyType, i, clientReader);
                }

                var expression = Expression.Lambda<Func<IDataRecord, object>>(Expression.Block(bodyVariables, bodyExpressions), record);

                return expression.Compile();
            }
        }

        static Expression GetValueExpression(ParameterExpression record, Type propertyType, int i, Delegate clientReader)
        {
            Expression val = null;

            //区分 clientReader
            Type parameterType = null;

            Delegate valueReader = null;

            Delegate clientBytesReader = null;

            Delegate clientCharsReader = null;

            if (clientReader is not null)
            {
                parameterType = GetReaderParameterType(clientReader);

                if (parameterType == typeof(Func<long, byte[], int, int, long>))
                {
                    clientBytesReader = clientReader;
                }
                else if (parameterType == typeof(Func<long, char[], int, int, long>))
                {
                    clientCharsReader = clientReader;
                }
                else
                {
                    valueReader = clientReader;
                }
            }

            //获取值表达式
            if (valueReader is null)
            {
                val = _GetRecordValue(record, propertyType, i, clientBytesReader, clientCharsReader);
            }
            else
            {
                val = _GetRecordValue(record, parameterType, i, null, null);

                val = Expression.Invoke(Expression.Constant(clientReader), val);
            }

            if (val.Type != propertyType)
                val = Expression.Convert(val, propertyType);

            val = Expression.Condition(
                Expression.Call(record, _IsDBNull, Expression.Constant(i)),
                Expression.Default(propertyType),
                val
                );

            return val;

            Expression _GetRecordValue(ParameterExpression record, Type valueType, int i, Delegate clientBytesReader, Delegate clientCharsReader)
            {
                Expression val = null;

                Type underlyingType = Nullable.GetUnderlyingType(valueType) ?? valueType;

                if (clientBytesReader is not null)
                {
                    ParameterExpression fieldOffset = Expression.Parameter(typeof(long));
                    ParameterExpression buffer = Expression.Parameter(typeof(byte[]));
                    ParameterExpression bufferoffset = Expression.Parameter(typeof(int));
                    ParameterExpression length = Expression.Parameter(typeof(int));

                    Expression<Func<long, byte[], int, int, long>> lambdaExpression = Expression.Lambda<Func<long, byte[], int, int, long>>(
                        Expression.Call(record, _GetBytes, Expression.Constant(i), fieldOffset, buffer, bufferoffset, length),
                         fieldOffset, buffer, bufferoffset, length
                        );

                    val = Expression.Invoke(Expression.Constant(clientBytesReader), lambdaExpression);
                }
                else if (clientCharsReader is not null)
                {
                    ParameterExpression fieldOffset = Expression.Parameter(typeof(long));
                    ParameterExpression buffer = Expression.Parameter(typeof(char[]));
                    ParameterExpression bufferoffset = Expression.Parameter(typeof(int));
                    ParameterExpression length = Expression.Parameter(typeof(int));

                    Expression<Func<long, char[], int, int, long>> lambdaExpression = Expression.Lambda<Func<long, char[], int, int, long>>(
                        Expression.Call(record, _GetChars, Expression.Constant(i), fieldOffset, buffer, bufferoffset, length),
                         fieldOffset, buffer, bufferoffset, length
                        );

                    val = Expression.Invoke(Expression.Constant(clientCharsReader), lambdaExpression);
                }
                else if (underlyingType == typeof(bool))
                {
                    val = Expression.Call(record, _GetBoolean, Expression.Constant(i));
                }
                else if (underlyingType == typeof(byte))
                {
                    val = Expression.Call(record, _GetByte, Expression.Constant(i));
                }
                else if (underlyingType == typeof(char))
                {
                    val = Expression.Call(record, _GetChar, Expression.Constant(i));
                }
                else if (underlyingType == typeof(DateTime))
                {
                    val = Expression.Call(record, _GetDateTime, Expression.Constant(i));
                }
                else if (underlyingType == typeof(decimal))
                {
                    val = Expression.Call(record, _GetDecimal, Expression.Constant(i));
                }
                else if (underlyingType == typeof(double))
                {
                    val = Expression.Call(record, _GetDouble, Expression.Constant(i));
                }
                else if (underlyingType == typeof(float))
                {
                    val = Expression.Call(record, _GetFloat, Expression.Constant(i));
                }
                else if (underlyingType == typeof(Guid))
                {
                    val = Expression.Call(record, _GetGuid, Expression.Constant(i));
                }
                else if (underlyingType == typeof(short))
                {
                    val = Expression.Call(record, _GetInt16, Expression.Constant(i));
                }
                else if (underlyingType == typeof(int))
                {
                    val = Expression.Call(record, _GetInt32, Expression.Constant(i));
                }
                else if (underlyingType == typeof(long))
                {
                    val = Expression.Call(record, _GetInt64, Expression.Constant(i));
                }
                else if (underlyingType == typeof(string))
                {
                    val = Expression.Call(record, _GetString, Expression.Constant(i));
                }
                else if (underlyingType == typeof(object))
                {
                    val = Expression.Call(record, _GetValue, Expression.Constant(i));
                }
                else if (underlyingType.IsSubclassOf(typeof(Enum)))
                {
                    ParameterExpression enumValue = Expression.Variable(typeof(object));

                    Expression assign = Expression.Assign(
                        enumValue,
                        Expression.Call(_EnumParse, Expression.Constant(underlyingType), Expression.Call(record, _GetString, Expression.Constant(i)), Expression.Constant(true))
                        );

                    Expression check = Expression.IfThen(
                        Expression.Not(Expression.Call(_EnumIsDefined, Expression.Constant(underlyingType), enumValue)),
                        Expression.Throw(Expression.New(_InvalidEnumArgumentExceptionConstructor,
                                                        Expression.Call(record, _GetName, Expression.Constant(i)),
                                                        Expression.Convert(enumValue, typeof(int)),
                                                        Expression.Constant(underlyingType)
                                                        )
                                        )
                        );

                    val = Expression.Block(new ParameterExpression[] { enumValue }, assign, check, enumValue);
                }
                else
                {
                    val = Expression.Call(_ChangeType, Expression.Call(record, _GetValue, Expression.Constant(i)), Expression.Constant(valueType));
                }

                if (val.Type != valueType)
                    val = Expression.Convert(val, valueType);

                return val;
            }
        }

        /// <summary>
        /// 使用SQL语句查询，并返回 DataTable
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<System.Data.DataTable> QueryDataTable(this IDbConnection connection, IDbTransaction transaction, Command command)
        {
            if (command is null)
                throw new ArgumentNullException(nameof(command));

            using (IDbCommand dbCommand = connection.CreateCommand(transaction, command.CommandText, command.Parameters, command.CommandTimeout))
            {
                CommandResult resultInfo = null;

                using (IDataReader reader = dbCommand.ExecuteReader())
                {
                    int index = 0;

                    do
                    {
                        if (index >= command.Results.Count)
                            yield break;

                        resultInfo = command.Results[index];

                        if (resultInfo is null)
                            yield break;

                        System.Data.DataTable table = new();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            table.Columns.Add(reader.GetName(i),
                                (resultInfo.Readers.TryGetValue(i, out Delegate clientReader) && clientReader is not null) ? clientReader.Method.ReturnType : reader.GetFieldType(i));
                        }

                        Action<DataRow, IDataRecord> fill = _GetRowFiller(reader.FieldCount, resultInfo.Readers);

                        while (reader.Read())
                        {
                            DataRow row = table.NewRow();

                            fill(row, reader);

                            table.Rows.Add(row);
                        }

                        yield return table;

                        index++;
                    }
                    while (reader.NextResult());
                }
            }

            Action<DataRow, IDataRecord> _GetRowFiller(int fieldCount, Dictionary<int, Delegate> clientReaders)
            {
                ParameterExpression record = Expression.Parameter(typeof(IDataRecord));

                ParameterExpression row = Expression.Parameter(typeof(DataRow));

                List<Expression> bodyExpressions = new();

                for (int i = 0; i < fieldCount; i++)
                {
                    clientReaders.TryGetValue(i, out Delegate clientReader);

                    bodyExpressions.Add(
                        Expression.Call(row, _DataRowIndexSet, Expression.Constant(i), GetValueExpression(record, typeof(object), i, clientReader))
                        );
                }

                var lambdaExpression = Expression.Lambda<Action<DataRow, IDataRecord>>(Expression.Block(bodyExpressions), row, record);

                return lambdaExpression.Compile();
            }
        }

        /// <summary>
        /// 执行一个SQL语句，并返回受影响的行数
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static int ExecuteNonQuery(this IDbConnection connection, IDbTransaction transaction, Command command)
        {
            if (command is null)
                throw new ArgumentNullException(nameof(command));

            using (IDbCommand dbCommand = connection.CreateCommand(transaction, command.CommandText, command.Parameters, command.CommandTimeout))
            {
                return dbCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 执行SQL语句，并返回查询结果的第一行的第一列
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        public static object ExecuteScalar(this IDbConnection connection, IDbTransaction transaction, Command command)
        {
            if (command is null)
                throw new ArgumentNullException(nameof(command));

            object result = null;

            using (IDbCommand dbCommand = connection.CreateCommand(transaction, command.CommandText, command.Parameters, command.CommandTimeout))
            {
                result = dbCommand.ExecuteScalar();
            }

            if (result is DBNull)
                result = null;

            if (command.Results.Any() == false)
                return result;

            CommandResult resultInfo = command.Results.First();

            if (resultInfo is null || resultInfo.Type is null)
                return result;

            if (result is null)
                return Activator.CreateInstance(resultInfo.Type);

            if (resultInfo.Readers.TryGetValue(0, out var clientReader) && clientReader is not null)
                return clientReader.DynamicInvoke(ChangeType(result, GetReaderParameterType(clientReader)));

            return ChangeType(result, resultInfo.Type);
        }

        static object ChangeType(object value, Type type)
        {
            if (value.GetType() == type)
                return value;

            Type underlyingType = Nullable.GetUnderlyingType(type) ?? type;

            if (underlyingType.IsSubclassOf(typeof(Enum)))
            {
                value = Enum.Parse(underlyingType, value.ToString(), true);

                if (Enum.IsDefined(underlyingType, value) == false)
                    throw new InvalidCastException($"{value} is not defined in {underlyingType.FullName}");
            }

            value = Convert.ChangeType(value, underlyingType);

            if (type != underlyingType)
                value = Convert.ChangeType(value, type);

            return value;
        }
    }
}
