using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Collections;
using DataBaseAccess.TableModel;

namespace DataBaseAccess
{
    public class MySQL
    {
        private string _connStr;

        public MySQL(string connStr)
        {
            _connStr = connStr;
        }

        public IEnumerable<T> Query<T>(string sql, params object[] args) where T : class, new()
        {
            return Query(typeof(T), sql, args: args).Cast<T>();
        }

        public IEnumerable Query(Type type, string sql, params object[] args)
        {
            //List<object> result = new();

            //try
            //{
            using (IDbConnection connection = new MySqlConnection(_connStr))
            {
                TypeCode typeCode = Type.GetTypeCode(type);

                if (typeCode == TypeCode.Empty)
                    yield break;

                connection.Open();

                var cmd = connection.CreateCommand();

                string[] paName = new string[args.Length];
                for (int i = 0; i < paName.Length; i++)
                {
                    string n = "@pa" + i.ToString();

                    paName[i] = n;

                    cmd.Parameters.Add(new MySqlParameter(n, args[i]));
                }


                cmd.CommandText = string.Format(sql, args: paName);

                cmd.CommandType = CommandType.Text;

                if (typeCode == TypeCode.Object)
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            object t = Activator.CreateInstance(type);

                            for (int i = 0; i < dr.FieldCount; i++)
                            {
                                var prop = type.GetProperty(dr.GetName(i), BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                                if (prop is not null)
                                    prop.SetValue(t, Convert.ChangeType(dr.GetValue(i), prop.PropertyType));
                            }

                            yield return t;
                        }

                    }
                }
                else
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            yield return Convert.ChangeType(dr[0], type);
                        }
                    }
                }
            }
            //}
            //catch (Exception ex)
            //{
            //    string msg = ex.Message;
            //}

            //return result;
        }

        public int ExecuteNonQuery(string sql, params object[] args)
        {
            using (IDbConnection connection = new MySqlConnection(_connStr))
            {
                connection.Open();

                var cmd = connection.CreateCommand();

                string[] paName = new string[args.Length];
                for (int i = 0; i < paName.Length; i++)
                {
                    string n = "@pa" + i.ToString();

                    paName[i] = n;

                    cmd.Parameters.Add(new MySqlParameter(n, args[i]));
                }


                cmd.CommandText = string.Format(sql, args: paName);

                cmd.CommandType = CommandType.Text;


                return cmd.ExecuteNonQuery();
            }
        }

        public object ExecuteScalar(string sql, params object[] args)
        {
            using (IDbConnection connection = new MySqlConnection(_connStr))
            {
                connection.Open();

                var cmd = connection.CreateCommand();

                string[] paName = new string[args.Length];
                for (int i = 0; i < paName.Length; i++)
                {
                    string n = "@pa" + i.ToString();

                    paName[i] = n;

                    cmd.Parameters.Add(new MySqlParameter(n, args[i]));
                }


                cmd.CommandText = string.Format(sql, args: paName);

                cmd.CommandType = CommandType.Text;


                return cmd.ExecuteScalar();
            }
        }

        public bool ExecuteTransaction(string sql, params object[] args)
        {
            using (IDbConnection connection = new MySqlConnection(_connStr))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                var transaction = connection.BeginTransaction();

                cmd.Transaction = transaction;

                string[] paName = new string[args.Length];
                for (int i = 0; i < paName.Length; i++)
                {
                    string n = "@pa" + i.ToString();

                    paName[i] = n;

                    cmd.Parameters.Add(new MySqlParameter(n, args[i]));
                }


                cmd.CommandText = string.Format(sql, args: paName);

                cmd.CommandType = CommandType.Text;

                try
                {
                    cmd.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch
                    {
                    }

                    return false;
                }
            }

            return true;
        }
    }
}
