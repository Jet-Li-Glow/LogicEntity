using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Interface;
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
        /// <param name="tableDescription"></param>
        /// <returns></returns>
        public JoinedTable Join(TableExpression tableDescription)
        {
            return new JoinedTable(this, "Join", tableDescription);
        }

        /// <summary>
        /// Inner Join
        /// </summary>
        /// <param name="tableDescription"></param>
        /// <returns></returns>
        public JoinedTable InnerJoin(TableExpression tableDescription)
        {
            return new JoinedTable(this, "Inner Join", tableDescription);
        }

        /// <summary>
        /// Cross Join
        /// </summary>
        /// <param name="tableDescription"></param>
        /// <returns></returns>
        public JoinedTable CrossJoin(TableExpression tableDescription)
        {
            return new JoinedTable(this, "Cross Join", tableDescription);
        }

        /// <summary>
        /// Left Join
        /// </summary>
        /// <param name="tableDescription"></param>
        /// <returns></returns>
        public JoinedTable LeftJoin(TableExpression tableDescription)
        {
            return new JoinedTable(this, "Left Join", tableDescription);
        }

        /// <summary>
        /// Right Join
        /// </summary>
        /// <param name="tableDescription"></param>
        /// <returns></returns>
        public JoinedTable RightJoin(TableExpression tableDescription)
        {
            return new JoinedTable(this, "Right Join", tableDescription);
        }

        /// <summary>
        /// Natural Join
        /// </summary>
        /// <param name="tableDescription"></param>
        /// <returns></returns>
        public JoinedTable NaturalJoin(TableExpression tableDescription)
        {
            return new JoinedTable(this, "Natural Join", tableDescription);
        }

        /// <summary>
        /// Natural Inner Join
        /// </summary>
        /// <param name="tableDescription"></param>
        /// <returns></returns>
        public JoinedTable NaturalInnerJoin(TableExpression tableDescription)
        {
            return new JoinedTable(this, "Natural Inner Join", tableDescription);
        }

        /// <summary>
        /// Natural Left Join
        /// </summary>
        /// <param name="tableDescription"></param>
        /// <returns></returns>
        public JoinedTable NaturalLeftJoin(TableExpression tableDescription)
        {
            return new JoinedTable(this, "Natural Left Join", tableDescription);
        }

        /// <summary>
        /// Natural Right Join
        /// </summary>
        /// <param name="tableDescription"></param>
        /// <returns></returns>
        public JoinedTable NaturalRightJoin(TableExpression tableDescription)
        {
            return new JoinedTable(this, "Natural Right Join", tableDescription);
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
