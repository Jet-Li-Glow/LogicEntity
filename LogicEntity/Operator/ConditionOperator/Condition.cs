using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Extension;
using LogicEntity.Model;
using LogicEntity.EnumCollection;
using LogicEntity.Interface;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 条件
    /// </summary>
    public class Condition : ConditionDescription
    {
        /// <summary>
        /// 参数
        /// </summary>
        private List<KeyValuePair<string, object>> _parameters = new();

        /// <summary>
        /// 条件
        /// </summary>
        /// <param name="left">左值</param>
        /// <param name="comparator">比较符</param>
        /// <param name="right">右值</param>
        public Condition(Description left, Comparator comparator, object right)
        {
            if (right is Description)
            {
                _conditionStr = left?.ToString() + " " + comparator.Description() + " " + (right as Description).ToString();

                return;
            }

            if (right is null && comparator == Comparator.Equal)
            {
                _conditionStr = left?.ToString() + " is Null";

                return;
            }

            if (comparator == Comparator.In)
            {
                if (right is ISelector)
                {
                    Command command = (right as ISelector).GetCommand();

                    foreach (KeyValuePair<string, object> parameter in command.Parameters)
                    {
                        string selectorKey = "@param" + DateTime.Now.Ticks;

                        command.CommandText = command.CommandText.Replace(parameter.Key, selectorKey);

                        _parameters.Add(KeyValuePair.Create(selectorKey, parameter.Value));
                    }

                    _conditionStr = left?.ToString() + " " + comparator.Description() + " (\n" + command.CommandText + "\n)";

                    return;
                }

                if (right is IEnumerable)
                {
                    List<string> ekeys = new List<string>();

                    foreach (object obj in right as IEnumerable)
                    {
                        string ekey = "@param" + DateTime.Now.Ticks;

                        _parameters.Add(KeyValuePair.Create(ekey, obj));

                        ekeys.Add(ekey);
                    }

                    _conditionStr = left?.ToString() + " " + comparator.Description() + " (" + string.Join(", ", ekeys) + ")";

                    return;
                }

                _conditionStr = left?.ToString() + " " + comparator.Description() + " (" + right + ")";

                return;
            }

            string key = "@param" + DateTime.Now.Ticks;

            _parameters.Add(KeyValuePair.Create(key, right));

            _conditionStr = left?.ToString() + " " + comparator.Description() + " " + key;
        }

        /// <summary>
        /// 条件
        /// </summary>
        /// <param name="conditionStr">条件字符串</param>
        public Condition(string conditionStr)
        {
            _conditionStr = conditionStr;
        }

        /// <summary>
        /// 条件字符串
        /// </summary>
        private string _conditionStr;

        /// <summary>
        /// 是否有多个表达式
        /// </summary>
        internal bool IsMultiple { get; private set; }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="parameters"></param>
        private void AddParameters(IEnumerable<KeyValuePair<string, object>> parameters)
        {
            if (parameters is not null)
                _parameters.AddRange(parameters);
        }

        /// <summary>
        /// 描述
        /// </summary>
        /// <returns></returns>
        internal override string Description()
        {
            return _conditionStr;
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        internal override IEnumerable<KeyValuePair<string, object>> GetParameters()
        {
            return _parameters.Select(s => KeyValuePair.Create(s.Key, s.Value));
        }

        /// <summary>
        /// 组合条件
        /// </summary>
        /// <param name="logicalOperator"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        private Condition After(LogicalOperator logicalOperator, Condition condition)
        {
            IsMultiple = true;

            if (condition is null)
            {
                _conditionStr += "\n" + logicalOperator.Description().PadLeft(5) + " ";

                return this;
            }

            string str = condition.IsMultiple ? "(" + condition.Description() + ")" : condition.Description();

            _conditionStr += "\n" + logicalOperator.Description().PadLeft(5) + " " + str;

            AddParameters(condition.GetParameters());

            return this;
        }

        /// <summary>
        /// 组合条件
        /// </summary>
        /// <param name="logicalOperator"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        private Condition After(LogicalOperator logicalOperator, bool condition)
        {
            _conditionStr += "\n" + logicalOperator.Description().PadLeft(5) + " " + condition.ToString();

            return this;
        }

        /// <summary>
        /// 与
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Condition operator &(Condition left, Condition right)
        {
            return left.After(LogicalOperator.And, right);
        }

        /// <summary>
        /// 与
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Condition operator &(Condition left, bool right)
        {
            return left.After(LogicalOperator.And, right);
        }

        /// <summary>
        /// 与
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Condition operator &(bool left, Condition right)
        {
            return new Condition(left.ToString()) & right;
        }

        /// <summary>
        /// 或
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Condition operator |(Condition left, Condition right)
        {
            return left.After(LogicalOperator.Or, right);
        }

        /// <summary>
        /// 或
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Condition operator |(Condition left, bool right)
        {
            return left.After(LogicalOperator.Or, right);
        }

        /// <summary>
        /// 或
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Condition operator |(bool left, Condition right)
        {
            return new Condition(left.ToString()) | right;
        }
    }
}
