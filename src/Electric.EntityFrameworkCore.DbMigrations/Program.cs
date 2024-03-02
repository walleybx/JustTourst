using Electric.EntityFrameworkCore.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//EntityFrameworkCore注入
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
        break;
}

//数据库类型
var dbType = provider == "MySql" ? DbType.MySql : DbType.SqlServer;
builder.Services.AddEntityFrameworkCore(dbType, connection, Assembly.GetExecutingAssembly().FullName);

var app = builder.Build();
app.Run();