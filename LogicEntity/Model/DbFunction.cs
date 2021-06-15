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
            return description?.Next(s => $"ASCII({s})");
        }

        /// <summary>
        /// 字符串的字符数
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description CharLength(this Description description)
        {
            return description?.Next(s => $"Char_Length({s})");
        }

        /// <summary>
        /// 字符串的字符数
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description CharacterLength(this Description description)
        {
            return description?.Next(s => $"Character_Length({s})");
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
                List<string> vs = new() { s };

                if (more is not null)
                    vs.AddRange(more.Select(m => m.FullContent));

                return $"Concat({string.Join(", ", vs)})";
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
                List<string> vs = new() { s };

                if (more is not null)
                    vs.AddRange(more);

                return $"Concat({string.Join(", ", vs)})";
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
                List<string> vs = new() { s };

                if (more is not null)
                    vs.AddRange(more.Select(m => m.FullContent));

                return $"Concat_Ws({separator}, {string.Join(", ", vs)})";
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
                List<string> vs = new() { s };

                if (more is not null)
                    vs.AddRange(more);

                return $"Concat_Ws({separator}, {string.Join(", ", vs)})";
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
                List<string> vs = new() { s };

                if (more is not null)
                    vs.AddRange(more.Select(m => m.FullContent));

                return $"Field({string.Join(", ", vs)})";
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
                List<string> vs = new() { s };

                if (more is not null)
                    vs.AddRange(more);

                return $"Field({string.Join(", ", vs)})";
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
            return description?.Next(s => $"Find_In_Set({s}, {strList})");
        }

        /// <summary>
        /// 当前字符串在字符串列表中的位置
        /// </summary>
        /// <param name="description">当前字符串</param>
        /// <param name="strList">字符串列表（以 , 分隔）</param>
        /// <returns></returns>
        public static Description FindInSet(this Description description, string strList)
        {
            return description?.Next(s => $"Find_In_Set({s}, {strList})");
        }

        /// <summary>
        /// 将数字格式化为 '##,###.##' 的形式
        /// </summary>
        /// <param name="description">数字</param>
        /// <param name="digits">保留的小数位数</param>
        /// <returns></returns>
        public static Description Format(this Description description, int digits)
        {
            return description?.Next(s => $"Format({s}, {digits})");
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
            return description?.Next(s => $"Insert({s}, {start}, {length}, {replace})");
        }

        /// <summary>
        /// 在字符串中的开始位置
        /// </summary>
        /// <param name="description"></param>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static Description Locate(this Description description, Description str)
        {
            return description?.Next(s => $"Locate({s}, {str})");
        }

        /// <summary>
        /// 在字符串中的开始位置
        /// </summary>
        /// <param name="description"></param>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static Description Locate(this Description description, string str)
        {
            return description?.Next(s => $"Locate({s}, {str})");
        }

        /// <summary>
        /// 将字母转换为小写字母
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Lcase(this Description description)
        {
            return description?.Next(s => $"Lcase({s})");
        }

        /// <summary>
        /// 将字母转换为大写字母
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Ucase(this Description description)
        {
            return description?.Next(s => $"Ucase({s})");
        }

        /// <summary>
        /// 字符串左侧的字符
        /// </summary>
        /// <param name="description">字符串</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static Description Left(this Description description, int length)
        {
            return description?.Next(s => $"Left({s}, {length})");
        }

        /// <summary>
        /// 字符串右侧的字符
        /// </summary>
        /// <param name="description"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Description Right(this Description description, int length)
        {
            return description?.Next(s => $"Right({s}, {length})");
        }

        /// <summary>
        /// 将字母转换为小写字母
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Lower(this Description description)
        {
            return description?.Next(s => $"Lower({s})");
        }

        /// <summary>
        /// 将字母转换为大写字母
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Upper(this Description description)
        {
            return description?.Next(s => $"Upper({s})");
        }

        /// <summary>
        /// 在字符串左侧填充字符串达到总长度
        /// </summary>
        /// <param name="description">字符串</param>
        /// <param name="length">总长度</param>
        /// <param name="str">用于填充的字符串</param>
        /// <returns></returns>
        public static Description LPad(this Description description, int length, Description str)
        {
            return description?.Next(s => $"LPad({s}, {length}, {str})");
        }

        /// <summary>
        /// 在字符串左侧填充字符串达到总长度
        /// </summary>
        /// <param name="description">字符串</param>
        /// <param name="length">总长度</param>
        /// <param name="str">用于填充的字符串</param>
        /// <returns></returns>
        public static Description LPad(this Description description, int length, string str)
        {
            return description?.Next(s => $"LPad({s}, {length}, {str})");
        }

        /// <summary>
        /// 在字符串右侧填充字符串达到总长度
        /// </summary>
        /// <param name="description">字符串</param>
        /// <param name="length">总长度</param>
        /// <param name="str">用于填充的字符串</param>
        /// <returns></returns>
        public static Description RPad(this Description description, int length, Description str)
        {
            return description?.Next(s => $"RPad({s}, {length}, {str})");
        }

        /// <summary>
        /// 在字符串右侧填充字符串达到总长度
        /// </summary>
        /// <param name="description">字符串</param>
        /// <param name="length">总长度</param>
        /// <param name="str">用于填充的字符串</param>
        /// <returns></returns>
        public static Description RPad(this Description description, int length, string str)
        {
            return description?.Next(s => $"RPad({s}, {length}, {str})");
        }

        /// <summary>
        /// 去掉字符串左侧空格
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description LTrim(this Description description)
        {
            return description?.Next(s => $"LTrim({s})");
        }

        /// <summary>
        /// 去掉字符串右侧空格
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description RTrim(this Description description)
        {
            return description?.Next(s => $"RTrim({s})");
        }

        /// <summary>
        /// 去掉字符串两侧空格
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Trim(this Description description)
        {
            return description?.Next(s => $"Trim({s})");
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="description">字符串</param>
        /// <param name="start">开始位置</param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        public static Description Mid(this Description description, int start, int length)
        {
            return description?.Next(s => $"Mid({s}, {start}, {length})");
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="description">字符串</param>
        /// <param name="start">开始位置</param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        public static Description SubStr(this Description description, int start, int length)
        {
            return description?.Next(s => $"SubStr({s}, {start}, {length})");
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="description">字符串</param>
        /// <param name="start">开始位置</param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        public static Description SubString(this Description description, int start, int length)
        {
            return description?.Next(s => $"SubString({s}, {start}, {length})");
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="description">字符串</param>
        /// <param name="separator">分隔符</param>
        /// <param name="num">分隔符序号</param>
        /// <returns></returns>
        public static Description SubStringIndex(this Description description, Description separator, int num)
        {
            return description?.Next(s => $"SubString_Index({s}, {separator}, {num})");
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="description">字符串</param>
        /// <param name="separator">分隔符</param>
        /// <param name="num">分隔符序号</param>
        /// <returns></returns>
        public static Description SubStringIndex(this Description description, string separator, int num)
        {
            return description?.Next(s => $"SubString_Index({s}, {separator}, {num})");
        }

        /// <summary>
        /// 字符串1 在 字符串2 中的位置
        /// </summary>
        /// <param name="description">字符串1</param>
        /// <param name="str">字符串2</param>
        /// <returns></returns>
        public static Description Position(this Description description, Description str)
        {
            return description?.Next(s => $"Position({s} In {str})");
        }

        /// <summary>
        /// 字符串1 在 字符串2 中的位置
        /// </summary>
        /// <param name="description">字符串1</param>
        /// <param name="str">字符串2</param>
        /// <returns></returns>
        public static Description Position(this Description description, string str)
        {
            return description?.Next(s => $"Position({s} In {str})");
        }

        /// <summary>
        /// 重复字符串
        /// </summary>
        /// <param name="description">字符串</param>
        /// <param name="times">重复次数</param>
        /// <returns></returns>
        public static Description Repeat(this Description description, int times)
        {
            return description?.Next(s => $"Repeat({s}, {times})");
        }

        /// <summary>
        /// 替换字符串
        /// </summary>
        /// <param name="description">字符串</param>
        /// <param name="original">旧字符串</param>
        /// <param name="replace">新字符串</param>
        /// <returns></returns>
        public static Description Replace(this Description description, Description original, Description replace)
        {
            return description?.Next(s => $"Replace({s}, {original}, {replace})");
        }

        /// <summary>
        /// 替换字符串
        /// </summary>
        /// <param name="description">字符串</param>
        /// <param name="original">旧字符串</param>
        /// <param name="replace">新字符串</param>
        /// <returns></returns>
        public static Description Replace(this Description description, Description original, string replace)
        {
            return description?.Next(s => $"Replace({s}, {original}, {replace})");
        }

        /// <summary>
        /// 替换字符串
        /// </summary>
        /// <param name="description">字符串</param>
        /// <param name="original">旧字符串</param>
        /// <param name="replace">新字符串</param>
        /// <returns></returns>
        public static Description Replace(this Description description, string original, Description replace)
        {
            return description?.Next(s => $"Replace({s}, {original}, {replace})");
        }

        /// <summary>
        /// 替换字符串
        /// </summary>
        /// <param name="description">字符串</param>
        /// <param name="original">旧字符串</param>
        /// <param name="replace">新字符串</param>
        /// <returns></returns>
        public static Description Replace(this Description description, string original, string replace)
        {
            return description?.Next(s => $"Replace({s}, {original}, {replace})");
        }

        /// <summary>
        /// 反转字符串
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Reverse(this Description description)
        {
            return description?.Next(s => $"Reverse({s})");
        }

        /// <summary>
        /// 返回空格
        /// </summary>
        /// <param name="count">空格的数量</param>
        /// <returns></returns>
        public static Description Space(int count)
        {
            return new Description($"Space({count})");
        }

        /// <summary>
        /// 比较字符串
        /// </summary>
        /// <param name="left">左值</param>
        /// <param name="right">右值</param>
        /// <returns></returns>
        public static Description Strcmp(this Description left, Description right)
        {
            return left?.Next(s => $"Strcmp({s}, {right})");
        }

        /// <summary>
        /// 绝对值
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Abs(this Description description)
        {
            return description?.Next(s => $"Abs({s})");
        }

        /// <summary>
        /// 余弦
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Sin(this Description description)
        {
            return description?.Next(s => $"Sin({s})");
        }

        /// <summary>
        /// 余弦
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Cos(this Description description)
        {
            return description?.Next(s => $"Cos({s})");
        }

        /// <summary>
        /// 余切
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Cot(this Description description)
        {
            return description?.Next(s => $"Cot({s})");
        }

        /// <summary>
        /// 反正弦
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Asin(this Description description)
        {
            return description?.Next(s => $"Asin({s})");
        }

        /// <summary>
        /// 反余弦
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Acos(this Description description)
        {
            return description?.Next(s => $"Acos({s})");
        }

        /// <summary>
        /// 反正切
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Atan(this Description description)
        {
            return description?.Next(s => $"Atan({s})");
        }

        /// <summary>
        /// 反正切
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Atan2(this Description x, Description y)
        {
            return x?.Next(s => $"Atan2({s}, {y})");
        }

        /// <summary>
        /// 反正切
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Atan2(this Description x, double y)
        {
            return x?.Next(s => $"Atan2({s}, {y})");
        }

        /// <summary>
        /// 平均值
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Avg(this Description description)
        {
            return description?.Next(s => $"Avg({s})");
        }

        /// <summary>
        /// 向上取整
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Ceil(this Description description)
        {
            return description?.Next(s => $"Ceil({s})");
        }

        /// <summary>
        /// 向上取整
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Ceiling(this Description description)
        {
            return description?.Next(s => $"Ceiling({s})");
        }

        /// <summary>
        /// 向下取整
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Floor(this Description description)
        {
            return description?.Next(s => $"Floor({s})");
        }

        /// <summary>
        /// 总数
        /// </summary>
        /// <returns></returns>
        public static Description Count()
        {
            return new Description("Count(*)");
        }

        /// <summary>
        /// 总数
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static Description Count(int i)
        {
            return new Description($"Count({i})");
        }

        /// <summary>
        /// 总数
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Count(this Description description)
        {
            return description?.Next(s => $"Count({s})");
        }

        /// <summary>
        /// 角度
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Degrees(this Description description)
        {
            return description?.Next(s => $"Degrees({s})");
        }

        /// <summary>
        /// 角度
        /// </summary>
        /// <param name="rad"></param>
        /// <returns></returns>
        public static Description Degrees(double rad)
        {
            return new Description($"Degrees({rad})");
        }

        /// <summary>
        /// 整除
        /// </summary>
        /// <param name="description"></param>
        /// <param name="divider"></param>
        /// <returns></returns>
        public static Description Div(this Description description, double divider)
        {
            return description?.Next(s => $"{s} Div {divider}");
        }

        /// <summary>
        /// E的幂
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Description Exp(double index)
        {
            return new Description($"Exp({index})");
        }

        /// <summary>
        /// 最大值
        /// </summary>
        /// <param name="description"></param>
        /// <param name="more"></param>
        /// <returns></returns>
        public static Description Greatest(this Description description, params Description[] more)
        {
            return description?.Next(s =>
            {
                List<string> vs = new() { s };

                if (more is not null)
                    vs.AddRange(more.Select(m => m.FullContent));

                return $"Greatest({string.Join(", ", vs)})";
            });
        }

        /// <summary>
        /// 最小值
        /// </summary>
        /// <param name="description"></param>
        /// <param name="more"></param>
        /// <returns></returns>
        public static Description Least(this Description description, params Description[] more)
        {
            return description?.Next(s =>
            {
                List<string> vs = new() { s };

                if (more is not null)
                    vs.AddRange(more.Select(m => m.FullContent));

                return $"Least({string.Join(", ", vs)})";
            });
        }

        /// <summary>
        /// 最大值
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Max(this Description description)
        {
            return description?.Next(s => $"Max({s})");
        }

        /// <summary>
        /// 最小值
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Min(this Description description)
        {
            return description?.Next(s => $"Min({s})");
        }

        public static Description LastInsertId()
        {
            return new Description("Last_Insert_Id()");
        }


        //自定义

        public static Description Distinct(this Description description)
        {
            return description?.Next(s => "Distinct " + s);
        }

        public static Description As(this Description description, string alias)
        {
            return description?.Next(s => s + " As " + alias);
        }
    }
}
