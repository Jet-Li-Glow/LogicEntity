using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Grammar;
using LogicEntity.Model;
using LogicEntity.Tool;

namespace LogicEntity.Operator
{
    /// <summary>
    /// Sql表达式
    /// </summary>
    internal class SqlExpression : ISqlExpression
    {
        Func<(string, IEnumerable<KeyValuePair<string, object>>)> _build;

        public SqlExpression(string command, params object[] objects)
        {
            _build = () =>
            {
                List<KeyValuePair<string, object>> parameters = new();

                List<string> formatArgs = new();

                if (objects is not null)
                {
                    foreach (object obj in objects)
                    {
                        if (obj is ISqlExpression sqlExpression)
                        {
                            (var cmd, var ps) = sqlExpression.Build();

                            formatArgs.Add(cmd);

                            if (ps is not null)
                                parameters.AddRange(ps);

                            continue;
                        }

                        string key = ToolService.UniqueName();

                        formatArgs.Add(key);

                        parameters.Add(KeyValuePair.Create(key, obj));
                    }
                }

                return (string.Format(command, formatArgs.ToArray()), parameters);
            };
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="commonTableExpression"></param>
        public SqlExpression(CommonTableExpression commonTableExpression)
        {
            _build = commonTableExpression.BuildDefinition;
        }

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        (string, IEnumerable<KeyValuePair<string, object>>) ISqlExpression.Build()
        {
            return _build?.Invoke() ?? (null, null);
        }

        public static SqlExpression With(bool isRecursive, params CommonTableExpression[] commonTableExpressions)
        {
            return new SqlExpression("With " + (isRecursive ? "Recursive" : string.Empty) + "\n  {0}\n",
                __Join(",\n  ", commonTableExpressions.Select(c => new SqlExpression(c))));
        }

        public static SqlExpression Select()
        {
            return new SqlExpression(nameof(Select));
        }

        public static SqlExpression Distinct()
        {
            return new SqlExpression(nameof(Distinct));
        }

        public static SqlExpression From(params TableExpression[] tables)
        {
            return new SqlExpression("From\n  {0}", __Join(",\n  ", tables));
        }

        public static SqlExpression Where(IValueExpression condition)
        {
            return new SqlExpression("Where {0}", condition);
        }

        public static SqlExpression GroupBy(params IValueExpression[] columns)
        {
            return new SqlExpression("Group By\n  {0}", __Join(",\n  ", columns));
        }

        public static SqlExpression Having(IValueExpression condition)
        {
            return new SqlExpression("Having\n  {0}", condition);
        }

        public static SqlExpression Window(params Window[] windows)
        {
            return new SqlExpression("Window\n{0}", __Join(",\n", windows.Select(window => new SqlExpression("  " + window.Alias.PadLeft(4) + " As ({0})", window))));
        }

        public static SqlExpression Union(ISelector selector)
        {
            return new SqlExpression("\n\nUnion\n\n\n{0}\n\n", selector);
        }

        public static SqlExpression UnionAll(ISelector selector)
        {
            return new SqlExpression("\n\nUnion All\n\n\n{0}\n\n", selector);
        }

        public static SqlExpression OrderBy(IValueExpression valueExpression)
        {
            return new SqlExpression("Order By\n  {0} Asc", valueExpression);
        }

        public static SqlExpression OrderByDescending(IValueExpression valueExpression)
        {
            return new SqlExpression("Order By\n  {0} Desc", valueExpression);
        }

        public static SqlExpression ThenBy(IValueExpression valueExpression)
        {
            return new SqlExpression("  , {0} Asc", valueExpression);
        }

        public static SqlExpression ThenByDescending(IValueExpression valueExpression)
        {
            return new SqlExpression("  , {0} Desc", valueExpression);
        }

        public static SqlExpression Limit(ulong limit)
        {
            return new SqlExpression($"Limit {limit}");
        }

        public static SqlExpression Limit(ulong offset, ulong limit)
        {
            return new SqlExpression($"Limit {offset}, {limit}");
        }

        public static SqlExpression ForUpdate()
        {
            return new SqlExpression("For Update");
        }

        public static SqlExpression Update(TableExpression table)
        {
            return new SqlExpression("Update\n  {0}", table);
        }

        public static SqlExpression Delete(params Table[] tables)
        {
            return new SqlExpression($"Delete {string.Join(", ", tables.Select(table => (table as TableExpression).FinalTableName))}");
        }

        public static SqlExpression Insert()
        {
            return new SqlExpression(nameof(Insert));
        }

        public static SqlExpression Replace()
        {
            return new SqlExpression(nameof(Replace));
        }

        public static SqlExpression Ignore()
        {
            return new SqlExpression(nameof(Ignore));
        }

        public static SqlExpression Into()
        {
            return new SqlExpression(nameof(Into));
        }

        public static SqlExpression PartitionBy(params IValueExpression[] valueExpressions)
        {
            return new SqlExpression("Partition By {0}", __Join(", ", valueExpressions));
        }

        public static SqlExpression Rows()
        {
            return new SqlExpression(nameof(Rows));
        }

        public static SqlExpression Range()
        {
            return new SqlExpression(nameof(Range));
        }

        public static SqlExpression Between()
        {
            return new SqlExpression(nameof(Between));
        }

        public static SqlExpression And()
        {
            return new SqlExpression(nameof(And));
        }

        public static SqlExpression CurrentRow()
        {
            return new SqlExpression("Current Row");
        }

        public static SqlExpression UnboundedPreceding()
        {
            return new SqlExpression("Unbounded Preceding");
        }

        public static SqlExpression UnboundedFollowing()
        {
            return new SqlExpression("Unbounded Following");
        }

        public static SqlExpression Preceding(IValueExpression valueExpression)
        {
            return new SqlExpression("{0} Preceding", valueExpression);
        }

        public static SqlExpression Preceding(int count)
        {
            return new SqlExpression($"{count} Preceding");
        }

        public static SqlExpression Following(IValueExpression valueExpression)
        {
            return new SqlExpression("{0} Following", valueExpression);
        }

        public static SqlExpression Following(int count)
        {
            return new SqlExpression($"{count} Following");
        }

        public static SqlExpression __Join(string separator, IEnumerable<ISqlExpression> sqlExpressions)
        {
            return new SqlExpression(string.Join(separator, sqlExpressions.Select((_, i) => "{" + i + "}")), sqlExpressions.ToArray());
        }
    }
}
