using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;

namespace DataBaseAccess.TableModel
{
    public class Student : Table
    {
        public override string schemaName => "testdb";

        public override string TableName => "Student";

        public Column StudentId { get; init; }

        public Column StudentName { get; init; }

        public Column MajorId { get; init; }

        public Column Birthday { get; init; }

        public Column Guid { get; init; }
    }
}
