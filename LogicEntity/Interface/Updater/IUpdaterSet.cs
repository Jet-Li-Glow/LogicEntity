using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;

namespace LogicEntity.Interface
{
    public interface IUpdaterSet
    {
        public IUpdaterWhere Set<Table1>(Action<Table1> setValue) where Table1 : Table, new();

        public IUpdaterWhere Set<Table1, Table2>(Action<Table1, Table2> setValue) where Table1 : Table, new() where Table2 : Table, new();

        public IUpdaterWhere Set<Table1, Table2, Table3>(Action<Table1, Table2, Table3> setValue) where Table1 : Table, new() where Table2 : Table, new() where Table3 : Table, new();

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4>(Action<Table1, Table2, Table3, Table4> setValue) where Table1 : Table, new() where Table2 : Table, new() where Table3 : Table, new() where Table4 : Table, new();

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4, Table5>(Action<Table1, Table2, Table3, Table4, Table5> setValue) where Table1 : Table, new() where Table2 : Table, new() where Table3 : Table, new() where Table4 : Table, new() where Table5 : Table, new();

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4, Table5, Table6>(Action<Table1, Table2, Table3, Table4, Table5, Table6> setValue) where Table1 : Table, new() where Table2 : Table, new() where Table3 : Table, new() where Table4 : Table, new() where Table5 : Table, new() where Table6 : Table, new();

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4, Table5, Table6, Table7>(Action<Table1, Table2, Table3, Table4, Table5, Table6, Table7> setValue) where Table1 : Table, new() where Table2 : Table, new() where Table3 : Table, new() where Table4 : Table, new() where Table5 : Table, new() where Table6 : Table, new() where Table7 : Table, new();

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8>(Action<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8> setValue) where Table1 : Table, new() where Table2 : Table, new() where Table3 : Table, new() where Table4 : Table, new() where Table5 : Table, new() where Table6 : Table, new() where Table7 : Table, new() where Table8 : Table, new();

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9>(Action<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9> setValue) where Table1 : Table, new() where Table2 : Table, new() where Table3 : Table, new() where Table4 : Table, new() where Table5 : Table, new() where Table6 : Table, new() where Table7 : Table, new() where Table8 : Table, new() where Table9 : Table, new();

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10>(Action<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10> setValue) where Table1 : Table, new() where Table2 : Table, new() where Table3 : Table, new() where Table4 : Table, new() where Table5 : Table, new() where Table6 : Table, new() where Table7 : Table, new() where Table8 : Table, new() where Table9 : Table, new() where Table10 : Table, new();

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10, Table11>(Action<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10, Table11> setValue) where Table1 : Table, new() where Table2 : Table, new() where Table3 : Table, new() where Table4 : Table, new() where Table5 : Table, new() where Table6 : Table, new() where Table7 : Table, new() where Table8 : Table, new() where Table9 : Table, new() where Table10 : Table, new() where Table11 : Table, new();

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10, Table11, Table12>(Action<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10, Table11, Table12> setValue) where Table1 : Table, new() where Table2 : Table, new() where Table3 : Table, new() where Table4 : Table, new() where Table5 : Table, new() where Table6 : Table, new() where Table7 : Table, new() where Table8 : Table, new() where Table9 : Table, new() where Table10 : Table, new() where Table11 : Table, new() where Table12 : Table, new();

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10, Table11, Table12, Table13>(Action<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10, Table11, Table12, Table13> setValue) where Table1 : Table, new() where Table2 : Table, new() where Table3 : Table, new() where Table4 : Table, new() where Table5 : Table, new() where Table6 : Table, new() where Table7 : Table, new() where Table8 : Table, new() where Table9 : Table, new() where Table10 : Table, new() where Table11 : Table, new() where Table12 : Table, new() where Table13 : Table, new();

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10, Table11, Table12, Table13, Table14>(Action<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10, Table11, Table12, Table13, Table14> setValue) where Table1 : Table, new() where Table2 : Table, new() where Table3 : Table, new() where Table4 : Table, new() where Table5 : Table, new() where Table6 : Table, new() where Table7 : Table, new() where Table8 : Table, new() where Table9 : Table, new() where Table10 : Table, new() where Table11 : Table, new() where Table12 : Table, new() where Table13 : Table, new() where Table14 : Table, new();

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10, Table11, Table12, Table13, Table14, Table15>(Action<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10, Table11, Table12, Table13, Table14, Table15> setValue) where Table1 : Table, new() where Table2 : Table, new() where Table3 : Table, new() where Table4 : Table, new() where Table5 : Table, new() where Table6 : Table, new() where Table7 : Table, new() where Table8 : Table, new() where Table9 : Table, new() where Table10 : Table, new() where Table11 : Table, new() where Table12 : Table, new() where Table13 : Table, new() where Table14 : Table, new() where Table15 : Table, new();

        public IUpdaterWhere Set<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10, Table11, Table12, Table13, Table14, Table15, Table16>(Action<Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10, Table11, Table12, Table13, Table14, Table15, Table16> setValue) where Table1 : Table, new() where Table2 : Table, new() where Table3 : Table, new() where Table4 : Table, new() where Table5 : Table, new() where Table6 : Table, new() where Table7 : Table, new() where Table8 : Table, new() where Table9 : Table, new() where Table10 : Table, new() where Table11 : Table, new() where Table12 : Table, new() where Table13 : Table, new() where Table14 : Table, new() where Table15 : Table, new() where Table16 : Table, new();
    }
}
