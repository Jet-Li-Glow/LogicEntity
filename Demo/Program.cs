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
using Demo.Tables;
using LogicEntity;
using LogicEntity.Collections.Generic;
using LogicEntity.Default.MySql;
using LogicEntity.Linq;
using LogicEntity.Method;
using MySql.Data.MySqlClient;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Version 1.0.1

            //开发计划 1.First().Id 适应性翻译
            //         2.Value<>实现 IComparable、等接口
            //         3.Assign方法适应Nullable<>
            //         4.Value<>的Json序列化和反序列化
            //         5.DateTimeOffset、TimeOnly等类型的支持
            //         6.join-on、select 无强类型限制 以实现动态join和动态select

            Console.WriteLine("-- Start --");

            //Db
            TestDb db = Database.TestDb;

            object data = null;

            int rowsAffected;

            //Select

            data = db.Students.ToList();

            data = db.Students.Select(s => new Student() { Id = s.Id }).ToList();

            data = db.Students.Select(s => new { Object = new { Nested = new { Id = s.Id } }, Array = new int[] { s.Id, s.Id + 1 } }).Take(1).ToList();

            data = db.Students.FirstOrDefault();

            data = db.Students.Max(s => s.Id);

            data = db.Students.Where(s => s.Id == 1).ToList();

            data = db.Students.Where(s => ((string)s.Name).Contains("小")).Take(1).ToList();

            data = db.Students.Where((s, i) => i > 0).ToList();

            data = db.Students.Join(db.Majors, (s, m) => s.MajorId == m.MajorId).Select((s, m) => new { s.Id, s.Name, m.MajorName }).ToList();

            data = db.Students.GroupBy(s => s.Id).Select(s => new { Key = s.Key, Name = s.Element.Name, GroupConcat = MyDbFunction.Group_Concat(s.Element.Name, "--"), Count = s.Count(), Sum = s.Sum(a => a.Id) }).Where(s => s.Key > 0).ToList();

            data = db.Students.Select((s, i) => new { s.Id, Index = i }).ToList();

            data = db.Students.Select(s => new { SubQuery = db.Majors.Where(m => m.MajorId == s.MajorId).Select(m => m.MajorName).First() }).ToList();

            data = db.Students.Union(db.Students).OrderBy(s => s.Id).Take(50).ToList();

            data = db.Students.Except(db.Students).OrderBy(s => s.Id).Take(50).ToList();

            data = db.Students.Intersect(db.Students).OrderBy(s => s.Id).Take(50).ToList();

            data = db.Students.OrderBy(s => s.Id).Skip(10).Take(10).ToList();

            data = db.Students.Take(100).Chunk(10).ToList();

            data = db.Students.Select(s => new { Id = ((int?)s.Id) ?? 0 }).Take(1).ToList();

            data = db.Students.Select(s => new { Id = s.Id > 0 ? (int)s.Id : -1 }).Take(1).ToList();

            data = db.Students.Select(s => s.Id.ToString() + "-" + s.Name).Take(1).ToList();

            data = db.Students.Select(s => ((DateTime)s.Birthday).AddDays(1)).Take(1).ToList();

            data = db.Students.Select(s => MyDbFunction.MyFunction(s.Id)).Take(1).ToList();

            data = db.Students.Select(s => s.Json).Take(1).ToList();

            data = db.Students.Select(s => MyConvert.ToDictionary(s.Json)).Take(1).ToList();

            data = db.Students.Select(s => ((Student.JsonObject)s.Json).Dictionary["Key - \" - \\ -"]).Take(1).ToList();

            data = db.Students.Take(1).Timeout(10).ToList();  //IDbCommand.CommandTimeout

            DataTable dataTable = db.Students.Take(1).Select(s => s.Id, s => s.Name);

            dataTable = db.Students.Take(1).Timeout(10).Select(s => s.Id, s => s.Name);  //IDbCommand.CommandTimeout

            data = db.Value(() => new { n = 1 }).RecursiveConcat(ns => ns.Where(s => s.n < 20).Select(s => new { n = s.n + 1 })).Take(20).ToList();

            data = db.Query<Student>("Select studentId + {0} As Id From Student Limit 10", 1).ToList();

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

            var autoIncrementId = db.Students.AddNext(new()
            {
                MajorId = 3,
                Name = "Auto Increment"
            });

            rowsAffected = db.Students.AddOrUpdate(new Student()
            {
                Id = new(() => db.Students.Max(s => s.Id)),
                Name = "Add or Update",
                MajorId = new(() => db.Majors.Max(m => m.MajorId))
            });

            rowsAffected = db.Students.AddOrUpdate(
                (oldValue, newValue) => new Student() { Name = oldValue.Name + " - " + newValue.Name + " - Update Factory" },
                new Student()
                {
                    Id = (int)autoIncrementId,
                    Name = "New Value",
                    MajorId = new(() => db.Majors.Max(m => m.MajorId))
                });

            rowsAffected = db.Students.AddRangeOrUpdate(db.Majors.Select(s => new Student()
            {
                Name = "Insert Into Select",
                MajorId = db.Majors.Max(m => m.MajorId)
            }).Take(1),
            (oldValue, newValue) => new Student() { Name = oldValue.Name + " - " + newValue.Name + " - Update Factory" });

            rowsAffected = db.Students.AddIgnore(new Student()
            {
                Id = (int)autoIncrementId,
                Name = "Insert Ignore",
                MajorId = new(() => db.Majors.Max(m => m.MajorId))
            });

            rowsAffected = db.Students.Replace(new Student()
            {
                Id = (int)autoIncrementId,
                Name = "Replaced Value",
                MajorId = new(() => db.Majors.Max(m => m.MajorId))
            });

            //Update

            rowsAffected = db.Students.Where(s => s.Id == 1)
                .Set(
                s => s.Float.SetValue(5.5f),
                s => ((Student.JsonObject)s.Json).Array[0].SetValue(-5)
                );

            rowsAffected = db.Students
                .Join(db.Majors, (s, m) => s.MajorId == m.MajorId)
                .Where((s, m) => m.MajorId == 3)
                .Set((s, m) => s.Name.SetValue("Joined Set" + m.MajorName));

            //Delete

            rowsAffected = db.Students.OrderByDescending(s => s.Id).Take(1).Remove();

            rowsAffected = db.Students
                .Join(db.Majors, (s, m) => s.MajorId == m.MajorId)
                .Where((s, m) => m.MajorId == db.Majors.Max(m => m.MajorId))
                .Remove((s, m) => s);

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

        [MethodFormat("Group_Concat({1} Separator {2})")]
        public static string Group_Concat(string val, string separator)
        {
            return default;
        }

        [MethodFormat("Sleep({1})")]
        public static int Sleep(int seconds)
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

            options.MemberFormat[typeof(object).GetMethod(nameof(object.ToString))] = "Cast({0} As Char)";  //Default implementation (can be omitted)

            options.PropertyConverters.Set<Student.JsonObject, string>(typeof(Student).GetProperty(nameof(Student.Json)), s => JsonSerializer.Deserialize<Student.JsonObject>(s), s => JsonSerializer.Serialize(s));

            return new LogicEntity.Default.MySql.LinqConvertProvider(options);
        }

        public ITable<Student> Students { get; init; }

        public ITable<Major> Majors { get; init; }

        public ITable<Monthly> Monthly { get; init; }
    }

    static class Database
    {
        public static string ConnectionStr = File.ReadAllText("ConnectionString.txt");

        public readonly static TestDb TestDb = new(ConnectionStr); //MySQL 8.0.31

        public readonly static TestDb TestReadOnlyDb = new(ConnectionStr);
    }
}
