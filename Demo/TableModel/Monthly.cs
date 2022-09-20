using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity;

namespace Demo.TableModel
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
        public Value<Guid> Guid { get; init; }

        /// <summary>
        /// 时间
        /// </summary>
        public Value<DateTime> DateTime { get; init; }

        /// <summary>
        /// 描述
        /// </summary>
        public Value<string> Description { get; init; }
    }
}
