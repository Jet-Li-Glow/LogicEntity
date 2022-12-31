using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LogicEntity.Linq.Expressions;

namespace LogicEntity
{
    /// <summary>
    /// 事务
    /// </summary>
    public class DbTransaction : IDisposable
    {
        AbstractDataBase _db;

        IDbConnection _connection;

        IDbTransaction _transaction;

        public DbTransaction(AbstractDataBase db, IsolationLevel? il)
        {
            _db = db;

            _connection = _db.CreateDbConnection();

            _connection.Open();

            _transaction = il.HasValue ? _connection.BeginTransaction(il.Value) : _connection.BeginTransaction();

            if (db.ThreadLocalTransaction is not null)
                throw new Exception("Nested transactions are not supported");

            db.ThreadLocalTransaction = this;
        }

        /// <summary>
        /// 隔离级别
        /// </summary>
        IsolationLevel IsolationLevel => _transaction.IsolationLevel;

        /// <summary>
        /// 提交
        /// </summary>
        public void Commit()
        {
            _transaction.Commit();
        }

        /// <summary>
        /// 回滚
        /// </summary>
        public void Rollback()
        {
            _transaction.Rollback();
        }

        //--------------------------------- Query ------------------------------------------------

        /// <summary>
        /// 使用SQL语句查询，并返回 Object 的集合
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IEnumerable<object> Query(Expression expression)
        {
            foreach (IEnumerable<object> result in Query(_db.LinqConvertProvider.Convert(expression)))
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
            return _connection.Query(_transaction, command);
        }

        //--------------------------------- QueryDataTable ---------------------------------------

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
            return _connection.QueryDataTable(_transaction, command);
        }

        //--------------------------------- ExecuteNonQuery --------------------------------------

        /// <summary>
        /// 执行一个命令，并返回受影响的行数
        /// </summary>
        /// <param name="expression">命令表达式</param>
        /// <returns></returns>
        public int ExecuteNonQuery(Expression expression)
        {
            return ExecuteNonQuery(_db.LinqConvertProvider.Convert(expression));
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
            return _connection.ExecuteNonQuery(_transaction, command);
        }

        //--------------------------------- ExecuteScalar ----------------------------------------

        /// <summary>
        /// 执行SQL语句，并返回查询结果的第一行的第一列
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public object ExecuteScalar(Expression expression)
        {
            return ExecuteScalar(_db.LinqConvertProvider.Convert(expression));
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
            return _connection.ExecuteScalar(_transaction, command);
        }

        /// <summary>
        /// 释放连接
        /// </summary>
        public void Dispose()
        {
            _transaction.Dispose();

            _connection.Dispose();

            _db.ThreadLocalTransaction = null;
        }
    }
}
