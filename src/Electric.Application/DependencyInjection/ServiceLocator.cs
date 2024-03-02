namespace Electric.Application.DependencyInjection
{
    public static class ServiceLocator
    {
        /// <summary>
        /// 实例
        /// </summary>
        public static IServiceProvider Instance { get; set; } = null;
    }
}