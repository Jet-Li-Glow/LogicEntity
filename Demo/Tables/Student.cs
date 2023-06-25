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
    [Table(nameof(Student), Schema = "testdb")]
    public class Student
    {
        [Column("StudentId")]
        public int Id { get; set; }

        [Column("StudentName")]
        public string Name { get; set; }

        public DateTime? Birthday { get; set; }

        public Gender? Gender { get; set; }

        public int MajorId { get; set; }

        public Guid? Guid { get; set; }

        public byte[] Bytes { get; set; }

        public float Float { get; set; }

        public double Double { get; set; }

        public decimal Decimal { get; set; }

        public bool Bool { get; set; }

        public long Long { get; set; }

        public JsonObject Json { get; set; }

        public class JsonObject
        {
            public PropertyObject Object { get; set; }

            public int[] Array { get; set; }

            public List<int> List { get; set; }

            public Dictionary<string, object> Dictionary { get; set; }

            public class PropertyObject
            {
                public string Property { get; set; }
            }
        }
    }
}
