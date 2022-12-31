using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Demo.Model;
using LogicEntity;
using LogicEntity.Json;

namespace Demo.Tables
{
    [Table("Student", Schema = "testdb")]
    public class Student
    {
        [Column("StudentId")]
        public Value<int> Id { get; set; }

        [Column("StudentName")]
        public Value<string> Name { get; set; }

        [Column("StudentName")]
        public Value<string> AnotherName { get; set; }

        public Value<DateTime?> Birthday { get; set; }

        public Value<Gender?> Gender { get; set; }

        public Value<int> MajorId { get; set; }

        public Value<Guid?> Guid { get; set; }

        public Value<byte[]> Bytes { get; set; }

        public Value<float> Float { get; set; }

        public Value<double> Double { get; set; }

        public Value<decimal> Decimal { get; set; }

        public Value<bool> Bool { get; set; }

        public Value<long> Long { get; set; }

        public Value<JsonObject> Json { get; set; }

        [Column("Json")]
        [JsonPath("$.Array")]
        public int[] JsonArray { get; set; }

        public class JsonObject
        {
            public PropertyObject Object { get; set; }

            public int[] Array { get; set; }

            public Dictionary<string, object> Dictionary { get; set; }

            public class PropertyObject
            {
                public string Property { get; set; }
            }
        }
    }
}
