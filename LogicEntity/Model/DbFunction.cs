using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Operator;

namespace LogicEntity.Model
{
    /// <summary>
    /// 数据库函数
    /// </summary>
    public static class DbFunction
    {
        public static Description Distinct(this Description description)
        {
            return description?.Next(s => "Distinct " + s);
        }

        public static Description As(this Description description, string alias)
        {
            return description?.Next(s => s + " As " + alias);
        }

        public static Description Count()
        {
            return new Description("Count(*)");
        }

        public static Description Count(int i)
        {
            return new Description("Count(" + i.ToString() + ")");
        }

        public static Description Count(this Description description)
        {
            return description?.Next(s => "Count(" + s + ")");
        }

        public static Description Max(this Description description)
        {
            return description?.Next(s => "Max(" + s + ")");
        }

        public static Description Min(this Description description)
        {
            return description?.Next(s => "Min(" + s + ")");
        }
    }
}
