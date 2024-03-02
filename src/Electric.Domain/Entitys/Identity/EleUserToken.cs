using Electric.Core;
using Electric.Domain.Entitys.Commons;
using System.Security.Claims;

namespace Electric.Domain.Entitys.Identity
{
    public class EleUserToken : Entity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; protected set; }

        /// <summary>
        /// 登录提供程序
        /// </summary>
        public string LoginProvider { get; protected set; }

        /// <summary>
        /// token名称
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// token值
        /// </summary>
        public string Value { get; protected set; }

        protected EleUserToken()
        {

        }

        public EleUserToken(Guid userId, string loginProvider, string name, string value)
        {
            Check.NotNull(loginProvider, nameof(loginProvider));
            Check.NotNull(name, nameof(name));
            Check.NotNull(value, nameof(value));

            UserId = userId;
            LoginProvider = loginProvider;
            Name = name;
            Value = value;
        }

        public void SetValue(string value)
        {
            Check.NotNull(value, nameof(value));

            Value = value;
        }
    }
}