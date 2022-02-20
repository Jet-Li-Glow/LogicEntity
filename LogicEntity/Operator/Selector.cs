﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Tool;
using LogicEntity.Interface;
using LogicEntity.Model;
using LogicEntity.Operator;
using LogicEntity.EnumCollection;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 查询操作器
    /// </summary>
    internal class Selector : OperatorBase, ISelect, IDistinct, IJoin, IOn, IThenBy, IHaving, ISelector
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
        /// 是否去重
        /// </summary>
        bool _distinct;

        /// <summary>
        /// 列
        /// </summary>
        List<Description> _columnDescriptions = new();

        /// <summary>
        /// 主表
        /// </summary>
        List<TableDescription> _mainTables = new();

        /// <summary>
        /// 是否有主表
        /// </summary>
        bool _hasMainTable;

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
        bool _hasConditions;

        /// <summary>
        /// 分组
        /// </summary>
        List<Description> _groupBy = new();

        /// <summary>
        /// 分组筛选条件
        /// </summary>
        ConditionDescription _having;

        /// <summary>
        /// 是否有分组筛选条件
        /// </summary>
        bool _hasHaving;

        /// <summary>
        /// 排序
        /// </summary>
        List<OrderDescription> _orderBy = new();

        /// <summary>
        /// 限制条数
        /// </summary>
        string _limit;

        /// <summary>
        /// 是否进行更新
        /// </summary>
        bool _isForUpdate;

        /// <summary>
        /// 联合表
        /// </summary>
        List<UnionDescription> _unionDescriptions = new();

        /// <summary>
        /// 列
        /// </summary>
        public IEnumerable<Description> Columns
        {
            get
            {
                List<Description> columns = new();

                if (_columnDescriptions.Any())
                {
                    foreach (Description column in _columnDescriptions)
                    {
                        if (column is not AllColumnDescription)
                        {
                            columns.Add(column);

                            continue;
                        }

                        columns.AddRange((column as AllColumnDescription).Table?.Columns ?? Enumerable.Empty<Description>());
                    }

                    return columns;
                }

                columns.AddRange(_mainTables.SelectMany(s => s?.Columns ?? Enumerable.Empty<Description>()));

                columns.AddRange(_relations.SelectMany(s => s.Table?.Columns ?? Enumerable.Empty<Description>()));

                return columns.AsEnumerable();
            }
        }

        /// <summary>
        /// 公共表格表达式
        /// </summary>
        /// <param name="commonTableExpressions"></param>
        /// <returns></returns>
        public ISelect With(params CommonTableExpression[] commonTableExpressions)
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
        public ISelect WithRecursive(params CommonTableExpression[] commonTableExpressions)
        {
            With(commonTableExpressions);

            _isCommonTableExpressionRecursive = true;

            return this;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="columnDescriptions"></param>
        /// <returns></returns>
        public IDistinct Select(params Description[] columnDescriptions)
        {
            if (columnDescriptions is not null)
                _columnDescriptions.AddRange(columnDescriptions);

            return this;
        }

        /// <summary>
        /// 取唯一的结果
        /// </summary>
        /// <returns></returns>
        public IFrom Distinct()
        {
            _distinct = true;

            return this;
        }

        /// <summary>
        /// 添加主表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IJoin From(params TableDescription[] tables)
        {
            _hasMainTable = true;

            if (tables is not null)
                _mainTables.AddRange(tables);

            return this;
        }

        /// <summary>
        /// 以 Join 添加从表
        /// </summary>
        /// <param name="table">从表</param>
        /// <returns></returns>
        public IOn Join(TableDescription table)
        {
            _relations.Add(new Relation(table, TableTier.Join));

            return this;
        }

        /// <summary>
        /// 以 Inner Join 添加从表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IOn InnerJoin(TableDescription table)
        {
            _relations.Add(new Relation(table, TableTier.InnerJoin));

            return this;
        }

        /// <summary>
        /// 以 Left Join 添加从表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IOn LeftJoin(TableDescription table)
        {
            _relations.Add(new Relation(table, TableTier.LeftJoin));

            return this;
        }

        /// <summary>
        /// 以 Right Join 添加从表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IOn RightJoin(TableDescription table)
        {
            _relations.Add(new Relation(table, TableTier.RightJoin));

            return this;
        }

        /// <summary>
        /// 以 Full Join 添加从表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IOn FullJoin(TableDescription table)
        {
            _relations.Add(new Relation(table, TableTier.FullJoin));

            return this;
        }

        /// <summary>
        /// 以 Natural Join 添加从表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IOn NaturalJoin(TableDescription table)
        {
            _relations.Add(new Relation(table, TableTier.NaturalJoin));

            return this;
        }

        /// <summary>
        /// 添加关联条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IJoin On(Condition condition)
        {
            Relation relation = _relations.LastOrDefault();

            if (relation is not null)
                relation.SetCondition(condition);

            return this;
        }

        /// <summary>
        /// 添加 WHERE 条件
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public IGroupBy Where(ConditionDescription condition)
        {
            _condition = condition;

            _hasConditions = true;

            return this;
        }

        /// <summary>
        /// 分组
        /// </summary>
        /// <param name="descriptions"></param>
        /// <returns></returns>
        public IHaving GroupBy(params Description[] descriptions)
        {
            _groupBy.AddRange(descriptions);

            return this;
        }

        /// <summary>
        /// 添加组筛选条件
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public IUnion Having(Condition condition)
        {
            _having = condition;

            _hasHaving = true;

            return this;
        }

        /// <summary>
        /// 联合
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IUnion Union(ISelector selector)
        {
            _unionDescriptions.Add(new UnionDescription() { TableTier = TableTier.Union, Selector = selector });

            return this;
        }

        /// <summary>
        /// 联合所有
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IUnion UnionAll(ISelector selector)
        {
            _unionDescriptions.Add(new UnionDescription() { TableTier = TableTier.UnionAll, Selector = selector });

            return this;
        }

        /// <summary>
        /// 升序排序
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public IThenBy OrderBy(Description description)
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
        public IThenBy OrderByDescending(Description description)
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
        public IThenBy ThenBy(Description description)
        {
            _orderBy.Add(new OrderDescription(description, true));

            return this;
        }

        /// <summary>
        /// 以降序进行后续排序
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public IThenBy ThenByDescending(Description description)
        {
            _orderBy.Add(new OrderDescription(description, false));

            return this;
        }

        /// <summary>
        /// 返回指定行数
        /// </summary>
        /// <param name="limit">返回的行数</param>
        /// <returns></returns>
        public IForUpdate Limit(int limit)
        {
            _limit = "Limit " + limit.ToString();

            return this;
        }

        /// <summary>
        /// 返回指定范围的行
        /// </summary>
        /// <param name="offset">跳过的行</param>
        /// <param name="limit">返回的行数</param>
        /// <returns></returns>
        public IForUpdate Limit(int offset, int limit)
        {
            _limit = "Limit " + offset.ToString() + ", " + limit.ToString();

            return this;
        }

        /// <summary>
        /// 返回前指定数量的行
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public IForUpdate Top(int limit)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 加锁
        /// </summary>
        /// <returns></returns>
        public ISelector ForUpdate()
        {
            _isForUpdate = true;

            return this;
        }

        /// <summary>
        /// 作为嵌套表
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public NestedTable As(string alias)
        {
            return new NestedTable(this, alias);
        }

        /// <summary>
        /// 获取参数名称唯一的命令
        /// </summary>
        /// <returns></returns>
        public override Command GetCommandWithUniqueParameterName()
        {
            //命令
            Command command = new();

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

            //查询
            string select = "Select";

            if (_distinct)
                select += " Distinct";

            select += "\n";

            //列
            string columns = string.Empty;

            if (_columnDescriptions.Any())
                columns = string.Join(",\n  ", _columnDescriptions.Select(s => s?.ToString()));
            else
                columns = "*";

            columns = "  " + columns;

            //主表
            string tables = string.Empty;

            if (_hasMainTable)
            {
                tables = "\nFrom\n  " + string.Join(",\n  ", _mainTables.Select(s =>
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

            //条件
            string conditions = string.Empty;

            if (_hasConditions)
            {
                conditions = "\nWhere ";

                ConditionDescription.Command conditionCommand = _condition?.GetCommand();

                if (conditionCommand is not null)
                {
                    conditions += conditionCommand.CommandText;

                    if (conditionCommand.Parameters is not null)
                        command.Parameters.AddRange(conditionCommand.Parameters);
                }
            }

            //分组
            string groupBy = string.Empty;

            if (_groupBy.Any())
            {
                groupBy = "\nGroup By " + string.Join(", ", _groupBy.Select(s => s?.ToString()));
            }

            //分组条件
            string having = string.Empty;

            if (_hasHaving)
            {
                having = "\nHaving ";

                ConditionDescription.Command havingCommand = _having?.GetCommand();

                if (havingCommand is not null)
                {
                    having += havingCommand.CommandText;

                    if (havingCommand.Parameters is not null)
                        command.Parameters.AddRange(havingCommand.Parameters);
                }
            }

            //联合表
            string union = string.Empty;

            foreach (UnionDescription unionDescription in _unionDescriptions)
            {
                Command unionCommand = unionDescription.Selector.GetCommandWithUniqueParameterName();

                union += "\n\n" + unionDescription.TableTier.Description() + "\n\n" + unionCommand.CommandText;

                command.Parameters.AddRange(unionCommand.Parameters);
            }

            //排序
            string orderBy = string.Empty;

            if (_orderBy.Any())
            {
                orderBy = "\nOrder By " + string.Join(", ", _orderBy.Select(o => o?.Description()));
            }

            //限制条数
            string limit = _limit.IsValid() ? "\n" + _limit : string.Empty;

            //加锁
            string forUpdate = _isForUpdate ? "\nFor Update" : string.Empty;

            //命令
            command.CommandText = $"{with}{select}{columns}{tables}{relations}{conditions}{groupBy}{having}{union}{orderBy}{limit}{forUpdate}";

            command.Parameters.AddRange(ExtraParameters);

            command.CommandTimeout = CommandTimeout;

            command.Readers = new();

            command.BytesReaders = new();

            command.CharsReaders = new();

            List<Description> cols = Columns.ToList();

            for (int i = 0; i < cols.Count; i++)
            {
                if (cols[i].Reader is not null)
                    command.Readers[i] = cols[i].Reader;
                else if (cols[i].BytesReader is not null)
                    command.BytesReaders[i] = cols[i].BytesReader;
                else if (cols[i].CharsReader is not null)
                    command.CharsReaders[i] = cols[i].CharsReader;
            }

            return command;
        }

        /// <summary>
        /// 联合表描述
        /// </summary>
        class UnionDescription
        {
            /// <summary>
            /// 表级别
            /// </summary>
            public TableTier TableTier { get; set; }

            /// <summary>
            /// 查询操作器
            /// </summary>
            public ISelector Selector { get; set; }
        }
    }
}
