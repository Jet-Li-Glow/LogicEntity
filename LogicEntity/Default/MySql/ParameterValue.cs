using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Default.MySql
{
    internal class ParameterValue
    {
        public object CommantText { get; set; }

        public IEnumerable<KeyValuePair<string, object>> Parameters { get; set; }

        public bool IsConstant { get; set; } = false;

        public object ConstantValue { get; set; }

        public bool IsParamArray { get; set; } = false;

        public IEnumerable<string> ParamValues { get; set; }
    }
}
