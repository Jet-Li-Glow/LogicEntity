using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Interface;

namespace LogicEntity.Model
{
    /// <summary>
    /// 事务
    /// </summary>
    public class DbTransaction : AbstractExecuter, IDisposable
    {
        AbstractDataBase _dataBase;

        IDbConnection _connection;

        IDbTransaction _transaction;

        public DbTransaction(AbstractDataBase dataBase)
        {
            _dataBase = dataBase;

            _connection = _dataBase.GetDbConnection();

            _connection.Open();

            _transaction = _connection.BeginTransaction();
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

        /// <summary>
        /// 使用SQL语句查询，并返回 T 类型的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        protected internal override IEnumerable<T> AbstractQuery<T>(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int commandTimeout)
        {
            return Query<T>(_connection, sql, keyValues, commandTimeout);
        }

        /// <summary>
        /// 使用SQL语句查询，并返回 DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        protected internal override DataTable AbstractQuery(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int commandTimeout)
        {
            return Query(_connection, sql, keyValues, commandTimeout);
        }

        /// <summary>
        /// 执行一个SQL语句，并返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        protected internal override int AbstractExecuteNonQuery(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int commandTimeout)
        {
            return ExecuteNonQuery(_connection, sql, keyValues, commandTimeout);
        }

        /// <summary>
        /// 执行SQL语句，并返回查询结果的第一行的第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        protected internal override object AbstractExecuteScalar(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int commandTimeout)
        {
            return ExecuteScalar(_connection, sql, keyValues, commandTimeout);
        }

        /// <summary>
        /// 释放连接
        /// </summary>
        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
