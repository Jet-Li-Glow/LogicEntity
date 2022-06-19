using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Grammar;
using LogicEntity.Model;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 表描述
    /// </summary>
    public abstract class TableExpression : ISqlExpression
    {
        /// <summary>
        /// 全名
        /// </summary>
        internal abstract string FullName { get; }

        /// <summary>
        /// 是否有别名
        /// </summary>
        internal abstract bool HasAlias { get; }

        /// <summary>
        /// 别名
        /// </summary>
        internal abstract string Alias { get; }

        /// <summary>
        /// 最后的表名
        /// </summary>
        internal string FinalTableName => HasAlias ? $"`{Alias}`" : FullName;

        /// <summary>
        /// 列
        /// </summary>
        internal abstract IEnumerable<Column> Columns { get; }

        /// <summary>
        /// 所有的列
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Column> All()
        {
            return Columns;
        }

        /// <summary>
        /// Join
        /// </summary>
        /// <param name="tableExpression"></param>
        /// <returns></returns>
        public JoinedTable Join(TableExpression tableExpression)
        {
            return new JoinedTable(this, "Join", tableExpression);
        }

        /// <summary>
        /// Inner Join
        /// </summary>
        /// <param name="tableExpression"></param>
        /// <returns></returns>
        public JoinedTable InnerJoin(TableExpression tableExpression)
        {
            return new JoinedTable(this, "Inner Join", tableExpression);
        }

        /// <summary>
        /// Cross Join
        /// </summary>
        /// <param name="tableExpression"></param>
        /// <returns></returns>
        public JoinedTable CrossJoin(TableExpression tableExpression)
        {
            return new JoinedTable(this, "Cross Join", tableExpression);
        }

        /// <summary>
        /// Left Join
        /// </summary>
        /// <param name="tableExpression"></param>
        /// <returns></returns>
        public JoinedTable LeftJoin(TableExpression tableExpression)
        {
            return new JoinedTable(this, "Left Join", tableExpression);
        }

        /// <summary>
        /// Right Join
        /// </summary>
        /// <param name="tableExpression"></param>
        /// <returns></returns>
        public JoinedTable RightJoin(TableExpression tableExpression)
        {
            return new JoinedTable(this, "Right Join", tableExpression);
        }

        /// <summary>
        /// Natural Join
        /// </summary>
        /// <param name="tableExpression"></param>
        /// <returns></returns>
        public JoinedTable NaturalJoin(TableExpression tableExpression)
        {
            return new JoinedTable(this, "Natural Join", tableExpression);
        }

        /// <summary>
        /// Natural Inner Join
        /// </summary>
        /// <param name="tableExpression"></param>
        /// <returns></returns>
        public JoinedTable NaturalInnerJoin(TableExpression tableExpression)
        {
            return new JoinedTable(this, "Natural Inner Join", tableExpression);
        }

        /// <summary>
        /// Natural Left Join
        /// </summary>
        /// <param name="tableExpression"></param>
        /// <returns></returns>
        public JoinedTable NaturalLeftJoin(TableExpression tableExpression)
        {
            return new JoinedTable(this, "Natural Left Join", tableExpression);
        }

        /// <summary>
        /// Natural Right Join
        /// </summary>
        /// <param name="tableExpression"></param>
        /// <returns></returns>
        public JoinedTable NaturalRightJoin(TableExpression tableExpression)
        {
            return new JoinedTable(this, "Natural Right Join", tableExpression);
        }

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        internal protected abstract (string, IEnumerable<KeyValuePair<string, object>>) Build();

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        (string, IEnumerable<KeyValuePair<string, object>>) ISqlExpression.Build()
        {
            return Build();
        }
    }
}
