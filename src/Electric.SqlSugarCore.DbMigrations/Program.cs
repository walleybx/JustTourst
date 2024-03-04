using Electric.SqlSugarCore.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

//注入
var provider = builder.Configuration.GetValue<string>("DataProvider");
var connection = string.Empty;
//启用的数据库类型
switch (provider)
{
    case "MsSql":
        connection = builder.Configuration.GetConnectionString("MsSqlConnection") ?? throw new InvalidOperationException("MsSqlConnection在appsettings.json未发现");
        break;
    case "MySql":
        connection = builder.Configuration.GetConnectionString("MySqlConnection") ?? throw new InvalidOperationException("MySqlConnection在appsettings.json未发现");
        //MySql需要传入版本，ServerVersion.AutoDetect根据连接字符串自动获取
        break;
}

//创建表
builder.Services.InitTables(provider == "MySql" ? SqlSugar.DbType.MySql : SqlSugar.DbType.SqlServer, connection);

//初始化数据
builder.Services.AddSeedData(provider == "MySql" ? SqlSugar.DbType.MySql : SqlSugar.DbType.SqlServer, connection);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.MapGet("/weatherforecast", () =>
{
    return "执行成功！";
});

app.Run();