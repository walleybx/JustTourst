using Electric.API;
using Electric.API.Extensions;
using Electric.Application.Auth;
using Electric.Application.DependencyInjection;
using Electric.Domain.DependencyInjection;
using Electric.EntityFrameworkCore.DependencyInjection;
using Electric.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

//时间格式
builder.Services.AddControllers().AddNewtonsoftJson(option =>
{
    option.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//开启日志
builder.Logging.AddLog4Net();

// Swagger/OpenAPI 服务
builder.AddElectricSwaggerGen();

//跨域配置
builder.AddElectricCorsOrigins();

//Application注入
builder.Services.AddApplication();

//JWT
var jwtBearer = builder.Configuration.GetSection(AppSettings.Authentication).GetSection(AppSettings.JwtBearer);
var jwtBearerSetting = new JwtBearerSetting(
    jwtBearer.GetValue<string>(AppSettings.Issuer),
    jwtBearer.GetValue<string>(AppSettings.Audience),
    jwtBearer.GetValue<string>(AppSettings.SecurityKey)
    );
builder.Services.AddJWT(jwtBearerSetting);

//领域层注入
builder.Services.AddDomain();

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
        //MySql需要传入版本，ServerVersion.AutoDetect根据连接字符串自动获取
        break;
}

//使用EF ORM
builder.Services.AddEntityFrameworkCore(provider == "MySql" ? DbType.MySql : DbType.SqlServer, connection);

var app = builder.Build();

//启动跨域
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//应用服务提供商
app.UseServiceProvider();

app.UseHttpsRedirection();

//启动身份验证
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
