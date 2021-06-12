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
    public static partial class DbFunction
    {
        /// <summary>
        /// 第一个字符的ASCII码
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description ASCII(this Description description)
        {
            return description?.Next(s => "ASCII(" + s + ")");
        }

        /// <summary>
        /// 字符串的字符数
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description CharLength(this Description description)
        {
            return description?.Next(s => "Char_Length(" + s + ")");
        }

        /// <summary>
        /// 字符串的字符数
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description CharacterLength(this Description description)
        {
            return description?.Next(s => "Character_Length(" + s + ")");
        }

        /// <summary>
        /// 合并字符串
        /// </summary>
        /// <param name="description">第一个字符</param>
        /// <param name="more">后续的字符</param>
        /// <returns></returns>
        public static Description Concat(this Description description, params Description[] more)
        {
            return description?.Next(s =>
            {
                string vs = string.Empty;

                if (more is not null && more.Any())
                    vs = ", " + string.Join(", ", more.Select(m => m.FullContent));

                return "Concat(" + s + vs + ")";
            });
        }

        /// <summary>
        /// 合并字符串
        /// </summary>
        /// <param name="description">第一个字符</param>
        /// <param name="more">后续的字符</param>
        /// <returns></returns>
        public static Description Concat(this Description description, params string[] more)
        {
            return description?.Next(s =>
            {
                string vs = string.Empty;

                if (more is not null && more.Any())
                    vs = ", " + string.Join<string>(", ", more);

                return "Concat(" + s + vs + ")";
            });
        }

        /// <summary>
        /// 合并字符串
        /// </summary>
        /// <param name="description">第一个字符</param>
        /// <param name="separator">分隔符</param>
        /// <param name="more">后续的字符</param>
        /// <returns></returns>
        public static Description ConcatWS(this Description description, string separator, params Description[] more)
        {
            return description?.Next(s =>
            {
                string vs = string.Empty;

                if (more is not null && more.Any())
                    vs = ", " + string.Join(", ", more.Select(m => m.FullContent));

                return "Concat_Ws(" + separator + ", " + s + vs + ")";
            });
        }

        /// <summary>
        /// 合并字符串
        /// </summary>
        /// <param name="description">第一个字符</param>
        /// <param name="separator">分隔符</param>
        /// <param name="more">后续的字符</param>
        /// <returns></returns>
        public static Description ConcatWS(this Description description, string separator, params string[] more)
        {
            return description?.Next(s =>
            {
                string vs = string.Empty;

                if (more is not null && more.Any())
                    vs = ", " + string.Join<string>(", ", more);

                return "Concat_Ws(" + separator + ", " + s + vs + ")";
            });
        }

        /// <summary>
        /// 返回当前字符串在字符串列表中的位置
        /// </summary>
        /// <param name="description">当前字符串</param>
        /// <param name="more">字符串列表</param>
        /// <returns></returns>
        public static Description Field(this Description description, params Description[] more)
        {
            return description?.Next(s =>
            {
                string vs = string.Empty;

                if (more is not null && more.Any())
                    vs = ", " + string.Join(", ", more.Select(m => m.FullContent));

                return "Field(" + s + vs + ")";
            });
        }

        /// <summary>
        /// 当前字符串在字符串列表中的位置
        /// </summary>
        /// <param name="description">当前字符串</param>
        /// <param name="more">字符串列表</param>
        /// <returns></returns>
        public static Description Field(this Description description, params string[] more)
        {
            return description?.Next(s =>
            {
                string vs = string.Empty;

                if (more is not null && more.Any())
                    vs = ", " + string.Join<string>(", ", more);

                return "Field(" + s + vs + ")";
            });
        }

        /// <summary>
        /// 当前字符串在字符串列表中的位置
        /// </summary>
        /// <param name="description">当前字符串</param>
        /// <param name="strList">字符串列表（以 , 分隔）</param>
        /// <returns></returns>
        public static Description FindInSet(this Description description, Description strList)
        {
            return description?.Next(s => "Find_In_Set(" + s + ", " + strList.FullContent + ")");
        }

        /// <summary>
        /// 当前字符串在字符串列表中的位置
        /// </summary>
        /// <param name="description">当前字符串</param>
        /// <param name="strList">字符串列表（以 , 分隔）</param>
        /// <returns></returns>
        public static Description FindInSet(this Description description, string strList)
        {
            return description?.Next(s => "Find_In_Set(" + s + ", " + strList + ")");
        }

        /// <summary>
        /// 将数字格式化为 '##,###.##' 的形式
        /// </summary>
        /// <param name="description">数字</param>
        /// <param name="digits">保留的小数位数</param>
        /// <returns></returns>
        public static Description Format(this Description description, int digits)
        {
            return description?.Next(s => "Format(" + s + ", " + digits + ")");
        }

        /// <summary>
        /// 替换字符串中的一部分
        /// </summary>
        /// <param name="description">原始字符串</param>
        /// <param name="start">开始位置</param>
        /// <param name="length">替换长度</param>
        /// <param name="replace">字符串</param>
        /// <returns></returns>
        public static Description Insert(this Description description, int start, int length, string replace)
        {
            return description?.Next(s => "Insert(" + s + ", " + start + ", " + length + ", " + replace + ")");
        }

        /// <summary>
        /// 在字符串中的开始位置
        /// </summary>
        /// <param name="description"></param>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static Description Locate(this Description description, Description str)
        {
            return description?.Next(s => "Locate(" + s + ", " + str + ")");
        }

        /// <summary>
        /// 在字符串中的开始位置
        /// </summary>
        /// <param name="description"></param>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static Description Locate(this Description description, string str)
        {
            return description?.Next(s => "Locate(" + s + ", " + str + ")");
        }

        /// <summary>
        /// 将字母转换为小写字母
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Lcase(this Description description)
        {
            return description?.Next(s => "Lcase(" + s + ")");
        }

        /// <summary>
        /// 字符串的前几个字符
        /// </summary>
        /// <param name="description">字符串</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static Description Left(this Description description, int length)
        {
            return description?.Next(s => "LEFT(" + s + ", " + length + ")");
        }

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

        public static Description LastInsertId()
        {
            return new Description("Last_Insert_Id()");
        }
    }
}
