using Electric.Domain.Entitys.Identity;
using Electric.Domain.Shared.Entitys.Identity;
using Microsoft.EntityFrameworkCore;

namespace Electric.EntityFrameworkCore.Repository
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ElePermission> Permissions { get; set; }
        public DbSet<EleUser> Users { get; set; }
        public DbSet<EleUserClaim> UserClaims { get; set; }
        public DbSet<EleUserLogin> UserLogins { get; set; }
        public DbSet<EleUserRole> UserRoles { get; set; }
        public DbSet<EleUserToken> UserTokens { get; set; }
        public DbSet<EleRole> Roles { get; set; }
        public DbSet<EleRoleClaim> RoleClaims { get; set; }
        public DbSet<EleRolePermission> RolePermissions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// 模型生成器
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //配置实体映射表
            ConfigureEntityTables(modelBuilder);

            //添加种子数据
            AddSeedData(modelBuilder);
        }

        /// <summary>
        /// 配置实体映射表
        /// </summary>
        /// <param name="modelBuilder"></param>
        private void ConfigureEntityTables(ModelBuilder modelBuilder)
        {
            // 配置实体类型映射到的表名
            modelBuilder.Entity<EleUser>(b =>
            {
                b.ToTable("EleUser");
                b.HasKey(x => x.Id);

                b.Property(u => u.NormalizedUserName).IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName(nameof(EleUser.NormalizedUserName));
                b.Property(u => u.NormalizedEmail).IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName(nameof(EleUser.NormalizedEmail));
                b.Property(u => u.PasswordHash).HasMaxLength(100)
                    .HasColumnName(nameof(EleUser.PasswordHash));
                b.Property(u => u.SecurityStamp).IsRequired().HasMaxLength(50)
                    .HasColumnName(nameof(EleUser.SecurityStamp));
                b.Property(u => u.TwoFactorEnabled).HasDefaultValue(false)
                    .HasColumnName(nameof(EleUser.TwoFactorEnabled));
                b.Property(u => u.LockoutEnabled).HasDefaultValue(false)
                    .HasColumnName(nameof(EleUser.LockoutEnabled));

                b.Property(u => u.AccessFailedCount)
                    .HasColumnName(nameof(EleUser.AccessFailedCount));

                b.HasMany(u => u.Claims).WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
                b.HasMany(u => u.Logins).WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
                b.HasMany(u => u.Roles).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
                b.HasMany(u => u.Tokens).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();

                b.HasIndex(u => u.NormalizedUserName);
                b.HasIndex(u => u.NormalizedEmail);
                b.HasIndex(u => u.UserName);
                b.HasIndex(u => u.Email);
                b.HasIndex(u => u.CreationTime);
            });

            modelBuilder.Entity<EleUserClaim>(b =>
            {
                b.ToTable("EleUserClaim");

                b.Property(x => x.Id).ValueGeneratedNever();

                b.Property(uc => uc.ClaimType).HasMaxLength(100).IsRequired();
                b.Property(uc => uc.ClaimValue).HasMaxLength(200);

                b.HasIndex(uc => uc.UserId);
            });

            modelBuilder.Entity<EleUserRole>(b =>
            {
                b.ToTable("EleUserRole");

                b.HasKey(ur => new { ur.UserId, ur.RoleId });

                b.HasOne<EleRole>().WithMany().HasForeignKey(ur => ur.RoleId).IsRequired();
                b.HasOne<EleUser>().WithMany(u => u.Roles).HasForeignKey(ur => ur.UserId).IsRequired();

                b.HasIndex(ur => new { ur.RoleId, ur.UserId });
            });

            modelBuilder.Entity<EleUserLogin>(b =>
            {
                b.ToTable("EleUserLogin");

                b.HasKey(x => new { x.UserId, x.LoginProvider });

                b.Property(ul => ul.LoginProvider).HasMaxLength(200)
                    .IsRequired();
                b.Property(ul => ul.ProviderKey).HasMaxLength(200)
                    .IsRequired();
                b.Property(ul => ul.ProviderDisplayName)
                    .HasMaxLength(100);

                b.HasIndex(l => new { l.LoginProvider, l.ProviderKey });
            });

            modelBuilder.Entity<EleUserToken>(b =>
            {
                b.ToTable("EleUserToken");

                b.HasKey(l => new { l.UserId, l.LoginProvider, l.Name });

                b.Property(ul => ul.LoginProvider).HasMaxLength(200)
                    .IsRequired();
                b.Property(ul => ul.Name).HasMaxLength(200).IsRequired();

            });

            modelBuilder.Entity<EleRole>(b =>
            {
                b.ToTable("EleRole");
                b.HasKey(x => x.Id);
                b.Property(r => r.Name).IsRequired().HasMaxLength(50);
                b.Property(r => r.NormalizedName).IsRequired().HasMaxLength(50);
                b.HasMany(r => r.Claims).WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
                //指定角色与角色权限表，一对多
                b.HasMany(r => r.Permissions).WithOne().HasForeignKey(x => x.RoleId).IsRequired();

                b.HasIndex(r => r.NormalizedName);
                b.HasIndex(u => u.CreationTime);
            });

            modelBuilder.Entity<EleRoleClaim>(b =>
            {
                b.ToTable("EleRoleClaim");

                b.Property(x => x.Id).ValueGeneratedNever();

                b.Property(uc => uc.ClaimType).HasMaxLength(200).IsRequired();
                b.Property(uc => uc.ClaimValue).HasMaxLength(200);

                b.HasIndex(uc => uc.RoleId);
            });
            modelBuilder.Entity<ElePermission>(b =>
            {
                b.ToTable("ElePermission");
                b.HasKey(x => x.Id);
                b.HasIndex(u => u.CreationTime);
            });
            modelBuilder.Entity<EleRolePermission>(b =>
            {
                b.ToTable("EleRolePermission");
                b.HasKey(rp => new { rp.RoleId, rp.PermissionId });

                b.HasOne<EleRole>().WithMany(r => r.Permissions).HasForeignKey(r => r.RoleId).IsRequired();
                b.HasOne<ElePermission>().WithMany().HasForeignKey(ur => ur.PermissionId).IsRequired();

                b.HasIndex(rp => new { rp.RoleId, rp.PermissionId });
            });
        }

        /// <summary>
        /// 添加种子数据
        /// </summary>
        /// <param name="modelBuilder"></param>
        private void AddSeedData(ModelBuilder modelBuilder)
        {
            //1. 角色Id
            var adminRoleId = Guid.NewGuid();
            // 2. 添加角色
            modelBuilder.Entity<EleRole>().HasData(
                new EleRole(adminRoleId, "管理员")
            );

            // 3. 添加用户
            var adminUserId = Guid.NewGuid();
            EleUser adminUser = new EleUser(adminUserId, "admin", "admin@eletric.com", "管理员");
            adminUser.SetPasswordHash("Abc123@");
            modelBuilder.Entity<EleUser>().HasData(adminUser);

            // 4. 给用户加入管理员权限
            modelBuilder.Entity<EleUserRole>()
                .HasData(new EleUserRole(adminUserId, adminRoleId));

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
            modelBuilder.Entity<ElePermission>().HasData(permissionList);

            // 6. 给角色分配权限
            var rolePermissionList = new List<EleRolePermission>();
            foreach (var permission in permissionList)
            {
                rolePermissionList.Add(new EleRolePermission(adminRoleId, permission.Id));
            }
            modelBuilder.Entity<EleRolePermission>()
                .HasData(rolePermissionList);
        }
    }
}
