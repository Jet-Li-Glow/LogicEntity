using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Tool;
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
        /// 条件字符串
        /// </summary>
        string _conditionStr;

        /// <summary>
        /// 参数
        /// </summary>
        List<KeyValuePair<string, object>> _parameters = new();

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
                _conditionStr = $"{left} {comparator.Description()} {right as Description}";

                return;
            }

            if (right is null && comparator == Comparator.Equal)
            {
                _conditionStr = left?.ToString() + " is Null";

                return;
            }

            if (right is null && comparator == Comparator.NotEqual)
            {
                _conditionStr = left?.ToString() + " is Not Null";

                return;
            }

            string key = ToolService.UniqueName();

            _parameters.Add(KeyValuePair.Create(key, right));

            _conditionStr = $"{left} {comparator.Description()} {key}";
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
        /// 条件
        /// </summary>
        internal Condition()
        {
        }

        /// <summary>
        /// 生成 In 条件
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static Condition In(Description left, ISelector right)
        {
            if (right is null)
                return new Condition($"{left} In (Null)");

            Condition condition = new();

            Model.Command selectorCommand = right.GetCommandWithUniqueParameterName();

            if (selectorCommand.Parameters is not null)
                condition._parameters.AddRange(selectorCommand.Parameters);

            condition._conditionStr = $"{left} In \n    (\n      {selectorCommand.CommandText.Replace("\n", "\n      ")}\n    )";

            return condition;
        }

        /// <summary>
        /// 生成 In 条件
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static Condition In(Description left, IEnumerable right)
        {
            if (right is null)
                return new Condition($"{left} In (Null)");

            Condition condition = new();

            List<string> keys = new List<string>();

            foreach (object obj in right)
            {
                string key = ToolService.UniqueName();

                condition._parameters.Add(KeyValuePair.Create(key, obj));

                keys.Add(key);
            }

            condition._conditionStr = $"{left} In ({string.Join(", ", keys)})";

            return condition;
        }

        /// <summary>
        /// 生成 In 条件
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static Condition In(Description left, object right)
        {
            if (right is null)
                return new Condition($"{left} In (Null)");

            if (right is ISelector)
                return In(left, right as ISelector);

            if (right is IEnumerable)
                return In(left, right as IEnumerable);

            return new Condition($"{left} In ({right})");
        }

        /// <summary>
        /// 生成 Between 条件
        /// </summary>
        /// <param name="description"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static Condition Between(Description description, object left, object right)
        {
            Condition condition = new();

            string keyLeft;

            if (left is Description)
            {
                keyLeft = left.ToString();
            }
            else
            {
                keyLeft = ToolService.UniqueName();

                condition._parameters.Add(KeyValuePair.Create(keyLeft, left));
            }

            string keyRight;

            if (right is Description)
            {
                keyRight = right.ToString();
            }
            else
            {
                keyRight = ToolService.UniqueName();

                condition._parameters.Add(KeyValuePair.Create(keyRight, right));
            }

            condition._conditionStr = $"{description} Between {keyLeft} And {keyRight}";

            return condition;
        }

        /// <summary>
        /// 生成 Not Between 条件
        /// </summary>
        /// <param name="description"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static Condition NotBetween(Description description, object left, object right)
        {
            Condition condition = new();

            string keyLeft;

            if (left is Description)
            {
                keyLeft = left.ToString();
            }
            else
            {
                keyLeft = ToolService.UniqueName();

                condition._parameters.Add(KeyValuePair.Create(keyLeft, left));
            }

            string keyRight;

            if (right is Description)
            {
                keyRight = right.ToString();
            }
            else
            {
                keyRight = ToolService.UniqueName();

                condition._parameters.Add(KeyValuePair.Create(keyRight, right));
            }

            condition._conditionStr = $"{description} Not Between {keyLeft} And {keyRight}";

            return condition;
        }

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

            Command conditionCommand = condition.GetCommand();

            string str = string.Empty;

            if (conditionCommand is not null)
            {
                str = conditionCommand.CommandText;

                if (condition.IsMultiple)
                    str = $"({str})";

                if (conditionCommand.Parameters is not null)
                    AddParameters(conditionCommand.Parameters);
            }

            _conditionStr += "\n" + logicalOperator.Description().PadLeft(5) + " " + str;

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
            _conditionStr += "\n" + logicalOperator.Description().PadLeft(5) + " " + condition;

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

        /// <summary>
        /// 获取命令
        /// </summary>
        /// <returns></returns>
        internal override Command GetCommand()
        {
            return new Command()
            {
                CommandText = _conditionStr,
                Parameters = _parameters.AsEnumerable()
            };
        }
    }
}
