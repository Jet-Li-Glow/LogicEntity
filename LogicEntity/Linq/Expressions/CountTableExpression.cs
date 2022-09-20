using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Linq.Expressions
{
    public class CountTableExpression : TableExpression
    {
        Type _type;

        public CountTableExpression(TableExpression source, Type type)
        {
            Source = source;

            _type = type;
        }

        public override Type Type => _type;

        public TableExpression Source { get; private set; }
    }
}
