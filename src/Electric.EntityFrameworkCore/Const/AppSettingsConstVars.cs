using Electric.EntityFrameworkCore.AppSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Electric.EntityFrameworkCore.Const
{
    /// <summary>
    /// 配置文件格式化
    /// </summary>
    public class AppSettingsConstVars
    {

        AppSettingsHelper appSettingsHelper = new AppSettingsHelper(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

        #region 数据库================================================================================
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        public static readonly string DbSqlConnection = AppSettingsHelper.GetContent("ConnectionStrings", "SqlConnection");
        /// <summary>
        /// 获取数据库类型
        /// </summary>
        public static readonly string DbDbType = AppSettingsHelper.GetContent("ConnectionStrings", "DbType");
        #endregion

        #region redis================================================================================

        /// <summary>
        /// 获取redis连接字符串
        /// </summary>
        public static readonly string RedisConfigConnectionString = AppSettingsHelper.GetContent("RedisConfigs", "ConnectionString");

        ///// <summary>
        ///// 启用redis作为缓存选择
        ///// </summary>
        //public static readonly bool RedisUseCache = AppSettingsHelper.GetContent("RedisConfigs", "UseCache").ObjToBool();
        ///// <summary>
        ///// 启用redis作为定时任务
        ///// </summary>
        //public static readonly bool RedisUseTimedTask = AppSettingsHelper.GetContent("RedisConfigs", "UseTimedTask").ObjToBool();


        public static readonly string JwtConfigSecretKey = AppSettingsHelper.GetContent("JwtConfig", "SecretKey") + AppSettingsHelper.GetMachineRandomKey(DbSqlConnection + AppSettingsHelper.GetMACIp(true));
        public static readonly string JwtConfigIssuer = !string.IsNullOrEmpty(AppSettingsHelper.GetContent("JwtConfig", "Issuer")) ? AppSettingsHelper.GetContent("JwtConfig", "Issuer") : AppSettingsHelper.GetHostName();
        public static readonly string JwtConfigAudience = AppSettingsHelper.GetContent("JwtConfig", "Audience");

        #endregion
    }
}
