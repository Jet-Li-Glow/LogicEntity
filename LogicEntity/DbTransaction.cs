using System;
using System.Collections;
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
    public class DbTransaction
    {
        IDbTransaction _transaction;

        public DbTransaction(IDbTransaction transaction)
        {
            _transaction = transaction;
        }

        /// <summary>
        /// 
        /// </summary>
        internal IDbTransaction DbTransactionProvider => _transaction;

        /// <summary>
        /// 隔离级别
        /// </summary>
        public IsolationLevel IsolationLevel => _transaction.IsolationLevel;

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
    }
}
