namespace Electric.Application.Session
{
    public interface IEleSession
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// 用户登录Id
        /// </summary>
        public Guid UserId { get; }
    }
}