using Electric.Application.Session;
using Electric.Domain.Entitys.Identity;
using Electric.Domain.Repository.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.RegularExpressions;

namespace Electric.Application.Auth
{
    /// <summary>
    /// API权限验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    public class EletricAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //从上下文容器获取注入的对象
            var _eleSession = context.HttpContext.RequestServices.GetService(typeof(IEleSession)) as IEleSession;
            var _roleRepository = context.HttpContext.RequestServices.GetService(typeof(IRoleRepository)) as IRoleRepository;
            var _userRepository = context.HttpContext.RequestServices.GetService(typeof(IUserRepository)) as IUserRepository;
            var _permissionRepository = context.HttpContext.RequestServices.GetService(typeof(IPermissionRepository)) as IPermissionRepository;


            //判断此Api在权限列表是否有配置，如果未配置，默认都拥有权限
            var allPermission = _permissionRepository.GetListAsync().Result;
            var url = context.HttpContext.Request.Path.Value?.ToLower() ?? string.Empty;
            var existPermission = allPermission.FirstOrDefault(x => context.HttpContext.Request.Method.ToLower().Equals(x.ApiMethod.ToLower())
            && (!string.IsNullOrEmpty(x.Url) ? Regex.Match(url, x.Url?.ToLower()).Success : false));

            if (existPermission == null)
            {
                return;
            }

            //获取角色
            var roles = _userRepository.GetRoleNamesAsync(_eleSession.UserId).Result;

            var elePermissions = new Dictionary<Guid, ElePermission>();
            foreach (var roleName in roles)
            {
                var role = _roleRepository.FindByNameAsync(roleName).Result;
                var ids = role.Permissions.Select(x => x.PermissionId).ToList();
                //获取角色列表
                var _rolePermissions = _permissionRepository.GetListAsync(ids).Result;
                //合并角色重复的权限
                foreach (var permission in _rolePermissions)
                {
                    if (!elePermissions.ContainsKey(permission.Id))
                    {
                        elePermissions.Add(permission.Id, permission);
                    }
                }
            }

            //是否有权限
            var hasPermission = elePermissions.Values.FirstOrDefault(x => context.HttpContext.Request.Method.ToLower().Equals(x.ApiMethod.ToLower())
            && (!string.IsNullOrEmpty(x.Url) ? Regex.Match(url, x.Url?.ToLower()).Success : false));

            //此API无访问权限
            if (hasPermission == null)
            {
                var result = new ForbidResult();
                context.Result = result;
            }
        }
    }
}