using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model
{
    public class StudentInfo
    {
        public int StudentId { get; set; }

        public string StudentName { get; set; }

        public DateTime? Birthday { get; set; }

        public Gender Gender { get; set; }

        public int MajorId { get; set; }

        public string MajorName { get; set; }

        public int MajorType { get; set; }

        public Guid? Guid { get; set; }

        public byte[] Bytes { get; set; }

        public float Float { get; set; }

        public double Double { get; set; }

        public decimal Decimal { get; set; }

        public bool Bool { get; set; }

        public long Long { get; set; }

        public Dictionary<string, object> Json { get; set; }
    }
}
