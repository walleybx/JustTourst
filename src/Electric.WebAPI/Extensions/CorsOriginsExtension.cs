using Electric.API;

namespace Electric.WebAPI.Extensions
{
    /// <summary>
    /// 跨域相关扩展
    /// </summary>
    public static class CorsOriginsExtension
    {
        /// <summary>
        /// 跨域相关配置
        /// </summary>
        /// <param name="builder"></param>
        public static void AddElectricCorsOrigins(this WebApplicationBuilder builder)
        {
            var corsOrigins = builder.Configuration.GetSection(AppSettings.CorsOrigins).Value;
            builder.Services.AddCors(m => m.AddDefaultPolicy(
            //a => a.SetIsOriginAllowed(_ => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials())); //允许任何地址访问
            a => a.WithOrigins(corsOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries)).AllowAnyMethod().AllowAnyHeader().AllowCredentials()));
        }
    }
}