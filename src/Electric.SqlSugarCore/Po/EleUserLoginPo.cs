using SqlSugar;

namespace Electric.SqlSugarCore.Po
{
    [SugarTable("EleUserLogin")]
    public class EleUserLoginPo
    {
        /// <summary>
        /// 登录提供程序
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string LoginProvider { get; set; }

        /// <summary>
        /// ProviderKey
        /// </summary>
        public string ProviderKey { get; set; }

        /// <summary>
        /// 提供程序显示名称
        /// </summary>
        public string ProviderDisplayName { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid UserId { get; set; }
    }
}