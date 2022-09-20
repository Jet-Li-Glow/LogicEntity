using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Linq.Expressions
{
    public class OriginalTableExpression : TableExpression
    {
        Type _type;

        public OriginalTableExpression(string schema, string name, Type type)
        {
            Schema = schema;

            Name = name;

            _type = type;
        }

        public override Type Type => _type;

        public string Schema { get; private set; }

        public string Name { get; private set; }
    }
}
