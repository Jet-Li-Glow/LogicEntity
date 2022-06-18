using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Tool;
using LogicEntity.Operator;
using System.Collections;
using LogicEntity.Grammar;

namespace LogicEntity.Model
{
    /// <summary>
    /// 数据库函数
    /// </summary>
    public static class DbFunction
    {
        static IValueExpression __GetDbFunctionDescription(string methodName, params object[] args)
        {
            List<string> strs = new();

            if (args is not null)
                strs.AddRange(args.Select((_, i) => "{" + i + "}"));

            return new ValueExpression(methodName + "(" + string.Join(", ", strs) + ")", args);
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Column Read(this IValueExpression valueExpression, Func<object, object> reader)
        {
            Column column = new Column(valueExpression);

            column.Reader = reader;

            return column;
        }

        /// <summary>
        /// 读取字节
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="bytesReader"></param>
        /// <returns></returns>
        public static Column ReadBytes(this IValueExpression valueExpression, Func<Func<long, byte[], int, int, long>, object> bytesReader)
        {
            Column column = new Column(valueExpression);

            column.BytesReader = bytesReader;

            return column;
        }

        /// <summary>
        /// 读取字符
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="charsReader"></param>
        /// <returns></returns>
        public static Column ReadChars(this IValueExpression valueExpression, Func<Func<long, char[], int, int, long>, object> charsReader)
        {
            Column column = new Column(valueExpression);

            column.CharsReader = charsReader;

            return column;
        }

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public static Column Write(this IValueExpression valueExpression, Func<object, object> writer)
        {
            Column column = new Column(valueExpression);

            if (valueExpression is Column col)
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
        /// 范围查找
        /// </summary>
        /// <param name="vs"></param>
        /// <returns></returns>
        public static IValueExpression In(this IValueExpression valueExpression, IEnumerable vs)
        {
            if (vs is null)
                vs = Enumerable.Empty<object>();

            object[] objs = vs.OfType<object>().ToArray();

            return new ValueExpression("{0} In (" + string.Join(", ", objs.Select((_, i) => "{" + (i + 1) + "}")) + ")", new object[] { valueExpression }.Concat(objs).ToArray());
        }

        /// <summary>
        /// 范围查找
        /// </summary>
        /// <param name="vs"></param>
        /// <returns></returns>
        public static IValueExpression In(this IValueExpression valueExpression, params object[] vs)
        {
            return valueExpression.In(vs.AsEnumerable());
        }

        /// <summary>
        /// 范围查找
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IValueExpression In(this IValueExpression valueExpression, ISelector selector)
        {
            return new ValueExpression("{0} In\n  {1}", valueExpression, selector);
        }

        /// <summary>
        /// 范围查找
        /// </summary>
        /// <param name="vs"></param>
        /// <returns></returns>
        public static IValueExpression NotIn(this IValueExpression valueExpression, IEnumerable vs)
        {
            if (vs is null)
                vs = Enumerable.Empty<object>();

            object[] objs = vs.OfType<object>().ToArray();

            return new ValueExpression("{0} Not In (" + string.Join(", ", objs.Select((_, i) => "{" + (i + 1) + "}")) + ")", new object[] { valueExpression }.Concat(objs).ToArray());
        }

        /// <summary>
        /// 范围查找
        /// </summary>
        /// <param name="vs"></param>
        /// <returns></returns>
        public static IValueExpression NotIn(this IValueExpression valueExpression, params object[] vs)
        {
            return valueExpression.NotIn(vs.AsEnumerable());
        }

        /// <summary>
        /// 范围查找
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IValueExpression NotIn(this IValueExpression valueExpression, ISelector selector)
        {
            return new ValueExpression("{0} Not In\n  {1}", valueExpression, selector);
        }

        /// <summary>
        /// 范围查找
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression Between(this IValueExpression valueExpression, object left, object right)
        {
            return new ValueExpression("{0} Between {1} And {2}", valueExpression, left, right);
        }

        /// <summary>
        /// 范围查找
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IValueExpression NotBetween(this IValueExpression valueExpression, object left, object right)
        {
            return new ValueExpression("{0} Not Between {1} And {2}", valueExpression, left, right);
        }

        /// <summary>
        /// 相似
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static IValueExpression Like(this IValueExpression valueExpression, string str)
        {
            return new ValueExpression("{0} Like {1}", valueExpression, str);
        }

        /// <summary>
        /// 列值
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Values(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Values), valueExpression);
        }

        /// <summary>
        /// 设置别名
        /// </summary>
        /// <param name="sqlExpression"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public static Column As(this ISqlExpression sqlExpression, string alias)
        {
            Column column = new Column(sqlExpression);

            if (sqlExpression is Column col)
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
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression ASCII(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(ASCII), valueExpression);
        }

        /// <summary>
        /// 字符串的字符数
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Char_Length(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Char_Length), valueExpression);
        }

        /// <summary>
        /// 字符串的字符数
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Character_Length(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Character_Length), valueExpression);
        }

        /// <summary>
        /// 合并字符串
        /// </summary>
        /// <param name="values">值</param>
        /// <returns></returns>
        public static IValueExpression Concat(params object[] values)
        {
            return __GetDbFunctionDescription(nameof(Concat), values);
        }

        /// <summary>
        /// 合并字符串
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IValueExpression Concat_Ws(params object[] values)
        {
            return __GetDbFunctionDescription(nameof(Concat_Ws), values);
        }

        /// <summary>
        /// 合并字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IValueExpression Group_Concat(object value)
        {
            return __GetDbFunctionDescription(nameof(Group_Concat), value);
        }

        /// <summary>
        /// 返回当前字符串在字符串列表中的位置
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IValueExpression Field(params object[] values)
        {
            return __GetDbFunctionDescription(nameof(Field), values);
        }

        /// <summary>
        /// 当前字符串在字符串列表中的位置
        /// </summary>
        /// <param name="valueExpression">当前字符串</param>
        /// <param name="strList">字符串列表（以 , 分隔）</param>
        /// <returns></returns>
        public static IValueExpression Find_In_Set(this IValueExpression valueExpression, object strList)
        {
            return __GetDbFunctionDescription(nameof(Find_In_Set), valueExpression, strList);
        }

        /// <summary>
        /// 将数字格式化为 '##,###.##' 的形式
        /// </summary>
        /// <param name="valueExpression">数字</param>
        /// <param name="digits">保留的小数位数</param>
        /// <returns></returns>
        public static IValueExpression Format(this IValueExpression valueExpression, int digits)
        {
            return __GetDbFunctionDescription(nameof(Format), valueExpression, digits);
        }

        /// <summary>
        /// 替换字符串中的一部分
        /// </summary>
        /// <param name="valueExpression">原始字符串</param>
        /// <param name="start">开始位置</param>
        /// <param name="length">替换长度</param>
        /// <param name="replace">字符串</param>
        /// <returns></returns>
        public static IValueExpression Insert(this IValueExpression valueExpression, int start, int length, object replace)
        {
            return __GetDbFunctionDescription(nameof(Insert), valueExpression, start, length, replace);
        }

        /// <summary>
        /// 在字符串中的开始位置
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static IValueExpression Locate(this IValueExpression valueExpression, object str)
        {
            return __GetDbFunctionDescription(nameof(Locate), valueExpression, str);
        }

        /// <summary>
        /// 将字母转换为小写字母
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Lcase(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Lcase), valueExpression);
        }

        /// <summary>
        /// 将字母转换为大写字母
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Ucase(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Ucase), valueExpression);
        }

        /// <summary>
        /// 将字母转换为小写字母
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Lower(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Lower), valueExpression);
        }

        /// <summary>
        /// 将字母转换为大写字母
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Upper(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Upper), valueExpression);
        }

        /// <summary>
        /// 字符串左侧的字符
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static IValueExpression Left(this IValueExpression valueExpression, int length)
        {
            return __GetDbFunctionDescription(nameof(Left), valueExpression, length);
        }

        /// <summary>
        /// 字符串右侧的字符
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static IValueExpression Right(this IValueExpression valueExpression, int length)
        {
            return __GetDbFunctionDescription(nameof(Right), valueExpression, length);
        }

        /// <summary>
        /// 在字符串左侧填充字符串达到总长度
        /// </summary>
        /// <param name="valueExpression">字符串</param>
        /// <param name="length">总长度</param>
        /// <param name="str">用于填充的字符串</param>
        /// <returns></returns>
        public static IValueExpression LPad(this IValueExpression valueExpression, int length, object str)
        {
            return __GetDbFunctionDescription(nameof(LPad), valueExpression, length, str);
        }

        /// <summary>
        /// 在字符串右侧填充字符串达到总长度
        /// </summary>
        /// <param name="valueExpression">字符串</param>
        /// <param name="length">总长度</param>
        /// <param name="str">用于填充的字符串</param>
        /// <returns></returns>
        public static IValueExpression RPad(this IValueExpression valueExpression, int length, object str)
        {
            return __GetDbFunctionDescription(nameof(RPad), valueExpression, length, str);
        }

        /// <summary>
        /// 去掉字符串左侧空格
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression LTrim(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(LTrim), valueExpression);
        }

        /// <summary>
        /// 去掉字符串右侧空格
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression RTrim(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(RTrim), valueExpression);
        }

        /// <summary>
        /// 去掉字符串两侧空格
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Trim(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Trim), valueExpression);
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="valueExpression">字符串</param>
        /// <param name="start">开始位置</param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        public static IValueExpression Mid(this IValueExpression valueExpression, int start, int length)
        {
            return __GetDbFunctionDescription(nameof(Mid), valueExpression, start, length);
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="valueExpression">字符串</param>
        /// <param name="start">开始位置</param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        public static IValueExpression SubStr(this IValueExpression valueExpression, int start, int length)
        {
            return __GetDbFunctionDescription(nameof(SubStr), valueExpression, start, length);
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="valueExpression">字符串</param>
        /// <param name="start">开始位置</param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        public static IValueExpression SubString(this IValueExpression valueExpression, int start, int length)
        {
            return __GetDbFunctionDescription(nameof(SubString), valueExpression, start, length);
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="valueExpression">字符串</param>
        /// <param name="separator">分隔符</param>
        /// <param name="num">分隔符序号</param>
        /// <returns></returns>
        public static IValueExpression SubString_Index(this IValueExpression valueExpression, object separator, int num)
        {
            return __GetDbFunctionDescription(nameof(SubString_Index), valueExpression, separator, num);
        }

        /// <summary>
        /// 字符串1 在 字符串2 中的位置
        /// </summary>
        /// <param name="valueExpression">字符串1</param>
        /// <param name="str">字符串2</param>
        /// <returns></returns>
        public static IValueExpression Position(this IValueExpression valueExpression, object str)
        {
            return __GetDbFunctionDescription(nameof(Position), valueExpression, new ValueExpression(" In {0} ", str));
        }

        /// <summary>
        /// 重复字符串
        /// </summary>
        /// <param name="valueExpression">字符串</param>
        /// <param name="times">重复次数</param>
        /// <returns></returns>
        public static IValueExpression Repeat(this IValueExpression valueExpression, int times)
        {
            return __GetDbFunctionDescription(nameof(Repeat), valueExpression, times);
        }

        /// <summary>
        /// 替换字符串
        /// </summary>
        /// <param name="valueExpression">字符串</param>
        /// <param name="original">旧字符串</param>
        /// <param name="replace">新字符串</param>
        /// <returns></returns>
        public static IValueExpression Replace(this IValueExpression valueExpression, object original, object replace)
        {
            return __GetDbFunctionDescription(nameof(Replace), valueExpression, original, replace);
        }

        /// <summary>
        /// 反转字符串
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Reverse(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Reverse), valueExpression);
        }

        /// <summary>
        /// 返回空格
        /// </summary>
        /// <param name="count">空格的数量</param>
        /// <returns></returns>
        public static IValueExpression Space(int count)
        {
            return __GetDbFunctionDescription(nameof(Space), count);
        }

        /// <summary>
        /// 比较字符串
        /// </summary>
        /// <param name="left">左值</param>
        /// <param name="right">右值</param>
        /// <returns></returns>
        public static IValueExpression Strcmp(this IValueExpression left, IValueExpression right)
        {
            return __GetDbFunctionDescription(nameof(Strcmp), left, right);
        }

        /// <summary>
        /// 绝对值
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Abs(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Abs), valueExpression);
        }

        /// <summary>
        /// 正弦
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Sin(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Sin), valueExpression);
        }

        /// <summary>
        /// 余弦
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Cos(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Cos), valueExpression);
        }

        /// <summary>
        /// 正切
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Tan(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Tan), valueExpression);
        }

        /// <summary>
        /// 余切
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Cot(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Cot), valueExpression);
        }

        /// <summary>
        /// 反正弦
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Asin(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Asin), valueExpression);
        }

        /// <summary>
        /// 反余弦
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Acos(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Acos), valueExpression);
        }

        /// <summary>
        /// 反正切
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Atan(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Atan), valueExpression);
        }

        /// <summary>
        /// 反正切
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static IValueExpression Atan2(object x, object y)
        {
            return __GetDbFunctionDescription(nameof(Atan2), x, y);
        }

        /// <summary>
        /// 平均值
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Avg(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Avg), valueExpression);
        }

        /// <summary>
        /// 向上取整
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Ceil(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Ceil), valueExpression);
        }

        /// <summary>
        /// 向上取整
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Ceiling(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Ceiling), valueExpression);
        }

        /// <summary>
        /// 向下取整
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Floor(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Floor), valueExpression);
        }

        /// <summary>
        /// 总数
        /// </summary>
        /// <returns></returns>
        public static IValueExpression Count()
        {
            return new ValueExpression("Count(*)");
        }

        /// <summary>
        /// 总数
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static IValueExpression Count(int i)
        {
            return __GetDbFunctionDescription(nameof(Count), i);
        }

        /// <summary>
        /// 总数
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Count(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Count), valueExpression);
        }

        /// <summary>
        /// 角度
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Degrees(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Degrees), valueExpression);
        }

        /// <summary>
        /// 弧度
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Radians(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Radians), valueExpression);
        }

        /// <summary>
        /// 整除
        /// </summary>
        /// <param name="divided"></param>
        /// <param name="divider"></param>
        /// <returns></returns>
        public static IValueExpression Div(this IValueExpression divided, object divider)
        {
            return new ValueExpression("{0} Div {1}", divided, divider);
        }

        /// <summary>
        /// 取余
        /// </summary>
        /// <param name="divided"></param>
        /// <param name="divider"></param>
        /// <returns></returns>
        public static IValueExpression Mod(this IValueExpression divided, object divider)
        {
            return __GetDbFunctionDescription(nameof(Mod), divided, divider);
        }

        /// <summary>
        /// E的幂
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static IValueExpression Exp(object index)
        {
            return __GetDbFunctionDescription(nameof(Exp), index);
        }

        /// <summary>
        /// 最大值
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="more"></param>
        /// <returns></returns>
        public static IValueExpression Greatest(this IValueExpression valueExpression, params object[] more)
        {
            return __GetDbFunctionDescription(nameof(Greatest), more);
        }

        /// <summary>
        /// 最小值
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="more"></param>
        /// <returns></returns>
        public static IValueExpression Least(this IValueExpression valueExpression, params object[] more)
        {
            return __GetDbFunctionDescription(nameof(Least), more);
        }

        /// <summary>
        /// e 的对数
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Ln(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Ln), valueExpression);
        }

        /// <summary>
        /// 对数
        /// </summary>
        /// <param name="baseNum"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public static IValueExpression Log(IValueExpression baseNum, object power)
        {
            return __GetDbFunctionDescription(nameof(Log), baseNum, power);
        }

        /// <summary>
        /// 10 的对数
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Log10(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Log10), valueExpression);
        }

        /// <summary>
        /// 2 的对数
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Log2(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Log2), valueExpression);
        }

        /// <summary>
        /// 最大值
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Max(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Max), valueExpression);
        }

        /// <summary>
        /// 最小值
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Min(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Min), valueExpression);
        }

        /// <summary>
        /// 圆周率
        /// </summary>
        /// <returns></returns>
        public static IValueExpression PI()
        {
            return new ValueExpression("PI()");
        }

        /// <summary>
        /// 幂
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static IValueExpression Pow(this IValueExpression x, object y)
        {
            return __GetDbFunctionDescription(nameof(Pow), x, y);
        }

        /// <summary>
        /// 随机数
        /// </summary>
        /// <returns></returns>
        public static IValueExpression Rand()
        {
            return new ValueExpression("Rand()");
        }

        /// <summary>
        /// 取整
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Round(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Round), valueExpression);
        }

        /// <summary>
        /// 正数、0 或 负数
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Sign(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Sign), valueExpression);
        }

        /// <summary>
        /// 算术平方根
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Sqrt(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Sqrt), valueExpression);
        }

        /// <summary>
        /// 求和
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Sum(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Sum), valueExpression);
        }

        /// <summary>
        /// 保留小数
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static IValueExpression Truncate(this IValueExpression valueExpression, int digits)
        {
            return __GetDbFunctionDescription(nameof(Truncate), valueExpression, digits);
        }

        /// <summary>
        /// 加年
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="years"></param>
        /// <returns></returns>
        public static IValueExpression AddYears(this IValueExpression valueExpression, int years)
        {
            return new ValueExpression("AddDate({0}, Interval {1} Year)", valueExpression, years);
        }

        /// <summary>
        /// 加月
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        public static IValueExpression AddMonths(this IValueExpression valueExpression, int months)
        {
            return new ValueExpression("AddDate({0}, Interval {1} Month)", valueExpression, months);
        }

        /// <summary>
        /// 加日
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public static IValueExpression AddDays(this IValueExpression valueExpression, int days)
        {
            return new ValueExpression("AddDate({0}, Interval {1} Day)", valueExpression, days);
        }

        /// <summary>
        /// 加小时
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static IValueExpression AddHours(this IValueExpression valueExpression, int hours)
        {
            return new ValueExpression("AddDate({0}, Interval {1} Hour)", valueExpression, hours);
        }

        /// <summary>
        /// 加分钟
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static IValueExpression AddMinutes(this IValueExpression valueExpression, int minutes)
        {
            return new ValueExpression("AddDate({0}, Interval {1} Minute)", valueExpression, minutes);
        }

        /// <summary>
        /// 加秒
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static IValueExpression AddSeconds(this IValueExpression valueExpression, int seconds)
        {
            return new ValueExpression("AddDate({0}, Interval {1} Second)", valueExpression, seconds);
        }

        /// <summary>
        /// 加时间
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static IValueExpression AddTime(this IValueExpression valueExpression, object time)
        {
            return __GetDbFunctionDescription(nameof(AddTime), valueExpression, time);
        }

        /// <summary>
        /// 当前日期
        /// </summary>
        /// <returns></returns>
        public static IValueExpression CurDate()
        {
            return __GetDbFunctionDescription(nameof(CurDate));
        }

        /// <summary>
        /// 当前日期
        /// </summary>
        /// <returns></returns>
        public static IValueExpression Current_Date()
        {
            return __GetDbFunctionDescription(nameof(Current_Date));
        }

        /// <summary>
        /// 当前时间
        /// </summary>
        /// <returns></returns>
        public static IValueExpression CurTime()
        {
            return __GetDbFunctionDescription(nameof(CurTime));
        }

        /// <summary>
        /// 当前时间
        /// </summary>
        /// <returns></returns>
        public static IValueExpression Current_Time()
        {
            return __GetDbFunctionDescription(nameof(Current_Time));
        }

        /// <summary>
        /// 当前日期和时间
        /// </summary>
        /// <returns></returns>
        public static IValueExpression Current_TimeStamp()
        {
            return __GetDbFunctionDescription(nameof(Current_TimeStamp));
        }

        /// <summary>
        /// 当前日期和时间
        /// </summary>
        /// <returns></returns>
        public static IValueExpression LocalTime()
        {
            return __GetDbFunctionDescription(nameof(LocalTime));
        }

        /// <summary>
        /// 当前日期和时间
        /// </summary>
        /// <returns></returns>
        public static IValueExpression LocalTimeStamp()
        {
            return __GetDbFunctionDescription(nameof(LocalTimeStamp));
        }

        /// <summary>
        /// 当前日期和时间
        /// </summary>
        /// <returns></returns>
        public static IValueExpression Now()
        {
            return __GetDbFunctionDescription(nameof(Now));
        }

        /// <summary>
        /// 当前日期和时间
        /// </summary>
        /// <returns></returns>
        public static IValueExpression SysDate()
        {
            return __GetDbFunctionDescription(nameof(SysDate));
        }

        /// <summary>
        /// 取日期
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Date(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Date), valueExpression);
        }

        /// <summary>
        /// 取时间
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Time(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Time), valueExpression);
        }

        /// <summary>
        /// 取年
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Year(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Year), valueExpression);
        }

        /// <summary>
        /// 取月
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Month(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Month), valueExpression);
        }

        /// <summary>
        /// 取天
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Day(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Day), valueExpression);
        }

        /// <summary>
        /// 取小时
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Hour(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Hour), valueExpression);
        }

        /// <summary>
        /// 取分钟
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Minute(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Minute), valueExpression);
        }

        /// <summary>
        /// 取秒
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Second(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Second), valueExpression);
        }

        /// <summary>
        /// 取微秒
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression MicroSecond(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(MicroSecond), valueExpression);
        }

        /// <summary>
        /// 当前月份的最后一天
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Last_Day(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Last_Day), valueExpression);
        }

        /// <summary>
        /// 星期几
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression WeekDay(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(WeekDay), valueExpression);
        }

        /// <summary>
        /// 星期几的名称
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression DayName(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(DayName), valueExpression);
        }

        /// <summary>
        /// 月份的名称
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression MonthName(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(MonthName), valueExpression);
        }

        /// <summary>
        /// 季度
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Quarter(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Quarter), valueExpression);
        }

        /// <summary>
        /// 当年的第几天
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression DayOfYear(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(DayOfYear), valueExpression);
        }

        /// <summary>
        /// 当月的第几天
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression DayOfMonth(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(DayOfMonth), valueExpression);
        }

        /// <summary>
        /// 当前星期的第几天
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression DayOfWeek(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(DayOfWeek), valueExpression);
        }

        /// <summary>
        /// 当年的第几个星期
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Week(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Week), valueExpression);
        }

        /// <summary>
        /// 当年的第几个星期
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression WeekOfYear(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(WeekOfYear), valueExpression);
        }

        /// <summary>
        /// 从 0000 年 1 月 1 日开始 n 天后的日期
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression From_Days(object valueExpression)
        {
            return __GetDbFunctionDescription(nameof(From_Days), valueExpression);
        }

        /// <summary>
        /// 距离 0000 年 1 月 1 日的天数
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression To_Days(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(To_Days), valueExpression);
        }

        /// <summary>
        /// 根据 年 和 天数 创建日期
        /// </summary>
        /// <param name="year"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static IValueExpression MakeDate(object year, object day)
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
        public static IValueExpression MakeTime(object hour, object minute, object second)
        {
            return __GetDbFunctionDescription(nameof(MakeTime), hour, minute, second);
        }

        /// <summary>
        /// 日期相减的天数
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static IValueExpression DateDiff(this IValueExpression date1, object date2)
        {
            return __GetDbFunctionDescription(nameof(DateDiff), date1, date2);
        }

        /// <summary>
        /// 日期格式化
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static IValueExpression Date_Format(this IValueExpression date, object format)
        {
            return __GetDbFunctionDescription(nameof(Date_Format), date, format);
        }

        /// <summary>
        /// 时间格式化
        /// </summary>
        /// <param name="time"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static IValueExpression Time_Format(this IValueExpression time, object format)
        {
            return __GetDbFunctionDescription(nameof(Time_Format), time, format);
        }

        /// <summary>
        /// 秒 转换 时间
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static IValueExpression Sec_To_Time(this IValueExpression seconds)
        {
            return __GetDbFunctionDescription(nameof(Sec_To_Time), seconds);
        }

        /// <summary>
        /// 时间 转换 秒
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static IValueExpression Time_To_Sec(this IValueExpression time)
        {
            return __GetDbFunctionDescription(nameof(Time_To_Sec), time);
        }

        /// <summary>
        /// 时间差
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns></returns>
        public static IValueExpression TimeDiff(this IValueExpression time1, object time2)
        {
            return __GetDbFunctionDescription(nameof(TimeDiff), time1, time2);
        }

        /// <summary>
        /// 二进制数字
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Bin(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Bin), valueExpression);
        }

        /// <summary>
        /// 二进制字符串
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Binary(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Binary), valueExpression);
        }

        /// <summary>
        /// Case
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Case(this IValueExpression valueExpression)
        {
            return new ValueExpression("Case {0}", valueExpression);
        }

        /// <summary>
        /// When
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IValueExpression When(this IValueExpression valueExpression, object value)
        {
            return new ValueExpression("{0}\n  When {1}", valueExpression, value);
        }

        /// <summary>
        /// Then
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IValueExpression Then(this IValueExpression valueExpression, object value)
        {
            return new ValueExpression("{0} Then {1}", valueExpression, value);
        }

        /// <summary>
        /// Else
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IValueExpression Else(this IValueExpression valueExpression, object value)
        {
            return new ValueExpression("{0}\n  Else {1}", valueExpression, value);
        }

        /// <summary>
        /// End
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression End(this IValueExpression valueExpression)
        {
            return new ValueExpression("{0}\nEnd", valueExpression);
        }

        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IValueExpression Cast(this IValueExpression valueExpression, object type)
        {
            return new ValueExpression("Cast({0} As {1})", valueExpression, type);
        }

        /// <summary>
        /// 转换字符串的字符集
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static IValueExpression Convert(this IValueExpression valueExpression, object charset)
        {
            return new ValueExpression("Convert({0} Using {1})", valueExpression, charset);
        }

        /// <summary>
        /// 返回第一个非空值
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IValueExpression Coalesce(params object[] values)
        {
            return __GetDbFunctionDescription(nameof(Coalesce), values);
        }

        /// <summary>
        /// 进制转换
        /// </summary>
        /// <param name="valueExpression">被转换的值</param>
        /// <param name="src">原始进制</param>
        /// <param name="des">新进制</param>
        /// <returns></returns>
        public static IValueExpression Conv(this IValueExpression valueExpression, int src, int des)
        {
            return __GetDbFunctionDescription(nameof(Conv), valueExpression, src, des);
        }

        /// <summary>
        /// IF条件表达式
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="trueResult"></param>
        /// <param name="falseResult"></param>
        /// <returns></returns>
        public static IValueExpression IF(object condition, object trueResult, object falseResult)
        {
            return __GetDbFunctionDescription(nameof(IF), condition, trueResult, falseResult);
        }

        /// <summary>
        /// 空替代
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        public static IValueExpression IFNull(this IValueExpression valueExpression, object replace)
        {
            return __GetDbFunctionDescription(nameof(IFNull), valueExpression, replace);
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression IsNull(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(IsNull), valueExpression);
        }

        /// <summary>
        /// 相等时返回 Null，不等时返回原值
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static IValueExpression NullIF(this IValueExpression valueExpression, object compare)
        {
            return __GetDbFunctionDescription(nameof(NullIF), valueExpression, compare);
        }

        /// <summary>
        /// 版本
        /// </summary>
        /// <returns></returns>
        public static IValueExpression Version()
        {
            return __GetDbFunctionDescription(nameof(Version));
        }

        /// <summary>
        /// 连接Id
        /// </summary>
        /// <returns></returns>
        public static IValueExpression Connection_Id()
        {
            return __GetDbFunctionDescription(nameof(Connection_Id));
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        /// <returns></returns>
        public static IValueExpression Current_User()
        {
            return __GetDbFunctionDescription(nameof(Current_User));
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        /// <returns></returns>
        public static IValueExpression Session_User()
        {
            return __GetDbFunctionDescription(nameof(Session_User));
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        /// <returns></returns>
        public static IValueExpression System_User()
        {
            return __GetDbFunctionDescription(nameof(System_User));
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        /// <returns></returns>
        public static IValueExpression User()
        {
            return __GetDbFunctionDescription(nameof(User));
        }

        /// <summary>
        /// 当前数据库名
        /// </summary>
        /// <returns></returns>
        public static IValueExpression Database()
        {
            return __GetDbFunctionDescription(nameof(Database));
        }

        /// <summary>
        /// 最后插入的Id
        /// </summary>
        /// <returns></returns>
        public static IValueExpression Last_Insert_Id()
        {
            return __GetDbFunctionDescription(nameof(Last_Insert_Id));
        }


        //自定义

        /// <summary>
        /// 去重
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Distinct(this IValueExpression valueExpression)
        {
            return new ValueExpression("Distinct {0}", valueExpression);
        }

        /// <summary>
        /// 按升序排序
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static IValueExpression OrderBy(this IValueExpression valueExpression, object order)
        {
            return new ValueExpression("{0} Order By {1} Asc", valueExpression, order);
        }

        /// <summary>
        /// 按降序排序
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static IValueExpression OrderByDescending(this IValueExpression valueExpression, object order)
        {
            return new ValueExpression("{0} Order By {1} Desc", valueExpression, order);
        }

        /// <summary>
        /// 后续按升序排序
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static IValueExpression ThenBy(this IValueExpression valueExpression, object order)
        {
            return new ValueExpression("{0}, {1}", valueExpression, order);
        }

        /// <summary>
        /// 后续按降序排序
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static IValueExpression ThenByDescending(this IValueExpression valueExpression, object order)
        {
            return new ValueExpression("{0}, {1} Desc", valueExpression, order);
        }

        /// <summary>
        /// 添加分隔符
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="sep"></param>
        /// <returns></returns>
        public static IValueExpression Separator(this IValueExpression valueExpression, string sep)
        {
            return new ValueExpression("{0} Separator '" + sep + "'", valueExpression);
        }

        // Window Function

        /// <summary>
        /// 窗口
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="window"></param>
        /// <returns></returns>
        public static ISqlExpression Over(this IValueExpression valueExpression, Window window)
        {
            return new SqlExpression("{0} Over " + window.Alias, valueExpression);
        }

        /// <summary>
        /// 窗口
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="setWindow"></param>
        /// <returns></returns>
        public static ISqlExpression Over(this IValueExpression valueExpression, Action<Window> setWindow)
        {
            Window window = new("");

            setWindow(window);

            return new SqlExpression("{0} Over ({1})", valueExpression, window);
        }

        /// <summary>
        /// 累计分布
        /// </summary>
        /// <returns></returns>
        public static IValueExpression Cume_Dist()
        {
            return __GetDbFunctionDescription(nameof(Cume_Dist));
        }

        /// <summary>
        /// 排名
        /// </summary>
        /// <returns></returns>
        public static IValueExpression Dense_Rank()
        {
            return __GetDbFunctionDescription(nameof(Dense_Rank));
        }

        /// <summary>
        /// 第一个值
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression First_Value(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(First_Value), valueExpression);
        }

        /// <summary>
        /// 前值
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="n"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static IValueExpression Lag(this IValueExpression valueExpression, ulong? n = null, object defaultValue = null)
        {
            List<object> args = new();

            args.Add(valueExpression);

            if (n.HasValue)
                args.Add(n.Value);

            if (defaultValue is not null)
                args.Add(defaultValue);

            return __GetDbFunctionDescription(nameof(Lag), args.ToArray());
        }

        /// <summary>
        /// 最后一个值
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        public static IValueExpression Last_Value(this IValueExpression valueExpression)
        {
            return __GetDbFunctionDescription(nameof(Last_Value), valueExpression);
        }

        /// <summary>
        /// 后值
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="n"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static IValueExpression Lead(IValueExpression valueExpression, ulong? n = null, object defaultValue = null)
        {
            List<object> args = new();

            args.Add(valueExpression);

            if (n.HasValue)
                args.Add(n.Value);

            if (defaultValue is not null)
                args.Add(defaultValue);

            return __GetDbFunctionDescription(nameof(Lead), args.ToArray());
        }

        /// <summary>
        /// 第n行
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static IValueExpression Nth_Value(IValueExpression valueExpression, ulong n)
        {
            return __GetDbFunctionDescription(nameof(Nth_Value), n);
        }

        /// <summary>
        /// 分组组号
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static IValueExpression Ntile(ulong n)
        {
            return __GetDbFunctionDescription(nameof(Ntile), n);
        }

        /// <summary>
        /// 百分比排名
        /// </summary>
        /// <returns></returns>
        public static IValueExpression Percent_Rank()
        {
            return __GetDbFunctionDescription(nameof(Percent_Rank));
        }

        /// <summary>
        /// 排名
        /// </summary>
        /// <returns></returns>
        public static IValueExpression Rank()
        {
            return __GetDbFunctionDescription(nameof(Rank));
        }

        /// <summary>
        /// 行号
        /// </summary>
        /// <returns></returns>
        public static IValueExpression Row_Number()
        {
            return __GetDbFunctionDescription(nameof(Row_Number));
        }

        //Json Function

        /// <summary>
        /// Json 数组
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IValueExpression Json_Array(params object[] values)
        {
            return __GetDbFunctionDescription(nameof(Json_Array), values);
        }

        /// <summary>
        /// Json对象
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IValueExpression Json_Object(params object[] values)
        {
            return __GetDbFunctionDescription(nameof(Json_Object), values);
        }

        /// <summary>
        /// Json值引用
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static IValueExpression Json_Quote(this IValueExpression str)
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
        public static IValueExpression Json_Contains(this IValueExpression target, object candidate, object path = null)
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
        public static IValueExpression Json_Contains_Path(this IValueExpression json_doc, string one_or_all, params object[] paths)
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

            return new ValueExpression("Json_Contains_Path({0}, '" + one_or_all + "' " + string.Join(string.Empty, strs) + ")", args.ToArray());
        }

        /// <summary>
        /// Json文档指定路径的值
        /// </summary>
        /// <param name="json_doc"></param>
        /// <param name="path"></param>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static IValueExpression Json_Extract(this IValueExpression json_doc, object path, params object[] paths)
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
        public static IValueExpression Json_Keys(this IValueExpression json_doc, params object[] paths)
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
        public static IValueExpression Json_Overlaps(this IValueExpression json_doc1, object json_doc2)
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
        public static IValueExpression Json_Search(this IValueExpression json_doc, string one_or_all, object search_str, object escape_char = null, params object[] paths)
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

            return new ValueExpression("Json_Search({0}, '" + one_or_all + "', {1} " + string.Join(string.Empty, strs) + ")", args.ToArray());
        }

        /// <summary>
        /// 是否被包含
        /// </summary>
        /// <param name="value"></param>
        /// <param name="json_array"></param>
        /// <returns></returns>
        public static IValueExpression Member_Of(this IValueExpression value, object json_array)
        {
            return new ValueExpression("{0} Member Of {1}", value, json_array);
        }

        /// <summary>
        /// 向数组中的元素追加值
        /// </summary>
        /// <param name="json_doc"></param>
        /// <param name="path"></param>
        /// <param name="val"></param>
        /// <param name="more"></param>
        /// <returns></returns>
        public static IValueExpression Json_Array_Append(this IValueExpression json_doc, object path, object val, params object[] more)
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
        public static IValueExpression Json_Array_Insert(this IValueExpression json_doc, object path, object val, params object[] more)
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
        public static IValueExpression Json_Insert(this IValueExpression json_doc, object path, object val, params object[] more)
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
        public static IValueExpression Json_Merge(this IValueExpression json_doc1, object json_doc2, params object[] json_docs)
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
        public static IValueExpression Json_Merge_Patch(this IValueExpression json_doc1, object json_doc2, params object[] json_docs)
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
        public static IValueExpression Json_Merge_Preserve(object json_doc1, object json_doc2, params object[] json_docs)
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
        public static IValueExpression Json_Remove(this IValueExpression json_doc, object path, params object[] paths)
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
        public static IValueExpression Json_Replace(this IValueExpression json_doc, object path, object val, params object[] more)
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
        public static IValueExpression Json_Set(this IValueExpression json_doc, object path, object val, params object[] more)
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
        public static IValueExpression Json_Unquote(this IValueExpression json_val)
        {
            return __GetDbFunctionDescription(nameof(Json_Unquote), json_val);
        }

        /// <summary>
        /// Json文档深度
        /// </summary>
        /// <param name="json_doc"></param>
        /// <returns></returns>
        public static IValueExpression Json_Depth(this IValueExpression json_doc)
        {
            return __GetDbFunctionDescription(nameof(Json_Depth), json_doc);
        }

        /// <summary>
        /// Json文档的长度
        /// </summary>
        /// <param name="json_doc"></param>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static IValueExpression Json_Length(this IValueExpression json_doc, params object[] paths)
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
        public static IValueExpression Json_Type(this IValueExpression json_val)
        {
            return __GetDbFunctionDescription(nameof(Json_Type), json_val);
        }

        /// <summary>
        /// 是否是有效的Json
        /// </summary>
        /// <param name="json_val"></param>
        /// <returns></returns>
        public static IValueExpression Json_Valid(this IValueExpression val)
        {
            return __GetDbFunctionDescription(nameof(Json_Valid), val);
        }

        /// <summary>
        /// Json模式验证
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        public static IValueExpression Json_Schema_Valid(this IValueExpression schema, object document)
        {
            return __GetDbFunctionDescription(nameof(Json_Schema_Valid), schema, document);
        }

        /// <summary>
        /// Json模式验证
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        public static IValueExpression Json_Schema_Validation_Report(this IValueExpression schema, object document)
        {
            return __GetDbFunctionDescription(nameof(Json_Schema_Validation_Report), schema, document);
        }

        /// <summary>
        /// Json值美化
        /// </summary>
        /// <param name="json_val"></param>
        /// <returns></returns>
        public static IValueExpression Json_Pretty(this IValueExpression json_val)
        {
            return __GetDbFunctionDescription(nameof(Json_Pretty), json_val);
        }
    }
}
