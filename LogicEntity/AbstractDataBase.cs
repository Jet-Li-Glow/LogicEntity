using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using LogicEntity.Grammar;
using LogicEntity.Model;
using LogicEntity.Operator;

namespace LogicEntity
{
    /// <summary>
    /// 数据库抽象模型
    /// </summary>
    public abstract class AbstractDataBase : AbstractExecuter
    {
        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns></returns>
        protected abstract IDbConnection __GetDbConnection();

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
        protected internal override IEnumerable<T> AbstractQuery<T>(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int? commandTimeout, Dictionary<int, Func<object, object>> clientReaders, Dictionary<int, Func<Func<long, byte[], int, int, long>, object>> clientBytesReaders, Dictionary<int, Func<Func<long, char[], int, int, long>, object>> clientCharsReaders)
        {
            using (IDbConnection connection = __GetDbConnection())
            {
                connection.Open();

                foreach (T t in Query<T>(connection, sql, keyValues, commandTimeout, clientReaders, clientBytesReaders, clientCharsReaders))
                    yield return t;
            }
        }

        /// <summary>
        /// 使用SQL语句查询，并返回 DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="clientReaders"></param>
        /// <returns></returns>
        protected internal override DataTable AbstractQuery(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int? commandTimeout, Dictionary<int, Func<object, object>> clientReaders)
        {
            using (IDbConnection connection = __GetDbConnection())
            {
                connection.Open();

                return Query(connection, sql, keyValues, commandTimeout, clientReaders);
            }
        }

        /// <summary>
        /// 执行一个SQL语句，并返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        protected internal override int AbstractExecuteNonQuery(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int? commandTimeout)
        {
            using (IDbConnection connection = __GetDbConnection())
            {
                connection.Open();

                return ExecuteNonQuery(connection, sql, keyValues, commandTimeout);
            }
        }

        /// <summary>
        /// 执行SQL语句，并返回查询结果的第一行的第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="keyValues"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="clientReader"></param>
        /// <returns></returns>
        protected internal override object AbstractExecuteScalar(string sql, IEnumerable<KeyValuePair<string, object>> keyValues, int? commandTimeout, Func<object, object> clientReader)
        {
            using (IDbConnection connection = __GetDbConnection())
            {
                connection.Open();

                return ExecuteScalar(connection, sql, keyValues, commandTimeout, clientReader);
            }
        }

        /// <summary>
        /// 插入下一条数据，并返回自增主键
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public ulong InsertNext<T>(T row) where T : Table, new()
        {
            Command insert = DBOperator.Insert(row).GetCommand();

            using (IDbConnection connection = __GetDbConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                foreach (KeyValuePair<string, object> kv in insert.Parameters)
                {
                    IDbDataParameter parameter = command.CreateParameter();

                    parameter.ParameterName = kv.Key;

                    parameter.Value = kv.Value;

                    command.Parameters.Add(parameter);
                }

                command.CommandText = insert.CommandText;

                command.CommandType = CommandType.Text;

                command.ExecuteNonQuery();


                command.CommandText = DBOperator.Select(DbFunction.Last_Insert_Id()).GetCommand().CommandText;

                command.Parameters.Clear();

                return (ulong)Convert.ChangeType(command.ExecuteScalar(), TypeCode.UInt64);
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

            using (IDbConnection connection = __GetDbConnection())
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

                        if (cmd.CommandTimeout.HasValue)
                            command.CommandTimeout = cmd.CommandTimeout.Value;

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
        /// 执行事务
        /// </summary>
        /// <param name="action"></param>
        public void ExecuteTransaction(Action<DbTransaction> action)
        {
            if (action is null)
                return;

            using (DbTransaction transaction = new DbTransaction(__GetDbConnection()))
            {
                action(transaction);
            }
        }
    }
}
