using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Model;

namespace Demo.TableModel
{
    /// <summary>
    /// 专业
    /// </summary>
    public class Major : Table
    {
        /// <summary>
        /// 专业Id
        /// </summary>
        public Column MajorId { get; init; }

        /// <summary>
        /// 专业名称
        /// </summary>
        public Column MajorName { get; init; }

        /// <summary>
        /// 专业类型
        /// </summary>
        public Column MajorType { get; init; }
    }
}
