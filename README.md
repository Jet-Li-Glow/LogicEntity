# LogicEntity
Build SQL and Execute with .NET 5

用法参照 Demo

目前还只在 MySql 上测试


如果需要将项目的目标框架转为.NET 5，可参照以下步骤：  
1.创建新项目，目标框架选择.NET 5。  
2.将旧项目中的文件复制到新项目中，全部保存，卸载并重新加载项目。  
3.添加依赖项。  
4.生成并运行。  

# 用法：
```C#
MyDb myDb = new("xxxxxxxx");
```
### 查询 1
```c#
Student student = new();

ISelector selector = DBOperator.Select().From(student);

List<Student> students = myDb.Query<Student>(selector).ToList();

object Id = students[0].StudentId.Value;
```
### 查询 2
```c#
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
            student.Guid
            )
            .From(student)
            .LeftJoin(major).On(major.MajorId == student.MajorId)
            .Where(true & (student.StudentId == 5 | student.StudentName == "小明")
                        & student.Birthday < DateTime.Now
                        & student.StudentId.In(5, 6, 7, "8", ")a")
                        & student.StudentName.Like("%小明%"))
            .GroupBy(student.StudentId, major.MajorId)
            .Having(student.StudentId > 0)
            .OrderBy(student.StudentId);
            
List<StudentInfo> students = myDb.Query<StudentInfo>(selector).ToList();

int Id = students[0].StudentId;
```
