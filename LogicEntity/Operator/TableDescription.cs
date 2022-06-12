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
    public abstract class TableDescription
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
        public ColumnCollection All()
        {
            return new ColumnCollection(Columns);
        }

        /// <summary>
        /// Join
        /// </summary>
        /// <param name="tableDescription"></param>
        /// <returns></returns>
        public JoinedTable Join(TableDescription tableDescription)
        {
            return new JoinedTable(this, "Join", tableDescription);
        }

        /// <summary>
        /// Inner Join
        /// </summary>
        /// <param name="tableDescription"></param>
        /// <returns></returns>
        public JoinedTable InnerJoin(TableDescription tableDescription)
        {
            return new JoinedTable(this, "Inner Join", tableDescription);
        }

        /// <summary>
        /// Cross Join
        /// </summary>
        /// <param name="tableDescription"></param>
        /// <returns></returns>
        public JoinedTable CrossJoin(TableDescription tableDescription)
        {
            return new JoinedTable(this, "Cross Join", tableDescription);
        }

        /// <summary>
        /// Left Join
        /// </summary>
        /// <param name="tableDescription"></param>
        /// <returns></returns>
        public JoinedTable LeftJoin(TableDescription tableDescription)
        {
            return new JoinedTable(this, "Left Join", tableDescription);
        }

        /// <summary>
        /// Right Join
        /// </summary>
        /// <param name="tableDescription"></param>
        /// <returns></returns>
        public JoinedTable RightJoin(TableDescription tableDescription)
        {
            return new JoinedTable(this, "Right Join", tableDescription);
        }

        /// <summary>
        /// Natural Join
        /// </summary>
        /// <param name="tableDescription"></param>
        /// <returns></returns>
        public JoinedTable NaturalJoin(TableDescription tableDescription)
        {
            return new JoinedTable(this, "Natural Join", tableDescription);
        }

        /// <summary>
        /// Natural Inner Join
        /// </summary>
        /// <param name="tableDescription"></param>
        /// <returns></returns>
        public JoinedTable NaturalInnerJoin(TableDescription tableDescription)
        {
            return new JoinedTable(this, "Natural Inner Join", tableDescription);
        }

        /// <summary>
        /// Natural Left Join
        /// </summary>
        /// <param name="tableDescription"></param>
        /// <returns></returns>
        public JoinedTable NaturalLeftJoin(TableDescription tableDescription)
        {
            return new JoinedTable(this, "Natural Left Join", tableDescription);
        }

        /// <summary>
        /// Natural Right Join
        /// </summary>
        /// <param name="tableDescription"></param>
        /// <returns></returns>
        public JoinedTable NaturalRightJoin(TableDescription tableDescription)
        {
            return new JoinedTable(this, "Natural Right Join", tableDescription);
        }

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        internal abstract (string, IEnumerable<KeyValuePair<string, object>>) Build();
    }
}
