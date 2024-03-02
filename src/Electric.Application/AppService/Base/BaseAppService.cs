using AutoMapper;
using Electric.Application.DependencyInjection;
using Electric.Application.Session;
using Electric.Core.Exceptions;
using Electric.Core.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Electric.Application.AppService.Base
{
    public class BaseAppService : IBaseAppService
    {
        /// <summary>
        /// Session
        /// </summary>
        protected readonly IEleSession _eleSession;

        /// <summary>
        /// 映射
        /// </summary>
        protected readonly IMapper _mapper;

        /// <summary>
        /// 工作单元
        /// </summary>
        protected readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// 注入
        /// </summary>
        public BaseAppService()
        {
            if (ServiceLocator.Instance == null)
            {
                throw new BusinessException("请在项目启动入口，调用UserServiceProvider");
            }
            _eleSession = ServiceLocator.Instance.GetService<IHttpContextAccessor>().HttpContext.RequestServices.GetService<IEleSession>();
            _mapper = ServiceLocator.Instance.GetService<IHttpContextAccessor>().HttpContext.RequestServices.GetService<IMapper>();
            _unitOfWork = ServiceLocator.Instance.GetService<IHttpContextAccessor>().HttpContext.RequestServices.GetService<IUnitOfWork>();
        }

        /// <summary>
        /// 抛出业务异常
        /// </summary>
        /// <param name="result"></param>
        /// <param name="message"></param>
        /// <exception cref="BusinessException"></exception>
        public void ThrowBusinessException(IdentityResult result, string? message = null)
        {
            //错误提示
            var errorMsg = message ?? string.Empty;
            var index = 1;

            //遍历错误列表
            foreach (var item in result.Errors)
            {
                errorMsg += $"错误{index}：错误码：{item.Code}，详细信息：{item.Description}\r\n";
                index++;
            }


            throw new BusinessException(errorMsg);
        }
    }
}