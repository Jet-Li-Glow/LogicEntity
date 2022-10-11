using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Linq.Expressions;
using Microsoft.VisualBasic;

namespace LogicEntity.Default.MySql.Linq.Expressions
{
    internal class AddOrUpdateOperateExpression : AddOperateExpression
    {
        public AddOrUpdateOperateExpression(TableExpression source, IEnumerable<object> elements, LambdaExpression updateFactory) : base(source, elements, true)
        {
            if (updateFactory is null)
                throw new ArgumentNullException(nameof(updateFactory));

            UpdateFactory = updateFactory;
        }

        public LambdaExpression UpdateFactory { get; private set; }
    }
}
