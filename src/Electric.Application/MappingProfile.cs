using AutoMapper;
using Electric.Application.Contracts.Dto.Identity.Accounts;
using Electric.Application.Contracts.Dto.Identity.Permissions;
using Electric.Application.Contracts.Dto.Identity.Roles;
using Electric.Application.Contracts.Dto.Identity.Users;
using Electric.Domain.Entitys.Identity;

namespace Electric.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //用户
            CreateMap<EleUser, UserDto>();

            //角色
            CreateMap<EleRole, RoleDto>();

            //权限
            CreateMap<ElePermission, PermissionDto>();
            CreateMap<ElePermission, AccountPermissionsDto>();
            CreateMap<EleRolePermission, RoleSavePermissionDto>();
            CreateMap<ElePermission, RoleGetPermissionDto>();
        }
    }
}