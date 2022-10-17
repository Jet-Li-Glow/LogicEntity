using LogicEntity.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LogicEntity.Default.MySql
{
    internal class UnionedSql : IDataManipulationSql
    {
        SortedList<SelectNodeType, int> _nodes = new() { { SelectNodeType.From, 0 } };

        public IDataManipulationSql SetSqlType(DataManipulationSqlType sqlType)
        {
            return new DataManipulationSql() { From = this, SqlType = sqlType };
        }

        public Type Type { get; set; }

        public bool IsCTE { get; set; } = false;

        public int? Timeout { get; set; }

        public IDataManipulationSql Left { get; set; }

        public string BinaryTableOperate { get; set; }

        public object Right { get; set; }

        List<OrderedTableExpression> _orderBy;

        public List<OrderedTableExpression> OrderBy
        {
            get
            {
                return _orderBy;
            }

            set
            {
                _orderBy = value;

                _nodes.Add(SelectNodeType.OrderBy, 0);
            }
        }

        SkipTaked _limit;

        public SkipTaked Limit
        {
            get
            {
                return _limit;
            }

            set
            {
                _limit = value;

                _nodes.Add(SelectNodeType.Limit, 0);
            }
        }

        public bool CanSet(SelectNodeType nodeType)
        {
            if (nodeType != SelectNodeType.OrderBy && nodeType != SelectNodeType.Limit)
                return false;

            return (int)nodeType > _nodes.Max(n => (int)n.Key);
        }

        public IDataManipulationSql SetDistinct(bool distinct)
        {
            return new DataManipulationSql() { From = this, Distinct = distinct };
        }

        public IDataManipulationSql SetHasIndex(bool hasIndex)
        {
            return new DataManipulationSql() { From = this, HasIndex = hasIndex };
        }

        public IDataManipulationSql SetDelete(List<string> tables)
        {
            //此处不应被调用
            throw new NotImplementedException();
        }

        public IDataManipulationSql SetSet(LambdaExpression[] assignments)
        {
            //此处不应被调用
            throw new NotImplementedException();
        }

        public IDataManipulationSql SetWhere(List<LambdaExpression> where)
        {
            return new DataManipulationSql() { From = this, Where = where };
        }

        public IDataManipulationSql SetGroupBy(LambdaExpression groupBy)
        {
            return new DataManipulationSql() { From = this, GroupBy = groupBy };
        }

        public IDataManipulationSql SetSelect(object select)
        {
            return new DataManipulationSql() { From = this, Select = select };
        }

        public IDataManipulationSql SetHaving(List<LambdaExpression> having)
        {
            return new DataManipulationSql() { From = this, Having = having };
        }

        public IDataManipulationSql SetOrderBy(List<OrderedTableExpression> orderBy)
        {
            if (CanSet(SelectNodeType.OrderBy))
            {
                OrderBy = orderBy;

                return this;
            }

            return new DataManipulationSql() { From = this, OrderBy = orderBy };
        }

        public IDataManipulationSql SetLimit(SkipTaked limit)
        {
            if (CanSet(SelectNodeType.Limit))
            {
                Limit = limit;

                return this;
            }

            return new DataManipulationSql() { From = this, Limit = limit };
        }

        public static implicit operator List<DataManipulationSqlJoinedInfo>(UnionedSql sql)
        {
            return new()
            {
                new()
                {
                    Table = sql
                }
            };
        }
    }
}
