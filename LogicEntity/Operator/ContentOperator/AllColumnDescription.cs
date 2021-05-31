using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicEntity.Operator
{
    /// <summary>
    /// 所有列描述（延迟加载）
    /// </summary>
    public class AllColumnDescription : Description
    {
        private TableDescription _table;

        public AllColumnDescription(TableDescription table)
        {
            _table = table;
        }

        protected override string Content => _table.FinalTableName + ".*";
    }
}
