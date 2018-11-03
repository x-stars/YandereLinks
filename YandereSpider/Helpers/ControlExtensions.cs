using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace XstarS.Windows.Controls
{
    /// <summary>
    /// 提供 WPF 用户控件 (<see cref="Control"/>) 及其派生类的扩展方法。
    /// </summary>
    internal static class ControlExtensions
    {
        /// <summary>
        /// 设定是否禁止当前 <see cref="WebBrowser"/> 的脚本错误提示。
        /// </summary>
        /// <param name="source">一个 <see cref="WebBrowser"/> 对象。</param>
        /// <param name="supresses">指示是否要禁止脚本错误提示。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static void SuppressScriptErrors(this WebBrowser source, bool supresses = true)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var innerComWebBrowserFiledInfo = typeof(WebBrowser).GetField(
                "_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (innerComWebBrowserFiledInfo is null) { return; }

            object innerComWebBrowser = innerComWebBrowserFiledInfo.GetValue(source);
            if (innerComWebBrowser is null) { return; }

            innerComWebBrowser.GetType().InvokeMember(
                "Silent", BindingFlags.SetProperty, null, innerComWebBrowser, new object[] { supresses });
        }
    }
}
