using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Model;
using LogicEntity.Model;

namespace Demo.TableModel
{
    public class Student : Table
    {
        /// <summary>
        /// 月表
        /// </summary>
        int _year;

        int _month;

        public Student()
        {
        }

        public override string SchemaName => "testdb";

        public override string TableName => "Student";

        public Column StudentId { get; init; }

        public Column StudentName { get; init; }

        public Column Birthday { get; init; }

        public Column Gender { get; init; }

        public Column MajorId { get; init; }

        public Column Guid { get; init; }

        public Column Bytes { get; set; }

        public Column Float { get; set; }

        public Column Double { get; set; }

        public Column Decimal { get; set; }

        public Column Bool { get; set; }

        public Column Long { get; set; }
    }
}
