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
        public Column StudentId { get; init; }

        public Column StudentName { get; init; }

        public Column MajorId { get; init; }

        public Column Birthday { get; init; }
    }
}
