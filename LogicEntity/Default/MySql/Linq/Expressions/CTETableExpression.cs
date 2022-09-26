using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Linq.Expressions;

namespace LogicEntity.Default.MySql.Linq.Expressions
{
    internal class CTETableExpression : TableExpression
    {
        Type _type;

        public CTETableExpression(string alias, Type type)
        {
            Alias = alias;

            _type = type;
        }

        public override Type Type => _type;

        public string Alias { get; private set; }
    }
}
