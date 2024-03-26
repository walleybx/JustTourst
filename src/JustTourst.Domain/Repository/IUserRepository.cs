using JustTourst.Domain.Model.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTourst.Domain.Repository
{
    public interface IUserRepository :IBasicRepository<TrsUser>
    {
        /// <summary>
        /// 根据用户账号查询用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<TrsUser> GetByUserName(string userName);
        /// <summary>
        /// 根据用户手机号码查询用户信息
        /// </summary>
        /// <param name="mobilePhone">手机号码</param>
        /// <returns></returns>
        Task<TrsUser> GetUserByMobilePhone(string mobilePhone);
        /// <summary>
        /// 根据Email、Account、手机号查询用户信息
        /// </summary>
        /// <param name="account">登录账号</param>
        /// <returns></returns>
        Task<TrsUser> GetUserByLogin(string account);
        /// <summary>
        /// 根据Email查询用户信息
        /// </summary>
        /// <param name="email">email</param>
        /// <returns></returns>
        Task<TrsUser> GetUserByEmail(string email);
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userLogOnEntity"></param>
        bool Insert(TrsUser entity, UserLogOn userLogOnEntity);
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userLogOnEntity"></param>
        Task<bool> InsertAsync(TrsUser entity, UserLogOn userLogOnEntity);

    }
}
