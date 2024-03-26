using JustTourst.Application.Contracts.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTourst.Application.Contracts.AppService.Identity
{
    public interface IUserAppService
    {
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<UserDto> GetAsync(Guid id);

        ///// <summary>
        ///// 根据用户名搜索，分页返回用户列表
        ///// </summary>
        ///// <param name="userPageRequestDto"></param>
        ///// <returns></returns>
        //public Task<UserPageResponseDto> GetPagedListAsync(UserPageRequestDto userPageRequestDto);

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userCreateDto"></param>
        public Task<UserDto> InsertAsync(UserCreateDto userCreateDto);

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userUpdateDto"></param>
        public Task<UserDto> UpdateAsync(Guid id, UserUpdateDto userUpdateDto);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        public Task DeleteAsync(Guid id);
    }
}
