using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class JsonPathExpression : SqlExpression, IValueExpression
    {
        List<PathInfo> _path = new();

        public void Member(MemberInfo member)
        {
            _path.Add(new()
            {
                AccessType = AccessType.Member,
                Value = member
            });
        }

        public void Member(IValueExpression member)
        {
            _path.Add(new()
            {
                AccessType = AccessType.Member,
                Value = member
            });
        }

        public void Index(IValueExpression index)
        {
            _path.Add(new()
            {
                AccessType = AccessType.Index,
                Value = index
            });
        }

        public SqlCommand BuildValue(BuildContext context)
        {
            List<TextInfo> strs = new()
            {
                new()
                {
                    Path = SqlNode.JsonPathRoot,
                    IsConstant = true
                }
            };

            foreach (PathInfo p in _path)
            {
                string text;

                bool isConstant = false;

                if (p.Value is MemberInfo memberInfo)
                {
                    text = memberInfo.Name;

                    isConstant = true;
                }
                else if (p.Value is ConstantExpression constantExpression)
                {
                    if (p.AccessType == AccessType.Member)
                    {
                        if (constantExpression.Value is string str)
                        {
                            text = SqlNode.JsonMemberName(str);

                            isConstant = true;
                        }
                        else
                        {
                            text = constantExpression.BuildValue(context).Text;

                            isConstant = false;
                        }
                    }
                    else
                    {
                        text = constantExpression.Value?.ToString();

                        isConstant = true;
                    }
                }
                else
                {
                    IValueExpression valueExpression = (IValueExpression)p.Value;

                    if (valueExpression is ParameterExpression parameterExpression && parameterExpression.Value is string str)
                    {
                        valueExpression = new ParameterExpression(SqlNode.JsonMemberName(str));
                    }

                    text = valueExpression.BuildValue(context).Text;

                    isConstant = false;
                }

                if (p.AccessType == AccessType.Member)
                {
                    strs.Add(new()
                    {
                        Path = SqlNode.Point,
                        IsConstant = true
                    });

                    strs.Add(new()
                    {
                        Path = text,
                        IsConstant = isConstant
                    });
                }
                else
                {
                    strs.Add(new()
                    {
                        Path = SqlNode.LeftIndexBracket,
                        IsConstant = true
                    });

                    strs.Add(new()
                    {
                        Path = text,
                        IsConstant = isConstant
                    });

                    strs.Add(new()
                    {
                        Path = SqlNode.RightIndexBracket,
                        IsConstant = true
                    });
                }
            }

            List<TextInfo> mergedStrs = new();

            foreach (var str in strs)
            {
                if (mergedStrs.Count == 0)
                {
                    mergedStrs.Add(str);

                    continue;
                }

                TextInfo last = mergedStrs[mergedStrs.Count - 1];

                if (last.IsConstant && str.IsConstant)
                {
                    last.Path += str.Path;
                }
                else
                {
                    mergedStrs.Add(str);
                }
            }

            return new()
            {
                Text = mergedStrs.Count > 1 ? SqlNode.Call(nameof(string.Concat), mergedStrs.Select(s => s.Text).ToArray()) : mergedStrs[0].Text
            };
        }

        class PathInfo
        {
            public AccessType AccessType { get; set; }

            public object Value { get; set; }
        }

        class TextInfo
        {
            public string Path { get; set; }

            public bool IsConstant { get; set; }

            public string Text => IsConstant ? SqlNode.SqlString(Path) : Path;
        }

        enum AccessType
        {
            Member,
            Index
        }
    }
}
