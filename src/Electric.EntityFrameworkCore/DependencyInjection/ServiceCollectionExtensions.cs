using Electric.Core.UOW;
using Electric.Domain.Repository;
using Electric.EntityFrameworkCore.Const;
using Electric.EntityFrameworkCore.Redis;
using Electric.EntityFrameworkCore.Repository;
using Electric.EntityFrameworkCore.UOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Data;

namespace Electric.EntityFrameworkCore.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// EntityFrameworkCore注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dbType"></param>
        /// <param name="connection"></param>
        /// <param name="assemblyName"></param>
        public static void AddEntityFrameworkCore(this IServiceCollection services, DbType dbType, string connection, string? assemblyName = null)
        {
            //Repository注入
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                //获取继承IRepository并且不是泛型的类
                List<Type> types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsGenericType && t.GetInterfaces().Contains(typeof(IRepository)))
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

            //数据库上下文注入
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                //启用的数据库类型
                switch (dbType)
                {
                    case DbType.SqlServer:
                        if (assemblyName == null)
                        {
                            options.UseSqlServer(connection);
                        }
                        else
                        {
                            options.UseSqlServer(connection, b => b.MigrationsAssembly(assemblyName));
                        }
                        break;
                    case DbType.MySql:
                        if (assemblyName == null)
                        {
                            options.UseMySql(connection, ServerVersion.AutoDetect(connection));

                        }
                        else
                        {
                            options.UseMySql(connection, ServerVersion.AutoDetect(connection), b => b.MigrationsAssembly(assemblyName));
                        }
                        break;
                }
            });

            //UOW
            services.AddScoped<IUnitOfWork, EFCoreUnitOfWork>();
        }

        /// <summary>
        /// Redis配置
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddRedisCacheSetup(this IServiceCollection services,string connection)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddTransient<IRedisManger, RedisManger>();
            // 配置启动Redis服务，虽然可能影响项目启动速度，但是不能在运行的时候报错，所以是合理的
            services.AddSingleton(sp =>
            {
                ////获取连接字符串
                //string redisConfiguration = connection;
                ////Redis连接配置对象
                //var configuration = ConfigurationOptions.Parse(redisConfiguration, true);
                //configuration.ResolveDns = true;
                //return ConnectionMultiplexer.Connect(configuration);
                string redisConfiguration = connection; // 确保这个变量包含您的连接字符串
                var configuration = ConfigurationOptions.Parse(redisConfiguration, true);
                configuration.ResolveDns = true;
                ConnectionMultiplexer connectionMultiplexer = null;
                try
                {
                    connectionMultiplexer = ConnectionMultiplexer.Connect(configuration);
                    // 连接成功，可以在这里进行一些日志记录或其他操作
                }
                catch (Exception ex)
                {
                    // 连接失败，处理异常
                    // 您可以记录错误信息，或者根据需要进行其他错误处理
                    Console.WriteLine("Failed to connect to Redis: " + ex.Message);
                    // 如果连接失败，您可能希望抛出异常或者返回null，这取决于您的应用程序需求
                    // throw;
                }
                return connectionMultiplexer;
            });
        }

    }
}
