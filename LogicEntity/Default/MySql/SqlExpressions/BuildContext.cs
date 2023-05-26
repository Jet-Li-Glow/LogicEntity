using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql.SqlExpressions
{
    internal record BuildContext
    {
        ParameterIndex _index = new ParameterIndex();

        public int Level { get; init; }

        public List<KeyValuePair<string, object>> SqlParameters { get; } = new();

        public IDataManipulationSql DataManipulationSql { get; init; }

        public LinqConvertProvider LinqConvertProvider { get; init; }

        public string GetParameterName()
        {
            string name = "@param" + _index.Value;

            _index.Value++;

            return name;
        }

        class ParameterIndex
        {
            public int Value { get; set; }
        }
    }
}
