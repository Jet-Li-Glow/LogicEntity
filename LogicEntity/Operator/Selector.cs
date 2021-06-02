﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Extension;
using LogicEntity.Interface;
using LogicEntity.Model;
using LogicEntity.Operator;
using LogicEntity.EnumCollection;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 查询操作器
    /// </summary>
    public class Selector : DBOperator, IFrom, IJoin, IOn, IThenBy, IHaving, ISelector
    {
        /// <summary>
        /// 是否去重
        /// </summary>
        private bool _distinct;

        /// <summary>
        /// 列
        /// </summary>
        private IEnumerable<Description> _columnDescriptions;

        /// <summary>
        /// 主表
        /// </summary>
        private TableDescription[] _mainTables;

        /// <summary>
        /// 从表
        /// </summary>
        private List<Relation> _relations = new();

        /// <summary>
        /// 条件
        /// </summary>
        private ConditionDescription _condition;

        /// <summary>
        /// 是否有条件
        /// </summary>
        private bool _hasConditions;

        /// <summary>
        /// 分组
        /// </summary>
        private List<Description> _groupBy = new();

        /// <summary>
        /// 分组筛选条件
        /// </summary>
        private ConditionDescription _having;

        /// <summary>
        /// 是否有分组筛选条件
        /// </summary>
        private bool _hasHaving;

        /// <summary>
        /// 排序
        /// </summary>
        private List<OrderDescription> _orderBy = new();

        /// <summary>
        /// 限制条数
        /// </summary>
        private string _limit;

        /// <summary>
        /// 查询操作器
        /// </summary>
        /// <param name="columnDescriptions"></param>
        public Selector(IEnumerable<Description> columnDescriptions)
        {
            _columnDescriptions = columnDescriptions;
        }

        /// <summary>
        /// 取唯一的结果
        /// </summary>
        /// <returns></returns>
        internal Selector Distinct()
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
            _mainTables = tables;

            return this;
        }

        /// <summary>
        /// 以 Join 添加从表
        /// </summary>
        /// <param name="table">从表</param>
        /// <returns></returns>
        public IOn Join(TableDescription table)
        {
            Relation relation = new Relation() { TableTier = TableTier.Join };

            relation.SetTable(table);

            _relations.Add(relation);

            return this;
        }

        /// <summary>
        /// 以 Inner Join 添加从表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IOn InnerJoin(TableDescription table)
        {
            Relation relation = new Relation() { TableTier = TableTier.InnerJoin };

            relation.SetTable(table);

            _relations.Add(relation);

            return this;
        }

        /// <summary>
        /// 以 Left Join 添加从表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IOn LeftJoin(TableDescription table)
        {
            Relation relation = new Relation() { TableTier = TableTier.LeftJoin };

            relation.SetTable(table);

            _relations.Add(relation);

            return this;
        }

        /// <summary>
        /// 以 Right Join 添加从表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IOn RightJoin(TableDescription table)
        {
            Relation relation = new Relation() { TableTier = TableTier.RightJoin };

            relation.SetTable(table);

            _relations.Add(relation);

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
        public IGroupBy Where(Condition condition)
        {
            _condition = condition;

            _hasConditions = true;

            return this;
        }

        /// <summary>
        /// 添加 WHERE 条件
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public IGroupBy With(ConditionCollection condition)
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
        /// 添加 HAVING 条件
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public IOrderBy Having(Condition condition)
        {
            _having = condition;

            _hasHaving = true;

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
        /// 返回指定条数
        /// </summary>
        /// <param name="limit">返回的条数</param>
        /// <returns></returns>
        public ISelector Limit(int limit)
        {
            _limit = "Limit " + limit.ToString();

            return this;
        }

        /// <summary>
        /// 返回指定范围的条目
        /// </summary>
        /// <param name="offset">跳过的条数</param>
        /// <param name="limit">返回的条数</param>
        /// <returns></returns>
        public ISelector Limit(int offset, int limit)
        {
            _limit = "Limit " + offset.ToString() + ", " + limit.ToString();

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
        /// 获取操作命令
        /// </summary>
        /// <returns></returns>
        public Command GetCommand()
        {
            //参数
            List<KeyValuePair<string, object>> parameters = new();

            //唯一
            string distinct = _distinct ? "Distinct" : string.Empty;

            //列
            string columns = string.Empty;

            if (_columnDescriptions is not null && _columnDescriptions.Any())
                columns = string.Join(",\n  ", _columnDescriptions.Select(s => s?.FullContent));
            else
                columns = "*";

            //主表
            string tables = string.Empty;

            if (_mainTables is not null)
            {
                tables = string.Join(",\n  ", _mainTables.Select(t => t?.Description()));

                parameters.AddRange(_mainTables.SelectMany(t => t?.GetParameters() ?? new List<KeyValuePair<string, object>>()));
            }

            //从表
            string relations = string.Empty;

            if (_relations.Any())
            {
                relations = "\n" + string.Join("\n", _relations.Select(r => r.Description()));

                parameters.AddRange(_relations.SelectMany(r => r.GetParameters()));
            }

            //条件
            string conditions = string.Empty;

            if (_hasConditions)
            {
                conditions = "\nWhere ";

                if (_condition is not null)
                {
                    conditions += _condition.Description();

                    parameters.AddRange(_condition.GetParameters());
                }
            }

            //分组
            string groupBy = string.Empty;

            if (_groupBy.Any())
            {
                groupBy = "\nGroup By " + string.Join(", ", _groupBy.Select(s => s?.FullContent));
            }

            //分组条件
            string having = string.Empty;

            if (_hasHaving)
            {
                having = "\nHaving ";

                if (_having is not null)
                {
                    having += _having.Description();

                    parameters.AddRange(_having.GetParameters());
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

            Command command = new Command();

            command.CommandText = $"Select {distinct}\n  {columns}\nFrom\n  {tables}{relations}{conditions}{groupBy}{having}{orderBy}{limit}";

            command.Parameters = new();

            for (int i = 0; i < parameters.Count; i++)
            {
                string key = "@param" + i.ToString();

                command.CommandText = command.CommandText.Replace(parameters[i].Key, key);

                command.Parameters.Add(KeyValuePair.Create(key, parameters[i].Value));
            }

            return command;
        }
    }
}
