using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity;
using MySqlX.XDevAPI;

namespace Demo.Tables
{
    /// <summary>
    /// 专业
    /// </summary>
    [Table(nameof(Major), Schema = "testdb")]
    public class Major
    {
        /// <summary>
        /// 专业Id
        /// </summary>
        public int MajorId { get; set; }

        /// <summary>
        /// 专业名称
        /// </summary>
        public string MajorName { get; set; }

        /// <summary>
        /// 专业类型
        /// </summary>
        public int MajorType { get; set; }
    }
}
