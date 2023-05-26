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

        bool CanAddNode(SelectNodeType nodeType);

        SelectExpression AddSelect();

        SelectExpression Distinct();

        SelectExpression AddIndex();

        SelectExpression AddWhere();

        SelectExpression AddGroupBy();

        SelectExpression AddHaving();

        ISelectSql AddOrderBy();

        ISelectSql AddThenBy();

        ISelectSql AddLimit();

        DeleteExpression AddDelete();

        UpdateExpression AddUpdateSet();

        Command Build(LinqConvertProvider linqConvertProvider);
    }
}
