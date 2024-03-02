using Electric.Application.Auth;
using Electric.Application.Contracts.AppService.Identity;
using Electric.Application.Contracts.Dto.Identity.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Electric.WebAPI.Controllers
{
    /// <summary>
    /// 权限管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [EletricAuthorize]
    public class PermissionsController : ControllerBase
    {
        private IPermissionAppService _permissionAppService;

        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="permissionAppService"></param>
        public PermissionsController(IPermissionAppService permissionAppService)
        {
            _permissionAppService = permissionAppService;
        }

        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> All()
        {
            //获取所有权限
            var permissionDtos = await _permissionAppService.GetAllAsync();

            return Ok(permissionDtos);
        }

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="permissionCreateDto"></param>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PermissionCreateDto permissionCreateDto)
        {
            var permission = await _permissionAppService.InsertAsync(permissionCreateDto);

            return Created(string.Empty, permission);
        }

        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="id"></param>
        /// <param name="permissionUpdateDto"></param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] PermissionUpdateDto permissionUpdateDto)
        {
            await _permissionAppService.UpdateAsync(id, permissionUpdateDto);

            return NoContent();
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            //删除权限
            await _permissionAppService.DeleteAsync(id);

            return NoContent();
        }
    }
}
