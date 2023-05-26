using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class CastExpression : SqlExpression, IValueExpression
    {
        public CastExpression(IValueExpression operand, DataType dataType)
        {
            Operand = operand;

            DataType = dataType;
        }

        public IValueExpression Operand { get; private set; }

        public DataType DataType { get; private set; }

        public SqlCommand BuildValue(BuildContext context)
        {
            return new()
            {
                Text = SqlNode.Cast(Operand.BuildValue(context).Text, DataType)
            };
        }
    }
}
