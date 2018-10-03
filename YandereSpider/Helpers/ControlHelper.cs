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
    internal static class ControlHelper
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

        /// <summary>
        /// 获取计算机上运行的 Internet Explorer 版本。
        /// </summary>
        /// <param name="source">一个 <see cref="WebBrowser"/> 对象。</param>
        /// <returns>计算机上运行的 Internet Explorer 的版本;
        /// 若为 <see langword="null"/>，表示不存在 Internet Explorer。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="SecurityException">
        /// 程序没有足够的权限读取注册表。</exception>
        public static int? GetInternetExplorerVersion(this WebBrowser source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            string ieRegKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer";
            string versionSz = (Registry.GetValue(ieRegKey, "svcVersion", null) ??
                Registry.GetValue(ieRegKey, "Version", null)) as string;
            string mainVersionSz = versionSz?.Split('.')[0];
            return (mainVersionSz is null) ? (int?)null : int.Parse(mainVersionSz);
        }

        /// <summary>
        /// 获取当前程序的 <see cref="WebBrowser"/> 运行的 Internet Explorer 的版本。
        /// </summary>
        /// <param name="source">一个 <see cref="WebBrowser"/> 对象。</param>
        /// <returns>当前程序的 <see cref="WebBrowser"/> 运行的 Internet Explorer 的版本;
        /// 若为 <see langword="null"/>，表示运行默认版本 (7)。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="SecurityException">
        /// 程序没有足够的权限读取注册表。</exception>
        public static int? GetVersion(this WebBrowser source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            string featureControlRegKey =
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\" +
                @"MAIN\FeatureControl\FEATURE_BROWSER_EMULATION";
            string appName = AppDomain.CurrentDomain.FriendlyName;
            int? versionDword = Registry.GetValue(featureControlRegKey, appName, null) as int?;
            return (versionDword.HasValue) ? (versionDword / 1000) : null;
        }

        /// <summary>
        /// 设置当前程序的 <see cref="WebBrowser"/> 运行的 Internet Explorer 的版本。
        /// </summary>
        /// <param name="source">一个 <see cref="WebBrowser"/> 对象。</param>
        /// <param name="version">Internet Explorer 的版本号，应介于 7 和 11。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="version"/> 不在 7 和 11 之间。</exception>
        /// <exception cref="SecurityException">
        /// 程序没有足够的权限修改注册表。</exception>
        public static void SetVersion(this WebBrowser source, int version)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if ((version < 7) || (version > 11))
            {
                throw new ArgumentOutOfRangeException(nameof(version));
            }

            string featureControlRegKey =
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\" + 
                @"MAIN\FeatureControl\FEATURE_BROWSER_EMULATION";
            string appName = AppDomain.CurrentDomain.FriendlyName;
            Registry.SetValue(featureControlRegKey, appName, version * 1000, RegistryValueKind.DWord);
        }
    }
}
