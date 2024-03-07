using AutoMapper;
using Electric.Domain.Entitys.Identity;
using Electric.SqlSugarCore.Po;

namespace Electric.SqlSugarCore.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //用户
            CreateMap<EleUser, EleUserPo>();
            CreateMap<EleUserRole, EleUserRolePo>();
            CreateMap<EleUserClaim, EleUserClaimPo>();
            CreateMap<EleUserLogin, EleUserLoginPo>();

            CreateMap<EleUserPo, EleUser>();
            CreateMap<EleUserRolePo, EleUserRole>();
            CreateMap<EleUserClaimPo, EleUserClaim>();
            CreateMap<EleUserLoginPo, EleUserLogin>();

            //角色
            CreateMap<EleRole, EleRolePo>();
            CreateMap<EleRoleClaim, EleRoleClaimPo>();
            CreateMap<EleRolePermission, EleRolePermissionPo>();

            CreateMap<EleRolePo, EleRole>();
            CreateMap<EleRoleClaimPo, EleRoleClaim>();
            CreateMap<EleRolePermissionPo, EleRolePermission>();

            //权限
            CreateMap<ElePermission, ElePermissionPo>();
            CreateMap<ElePermissionPo, ElePermission>();
        }
    }
}
