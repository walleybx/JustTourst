using AutoMapper;
using Electric.Application.AppService.Base;
using Electric.Application.Auth;
using Electric.Application.Exception;
using Electric.Application.Session;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Electric.Application.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            //Dto映射
            IConfigurationProvider config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            services.AddSingleton(config);

            //Session注入
            services.AddTransient<IEleSession, EleSession>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            //Mapper注入
            services.AddScoped<IMapper, Mapper>();

            //AppService注入
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                //获取继承IBaseAppService的类
                List<Type> types = assembly.GetTypes()
                .Where(t => t.IsClass && t.GetInterfaces().Contains(typeof(IBaseAppService)))
                .ToList();

                types.ForEach(impl =>
                {
                    //获取该类所继承的所有接口
                    Type[] interfaces = impl.GetInterfaces();
                    interfaces.ToList().ForEach(i =>
                    {
                        services.AddScoped(i, impl);
                    });
                });
            }

            //异常过滤器
            services.AddControllers(configure =>
            {
                configure.Filters.Add<ElectricExceptionFilterAttribute>();
            });
        }

        /// <summary>
        /// 添加JWT中间件
        /// </summary>
        /// <param name="services"></param>
        /// <param name="jwtBearerSetting"></param>
        public static void AddJWT(this IServiceCollection services, JwtBearerSetting jwtBearerSetting)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = jwtBearerSetting.AuthenticateScheme;
                options.DefaultChallengeScheme = jwtBearerSetting.AuthenticateScheme;
                options.DefaultScheme = jwtBearerSetting.AuthenticateScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = jwtBearerSetting.ValidateIssuer,//是否验证Issuer
                    ValidIssuer = jwtBearerSetting.Issuer,//Issuer

                    ValidateAudience = jwtBearerSetting.ValidateAudience,//是否验证Audience
                    ValidAudience = jwtBearerSetting.Audience,//Audience

                    ValidateIssuerSigningKey = jwtBearerSetting.ValidateIssuerSigningKey,//是否验证SecurityKey
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtBearerSetting.SecurityKey)),//拿到SecurityKey

                    ValidateLifetime = jwtBearerSetting.ValidateLifetime,//是否验证失效时间
                    ClockSkew = TimeSpan.FromSeconds(jwtBearerSetting.ClockSkew)//偏差秒数：防止客户端与服务器时间偏差
                };
            });

            //注入JWT配置
            services.AddSingleton(jwtBearerSetting);
        }

        /// <summary>
        /// 应用服务提供商
        /// </summary>
        /// <param name="app"></param>
        public static void UseServiceProvider(this IApplicationBuilder app)
        {
            ServiceLocator.Instance = app.ApplicationServices;
        }
    }
}
