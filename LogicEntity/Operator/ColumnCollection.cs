using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 列集合
    /// </summary>
    public class ColumnCollection : IEnumerable<Column>
    {
        List<Column> _columns = new();

        ColumnCollection(Column columns)
        {
            _columns.Add(columns);
        }

        internal ColumnCollection(IEnumerable<Column> columns)
        {
            _columns.AddRange(columns);
        }

        public IEnumerator<Column> GetEnumerator()
        {
            return _columns.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static implicit operator ColumnCollection(Column column)
        {
            return new ColumnCollection(column);
        }

        public static implicit operator ColumnCollection(Description description)
        {
            return new ColumnCollection(new Column(description));
        }
    }
}
