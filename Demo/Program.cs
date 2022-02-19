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
            //开发计划  1.ConditionDescription 及其他所有带有Parameters属性的类修改为GetCommand规范化提取数据。
            //          3.添加Windows支持。
            //          4.添加Json相关函数支持。

            Console.WriteLine("-- Start --");

            //Db
            MySqlDb myDb = Database.TestDb;

            string commandText;

            int rowsAffected;

            //查询
            ISelector selector;

            List<Student> studentEntitys;

            List<StudentInfo> students;

            //查询 1
            Student table = new();

            studentEntitys = myDb.Query<Student>(DBOperator.Select().From(table)).ToList();

            students = myDb.Query<StudentInfo>(DBOperator.Select().From(table)).ToList();

            //查询 2
            Student student = new();

            Student studentBeta = new();

            Student unionStudent = new();

            Student inStudent = new();

            Major major = new();

            Major nestedMajor = new();

            var nested = DBOperator.Select().From(nestedMajor).Where(nestedMajor.MajorId == 1).As("nestedMajor");

            selector = DBOperator.Select(
                student.StudentId,
                (student.StudentId + 1).As("StudentIdPlus"),
                student.StudentId.As("Alpha"),
                studentBeta.StudentId.As("Beta"),
                new Description("student.StudentId").As("Gamma"),
                student.StudentName.ReadChars(read =>
                {
                    List<char> chars = new();

                    char[] buffer = new char[200];

                    long charsRead = 0;

                    do
                    {
                        charsRead = read(chars.Count, buffer, 0, buffer.Length);

                        chars.AddRange(buffer.Take((int)charsRead));
                    }
                    while (charsRead == buffer.Length);

                    return new string(chars.ToArray(), 0, chars.Count);
                }),
                student.StudentName.Distinct().OrderBy(student.StudentId).ThenByDescending(student.StudentName).Separator("*").Group_Concat().As("concatName"),
                student.Birthday,
                student.Gender,
                student.MajorId,
                student.Guid,
                student.Bytes.ReadBytes(read =>
                {
                    List<byte> bytes = new();

                    byte[] buffer = new byte[200];

                    long bytesRead = 0;

                    do
                    {
                        bytesRead = read(bytes.Count, buffer, 0, buffer.Length);

                        bytes.AddRange(buffer.Take((int)bytesRead));
                    }
                    while (bytesRead == buffer.Length);

                    return bytes.ToArray();
                }),
                student.Float,
                student.Double.Sum().As("Double"),
                student.Decimal.Max().As("Decimal"),
                student.Bool,
                student.Long.Read(l => (long)l + 1),
                student.Json,          //可在构造函数中通过 Read 和 Write 设置序列化和反序列化方法 或 显式调用这两个方法
                nested.Column("MajorId").As("nestedMajorId"),
                major.All()
                )
                .From(student)
                .InnerJoin(studentBeta.As("studentBeta")).On(student.StudentId == studentBeta.StudentId)
                .LeftJoin(major).On(major.MajorId == student.MajorId)
                .LeftJoin(nested).On(nested.Column("MajorId") == student.MajorId)
                .Where(true & (student.StudentId == 1 | student.StudentName.Like("%小红%"))
                            & student.Birthday < DateTime.Now
                            & student.StudentId.In(1, 2, 3, 4, 5, 6, 7, "8", ")a")
                            & student.StudentId.In(DBOperator.Select(inStudent.StudentId)
                                                             .From(inStudent)
                                                             .Where(inStudent.StudentId.In(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12)))
                            & student.StudentId >= 0
                       )
                .GroupBy(student.StudentId, major.MajorId)
                .Having(student.StudentId > 0)
                .OrderBy(student.StudentId)
                .ThenBy(major.MajorId)
                .Limit(1000);

            commandText = selector.GetCommand().CommandText;

            students = myDb.Query<StudentInfo>(selector).ToList();

            //查询 3
            Student mulStudent = new Student();

            ConditionCollection conditions = new ConditionCollection();

            conditions.Add(mulStudent.StudentId == 10);
            conditions.Add(mulStudent.StudentId == 11);

            conditions.LogicalOperator = LogicalOperator.Or;

            selector = DBOperator.Select().From(mulStudent).Conditions(conditions);

            commandText = selector.GetCommand().CommandText;

            students = myDb.Query<StudentInfo>(selector).ToList();

            //查询 4
            Student unionA = new Student();

            Student unionB = new Student();

            selector = DBOperator.Select().From(unionA).Where(unionA.StudentId == 1)
                .UnionAll(DBOperator.Select().From(unionB).Where(unionB.StudentId == 2))
                .UnionAll(DBOperator.Select().From(unionB).Where(unionB.StudentId == 3))
                .OrderBy(new Description("StudentId"))
                .Limit(10);

            commandText = selector.GetCommand().CommandText;

            students = myDb.Query<StudentInfo>(selector).ToList();

            //查询 5
            Student singleColumnTable = new Student();

            List<Guid?> guids = myDb.Query<Guid?>(DBOperator.Select(singleColumnTable.Guid).From(singleColumnTable).Where(singleColumnTable.StudentId < 10)).ToList();

            //查询 6
            Student singleObjectTable = new();

            Dictionary<string, object> keyValues = myDb.ExecuteScalar<Dictionary<string, object>>(DBOperator.Select(singleObjectTable.Json).From(singleObjectTable).Limit(1));

            //查询 7
            CommonTableExpression cte = new CommonTableExpression("cte");

            cte.DefineColumns("n");

            cte.Selector = DBOperator.Select(new Description("1"))
                           .UnionAll(DBOperator.Select(new Description("n") + 1)
                                     .From(cte)
                                     .Where(cte.Column("n") < 10));

            CommonTableExpression cte2 = new CommonTableExpression("cte2");

            cte2.DefineColumns("n");

            cte2.Selector = DBOperator.Select(new Description("20"))
                           .UnionAll(DBOperator.Select(new Description("n") + 1)
                                     .From(cte2)
                                     .Where(cte2.Column("n") < 30));

            selector = DBOperator.WithRecursive(cte, cte2)
                                 .Select()
                                 .From(cte)
                                 .UnionAll(DBOperator.Select().From(cte2));

            commandText = selector.GetCommand().CommandText;

            List<int> ns = myDb.Query<int>(selector).ToList();


            //插入
            Student data = new Student();
            
            data.StudentName.Value = Path.GetRandomFileName();
            data.MajorId.Value = 2;
            data.Birthday.Value = DateTime.Now;
            data.Guid.Value = Guid.NewGuid();

            //可在构造函数中通过 Read 和 Write 设置序列化和反序列化方法 或 显式调用这两个方法
            data.Json.Value = new Dictionary<string, object>() { { "Number", new Random().NextDouble() }, { "Object", new object() } };

            IInsertor insertor;

            //插入 1
            ulong Id = myDb.InsertNext(data);

            //插入 2
            insertor = DBOperator.Insert(data);

            commandText = insertor.GetCommand().CommandText;

            rowsAffected = myDb.ExecuteNonQuery(insertor);

            //插入 3 Ignore
            Student insertIgnoreTable = new();

            data.StudentId.Value = Id;

            insertor = DBOperator.InsertIgnore(insertIgnoreTable)
                .Columns(insertIgnoreTable.StudentId, insertIgnoreTable.StudentName, insertIgnoreTable.MajorId, insertIgnoreTable.Birthday, insertIgnoreTable.Guid)
                .Row(data);

            commandText = insertor.GetCommand().CommandText;

            rowsAffected = myDb.ExecuteNonQuery(insertor);

            //插入 4 Replace
            Student replaceIntoTable = new();

            insertor = DBOperator.ReplaceInto(replaceIntoTable)
                .Columns(replaceIntoTable.StudentId, replaceIntoTable.StudentName, replaceIntoTable.MajorId, replaceIntoTable.Birthday, replaceIntoTable.Guid)
                .Row(data);

            commandText = insertor.GetCommand().CommandText;

            rowsAffected = myDb.ExecuteNonQuery(insertor);

            //插入 5 Save (OnDuplicateKeyUpdate)
            Student saveData = new Student();

            saveData.StudentId.Value = Id;
            saveData.StudentName.Value = Path.GetRandomFileName();
            saveData.MajorId.Value = 2;
            saveData.Birthday.Value = DateTime.Now;
            saveData.Guid.Value = Guid.NewGuid();
            saveData.Json.Value = new Dictionary<string, object>() { { "Number", new Random().NextDouble() }, { "Object", new object() } };

            insertor = DBOperator.Save(saveData);

            commandText = insertor.GetCommand().CommandText;

            rowsAffected = myDb.ExecuteNonQuery(insertor);

            //插入 6 OnDuplicateKeyUpdate
            Student insertUpdateTable = new();

            insertor = DBOperator.InsertInto(insertUpdateTable)
                .Columns(
                    insertUpdateTable.StudentId,
                    insertUpdateTable.StudentName,
                    insertUpdateTable.MajorId,
                    insertUpdateTable.Birthday,
                    insertUpdateTable.Guid
                )
                .Row(data, data)
                .OnDuplicateKeyUpdate((s, row) =>
                {
                    s.StudentName.Value = row.StudentName;
                    s.Birthday.Value = row.Birthday;
                    s.MajorId.Value = s.MajorId;
                });

            commandText = insertor.GetCommand().CommandText;

            rowsAffected = myDb.ExecuteNonQuery(insertor);

            Student insertUpdateTableBeta = new();

            insertor = DBOperator.InsertInto(insertUpdateTableBeta)
                .Columns(
                    insertUpdateTableBeta.StudentId,
                    insertUpdateTableBeta.StudentName,
                    insertUpdateTableBeta.MajorId,
                    insertUpdateTableBeta.Birthday,
                    insertUpdateTableBeta.Guid
                )
                .Rows(new List<Student>() { data, data })
                .OnDuplicateKeyUpdate(s =>
                {
                    s.StudentName.Value = s.StudentName.Values();
                    s.Birthday.Value = s.Birthday.Values();
                    s.MajorId.Value = s.MajorId;
                });

            commandText = insertor.GetCommand().CommandText;

            rowsAffected = myDb.ExecuteNonQuery(insertor);

            //插入 7 Partition
            Monthly monthly_2021_12 = new Monthly(new DateTime(2021, 12, 1));

            insertor = DBOperator.InsertInto(monthly_2021_12)
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

            commandText = insertor.GetCommand().CommandText;

            rowsAffected = myDb.ExecuteNonQuery(insertor);

            //插入 8

            Student Target = new Student();

            Major Original = new Major();

            insertor = DBOperator.InsertInto(Target).Columns(Target.StudentName, Target.Birthday, Target.MajorId)
                .Rows(
                    DBOperator.Select(
                        Original.MajorName.As("StudentName"),
                        DbFunction.Now().As("Birthday"),
                        Original.MajorId)
                    .From(Original)
                    .Where(Original.MajorId == 3)
                );

            commandText = insertor.GetCommand().CommandText;

            rowsAffected = myDb.ExecuteNonQuery(insertor);

            //更新
            Student updateData = new();

            updateData.Json.Value = new Dictionary<string, object>() { { "Number", new Random().NextDouble() }, { "Object", new object() } };

            IUpdater updater;

            //更新 1
            updater = DBOperator.ApplyChanges(updateData).On(updateData.StudentId == Id & updateData.MajorId > 0);

            commandText = updater.GetCommand().CommandText;

            rowsAffected = myDb.ExecuteNonQuery(updater);

            //更新 2
            Major updateMajor = new();

            updater = DBOperator.Update(updateData)
                .LeftJoin(updateMajor).On(updateData.MajorId == updateMajor.MajorId)
                .Set(s =>
                {
                    s.StudentName.Value = updateMajor.MajorName;
                    s.Birthday.Value = DateTime.Now;
                    s.Json.Value = new Dictionary<string, object>() { { "Number", new Random().NextDouble() }, { "Object", new object() } };
                })
                .Where(updateData.StudentId == Id);

            commandText = updater.GetCommand().CommandText;

            rowsAffected = myDb.ExecuteNonQuery(updater);

            //更新 3
            Major cteMajor = new();

            CommonTableExpression updateCTE = new("cte");

            updateCTE.Selector = DBOperator.Select().From(cteMajor).Where(cteMajor.MajorId == 1);

            Major cteUpdateMajor = new();

            updater = DBOperator.With(updateCTE)
                                .Update(cteUpdateMajor)
                                .InnerJoin(updateCTE).On(updateCTE.Column("MajorId") == cteUpdateMajor.MajorId)
                                .Set(s => s.MajorName.Value = s.MajorName.Concat("+"));

            commandText = updater.GetCommand().CommandText;

            rowsAffected = myDb.ExecuteNonQuery(updater);

            //删除
            IDeleter deleter;

            //删除 1
            Student deleteTable = new Student();

            deleter = DBOperator.DeleterFrom(deleteTable).Where(deleteTable.StudentId.In(71116, -1)).OrderBy(deleteTable.StudentId).Limit(2);

            commandText = deleter.GetCommand().CommandText;

            rowsAffected = myDb.ExecuteNonQuery(deleter);

            //删除 2
            Student deleteStudent = new();

            Major deleteMajor = new();

            deleter = DBOperator.Delete(deleteStudent)
                                .From(deleteStudent)
                                .InnerJoin(deleteMajor).On(deleteMajor.MajorId == deleteStudent.MajorId)
                                .Where(deleteMajor.MajorId == 5);

            commandText = deleter.GetCommand().CommandText;

            rowsAffected = myDb.ExecuteNonQuery(deleter);

            //删除 3
            CommonTableExpression deleteCTE = new("cte");

            deleteCTE.Selector = DBOperator.Select(new Description("-1"));

            Student cteDeleteStudent = new();

            deleter = DBOperator.With(deleteCTE).DeleterFrom(cteDeleteStudent).Where(cteDeleteStudent.StudentId.In(DBOperator.Select().From(deleteCTE)));

            commandText = deleter.GetCommand().CommandText;

            rowsAffected = myDb.ExecuteNonQuery(deleter);

            //事务 1
            Student transactionDeleteTable = new Student();

            IDeleter transactionDeleter = DBOperator.DeleterFrom(transactionDeleteTable).Where(transactionDeleteTable.StudentId.In(71116, -1));

            Student transactionStudent = new Student();
            transactionStudent.StudentId.Value = 71116;
            transactionStudent.StudentName.Value = "事务";

            IInsertor transactionInsertor = DBOperator.Insert(transactionStudent);

            bool success = myDb.ExecuteTransaction(transactionDeleter, transactionInsertor);

            //事务2
            myDb.ExecuteTransaction(transaction =>
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
