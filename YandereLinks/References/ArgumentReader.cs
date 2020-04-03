using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS.CommandLine
{
    /// <summary>
    /// 简单的通用命令行参数解析器 <see cref="ArgumentReader"/>。
    /// 为命令行参数解析器提供基类和默认实现。
    /// </summary>
    /// <remarks>
    /// 不支持 Unix / Linux shell 中连字符 "-" 后接多个开关参数的解析。
    /// 不支持 PowerShell 中允许省略参数名称的有名参数的解析。
    /// 不支持一个参数名称后跟多个参数值的有名参数的解析。
    /// 不支持多个同名的有名参数的解析。
    /// </remarks>
    [Serializable]
    public class ArgumentReader
    {
        /// <summary>
        /// 初始化命令行参数解析器 <see cref="ArgumentReader"/> 的新实例。
        /// </summary>
        /// <remarks>
        /// 输入的参数名称列表用于解析无名参数；若无需解析无名参数，可留空。
        /// </remarks>
        /// <param name="arguments">待解析的参数列表。</param>
        /// <param name="ignoreCase">参数名称是否忽略大小写。</param>
        /// <param name="parameterNames">所有有名参数名称列表。</param>
        /// <param name="switchNames">所有开关参数名称列表。</param>
        public ArgumentReader(string[] arguments, bool ignoreCase,
            string[] parameterNames = null, string[] switchNames = null)
        {
            this.Arguments = Array.AsReadOnly(arguments ?? Array.Empty<string>());
            this.SwitchNames = Array.AsReadOnly(switchNames ?? Array.Empty<string>());
            this.ParameterNames = Array.AsReadOnly(parameterNames ?? Array.Empty<string>());
            this.NameComparer = ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
        }

        /// <summary>
        /// 待解析的参数列表。
        /// </summary>
        public IReadOnlyList<string> Arguments { get; }

        /// <summary>
        /// 有名参数名称列表。
        /// </summary>
        public IReadOnlyList<string> ParameterNames { get; }

        /// <summary>
        /// 开关参数名称列表。
        /// </summary>
        public IReadOnlyList<string> SwitchNames { get; }

        /// <summary>
        /// 比较参数名称时采用的字符串比较器。
        /// </summary>
        protected IEqualityComparer<string> NameComparer { get; }

        /// <summary>
        /// 解析指定名称的有名参数。
        /// </summary>
        /// <param name="name">要解析的有名参数的名称。</param>
        /// <returns>名称为 <paramref name="name"/> 的有名参数的值；
        /// 若不存在则为 <see langword="null"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> 为 <see langword="null"/>。</exception>
        public virtual string GetParameter(string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            for (int i = 0; i < this.Arguments.Count - 1; i++)
            {
                if (this.NameComparer.Equals(this.Arguments[i], name))
                {
                    return this.Arguments[i + 1];
                }
            }

            return null;
        }

        /// <summary>
        /// 解析指定位置的无名参数。
        /// </summary>
        /// <param name="position">要解析的无名参数在所有无名参数中的位置。</param>
        /// <returns>位置为 <paramref name="position"/> 的无名参数的值；
        /// 若不存在则为 <see langword="null"/>。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="position"/> 小于 0。</exception>
        public virtual string GetParameter(int position)
        {
            if (position < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            for (int index = 0, positionNow = 0; index < this.Arguments.Count; index++)
            {
                if (this.SwitchNames.Contains(this.Arguments[index], this.NameComparer))
                {
                }
                else if (this.ParameterNames.Contains(this.Arguments[index], this.NameComparer))
                {
                    index++;
                }
                else if (positionNow == position)
                {
                    return this.Arguments[index];
                }
                else
                {
                    positionNow++;
                }
            }

            return null;
        }

        /// <summary>
        /// 解析指定名称的开关参数。
        /// </summary>
        /// <param name="name">要解析的开关参数的名称。</param>
        /// <returns>若名称为 <paramref name="name"/> 的开关参数存在，
        /// 则为 <see langword="true"/>； 否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> 为 <see langword="null"/>。</exception>
        public virtual bool GetSwitch(string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return this.Arguments.Contains(name, this.NameComparer);
        }
    }
}
