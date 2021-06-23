using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.EnumCollection;
using LogicEntity.Interface;
using LogicEntity.Model;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 更新操作器
    /// </summary>
    internal class Updater<T> : OperatorBase, IUpdaterJoin<T>, IUpdaterOn<T>, IUpdaterWhere where T : Table, new()
    {
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
        /// 更新操作器
        /// </summary>
        /// <param name="table"></param>
        public Updater(T table)
        {
            _table = table;
        }

        /// <summary>
        /// 添加从表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IUpdaterOn<T> Join(TableDescription table)
        {
            Relation relation = new Relation() { TableTier = TableTier.Join };

            relation.SetTable(table);

            _relations.Add(relation);

            return this;
        }

        /// <summary>
        /// 添加从表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IUpdaterOn<T> InnerJoin(TableDescription table)
        {
            Relation relation = new Relation() { TableTier = TableTier.InnerJoin };

            relation.SetTable(table);

            _relations.Add(relation);

            return this;
        }

        /// <summary>
        /// 左连接
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IUpdaterOn<T> LeftJoin(TableDescription table)
        {
            Relation relation = new Relation() { TableTier = TableTier.LeftJoin };

            relation.SetTable(table);

            _relations.Add(relation);

            return this;
        }

        /// <summary>
        /// 右连接
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IUpdaterOn<T> RightJoin(TableDescription table)
        {
            Relation relation = new Relation() { TableTier = TableTier.RightJoin };

            relation.SetTable(table);

            _relations.Add(relation);

            return this;
        }

        /// <summary>
        /// 全连接
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IUpdaterOn<T> FullJoin(TableDescription table)
        {
            Relation relation = new Relation() { TableTier = TableTier.FullJoin };

            relation.SetTable(table);

            _relations.Add(relation);

            return this;
        }

        /// <summary>
        /// 自然连接
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IUpdaterOn<T> NaturalJoin(TableDescription table)
        {
            Relation relation = new Relation() { TableTier = TableTier.NaturalJoin };

            relation.SetTable(table);

            _relations.Add(relation);

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
        public IUpdater With(ConditionCollection condition)
        {
            _condition = condition;

            _hasConditions = true;

            return this;
        }

        public override Command GetCommand()
        {
            int index = 0;

            Command command = new Command();

            command.Parameters = new();

            //从表
            string relations = string.Empty;

            if (_relations.Any())
            {
                relations = "\n" + string.Join("\n", _relations);

                foreach (KeyValuePair<string, object> parameter in _relations.SelectMany(r => r.GetParameters()))
                {
                    string key = "@param" + index.ToString();

                    relations = relations.Replace(parameter.Key, key);

                    command.Parameters.Add(KeyValuePair.Create(key, parameter.Value));

                    index++;
                }
            }

            //值
            T t = new();

            _setValue?.Invoke(t);

            var properties = t.GetType().GetProperties().Where(p => p.PropertyType == typeof(Column));

            List<string> columns = new();

            foreach (PropertyInfo property in properties)
            {
                Column column = property.GetValue(t) as Column;

                if (column is null)
                    continue;

                if (column.IsValueSet == false)
                    continue;

                if (column.Value is Description)
                {
                    columns.Add(column.ToString() + " = " + (column.Value as Description).ToString());

                    continue;
                }

                string key = "@param" + index.ToString();

                columns.Add(column.ToString() + " = " + key);

                command.Parameters.Add(KeyValuePair.Create(key, column.Value));

                index++;
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

                    foreach (KeyValuePair<string, object> parameter in _condition.GetParameters())
                    {
                        string key = "@param" + index.ToString();

                        conditions = conditions.Replace(parameter.Key, key);

                        command.Parameters.Add(KeyValuePair.Create(key, parameter.Value));

                        index++;
                    }
                }
            }

            command.CommandText = $"Update {_table.FullName}{relations}{set}{conditions}";

            command.Parameters.AddRange(ExtraParameters);

            command.CommandTimeout = CommandTimeout;

            return command;
        }
    }
}
