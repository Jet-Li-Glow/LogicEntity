using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Xml;
using Demo.Model;
using Demo.TableModel;
using LogicEntity;
using LogicEntity.Default.MySql;
using LogicEntity.Method;
using MySql.Data.MySqlClient;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Version 1.0.0

            //开发计划  1.Develop

            Console.WriteLine("-- Start --");

            //Db
            TestDb db = Database.TestDb;

            object data = null;

            int rowsAffected;

            //Select

            data = db.Students.ToList();

            data = db.Students.Where(s => s.Id == 1).ToList();

            data = db.Students.Where(s => ((string)s.Name).Contains("123")).ToList();

            data = db.Students.Where((s, i) => i > 0).ToList();

            data = db.Students.Join(db.Majors, (s, m) => s.MajorId == m.MajorId).Select((s, m) => new { s.Id, s.Name, m.MajorName }).ToList();

            data = db.Students.GroupBy(s => s.Id).Select(s => new { Key = s.Key, Name = s.Element.Name, Count = s.Count(), Sum = s.Sum(a => a.Id) }).Where(s => s.Key > 0);

            data = db.Students.Select((s, i) => new { s.Id, Index = i }).ToList();

            data = db.Students.Select(s => new { SubQuery = db.Majors.Where(m => m.MajorId == s.MajorId).Select(m => m.MajorName).First() }).ToList();

            data = db.Students.OrderBy(s => s.Id).Skip(10).Take(10);

            data = db.Students.Select(s => new { Id = ((int?)s.Id) ?? 0 }).Take(1).ToList();

            data = db.Students.Select(s => new { Id = s.Id > 0 ? (int)s.Id : -1 }).Take(1).ToList();

            data = db.Students.Select(s => s.Id.ToString() + "-" + s.Name).Take(1).ToList();

            data = db.Students.Select(s => ((DateTime)s.Birthday).AddDays(1)).Take(1).ToList();

            data = db.Students.Select(s => MyDbFunction.MyFunction(s.Id)).Take(1).ToList();

            data = db.Students.Select(s => s.Json).Take(1).ToList();

            data = db.Students.Select(s => MyConvert.ToDictionary(s.Json)).Take(1).ToList();

            data = db.Students.Select(s => s.JsonArray[4]).Take(1).ToList();

            data = db.Students.Select(s => ((Student.JsonObject)s.Json).Dictionary["Key - \" - \\ -"]).Take(1).ToList();

            DataTable dataTable = db.Students.Take(1).Select(s => s.Id, s => s.Name);

            //Insert

            rowsAffected = db.Students.Add(new Student()
            {
                Name = Path.GetRandomFileName(),
                MajorId = 3,
                Json = new Student.JsonObject()
                {
                    Object = new()
                    {
                        Property = "Insert Property Value"
                    }
                }
            });

            //Update

            rowsAffected = db.Students.Where(s => s.Id == 1).Set(s => s.Float.Assign(5.5f));

            //Delete

            rowsAffected = db.Students.OrderByDescending(s => s.Id).Take(1).Remove();

            //Transaction

            db.ExecuteTransaction(transaction =>
            {
                try
                {
                    rowsAffected = db.Students.OrderByDescending(s => s.Id).Take(1).Remove();

                    Service.Add();

                    Service.Remove();

                    throw new Exception();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }, IsolationLevel.ReadCommitted);

            Test.Run();

            Console.WriteLine("-- End --");

            Console.ReadKey();
        }

        //static void Test()
        //{
        //    ISelector selector = DBOperator.Select(new ValueExpression("Sleep(10)"));

        //    selector.SetTimeout(5);

        //    try
        //    {
        //        int v = Database.TestDb.Query<int>(selector).FirstOrDefault();
        //    }
        //    catch
        //    {
        //    }

        //    Guid guid = Guid.NewGuid();

        //    //插入
        //    Monthly data = new(new DateTime(2021, 12, 1));

        //    data.Guid.Value = guid;
        //    data.DateTime.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //    data.Description.Value = Path.GetRandomFileName() + " Monthly Test";

        //    Database.TestDb.ExecuteNonQuery(DBOperator.Save(data));

        //    Monthly table = new(new DateTime(2021, 12, 1));

        //    Monthly d = Database.TestDb.Query<Monthly>(DBOperator.Select().From(table).Where(table.Guid == data.Guid.Value)).Single();

        //    Assert(d.Guid.Value, data.Guid.Value);
        //    Assert(d.DateTime.Value, data.DateTime.Value);
        //    Assert(d.Description.Value, data.Description.Value);

        //    //更新
        //    data = new(new DateTime(2021, 12, 1));

        //    data.DateTime.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //    data.Description.Value = Path.GetRandomFileName() + " Monthly Test";

        //    Database.TestDb.ExecuteNonQuery(DBOperator.ApplyChanges(data).On(data.Guid == guid));

        //    d = Database.TestDb.Query<Monthly>(DBOperator.Select().From(table).Where(table.Guid == guid)).Single();

        //    Assert(d.Guid.Value, guid);
        //    Assert(d.DateTime.Value, data.DateTime.Value);
        //    Assert(d.Description.Value, data.Description.Value);

        //    //删除
        //    table = new(new DateTime(2021, 12, 1));

        //    Database.TestDb.ExecuteNonQuery(DBOperator.DeleteFrom(table).Where(table.Guid == guid));

        //    Assert(Database.TestDb.Query<Monthly>(DBOperator.Select().From(table).Where(table.Guid == data.Guid.Value)).Count() == 0);

        //    //Json
        //    Database.TestDb.Query(
        //        DBOperator.Select(new ValueExpression("{0}", JsonSerializer.Serialize(new object[] { new { Id = 1 }, "name", 100 })).Json_Contains_Path(OneOrAll.One, "$[1]")));

        //    Database.TestDb.Query(
        //        DBOperator.Select(new ValueExpression("{0}", JsonSerializer.Serialize(new object[] { new { Id = 1 }, "name", 100 })).Json_Search(OneOrAll.All, "name", null, "$")));

        //    Database.TestDb.Query(
        //        DBOperator.Select(new ValueExpression("{0}", JsonSerializer.Serialize(new object[] { new { Id = 1 }, "name", 100 })).Json_Search(OneOrAll.All, "name", 'a', "$")));
        //}
    }

    static class MyConvert
    {
        public static object ToDictionary(object value)
        {
            return JsonSerializer.Deserialize<Dictionary<string, object>>(value.ToString());
        }
    }

    /// <summary>
    /// 自定义函数
    /// </summary>
    static class MyDbFunction
    {
        [MethodFormat("({1} + 1)")]
        public static object MyFunction(this Value<int> valueExpression)
        {
            return default;
        }
    }

    static class Service
    {
        public static void Add()
        {
            Database.TestDb.Students.Add(new Student()
            {
                Name = "Service Add",
                MajorId = 3,
                Json = new Student.JsonObject()
                {
                    Object = new()
                    {
                        Property = "Insert Property Value"
                    }
                }
            });
        }

        public static void Remove()
        {
            Database.TestDb.Students.OrderByDescending(s => s.Id).Take(1).Remove();
        }
    }

    class TestDb : AbstractDataBase
    {
        string _connectionStr;

        public TestDb(string connectionStr)
        {
            _connectionStr = connectionStr;
        }

        protected override IDbConnection CreateDbConnection()
        {
            return new MySqlConnection(_connectionStr);
        }

        protected override ILinqConvertProvider GetLinqConvertProvider()
        {
            LinqConvertOptions options = new();

            options.PropertyConverters.Set<Student.JsonObject, string>(typeof(Student).GetProperty(nameof(Student.Json)), s => JsonSerializer.Deserialize<Student.JsonObject>(s), s => JsonSerializer.Serialize(s));
            options.PropertyConverters.Set<int[], string>(typeof(Student).GetProperty(nameof(Student.JsonArray)), s => JsonSerializer.Deserialize<int[]>(s), s => JsonSerializer.Serialize(s));

            return new LogicEntity.Default.MySql.LinqConvertProvider(options);
        }

        public ITable<Student> Students { get; init; }

        public ITable<Major> Majors { get; init; }
    }

    static class Database
    {
        static string connectionStr = File.ReadAllText("ConnectionString.txt");

        public readonly static TestDb TestDb = new(connectionStr);

        public readonly static TestDb TestReadOnlyDb = new(connectionStr);
    }
}
