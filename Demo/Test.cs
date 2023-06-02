using Demo.Model;
using Demo.Tables;
using LogicEntity;
using LogicEntity.Collections;
using LogicEntity.Collections.Generic;
using LogicEntity.Default.MySql;
using LogicEntity.Linq;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Demo
{
    internal static class Test
    {
        public static void Run()
        {
            TestDb db = Database.TestDb;

            Command command;

            int rowsAffected;

            LinqConvertOptions options = new();

            options.PropertyConverters.Set<Student.JsonObject, string>(typeof(Student).GetProperty(nameof(Student.Json)), s => JsonSerializer.Deserialize<Student.JsonObject>(s), s => JsonSerializer.Serialize(s));

            ILinqConvertProvider linqConvertProvider = new LogicEntity.Default.MySql.LinqConvertProvider(options);

            var parameter = new
            {
                Object = new
                {
                    Integer = 0
                },
                TimeSpan = new TimeSpan(1, 5, 5),
                String = "pName",
                Integers = new List<int>() { 1, 2, 3, 4, 5, 6 }
            };

            object data;

            //Select - 1
            Func<object, object> read = s => JsonSerializer.Deserialize<Dictionary<string, object>>(s.ToString());

            Func<Func<long, byte[], int, int, long>, byte[]> bytesReader = read =>
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
            };

            Func<Func<long, char[], int, int, long>, char[]> charsReader = read =>
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

                return chars.ToArray();
            };

            IDataTable dataTable = db.Students
                .Join(db.Students.Join(db.Students, (a, b) => a.Id == b.Id).Select((a, b) => a), (a, b) => a.Id == b.Id)
                .Where((a, b) => true
                    && a.Id > parameter.Object.Integer
                    && (a.Name == null || a.Name == Test.Value)
                    && (a.Id == 4 || (a.Id >= 5 && a.Id <= 6) || false)
                    && ((string)a.Name).Contains("123")
                    && parameter.Integers.Contains(a.Id)
                    && db.Students.Select(s => s.Id).Contains(a.Id)
                    )
                .Where((a, b) => a.Id < 7)
                .GroupBy((a, b) => new { a.Id, a.Name })
                .Select(g => new
                {
                    Id = g.Key.Id,
                    Name = (((string)g.Key.Name) ?? "Null Replace")
                        + " - "
                        + g.Key.Id.ToString()
                        + string.Concat(g.Key.Name, g.Element((a, b) => a.MajorId), g.Element((a, b) => b.MajorId))
                        + string.Join("--", parameter.String, g.Element((a, b) => a.Name)),

                    Calculation = (-g.Key.Id + 1) * 5 + Math.Round(1.25) + ((string)g.Key.Name).Length + g.Element((a, b) => ((Student.JsonObject)a.Json).Array).Length,

                    Condition = g.Key.Id > 0 ? (int)g.Key.Id : 0,

                    Any = g.Key.Id == DbFunction.Any(db.Students.Select(b => b.Id)),
                    All = g.Key.Id == DbFunction.All(db.Students.Select(b => b.Id)),

                    GroupKey = g.Key,

                    Aveage = g.Average((a, b) => a.Id) + db.Students.Average(s => s.Id),
                    Count = g.Count() + db.Students.Count(),
                    LongCount = g.LongCount() + db.Students.LongCount(),
                    Max = g.Max((a, b) => a.Id) + db.Students.Max(s => s.Id),
                    Min = g.Min((a, b) => a.Id) + db.Students.Min(s => s.Id),
                    Sum = g.Sum((a, b) => a.Id) + db.Students.Sum(s => s.Id),

                    DateTime = ((DateTime)g.Element((a, b) => a.Birthday))
                        .AddYears(1)
                        .AddMilliseconds(1)
                        .AddDays(1.1)
                        .AddHours(1.1)
                        .AddMinutes(1.1)
                        .AddSeconds(1.1)
                        .AddMilliseconds(1.1)
                        .AddTicks(100)
                        .Add(parameter.TimeSpan)
                        - g.Element((a, b) => a.Birthday),

                    Object = new
                    {
                        Nested = new
                        {
                            Id = g.Key.Id
                        }
                    },

                    Array = new int[] { g.Key.Id, g.Element((a, b) => a.Id) },

                    Json = g.Element((a, b) => a.Json),
                    JsonValue = ((Student.JsonObject)g.Element((a, b) => a.Json)).Object.Property
                        + ((Student.JsonObject)g.Element((a, b) => a.Json)).Array[0]
                        + ((Student.JsonObject)g.Element((a, b) => a.Json)).Dictionary["A.B"]
                        + ((Student.JsonObject)g.Element((a, b) => a.Json)).Dictionary["A\"B\\C"],
                    JsonArrayItemItem = g.Element((a, b) => ((Student.JsonObject)a.Json).Array[7]),
                    SubQuery = db.Students.
                               Where(a => a.Id == g.Key.Id && a.Name == g.Element((a, b) => a.Name))
                               .Select(a => db.Students.Where(b => b.Id == a.Id).Average(b => b.Id))
                               .First(),

                    Json2 = read(g.Element((a, b) => a.Json)),
                    Json3 = Test.ToDictionary(g.Element((a, b) => a.Json)),
                    Json4 = g.Element((a, b) => a.Json).Read<string, object>(v => read(v)),
                    Bytes = g.Element((a, b) => a.Bytes).ReadBytes(bytesReader),
                    Chars = g.Key.Name.ReadChars(charsReader),

                    Tuple = Tuple.Create((int)g.Key.Id, (string)g.Key.Name) == Tuple.Create(1, (string)db.Students.Select(s => s.Name).First())
                })
                .Where(s => s.Id > 0)
                .OrderBy(a => a.Id)
                .ThenBy(a => a.Name)
                .Skip(10)
                .Take(1)
                .Distinct()
                .Timeout(10)
                ;

            command = linqConvertProvider.Convert(dataTable.Expression);

            data = ((IEnumerable)dataTable).Cast<object>().ToList();

            //Select - 2
            dataTable = db.Students
                .GroupBy(s => s.Id)
                .Select(s => new
                {
                    Id = s.Key + 1,
                    Name = s.Element.Name,
                    s.Element.Birthday,
                    s.Element.Gender,
                    s.Element.MajorId,
                    s.Element.Guid,
                    s.Element.Bytes,
                    s.Element.Float,
                    s.Element.Double,
                    s.Element.Decimal,
                    s.Element.Bool,
                    s.Element.Long,
                    Avg = db.Students.Average(b => b.Id ==
                        (
                        s.Element.Id
                        + s.Average(c => c.Id)
                        + db.Students.Where(d => d.Id == b.Id).Select(d => d.Id).First())
                        )
                })
                .Take(1);

            command = linqConvertProvider.Convert(dataTable.Expression);

            data = ((IEnumerable)dataTable).Cast<object>().ToList();

            //Select - 3
            data = db.Value(() => new { n = 1 })
                .Select(s => s.n)
                .Where(s => s > 0)
                .OrderBy(s => s)
                .GroupBy(s => s)
                .Select(s => s.Key)
                .Where(s => s > -1)
                .Union(db.Value(() => 2).Where(s => s > -2))
                .OrderBy(s => s)
                .Take(10)
                .ToList();

            //Select - 3
            data = db.Value(() => new
            {
                SubQuery1 = db.Value(() => new { n = 1 })
                    .RecursiveConcat(ns => ns.Where(s => s.n < 40).Select(s => new { n = s.n + 1 }))
                    .Take(20)
                    .Select(s => s.n)
                    .OrderByDescending(s => s)
                    .First(),
                SubQuery2 = db.Value(() => new { n = 100 })
                    .RecursiveUnion(ns => ns.Where(s => s.n < 200).Select(s => new { n = s.n + 1 }))
                    .Take(20)
                    .Select(s => s.n)
                    .OrderByDescending(s => s)
                    .First()
            }).ToList();

            //Select - 4
            data = db.Students.Select(s => new { n = (int)s.Id + 100 }).Take(5).Union(
                db.Value(() => new { n = 1 })
                .RecursiveConcat(ns => ns.Where(s => s.n < 40).Select(s => new { n = s.n + 1 }))
                .Take(20)
                ).ToList();

            //Select - 5
            dataTable = db.Students
                .Where(s => s.Id > 0)
                .Where(s => s.Id > 0)
                .Where((s, i) => s.Id > 0 && i > -1)
                .Select((s, i) => new
                {
                    Index = i,
                    IndexPlus = i + s.Id,
                    Id = (int)s.Id,
                    Name = (string)s.Name
                })
                .Select((s, i) => new
                {
                    Index = i,
                    IndexPlus = i + s.Id,
                    Id = s.Id,
                    Name = s.Name
                })
                .Where((s, i) => s.Id > 0 && i > -1)
                .Where((s, i) => s.Id > 0 && i > -1)
                .Take(1)
                ;

            command = linqConvertProvider.Convert(dataTable.Expression);

            data = ((IEnumerable)dataTable).Cast<object>().ToList();

            //Select - 6
            dataTable = db.Students
                .NaturalJoin(db.Students)
                .Select((a, b) => a)
                .Take(1)
                ;

            command = linqConvertProvider.Convert(dataTable.Expression);

            data = ((IEnumerable)dataTable).Cast<object>().ToList();

            //Select - 7
            dataTable = db.Students.Select(s =>
                db.Students
                    .Join(db.Students, (a, b) => a.Id == b.Id)
                    .InnerJoin(db.Students, (a, b, c) => a.Id == c.Id)
                    .CrossJoin(db.Students, (a, b, c, d) => a.Id == d.Id)
                    .LeftJoin(db.Students, (a, b, c, d, e) => a.Id == e.Id)
                    .RightJoin(db.Students, (a, b, c, d, e, f) => a.Id == f.Id)
                    .Select((a, b, c, d, e, f) => a.Id)
                    .First()
                    )
                .Take(1);

            command = linqConvertProvider.Convert(dataTable.Expression);

            data = ((IEnumerable)dataTable).Cast<object>().ToList();

            //Select - 8
            data = db.Students.Select(
                s => s.Id.As("Const Id"),
                s => s.Name,
                s => s.Json,
                s => s.Bytes.As("Bytes").ReadBytes(bytesReader),
                s => s.Name.As("Chars").ReadChars(charsReader)
                );

            //Select - 9
            data = db.Students.Average(s => s.Id + s.Id);
            data = db.Students.Count();
            data = db.Students.LongCount();
            data = db.Students.Max(s => s.Id + s.Id);
            data = db.Students.Min(s => s.Id + s.Id);
            data = db.Students.Take(10).Sum(s => s.Id + s.Id);

            data = db.Students.Any(a => a.Name == "小明");
            data = db.Students.All(a => a.Name == "小明");

            //Select - 10
            data = db.Value(() => db.Students.Max(s => s.Id) + 1).ToList();

            //Select - 11
            data = db.Students.Select(s => new MyClass(s.Id + 1) { Name = s.Name + " - " }).Take(1).ToList();

            //Select - 12
            var nsdata = db.Value(() => new { n = 1 }).RecursiveConcat(ns =>
                db.Students.Join(ns, (a, b) => a.Id == b.n)
                .Select((a, b) => new { n = b.n + 1 })
                ).Take(20);

            data = nsdata.ToList();

            data = db.Value(() => new { n = 1 }).RecursiveConcat(ns => ns).Take(20).ToList();

            data = db.Students.Join(nsdata, (a, b) => a.Id == b.n).Select((a, b) => new { b.n }).Union(nsdata).ToList();

            //Select - 13
            try
            {
                db.Value(() => MyDbFunction.Sleep(10)).Timeout(5).First();

                throw new Exception();
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
            }

            //Select - 14
            List<int> ids = new();

            data = db.Students.Where(s => ids.Contains(s.Id)).ToList();

            //Select - 15
            data = db.Students.Select(s => new { s.Id }).UnionBy(db.Students.Select(s => new { s.Id }), s => s.Id).Take(10).ToList();

            //Select - 16
            data = db.Students.SkipWhile((s, i) => i < 5 || i > 15).ToList();

            //Select - 17
            int v = 1;

            Assert(db.Value(() => (v + v) * (v + v)).First(), 4);

            //Select - 18
            data = db.Students.FirstOrDefault(s => s.Gender == Gender.Male);

            //Select - 19
            data = db.Students.Where(s => ((Student.JsonObject)s.Json).Array.Contains(5)).FirstOrDefault();

            //Select - 20
            data = db.Students.OrderBy(s => s.Id).Select(s => new { s.Id, s.Name }).ToList();

            //Insert - 1
            rowsAffected = db.Students.Add(new Student()
            {
                Name = new(() => db.Students.Select(s => s.Name + "Add Operate").First()),
                Birthday = DateTime.Now,
                Gender = Gender.Male,
                MajorId = 3,
                Guid = Guid.NewGuid(),
                Bytes = Encoding.UTF8.GetBytes("123"),
                Float = 4f,
                Double = 5d,
                Decimal = 6m,
                Bool = true,
                Long = 7L,
                Json = new Student.JsonObject()
                {
                    Array = new[] { 8, 9, 10 },
                    Object = new()
                    {
                        Property = "Insert Property Value"
                    },
                    Dictionary = new() { { "Dictionary \"-\\ Key", "Dictionary Key Value" } }
                }
            }, new Student()
            {
                Name = "Add Operate 2",
                MajorId = 3,
                Birthday = DateTime.Now,
                Float = 0,
                Double = 1
            });

            //Insert - 2
            rowsAffected = db.Students.Timeout(5).Add(new Student() { Name = "Add Timeout", MajorId = 1 });

            //Insert - 3
            int maxId = db.Students.Max(s => s.Id);

            rowsAffected = new TestDb80(Database.ConnectionStr).Students.Timeout(5).AddOrUpdate(
                (oldValue, newValue) => new Student() { Name = oldValue.Name + " - " + newValue.Name + " - Update Factory 80" },
                new Student()
                {
                    Id = maxId,
                    Name = "New Value 80",
                    Birthday = DateTime.Now,
                    MajorId = new(() => db.Majors.Max(m => m.MajorId))
                },
                new Student()
                {
                    Id = maxId - 1,
                    Name = "New Value 80",
                    Birthday = DateTime.Now,
                    MajorId = new(() => db.Majors.Max(m => m.MajorId))
                });

            rowsAffected = db.Students.Where(s => s.Id == maxId || s.Id == maxId - 1).Remove();

            //Monthly
            data = db.Monthly.Create((s, t) => (s, t + "_2022_9")).ToList();

            rowsAffected = db.Monthly.Create((s, t) => (s, t + "_2022_9")).Add(new Monthly()
            {
                Guid = Guid.NewGuid(),
                DateTime = DateTime.Now,
                Description = "Description Value"
            });

            rowsAffected = db.Monthly.Create((s, t) => (s, t + "_2022_9")).OrderByDescending(s => s.DateTime).Take(1).Set(s => s.Description.SetValue(s.Description + " - Update"));

            rowsAffected = db.Monthly.Create((s, t) => (s, t + "_2022_9")).OrderByDescending(s => s.DateTime).Take(1).Remove();

            Console.WriteLine("测试通过");
        }

        static readonly string Value = "Test Static Value";

        public static object ToDictionary(object value)
        {
            return JsonSerializer.Deserialize<Dictionary<string, object>>(value.ToString());
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

        class MyClass
        {
            public MyClass(int Id)
            {
                this.Id = Id;
            }

            public int Id { get; set; }

            public string Name { get; set; }
        }

        class TestDb80 : AbstractDataBase
        {
            string _connectionStr;

            public TestDb80(string connectionStr)
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

                options.UpdateFactoryVersion = UpdateFactoryVersion.V8_0;

                return new LogicEntity.Default.MySql.LinqConvertProvider(options);
            }

            public ITable<Student> Students { get; init; }
        }
    }
}
