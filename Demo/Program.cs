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
using LogicEntity.Model;
using LogicEntity.Interface;
using System.Threading.Tasks;
using System.Text;
using LogicEntity.EnumCollection;
using System.IO;

namespace DataBaseAccess
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-- Start --");


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
               major.MajorName,
               student.StudentName.Distinct().OrderBy(student.StudentId).ThenByDescending(student.StudentName).Separator("*").Group_Concat())
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
                    student.StudentId.As("StudentId"),
                    student.StudentName.As("StudentName"),
                    student.Birthday,
                    student.MajorId,
                    student.Guid,
                    student.Bytes,
                    major.MajorName,
                    major.MajorType
                    )
                .From(student)
                .LeftJoin(major).On(true & (major.MajorId == student.MajorId & major.MajorId > 0) & true)
                .Where(student.StudentId.In(1, ")3", 2, 4) &
                student.StudentId.In(DBOperator.Select(student.StudentId)
                                     .From(student)
                                     .Where(student.StudentId.In(1, 2, 4))));

            string tests = testselector.GetCommand().CommandText;

            //查询
            //List<StudentInfo> students = Database.TestDb.Query<StudentInfo>(testselector).ToList();

            //DataTable dt = Database.TestDb.Query(testselector);

            Student joinStudent = new Student(2019, 5);  //月表

            Major joinMajor = new Major();

            ISelector joinSelect = DBOperator.Select().From(joinStudent).FullJoin(joinMajor);

            joinSelect.SetCommandTimeout(20);

            string joinText = joinSelect.GetCommand().CommandText;

            //List<StudentInfo> joinResult = Database.TestDb.Query<StudentInfo>(joinSelect).ToList();


            ISelector njoinSelect = DBOperator.Select().From(joinStudent).Limit(1).ForUpdate();

            string njoinText = njoinSelect.GetCommand().CommandText;

            //List<StudentInfo> njoinResult = Database.TestDb.Query<StudentInfo>(njoinSelect).ToList();

            //多条件查询
            Student mulStudent = new Student();

            ConditionCollection conditions = new ConditionCollection();

            conditions.Add(mulStudent.StudentId == 10);
            conditions.Add(mulStudent.StudentId == 11);

            conditions.LogicalOperator = LogicalOperator.Or;

            ISelector mulConditon = DBOperator.Select().From(mulStudent).With(conditions);

            string nulCondText = mulConditon.GetCommand().CommandText;

            //List<StudentInfo> mulCondResult = Database.TestDb.Query<StudentInfo>(mulConditon).ToList();


            //List<Guid?> Ids = Database.TestDb.Query<Guid?>(DBOperator.Select(student.Guid).From(student).Where(student.StudentId < 10)).ToList();


            //List<StudentInfo> allStudents = Database.TestReadOnlyDb.Query<StudentInfo>(DBOperator.Select().From(new Student())).ToList();

            //int sc = Database.TestDb.ExecuteScalar<int>(DBOperator.Select(DbFunction.Last_Insert_Id()));

            //联合查询

            Student unionA = new Student();

            Student unionB = new Student();

            ISelector unionSelect = DBOperator.Select().From(unionA).Where(unionA.StudentId == 1)
                .UnionAll(DBOperator.Select().From(unionB).Where(unionB.StudentId == 2));

            string unionText = unionSelect.GetCommand().CommandText;

            //List<StudentInfo> unionResult = Database.TestDb.Query<StudentInfo>(unionSelect).ToList();

            //插入

            Student data = new Student();
            //data.StudentId.Value = 5;
            data.StudentName.Value = Path.GetRandomFileName();
            data.MajorId.Value = 2;
            data.Birthday.Value = DateTime.Now;
            data.Guid.Value = Guid.NewGuid();

            IInsertor insertor = DBOperator.Insert(data);

            string insertText = insertor.GetCommand().CommandText;

            //int insertAffected = Database.TestDb.ExecuteNonQuery(DBOperator.Insert(data));

            //ulong newId = Database.TestDb.InsertNext(data);

            //保存

            Student saveData = new Student();
            saveData.StudentId.Value = 71126;
            saveData.StudentName.Value = "小刘";
            saveData.MajorId.Value = 2;
            saveData.Birthday.Value = DateTime.Now;
            saveData.Guid.Value = Guid.NewGuid();

            IInsertor saver = DBOperator.Save(saveData);

            string saverText = saver.GetCommand().CommandText;

            //int saveAffected = Database.TestDb.ExecuteNonQuery(saver);

            //批量插入
            Student insertTable = new Student();

            Student data1 = new Student();
            data1.StudentId.Value = 14;
            data1.StudentName.Value = "小喵111";
            data1.Birthday.Value = DateTime.Now;
            data1.MajorId.Value = 7;

            Student data2 = new Student();
            data2.StudentId.Value = 15;
            data2.StudentName.Value = "小喵222";
            data2.Birthday.Value = DateTime.Now;
            data2.MajorId.Value = 7;

            Student data3 = new Student();
            data3.StudentId.Value = 16;
            data3.StudentName.Value = "小喵333";
            data3.Birthday.Value = DateTime.Now;
            data3.MajorId.Value = 7;

            IInsertor batch = DBOperator.InsertInto(insertTable)
                .Columns(insertTable.StudentId, insertTable.StudentName, insertTable.MajorId, insertTable.Birthday)
                .Row(data1, data2, data3)
                .OnDuplicateKeyUpdate((s, row) =>
                {
                    s.StudentName.Value = row.StudentName;
                    s.Birthday.Value = row.Birthday;
                    s.MajorId.Value = s.MajorId;
                });

            IInsertor batchB = DBOperator.InsertInto(insertTable)
                .Columns(insertTable.StudentId, insertTable.StudentName, insertTable.MajorId, insertTable.Birthday)
                .Row(data1, data2, data3)
                .OnDuplicateKeyUpdate((s) =>
                {
                    s.StudentName.Value = s.StudentName.Values();
                    s.Birthday.Value = s.Birthday.Values();
                    s.MajorId.Value = s.MajorId;
                });

            string batchText = batch.GetCommand().CommandText;

            //int batchAffected = Database.TestDb.ExecuteNonQuery(batch);

            //查询插入

            Student Target = new Student();

            Major OriginalData = new Major();

            IInsertor selectInsert = DBOperator.InsertInto(Target).Columns(Target.StudentName, Target.Birthday, Target.MajorId)
                .Rows(DBOperator.Select(OriginalData.MajorName.As("StudentName"),
                                        new Description("'2001-05-01 05:00:00'").As("Birthday"),
                                        OriginalData.MajorId)
                                        .From(OriginalData)
                                        .Where(OriginalData.MajorId == 3));

            string selectInsertText = selectInsert.GetCommand().CommandText;

            //int selectInsertAffected = Database.TestDb.ExecuteNonQuery(selectInsert);

            //更新
            Student changedStudent = new Student();

            changedStudent.StudentName.Value = changedStudent.StudentName;
            changedStudent.Birthday.Value = DateTime.Now;
            changedStudent.MajorId.Value = changedStudent.MajorId - 1;
            changedStudent.Guid.Value = Guid.NewGuid();
            changedStudent.Bytes.Value = Encoding.UTF8.GetBytes("Bytes");

            IUpdater changer = DBOperator.ApplyChanges(changedStudent).On(changedStudent.StudentId == 2 & changedStudent.MajorId > 0);

            string changerText = changer.GetCommand().CommandText;

            //int changeAffected = Database.TestDb.ExecuteNonQuery(changer);

            Major updateMajor = new Major();

            IUpdater updater = DBOperator.Update(changedStudent)
                .LeftJoin(updateMajor).On(changedStudent.MajorId == updateMajor.MajorId & updateMajor.MajorId == 1)
                .Set(s =>
                {
                    s.StudentName.Value = updateMajor.MajorName;
                    s.Birthday.Value = DateTime.Now;
                })
                .Where(changedStudent.StudentId == 4);

            string updaterText = updater.GetCommand().CommandText;

            //int updateAffected = Database.TestDb.ExecuteNonQuery(updater);

            //删除
            Student deleteTable = new Student();

            IDeleter deleter = DBOperator.Delete(deleteTable).Where(deleteTable.StudentId == 71116);

            string testdel = deleter.GetCommand().CommandText;

            //int delAffected = Database.TestDb.ExecuteNonQuery(deleter);

            //事务
            Student traStu = new Student();

            IDeleter traDel = DBOperator.Delete(traStu).Where(traStu.StudentId.In(71124, "aaa"));

            Student traStuB = new Student();
            traStuB.StudentId.Value = 71120;
            traStuB.StudentName.Value = "事务";

            IInsertor traInsert = DBOperator.Insert(traStuB);

            //bool success = Database.TestDb.ExecuteTransaction(new List<IDbOperator>() { traDel, traInsert }, out int affected, out Exception exception);

            //bool successB = Database.TestDb.ExecuteTransaction(traDel, traInsert);

            //事务2
            //using (DbTransaction transaction = Database.TestDb.BeginTransaction())
            //{
            //    try
            //    {
            //        List<Student> s = transaction.Query<Student>("select * from student where studentId < {0}", 10).ToList();

            //        List<DateTime?> ds = transaction.Query<DateTime?>("select birthday from student where studentId < {0}", 10).ToList();

            //        Student first = new Student();

            //        first.Birthday.Value = (s.First().Birthday.Value as DateTime?).Value.AddMonths(1);

            //        transaction.ExecuteNonQuery(DBOperator.ApplyChanges(first).On(first.StudentId == 2));

            //        transaction.ExecuteNonQuery("select");

            //        transaction.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        transaction.Rollback();
            //    }
            //}

            int a = 0;

            Console.WriteLine("-- End --");

            Console.ReadKey();
        }
    }

    /// <summary>
    /// 自定义函数
    /// </summary>
    static class MyDbFunction
    {
        public static Description MyFunction(this Description description)
        {
            return description?.Next(s => $"MyFunction({s})");
        }
    }

    class MySqlDb : AbstractDataBase
    {
        private readonly string _connectionStr;

        public MySqlDb(string connectionStr)
        {
            _connectionStr = connectionStr;
        }

        public override IDbConnection GetDbConnection()
        {
            return new MySqlConnection(_connectionStr);
        }
    }

    static class Database
    {
        public static MySqlDb TestDb = new("Database=testdb;Data Source=localhost;Port=1530;User Id=testuser;Password=logicentity2021;");

        /// <summary>
        /// 只读库
        /// </summary>
        public static MySqlDb TestReadOnlyDb = new("Database=testdb;Data Source=localhost;Port=1530;User Id=testuser;Password=logicentity2021;");
    }
}
