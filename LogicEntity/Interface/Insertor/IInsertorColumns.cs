﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;

namespace LogicEntity.Interface
{
    public interface IInsertorColumns<T> where T : Table
    {
        public IInsertorValues<T> Columns(params Column[] columns);
    }
}