using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity;

namespace Demo.Tables
{
    /// <summary>
    /// 月表
    /// </summary>
    public class Monthly
    {
        string _tableName;

        public Monthly()
        {
            _tableName = nameof(Monthly);
        }

        public Monthly(DateTime dateTime)
        {
            _tableName = nameof(Monthly) + dateTime.ToString("_yyyy_MM");
        }

        /// <summary>
        /// Guid
        /// </summary>
        public Value<Guid> Guid { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public Value<DateTime> DateTime { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public Value<string> Description { get; set; }
    }
}
