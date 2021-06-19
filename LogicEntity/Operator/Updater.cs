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
    public class Updater<T> : IUpdaterJoin<T>, IUpdaterOn<T>, IUpdaterWhere where T : Table, new()
    {
        private T _table;

        private List<Relation> _relations = new();

        private Action<T> _setValue;

        private ConditionDescription _condition;

        private bool _hasConditions;

        /// <summary>
        /// 超时时间（秒）
        /// </summary>
        private int _commandTimeout = 0;

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

        public IUpdaterOn<T> LeftJoin(TableDescription table)
        {
            Relation relation = new Relation() { TableTier = TableTier.LeftJoin };

            relation.SetTable(table);

            _relations.Add(relation);

            return this;
        }

        public IUpdaterOn<T> RightJoin(TableDescription table)
        {
            Relation relation = new Relation() { TableTier = TableTier.RightJoin };

            relation.SetTable(table);

            _relations.Add(relation);

            return this;
        }

        public IUpdaterOn<T> FullJoin(TableDescription table)
        {
            Relation relation = new Relation() { TableTier = TableTier.FullJoin };

            relation.SetTable(table);

            _relations.Add(relation);

            return this;
        }

        public IUpdaterOn<T> NaturalJoin(TableDescription table)
        {
            Relation relation = new Relation() { TableTier = TableTier.NaturalJoin };

            relation.SetTable(table);

            _relations.Add(relation);

            return this;
        }

        public IUpdaterJoin<T> On(Condition condition)
        {
            Relation relation = _relations.LastOrDefault();

            if (relation is not null)
                relation.SetCondition(condition);

            return this;
        }

        public IUpdaterWhere Set(Action<T> setValue)
        {
            _setValue = setValue;

            return this;
        }

        public IUpdater Where(Condition condition)
        {
            _condition = condition;

            _hasConditions = true;

            return this;
        }

        public IUpdater With(ConditionCollection condition)
        {
            _condition = condition;

            _hasConditions = true;

            return this;
        }

        public IUpdater SetCommandTimeout(int seconds)
        {
            _commandTimeout = seconds;

            return this;
        }

        public Command GetCommand()
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

            command.CommandTimeout = _commandTimeout;

            return command;
        }
    }
}
