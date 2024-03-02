using Electric.Application.Auth;
using Electric.Application.Contracts.AppService.Identity;
using Electric.Application.Contracts.Dto.Identity.Roles;
using Microsoft.AspNetCore.Mvc;

namespace Electric.WebAPI.Controllers
{
    /// <summary>
    /// 角色
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [EletricAuthorize]
    public class RolesController : ControllerBase
    {
        private IRoleAppService _roleAppService;

        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="roleAppService"></param>
        public RolesController(IRoleAppService roleAppService)
        {
            _roleAppService = roleAppService;
        }

        /// <summary>
        /// 角色搜索
        /// </summary>
        /// <param name="rolePageRequestDto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] RolePageRequestDto rolePageRequestDto)
        {
            //角色搜索
            var roles = await _roleAppService.GetPagedListAsync(rolePageRequestDto);

            return Ok(roles);
        }

        /// <summary>
        /// 获取所有角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleAppService.GetAllAsync();

            return Ok(roles);
        }

        /// <summary>
        /// 保存角色的权限列表
        /// </summary>
        /// <param name="id">角色Id</param>
        /// <param name="rolePermissionDto">以,分割权限Id</param>
        /// <returns></returns>
        [HttpPut("{id}/permissions")]
        public async Task<IActionResult> SavePermissions(Guid id, [FromBody] RoleSavePermissionDto rolePermissionDto)
        {
            await _roleAppService.SavePermissionsAsync(id, rolePermissionDto);

            return Ok();
        }

        /// <summary>
        /// 获取角色的权限列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}/permissions")]
        public async Task<IActionResult> GetPermissions(Guid id)
        {
            //获取角色的权限列表
            var rolePermissionDtos = await _roleAppService.GetPermissionsAsync(id);

            return Ok(rolePermissionDtos);
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="roleCreateDto"></param>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RoleCreateDto roleCreateDto)
        {
            var role = await _roleAppService.InsertAsync(roleCreateDto);
            return Created(string.Empty, role);
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleUpdateDto"></param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] RoleUpdateDto roleUpdateDto)
        {
            await _roleAppService.UpdateAsync(id, roleUpdateDto);
            return NoContent();
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _roleAppService.DeleteAsync(id);
            return NoContent();
        }
    }
}
