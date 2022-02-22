using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Tool;
using LogicEntity.Operator;

namespace LogicEntity.Model
{
    /// <summary>
    /// 数据库函数
    /// </summary>
    public static class DbFunction
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
        public static Description Char_Length(this Description description)
        {
            return description?.Next(s => $"Char_Length({s})");
        }

        /// <summary>
        /// 字符串的字符数
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Character_Length(this Description description)
        {
            return description?.Next(s => $"Character_Length({s})");
        }

        /// <summary>
        /// 合并字符串
        /// </summary>
        /// <param name="description">第一个字符</param>
        /// <param name="more">后续的字符</param>
        /// <returns></returns>
        public static Description Concat(this Description description, params object[] more)
        {
            return description?.Next(s =>
            {
                List<string> vs = new() { s };

                if (more is not null)
                    vs.AddRange(more.Select(m => m.ToSqlParam()));

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
        public static Description Concat_Ws(this Description description, object separator, params object[] more)
        {
            return description?.Next(s =>
            {
                List<string> vs = new() { s };

                if (more is not null)
                    vs.AddRange(more.Select(m => m.ToSqlParam()));

                return $"Concat_Ws({separator.ToSqlParam()}, {string.Join(", ", vs)})";
            });
        }

        /// <summary>
        /// 合并字符串
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Group_Concat(this Description description)
        {
            return description?.Next(s => $"Group_Concat({s})");
        }

        /// <summary>
        /// 返回当前字符串在字符串列表中的位置
        /// </summary>
        /// <param name="description">当前字符串</param>
        /// <param name="more">字符串列表</param>
        /// <returns></returns>
        public static Description Field(this Description description, params object[] more)
        {
            return description?.Next(s =>
            {
                List<string> vs = new() { s };

                if (more is not null)
                    vs.AddRange(more.Select(m => m.ToSqlParam()));

                return $"Field({string.Join(", ", vs)})";
            });
        }

        /// <summary>
        /// 当前字符串在字符串列表中的位置
        /// </summary>
        /// <param name="description">当前字符串</param>
        /// <param name="strList">字符串列表（以 , 分隔）</param>
        /// <returns></returns>
        public static Description Find_In_Set(this Description description, object strList)
        {
            return description?.Next(s => $"Find_In_Set({s}, {strList.ToSqlParam()})");
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
        public static Description Insert(this Description description, int start, int length, object replace)
        {
            return description?.Next(s => $"Insert({s}, {start}, {length}, {replace.ToSqlParam()})");
        }

        /// <summary>
        /// 在字符串中的开始位置
        /// </summary>
        /// <param name="description"></param>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static Description Locate(this Description description, object str)
        {
            return description?.Next(s => $"Locate({s}, {str.ToSqlParam()})");
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
        public static Description LPad(this Description description, int length, object str)
        {
            return description?.Next(s => $"LPad({s}, {length}, {str.ToSqlParam()})");
        }

        /// <summary>
        /// 在字符串右侧填充字符串达到总长度
        /// </summary>
        /// <param name="description">字符串</param>
        /// <param name="length">总长度</param>
        /// <param name="str">用于填充的字符串</param>
        /// <returns></returns>
        public static Description RPad(this Description description, int length, object str)
        {
            return description?.Next(s => $"RPad({s}, {length}, {str.ToSqlParam()})");
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
        public static Description SubString_Index(this Description description, object separator, int num)
        {
            return description?.Next(s => $"SubString_Index({s}, {separator.ToSqlParam()}, {num})");
        }

        /// <summary>
        /// 字符串1 在 字符串2 中的位置
        /// </summary>
        /// <param name="description">字符串1</param>
        /// <param name="str">字符串2</param>
        /// <returns></returns>
        public static Description Position(this Description description, object str)
        {
            return description?.Next(s => $"Position({s} In {str.ToSqlParam()})");
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
        public static Description Replace(this Description description, object original, object replace)
        {
            return description?.Next(s => $"Replace({s}, {original.ToSqlParam()}, {replace.ToSqlParam()})");
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
        /// 正弦
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
        /// 正切
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Tan(this Description description)
        {
            return description?.Next(s => $"Tan({s})");
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
        /// 弧度
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Radians(this Description description)
        {
            return description?.Next(s => $"Radians({s})");
        }

        /// <summary>
        /// 弧度
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>
        public static Description Radians(double deg)
        {
            return new Description($"Radians({deg})");
        }

        /// <summary>
        /// 整除
        /// </summary>
        /// <param name="divided"></param>
        /// <param name="divider"></param>
        /// <returns></returns>
        public static Description Div(this Description divided, Description divider)
        {
            return divided?.Next(s => $"{s} Div {divider}");
        }

        /// <summary>
        /// 整除
        /// </summary>
        /// <param name="divided"></param>
        /// <param name="divider"></param>
        /// <returns></returns>
        public static Description Div(this Description divided, double divider)
        {
            return divided?.Next(s => $"{s} Div {divider}");
        }

        /// <summary>
        /// 整除
        /// </summary>
        /// <param name="divided"></param>
        /// <param name="divider"></param>
        /// <returns></returns>
        public static Description Div(double divided, Description divider)
        {
            return new Description($"{divided} Div {divider}");
        }

        /// <summary>
        /// 整除
        /// </summary>
        /// <param name="divided"></param>
        /// <param name="divider"></param>
        /// <returns></returns>
        public static Description Div(double divided, double divider)
        {
            return new Description($"{divided} Div {divider}");
        }

        /// <summary>
        /// 取余
        /// </summary>
        /// <param name="divided"></param>
        /// <param name="divider"></param>
        /// <returns></returns>
        public static Description Mod(this Description divided, Description divider)
        {
            return divided?.Next(s => $"Mod({s}, {divider})");
        }

        /// <summary>
        /// 取余
        /// </summary>
        /// <param name="divided"></param>
        /// <param name="divider"></param>
        /// <returns></returns>
        public static Description Mod(this Description divided, double divider)
        {
            return divided?.Next(s => $"Mod({s}, {divider})");
        }

        /// <summary>
        /// 取余
        /// </summary>
        /// <param name="divided"></param>
        /// <param name="divider"></param>
        /// <returns></returns>
        public static Description Mod(double divided, Description divider)
        {
            return new Description($"Mod({divided}, {divider})");
        }

        /// <summary>
        /// 取余
        /// </summary>
        /// <param name="divided"></param>
        /// <param name="divider"></param>
        /// <returns></returns>
        public static Description Mod(double divided, double divider)
        {
            return new Description($"Mod({divided}, {divider})");
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
                    vs.AddRange(more.Select(m => m.ToString()));

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
                    vs.AddRange(more.Select(m => m.ToString()));

                return $"Least({string.Join(", ", vs)})";
            });
        }

        /// <summary>
        /// e 的对数
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Ln(this Description description)
        {
            return description?.Next(s => $"Ln({s})");
        }

        /// <summary>
        /// e 的对数
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static Description Ln(double val)
        {
            return new Description($"Ln({val})");
        }

        /// <summary>
        /// 对数
        /// </summary>
        /// <param name="baseNum">底数</param>
        /// <param name="power"></param>
        /// <returns></returns>
        public static Description Log(Description baseNum, Description power)
        {
            return new Description($"Log({baseNum}, {power})");
        }

        /// <summary>
        /// 对数
        /// </summary>
        /// <param name="baseNum">底数</param>
        /// <param name="power"></param>
        /// <returns></returns>
        public static Description Log(Description baseNum, double power)
        {
            return new Description($"Log({baseNum}, {power})");
        }

        /// <summary>
        /// 对数
        /// </summary>
        /// <param name="baseNum">底数</param>
        /// <param name="power"></param>
        /// <returns></returns>
        public static Description Log(double baseNum, Description power)
        {
            return new Description($"Log({baseNum}, {power})");
        }

        /// <summary>
        /// 对数
        /// </summary>
        /// <param name="baseNum">底数</param>
        /// <param name="power"></param>
        /// <returns></returns>
        public static Description Log(double baseNum, double power)
        {
            return new Description($"Log({baseNum}, {power})");
        }

        /// <summary>
        /// 10 的对数
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Log10(this Description description)
        {
            return description?.Next(s => $"Log10({s})");
        }

        /// <summary>
        /// 10 的对数
        /// </summary>
        /// <param name="power"></param>
        /// <returns></returns>
        public static Description Log10(double power)
        {
            return new Description($"Log10({power})");
        }

        /// <summary>
        /// 2 的对数
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Log2(this Description description)
        {
            return description?.Next(s => $"Log2({s})");
        }

        /// <summary>
        /// 2 的对数
        /// </summary>
        /// <param name="power"></param>
        /// <returns></returns>
        public static Description Log2(double power)
        {
            return new Description($"Log2({power})");
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

        /// <summary>
        /// 圆周率
        /// </summary>
        /// <returns></returns>
        public static Description PI()
        {
            return new Description("PI()");
        }

        /// <summary>
        /// 幂
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Description Pow(this Description x, Description y)
        {
            return x.Next(s => $"Pow({s}, {y})");
        }

        /// <summary>
        /// 幂
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Description Pow(this Description x, double y)
        {
            return x.Next(s => $"Pow({s}, {y})");
        }

        /// <summary>
        /// 幂
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Description Pow(double x, Description y)
        {
            return new Description($"Pow({x}, {y})");
        }

        /// <summary>
        /// 幂
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Description Pow(double x, double y)
        {
            return new Description($"Pow({x}, {y})");
        }

        /// <summary>
        /// 幂
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Description Power(this Description x, Description y)
        {
            return x.Next(s => $"Power({s}, {y})");
        }

        /// <summary>
        /// 幂
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Description Power(this Description x, double y)
        {
            return x.Next(s => $"Power({s}, {y})");
        }

        /// <summary>
        /// 幂
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Description Power(double x, Description y)
        {
            return new Description($"Power({x}, {y})");
        }

        /// <summary>
        /// 幂
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Description Power(double x, double y)
        {
            return new Description($"Power({x}, {y})");
        }

        /// <summary>
        /// 随机数
        /// </summary>
        /// <returns></returns>
        public static Description Rand()
        {
            return new Description("Rand()");
        }

        /// <summary>
        /// 取整
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Round(this Description description)
        {
            return description?.Next(s => $"Round({s})");
        }

        /// <summary>
        /// 正数、0 或 负数
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Sign(this Description description)
        {
            return description?.Next(s => $"Sign({s})");
        }

        /// <summary>
        /// 算术平方根
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Sqrt(this Description description)
        {
            return description?.Next(s => $"Sqrt({s})");
        }

        /// <summary>
        /// 求和
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Sum(this Description description)
        {
            return description?.Next(s => $"Sum({s})");
        }

        /// <summary>
        /// 保留小数
        /// </summary>
        /// <param name="description"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static Description Truncate(this Description description, int digits)
        {
            return description?.Next(s => $"Truncate({s}, {digits})");
        }

        /// <summary>
        /// 加年
        /// </summary>
        /// <param name="description"></param>
        /// <param name="years"></param>
        /// <returns></returns>
        public static Description AddYears(this Description description, int years)
        {
            return description?.Next(s => $"AddDate({s}, Interval {years} Year)");
        }

        /// <summary>
        /// 加月
        /// </summary>
        /// <param name="description"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public static Description AddMonths(this Description description, int months)
        {
            return description?.Next(s => $"AddDate({s}, Interval {months} Month)");
        }

        /// <summary>
        /// 加日
        /// </summary>
        /// <param name="description"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public static Description AddDays(this Description description, int days)
        {
            return description?.Next(s => $"AddDate({s}, Interval {days} Day)");
        }

        /// <summary>
        /// 加小时
        /// </summary>
        /// <param name="description"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static Description AddHours(this Description description, int hours)
        {
            return description?.Next(s => $"AddDate({s}, Interval {hours} Hour)");
        }

        /// <summary>
        /// 加分钟
        /// </summary>
        /// <param name="description"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static Description AddMinutes(this Description description, int minutes)
        {
            return description?.Next(s => $"AddDate({s}, Interval {minutes} Minute)");
        }

        /// <summary>
        /// 加秒
        /// </summary>
        /// <param name="description"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static Description AddSeconds(this Description description, int seconds)
        {
            return description?.Next(s => $"AddDate({s}, Interval {seconds} Second)");
        }

        /// <summary>
        /// 加时间
        /// </summary>
        /// <param name="description"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static Description AddTime(this Description description, string time)
        {
            return description?.Next(s => $"AddTime({s}, '{time}')");
        }

        /// <summary>
        /// 加时间
        /// </summary>
        /// <param name="description"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static Description AddTime(this Description description, TimeSpan time)
        {
            return description?.Next(s => $"AddTime({s}, '{time}')");
        }

        /// <summary>
        /// 当前日期
        /// </summary>
        /// <returns></returns>
        public static Description CurDate()
        {
            return new Description("CurDate()");
        }

        /// <summary>
        /// 当前日期
        /// </summary>
        /// <returns></returns>
        public static Description Current_Date()
        {
            return new Description("Current_Date()");
        }

        /// <summary>
        /// 当前时间
        /// </summary>
        /// <returns></returns>
        public static Description CurTime()
        {
            return new Description("CurTime()");
        }

        /// <summary>
        /// 当前时间
        /// </summary>
        /// <returns></returns>
        public static Description Current_Time()
        {
            return new Description("Current_Time()");
        }

        /// <summary>
        /// 当前日期和时间
        /// </summary>
        /// <returns></returns>
        public static Description Current_TimeStamp()
        {
            return new Description("Current_TimeStamp()");
        }

        /// <summary>
        /// 当前日期和时间
        /// </summary>
        /// <returns></returns>
        public static Description LocalTime()
        {
            return new Description("LocalTime()");
        }

        /// <summary>
        /// 当前日期和时间
        /// </summary>
        /// <returns></returns>
        public static Description LocalTimeStamp()
        {
            return new Description("LocalTimeStamp()");
        }

        /// <summary>
        /// 当前日期和时间
        /// </summary>
        /// <returns></returns>
        public static Description Now()
        {
            return new Description("Now()");
        }

        /// <summary>
        /// 当前日期和时间
        /// </summary>
        /// <returns></returns>
        public static Description SysDate()
        {
            return new Description("SysDate()");
        }

        /// <summary>
        /// 取日期
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Date(this Description description)
        {
            return description?.Next(s => $"Date({s})");
        }

        /// <summary>
        /// 取时间
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Time(this Description description)
        {
            return description?.Next(s => $"Time({s})");
        }

        /// <summary>
        /// 取年
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Year(this Description description)
        {
            return description?.Next(s => $"Year({s})");
        }

        /// <summary>
        /// 取月
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Month(this Description description)
        {
            return description?.Next(s => $"Month({s})");
        }

        /// <summary>
        /// 取天
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Day(this Description description)
        {
            return description?.Next(s => $"Day({s})");
        }

        /// <summary>
        /// 取小时
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Hour(this Description description)
        {
            return description?.Next(s => $"Hour({s})");
        }

        /// <summary>
        /// 取分钟
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Minute(this Description description)
        {
            return description?.Next(s => $"Minute({s})");
        }

        /// <summary>
        /// 取秒
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Second(this Description description)
        {
            return description?.Next(s => $"Second({s})");
        }

        /// <summary>
        /// 取微秒
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description MicroSecond(this Description description)
        {
            return description?.Next(s => $"MicroSecond({s})");
        }

        /// <summary>
        /// 当前月份的最后一天
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Last_Day(this Description description)
        {
            return description?.Next(s => $"Last_Day({s})");
        }

        /// <summary>
        /// 星期几
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description WeekDay(this Description description)
        {
            return description?.Next(s => $"WeekDay({s})");
        }

        /// <summary>
        /// 星期几的名称
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description DayName(this Description description)
        {
            return description?.Next(s => $"DayName({s})");
        }

        /// <summary>
        /// 月份的名称
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description MonthName(this Description description)
        {
            return description?.Next(s => $"MonthName({s})");
        }

        /// <summary>
        /// 季度
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Quarter(this Description description)
        {
            return description?.Next(s => $"Quarter({s})");
        }

        /// <summary>
        /// 当年的第几天
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description DayOfYear(this Description description)
        {
            return description?.Next(s => $"DayOfYear({s})");
        }

        /// <summary>
        /// 当月的第几天
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description DayOfMonth(this Description description)
        {
            return description?.Next(s => $"DayOfMonth({s})");
        }

        /// <summary>
        /// 当前星期的第几天
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description DayOfWeek(this Description description)
        {
            return description?.Next(s => $"DayOfWeek({s})");
        }

        /// <summary>
        /// 当年的第几个星期
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Week(this Description description)
        {
            return description?.Next(s => $"Week({s})");
        }

        /// <summary>
        /// 当年的第几个星期
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description WeekOfYear(this Description description)
        {
            return description?.Next(s => $"WeekOfYear({s})");
        }

        /// <summary>
        /// 从 0000 年 1 月 1 日开始 n 天后的日期
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description From_Days(this Description description)
        {
            return description?.Next(s => $"From_Days({s})");
        }

        /// <summary>
        /// 从 0000 年 1 月 1 日开始 n 天后的日期
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public static Description From_Days(int days)
        {
            return new Description($"From_Days({days})");
        }

        /// <summary>
        /// 距离 0000 年 1 月 1 日的天数
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description To_Days(this Description description)
        {
            return description?.Next(s => $"To_Days({s})");
        }

        /// <summary>
        /// 根据 年 和 天数 创建日期
        /// </summary>
        /// <param name="year"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static Description MakeDate(this Description year, Description day)
        {
            return year?.Next(s => $"MakeDate({s}, {day})");
        }

        /// <summary>
        /// 根据 年 和 天数 创建日期
        /// </summary>
        /// <param name="year"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static Description MakeDate(this Description year, int day)
        {
            return year?.Next(s => $"MakeDate({s}, {day})");
        }

        /// <summary>
        /// 根据 年 和 天数 创建日期
        /// </summary>
        /// <param name="year"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static Description MakeDate(int year, Description day)
        {
            return new Description($"MakeDate({year}, {day})");
        }

        /// <summary>
        /// 根据 年 和 天数 创建日期
        /// </summary>
        /// <param name="year"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static Description MakeDate(int year, int day)
        {
            return new Description($"MakeDate({year}, {day})");
        }

        /// <summary>
        /// 根据 时、分、秒 创建时间
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Description MakeTime(int hour, int minute, int second)
        {
            return new Description($"MakeTime({hour}, {minute}, {second})");
        }

        /// <summary>
        /// 日期相减的天数
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static Description DateDiff(this Description date1, Description date2)
        {
            return date1?.Next(s => $"DateDiff({s}, {date2})");
        }

        /// <summary>
        /// 日期相减的天数
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static Description DateDiff(this Description date1, string date2)
        {
            return date1?.Next(s => $"DateDiff({s}, '{date2}')");
        }

        /// <summary>
        /// 日期相减的天数
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static Description DateDiff(this Description date1, DateTime date2)
        {
            return date1?.Next(s => $"DateDiff({s}, '{date2}')");
        }

        /// <summary>
        /// 日期格式化
        /// </summary>
        /// <param name="description"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static Description Date_Format(this Description description, string format)
        {
            return description?.Next(s => $"Date_Format({s}, '{format}')");
        }

        /// <summary>
        /// 时间格式化
        /// </summary>
        /// <param name="description"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static Description Time_Format(this Description description, string format)
        {
            return description?.Next(s => $"Time_Format({s}, '{format}')");
        }

        /// <summary>
        /// 秒 转换 时间
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static Description Sec_To_Time(this Description seconds)
        {
            return seconds?.Next(s => $"Sec_To_Time({s})");
        }

        /// <summary>
        /// 秒 转换 时间
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static Description Sec_To_Time(int seconds)
        {
            return new Description($"Sec_To_Time({seconds})");
        }

        /// <summary>
        /// 时间 转换 秒
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static Description Time_To_Sec(this Description time)
        {
            return time?.Next(s => $"Time_To_Sec({s})");
        }

        /// <summary>
        /// 时间 转换 秒
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static Description Time_To_Sec(string time)
        {
            return new Description($"Time_To_Sec('{time}')");
        }

        /// <summary>
        /// 时间差
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns></returns>
        public static Description TimeDiff(this Description time1, Description time2)
        {
            return time1.Next(s => $"TimeDiff({s}, {time2})");
        }

        /// <summary>
        /// 时间差
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns></returns>
        public static Description TimeDiff(this Description time1, string time2)
        {
            return time1.Next(s => $"TimeDiff({s}, '{time2}')");
        }

        /// <summary>
        /// 时间差
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns></returns>
        public static Description TimeDiff(this Description time1, DateTime time2)
        {
            return time1.Next(s => $"TimeDiff({s}, '{time2}')");
        }

        /// <summary>
        /// 二进制数字
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Bin(this Description description)
        {
            return description?.Next(s => $"Bin({s})");
        }

        /// <summary>
        /// 二进制字符串
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Binary(this Description description)
        {
            return description?.Next(s => $"Binary({s})");
        }

        /// <summary>
        /// Case
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Case(this Description description)
        {
            return description?.Next(s => $"Case {s}");
        }

        /// <summary>
        /// Case
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Case(object obj)
        {
            return new Description($"Case {obj}");
        }

        /// <summary>
        /// When
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description When(this Description description, object obj)
        {
            return description?.Next(s => $"{s}\n  When {obj.ToSqlParam()}");
        }

        /// <summary>
        /// Then
        /// </summary>
        /// <param name="description"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Description Then(this Description description, object obj)
        {
            return description?.Next(s => $"{s} Then {obj.ToSqlParam()}");
        }

        /// <summary>
        /// Else
        /// </summary>
        /// <param name="description"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Description Else(this Description description, object obj)
        {
            return description?.Next(s => $"{s}\n  Else {obj.ToSqlParam()}");
        }

        /// <summary>
        /// End
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description End(this Description description)
        {
            return description?.Next(s => $"{s}\nEnd");
        }

        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="description"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Description Cast(this Description description, string type)
        {
            return description?.Next(s => $"Cast({s} As {type})");
        }

        /// <summary>
        /// 转换字符串的字符集
        /// </summary>
        /// <param name="description"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static Description Convert(this Description description, string charset)
        {
            return description?.Next(s => $"Convert({s} Using {charset})");
        }

        /// <summary>
        /// 返回第一个非空值
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static Description Coalesce(params object[] objs)
        {
            if (objs is null)
                return new Description($"Coalesce(Null)");

            return new Description($"Coalesce({string.Join(", ", objs.Select(o => o.ToSqlParam()))})");
        }

        /// <summary>
        /// 进制转换
        /// </summary>
        /// <param name="description">被转换的值</param>
        /// <param name="src">原始进制</param>
        /// <param name="des">新进制</param>
        /// <returns></returns>
        public static Description Conv(this Description description, int src, int des)
        {
            return description?.Next(s => $"Conv({s}, {src}, {des})");
        }

        /// <summary>
        /// IF条件表达式
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="trueResult"></param>
        /// <param name="falseResult"></param>
        /// <returns></returns>
        public static Description IF(ConditionDescription condition, Description trueResult, Description falseResult)
        {
            return new Description($"IF({condition.ConditionStr}, {trueResult}, {falseResult})");
        }

        /// <summary>
        /// 空替代
        /// </summary>
        /// <param name="description"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        public static Description IFNull(this Description description, object replace)
        {
            return description?.Next(s => $"IFNull({s}, {replace.ToSqlParam()})");
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description IsNull(this Description description)
        {
            return description?.Next(s => $"IsNull({s})");
        }

        /// <summary>
        /// 相等时返回 Null，不等时返回原值
        /// </summary>
        /// <param name="description"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static Description NullIF(this Description description, Description compare)
        {
            return description?.Next(s => $"NullIF({s}, {compare})");
        }

        /// <summary>
        /// 版本
        /// </summary>
        /// <returns></returns>
        public static Description Version()
        {
            return new Description("Version()");
        }

        /// <summary>
        /// 连接Id
        /// </summary>
        /// <returns></returns>
        public static Description Connection_Id()
        {
            return new Description("Connection_Id()");
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        /// <returns></returns>
        public static Description Current_User()
        {
            return new Description("Current_User()");
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        /// <returns></returns>
        public static Description Session_User()
        {
            return new Description("Session_User()");
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        /// <returns></returns>
        public static Description System_User()
        {
            return new Description("System_User()");
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        /// <returns></returns>
        public static Description User()
        {
            return new Description("User()");
        }

        /// <summary>
        /// 当前数据库名
        /// </summary>
        /// <returns></returns>
        public static Description Database()
        {
            return new Description("Database()");
        }

        /// <summary>
        /// 最后插入的Id
        /// </summary>
        /// <returns></returns>
        public static Description Last_Insert_Id()
        {
            return new Description("Last_Insert_Id()");
        }


        //自定义

        /// <summary>
        /// 去重
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Distinct(this Description description)
        {
            return description?.Next(s => "Distinct " + s);
        }

        /// <summary>
        /// 按升序排序
        /// </summary>
        /// <param name="description"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static Description OrderBy(this Description description, Description order)
        {
            return description?.Next(s => $"{s} Order By {order}");
        }

        /// <summary>
        /// 按降序排序
        /// </summary>
        /// <param name="description"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static Description OrderByDescending(this Description description, Description order)
        {
            return description?.Next(s => $"{s} Order By {order} Desc");
        }

        /// <summary>
        /// 后续按升序排序
        /// </summary>
        /// <param name="description"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static Description ThenBy(this Description description, Description order)
        {
            return description?.Next(s => $"{s}, {order}");
        }

        /// <summary>
        /// 后续按降序排序
        /// </summary>
        /// <param name="description"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static Description ThenByDescending(this Description description, Description order)
        {
            return description?.Next(s => $"{s}, {order} Desc");
        }

        /// <summary>
        /// 添加分隔符
        /// </summary>
        /// <param name="description"></param>
        /// <param name="sep"></param>
        /// <returns></returns>
        public static Description Separator(this Description description, string sep)
        {
            return description?.Next(s => $"{s} Separator '{sep}'");
        }

        /// <summary>
        /// 列值
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Values(this Description description)
        {
            return description?.Next(s => $"Values({s})");
        }

        /// <summary>
        /// 设置别名
        /// </summary>
        /// <param name="description"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public static Description As(this Description description, string alias)
        {
            Description next = description?.Next(s => s + " As `" + alias + "`");

            if (next is not null)
                next.Alias = alias;

            return next;
        }

        // Window Function

        /// <summary>
        /// 窗口
        /// </summary>
        /// <param name="description"></param>
        /// <param name="window"></param>
        /// <returns></returns>
        public static Description Over(this Description description, Window window)
        {
            return description?.Next(s => s + $" Over {window.Alias}");
        }

        /// <summary>
        /// 窗口
        /// </summary>
        /// <param name="description"></param>
        /// <param name="setWindow"></param>
        /// <returns></returns>
        public static Description Over(this Description description, Action<Window> setWindow)
        {
            Window window = new("");

            setWindow(window);

            return description?.Next(s => s + $" Over ({window.ToString().Trim()})");
        }

        /// <summary>
        /// 累计分布
        /// </summary>
        /// <returns></returns>
        public static Description Cume_Dist()
        {
            return new Description("Cume_Dist()");
        }

        /// <summary>
        /// 排名
        /// </summary>
        /// <returns></returns>
        public static Description Dense_Rank()
        {
            return new Description("Dense_Rank()");
        }

        /// <summary>
        /// 第一个值
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description First_Value(Description description)
        {
            return new Description($"First_Value({description})");
        }

        /// <summary>
        /// 前值
        /// </summary>
        /// <param name="description"></param>
        /// <param name="n"></param>
        /// <param name="defaultDescription"></param>
        /// <returns></returns>
        public static Description Lag(Description description, ulong? n = null, Description defaultDescription = null)
        {
            List<string> ps = new();

            ps.Add(description?.ToString());

            if (n.HasValue)
                ps.Add(n.Value.ToString());

            if (defaultDescription is not null)
                ps.Add(defaultDescription.ToString());

            return new Description($"Lag({string.Join(", ", ps)})");
        }

        /// <summary>
        /// 最后一个值
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Last_Value(Description description)
        {
            return new Description($"Last_Value({description})");
        }

        /// <summary>
        /// 后值
        /// </summary>
        /// <param name="description"></param>
        /// <param name="n"></param>
        /// <param name="defaultDescription"></param>
        /// <returns></returns>
        public static Description Lead(Description description, ulong? n = null, Description defaultDescription = null)
        {
            List<string> ps = new();

            ps.Add(description?.ToString());

            if (n.HasValue)
                ps.Add(n.Value.ToString());

            if (defaultDescription is not null)
                ps.Add(defaultDescription.ToString());

            return new Description($"Lead({string.Join(", ", ps)})");
        }

        /// <summary>
        /// 第n行
        /// </summary>
        /// <param name="description"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static Description Nth_Value(Description description, ulong n)
        {
            return new Description($"Nth_Value({description}, {n})");
        }

        /// <summary>
        /// 分组组号
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static Description Ntile(ulong n)
        {
            return new Description($"Ntile({n})");
        }

        /// <summary>
        /// 百分比排名
        /// </summary>
        /// <returns></returns>
        public static Description Percent_Rank()
        {
            return new Description("Percent_Rank()");
        }

        /// <summary>
        /// 排名
        /// </summary>
        /// <returns></returns>
        public static Description Rank()
        {
            return new Description("Rank()");
        }

        /// <summary>
        /// 行号
        /// </summary>
        /// <returns></returns>
        public static Description Row_Number()
        {
            return new Description("Row_Number()");
        }

        //Json Function


    }
}
