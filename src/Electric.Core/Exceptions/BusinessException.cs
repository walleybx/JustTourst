using Microsoft.Extensions.Logging;

namespace Electric.Core.Exceptions
{
    [Serializable]
    public class BusinessException : Exception, IBusinessException
    {
        public string? Details { get; set; }

        public LogLevel LogLevel { get; set; } = LogLevel.Warning;

        public BusinessException()
        {

        }

        public BusinessException(string? message = null)
           : base(message)
        {

        }

        public BusinessException(string? message = null, string? details = null, Exception? innerException = null, LogLevel logLevel = LogLevel.Warning)
            : base(message, innerException)
        {
            Details = details;
            LogLevel = logLevel;
        }
    }
}
