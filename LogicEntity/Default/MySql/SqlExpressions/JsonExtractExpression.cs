using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class JsonExtractExpression : SqlExpression, IValueExpression
    {
        public JsonExtractExpression(IValueExpression jsonDocument)
        {
            JsonDocument = jsonDocument;
        }

        public IValueExpression JsonDocument { get; private set; }

        public JsonPathExpression Path { get; } = new();

        public void Member(MemberInfo member)
        {
            Path.Member(member);
        }

        public void Member(IValueExpression member)
        {
            Path.Member(member);
        }

        public void Index(IValueExpression index)
        {
            Path.Index(index);
        }

        public SqlCommand BuildValue(BuildContext context)
        {
            return new()
            {
                Text = SqlNode.Call(
                    "Json_UnQuote",
                    SqlNode.Call(
                        "Json_Extract",
                        JsonDocument.BuildValue(context).Text,
                        Path.BuildValue(context).Text
                    )
                )
            };
        }
    }
}
