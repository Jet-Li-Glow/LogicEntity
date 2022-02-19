using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.EnumCollection;
using LogicEntity.Interface;
using LogicEntity.Model;
using LogicEntity.Tool;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 删除操作器
    /// </summary>
    internal class Deleter : OperatorBase, IDelete, IDeleterFrom, IDeleterJoin, IDeleterOn, IDeleterThenBy
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
        /// 删除表
        /// </summary>
        List<Table> _deleteTables = new();

        /// <summary>
        /// 来源表
        /// </summary>
        List<TableDescription> _fromTables = new();

        /// <summary>
        /// 从表
        /// </summary>
        List<Relation> _relations = new();

        /// <summary>
        /// 条件
        /// </summary>
        ConditionDescription _condition;

        /// <summary>
        /// 是否有条件
        /// </summary>
        bool _hasConditons;

        /// <summary>
        /// 排序
        /// </summary>
        List<OrderDescription> _orderBy = new();

        /// <summary>
        /// 限制条数
        /// </summary>
        string _limit;

        /// <summary>
        /// 公共表格表达式
        /// </summary>
        /// <param name="commonTableExpressions"></param>
        /// <returns></returns>
        public IDelete With(params CommonTableExpression[] commonTableExpressions)
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
        public IDelete WithRecursive(params CommonTableExpression[] commonTableExpressions)
        {
            With(commonTableExpressions);

            _isCommonTableExpressionRecursive = true;

            return this;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        public IDeleterFrom Delete(params Table[] tables)
        {
            if (tables is not null)
                _deleteTables.AddRange(tables);

            return this;
        }

        /// <summary>
        /// 主表
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        public IDeleterJoin From(params TableDescription[] tables)
        {
            if (tables is not null)
                _fromTables.AddRange(tables);

            return this;
        }

        /// <summary>
        /// Join
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IDeleterOn Join(TableDescription table)
        {
            _relations.Add(new Relation(table, TableTier.Join));

            return this;
        }

        /// <summary>
        /// InnerJoin
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IDeleterOn InnerJoin(TableDescription table)
        {
            _relations.Add(new Relation(table, TableTier.InnerJoin));

            return this;
        }

        /// <summary>
        /// LeftJoin
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IDeleterOn LeftJoin(TableDescription table)
        {
            _relations.Add(new Relation(table, TableTier.LeftJoin));

            return this;
        }

        /// <summary>
        /// RightJoin
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IDeleterOn RightJoin(TableDescription table)
        {
            _relations.Add(new Relation(table, TableTier.RightJoin));

            return this;
        }

        /// <summary>
        /// FullJoin
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IDeleterOn FullJoin(TableDescription table)
        {
            _relations.Add(new Relation(table, TableTier.FullJoin));

            return this;
        }

        /// <summary>
        /// NaturalJoin
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IDeleterOn NaturalJoin(TableDescription table)
        {
            _relations.Add(new Relation(table, TableTier.NaturalJoin));

            return this;
        }

        /// <summary>
        /// 关联条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IDeleterJoin On(Condition condition)
        {
            Relation relation = _relations.LastOrDefault();

            if (relation is not null)
                relation.SetCondition(condition);

            return this;
        }

        /// <summary>
        /// 条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IDeleterOrderBy Where(Condition condition)
        {
            _condition = condition;

            _hasConditons = true;

            return this;
        }

        /// <summary>
        /// 添加条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IDeleterOrderBy Conditions(ConditionCollection condition)
        {
            _condition = condition;

            _hasConditons = true;

            return this;
        }

        /// <summary>
        /// 升序排序
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public IDeleterThenBy OrderBy(Description description)
        {
            _orderBy.Clear();

            _orderBy.Add(new OrderDescription(description, true));

            return this;
        }

        /// <summary>
        /// 降序排序
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public IDeleterThenBy OrderByDescending(Description description)
        {
            _orderBy.Clear();

            _orderBy.Add(new OrderDescription(description, false));

            return this;
        }

        /// <summary>
        /// 以升序进行后续排序
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public IDeleterThenBy ThenBy(Description description)
        {
            _orderBy.Add(new OrderDescription(description, true));

            return this;
        }

        /// <summary>
        /// 以降序进行后续排序
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public IDeleterThenBy ThenByDescending(Description description)
        {
            _orderBy.Add(new OrderDescription(description, false));

            return this;
        }

        /// <summary>
        /// 返回指定行数
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public IDeleter Limit(int limit)
        {
            _limit = "Limit " + limit.ToString();

            return this;
        }

        /// <summary>
        /// 返回指定范围的行
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public IDeleter Limit(int offset, int limit)
        {
            _limit = "Limit " + offset.ToString() + ", " + limit.ToString();

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

            //删除
            string delete = "Delete";

            if (_deleteTables.Any())
            {
                delete += $" {string.Join(", ", _deleteTables.Select(s => s.FinalTableName))}";
            }

            //主表
            string from = $"\nFrom " + string.Join(", ", _fromTables.Select(s =>
            {
                if (s is null)
                    return string.Empty;

                TableDescription.Command tableCommand = s.GetCommand();

                if (tableCommand is null)
                    return string.Empty;

                if (tableCommand.Parameters is not null)
                    command.Parameters.AddRange(tableCommand.Parameters);

                return tableCommand.CommandText;
            }));

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

            //条件
            string conditions = string.Empty;

            if (_hasConditons)
            {
                conditions = "\nWhere ";

                if (_condition is not null)
                {
                    conditions += _condition;

                    command.Parameters.AddRange(_condition.Parameters);
                }
            }

            //排序
            string orderBy = string.Empty;

            if (_orderBy.Any())
            {
                orderBy = "\nOrder By " + string.Join(", ", _orderBy.Select(o => o?.Description()));
            }

            //限制条数
            string limit = _limit.IsValid() ? "\n" + _limit : string.Empty;

            //命令
            command.CommandText = $"{with}{delete}{from}{relations}{conditions}{orderBy}{limit}";

            command.Parameters.AddRange(ExtraParameters);

            command.CommandTimeout = CommandTimeout;

            return command;
        }
    }
}
