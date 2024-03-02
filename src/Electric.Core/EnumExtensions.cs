using Electric.Core.Exceptions;

namespace Electric.Core
{
    /// <summary>
    /// 枚举扩展方法
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 字符串转Enum
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="str">字符串</param>
        /// <returns>转换的枚举</returns>
        public static T ToEnum<T>(this string str) where T : Enum
        {
            try
            {
                return (T)Enum.Parse(typeof(T), str);
            }
            catch
            {
                throw new BusinessException($"字符串转枚举失败，字符串：{str}，枚举类型：{typeof(T)}");
            }
        }
    }
}