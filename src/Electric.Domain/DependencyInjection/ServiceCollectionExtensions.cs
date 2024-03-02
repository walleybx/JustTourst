using Electric.Domain.Entitys.Identity;
using Electric.Domain.Manager.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Electric.Domain.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 领域注入
        /// </summary>
        /// <param name="services"></param>
        public static void AddDomain(this IServiceCollection services)
        {
            //注入UserManager
            services.TryAddScoped(typeof(UserManager));
            services.TryAddScoped(typeof(UserManager<EleUser>), provider => provider.GetService(typeof(UserManager)));

            //注入UserStore
            services.TryAddScoped(typeof(UserStore));
            services.TryAddScoped(typeof(IUserStore<EleUser>), provider => provider.GetService(typeof(UserStore)));

            //注入RoleManager
            services.TryAddScoped<RoleManager>();
            services.TryAddScoped(typeof(RoleManager<EleRole>), provider => provider.GetService(typeof(RoleManager)));

            //注入RoleStore
            services.TryAddScoped<RoleStore>();
            services.TryAddScoped(typeof(IRoleStore<EleRole>), provider => provider.GetService(typeof(RoleStore)));

            //注入PermissionManager
            services.TryAddScoped(typeof(PermissionManager));

            //配置Identity的用户和角色
            services.AddIdentityCore<EleUser>().AddRoles<EleRole>().AddUserStore<UserStore>()
                .AddRoleStore<RoleStore>();
        }
    }
}
