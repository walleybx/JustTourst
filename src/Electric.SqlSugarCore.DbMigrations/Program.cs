using Electric.SqlSugarCore.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

//ע��
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
        //MySql��Ҫ����汾��ServerVersion.AutoDetect���������ַ����Զ���ȡ
        break;
}

//������
builder.Services.InitTables(provider == "MySql" ? SqlSugar.DbType.MySql : SqlSugar.DbType.SqlServer, connection);

//��ʼ������
builder.Services.AddSeedData(provider == "MySql" ? SqlSugar.DbType.MySql : SqlSugar.DbType.SqlServer, connection);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.MapGet("/weatherforecast", () =>
{
    return "ִ�гɹ���";
});

app.Run();