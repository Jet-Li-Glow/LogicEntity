using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;

namespace Demo.TableModel
{
    /// <summary>
    /// 月表
    /// </summary>
    public class Monthly : Table
    {
        string _tableName;

        public Monthly()
        {
            string _tableName = nameof(Monthly);
        }

        public Monthly(DateTime dateTime)
        {
            _tableName = nameof(Monthly) + dateTime.ToString("_yyyy_MM");
        }

        public override string __TableName => _tableName;

        /// <summary>
        /// Guid
        /// </summary>
        public Column Guid { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public Column DateTime { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public Column Description { get; set; }
    }
}
