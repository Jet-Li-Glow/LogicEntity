﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Operator;

namespace LogicEntity.Interface
{
    public interface IOrderBy : ILimit
    {
        public IThenBy OrderBy(Description description);

        public IThenBy OrderByDescending(Description description);
    }
}
