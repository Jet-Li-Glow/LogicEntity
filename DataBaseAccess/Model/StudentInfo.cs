﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBaseAccess.TableModel;

namespace DataBaseAccess.Operator
{
    public class StudentInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? Birthday { get; set; }

        public int MajorId { get; set; }

        public string MajorName { get; set; }

        public int MajorType { get; set; }
    }
}
