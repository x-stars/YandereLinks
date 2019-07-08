using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace XstarS.Windows.Controls
{
    /// <summary>
    /// 提供 WPF 用户控件 <see cref="Control"/> 及其派生类的扩展方法。
    /// </summary>
    internal static class ControlExtensions
    {
        /// <summary>
        /// 设定是否禁止当前 <see cref="WebBrowser"/> 的脚本错误提示。
        /// </summary>
        /// <param name="browser">要设定脚本错误提示的 <see cref="WebBrowser"/> 对象。</param>
        /// <param name="supresses">指示是否要禁止脚本错误提示。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="browser"/> 为 <see langword="null"/>。</exception>
        public static void SuppressScriptErrors(this WebBrowser browser, bool supresses = true)
        {
            if (browser is null)
            {
                throw new ArgumentNullException(nameof(browser));
            }

            var if__axIWebBrowser2 = typeof(WebBrowser).GetField(
                "_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (if__axIWebBrowser2 is null) { return; }

            var _axIWebBrowser2 = if__axIWebBrowser2.GetValue(browser);
            if (_axIWebBrowser2 is null) { return; }

            _axIWebBrowser2.GetType().InvokeMember(
                "Silent", BindingFlags.SetProperty, null, _axIWebBrowser2, new object[] { supresses });
        }
    }
}
