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
            if (description is null)
                return null;

            Description newDescription = description.ObjectClone();
            newDescription.AddBeforeConvertor(s => "Distinct " + s);

            return newDescription;
        }

        public static Description As(this Description description, string alias)
        {
            if (description is null)
                return null;

            Description newDescription = description.ObjectClone();
            newDescription.AddBeforeConvertor(s => s + " As " + alias);

            return newDescription;
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
            if (description is null)
                return null;

            Description newDescription = description.ObjectClone();
            newDescription.AddBeforeConvertor(s => "Count(" + s + ")");

            return newDescription;
        }

        public static Description Max(this Description description)
        {
            if (description is null)
                return null;

            Description newDescription = description.ObjectClone();
            newDescription.AddBeforeConvertor(s => "Max(" + s + ")");

            return newDescription;
        }

        public static Description Min(this Description description)
        {
            if (description is null)
                return null;

            Description newDescription = description.ObjectClone();
            newDescription.AddBeforeConvertor(s => "Min(" + s + ")");

            return newDescription;
        }
    }
}
