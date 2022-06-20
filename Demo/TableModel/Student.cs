using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Demo.Model;
using LogicEntity.Model;

namespace Demo.TableModel
{
    public class Student : Table
    {
        public Student()
        {
            AnotherName.ColumnName = "StudentName";

            Json.Reader = j =>
            {
                return JsonSerializer.Deserialize<Dictionary<string, object>>(j.ToString());
            };

            Json.Writer = j =>
            {
                if (j is string)
                    return j;

                return JsonSerializer.Serialize(j);
            };
        }

        public override string __SchemaName => "testdb";

        public override string __TableName => "Student";

        public Column StudentId { get; init; }

        public Column StudentName { get; init; }

        public Column AnotherName { get; init; }

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

        public Column Json { get; set; }
    }
}
