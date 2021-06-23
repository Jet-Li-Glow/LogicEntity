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
        /// <summary>
        /// 月表
        /// </summary>
        int _year;

        int _month;

        public Student()
        { 
        }

        public Student(int year, int month)
        {
            _year = year;
            _month = month;
        }

        public override string schemaName => "testdb";

        public override string TableName => "Student";

        public Column StudentId { get; init; }

        public Column StudentName { get; init; }

        public Column MajorId { get; init; }

        public Column Birthday { get; init; }

        public Column Guid { get; init; }

        public Column Bytes { get; set; }
    }
}
