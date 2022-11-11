using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Linq.Expressions
{
    public class AddOperateExpression : OperateExpression
    {
        public AddOperateExpression(TableExpression source, IEnumerable<object> elements)
        {
            if (elements is null)
                throw new ArgumentNullException(nameof(elements));

            Source = source;

            Elements = elements;
        }

        public TableExpression Source { get; private set; }

        public IEnumerable<object> Elements { get; private set; }
    }
}
