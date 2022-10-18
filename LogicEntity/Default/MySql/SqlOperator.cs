using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql
{
    internal enum SqlOperator
    {
        /// <summary>
        /// Interval
        /// </summary>
        Interval = 0,

        /// <summary>
        /// Binary
        /// </summary>
        Binary = 1,
        /// <summary>
        /// Collate
        /// </summary>
        Collate = 1,

        /// <summary>
        /// !
        /// </summary>
        Exclamation = 2,

        /// <summary>
        /// -
        /// </summary>
        Negate = 3,

        /// <summary>
        /// ^
        /// </summary>
        ExclusiveOr = 4,

        /// <summary>
        /// *
        /// </summary>
        Multiply = 5,
        /// <summary>
        /// /，DIV
        /// </summary>
        Divide = 5,
        /// <summary>
        /// %，MOD
        /// </summary>
        Modulo = 5,

        /// <summary>
        /// +
        /// </summary>
        Add = 6,
        /// <summary>
        /// -
        /// </summary>
        Subtract = 6,

        /// <summary>
        /// &lt;&lt;
        /// </summary>
        LeftShift = 7,
        /// <summary>
        /// &gt;&gt;
        /// </summary>
        RightShift = 7,

        /// <summary>
        /// &amp;
        /// </summary>
        And = 8,

        /// <summary>
        /// |
        /// </summary>
        Or = 9,

        /// <summary>
        /// =. &lt;=&gt;
        /// </summary>
        Equal = 10,
        /// <summary>
        /// &gt;
        /// </summary>
        GreaterThan = 10,
        /// <summary>
        /// &gt;=
        /// </summary>
        GreaterThanOrEqual = 10,
        /// <summary>
        /// &lt;
        /// </summary>
        LessThan = 10,
        /// <summary>
        /// &lt;=
        /// </summary>
        LessThanOrEqual = 10,
        /// <summary>
        /// &lt;&gt;，!=
        /// </summary>
        NotEqual = 10,
        /// <summary>
        /// is
        /// </summary>
        Is = 10,
        /// <summary>
        /// Like
        /// </summary>
        Like = 10,
        /// <summary>
        /// Regexp
        /// </summary>
        Regexp = 10,
        /// <summary>
        /// In
        /// </summary>
        In = 10,
        /// <summary>
        /// Member Of
        /// </summary>
        MemberOf = 10,

        /// <summary>
        /// Between
        /// </summary>
        Between = 11,
        /// <summary>
        /// Case
        /// </summary>
        Case = 11,
        /// <summary>
        /// When
        /// </summary>
        When = 11,
        /// <summary>
        /// Then
        /// </summary>
        Then = 11,
        /// <summary>
        /// Else
        /// </summary>
        Else = 11,

        /// <summary>
        /// Not
        /// </summary>
        Not = 12,

        /// <summary>
        /// And，&amp;&amp;
        /// </summary>
        AndAlso = 13,

        /// <summary>
        /// XOR
        /// </summary>
        XOR = 14,

        /// <summary>
        /// Or，||
        /// </summary>
        OrElse = 15,

        /// <summary>
        /// =，:=
        /// </summary>
        Assignment = 16
    }
}
