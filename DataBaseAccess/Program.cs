using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataBaseAccess.TableModel;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Dapper;
using Dapper.Contrib.Extensions;
using LogicEntity;
using DataBaseAccess.Operator;
using System.Linq.Expressions;
using System.Reflection;
using LogicEntity.Operator;
using System.Data.Common;
using LogicEntity.Model;
using LogicEntity.Interface;

namespace DataBaseAccess
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-- Start --");


            TestDb testDb = new("Database=testdb;Data Source=localhost;Port=1530;User Id=testuser;Password=logicentity2021;");

            Student student = new();

            Major major = new();

            Major studentMajor = new();

            var nested = DBOperator.Select(student.StudentId).From(student).Where(student.StudentId == 1).As("nestedStudent");

            ISelector selector = DBOperator.Select(
               student.All(),
               nested.All(),
               nested.Column("nestedId").As("nestedTableId"),
               student.StudentId,
               student.StudentId.Count(),
               student.StudentId.Max().As("IdMax"),
               student.StudentId.Distinct().Count().As("sId"),
               student.StudentId,
               student.StudentId.As("a").Count().As("b"),
               student.StudentName,
               student.MajorId,
               studentMajor.MajorType,
               major.MajorName)
               .From(nested,
               studentMajor,
               student.As("cStudent"))
               .LeftJoin(major).On(student.MajorId == major.MajorId)
               .LeftJoin(studentMajor.As("StudentMajor")).On(student.MajorId == studentMajor.MajorId)
               .Where((student.StudentId >= 10 & student.StudentId <= 20 | student.StudentId == 1) | (student.StudentId >= 50 & student.StudentId <= 60 & student.StudentId == null) | student.StudentName.Like("张三")
               | student.StudentId.In(new List<int>() { 1, 2, 3, 4, 5 }))
               .GroupBy(student.StudentId, major.MajorId)
               .Having(studentMajor.MajorType == 1 & new Description("myId").Max() == 2 & student.StudentId.Min() > 1)
               .OrderBy(student.StudentId)
               .ThenBy(studentMajor.MajorId)
               .ThenByDescending(studentMajor.MajorType)
               .Limit(5, 10);

            Command command = selector.GetCommand();

            string entitySql = command.CommandText;

            List<KeyValuePair<string, object>> paramC = command.Parameters.ToList();

            ISelector testselector = DBOperator.Select(
                    student.StudentId.As("Id"),
                    student.StudentName.As("Name"),
                    student.Birthday,
                    student.MajorId,
                    major.MajorName,
                    major.MajorType
                    )
                .From(student)
                .LeftJoin(major).On(major.MajorId == student.MajorId)
                .Where(student.Birthday > new DateTime(1996, 2, 12, 7, 0, 0) & student.StudentId.In(1, ")3"));

            string tests = testselector.GetCommand().CommandText;

            //查询
            //List<StudentInfo> students = testDb.Query<StudentInfo>(testselector).ToList();

            //DataTable dt = testDb.Query(testselector);

            Student data = new Student();
            //data.StudentId.Value = 5;
            data.StudentName.Value = "小刘";
            data.Birthday.Value = new DateTime(2001, 3, 5);
            data.MajorId.Value = 2;

            //插入
            IInsertor insertor = DBOperator.Insert(data);

            //int insertAffected = testDb.ExecuteNonQuery(insertor);

            //删除
            Student deleteTable = new Student();

            IDeleter deleter = DBOperator.Delete().From(deleteTable).Where(deleteTable.StudentId >= 5);

            string testdel = deleter.GetCommand().CommandText;

            //int delAffected = testDb.ExecuteNonQuery(deleter);

            int a = 0;
            //string r = System.Text.Json.JsonSerializer.Serialize(models);

            List<int> t = new List<int>().DefaultIfEmpty(5)
                .OrderBy(s => s + a)
                .OrderByDescending(s => s)
                .ThenBy(s => s)
                .ThenByDescending(s => s)
                .ToList();

            //new MySqlConnection().Query("");

            Console.WriteLine("-- End --");

            Console.ReadKey();
        }
    }

    class TestDb : AbstractDataBase
    {
        private readonly string _connectionStr;

        public TestDb(string connectionStr)
        {
            _connectionStr = connectionStr;
        }

        public override IDbConnection GetDbConnection()
        {
            return new MySqlConnection(_connectionStr);
        }

        public override IDataParameter GetDbParameter(string key, object value)
        {
            return new MySqlParameter(key, value);
        }
    }
}
