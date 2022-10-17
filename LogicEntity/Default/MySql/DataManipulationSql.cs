using LogicEntity.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LogicEntity.Default.MySql
{
    /// <summary>
    /// 数据操作Sql
    /// </summary>
    internal class DataManipulationSql : IDataManipulationSql
    {
        SortedList<SelectNodeType, int> _nodes = new();

        public DataManipulationSqlType SqlType { get; set; } = DataManipulationSqlType.Select;

        public IDataManipulationSql SetSqlType(DataManipulationSqlType sqlType)
        {
            SqlType = sqlType;

            return this;
        }

        public Type Type { get; set; }

        public bool IsCTE { get; set; } = false;

        public int? Timeout { get; set; }

        object _select;

        public object Select
        {
            get
            {
                return _select;
            }

            set
            {
                _select = value;

                _nodes.Add(SelectNodeType.Select, 0);
            }
        }

        public bool Distinct { get; set; } = false;

        public bool HasIndex { get; set; } = false;

        public List<string> Delete { get; set; }

        List<DataManipulationSqlJoinedInfo> _from;

        public List<DataManipulationSqlJoinedInfo> From
        {
            get
            {
                return _from;
            }

            set
            {
                _from = value;

                _nodes.Add(SelectNodeType.From, 0);
            }
        }

        public LambdaExpression[] Set { get; set; }

        List<LambdaExpression> _where;

        public List<LambdaExpression> Where
        {
            get
            {
                return _where;
            }

            set
            {
                _where = value;

                _nodes.Add(SelectNodeType.Where, 0);
            }
        }

        LambdaExpression _groupBy;

        public LambdaExpression GroupBy
        {
            get
            {
                return _groupBy;
            }

            set
            {
                _groupBy = value;

                _nodes.Add(SelectNodeType.GroupBy, 0);
            }
        }

        List<LambdaExpression> _having;

        public List<LambdaExpression> Having
        {
            get
            {
                return _having;
            }

            set
            {
                _having = value;

                _nodes.Add(SelectNodeType.Having, 0);
            }
        }

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
            if (_nodes.Any() == false)
                return true;

            if (_nodes.ContainsKey(nodeType))
                return false;

            return (int)nodeType > _nodes.Max(n => (int)n.Key);
        }

        public IDataManipulationSql SetDistinct(bool distinct)
        {
            Distinct = distinct;

            return this;
        }

        public IDataManipulationSql SetHasIndex(bool hasIndex)
        {
            HasIndex = hasIndex;

            return this;
        }

        public IDataManipulationSql SetDelete(List<string> tables)
        {
            Delete = tables;

            return this;
        }
        public IDataManipulationSql SetSet(LambdaExpression[] assignments)
        {
            Set = assignments;

            return this;
        }

        public IDataManipulationSql SetWhere(List<LambdaExpression> where)
        {
            DataManipulationSql sql = this;

            if (sql.CanSet(SelectNodeType.Where) == false)
            {
                sql = new() { From = sql };
            }

            sql.Where = where;

            return sql;
        }

        public IDataManipulationSql SetGroupBy(LambdaExpression groupBy)
        {
            DataManipulationSql sql = this;

            if (sql.CanSet(SelectNodeType.GroupBy) == false)
            {
                sql = new() { From = sql };
            }

            sql.GroupBy = groupBy;

            return sql;
        }

        public IDataManipulationSql SetSelect(object select)
        {
            DataManipulationSql sql = this;

            if (sql.CanSet(SelectNodeType.Select) == false)
            {
                sql = new() { From = sql };
            }

            sql.Select = select;

            return sql;
        }

        public IDataManipulationSql SetHaving(List<LambdaExpression> having)
        {
            DataManipulationSql sql = this;

            if (sql.CanSet(SelectNodeType.Having) == false)
            {
                sql = new() { From = sql };
            }

            sql.Having = having;

            return sql;
        }

        public IDataManipulationSql SetOrderBy(List<OrderedTableExpression> orderBy)
        {
            DataManipulationSql sql = this;

            if (sql.CanSet(SelectNodeType.OrderBy) == false)
            {
                sql = new() { From = sql };
            }

            sql.OrderBy = orderBy;

            return sql;
        }

        public IDataManipulationSql SetLimit(SkipTaked limit)
        {
            DataManipulationSql sql = this;

            if (sql.CanSet(SelectNodeType.Limit) == false)
            {
                sql = new() { From = sql };
            }

            sql.Limit = limit;

            return sql;
        }

        public static implicit operator List<DataManipulationSqlJoinedInfo>(DataManipulationSql sql)
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
