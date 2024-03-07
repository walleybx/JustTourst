using SqlSugar;

namespace Electric.SqlSugarCore.Po
{
    [SugarTable("EleUserToken")]
    public class EleUserTokenPo
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid UserId { get; set; }

        /// <summary>
        /// 登录提供程序
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string LoginProvider { get; set; }

        /// <summary>
        /// token名称
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string Name { get; set; }

        /// <summary>
        /// token值
        /// </summary>
        public string Value { get; set; }
    }
}