using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using LogicEntity.Collections.Generic;
using LogicEntity.Linq.Expressions;

namespace LogicEntity
{
    /// <summary>
    /// 数据库抽象模型
    /// </summary>
    public abstract class AbstractDataBase
    {
        internal ConcurrentDictionary<Thread, DbTransaction> Transactions { get; } = new();

        /// <summary>
        /// 阻止程序集外部使用
        /// </summary>
        internal ILinqConvertProvider LinqConvertProvider { get; private set; }

        /// <summary>
        /// 构造
        /// </summary>
        public AbstractDataBase()
        {
            LinqConvertProvider = GetLinqConvertProvider();

            foreach (PropertyInfo property in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(t => t.PropertyType.IsGenericType && t.PropertyType.GetGenericTypeDefinition() == typeof(ITable<>)))
            {
                if (property.CanWrite == false)
                    continue;

                Type entityType = property.PropertyType.GetGenericArguments()[0];

                TableAttribute tableAttribute = entityType.GetCustomAttribute<TableAttribute>(true);

                Type type = typeof(DataTableImpl<>).MakeGenericType(new Type[] { entityType });

                object table = type.GetConstructor(new Type[] { typeof(AbstractDataBase), typeof(TableExpression) })
                    .Invoke(new object[] { this, new OriginalTableExpression(tableAttribute?.Schema, tableAttribute?.Name ?? property.Name, entityType) });

                property.SetValue(this, table);
            }
        }

        /// <summary>
        /// 获取Linq提供程序
        /// </summary>
        /// <returns></returns>
        protected abstract ILinqConvertProvider GetLinqConvertProvider();

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        internal protected abstract IDbConnection CreateDbConnection();

        //--------------------------------- Query ------------------------------------------------

        /// <summary>
        /// 使用 Expression 查询，并返回 Object 的集合
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IEnumerable<object> Query(Expression expression)
        {
            foreach (IEnumerable<object> result in Query(LinqConvertProvider.Convert(expression)))
            {
                foreach (object obj in result)
                {
                    yield return obj;
                }

                yield break;
            }
        }

        /// <summary>
        /// 使用SQL语句查询，并返回 T 类型的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string commandText, params object[] parameters)
        {
            foreach (IEnumerable<object> result in Query(new Command(typeof(T), commandText, parameters)))
            {
                foreach (object obj in result)
                {
                    yield return (T)obj;
                }

                yield break;
            }
        }

        /// <summary>
        /// 使用SQL语句查询，并返回 T 类型的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string commandText, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            return Query<T>(commandText, parameters, null);
        }

        /// <summary>
        /// 使用SQL语句查询，并返回 T 类型的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string commandText, IEnumerable<KeyValuePair<string, object>> parameters, int? commandTimeout)
        {
            foreach (IEnumerable<object> result in Query(new Command(typeof(T), commandText, parameters, commandTimeout)))
            {
                foreach (object obj in result)
                {
                    yield return (T)obj;
                }

                yield break;
            }
        }

        /// <summary>
        /// 使用SQL语句查询，并返回所有的结果集
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public IEnumerable<IEnumerable<object>> Query(Command command)
        {
            if (Transactions.TryGetValue(Thread.CurrentThread, out DbTransaction transaction))
            {
                foreach (IEnumerable<object> result in transaction.Query(command))
                    yield return result;

                yield break;
            }

            using (IDbConnection connection = CreateDbConnection())
            {
                connection.Open();

                foreach (IEnumerable<object> result in connection.Query(null, command))
                    yield return result;
            }
        }

        //--------------------------------- QueryDataTable ---------------------------------------

        /// <summary>
        /// 使用 Expression 查询，并返回 DataTable
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public System.Data.DataTable QueryDataTable(Expression expression)
        {
            return QueryDataTable(LinqConvertProvider.Convert(expression)).First();
        }

        /// <summary>
        /// 使用SQL语句查询，并返回 DataTable
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public System.Data.DataTable QueryDataTable(string commandText, params object[] parameters)
        {
            return QueryDataTable(new Command(null, commandText, parameters)).First();
        }

        /// <summary>
        /// 使用SQL语句查询，并返回 DataTable
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public System.Data.DataTable QueryDataTable(string commandText, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            return QueryDataTable(commandText, parameters, null);
        }

        /// <summary>
        /// 使用SQL语句查询，并返回 DataTable
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public System.Data.DataTable QueryDataTable(string commandText, IEnumerable<KeyValuePair<string, object>> parameters, int? commandTimeout)
        {
            return QueryDataTable(new Command(null, commandText, parameters, commandTimeout)).First();
        }

        /// <summary>
        /// 使用SQL语句查询，并返回 DataTable
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public IEnumerable<System.Data.DataTable> QueryDataTable(Command command)
        {
            if (Transactions.TryGetValue(Thread.CurrentThread, out DbTransaction transaction))
            {
                foreach (System.Data.DataTable result in transaction.QueryDataTable(command))
                    yield return result;

                yield break;
            }

            using (IDbConnection connection = CreateDbConnection())
            {
                connection.Open();

                foreach (System.Data.DataTable result in connection.QueryDataTable(null, command))
                    yield return result;
            }
        }

        //--------------------------------- ExecuteNonQuery --------------------------------------

        /// <summary>
        /// 执行一个命令，并返回受影响的行数
        /// </summary>
        /// <param name="expression">命令表达式</param>
        /// <returns></returns>
        public int ExecuteNonQuery(Expression expression)
        {
            return ExecuteNonQuery(LinqConvertProvider.Convert(expression));
        }

        /// <summary>
        /// 执行一个SQL语句，并返回受影响的行数
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandText, params object[] parameters)
        {
            return ExecuteNonQuery(new Command(null, commandText, parameters));
        }

        /// <summary>
        /// 执行一个SQL语句，并返回受影响的行数
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandText, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            return ExecuteNonQuery(commandText, parameters, null);
        }

        /// <summary>
        /// 执行一个SQL语句，并返回受影响的行数
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandText, IEnumerable<KeyValuePair<string, object>> parameters, int? commandTimeout)
        {
            return ExecuteNonQuery(new Command(null, commandText, parameters, commandTimeout));
        }

        /// <summary>
        /// 执行一个SQL语句，并返回受影响的行数
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(Command command)
        {
            if (Transactions.TryGetValue(Thread.CurrentThread, out DbTransaction transaction))
                return transaction.ExecuteNonQuery(command);

            using (IDbConnection connection = CreateDbConnection())
            {
                connection.Open();

                return connection.ExecuteNonQuery(null, command);
            }
        }

        //--------------------------------- ExecuteScalar ----------------------------------------

        /// <summary>
        /// 执行SQL语句，并返回查询结果的第一行的第一列
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public object ExecuteScalar(Expression expression)
        {
            return ExecuteScalar(LinqConvertProvider.Convert(expression));
        }

        /// <summary>
        /// 执行SQL语句，并返回查询结果的第一行的第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string commandText, params object[] parameters)
        {
            return (T)ExecuteScalar(new Command(typeof(T), commandText, parameters));
        }

        /// <summary>
        /// 执行SQL语句，并返回查询结果的第一行的第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string commandText, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            return ExecuteScalar<T>(commandText, parameters);
        }

        /// <summary>
        /// 执行SQL语句，并返回查询结果的第一行的第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string commandText, IEnumerable<KeyValuePair<string, object>> parameters, int? commandTimeout)
        {
            return (T)ExecuteScalar(new Command(typeof(T), commandText, parameters, commandTimeout));
        }

        /// <summary>
        /// 执行SQL语句，并返回查询结果的第一行的第一列
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public object ExecuteScalar(Command command)
        {
            if (Transactions.TryGetValue(Thread.CurrentThread, out DbTransaction transaction))
                return transaction.ExecuteScalar(command);

            using (IDbConnection connection = CreateDbConnection())
            {
                connection.Open();

                return connection.ExecuteScalar(null, command);
            }
        }

        //--------------------------------- Transaction ------------------------------------------

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="commands">命令</param>
        /// <returns></returns>
        public bool TryExecuteTransaction(params Command[] commands)
        {
            if (commands is null)
                throw new ArgumentNullException(nameof(commands));

            return TryExecuteTransaction(commands, out int _, out Exception _);
        }

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="commands">命令</param>
        /// <param name="affected">受影响的行数</param>
        /// <param name="exception">事务执行失败时的异常</param>
        /// <returns>事务是否执行成功</returns>
        public bool TryExecuteTransaction(IEnumerable<Command> commands, out int affected, out Exception exception)
        {
            if (commands is null)
                throw new ArgumentNullException(nameof(commands));

            bool success = false;

            int transactionAffected = 0;

            TryExecuteTransaction(() =>
            {
                foreach (Command command in commands)
                {
                    transactionAffected += ExecuteNonQuery(command);
                }
            }, out exception);


            affected = transactionAffected;

            return success;
        }

        public bool TryExecuteTransaction(Action action)
        {
            return TryExecuteTransaction(action, out Exception _);
        }

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="changes">更改</param>
        /// <returns></returns>
        public bool TryExecuteTransaction(Action action, out Exception exception)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));

            bool success = false;

            Exception transactionException = null;

            ExecuteTransaction(transaction =>
            {
                try
                {
                    action();

                    transaction.Commit();

                    success = true;
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch
                    {
                    }

                    transactionException = ex;

                    success = false;
                }
            });

            exception = transactionException;

            return success;
        }

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="action"></param>
        /// <param name="il"></param>
        public void ExecuteTransaction(Action<DbTransaction> action, IsolationLevel? il = null)
        {
            if (action is null)
                return;

            using (DbTransaction transaction = new DbTransaction(this, il))
            {
                action(transaction);
            }
        }
    }
}
