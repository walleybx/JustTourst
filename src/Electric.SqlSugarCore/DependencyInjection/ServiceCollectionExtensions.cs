using Electric.Core.UOW;
using Electric.Domain.Entitys.Identity;
using Electric.Domain.Repository;
using Electric.Domain.Shared.Entitys.Identity;
using Electric.SqlSugarCore.UOW;
using Electric.SqlSugarCore.Mapping;
using Electric.SqlSugarCore.Po;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace Electric.SqlSugarCore.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSqlSugarCore(this IServiceCollection services, DbType dbType, string connection)
        {
            //Repository注入
            //1、遍历程序集中的每个类（types），筛选出继承自接口IRepository并且不是泛型类的类型。
            //2、对于符合条件的类，获取其实现的所有接口（interfaces），然后将其注册到依赖注入容器中。
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

            //注册SqlSugar用AddScoped
            services.AddScoped<ISqlSugarClient>(s =>
            {
                //Scoped用SqlSugarClient 
                SqlSugarClient sqlSugar = new SqlSugarClient(new ConnectionConfig()
                {
                    DbType = dbType,
                    ConnectionString = connection,
                    IsAutoCloseConnection = true,
                },
               db =>
               {
                   //单例参数配置，所有上下文生效
                   db.Aop.OnLogExecuting = (sql, pars) =>
                   {
                       //打印日志
                       Console.WriteLine(sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                   };
               });
                return sqlSugar;
            });

            //Mapper
            var config = new SugarMapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            services.AddSingleton(config);
            services.AddScoped<SqlSugarMapper>();

            //UOW
            services.AddScoped(typeof(IUnitOfWork), typeof(SqlSugarCoreUnitOfWork));
        }

        /// <summary>
        /// 初始化表
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="connection"></param>
        public static void InitTables(this IServiceCollection services, DbType dbType, string connection)
        {
            var db = new SqlSugarClient(new ConnectionConfig()
            {
                DbType = dbType,
                ConnectionString = connection,
                IsAutoCloseConnection = true,
                MoreSettings = new ConnMoreSettings()
                {
                    SqlServerCodeFirstNvarchar = true
                }
            });
            //建库：如果不存在创建数据库存在不会重复创建 
            db.DbMaintenance.CreateDatabase();

            //建表
            db.CodeFirst.SetStringDefaultLength(200)
            .InitTables(
                typeof(EleUserPo),
                typeof(EleUserClaimPo),
                typeof(EleUserLoginPo),
                typeof(EleUserRolePo),
                typeof(EleUserTokenPo),

                typeof(EleRolePo),
                typeof(EleRoleClaimPo),
                typeof(EleRolePermissionPo),

                typeof(ElePermissionPo)
            );
        }

        /// <summary>
        /// 添加种子数据
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void AddSeedData(this IServiceCollection services, DbType dbType, string connection)
        {
            var db = new SqlSugarClient(new ConnectionConfig()
            {
                DbType = dbType,
                ConnectionString = connection,
                IsAutoCloseConnection = true
            });

            //1. 角色Id
            var adminRoleId = Guid.NewGuid();
            var role = new EleRole(adminRoleId, "管理员");
            // 2. 添加角色
            db.Insertable<EleRolePo>(role).ExecuteCommand();

            // 3. 添加用户
            var adminUserId = Guid.NewGuid();
            EleUser adminUser = new EleUser(adminUserId, "admin", "admin@eletric.com", "管理员");
            var ph = new PasswordHasher<EleUser>();
            var passwordHash = ph.HashPassword(adminUser, "Abc123@");
            adminUser.SetPasswordHash(passwordHash);
            db.Insertable<EleUserPo>(adminUser).ExecuteCommand();

            // 4. 给用户加入管理员权限
            db.Insertable<EleUserRolePo>(new EleUserRole(adminUserId, adminRoleId)).ExecuteCommand();

            //5. 初始化权限
            var systemId = Guid.NewGuid();
            var systemUserId = Guid.NewGuid();
            var systemRoleId = Guid.NewGuid();
            var systemPermissionId = Guid.NewGuid();
            var systemRolePermissionId = Guid.NewGuid();
            Guid? emptyParentNode = null;
            var permissionList = new List<ElePermission>
            {
                #region 菜单权限
                new ElePermission(systemId, emptyParentNode, "系统管理",  "system", PermissionType.Menu, PermissionApiMethod.GET, PermissionStatus.Normal, icon: "el-icon-s-tools"),
                new ElePermission(systemUserId, systemId, "用户管理",  "system.user", PermissionType.Menu, PermissionApiMethod.GET, PermissionStatus.Normal, "el-icon-user-solid"),
                new ElePermission(systemRoleId, systemId,  "角色管理",  "system.role", PermissionType.Menu, PermissionApiMethod.GET, PermissionStatus.Normal, "peoples"),
                new ElePermission(systemPermissionId, systemId,  "菜单管理",  "system.permission", PermissionType.Menu, PermissionApiMethod.GET, PermissionStatus.Normal,  "list"),
                new ElePermission(systemRolePermissionId, systemId,  "角色权限",  "system.rolepermission", PermissionType.Menu, PermissionApiMethod.GET, PermissionStatus.Normal, "example"),
                #endregion

                
                #region 按钮、元素权限
                new ElePermission(Guid.NewGuid(), systemUserId, "添加",  "system.user.add", PermissionType.Element, PermissionApiMethod.GET, PermissionStatus.Normal),
                new ElePermission(Guid.NewGuid(), systemUserId, "编辑",  "system.user.edit", PermissionType.Element, PermissionApiMethod.GET, PermissionStatus.Normal),
                new ElePermission(Guid.NewGuid(), systemUserId, "删除",  "system.user.delete", PermissionType.Element, PermissionApiMethod.GET, PermissionStatus.Normal),

                new ElePermission(Guid.NewGuid(), systemRoleId, "添加",  "system.role.add", PermissionType.Element, PermissionApiMethod.GET, PermissionStatus.Normal),
                new ElePermission(Guid.NewGuid(), systemRoleId, "编辑",  "system.role.edit", PermissionType.Element, PermissionApiMethod.GET, PermissionStatus.Normal),
                new ElePermission(Guid.NewGuid(), systemRoleId, "删除",  "system.role.delete", PermissionType.Element, PermissionApiMethod.GET, PermissionStatus.Normal),

                 new ElePermission(Guid.NewGuid(), systemPermissionId, "添加",  "system.permission.add", PermissionType.Element, PermissionApiMethod.GET, PermissionStatus.Normal),
                new ElePermission(Guid.NewGuid(), systemPermissionId, "编辑",  "system.permission.edit", PermissionType.Element, PermissionApiMethod.GET, PermissionStatus.Normal),
                new ElePermission(Guid.NewGuid(), systemPermissionId, "删除",  "system.permission.delete", PermissionType.Element, PermissionApiMethod.GET, PermissionStatus.Normal),

                new ElePermission(Guid.NewGuid(), systemRolePermissionId, "更新",  "system.rolepermission.update", PermissionType.Element, PermissionApiMethod.GET, PermissionStatus.Normal),
                #endregion
            };
            foreach (var permission in permissionList)
            {
                db.Insertable<ElePermissionPo>(permission).ExecuteCommand();
            }

            // 6. 给角色分配权限
            foreach (var permission in permissionList)
            {
                db.Insertable<EleRolePermissionPo>(new EleRolePermission(adminRoleId, permission.Id)).ExecuteCommand();
            }
        }
    }
}