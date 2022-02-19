using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.EnumCollection;
using LogicEntity.Interface;
using LogicEntity.Model;
using LogicEntity.Tool;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 更新操作器
    /// </summary>
    internal class Updater<T> : OperatorBase, IUpdate<T>, IUpdaterJoin<T>, IUpdaterOn<T>, IUpdaterWhere where T : Table, new()
    {
        /// <summary>
        /// 公共表格表达式是否递归
        /// </summary>
        bool _isCommonTableExpressionRecursive = false;

        /// <summary>
        /// 公共表格表达式
        /// </summary>
        List<CommonTableExpression> _commonTableExpression = new();

        /// <summary>
        /// 表
        /// </summary>
        T _table;

        /// <summary>
        /// 关联关系
        /// </summary>
        List<Relation> _relations = new();

        /// <summary>
        /// 设置值
        /// </summary>
        Action<T> _setValue;

        /// <summary>
        /// 条件
        /// </summary>
        ConditionDescription _condition;

        /// <summary>
        /// 是否有条件
        /// </summary>
        bool _hasConditions;

        /// <summary>
        /// 公共表格表达式
        /// </summary>
        /// <param name="commonTableExpressions"></param>
        /// <returns></returns>
        public IUpdate<T> With(params CommonTableExpression[] commonTableExpressions)
        {
            if (commonTableExpressions is not null)
                _commonTableExpression.AddRange(commonTableExpressions);

            return this;
        }

        /// <summary>
        /// 公共表格表达式
        /// </summary>
        /// <param name="commonTableExpressions"></param>
        /// <returns></returns>
        public IUpdate<T> WithRecursive(params CommonTableExpression[] commonTableExpressions)
        {
            With(commonTableExpressions);

            _isCommonTableExpressionRecursive = true;

            return this;
        }

        /// <summary>
        /// 更新表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IUpdaterJoin<T> Update(T table)
        {
            _table = table;

            return this;
        }

        /// <summary>
        /// 添加从表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IUpdaterOn<T> Join(TableDescription table)
        {
            _relations.Add(new Relation(table, TableTier.Join));

            return this;
        }

        /// <summary>
        /// 添加从表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IUpdaterOn<T> InnerJoin(TableDescription table)
        {
            _relations.Add(new Relation(table, TableTier.InnerJoin));

            return this;
        }

        /// <summary>
        /// 左连接
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IUpdaterOn<T> LeftJoin(TableDescription table)
        {
            _relations.Add(new Relation(table, TableTier.LeftJoin));

            return this;
        }

        /// <summary>
        /// 右连接
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IUpdaterOn<T> RightJoin(TableDescription table)
        {
            _relations.Add(new Relation(table, TableTier.RightJoin));

            return this;
        }

        /// <summary>
        /// 全连接
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IUpdaterOn<T> FullJoin(TableDescription table)
        {
            _relations.Add(new Relation(table, TableTier.FullJoin));

            return this;
        }

        /// <summary>
        /// 自然连接
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IUpdaterOn<T> NaturalJoin(TableDescription table)
        {
            _relations.Add(new Relation(table, TableTier.NaturalJoin));

            return this;
        }

        /// <summary>
        /// 添加关联条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IUpdaterJoin<T> On(Condition condition)
        {
            Relation relation = _relations.LastOrDefault();

            if (relation is not null)
                relation.SetCondition(condition);

            return this;
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="setValue"></param>
        /// <returns></returns>
        public IUpdaterWhere Set(Action<T> setValue)
        {
            _setValue = setValue;

            return this;
        }

        /// <summary>
        /// 添加条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IUpdater Where(Condition condition)
        {
            _condition = condition;

            _hasConditions = true;

            return this;
        }

        /// <summary>
        /// 添加条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IUpdater Conditions(ConditionCollection condition)
        {
            _condition = condition;

            _hasConditions = true;

            return this;
        }

        /// <summary>
        /// 获取参数名称唯一的命令
        /// </summary>
        /// <returns></returns>
        public override Command GetCommandWithUniqueParameterName()
        {
            Command command = new Command();

            command.Parameters = new();

            //CTE
            string with = string.Empty;

            if (_commonTableExpression.Any())
            {
                with += "With";

                if (_isCommonTableExpressionRecursive)
                    with += " Recursive";

                with += "\n";

                with += string.Join(",\n", _commonTableExpression.Select(s =>
                {
                    CommonTableExpression.CommonTableExpressionCommand cteCommand = s.GetCommonTableExpressionCommand();

                    if (cteCommand is null)
                        return string.Empty;

                    if (cteCommand.Parameters is not null)
                        command.Parameters.AddRange(cteCommand.Parameters);

                    return cteCommand.CommandText;
                }));

                with += "\n";
            }

            //从表
            string relations = string.Empty;

            if (_relations.Any())
            {
                relations = "\n" + string.Join("\n", _relations.Select(s =>
                {
                    if (s is null)
                        return string.Empty;

                    Relation.Command relationCommand = s.GetCommand();

                    if (relationCommand is null)
                        return string.Empty;

                    if (relationCommand.Parameters is not null)
                        command.Parameters.AddRange(relationCommand.Parameters);

                    return relationCommand.CommandText;
                }));
            }

            //值
            T t = new();

            _setValue?.Invoke(t);

            List<string> columns = new();

            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                Column column = property.GetValue(t) as Column;

                if (column is null)
                    continue;

                if (column.IsValueSet == false)
                    continue;

                if (column.Writer is not null)
                    column.Value = column.Writer(column.Value);

                if (column.Value is Description)
                {
                    columns.Add(column.ToString() + " = " + (column.Value as Description).ToString());

                    continue;
                }

                string key = ToolService.UniqueName();

                columns.Add(column.ToString() + " = " + key);

                command.Parameters.Add(KeyValuePair.Create(key, column.Value));
            }

            string set = "\nSet " + string.Join(",\n    ", columns);

            //条件
            string conditions = string.Empty;

            if (_hasConditions)
            {
                conditions = "\nWhere ";

                if (_condition is not null)
                {
                    conditions += _condition;

                    command.Parameters.AddRange(_condition.Parameters ?? new List<KeyValuePair<string, object>>());
                }
            }

            command.CommandText = $"{with}Update {_table.FullName}{relations}{set}{conditions}";

            command.Parameters.AddRange(ExtraParameters);

            command.CommandTimeout = CommandTimeout;

            return command;
        }
    }
}
