using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Collections;
using LogicEntity.Default.MySql.SqlExpressions;
using LogicEntity.Linq.Expressions;
using Microsoft.VisualBasic;

namespace LogicEntity.Default.MySql.Linq.Expressions
{
    internal class AddOrUpdateWithFactoryOperateExpression : OperateExpression
    {
        public AddOrUpdateWithFactoryOperateExpression(InsertExpression.AddOperateType addOperate, TableExpression source, IEnumerable elements, bool update, LambdaExpression updateFactory)
        {
            ArgumentNullException.ThrowIfNull(source);

            ArgumentNullException.ThrowIfNull(elements);

            AddOperate = addOperate;

            Source = source;

            if (elements is IDataTable dataTable)
            {
                DataTable = dataTable;

                DataSource = AddDataSource.DataTable;
            }
            else
            {
                Elements = elements;

                DataSource = AddDataSource.Entity;
            }

            Update = update;

            UpdateFactory = updateFactory;
        }

        public AddOrUpdateWithFactoryOperateExpression(InsertExpression.AddOperateType addOperate, TableExpression source, IEnumerable elements) : this(addOperate, source, elements, false, null)
        {

        }

        public AddOrUpdateWithFactoryOperateExpression(InsertExpression.AddOperateType addOperate, TableExpression source, IEnumerable elements, LambdaExpression updateFactory) : this(addOperate, source, elements, true, updateFactory)
        {

        }

        public InsertExpression.AddOperateType AddOperate { get; private set; }

        public TableExpression Source { get; private set; }

        public IEnumerable Elements { get; private set; }

        public IDataTable DataTable { get; private set; }

        public AddDataSource DataSource { get; private set; }

        public bool Update { get; private set; }

        public LambdaExpression UpdateFactory { get; private set; }
    }

    internal enum AddDataSource
    {
        Entity,
        DataTable
    }
}
