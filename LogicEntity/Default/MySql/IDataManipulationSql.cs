using LogicEntity.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql
{
    internal interface IDataManipulationSql
    {
        public IDataManipulationSql SetSqlType(DataManipulationSqlType sqlType);

        Type Type { get; set; }

        bool IsCTE { get; set; }

        public int? Timeout { get; set; }

        bool CanSet(SelectNodeType nodeType);

        IDataManipulationSql SetDistinct(bool distinct);

        IDataManipulationSql SetHasIndex(bool hasIndex);

        IDataManipulationSql SetDelete(List<string> tables);

        IDataManipulationSql SetSet(LambdaExpression[] assignments);

        IDataManipulationSql SetWhere(List<LambdaExpression> where);

        IDataManipulationSql SetGroupBy(LambdaExpression groupBy);

        IDataManipulationSql SetSelect(object select);

        IDataManipulationSql SetHaving(List<LambdaExpression> having);

        IDataManipulationSql SetOrderBy(List<OrderedTableExpression> orderBy);

        IDataManipulationSql SetLimit(SkipTaked limit);
    }
}
