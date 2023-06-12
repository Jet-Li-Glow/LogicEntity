using LogicEntity.Default.MySql.SqlExpressions;
using LogicEntity.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql
{
    internal interface IDataManipulationSql
    {
        Type Type { get; set; }

        int? Timeout { get; set; }

        List<CommonTableExpression> CommonTableExpressions { get; }

        Command Build(LinqConvertProvider linqConvertProvider);
    }
}
