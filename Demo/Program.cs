using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataBaseAccess.TableModel;
using MySql.Data.MySqlClient;
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
                .LeftJoin(major).On(true | major.MajorId == student.MajorId & true & major.MajorId == 2 & true)
                .Where(student.Birthday > new DateTime(1996, 2, 12, 7, 0, 0) & student.StudentId.In(1, ")3"));

            string tests = testselector.GetCommand().CommandText;

            //查询
            //List<StudentInfo> students = testDb.Query<StudentInfo>(testselector).ToList();

            //DataTable dt = testDb.Query(testselector);


            //插入

            Student data = new Student();
            //data.StudentId.Value = 5;
            data.StudentName.Value = "小刘";
            data.Birthday.Value = null; // new DateTime(2001, 3, 5);
            data.MajorId.Value = 2;

            IInsertor insertor = DBOperator.Insert(data);

            //int insertAffected = testDb.ExecuteNonQuery(insertor);

            //批量插入
            Student insertTable = new Student();

            Student data1 = new Student();
            data1.StudentName.Value = "小喵1";
            data1.Birthday.Value = new DateTime(1995, 5, 10);
            data1.MajorId.Value = 1;

            Student data2 = new Student();
            data2.StudentName.Value = "小喵2";
            data2.Birthday.Value = new DateTime(1996, 5, 10);
            data2.MajorId.Value = 2;

            Student data3 = new Student();
            data3.StudentName.Value = "小喵3";
            data3.Birthday.Value = new DateTime(1997, 5, 10);
            data3.MajorId.Value = 3;

            IInsertor batch = DBOperator.InsertInto(insertTable)
                .Columns(insertTable.StudentName, insertTable.MajorId, insertTable.Birthday)
                .Rows(new List<Student>() { data1, data2, data3 });

            string batchText = batch.GetCommand().CommandText;

            //int batchAffected = testDb.ExecuteNonQuery(batch);

            //更新
            Student changedStudent = new Student();

            changedStudent.StudentName.Value = "小刘123";
            changedStudent.MajorId.Value = 4;

            IUpdater changer = DBOperator.ApplyChanges(changedStudent).On(changedStudent.StudentName == "小刘" & changedStudent.MajorId == 2);

            string changerText = changer.GetCommand().CommandText;

            //int changeAffected = testDb.ExecuteNonQuery(changer);

            Major updateMajor = new Major();

            IUpdater updater = DBOperator.Update(changedStudent)
                .LeftJoin(updateMajor).On(changedStudent.MajorId == updateMajor.MajorId & updateMajor.MajorId == 1)
                .Set(s => s.StudentName.Value = updateMajor.MajorName)
                .Where(changedStudent.StudentId == 4);

            string updaterText = updater.GetCommand().CommandText;

            //int updateAffected = testDb.ExecuteNonQuery(updater);

            //删除
            Student deleteTable = new Student();

            IDeleter deleter = DBOperator.Delete().From(deleteTable).Where(deleteTable.StudentId >= 5);

            string testdel = deleter.GetCommand().CommandText;

            //int delAffected = testDb.ExecuteNonQuery(deleter);

            int a = 0;

            List<int> t = new List<int>().DefaultIfEmpty(5)
                .OrderBy(s => s + a)
                .OrderByDescending(s => s)
                .ThenBy(s => s)
                .ThenByDescending(s => s)
                .ToList();

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
