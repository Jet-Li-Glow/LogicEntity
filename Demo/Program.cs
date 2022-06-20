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
using LogicEntity.Grammar;
using LogicEntity.Model;
using LogicEntity.Operator;
using MySql.Data.MySqlClient;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Version 0.7.0

            //开发计划  1.Debug
            //          2.Partition

            Console.WriteLine("-- Start --");

            //Db
            MySQL db = Database.TestDb;

            string commandText;

            int rowsAffected;

            //查询
            ISelector selector;

            List<Student> studentEntitys;

            List<StudentInfo> students;

            //查询 1
            Student table = new();

            studentEntitys = db.Query<Student>(DBOperator.Select().From(table)).ToList();

            students = db.Query<StudentInfo>(DBOperator.Select().From(table)).ToList();

            //查询 2
            Student student = new();

            Student studentBeta = new();

            Student inStudent = new();

            Student subQuerySutdent = new();

            Major major = new();

            Major subQueryMajor = new();

            Major nestedMajor = new();

            var nested = DBOperator.Select(nestedMajor.MajorId, nestedMajor.MajorName.As("nestedMajorName")).From(nestedMajor).Where(nestedMajor.MajorId == 1).As("nestedMajor");

            Window w = new Window("w").PartitionBy(student.StudentId).OrderBy(student.StudentId).Rows().Between().UnboundedPreceding().And().UnboundedFollowing();

            Window w2 = new Window("w2").PartitionBy(student.StudentId).OrderBy(student.StudentId).Rows().Between().Preceding(1).And().Following(1);

            selector = DBOperator.Select(
                student.StudentId,
                (student.StudentId + 1).As("StudentIdPlus"),
                student.StudentId.As("Alpha"),
                studentBeta.StudentId.As("Beta"),
                new ValueExpression("student.StudentId").As("Gamma"),
                student.StudentId > 50,
                new ValueExpression("student.StudentId > {0}", 100),
                DBOperator.Select(subQuerySutdent.MajorId).From(subQuerySutdent).Limit(1) + 1,
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
                DbFunction.Group_Concat(student.StudentName.Distinct().OrderBy(student.StudentId).ThenByDescending(student.StudentName).Separator("*")).As("concatName"),
                student.AnotherName,  //自定义列名
                student.Birthday,
                student.Gender,
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
                student.Long.Read(v => (long)v),
                student.Json,          //可在构造函数中通过 Read 和 Write 设置序列化和反序列化方法 或 显式调用这两个方法
                student.Json.Json_Extract("$"),
                nested.Column(nameof(Major.MajorId)).As("nestedMajorIdA"),
                nested.All(),
                student.MajorId,
                DbFunction.Row_Number().Over(w).As("rowNumber"),
                DbFunction.Row_Number().Over(t => t.OrderBy(student.StudentId).ThenByDescending(student.MajorId)).As("rowNumber2")
                )
                .From(student
                .InnerJoin(studentBeta.As("studentBeta")).On(student.StudentId == studentBeta.StudentId)
                .LeftJoin(major).On(major.MajorId == DBOperator.Select(subQueryMajor.MajorId)
                                                               .From(subQueryMajor)
                                                               .Where(subQueryMajor.MajorId == student.MajorId)
                                                               .OrderBy(subQueryMajor.MajorId)
                                                               .Limit(1))
                .LeftJoin(nested).On(nested.Column(nameof(Major.MajorId)) == student.MajorId)
                )
                .Where(true & (student.StudentId == 1 | student.StudentName.Like("%小红%") | student.StudentId == null)
                            & student.Birthday < DateTime.Now
                            & student.StudentId.In(1, 2, 3, 4, 5, 6, 7, "8", ")a; -- ")
                            & student.StudentId.In(DBOperator.Select(inStudent.StudentId)
                                                             .From(inStudent)
                                                             .Where(inStudent.StudentId.In(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12)))
                            & student.StudentId >= 0
                            & student.StudentId != null
                       )
                .GroupBy(student.StudentId, major.MajorId)
                .Having(student.StudentId > 0)
                .Window(w, w2)
                .OrderBy(student.StudentId)
                .ThenBy(major.MajorId)
                .Limit(1000);

            selector.SetTimeout(30);

            Command command = selector.GetCommand();

            students = db.Query<StudentInfo>(selector).ToList();

            var dataTable = db.Query(selector);

            //查询 3
            student = new();

            ConditionCollection conditions = new ConditionCollection();

            conditions.Add(student.StudentId == 10);
            conditions.Add(student.StudentId == 11);

            conditions.LogicalOperator = LogicalOperator.Or;

            selector = DBOperator.Select().From(student).Where(conditions);

            commandText = selector.GetCommand().CommandText;

            students = db.Query<StudentInfo>(selector).ToList();

            //查询 4
            student = new();

            Student unionStudent = new();
            Student unionStudent2 = new();

            selector = DBOperator.Select().From(student).Where(student.StudentId == 1)
                .UnionAll(DBOperator.Select().From(unionStudent).Where(unionStudent.StudentId == 2))
                .UnionAll(DBOperator.Select().From(unionStudent2).Where(unionStudent2.StudentId == 3));

            commandText = selector.GetCommand().CommandText;

            students = db.Query<StudentInfo>(selector).ToList();

            //查询 5
            student = new();

            List<Guid?> guids = db.Query<Guid?>(DBOperator.Select(student.Guid).From(student).Where(student.StudentId < 10)).ToList();

            //查询 6
            student = new();

            nested = DBOperator.Select(student.Json.As("studentJson")).From(student).Limit(1).As("nestedStudent");

            Dictionary<string, object> keyValues = db.ExecuteScalar<Dictionary<string, object>>(DBOperator.Select(nested.Column("studentJson")).From(nested));

            student = new();

            Gender? gender = db.ExecuteScalar<Gender?>(DBOperator.Select(student.Gender).From(student).Where(student.StudentId == 4));

            //查询 7
            CommonTableExpression cte = new CommonTableExpression("cte");

            cte.DefineColumns("n");

            cte.Selector = DBOperator.Select(new ValueExpression("1"))
                           .UnionAll(DBOperator.Select(new ValueExpression("n") + 1)
                                     .From(cte)
                                     .Where(cte.Column("n") < 10)
                                     ).Limit(20);

            CommonTableExpression cte2 = new CommonTableExpression("cte2");

            cte2.DefineColumns("n");

            cte2.Selector = DBOperator.Select(new ValueExpression("20"))
                           .UnionAll(DBOperator.Select(new ValueExpression("n") + 1)
                                     .From(cte2)
                                     .Where(cte2.Column("n") < 30));

            selector = DBOperator.WithRecursive(cte, cte2)
                                 .Select()
                                 .From(cte.As("a"))
                                 .UnionAll(DBOperator.Select().From(cte2.As("b")));

            commandText = selector.GetCommand().CommandText;

            List<int> ns = db.Query<int>(selector).ToList();


            //插入
            Student data = new();

            data.StudentName.Value = Path.GetRandomFileName();
            data.MajorId.Value = 2;
            data.Birthday.Value = DateTime.Now;
            data.Guid.Value = Guid.NewGuid();

            //可在构造函数中通过 Read 和 Write 设置序列化和反序列化方法 或 显式调用这两个方法
            data.Json.Value = new Dictionary<string, object>() { { "Number", new Random().NextDouble() }, { "Object", new object() } };

            IInsertor insertor;

            //插入 1
            ulong Id = db.InsertNext(data);

            //插入 2
            data.Json.Value = null;

            insertor = DBOperator.Insert(data);

            commandText = insertor.GetCommand().CommandText;

            rowsAffected = db.ExecuteNonQuery(insertor);

            //插入 3 Ignore
            student = new();

            data.StudentId.Value = Id;

            insertor = DBOperator.InsertIgnore(student)
                .Columns(student.StudentId, student.StudentName, student.MajorId, student.Birthday, student.Guid)
                .Row(data);

            commandText = insertor.GetCommand().CommandText;

            rowsAffected = db.ExecuteNonQuery(insertor);

            //插入 4 Replace
            student = new();

            insertor = DBOperator.ReplaceInto(student)
                .Columns(student.StudentId, student.StudentName, student.MajorId, student.Birthday, student.Guid)
                .Row(data);

            commandText = insertor.GetCommand().CommandText;

            rowsAffected = db.ExecuteNonQuery(insertor);

            //插入 5 Save (OnDuplicateKeyUpdate)
            data = new();

            data.StudentId.Value = Id;
            data.StudentName.Value = Path.GetRandomFileName();
            data.MajorId.Value = 2;
            data.Birthday.Value = DateTime.Now;
            data.Guid.Value = Guid.NewGuid();
            data.Json.Value = new Dictionary<string, object>() { { "Number", new Random().NextDouble() }, { "Object", new object() } };

            insertor = DBOperator.Save(data);

            commandText = insertor.GetCommand().CommandText;

            rowsAffected = db.ExecuteNonQuery(insertor);

            //插入 6 OnDuplicateKeyUpdate
            student = new();

            insertor = DBOperator.InsertInto(student)
                .Columns(
                    student.StudentId,
                    student.StudentName,
                    student.MajorId,
                    student.Birthday,
                    student.Guid
                )
                .Row(data)
                .OnDuplicateKeyUpdate((s, row) =>
                {
                    s.StudentName.Value = DbFunction.Concat(row.StudentName, "- updated");
                    s.Birthday.Value = row.Birthday;
                    s.MajorId.Value = s.MajorId;
                });

            commandText = insertor.GetCommand().CommandText;

            rowsAffected = db.ExecuteNonQuery(insertor);


            student = new();

            insertor = DBOperator.InsertInto(student)
                .Columns(
                    student.StudentId,
                    student.StudentName,
                    student.MajorId,
                    student.Birthday,
                    student.Guid
                )
                .Rows(new Student[] { data, data })
                .OnDuplicateKeyUpdate(s =>
                {
                    s.StudentName.Value = s.StudentName.Values();
                    s.Birthday.Value = s.Birthday.Values();
                    s.MajorId.Value = s.MajorId;
                });

            commandText = insertor.GetCommand().CommandText;

            rowsAffected = db.ExecuteNonQuery(insertor);

            //插入 7 Sharding
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

            rowsAffected = db.ExecuteNonQuery(insertor);

            //插入 8
            student = new();

            major = new();

            insertor = DBOperator.InsertInto(student).Columns(student.StudentName, student.Birthday, student.MajorId)
                .Rows(
                    DBOperator.Select(
                        major.MajorName.As("StudentName"),
                        DbFunction.Now().As("Birthday"),
                        major.MajorId)
                    .From(major)
                    .Where(major.MajorId == 3)
                );

            commandText = insertor.GetCommand().CommandText;

            rowsAffected = db.ExecuteNonQuery(insertor);

            //插入 9
            student = new();

            insertor = DBOperator.InsertInto(student)
                .Set(s =>
                {
                    s.StudentName.Value = "Insert Set";
                    s.MajorId.Value = 3;
                    s.Birthday.Value = DateTime.Now;
                });

            commandText = insertor.GetCommand().CommandText;

            rowsAffected = db.ExecuteNonQuery(insertor);

            //更新
            IUpdater updater;

            //更新 1
            student = new();

            student.StudentName.Value = "ApplyChanges Name";
            student.Json.Value = null;

            updater = DBOperator.ApplyChanges(student)
                .On(student.StudentId == Id & student.MajorId > 0)
                .OrderBy(student.StudentId)
                .Limit(1);

            commandText = updater.GetCommand().CommandText;

            rowsAffected = db.ExecuteNonQuery(updater);

            //更新 2
            student = new();

            student.StudentName.Value = major.MajorName;
            student.Birthday.Value = DateTime.Now;
            student.Json.Value = new Dictionary<string, object>() { { "Number", new Random().NextDouble() }, { "Object", new object() } };

            major = new();

            major.MajorName.Value = $"Mul-Table Updated {DateTime.Now}";

            updater = DBOperator.Update(student
                .LeftJoin(major).On(student.MajorId == major.MajorId)
                )
                .ApplyChanges(student, major)
                .On(student.StudentId == Id);

            commandText = updater.GetCommand().CommandText;

            rowsAffected = db.ExecuteNonQuery(updater);

            //更新 3
            major = new();

            cte = new("cte");

            cte.Selector = DBOperator.Select().From(major).Where(major.MajorId == 1);

            Major cteUpdateMajor = new();

            cteUpdateMajor.MajorName.Value = DbFunction.Concat(cteUpdateMajor.MajorName, "+");

            updater = DBOperator.With(cte)
                                .Update(cteUpdateMajor
                                .InnerJoin(cte).On(cte.Column("MajorId") == cteUpdateMajor.MajorId)
                                )
                                .ApplyChanges(cteUpdateMajor);

            commandText = updater.GetCommand().CommandText;

            rowsAffected = db.ExecuteNonQuery(updater);

            //删除
            IDeleter deleter;

            //删除 1
            student = new();

            deleter = DBOperator.DeleteFrom(student).Where(student.StudentId >= Id).OrderBy(student.StudentId).Limit(5);

            commandText = deleter.GetCommand().CommandText;

            rowsAffected = db.ExecuteNonQuery(deleter);

            //删除 2
            student = new();

            major = new();

            deleter = DBOperator.Delete(student)
                                .From(student
                                .InnerJoin(major).On(major.MajorId == student.MajorId)
                                )
                                .Where(major.MajorId == 5);

            commandText = deleter.GetCommand().CommandText;

            rowsAffected = db.ExecuteNonQuery(deleter);

            //删除 3
            cte = new("cte");

            cte.Selector = DBOperator.Select(new ValueExpression("-1").As("n"));

            student = new();

            deleter = DBOperator.With(cte).DeleteFrom(student).Where(student.StudentId.In(DBOperator.Select().From(cte)));

            commandText = deleter.GetCommand().CommandText;

            rowsAffected = db.ExecuteNonQuery(deleter);

            //事务 1
            student = new();

            IDeleter transactionDeleter = DBOperator.DeleteFrom(student).Where(student.StudentId.In(71116, -1));

            Student transactionStudent = new();
            transactionStudent.StudentId.Value = 71116;
            transactionStudent.StudentName.Value = "事务";

            IInsertor transactionInsertor = DBOperator.Insert(transactionStudent);

            bool success = db.ExecuteTransaction(transactionDeleter, transactionInsertor);

            //事务2
            db.ExecuteTransaction(transaction =>
            {
                try
                {
                    List<Student> s = transaction.Query<Student>("select * from student where studentId < {0}", 10).ToList();

                    List<DateTime?> ds = transaction.Query<DateTime?>("select birthday from student where studentId < {0}", 10).ToList();

                    Student first = new();

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

            //Test
            Test();

            Console.WriteLine("测试通过");

            Console.WriteLine("-- End --");

            Console.ReadKey();
        }

        static void Test()
        {
            ISelector selector = DBOperator.Select(new ValueExpression("Sleep(10)"));

            selector.SetTimeout(5);

            try
            {
                int v = Database.TestDb.Query<int>(selector).FirstOrDefault();
            }
            catch
            {
            }

            Guid guid = Guid.NewGuid();

            //插入
            Monthly data = new(new DateTime(2021, 12, 1));

            data.Guid.Value = guid;
            data.DateTime.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            data.Description.Value = Path.GetRandomFileName() + " Monthly Test";

            Database.TestDb.ExecuteNonQuery(DBOperator.Save(data));

            Monthly table = new(new DateTime(2021, 12, 1));

            Monthly d = Database.TestDb.Query<Monthly>(DBOperator.Select().From(table).Where(table.Guid == data.Guid.Value)).Single();

            Assert(d.Guid.Value, data.Guid.Value);
            Assert(d.DateTime.Value, data.DateTime.Value);
            Assert(d.Description.Value, data.Description.Value);

            //更新
            data = new(new DateTime(2021, 12, 1));

            data.DateTime.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            data.Description.Value = Path.GetRandomFileName() + " Monthly Test";

            Database.TestDb.ExecuteNonQuery(DBOperator.ApplyChanges(data).On(data.Guid == guid));

            d = Database.TestDb.Query<Monthly>(DBOperator.Select().From(table).Where(table.Guid == guid)).Single();

            Assert(d.Guid.Value, guid);
            Assert(d.DateTime.Value, data.DateTime.Value);
            Assert(d.Description.Value, data.Description.Value);

            //删除
            table = new(new DateTime(2021, 12, 1));

            Database.TestDb.ExecuteNonQuery(DBOperator.DeleteFrom(table).Where(table.Guid == guid));

            Assert(Database.TestDb.Query<Monthly>(DBOperator.Select().From(table).Where(table.Guid == data.Guid.Value)).Count() == 0);

            //Json
            Database.TestDb.Query(
                DBOperator.Select(new ValueExpression("{0}", JsonSerializer.Serialize(new object[] { new { Id = 1 }, "name", 100 })).Json_Contains_Path(OneOrAll.One, "$[1]")));

            Database.TestDb.Query(
                DBOperator.Select(new ValueExpression("{0}", JsonSerializer.Serialize(new object[] { new { Id = 1 }, "name", 100 })).Json_Search(OneOrAll.All, "name", null, "$")));

            Database.TestDb.Query(
                DBOperator.Select(new ValueExpression("{0}", JsonSerializer.Serialize(new object[] { new { Id = 1 }, "name", 100 })).Json_Search(OneOrAll.All, "name", 'a', "$")));
        }

        static void Assert(object left, object right)
        {
            Assert(JsonSerializer.Serialize(left).Equals(JsonSerializer.Serialize(right), StringComparison.OrdinalIgnoreCase));
        }

        static void Assert(bool val)
        {
            if (val == false)
                throw new Exception(nameof(Assert));
        }
    }

    /// <summary>
    /// 自定义函数
    /// </summary>
    static class MyDbFunction
    {
        public static IValueExpression MyFunction(this IValueExpression valueExpression)
        {
            return new ValueExpression("MyFunction({0})", valueExpression);
        }
    }

    class MySQL : AbstractDataBase
    {
        private readonly string _connectionStr;

        public MySQL(string connectionStr)
        {
            _connectionStr = connectionStr;
        }

        protected override IDbConnection __GetDbConnection()
        {
            return new MySqlConnection(_connectionStr);
        }
    }

    static class Database
    {
        static string connectionStr = File.ReadAllText("ConnectionString.txt");

        public readonly static MySQL TestDb = new(connectionStr);

        /// <summary>
        /// 只读库
        /// </summary>
        public readonly static MySQL TestReadOnlyDb = new(connectionStr);
    }
}
