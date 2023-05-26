using LogicEntity.Default.MySql.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class InsertExpression
    {
        public AddOperateType AddOperate { get; set; }

        public OriginalTableExpression Table { get; set; }

        public IReadOnlyCollection<PropertyInfo> Columns { get; set; }

        public IInsertRowsExpression Rows { get; set; }

        public bool OnDuplicateKeyUpdate { get; set; } = false;

        public IReadOnlyCollection<AssignmentExpression> Assignments { get; set; }

        public UpdateFactoryVersion UpdateFactoryVersion { get; set; } = UpdateFactoryVersion.V5_7;

        public Command Build()
        {
            Command command = new();

            BuildContext context = new();

            string onDuplicateKeyUpdate = string.Empty;

            if (OnDuplicateKeyUpdate)
            {
                onDuplicateKeyUpdate = "\nOn Duplicate Key Update\n" + string.Join(",\n", Assignments.Select(s => s.BuildValue(context).Text)).Indent(2);

                if (UpdateFactoryVersion == UpdateFactoryVersion.V8_0)
                    onDuplicateKeyUpdate = $"\nAs {SqlNode.NewRowAlias}" + onDuplicateKeyUpdate;
            }

            InsertRowsSqlCommand rowsCommand = Rows.BuildRows(context);

            if (Columns is null)
                Columns = rowsCommand.ColumnMembers.Cast<PropertyInfo>().ToList();

            string addOperate = string.Empty;

            if (AddOperate == AddOperateType.Insert)
                addOperate = "Insert Into";
            else if (AddOperate == AddOperateType.InsertIgnore)
                addOperate = "Insert Ignore";
            else if (AddOperate == AddOperateType.Replace)
                addOperate = "Replace Into";
            else
                throw new NotImplementedException("Unsupported AddOperateType");

            command.CommandText = addOperate + " "
                + Table.BuildTableDefinition(context).Text
                + "\n(\n" + string.Join(",\n", Columns.Select(p => SqlNode.SqlName(SqlNode.ColumnName(p)))).Indent(2) + "\n)"
                + "\n" + rowsCommand.Text
                + onDuplicateKeyUpdate;

            command.Parameters.AddRange(context.SqlParameters);

            command.CommandTimeout = Table.Timeout;

            return command;
        }

        public enum AddOperateType
        {
            Insert,
            InsertIgnore,
            Replace
        }
    }
}
