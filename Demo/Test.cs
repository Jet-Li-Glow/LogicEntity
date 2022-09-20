using Demo.Model;
using Demo.TableModel;
using LogicEntity;
using LogicEntity.Default.MySql;
using System;
using System.Collections;
using System.Collections.Generic;
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

                    Calculation = (-g.Key.Id + 1) * 5 + Math.Round(1.25) + ((string)g.Key.Name).Length + g.Element((a, b) => a.JsonArray).Length,

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

                    Json = g.Element((a, b) => a.Json),
                    JsonValue = ((Student.JsonObject)g.Element((a, b) => a.Json)).Object.Property
                        + ((Student.JsonObject)g.Element((a, b) => a.Json)).Array[0]
                        + ((Student.JsonObject)g.Element((a, b) => a.Json)).Dictionary["A.B"]
                        + ((Student.JsonObject)g.Element((a, b) => a.Json)).Dictionary["A\"B\\C"],
                    JsonArrayItemItem = g.Element((a, b) => a.JsonArray[7]),
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
            dataTable = db.Students
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

            //Select - 4
            dataTable = db.Students
                .NaturalJoin(db.Students)
                .Select((a, b) => a)
                .Take(1)
                ;

            command = linqConvertProvider.Convert(dataTable.Expression);

            data = ((IEnumerable)dataTable).Cast<object>().ToList();

            //Select - 5
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

            //Select - 6
            data = db.Students.Select(
                s => s.Id.As("Const Id"),
                s => s.Name,
                s => s.Json,
                s => s.Bytes.As("Bytes").ReadBytes(bytesReader),
                s => s.Name.As("Chars").ReadChars(charsReader)
                );

            //Select - 7
            object result = null;

            result = db.Students.Average(s => s.Id + s.Id);
            result = db.Students.Count();
            result = db.Students.LongCount();
            result = db.Students.Max(s => s.Id + s.Id);
            result = db.Students.Min(s => s.Id + s.Id);
            result = db.Students.Take(1).Sum(s => s.Id + s.Id);

            result = db.Students.Any(a => a.Name == "小明");
            result = db.Students.All(a => a.Name == "小明");

            //Insert - 1
            rowsAffected = db.Students.Add(new Student()
            {
                Name = new(() => db.Students.Select(s => s.Name + "Add Operate").First()),
                MajorId = 3,
                Gender = Gender.Male,
                Birthday = DateTime.Now,
                Decimal = 2,
                Json = new Student.JsonObject()
                {
                    Array = new[] { 5, 6, 7 },
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
            var autoIncrementId = db.Students.AddNext(new()
            {
                MajorId = 3,
                Name = "Auto Increment"
            });

            //Insert - 3
            rowsAffected = db.Students.AddOrUpdate(new Student()
            {
                Id = new(() => db.Students.Max(s => s.Id)),
                Name = "Add or Update",
                MajorId = new(() => db.Majors.Max(m => m.MajorId))
            });

            //Update - 1
            rowsAffected = db.Students.OrderByDescending(s => s.Id)
                .Take(1)
                .Set(
                s => s.Name.Assign<Value<string>>(s.Name + " Last"),
                s => s.Decimal.Assign(0m),
                s => ((Student.JsonObject)s.Json).Array[0].Assign(-5)
                );

            //Update - 2
            rowsAffected = db.Students.Join(db.Majors, (s, m) => s.MajorId == m.MajorId)
                .Where((s, m) => m.MajorId == 3)
                .Set((s, m) => s.Name.Assign("Joined Set"));

            //Delete - 1
            rowsAffected = db.Students.OrderByDescending(s => s.Id).Take(1).Remove();

            //Delete - 2
            rowsAffected = db.Students.Join(db.Majors, (s, m) => s.MajorId == m.MajorId)
                .Where((s, m) => m.MajorId == db.Majors.Max(m => m.MajorId))
                .Remove((s, m) => s);

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
    }
}
