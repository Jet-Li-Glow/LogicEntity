using LogicEntity.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.Linq.Expressions
{
    internal class ExceptAllTableExpression : ExceptTableExpression
    {
        public ExceptAllTableExpression(TableExpression left, TableExpression right) : base(left, right)
        {

        }
    }
}
