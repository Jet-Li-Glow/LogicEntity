using LogicEntity.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LogicEntity.Default.MySql
{
    /// <summary>
    /// 数据操作Sql
    /// </summary>
    internal class DataManipulationSql
    {
        SortedList<SelectNodeType, int> _nodes = new();

        public DataManipulationSqlType DataManipulationType { get; set; } = DataManipulationSqlType.Select;

        public Type Type { get; set; }

        //public bool IsWithRecursive { get; set; }

        //public List<string> With { get; } = new();

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

        object _from;

        public object From
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

        object _union;

        public object Union
        {
            get
            {
                return _union;
            }

            set
            {
                _union = value;

                _nodes.Add(SelectNodeType.Union, 0);
            }
        }

        public bool UnionDistinct { get; set; }

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

        public int Max => _nodes.Max(n => (int)n.Key);

        public bool CanAdd(SelectNodeType nodeType)
        {
            if (_nodes.Any() == false)
                return true;

            if (_nodes.ContainsKey(nodeType))
                return false;

            return (int)nodeType > Max;
        }
    }
}
