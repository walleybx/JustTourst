using Electric.Application.AppService.Base;
using Electric.Application.Contracts.AppService.Identity;
using Electric.Application.Contracts.Dto.Identity.Users;
using Electric.Core.Exceptions;
using Electric.Domain.Entitys.Identity;
using Electric.Domain.Manager.Identity;
using Electric.Domain.Repository.Identity;

namespace Electric.Application.AppService.Identity
{
    public class UserAppService : BaseAppService, IUserAppService
    {
        /// <summary>
        /// 用户管理器
        /// </summary>
        private UserManager _userManager;

        /// <summary>
        /// 用户
        /// </summary>
        private IUserRepository _userRepository;

        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="userRepository"></param>
        public UserAppService(UserManager userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="userCreateDto"></param>
        /// <returns></returns>
        public async Task<UserDto> InsertAsync(UserCreateDto userCreateDto)
        {
            var user = new EleUser(Guid.NewGuid(), userCreateDto.UserName, userCreateDto.Email, userCreateDto.FullName, userCreateDto.Remark)
            {
                CreatorId = _eleSession.UserId
            };

            //插入用户
            var result = await _userManager.CreateAsync(user, userCreateDto.Password);

            //新增失败
            if (!result.Succeeded)
            {
                ThrowBusinessException(result, "新增用户失败！");
            }

            return _mapper.Map<UserDto>(user);
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userUpdateDto"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<UserDto> UpdateAsync(Guid id, UserUpdateDto userUpdateDto)
        {
            //判断用户Id，是否存在
            var entity = await _userManager.FindByIdAsync(id.ToString());
            if (entity == null)
            {
                throw new BusinessException("该用户Id不存在，Id:" + id.ToString());
            }

            //更新用户信息
            entity.LastModifierId = _eleSession.UserId;
            entity.Status = userUpdateDto.Status;
            entity.FullName = userUpdateDto.FullName;
            if (userUpdateDto.Password != null)
            {
                entity.SetPasswordHash(userUpdateDto.Password);
            }
            entity.Remark = userUpdateDto.Remark;
            entity.SetUserName(userUpdateDto.UserName);

            //设置角色
            await _userManager.SetRolesAsync(entity, userUpdateDto.RoleNames);

            //更新用户
            var result = await _userManager.UpdateAsync(entity);

            //更新用户失败
            if (!result.Succeeded)
            {
                ThrowBusinessException(result, "更新用户失败！");
            }

            return _mapper.Map<UserDto>(entity);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Guid id)
        {
            if (id == _eleSession.UserId)
            {
                throw new BusinessException("不可删除自己！");
            }

            //判断用户Id，是否存在
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return;
            }

            //删除用户
            await _userRepository.DeleteAsync(id);

            //提交所有更新
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 根据Id，获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserDto> GetAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            return _mapper.Map<UserDto>(user);
        }

        /// <summary>
        /// 根据搜索条件，返回用户列表
        /// </summary>
        /// <param name="userPageRequestDto"></param>
        /// <returns></returns>
        public async Task<UserPageResponseDto> GetPagedListAsync(UserPageRequestDto userPageRequestDto)
        {
            //根据搜索条件，获取用户列表
            var users = await _userManager.GetListAsync(userPageRequestDto.UserName, userPageRequestDto.Page, userPageRequestDto.PrePage);
            //根据搜索条件，获取总记录数
            var total = await _userManager.GetCountAsync(userPageRequestDto.UserName);

            //获取用户的创建者
            var userDtos = _mapper.Map<List<UserDto>>(users);
            foreach (var userDto in userDtos)
            {
                var user = users.Find(x => x.Id == userDto.Id);
                userDto.Roles = await _userManager.GetRolesAsync(user);
                if (user.CreatorId != null && !default(Guid).Equals(user.CreatorId))
                {
                    userDto.Creator = (await _userManager.FindByIdAsync(user.CreatorId.ToString()))?.UserName ?? string.Empty;
                }
            }

            //返回
            return new UserPageResponseDto()
            {
                Page = userPageRequestDto.Page,
                PrePage = userPageRequestDto.PrePage,
                Users = userDtos,
                Total = total
            };
        }
    }
}
