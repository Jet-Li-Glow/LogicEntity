using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 关联表
    /// </summary>
    public class JoinedTable : TableExpression
    {
        TableExpression _left;

        string _join;

        TableExpression _right;

        IValueExpression _joinSpecification;

        IEnumerable<Column> _columns;

        internal JoinedTable(TableExpression left, string join, TableExpression right)
        {
            _left = left;

            _join = join;

            _right = right;

            _columns = left.Columns.Concat(right.Columns);
        }

        internal override string FullName => string.Empty;

        internal override bool HasAlias => false;

        internal override string Alias => string.Empty;

        internal override IEnumerable<Column> Columns => _columns;

        public TableExpression On(IValueExpression joinSpecification)
        {
            _joinSpecification = joinSpecification;

            return this;
        }

        internal protected override (string, IEnumerable<KeyValuePair<string, object>>) Build()
        {
            List<KeyValuePair<string, object>> parameters = new();

            (var leftStr, var leftPs) = _left?.Build() ?? (null, null);

            if (leftPs is not null)
                parameters.AddRange(leftPs);

            (var rightStr, var rightPs) = _right?.Build() ?? (null, null);

            if (rightPs is not null)
                parameters.AddRange(rightPs);

            string onStr = string.Empty;

            if (_joinSpecification is not null)
            {
                onStr += "On";

                (var cmd, var ps) = _joinSpecification.Build();

                onStr += " " + cmd;

                if (ps is not null)
                    parameters.AddRange(ps);
            }

            return ($"{leftStr}\n  {_join} {rightStr} {onStr}", parameters);
        }
    }
}
