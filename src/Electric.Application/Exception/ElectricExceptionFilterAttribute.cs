using Electric.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Electric.Application.Exception
{
    /// <summary>
    /// 异常过滤器
    /// </summary>
    public class ElectricExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<ElectricExceptionFilterAttribute> _logger;

        /// <summary>
        /// 依赖注入日志对象
        /// </summary>
        /// <param name="logger"></param>
        public ElectricExceptionFilterAttribute(ILogger<ElectricExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            //如果异常没有被处理则进行处理
            if (context.ExceptionHandled == false)
            {
                var exceptionMsg = $"异常：{context.HttpContext.Request.Path} {context.Exception.Message} {context.Exception.StackTrace}";

                var businessException = context.Exception as BusinessException;
                if (businessException == null)
                {
                    // 记录日志
                    _logger.LogError(exceptionMsg);
                    context.Result = new ContentResult
                    {
                        // 返回状态码500，表示服务器未知异常，为了安全起见不返回实际的错误信息
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Content = "服务器异常，请联系管理员"
                    };
                }
                else
                {
                    // 记录日志
                    _logger.Log(businessException.LogLevel, $"{exceptionMsg} 详细信息：{businessException.Details} 内部异常：{businessException.InnerException}");
                    context.Result = new ContentResult
                    {
                        // 返回状态码501，表示服务器业务异常
                        StatusCode = StatusCodes.Status501NotImplemented,
                        Content = context.Exception.Message
                    };
                }

                //设置为true，表示异常被处理了
                context.ExceptionHandled = true;
            }
        }
    }
}
