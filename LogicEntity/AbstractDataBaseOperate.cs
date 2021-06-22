using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicEntity.Interface;
using LogicEntity.Model;
using LogicEntity.Operator;

namespace LogicEntity
{
    /// <summary>
    /// 数据库操作
    /// </summary>
    public abstract partial class AbstractDataBase
    {
        /// <summary>
        /// 插入下一条数据，并返回自增主键
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public ulong InsertNext<T>(T row) where T : Table, new()
        {
            Command insert = DBOperator.Insert(row).GetCommand();

            using (IDbConnection connection = GetDbConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                foreach (KeyValuePair<string, object> kv in insert.Parameters)
                {
                    IDbDataParameter parameter = command.CreateParameter();

                    parameter.ParameterName = kv.Key;

                    parameter.Value = kv.Value;

                    command.Parameters.Add(parameter);
                }

                command.CommandText = insert.CommandText;

                command.CommandType = CommandType.Text;

                command.ExecuteNonQuery();


                command.CommandText = DBOperator.Select(DbFunction.Last_Insert_Id()).GetCommand().CommandText;

                command.Parameters.Clear();

                return (ulong)Convert.ChangeType(command.ExecuteScalar(), TypeCode.UInt64);
            }
        }
    }
}
