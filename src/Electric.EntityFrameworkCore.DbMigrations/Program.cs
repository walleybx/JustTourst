using Electric.EntityFrameworkCore.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//EntityFrameworkCoreע��
var provider = builder.Configuration.GetValue<string>("DataProvider");
var connection = string.Empty;

//���õ����ݿ�����
switch (provider)
{
    case "MsSql":
        connection = builder.Configuration.GetConnectionString("MsSqlConnection") ?? throw new InvalidOperationException("MsSqlConnection��appsettings.jsonδ����");
        break;
    case "MySql":
        connection = builder.Configuration.GetConnectionString("MySqlConnection") ?? throw new InvalidOperationException("MySqlConnection��appsettings.jsonδ����");
        break;
}

//���ݿ�����
var dbType = provider == "MySql" ? DbType.MySql : DbType.SqlServer;
builder.Services.AddEntityFrameworkCore(dbType, connection, Assembly.GetExecutingAssembly().FullName);

var app = builder.Build();
app.Run();