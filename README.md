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
```C#
Student student = new();

ISelector selector = DBOperator.Select().From(student);

List<Student> students = myDb.Query<Student>(selector).ToList();
```
