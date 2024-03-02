using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Electric.Application.Auth
{
    /// <summary>
    /// JWT配置
    /// </summary>
    public class JwtBearerSetting
    {
        /// <summary>
        /// 身份验证方案
        /// </summary>
        public string AuthenticateScheme { get; }

        /// <summary>
        /// 验证发行者
        /// </summary>
        public bool ValidateIssuer { get; }

        /// <summary>
        /// 发行者
        /// </summary>
        public string Issuer { get; }

        /// <summary>
        /// 验证接受者
        /// </summary>
        public bool ValidateAudience { get; }

        /// <summary>
        /// 接收者
        /// </summary>
        public string Audience { get; }

        /// <summary>
        /// 验证安全密钥
        /// </summary>
        public bool ValidateIssuerSigningKey { get; }

        /// <summary>
        /// 安全密钥
        /// </summary>
        public string SecurityKey { get; }

        /// <summary>
        /// 是否验证失效时间
        /// </summary>
        public bool ValidateLifetime { get; }

        /// <summary>
        /// 偏差秒数：防止客户端与服务器时间偏差
        /// </summary>
        public int ClockSkew { get; }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="issuer"></param>
        /// <param name="audience"></param>
        /// <param name="securityKey"></param>
        /// <param name="authenticateScheme">默认值：Bearer</param>
        /// <param name="validateIssuer">默认值：true</param>
        /// <param name="validateAudience">默认值：true</param>
        /// <param name="validateIssuerSigningKey">默认值：true</param>
        /// <param name="validateLifetime">默认值：true</param>
        /// <param name="clockSkew">默认值：5秒</param>
        public JwtBearerSetting(string issuer, string audience, string securityKey, string authenticateScheme = JwtBearerDefaults.AuthenticationScheme,
            bool validateIssuer = true, bool validateAudience = true, bool validateIssuerSigningKey = true, bool validateLifetime = true, int clockSkew = 5)
        {
            Issuer = issuer;
            Audience = audience;
            SecurityKey = securityKey;
            AuthenticateScheme = authenticateScheme;
            ValidateIssuer = validateIssuer;
            ValidateAudience = validateAudience;
            ValidateIssuerSigningKey = validateIssuerSigningKey;
            ValidateLifetime = validateLifetime;
            ClockSkew = clockSkew;
        }
    }
}
