# LogicEntity
Build SQL and Execute  
Linq To SQL  
ORM  
SQL Queries  
Subqueries  

### Usage:
Same as IEnumerable.

Database:
```C#
var db = new MyDb("--connectionString--");
```

Select:

```C#
var data = db.Students.ToList();
```

Insert:

```C#
var rowsAffected = db.Students.Add(new Student()
{
    Id = 1,
    Name = "Name String",
    MajorId = 3,
    Json = new Student.JsonObject()
    {
        Object = new()
        {
            Property = "Insert Property Value"
        }
    }
});
```

Update:

```C#
var rowsAffected = db.Students.Where(s => s.Id == 1)
    .Set
    (
        s => s.Float.Assign(5.5f),
        s => ((Student.JsonObject)s.Json).Array[0].Assign(-5)
    );
```

Delete:

```C#
var rowsAffected = db.Students.OrderByDescending(s => s.Id).Take(1).Remove();
```

Transaction:
```C#
db.ExecuteTransaction(transaction =>
{
    try
    {
        var rowsAffected = db.Students.OrderByDescending(s => s.Id).Take(1).Remove();

        transaction.Commit();
    }
    catch
    {
        transaction.Rollback();
    }
}, IsolationLevel.ReadCommitted);
```

SQL:

```C#
var data = db.Query<Student>("Select studentId + {0} As Id From Student Limit 10", 1).ToList();
```

CTE:
```C#
var data = db.Value(() => new { n = 1 }).RecursiveConcat(ns => ns.Where(s => s.n < 20).Select(s => new { n = s.n + 1 })).Take(20).ToList();
```


