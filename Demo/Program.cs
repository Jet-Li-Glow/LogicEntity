using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Demo.Model;
using Demo.TableModel;
using LogicEntity;
using LogicEntity.EnumCollection;
using LogicEntity.Interface;
using LogicEntity.Model;
using LogicEntity.Operator;
using MySql.Data.MySqlClient;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-- Start --");


            //

            MySqlDb myDb = Database.TestDb;

            Student student = new();

            Major major = new();

            ISelector selector = DBOperator.Select(
                student.StudentId,
                (student.StudentId + 1).As("StudentIdPlus"),
                student.StudentId.As("Alpha"),
                student.StudentId.As("Beta"),
                new Description("student.StudentId").As("Gamma"),
                student.StudentName,
                student.Birthday,
                student.Gender,
                student.MajorId,
                student.Guid,
                student.Bytes,
                student.Float,
                student.Double.Sum().As("Double"),
                student.Decimal.Max().As("Decimal"),
                student.Bool,
                student.Long.Read(l => (long)l + 1),
                student.Json          //可在构造函数中通过 Read 和 Write 设置序列化和反序列化方法 或 显式调用这两个方法
                )
                .From(student)
                .LeftJoin(major).On(major.MajorId == student.MajorId)
                .Where(true & (student.StudentId == 2 | student.StudentName == "小红")
                            & student.Birthday < DateTime.Now
                            & student.StudentId.In(2, 5, 6, 7, "8", ")a")
                            & student.StudentName.Like("%小红%"))
                .GroupBy(student.StudentId, major.MajorId)
                .Having(student.StudentId > 0)
                .OrderBy(student.StudentId);

            List<StudentInfo> students = myDb.Query<StudentInfo>(selector).ToList();

            int Id = students[0].StudentId;

            //

            Major testMajor = new();

            Major studentMajor = new();

            var nested = DBOperator.Select().From(student).Where(student.StudentId == 1).As("nestedStudent");

            ISelector joinSelector = DBOperator.Select(
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
               testMajor.MajorName,
               student.StudentName.Distinct().OrderBy(student.StudentId).ThenByDescending(student.StudentName).Separator("*").Group_Concat())
               .From(nested,
               studentMajor,
               student.As("cStudent"))
               .LeftJoin(testMajor).On(student.MajorId == testMajor.MajorId)
               .LeftJoin(studentMajor.As("StudentMajor")).On(student.MajorId == studentMajor.MajorId)
               .Where((student.StudentId >= 10 & student.StudentId <= 20 | student.StudentId == 1) | (student.StudentId >= 50 & student.StudentId <= 60 & student.StudentId == null) | student.StudentName.Like("张三")
               | student.StudentId.In(new List<int>() { 1, 2, 3, 4, 5 }))
               .GroupBy(student.StudentId, testMajor.MajorId)
               .Having(studentMajor.MajorType == 1 & new Description("myId").Max() == 2 & student.StudentId.Min() > 1)
               .OrderBy(student.StudentId)
               .ThenBy(studentMajor.MajorId)
               .ThenByDescending(studentMajor.MajorType)
               .Limit(5, 10);

            Command command = joinSelector.GetCommand();

            string entitySql = command.CommandText;

            List<KeyValuePair<string, object>> paramC = command.Parameters.ToList();

            ISelector testselector = DBOperator.Select(
                    student.StudentId.As("StudentId"),
                    student.StudentName.As("StudentName"),
                    student.Birthday,
                    student.MajorId,
                    student.Guid,
                    student.Bytes,
                    student.Json,
                    testMajor.MajorName,
                    testMajor.MajorType
                    )
                .From(student)
                .LeftJoin(testMajor).On(true & (testMajor.MajorId == student.MajorId & testMajor.MajorId > 0) & true)
                .Where(student.StudentId.In(1, ")3", 2, 4) &
                student.StudentId.In(DBOperator.Select(student.StudentId)
                                     .From(student)
                                     .Where(student.StudentId.In(1, 2, 4))));

            string tests = testselector.GetCommand().CommandText;

            
            //查询
            List<StudentInfo> testStudents = Database.TestDb.Query<StudentInfo>(testselector).ToList();

            DataTable dt = Database.TestDb.Query(testselector);

            Student joinStudent = new Student();

            Major joinMajor = new Major();

            ISelector joinSelect = DBOperator.Select().From(joinStudent).FullJoin(joinMajor);

            joinSelect.SetCommandTimeout(20);

            string joinText = joinSelect.GetCommand().CommandText;

            //List<StudentInfo> joinResult = Database.TestDb.Query<StudentInfo>(joinSelect).ToList();


            ISelector njoinSelect = DBOperator.Select().From(joinStudent).Limit(1).ForUpdate();

            string njoinText = njoinSelect.GetCommand().CommandText;

            List<StudentInfo> njoinResult = Database.TestDb.Query<StudentInfo>(njoinSelect).ToList();

            //多条件查询
            Student mulStudent = new Student();

            ConditionCollection conditions = new ConditionCollection();

            conditions.Add(mulStudent.StudentId == 10);
            conditions.Add(mulStudent.StudentId == 11);

            conditions.LogicalOperator = LogicalOperator.Or;

            ISelector mulConditon = DBOperator.Select().From(mulStudent).Conditions(conditions);

            string nulCondText = mulConditon.GetCommand().CommandText;

            List<StudentInfo> mulCondResult = Database.TestDb.Query<StudentInfo>(mulConditon).ToList();


            List<Guid?> Ids = Database.TestDb.Query<Guid?>(DBOperator.Select(student.Guid).From(student).Where(student.StudentId < 10)).ToList();


            Student allStudent = new Student();

            List<StudentInfo> allStudents = Database.TestReadOnlyDb.Query<StudentInfo>(DBOperator.Select().From(allStudent)).ToList();

            Dictionary<string, object> keyValues = Database.TestReadOnlyDb.ExecuteScalar<Dictionary<string, object>>(
                DBOperator.Select(allStudent.Json).From(allStudent).Limit(1));

            //int sc = Database.TestDb.ExecuteScalar<int>(DBOperator.Select(DbFunction.Last_Insert_Id()));

            //联合查询

            Student unionA = new Student();

            Student unionB = new Student();

            ISelector unionSelect = DBOperator.Select().From(unionA).Where(unionA.StudentId == 1)
                .UnionAll(DBOperator.Select().From(unionB).Where(unionB.StudentId == 2));

            string unionText = unionSelect.GetCommand().CommandText;

            List<StudentInfo> unionResult = Database.TestDb.Query<StudentInfo>(unionSelect).ToList();

            //插入

            Student data = new Student();
            //data.StudentId.Value = 5;
            data.StudentName.Value = Path.GetRandomFileName();
            data.MajorId.Value = 2;
            data.Birthday.Value = DateTime.Now;
            data.Guid.Value = Guid.NewGuid();

            //可在构造函数中通过 Read 和 Write 设置序列化和反序列化方法 或 显式调用这两个方法
            data.Json.Value = new Dictionary<string, object>() { { "Number", new Random().NextDouble() }, { "Object", new object() } };
            

            IInsertor insertor = DBOperator.Insert(data);

            string insertText = insertor.GetCommand().CommandText;

            int insertAffected = Database.TestDb.ExecuteNonQuery(DBOperator.Insert(data));

            //插入 忽略冲突

            Student ignoreStudent = new Student();

            ignoreStudent.StudentId.Value = 5;
            ignoreStudent.StudentName.Value = Path.GetRandomFileName();
            ignoreStudent.MajorId.Value = 2;
            ignoreStudent.Birthday.Value = DateTime.Now;
            ignoreStudent.Guid.Value = Guid.NewGuid();

            IInsertor ignoreInsertor = DBOperator.InsertIgnore(ignoreStudent)
                .Columns(ignoreStudent.StudentId, ignoreStudent.StudentName, ignoreStudent.MajorId, ignoreStudent.Birthday, ignoreStudent.Guid)
                .Row(ignoreStudent);

            string ignoreText = ignoreInsertor.GetCommand().CommandText;

            int ignoreAffected = Database.TestDb.ExecuteNonQuery(ignoreInsertor);

            //插入 冲突时替换

            Student replaceStudent = new Student();

            replaceStudent.StudentId.Value = 5;
            replaceStudent.StudentName.Value = Path.GetRandomFileName();
            replaceStudent.MajorId.Value = 2;
            replaceStudent.Birthday.Value = DateTime.Now;
            replaceStudent.Guid.Value = Guid.NewGuid();

            IInsertor replaceInsertor = DBOperator.ReplaceInto(replaceStudent)
                .Columns(replaceStudent.StudentId, replaceStudent.StudentName, replaceStudent.MajorId, replaceStudent.Birthday, replaceStudent.Guid)
                .Row(replaceStudent);

            string replaceText = replaceInsertor.GetCommand().CommandText;

            int replaceAffected = Database.TestDb.ExecuteNonQuery(replaceInsertor);

            //插入并返回自增主键
            ulong newId = Database.TestDb.InsertNext(data);

            //保存

            Student saveData = new Student();
            saveData.StudentId.Value = 71126;
            saveData.StudentName.Value = "小刘";
            saveData.MajorId.Value = 2;
            saveData.Birthday.Value = DateTime.Now;
            saveData.Guid.Value = Guid.NewGuid();

            IInsertor saver = DBOperator.Save(saveData);

            string saverText = saver.GetCommand().CommandText;

            int saveAffected = Database.TestDb.ExecuteNonQuery(saver);

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
                .Rows(new List<Student>() { data1, data2, data3, data3, data3, data3, data3, data3, data3, data3, data3 })
                .OnDuplicateKeyUpdate((s) =>
                {
                    s.StudentName.Value = s.StudentName.Values();
                    s.Birthday.Value = s.Birthday.Values();
                    s.MajorId.Value = s.MajorId;
                });

            string batchText = batch.GetCommand().CommandText;

            string batchBText = batchB.GetCommand().CommandText;

            int batchAffected = Database.TestDb.ExecuteNonQuery(batch);

            int batchAffectedB = Database.TestDb.ExecuteNonQuery(batchB);

            //月表
            Monthly monthly_2021_12 = new Monthly(new DateTime(2021, 12, 1));

            IInsertor monthlyInsertor = DBOperator.InsertInto(monthly_2021_12)
                .Columns(monthly_2021_12.Guid, monthly_2021_12.DateTime, monthly_2021_12.Description)
                .Row(new
                {
                    Guid = new Guid("{5954B812-D34B-4F10-8A07-B17A146FAB5C}"),
                    DateTime = DateTime.Now,
                    Description = Path.GetRandomFileName()
                }).OnDuplicateKeyUpdate(s =>
                {
                    s.DateTime.Value = s.DateTime.Values();
                    s.Description.Value = s.Description.Values();
                });

            int monthlyInserted = Database.TestDb.ExecuteNonQuery(monthlyInsertor);

            monthlyInsertor = DBOperator.InsertInto(monthly_2021_12)
                .Columns(monthly_2021_12.Guid, monthly_2021_12.DateTime, monthly_2021_12.Description)
                .Row(new
                {
                    Guid = new Guid("{2AA827DA-149B-46B7-89B0-A8515BEAB1C7}"),
                    DateTime = DateTime.Now,
                    Description = Path.GetRandomFileName()
                }).OnDuplicateKeyUpdate((s, row) =>
                {
                    s.DateTime.Value = row.DateTime;
                    s.Description.Value = row.Description;
                });

            monthlyInserted = Database.TestDb.ExecuteNonQuery(monthlyInsertor);

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

            int selectInsertAffected = Database.TestDb.ExecuteNonQuery(selectInsert);

            //更新
            Student changedStudent = new Student();

            changedStudent.StudentName.Value = changedStudent.StudentName;
            changedStudent.Birthday.Value = DateTime.Now;
            changedStudent.MajorId.Value = changedStudent.MajorId - 1 + 1;
            changedStudent.Guid.Value = Guid.NewGuid();
            changedStudent.Bytes.Value = Encoding.UTF8.GetBytes("Bytes");
            changedStudent.Json.Value = new Dictionary<string, object>() { { "Number", new Random().NextDouble() }, { "Object", new object() } };

            IUpdater changer = DBOperator.ApplyChanges(changedStudent).On(changedStudent.StudentId == 2 & changedStudent.MajorId > 0);

            string changerText = changer.GetCommand().CommandText;

            int changeAffected = Database.TestDb.ExecuteNonQuery(changer);

            Major updateMajor = new Major();

            IUpdater updater = DBOperator.Update(changedStudent)
                .LeftJoin(updateMajor).On(changedStudent.MajorId == updateMajor.MajorId & updateMajor.MajorId == 1)
                .Set(s =>
                {
                    s.StudentName.Value = updateMajor.MajorName;
                    s.Birthday.Value = DateTime.Now;
                    s.Json.Value = new Dictionary<string, object>() { { "Number", new Random().NextDouble() }, { "Object", new object() } };
                })
                .Where(changedStudent.StudentId == 4);

            string updaterText = updater.GetCommand().CommandText;

            int updateAffected = Database.TestDb.ExecuteNonQuery(updater);

            //删除
            Student deleteTable = new Student();

            IDeleter deleter = DBOperator.Delete(deleteTable).Where(deleteTable.StudentId.In(71116, newId));

            string testdel = deleter.GetCommand().CommandText;

            //int delAffected = Database.TestDb.ExecuteNonQuery(deleter);

            //事务
            Student traStu = new Student();

            IDeleter traDel = DBOperator.Delete(traStu).Where(traStu.StudentId.In(71124, "aaa"));

            Student traStuB = new Student();
            traStuB.StudentId.Value = 71120;
            traStuB.StudentName.Value = "事务";

            IInsertor traInsert = DBOperator.Insert(traStuB);

            bool success = Database.TestDb.ExecuteTransaction(new List<IDbOperator>() { traDel, traInsert }, out int affected, out Exception exception);

            bool successB = Database.TestDb.ExecuteTransaction(traDel, traInsert);

            //事务2
            Database.TestDb.ExecuteTransaction(transaction =>
            {
                try
                {
                    List<Student> s = transaction.Query<Student>("select * from student where studentId < {0}", 10).ToList();

                    List<DateTime?> ds = transaction.Query<DateTime?>("select birthday from student where studentId < {0}", 10).ToList();

                    Student first = new Student();

                    first.Birthday.Value = (s.First().Birthday.Value as DateTime?).Value.AddMonths(1);

                    transaction.ExecuteNonQuery(DBOperator.ApplyChanges(first).On(first.StudentId == 2));

                    transaction.ExecuteNonQuery("select");

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            });

            Console.WriteLine("测试通过");

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
        static string connectionStr = File.ReadAllText("ConnectionString.txt");

        public readonly static MySqlDb TestDb = new(connectionStr);

        /// <summary>
        /// 只读库
        /// </summary>
        public readonly static MySqlDb TestReadOnlyDb = new(connectionStr);
    }
}
