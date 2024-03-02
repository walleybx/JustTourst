using Electric.Core;
using Electric.Domain.Entitys.Commons;
using Microsoft.AspNetCore.Identity;

namespace Electric.Domain.Entitys.Identity
{
    public class EleUserLogin : Entity
    {
        /// <summary>
        /// 登录提供程序
        /// </summary>
        public string LoginProvider { get; protected set; }

        /// <summary>
        /// ProviderKey
        /// </summary>
        public string ProviderKey { get; protected set; }

        /// <summary>
        /// 提供程序显示名称
        /// </summary>
        public string ProviderDisplayName { get; protected set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; protected set; }

        protected EleUserLogin()
        {
        }

        public EleUserLogin(string loginProvider, string providerKey, string providerDisplayName, Guid userId)
        {
            Check.NotNull(loginProvider, nameof(loginProvider));
            Check.NotNull(providerKey, nameof(providerKey));
            Check.NotNull(providerDisplayName, nameof(providerDisplayName));

            LoginProvider = loginProvider;
            ProviderKey = providerKey;
            ProviderDisplayName = providerDisplayName;
            UserId = userId;
        }

        public EleUserLogin(UserLoginInfo login, Guid userId)
        {
            Check.NotNull(login, nameof(login));

            LoginProvider = login.LoginProvider;
            ProviderKey = login.ProviderKey;
            ProviderDisplayName = login.ProviderDisplayName;
            UserId = userId;
        }
    }
}