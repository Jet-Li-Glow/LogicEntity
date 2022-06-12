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
        static Description __GetDbFunctionDescription(string methodName, params object[] args)
        {
            List<string> strs = new();

            if (args is not null)
                strs.AddRange(args.Select((_, i) => "{" + i + "}"));

            return new Description(methodName + "(" + string.Join(", ", strs) + ")", args);
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="description"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Column Read(this Description description, Func<object, object> reader)
        {
            Column column = new Column(description);

            if (description is Column col)
            {
                column.Reader = col.Reader;

                column.BytesReader = col.BytesReader;

                column.CharsReader = col.CharsReader;

                column.Writer = col.Writer;
            }

            column.Reader = reader;

            return column;
        }

        /// <summary>
        /// 读取字节
        /// </summary>
        /// <param name="description"></param>
        /// <param name="bytesReader"></param>
        /// <returns></returns>
        public static Column ReadBytes(this Description description, Func<Func<long, byte[], int, int, long>, object> bytesReader)
        {
            Column column = new Column(description);

            if (description is Column col)
            {
                column.Reader = col.Reader;

                column.BytesReader = col.BytesReader;

                column.CharsReader = col.CharsReader;

                column.Writer = col.Writer;
            }

            column.BytesReader = bytesReader;

            return column;
        }

        /// <summary>
        /// 读取字符
        /// </summary>
        /// <param name="description"></param>
        /// <param name="charsReader"></param>
        /// <returns></returns>
        public static Column ReadChars(this Description description, Func<Func<long, char[], int, int, long>, object> charsReader)
        {
            Column column = new Column(description);

            if (description is Column col)
            {
                column.Reader = col.Reader;

                column.BytesReader = col.BytesReader;

                column.CharsReader = col.CharsReader;

                column.Writer = col.Writer;
            }

            column.CharsReader = charsReader;

            return column;
        }

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="description"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public static Column Write(this Description description, Func<object, object> writer)
        {
            Column column = new Column(description);

            if (description is Column col)
            {
                column.Reader = col.Reader;

                column.BytesReader = col.BytesReader;

                column.CharsReader = col.CharsReader;

                column.Writer = col.Writer;
            }

            column.Writer = writer;

            return column;
        }

        /// <summary>
        /// 列值
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Values(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Values), description);
        }

        /// <summary>
        /// 设置别名
        /// </summary>
        /// <param name="description"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public static Column As(this Description description, string alias)
        {
            Column column = new Column(description);

            if (description is Column col)
            {
                column.Reader = col.Reader;

                column.BytesReader = col.BytesReader;

                column.CharsReader = col.CharsReader;

                column.Writer = col.Writer;
            }

            column.Alias = alias;

            return column;
        }

        //-----------------------------------------------------------------------------------

        /// <summary>
        /// 第一个字符的ASCII码
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description ASCII(this Description description)
        {
            return __GetDbFunctionDescription(nameof(ASCII), description);
        }

        /// <summary>
        /// 字符串的字符数
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Char_Length(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Char_Length), description);
        }

        /// <summary>
        /// 字符串的字符数
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Character_Length(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Character_Length), description);
        }

        /// <summary>
        /// 合并字符串
        /// </summary>
        /// <param name="values">值</param>
        /// <returns></returns>
        public static Description Concat(params object[] values)
        {
            return __GetDbFunctionDescription(nameof(Concat), values);
        }

        /// <summary>
        /// 合并字符串
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Description Concat_Ws(params object[] values)
        {
            return __GetDbFunctionDescription(nameof(Concat_Ws), values);
        }

        /// <summary>
        /// 合并字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Description Group_Concat(object value)
        {
            return __GetDbFunctionDescription(nameof(Group_Concat), value);
        }

        /// <summary>
        /// 返回当前字符串在字符串列表中的位置
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Description Field(params object[] values)
        {
            return __GetDbFunctionDescription(nameof(Field), values);
        }

        /// <summary>
        /// 当前字符串在字符串列表中的位置
        /// </summary>
        /// <param name="description">当前字符串</param>
        /// <param name="strList">字符串列表（以 , 分隔）</param>
        /// <returns></returns>
        public static Description Find_In_Set(this Description description, object strList)
        {
            return __GetDbFunctionDescription(nameof(Find_In_Set), description, strList);
        }

        /// <summary>
        /// 将数字格式化为 '##,###.##' 的形式
        /// </summary>
        /// <param name="description">数字</param>
        /// <param name="digits">保留的小数位数</param>
        /// <returns></returns>
        public static Description Format(this Description description, int digits)
        {
            return __GetDbFunctionDescription(nameof(Format), description, digits);
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
            return __GetDbFunctionDescription(nameof(Insert), description, start, length, replace);
        }

        /// <summary>
        /// 在字符串中的开始位置
        /// </summary>
        /// <param name="description"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Description Locate(this Description description, object str)
        {
            return __GetDbFunctionDescription(nameof(Locate), description, str);
        }

        /// <summary>
        /// 将字母转换为小写字母
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Lcase(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Lcase), description);
        }

        /// <summary>
        /// 将字母转换为大写字母
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Ucase(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Ucase), description);
        }

        /// <summary>
        /// 将字母转换为小写字母
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Lower(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Lower), description);
        }

        /// <summary>
        /// 将字母转换为大写字母
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Upper(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Upper), description);
        }

        /// <summary>
        /// 字符串左侧的字符
        /// </summary>
        /// <param name="description"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Description Left(this Description description, int length)
        {
            return __GetDbFunctionDescription(nameof(Left), description, length);
        }

        /// <summary>
        /// 字符串右侧的字符
        /// </summary>
        /// <param name="description"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Description Right(this Description description, int length)
        {
            return __GetDbFunctionDescription(nameof(Right), description, length);
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
            return __GetDbFunctionDescription(nameof(LPad), description, length, str);
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
            return __GetDbFunctionDescription(nameof(RPad), description, length, str);
        }

        /// <summary>
        /// 去掉字符串左侧空格
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description LTrim(this Description description)
        {
            return __GetDbFunctionDescription(nameof(LTrim), description);
        }

        /// <summary>
        /// 去掉字符串右侧空格
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description RTrim(this Description description)
        {
            return __GetDbFunctionDescription(nameof(RTrim), description);
        }

        /// <summary>
        /// 去掉字符串两侧空格
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Trim(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Trim), description);
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
            return __GetDbFunctionDescription(nameof(Mid), description, start, length);
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
            return __GetDbFunctionDescription(nameof(SubStr), description, start, length);
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
            return __GetDbFunctionDescription(nameof(SubString), description, start, length);
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
            return __GetDbFunctionDescription(nameof(SubString_Index), description, separator, num);
        }

        /// <summary>
        /// 字符串1 在 字符串2 中的位置
        /// </summary>
        /// <param name="description">字符串1</param>
        /// <param name="str">字符串2</param>
        /// <returns></returns>
        public static Description Position(this Description description, object str)
        {
            return __GetDbFunctionDescription(nameof(Position), description, new Description(" In {0} ", str));
        }

        /// <summary>
        /// 重复字符串
        /// </summary>
        /// <param name="description">字符串</param>
        /// <param name="times">重复次数</param>
        /// <returns></returns>
        public static Description Repeat(this Description description, int times)
        {
            return __GetDbFunctionDescription(nameof(Repeat), description, times);
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
            return __GetDbFunctionDescription(nameof(Replace), description, original, replace);
        }

        /// <summary>
        /// 反转字符串
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Reverse(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Reverse), description);
        }

        /// <summary>
        /// 返回空格
        /// </summary>
        /// <param name="count">空格的数量</param>
        /// <returns></returns>
        public static Description Space(int count)
        {
            return __GetDbFunctionDescription(nameof(Space), count);
        }

        /// <summary>
        /// 比较字符串
        /// </summary>
        /// <param name="left">左值</param>
        /// <param name="right">右值</param>
        /// <returns></returns>
        public static Description Strcmp(this Description left, Description right)
        {
            return __GetDbFunctionDescription(nameof(Strcmp), left, right);
        }

        /// <summary>
        /// 绝对值
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Abs(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Abs), description);
        }

        /// <summary>
        /// 正弦
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Sin(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Sin), description);
        }

        /// <summary>
        /// 余弦
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Cos(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Cos), description);
        }

        /// <summary>
        /// 正切
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Tan(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Tan), description);
        }

        /// <summary>
        /// 余切
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Cot(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Cot), description);
        }

        /// <summary>
        /// 反正弦
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Asin(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Asin), description);
        }

        /// <summary>
        /// 反余弦
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Acos(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Acos), description);
        }

        /// <summary>
        /// 反正切
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Atan(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Atan), description);
        }

        /// <summary>
        /// 反正切
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Description Atan2(object x, object y)
        {
            return __GetDbFunctionDescription(nameof(Atan2), x, y);
        }

        /// <summary>
        /// 平均值
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Avg(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Avg), description);
        }

        /// <summary>
        /// 向上取整
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Ceil(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Ceil), description);
        }

        /// <summary>
        /// 向上取整
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Ceiling(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Ceiling), description);
        }

        /// <summary>
        /// 向下取整
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Floor(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Floor), description);
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
            return __GetDbFunctionDescription(nameof(Count), i);
        }

        /// <summary>
        /// 总数
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Count(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Count), description);
        }

        /// <summary>
        /// 角度
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Degrees(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Degrees), description);
        }

        /// <summary>
        /// 弧度
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Radians(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Radians), description);
        }

        /// <summary>
        /// 整除
        /// </summary>
        /// <param name="divided"></param>
        /// <param name="divider"></param>
        /// <returns></returns>
        public static Description Div(this Description divided, object divider)
        {
            return new Description("{0} Div {1}", divided, divider);
        }

        /// <summary>
        /// 取余
        /// </summary>
        /// <param name="divided"></param>
        /// <param name="divider"></param>
        /// <returns></returns>
        public static Description Mod(this Description divided, object divider)
        {
            return __GetDbFunctionDescription(nameof(Mod), divided, divider);
        }

        /// <summary>
        /// E的幂
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Description Exp(object index)
        {
            return __GetDbFunctionDescription(nameof(Exp), index);
        }

        /// <summary>
        /// 最大值
        /// </summary>
        /// <param name="description"></param>
        /// <param name="more"></param>
        /// <returns></returns>
        public static Description Greatest(this Description description, params object[] more)
        {
            return __GetDbFunctionDescription(nameof(Greatest), more);
        }

        /// <summary>
        /// 最小值
        /// </summary>
        /// <param name="description"></param>
        /// <param name="more"></param>
        /// <returns></returns>
        public static Description Least(this Description description, params object[] more)
        {
            return __GetDbFunctionDescription(nameof(Least), more);
        }

        /// <summary>
        /// e 的对数
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Ln(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Ln), description);
        }

        /// <summary>
        /// 对数
        /// </summary>
        /// <param name="baseNum"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public static Description Log(Description baseNum, object power)
        {
            return __GetDbFunctionDescription(nameof(Log), baseNum, power);
        }

        /// <summary>
        /// 10 的对数
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Log10(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Log10), description);
        }

        /// <summary>
        /// 2 的对数
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Log2(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Log2), description);
        }

        /// <summary>
        /// 最大值
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Max(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Max), description);
        }

        /// <summary>
        /// 最小值
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Min(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Min), description);
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
        public static Description Pow(this Description x, object y)
        {
            return __GetDbFunctionDescription(nameof(Pow), x, y);
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
            return __GetDbFunctionDescription(nameof(Round), description);
        }

        /// <summary>
        /// 正数、0 或 负数
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Sign(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Sign), description);
        }

        /// <summary>
        /// 算术平方根
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Sqrt(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Sqrt), description);
        }

        /// <summary>
        /// 求和
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Sum(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Sum), description);
        }

        /// <summary>
        /// 保留小数
        /// </summary>
        /// <param name="description"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static Description Truncate(this Description description, int digits)
        {
            return __GetDbFunctionDescription(nameof(Truncate), description, digits);
        }

        /// <summary>
        /// 加年
        /// </summary>
        /// <param name="description"></param>
        /// <param name="years"></param>
        /// <returns></returns>
        public static Description AddYears(this Description description, int years)
        {
            return new Description("AddDate({0}, Interval {1} Year)", description, years);
        }

        /// <summary>
        /// 加月
        /// </summary>
        /// <param name="description"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public static Description AddMonths(this Description description, int months)
        {
            return new Description("AddDate({0}, Interval {1} Month)", description, months);
        }

        /// <summary>
        /// 加日
        /// </summary>
        /// <param name="description"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public static Description AddDays(this Description description, int days)
        {
            return new Description("AddDate({0}, Interval {1} Day)", description, days);
        }

        /// <summary>
        /// 加小时
        /// </summary>
        /// <param name="description"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static Description AddHours(this Description description, int hours)
        {
            return new Description("AddDate({0}, Interval {1} Hour)", description, hours);
        }

        /// <summary>
        /// 加分钟
        /// </summary>
        /// <param name="description"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static Description AddMinutes(this Description description, int minutes)
        {
            return new Description("AddDate({0}, Interval {1} Minute)", description, minutes);
        }

        /// <summary>
        /// 加秒
        /// </summary>
        /// <param name="description"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static Description AddSeconds(this Description description, int seconds)
        {
            return new Description("AddDate({0}, Interval {1} Second)", description, seconds);
        }

        /// <summary>
        /// 加时间
        /// </summary>
        /// <param name="description"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static Description AddTime(this Description description, object time)
        {
            return __GetDbFunctionDescription(nameof(AddTime), description, time);
        }

        /// <summary>
        /// 当前日期
        /// </summary>
        /// <returns></returns>
        public static Description CurDate()
        {
            return __GetDbFunctionDescription(nameof(CurDate));
        }

        /// <summary>
        /// 当前日期
        /// </summary>
        /// <returns></returns>
        public static Description Current_Date()
        {
            return __GetDbFunctionDescription(nameof(Current_Date));
        }

        /// <summary>
        /// 当前时间
        /// </summary>
        /// <returns></returns>
        public static Description CurTime()
        {
            return __GetDbFunctionDescription(nameof(CurTime));
        }

        /// <summary>
        /// 当前时间
        /// </summary>
        /// <returns></returns>
        public static Description Current_Time()
        {
            return __GetDbFunctionDescription(nameof(Current_Time));
        }

        /// <summary>
        /// 当前日期和时间
        /// </summary>
        /// <returns></returns>
        public static Description Current_TimeStamp()
        {
            return __GetDbFunctionDescription(nameof(Current_TimeStamp));
        }

        /// <summary>
        /// 当前日期和时间
        /// </summary>
        /// <returns></returns>
        public static Description LocalTime()
        {
            return __GetDbFunctionDescription(nameof(LocalTime));
        }

        /// <summary>
        /// 当前日期和时间
        /// </summary>
        /// <returns></returns>
        public static Description LocalTimeStamp()
        {
            return __GetDbFunctionDescription(nameof(LocalTimeStamp));
        }

        /// <summary>
        /// 当前日期和时间
        /// </summary>
        /// <returns></returns>
        public static Description Now()
        {
            return __GetDbFunctionDescription(nameof(Now));
        }

        /// <summary>
        /// 当前日期和时间
        /// </summary>
        /// <returns></returns>
        public static Description SysDate()
        {
            return __GetDbFunctionDescription(nameof(SysDate));
        }

        /// <summary>
        /// 取日期
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Date(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Date), description);
        }

        /// <summary>
        /// 取时间
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Time(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Time), description);
        }

        /// <summary>
        /// 取年
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Year(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Year), description);
        }

        /// <summary>
        /// 取月
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Month(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Month), description);
        }

        /// <summary>
        /// 取天
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Day(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Day), description);
        }

        /// <summary>
        /// 取小时
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Hour(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Hour), description);
        }

        /// <summary>
        /// 取分钟
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Minute(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Minute), description);
        }

        /// <summary>
        /// 取秒
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Second(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Second), description);
        }

        /// <summary>
        /// 取微秒
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description MicroSecond(this Description description)
        {
            return __GetDbFunctionDescription(nameof(MicroSecond), description);
        }

        /// <summary>
        /// 当前月份的最后一天
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Last_Day(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Last_Day), description);
        }

        /// <summary>
        /// 星期几
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description WeekDay(this Description description)
        {
            return __GetDbFunctionDescription(nameof(WeekDay), description);
        }

        /// <summary>
        /// 星期几的名称
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description DayName(this Description description)
        {
            return __GetDbFunctionDescription(nameof(DayName), description);
        }

        /// <summary>
        /// 月份的名称
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description MonthName(this Description description)
        {
            return __GetDbFunctionDescription(nameof(MonthName), description);
        }

        /// <summary>
        /// 季度
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Quarter(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Quarter), description);
        }

        /// <summary>
        /// 当年的第几天
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description DayOfYear(this Description description)
        {
            return __GetDbFunctionDescription(nameof(DayOfYear), description);
        }

        /// <summary>
        /// 当月的第几天
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description DayOfMonth(this Description description)
        {
            return __GetDbFunctionDescription(nameof(DayOfMonth), description);
        }

        /// <summary>
        /// 当前星期的第几天
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description DayOfWeek(this Description description)
        {
            return __GetDbFunctionDescription(nameof(DayOfWeek), description);
        }

        /// <summary>
        /// 当年的第几个星期
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Week(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Week), description);
        }

        /// <summary>
        /// 当年的第几个星期
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description WeekOfYear(this Description description)
        {
            return __GetDbFunctionDescription(nameof(WeekOfYear), description);
        }

        /// <summary>
        /// 从 0000 年 1 月 1 日开始 n 天后的日期
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description From_Days(object description)
        {
            return __GetDbFunctionDescription(nameof(From_Days), description);
        }

        /// <summary>
        /// 距离 0000 年 1 月 1 日的天数
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description To_Days(this Description description)
        {
            return __GetDbFunctionDescription(nameof(To_Days), description);
        }

        /// <summary>
        /// 根据 年 和 天数 创建日期
        /// </summary>
        /// <param name="year"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static Description MakeDate(object year, object day)
        {
            return __GetDbFunctionDescription(nameof(MakeDate), year, day);
        }

        /// <summary>
        /// 根据 时、分、秒 创建时间
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Description MakeTime(object hour, object minute, object second)
        {
            return __GetDbFunctionDescription(nameof(MakeTime), hour, minute, second);
        }

        /// <summary>
        /// 日期相减的天数
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static Description DateDiff(this Description date1, object date2)
        {
            return __GetDbFunctionDescription(nameof(DateDiff), date1, date2);
        }

        /// <summary>
        /// 日期格式化
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static Description Date_Format(this Description date, object format)
        {
            return __GetDbFunctionDescription(nameof(Date_Format), date, format);
        }

        /// <summary>
        /// 时间格式化
        /// </summary>
        /// <param name="time"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static Description Time_Format(this Description time, object format)
        {
            return __GetDbFunctionDescription(nameof(Time_Format), time, format);
        }

        /// <summary>
        /// 秒 转换 时间
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static Description Sec_To_Time(this Description seconds)
        {
            return __GetDbFunctionDescription(nameof(Sec_To_Time), seconds);
        }

        /// <summary>
        /// 时间 转换 秒
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static Description Time_To_Sec(this Description time)
        {
            return __GetDbFunctionDescription(nameof(Time_To_Sec), time);
        }

        /// <summary>
        /// 时间差
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns></returns>
        public static Description TimeDiff(this Description time1, object time2)
        {
            return __GetDbFunctionDescription(nameof(TimeDiff), time1, time2);
        }

        /// <summary>
        /// 二进制数字
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Bin(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Bin), description);
        }

        /// <summary>
        /// 二进制字符串
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Binary(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Binary), description);
        }

        /// <summary>
        /// Case
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Case(this Description description)
        {
            return new Description("Case {0}", description);
        }

        /// <summary>
        /// When
        /// </summary>
        /// <param name="description"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Description When(this Description description, object value)
        {
            return new Description("{0}\n  When {1}", description, value);
        }

        /// <summary>
        /// Then
        /// </summary>
        /// <param name="description"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Description Then(this Description description, object value)
        {
            return new Description("{0} Then {1}", description, value);
        }

        /// <summary>
        /// Else
        /// </summary>
        /// <param name="description"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Description Else(this Description description, object value)
        {
            return new Description("{0}\n  Else {1}", description, value);
        }

        /// <summary>
        /// End
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description End(this Description description)
        {
            return new Description("{0}\nEnd", description);
        }

        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="description"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Description Cast(this Description description, object type)
        {
            return new Description("Cast({0} As {1})", description, type);
        }

        /// <summary>
        /// 转换字符串的字符集
        /// </summary>
        /// <param name="description"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static Description Convert(this Description description, object charset)
        {
            return new Description("Convert({0} Using {1})", description, charset);
        }

        /// <summary>
        /// 返回第一个非空值
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Description Coalesce(params object[] values)
        {
            return __GetDbFunctionDescription(nameof(Coalesce), values);
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
            return __GetDbFunctionDescription(nameof(Conv), description, src, des);
        }

        /// <summary>
        /// IF条件表达式
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="trueResult"></param>
        /// <param name="falseResult"></param>
        /// <returns></returns>
        public static Description IF(object condition, object trueResult, object falseResult)
        {
            return __GetDbFunctionDescription(nameof(IF), condition, trueResult, falseResult);
        }

        /// <summary>
        /// 空替代
        /// </summary>
        /// <param name="description"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        public static Description IFNull(this Description description, object replace)
        {
            return __GetDbFunctionDescription(nameof(IFNull), description, replace);
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description IsNull(this Description description)
        {
            return __GetDbFunctionDescription(nameof(IsNull), description);
        }

        /// <summary>
        /// 相等时返回 Null，不等时返回原值
        /// </summary>
        /// <param name="description"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static Description NullIF(this Description description, object compare)
        {
            return __GetDbFunctionDescription(nameof(NullIF), description, compare);
        }

        /// <summary>
        /// 版本
        /// </summary>
        /// <returns></returns>
        public static Description Version()
        {
            return __GetDbFunctionDescription(nameof(Version));
        }

        /// <summary>
        /// 连接Id
        /// </summary>
        /// <returns></returns>
        public static Description Connection_Id()
        {
            return __GetDbFunctionDescription(nameof(Connection_Id));
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        /// <returns></returns>
        public static Description Current_User()
        {
            return __GetDbFunctionDescription(nameof(Current_User));
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        /// <returns></returns>
        public static Description Session_User()
        {
            return __GetDbFunctionDescription(nameof(Session_User));
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        /// <returns></returns>
        public static Description System_User()
        {
            return __GetDbFunctionDescription(nameof(System_User));
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        /// <returns></returns>
        public static Description User()
        {
            return __GetDbFunctionDescription(nameof(User));
        }

        /// <summary>
        /// 当前数据库名
        /// </summary>
        /// <returns></returns>
        public static Description Database()
        {
            return __GetDbFunctionDescription(nameof(Database));
        }

        /// <summary>
        /// 最后插入的Id
        /// </summary>
        /// <returns></returns>
        public static Description Last_Insert_Id()
        {
            return __GetDbFunctionDescription(nameof(Last_Insert_Id));
        }


        //自定义

        /// <summary>
        /// 去重
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Distinct(this Description description)
        {
            return new Description("Distinct {0}", description);
        }

        /// <summary>
        /// 按升序排序
        /// </summary>
        /// <param name="description"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static Description OrderBy(this Description description, object order)
        {
            return new Description("{0} Order By {1} Asc", description, order);
        }

        /// <summary>
        /// 按降序排序
        /// </summary>
        /// <param name="description"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static Description OrderByDescending(this Description description, object order)
        {
            return new Description("{0} Order By {1} Desc", description, order);
        }

        /// <summary>
        /// 后续按升序排序
        /// </summary>
        /// <param name="description"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static Description ThenBy(this Description description, object order)
        {
            return new Description("{0}, {1}", description, order);
        }

        /// <summary>
        /// 后续按降序排序
        /// </summary>
        /// <param name="description"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static Description ThenByDescending(this Description description, object order)
        {
            return new Description("{0}, {1} Desc", description, order);
        }

        /// <summary>
        /// 添加分隔符
        /// </summary>
        /// <param name="description"></param>
        /// <param name="sep"></param>
        /// <returns></returns>
        public static Description Separator(this Description description, string sep)
        {
            return new Description("{0} Separator '" + sep + "'", description);
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
            return new Description("{0} Over " + window.Alias, description);
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

            return new Description("{0} Over ({1})", description, window);
        }

        /// <summary>
        /// 累计分布
        /// </summary>
        /// <returns></returns>
        public static Description Cume_Dist()
        {
            return __GetDbFunctionDescription(nameof(Cume_Dist));
        }

        /// <summary>
        /// 排名
        /// </summary>
        /// <returns></returns>
        public static Description Dense_Rank()
        {
            return __GetDbFunctionDescription(nameof(Dense_Rank));
        }

        /// <summary>
        /// 第一个值
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description First_Value(this Description description)
        {
            return __GetDbFunctionDescription(nameof(First_Value), description);
        }

        /// <summary>
        /// 前值
        /// </summary>
        /// <param name="description"></param>
        /// <param name="n"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Description Lag(this Description description, ulong? n = null, object defaultValue = null)
        {
            List<object> args = new();

            args.Add(description);

            if (n.HasValue)
                args.Add(n.Value);

            if (defaultValue is not null)
                args.Add(defaultValue);

            return __GetDbFunctionDescription(nameof(Lag), args.ToArray());
        }

        /// <summary>
        /// 最后一个值
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Description Last_Value(this Description description)
        {
            return __GetDbFunctionDescription(nameof(Last_Value), description);
        }

        /// <summary>
        /// 后值
        /// </summary>
        /// <param name="description"></param>
        /// <param name="n"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Description Lead(Description description, ulong? n = null, object defaultValue = null)
        {
            List<object> args = new();

            args.Add(description);

            if (n.HasValue)
                args.Add(n.Value);

            if (defaultValue is not null)
                args.Add(defaultValue);

            return __GetDbFunctionDescription(nameof(Lead), args.ToArray());
        }

        /// <summary>
        /// 第n行
        /// </summary>
        /// <param name="description"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static Description Nth_Value(Description description, ulong n)
        {
            return __GetDbFunctionDescription(nameof(Nth_Value), n);
        }

        /// <summary>
        /// 分组组号
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static Description Ntile(ulong n)
        {
            return __GetDbFunctionDescription(nameof(Ntile), n);
        }

        /// <summary>
        /// 百分比排名
        /// </summary>
        /// <returns></returns>
        public static Description Percent_Rank()
        {
            return __GetDbFunctionDescription(nameof(Percent_Rank));
        }

        /// <summary>
        /// 排名
        /// </summary>
        /// <returns></returns>
        public static Description Rank()
        {
            return __GetDbFunctionDescription(nameof(Rank));
        }

        /// <summary>
        /// 行号
        /// </summary>
        /// <returns></returns>
        public static Description Row_Number()
        {
            return __GetDbFunctionDescription(nameof(Row_Number));
        }

        //Json Function

        /// <summary>
        /// Json 数组
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Description Json_Array(params object[] values)
        {
            return __GetDbFunctionDescription(nameof(Json_Array), values);
        }

        /// <summary>
        /// Json对象
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Description Json_Object(params object[] values)
        {
            return __GetDbFunctionDescription(nameof(Json_Object), values);
        }

        /// <summary>
        /// Json值引用
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Description Json_Quote(this Description str)
        {
            return __GetDbFunctionDescription(nameof(Json_Quote), str);
        }

        /// <summary>
        /// Json文档是否包含指定内容
        /// </summary>
        /// <param name="target"></param>
        /// <param name="candidate"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Description Json_Contains(this Description target, object candidate, object path = null)
        {
            List<object> args = new() { target, candidate };

            if (path is not null)
                args.Add(path);

            return __GetDbFunctionDescription(nameof(Json_Contains), args.ToArray());
        }

        /// <summary>
        /// Json文档在指定路径是否包含指定内容
        /// </summary>
        /// <param name="json_doc"></param>
        /// <param name="one_or_all"></param>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static Description Json_Contains_Path(this Description json_doc, string one_or_all, params object[] paths)
        {
            if (one_or_all.Equals("one", StringComparison.OrdinalIgnoreCase) == false && one_or_all.Equals("all", StringComparison.OrdinalIgnoreCase) == false)
                one_or_all = "Null";

            List<string> strs = new();

            List<object> args = new() { json_doc };

            if (paths is not null)
            {
                strs.AddRange(paths.Select((_, i) => ",{" + (i + args.Count) + "}"));

                args.AddRange(paths);
            }

            return new Description("Json_Contains_Path({0}, '" + one_or_all + "' " + string.Join(string.Empty, strs) + ")", args.ToArray());
        }

        /// <summary>
        /// Json文档指定路径的值
        /// </summary>
        /// <param name="json_doc"></param>
        /// <param name="path"></param>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static Description Json_Extract(this Description json_doc, object path, params object[] paths)
        {
            List<object> args = new() { json_doc, path };

            if (paths is not null)
                args.Add(paths);

            return __GetDbFunctionDescription(nameof(Json_Extract), args.ToArray());
        }

        /// <summary>
        /// Json对象的键
        /// </summary>
        /// <param name="json_doc"></param>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static Description Json_Keys(this Description json_doc, params object[] paths)
        {
            List<object> args = new() { json_doc };

            if (paths is not null)
                args.AddRange(paths);

            return __GetDbFunctionDescription(nameof(Json_Keys), args.ToArray());
        }

        /// <summary>
        /// 是否重叠
        /// </summary>
        /// <param name="json_doc1"></param>
        /// <param name="json_doc2"></param>
        /// <returns></returns>
        public static Description Json_Overlaps(this Description json_doc1, object json_doc2)
        {
            return __GetDbFunctionDescription(nameof(Json_Overlaps), json_doc1, json_doc2);
        }

        /// <summary>
        /// Json搜索
        /// </summary>
        /// <param name="json_doc"></param>
        /// <param name="one_or_all"></param>
        /// <param name="search_str"></param>
        /// <param name="escape_char"></param>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static Description Json_Search(this Description json_doc, string one_or_all, object search_str, object escape_char = null, params object[] paths)
        {
            if (one_or_all.Equals("one", StringComparison.OrdinalIgnoreCase) == false && one_or_all.Equals("all", StringComparison.OrdinalIgnoreCase) == false)
                one_or_all = "Null";

            List<string> strs = new();

            List<object> args = new() { json_doc, search_str };

            if (paths is not null)
            {
                strs.AddRange(paths.Select((_, i) => ",{" + (i + args.Count) + "}"));

                args.AddRange(paths);
            }

            return new Description("Json_Search({0}, '" + one_or_all + "', {1} " + string.Join(string.Empty, strs) + ")", args.ToArray());
        }

        /// <summary>
        /// 是否被包含
        /// </summary>
        /// <param name="value"></param>
        /// <param name="json_array"></param>
        /// <returns></returns>
        public static Description Member_Of(this Description value, object json_array)
        {
            return new Description("{0} Member Of {1}", value, json_array);
        }

        /// <summary>
        /// 向数组中的元素追加值
        /// </summary>
        /// <param name="json_doc"></param>
        /// <param name="path"></param>
        /// <param name="val"></param>
        /// <param name="more"></param>
        /// <returns></returns>
        public static Description Json_Array_Append(this Description json_doc, object path, object val, params object[] more)
        {
            List<object> args = new() { json_doc, path, val };

            if (more is not null)
                args.AddRange(more);

            return __GetDbFunctionDescription(nameof(Json_Array_Append), args.ToArray());
        }

        /// <summary>
        /// 向数组中插入元素
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Description Json_Array_Insert(this Description json_doc, object path, object val, params object[] more)
        {
            List<object> args = new() { json_doc, path, val };

            if (more is not null)
                args.AddRange(more);

            return __GetDbFunctionDescription(nameof(Json_Array_Insert), args.ToArray());
        }

        /// <summary>
        /// 将值插入到Json文档的指定路径中
        /// </summary>
        /// <param name="json_doc"></param>
        /// <param name="path"></param>
        /// <param name="val"></param>
        /// <param name="more"></param>
        /// <returns></returns>
        public static Description Json_Insert(this Description json_doc, object path, object val, params object[] more)
        {
            List<object> args = new() { json_doc, path, val };

            if (more is not null)
                args.AddRange(more);

            return __GetDbFunctionDescription(nameof(Json_Insert), args.ToArray());
        }

        /// <summary>
        /// 合并Json文档
        /// </summary>
        /// <param name="json_doc1"></param>
        /// <param name="json_doc2"></param>
        /// <param name="json_docs"></param>
        /// <returns></returns>
        public static Description Json_Merge(this Description json_doc1, object json_doc2, params object[] json_docs)
        {
            List<object> args = new() { json_doc1, json_doc2 };

            if (json_docs is not null)
                args.AddRange(json_docs);

            return __GetDbFunctionDescription(nameof(Json_Merge), args.ToArray());
        }

        /// <summary>
        /// 合并Json文档（RFC 7396）
        /// </summary>
        /// <param name="json_doc1"></param>
        /// <param name="json_doc2"></param>
        /// <param name="json_docs"></param>
        /// <returns></returns>
        public static Description Json_Merge_Patch(this Description json_doc1, object json_doc2, params object[] json_docs)
        {
            List<object> args = new() { json_doc1, json_doc2 };

            if (json_docs is not null)
                args.AddRange(json_docs);

            return __GetDbFunctionDescription(nameof(Json_Merge_Patch), args.ToArray());
        }

        /// <summary>
        /// 合并Json文档
        /// </summary>
        /// <param name="json_doc1"></param>
        /// <param name="json_doc2"></param>
        /// <param name="json_docs"></param>
        /// <returns></returns>
        public static Description Json_Merge_Preserve(object json_doc1, object json_doc2, params object[] json_docs)
        {
            List<object> args = new() { json_doc1, json_doc2 };

            if (json_docs is not null)
                args.AddRange(json_docs);

            return __GetDbFunctionDescription(nameof(Json_Merge_Preserve), args.ToArray());
        }

        /// <summary>
        /// 从Json文档中移除
        /// </summary>
        /// <param name="json_doc"></param>
        /// <param name="path"></param>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static Description Json_Remove(this Description json_doc, object path, params object[] paths)
        {
            List<object> args = new() { json_doc, path };

            if (paths is not null)
                args.AddRange(paths);

            return __GetDbFunctionDescription(nameof(Json_Remove), args.ToArray());
        }

        /// <summary>
        /// 从Json文档中替换
        /// </summary>
        /// <param name="json_doc"></param>
        /// <param name="path"></param>
        /// <param name="val"></param>
        /// <param name="more"></param>
        /// <returns></returns>
        public static Description Json_Replace(this Description json_doc, object path, object val, params object[] more)
        {
            List<object> args = new() { json_doc, path, val };

            if (more is not null)
                args.AddRange(more);

            return __GetDbFunctionDescription(nameof(Json_Replace), args.ToArray());
        }

        /// <summary>
        /// 设置Json文档中的值
        /// </summary>
        /// <param name="json_doc"></param>
        /// <param name="path"></param>
        /// <param name="val"></param>
        /// <param name="more"></param>
        /// <returns></returns>
        public static Description Json_Set(this Description json_doc, object path, object val, params object[] more)
        {
            List<object> args = new() { json_doc, path, val };

            if (more is not null)
                args.AddRange(more);

            return __GetDbFunctionDescription(nameof(Json_Set), args.ToArray());
        }

        /// <summary>
        /// 解除Json值引用
        /// </summary>
        /// <param name="json_val"></param>
        /// <returns></returns>
        public static Description Json_Unquote(this Description json_val)
        {
            return __GetDbFunctionDescription(nameof(Json_Unquote), json_val);
        }

        /// <summary>
        /// Json文档深度
        /// </summary>
        /// <param name="json_doc"></param>
        /// <returns></returns>
        public static Description Json_Depth(this Description json_doc)
        {
            return __GetDbFunctionDescription(nameof(Json_Depth), json_doc);
        }

        /// <summary>
        /// Json文档的长度
        /// </summary>
        /// <param name="json_doc"></param>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static Description Json_Length(this Description json_doc, params object[] paths)
        {
            List<object> args = new() { json_doc };

            if (paths is not null)
                args.AddRange(paths);

            return __GetDbFunctionDescription(nameof(Json_Length), args.ToArray());
        }

        /// <summary>
        /// Json值类型
        /// </summary>
        /// <param name="json_val"></param>
        /// <returns></returns>
        public static Description Json_Type(this Description json_val)
        {
            return __GetDbFunctionDescription(nameof(Json_Type), json_val);
        }

        /// <summary>
        /// 是否是有效的Json
        /// </summary>
        /// <param name="json_val"></param>
        /// <returns></returns>
        public static Description Json_Valid(this Description val)
        {
            return __GetDbFunctionDescription(nameof(Json_Valid), val);
        }

        /// <summary>
        /// Json模式验证
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        public static Description Json_Schema_Valid(this Description schema, object document)
        {
            return __GetDbFunctionDescription(nameof(Json_Schema_Valid), schema, document);
        }

        /// <summary>
        /// Json模式验证
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        public static Description Json_Schema_Validation_Report(this Description schema, object document)
        {
            return __GetDbFunctionDescription(nameof(Json_Schema_Validation_Report), schema, document);
        }

        /// <summary>
        /// Json值美化
        /// </summary>
        /// <param name="json_val"></param>
        /// <returns></returns>
        public static Description Json_Pretty(this Description json_val)
        {
            return __GetDbFunctionDescription(nameof(Json_Pretty), json_val);
        }
    }
}
