using Electric.EntityFrameworkCore.Redis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Electric.WebAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]   
    public class RedisDataController : ControllerBase
    {
        private IRedisManger _redisManger;

        private readonly IConfiguration _configuration;

        private RedisDataController(IRedisManger redisManger, IConfiguration configuration)
        {
            _redisManger = redisManger;
            _configuration = configuration;
        }

        /// <summary>
        /// 设置库存数量
        /// </summary>
        [HttpGet]
        public IActionResult SetStoreNumber()
        {
            //设置库存数量有1000
            bool isok = _redisManger.SetStringKey("store", 1000, new TimeSpan(100, 0, 0));
            return Ok(isok ? "设置成功" : "设置失败");
        }
        /// <summary>
        /// 获取库存数量
        /// </summary>
        [HttpGet]
        public IActionResult GetStoreNumber()
        {
            int num = Convert.ToInt32(_redisManger.GetStringValue("store"));
            return Ok(num);
        }

    }
}
