using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Linq.Expressions
{
    public class AddOperateExpression : OperateExpression
    {
        public AddOperateExpression(TableExpression source, IEnumerable<object> elements, bool update)
        {
            Source = source;

            Elements = elements;

            Update = update;
        }

        public TableExpression Source { get; private set; }

        public IEnumerable<object> Elements { get; private set; }

        /// <summary>
        /// Whether to update when the key is duplicated
        /// </summary>
        public bool Update { get; private set; }
    }
}
