using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XstarS
{
    /// <summary>
    /// 提供参数检查和抛出对应的异常的方法。
    /// </summary>
    internal static class ArgumentCheckHelper
    {
        /// <summary>
        /// 检查当前对象是否不为 <see langword="null"/>。
        /// </summary>
        /// <param name="source">一个 <see cref="object"/> 对象。</param>
        /// <param name="paramName">参数名称，一般使用 <see langword="nameof"/> 获取。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static void CheckNotNull(this object source,
            string paramName = null)
        {
            if (source is null)
            { throw new ArgumentNullException(paramName); }
        }

        /// <summary>
        /// 检查当前对象是否满足指定条件。
        /// </summary>
        /// <typeparam name="T"><paramref name="source"/> 的类型。</typeparam>
        /// <param name="source">一个值类型对象。</param>
        /// <param name="predicate">判断条件。</param>
        /// <param name="paramName">参数名称，一般使用 <see langword="nameof"/> 获取。</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 不满足 <paramref name="predicate"/> 中的条件。</exception>
        public static void CheckMeet<T>(this T source,
            Predicate<T> predicate,
            string paramName = null) where T : struct
        {
            if (!predicate(source))
            { throw new ArgumentException("NotMeet", paramName); }
        }

        /// <summary>
        /// 检查当前对象是否不为 <see langword="null"/> 且满足指定条件。
        /// </summary>
        /// <typeparam name="T"><paramref name="source"/> 的类型。</typeparam>
        /// <param name="source">一个引用类型对象。</param>
        /// <param name="predicate">判断条件。</param>
        /// <param name="paramName">参数名称，一般使用 <see langword="nameof"/> 获取。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 不满足 <paramref name="predicate"/> 中的条件。</exception>
        public static void CheckNotNullAndMeet<T>(this T source,
            Predicate<T> predicate,
            string paramName = null) where T : class
        {
            source.CheckNotNull(paramName);
            if (!predicate(source))
            { throw new ArgumentException("NotMeet", paramName); }
        }

        /// <summary>
        /// 检查当前字符串是否不为 <see langword="null"/> 或空字符串 <see cref="string.Empty"/>。
        /// </summary>
        /// <param name="source">一个字符串。</param>
        /// <param name="paramName">参数名称，一般使用 <see langword="nameof"/> 获取。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 为空字符串。</exception>
        public static void CheckNotNullOrEmpty(this string source,
            string paramName = null)
        {
            source.CheckNotNull();
            if (string.IsNullOrEmpty(source))
            { throw new ArgumentException("IsEmpty", paramName); }
        }

        /// <summary>
        /// 检查当前字符串是否不为 <see langword="null"/> 或空字符串 <see cref="string.Empty"/>
        /// 或空白字符串（参见 <see cref="char.IsWhiteSpace(char)"/>）。
        /// </summary>
        /// <param name="source">一个字符串。</param>
        /// <param name="paramName">参数名称，一般使用 <see langword="nameof"/> 获取。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 为空字符串或全为空白字符。</exception>
        public static void CheckNotNullOrWhitespace(this string source,
            string paramName = null)
        {
            source.CheckNotNullOrEmpty();
            if (string.IsNullOrWhiteSpace(source))
            { throw new ArgumentException("IsWhitespace", paramName); }
        }

        /// <summary>
        /// 检查当前集合是否不为 <see langword="null"/> 或空集合。
        /// </summary>
        /// <param name="source">一个集合。</param>
        /// <param name="paramName">参数名称，一般使用 <see langword="nameof"/> 获取。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 为空集合。</exception>
        public static void CheckNotNullOrEmpty<T>(this IEnumerable<T> source,
            string paramName = null)
        {
            source.CheckNotNull(paramName);
            if (!source.Any())
            { throw new ArgumentException("IsEmpty", paramName); }
        }

        /// <summary>
        /// 检查当前整数是否在指定的范围内。
        /// </summary>
        /// <param name="source">一个 32 位有符号整数。</param>
        /// <param name="minValue"><paramref name="source"/> 允许的最小值。</param>
        /// <param name="maxValue"><paramref name="source"/> 允许的最大值。</param>
        /// <param name="paramName">参数名称，一般使用 <see langword="nameof"/> 获取。</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="source"/> 的值不在
        /// <paramref name="minValue"/> 和 <paramref name="maxValue"/> 之间。</exception>
        public static void CheckInRange(this int source,
            int minValue = 0, int maxValue = int.MaxValue,
            string paramName = null)
        {
            if ((source < minValue) || (source > maxValue))
            { throw new ArgumentOutOfRangeException(paramName); }
        }
    }
}
