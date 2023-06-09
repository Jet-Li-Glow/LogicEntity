using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal class OriginalTableExpression : SqlExpression, ISelectSql
    {
        public OriginalTableExpression(string schema, string name)
        {
            Schema = schema;

            Name = name;
        }

        public string Schema { get; private set; }

        public string Name { get; private set; }

        string _alisas;

        public string Alias
        {
            get
            {
                return _alisas;
            }

            set
            {
                _alisas = value;

                HasAlias = true;
            }
        }

        public bool HasAlias { get; private set; } = false;

        public string ShortName => HasAlias ? Alias : FullName;

        public OrderKeys OrderBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public OffsetLimit Limit { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int? Timeout { get; set; }

        public List<CommonTableExpression> CommonTableExpressions => throw new NotImplementedException();

        public string FullName
        {
            get
            {
                string tableName = SqlNode.SqlName(Name);

                if (string.IsNullOrEmpty(Schema) == false)
                    tableName = SqlNode.Member(SqlNode.SqlName(Schema), tableName);

                return tableName;
            }
        }

        public bool IsVector { get; } = true;

        public IList<ColumnInfo> Columns => throw new NotImplementedException();

        public SqlCommand BuildTableDefinition(BuildContext context)
        {
            return new()
            {
                Text = FullName
            };
        }

        public SelectExpression AddJoin()
        {
            return new SelectExpression(this).AddJoin();
        }

        public SelectSqlCommand BuildSelect(BuildContext context)
        {
            return new SelectExpression(this)
            {
                Type = Type
            }.BuildSelect(context);
        }

        public ISqlExpression[] GetOrderByParameters()
        {
            throw new NotImplementedException();
        }

        public bool CanAddNode(SelectNodeType nodeType)
        {
            return true;
        }

        public SelectExpression AddSelect()
        {
            return new SelectExpression(this).AddSelect();
        }

        public SelectExpression Distinct()
        {
            return new SelectExpression(this)
            {
                Type = Type
            }.Distinct();
        }

        public SelectExpression AddIndex()
        {
            return new SelectExpression(this)
            {
                Type = Type
            }.AddIndex();
        }

        public SelectExpression AddWhere()
        {
            return new SelectExpression(this)
            {
                Type = Type
            }.AddWhere();
        }

        public SelectExpression AddGroupBy()
        {
            return new SelectExpression(this).AddGroupBy();
        }

        public SelectExpression AddHaving()
        {
            return new SelectExpression(this)
            {
                Type = Type
            }.AddHaving();
        }

        public ISelectSql AddOrderBy()
        {
            return new SelectExpression(this)
            {
                Type = Type
            }.AddOrderBy();
        }

        public ISelectSql AddThenBy()
        {
            throw new NotImplementedException();
        }

        public ISelectSql AddLimit()
        {
            return new SelectExpression(this)
            {
                Type = Type
            }.AddLimit();
        }

        public DeleteExpression AddDelete()
        {
            return new SelectExpression(this).AddDelete();
        }

        public UpdateExpression AddUpdateSet()
        {
            return new SelectExpression(this).AddUpdateSet();
        }
    }
}
