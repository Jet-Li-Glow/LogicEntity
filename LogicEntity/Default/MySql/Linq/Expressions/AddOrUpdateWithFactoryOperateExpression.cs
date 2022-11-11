using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Collections;
using LogicEntity.Linq.Expressions;
using Microsoft.VisualBasic;

namespace LogicEntity.Default.MySql.Linq.Expressions
{
    internal class AddOrUpdateWithFactoryOperateExpression : OperateExpression
    {
        public AddOrUpdateWithFactoryOperateExpression(TableExpression source, IEnumerable<object> elements, bool update)
        {
            if (elements is null)
                throw new ArgumentNullException(nameof(elements));

            Source = source;

            Elements = elements;

            Update = update;

            DataSource = AddDataSource.Entity;
        }

        public AddOrUpdateWithFactoryOperateExpression(string addOperate, TableExpression source, IEnumerable<object> elements) : this(source, elements, false)
        {
            if (string.IsNullOrEmpty(addOperate))
                throw new ArgumentNullException(nameof(addOperate));

            AddOperate = addOperate;
        }

        public AddOrUpdateWithFactoryOperateExpression(TableExpression source, IEnumerable<object> elements, LambdaExpression updateFactory) : this(source, elements, true)
        {
            if (updateFactory is null)
                throw new ArgumentNullException(nameof(updateFactory));

            UpdateFactory = updateFactory;
        }

        public AddOrUpdateWithFactoryOperateExpression(TableExpression source, IDataTable dataTable, bool update)
        {
            if (dataTable is null)
                throw new ArgumentNullException(nameof(dataTable));

            Source = source;

            DataTable = dataTable;

            Update = update;

            DataSource = AddDataSource.DataTable;
        }

        public AddOrUpdateWithFactoryOperateExpression(string addOperate, TableExpression source, IDataTable dataTable) : this(source, dataTable, false)
        {
            if (string.IsNullOrEmpty(addOperate))
                throw new ArgumentNullException(nameof(addOperate));

            AddOperate = addOperate;
        }

        public AddOrUpdateWithFactoryOperateExpression(TableExpression source, IDataTable dataTable, LambdaExpression updateFactory) : this(source, dataTable, true)
        {
            if (updateFactory is null)
                throw new ArgumentNullException(nameof(updateFactory));

            UpdateFactory = updateFactory;
        }

        public string AddOperate { get; private set; } = "Insert Into";

        public TableExpression Source { get; private set; }

        public IEnumerable<object> Elements { get; private set; }

        public bool Update { get; private set; }

        public AddDataSource DataSource { get; private set; }

        public IDataTable DataTable { get; private set; }

        public LambdaExpression UpdateFactory { get; private set; }
    }

    internal enum AddDataSource
    {
        Entity,
        DataTable
    }
}
