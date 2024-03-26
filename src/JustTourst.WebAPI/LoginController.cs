using Microsoft.AspNetCore.Mvc;

namespace JustTourst.WebAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class loginController : ControllerBase
    {
        /// <summary>
        /// 根据id获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            //var user = await _userAppService.GetAsync(id);

            //return Ok(user);
        }

        /// <summary>
        /// 根据用户名搜索，分页返回用户列表
        /// </summary>
        /// <param name="userPageRequestDto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] UserPageRequestDto userPageRequestDto)
        {
            //根据用户名搜索，分页返回用户列表
            var UserPageResponseDto = await _userAppService.GetPagedListAsync(userPageRequestDto);

            return Ok(UserPageResponseDto);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userCreateDto"></param>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserCreateDto userCreateDto)
        {
            var users = await _userAppService.InsertAsync(userCreateDto);

            return Created(string.Empty, users);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userUpdateDto"></param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UserUpdateDto userUpdateDto)
        {
            await _userAppService.UpdateAsync(id, userUpdateDto);

            return NoContent();
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userAppService.DeleteAsync(id);

            return NoContent();
        }
    }
}
