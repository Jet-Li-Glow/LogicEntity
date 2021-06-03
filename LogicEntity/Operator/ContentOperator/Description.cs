using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.EnumCollection;
using LogicEntity.Extension;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 字符串描述
    /// </summary>
    public class Description
    {
        private string _content;

        /// <summary>
        /// 查询前转换器
        /// </summary>
        protected List<Func<string, string>> BeforeConvertors = new List<Func<string, string>>();

        /// <summary>
        /// 字符串描述
        /// </summary>
        public Description()
        {
        }

        /// <summary>
        /// 字符串描述
        /// </summary>
        /// <param name="content">描述内容</param>
        public Description(string content)
        {
            _content = content;
        }

        /// <summary>
        /// 添加查询前转换器
        /// </summary>
        /// <param name="convertor"></param>
        private void AddBeforeConvertor(Func<string, string> convertor)
        {
            BeforeConvertors.Add(convertor);
        }

        /// <summary>
        /// 主体内容
        /// </summary>
        protected virtual string Content => _content;

        /// <summary>
        /// 转换后的主体内容
        /// </summary>
        internal string FullContent
        {
            get
            {
                string s = Content;

                foreach (Func<string, string> convert in BeforeConvertors)
                {
                    if (convert is null)
                        continue;

                    s = convert(s);
                }

                return s;
            }
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        private Description ObjectClone()
        {
            Description description = MemberwiseClone() as Description;
            description.BeforeConvertors = new(BeforeConvertors);

            return description;
        }

        /// <summary>
        /// 获取下一个描述
        /// </summary>
        /// <param name="convertor"></param>
        /// <returns></returns>
        public Description Next(Func<string, string> convertor)
        {
            Description description = ObjectClone();

            description.AddBeforeConvertor(convertor);

            return description;
        }

        /// <summary>
        /// 加
        /// </summary>
        /// <param name="description"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Description operator +(Description description, object value)
        {
            return description?.Next((s) => s + " + " + value);
        }

        /// <summary>
        /// 减
        /// </summary>
        /// <param name="description"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Description operator -(Description description, object value)
        {
            return description?.Next((s) => s + " - " + value);
        }

        /// <summary>
        /// 乘
        /// </summary>
        /// <param name="description"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Description operator *(Description description, object value)
        {
            return description?.Next((s) => s + " * " + value);
        }

        /// <summary>
        /// 除
        /// </summary>
        /// <param name="description"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Description operator /(Description description, object value)
        {
            return description?.Next((s) => s + " / " + value);
        }

        /// <summary>
        /// 相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Condition operator ==(Description left, object right)
        {
            return new Condition(left, Comparator.Equal, right);
        }

        /// <summary>
        /// 不等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Condition operator !=(Description left, object right)
        {
            return new Condition(left, Comparator.NotEqual, right);
        }

        /// <summary>
        /// 大于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Condition operator >(Description left, object right)
        {
            return new Condition(left, Comparator.GreaterThan, right);
        }

        /// <summary>
        /// 小于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Condition operator <(Description left, object right)
        {
            return new Condition(left, Comparator.LessThan, right);
        }

        /// <summary>
        /// 大于等于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Condition operator >=(Description left, object right)
        {
            return new Condition(left, Comparator.GreaterThanOrEqual, right);
        }

        /// <summary>
        /// 小于等于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Condition operator <=(Description left, object right)
        {
            return new Condition(left, Comparator.LessThanOrEqual, right);
        }

        /// <summary>
        /// 枚举查找
        /// </summary>
        /// <param name="vs"></param>
        /// <returns></returns>
        public Condition In(IEnumerable vs)
        {
            return new Condition(this, Comparator.In, vs);
        }

        /// <summary>
        /// 枚举查找
        /// </summary>
        /// <param name="vs"></param>
        /// <returns></returns>
        public Condition In(params object[] vs)
        {
            return new Condition(this, Comparator.In, vs);
        }

        /// <summary>
        /// 相似
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public Condition Like(string str)
        {
            return new Condition(this, Comparator.Like, str);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
