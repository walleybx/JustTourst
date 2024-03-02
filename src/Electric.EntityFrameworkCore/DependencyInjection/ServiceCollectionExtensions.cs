using Electric.Core.UOW;
using Electric.Domain.Repository;
using Electric.EntityFrameworkCore.Repository;
using Electric.EntityFrameworkCore.UOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
    }
}
