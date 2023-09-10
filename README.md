# CommonApi
基于单体架构的Asp.net Core Webapi

# 整体架构

```ts
├─.template.config--模板的配置文件
└─src
    ├─1-PresentationLayer--表现层
    │  ├─CommonApi.Api--webapi
    │  │  ├─Controllers--controllers
    │  │  └─Properties--配置文件
    │  └─CommonApi.Common--通用webapi配置
    │      ├─Common--通用
    │      ├─Extensions--扩展
    │      └─Middlewares--中间件
    ├─2-BusinessLayer--业务层
    │  └─CommonApi.Business
    │      ├─IServices
    │      └─Services
    ├─3-DataLayer--数据层
    │  ├─CommonApi.DataBase--数据库
    │  │  └─Dapper
    │  ├─CommonApi.Mapper--实体对象映射
    │  │  └─Mapster
    │  ├─CommonApi.Repository--仓储层
    │  │  ├─IRepositories
    │  │  └─Repositories
    │  └─CommonApi.Validation--fluentvalidation
    │      └─RequestValidators
    ├─4-EntityLayer--实体层
    │  ├─CommonApi.DTO--请求响应和中间dto
    │  │  ├─Requests
    │  │  ├─Responses
    │  │  └─ServiceDTO
    │  └─CommonApi.Entity--数据库实体类
    └─5-CommonLayer--公共层
        └─CommonApi.Util--常用工具类
            ├─Extensions--扩展类
            └─Helpers--帮助类
```

# nuget包

1. Serilog
2. FluentValidation
3. Mapster
4. Riok.Mapperly
5. IGeekFan.AspNetCore.Knife4jUI
6. Microsoft.AspNetCore.Authentication.JwtBearer
7. Scrutor
8. Swashbuckle.AspNetCore
9. Dapper
10. Dapper.SqlBuilder
11. ZString
13. Enums.NET
14. DeepCloner

# 项目规范

## 重要规范

1. 除特殊情况外禁止使用`dynamic`进行传递参数

## Controller规范

1. `Controller`里面只放对请求实体和响应实体的校验和对业务类方法的调用，禁止在`Controller`里面写业务逻辑

2. 除文件等特殊返回结果外，`Controller`统一使用`ResponseResult<T>`返回结果。示例如下：
   ```C#
   [HttpGet]
   public ResponseResult<string> GetData()
   {
       //成功返回Success("");
       return Success("");
   	//失败返回Fail();
   }
   ```

   

# 命名规范

1. `Controller`类以`Controller`结尾
2. 业务类以`Service`结尾，接口在实现类的前面加I即可
3. 仓储类以`Repository`结尾
4. 请求实体类以`Request`结尾
5. 响应实体类以`Response`结尾
6. 业务层实体类以`Dto`结尾
7. 枚举类以`Enum`结尾
8. 帮助类以`Helper`结尾
9. 扩展类以`Extenson`结尾

# 一些常见方法的使用

## 日志记录

项目采用serilog记录日志，实现微软官方自带log接口。微软官方日志包安装在CommonApi.Util项目中。记录日志要使用结构化的方式，不要使用拼接字符串的方式记录。结构化的好处在于结构清晰，便于查找，最重要的是结构化日志会缓存模板，可以大幅减少字符串的生成和内存占用。具体使用参考[serilog官方文档](https://github.com/serilog/serilog/wiki/Structured-Data)，简单使用方式如下：

```C#
public class Demo
{
    private readonly ILogger<Demo> _logger;
	public Demo(ILogger<Demo> logger)
    {
        _logger = logger;
    }    
    
    public void GetDemo()
    {
        _logger.LogInformation("Retrieved {Count} records", count) // 输出为{ "Count": 456 }
        //记录类
        var fruit = new[] { "Apple", "Pear", "Orange" };
		_logger.Information("In my bowl I have {Fruit}", fruit);//输出为{ "Fruit": ["Apple", "Pear", "Orange"] }
    }
}
```



## Dapper的使用

不要使用sql拼接的方式查询，因为dapper内置有缓存会根据sql进行缓存，拼接sql会造成大量sql被缓存，容易造成内存泄漏。应该使用参数化的方式进行查询，示例如下：

### Query

```C#
var parameters = new { UserName = username, Password = password };
var sql = "SELECT * from users where username = @UserName and password = @Password";
var result = await _dapperHlper.QueryListAsync(sql, parameters);
```

### Execute

```c#
//批量插入
var foos = new List<Foo>
{
    { new Foo { A = 1, B = 1 } }
    { new Foo { A = 2, B = 2 } }
    { new Foo { A = 3, B = 3 } }
};

var count = await _dapperHlper.ExecuteAsync(@"insert MyTable(colA, colB) values (@a, @b)", foos);
Assert.Equal(foos.Count, count);
```

### 动态拼接where

该功能需要使用Dapper.SqlBuilder包，以下示例来自dapper单元测试仓库：https://github.com/DapperLib/Dapper/blob/main/tests/Dapper.Tests/SqlBuilderTests.cs

```C#
public void TestSqlBuilderWithDapperQuery()
{
    var sb = new SqlBuilder();
    var template = sb.AddTemplate("SELECT /**select**/ FROM #Users /**where**/");
    sb.Where("Age <= @Age", new { Age = 18 })
      .Where("Country = @Country", new { Country = "USA" })
      .Select("Name,Age,Country");

    const string createSql = @"
        create table #Users (Name varchar(20),Age int,Country nvarchar(5));
        insert #Users values('Sam',16,'USA'),('Tom',25,'UK'),('Henry',14,'UK')";
    try
    {
        connection.Execute(createSql);

        var result = connection.Query(template.RawSql,template.Parameters).ToArray();

        Assert.Equal("SELECT Name,Age,Country\n FROM #Users WHERE Age <= @Age AND Country = @Country\n", template.RawSql);

        Assert.Single(result);

        Assert.Equal(16, (int)result[0].Age);
        Assert.Equal("Sam", (string)result[0].Name);
        Assert.Equal("USA", (string)result[0].Country);
    }
    finally
    {
        connection.Execute("drop table #Users");
    }
}
public void TestSqlBuilderUpdateSet()
        {
            var id = 1;
            var vip = true;
            var updatetime = DateTime.Parse("2020/01/01");

            var sb = new SqlBuilder()
                   .Set("Vip = @vip", new { vip })
                   .Set("Updatetime = @updatetime", new { updatetime })
                   .Where("Id = @id", new { id })
            ;
            var template = sb.AddTemplate("update #Users /**set**/ /**where**/");

            const string createSql = @"
                create table #Users (Id int,Name varchar(20),Age int,Country nvarchar(5),Vip bit,Updatetime datetime);
                insert #Users (Id,Name,Age,Country) values(1,'Sam',16,'USA'),(2,'Tom',25,'UK'),(3,'Henry',14,'UK')";
            try
            {
                connection.Execute(createSql);

                var effectCount = connection.Execute(template.RawSql, template.Parameters);

                var result = connection.QueryFirst("select * from #Users where Id = 1");

                Assert.Equal("update #Users SET Vip = @vip , Updatetime = @updatetime\n WHERE Id = @id\n", template.RawSql);


                Assert.True((bool)result.Vip);
                Assert.Equal(updatetime, (DateTime)result.Updatetime);
            }
            finally
            {
                connection.Execute("drop table #Users");
            }
        }
```



### 参考链接

1. https://github.com/DapperLib/Dapper
2. https://www.learndapper.com/parameters
3. https://dappertutorial.net/parameter-anonymous
