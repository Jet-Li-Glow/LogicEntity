using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class AssignmentExpression : SqlExpression, IValueExpression
    {
        public AssignmentExpression(IValueExpression left, IValueExpression right)
        {
            Left = left;

            Right = right;
        }

        public IValueExpression Left { get; private set; }

        public IValueExpression Right { get; private set; }

        public SqlCommand BuildValue(BuildContext context)
        {
            if (Left is JsonExtractExpression jsonExtractExpression)
            {
                return new()
                {
                    Text = SqlNode.Assign(
                        jsonExtractExpression.JsonDocument.BuildValue(context).Text,
                        new MethodCallExpression("Json_Set", jsonExtractExpression.JsonDocument, jsonExtractExpression.Path, Right).BuildValue(context).Text
                        )
                };
            }

            return new()
            {
                Text = SqlNode.Assign(Left.BuildValue(context).Text, Right.BuildValue(context).Text)
            };
        }
    }
}
