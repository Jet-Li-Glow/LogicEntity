using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Linq.Expressions
{
    public class AddOrUpdateOperateExpression : OperateExpression
    {
        public AddOrUpdateOperateExpression(TableExpression source, IEnumerable elements)
        {
            if (elements is null)
                throw new ArgumentNullException(nameof(elements));

            Source = source;

            Elements = elements;
        }

        public TableExpression Source { get; private set; }

        public IEnumerable Elements { get; private set; }
    }
}
