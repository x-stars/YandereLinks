#if !CORE

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace XstarS.Win32
{
    /// <summary>
    /// 提供 Win32 系统组件相关的帮助方法。
    /// </summary>
    internal static class SystemComponents
    {
        /// <summary>
        /// 获取计算机上运行的 Internet Explorer 版本。
        /// </summary>
        /// <returns>计算机上运行的 Internet Explorer 的版本;
        /// 若为 <see langword="null"/>，表示不存在 Internet Explorer。</returns>
        /// <exception cref="SecurityException">
        /// 程序没有足够的权限读取注册表。</exception>
        public static int? GetInternetExplorerVersion()
        {
            string ieRegKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer";
            string versionSz = (Registry.GetValue(ieRegKey, "svcVersion", null) ??
                Registry.GetValue(ieRegKey, "Version", null)) as string;
            string mainVersionSz = versionSz?.Split('.')[0];
            return (mainVersionSz is null) ? (int?)null : int.Parse(mainVersionSz);
        }

        /// <summary>
        /// 获取当前程序的内置浏览器运行的 Internet Explorer 的版本。
        /// </summary>
        /// <returns>当前程序的内置浏览器运行的 Internet Explorer 的版本;
        /// 若为 <see langword="null"/>，表示运行默认版本 (7)。</returns>
        /// <exception cref="SecurityException">
        /// 程序没有足够的权限读取注册表。</exception>
        public static int? GetWebBrowserVersion()
        {
            string featureControlRegKey =
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\" +
                @"MAIN\FeatureControl\FEATURE_BROWSER_EMULATION";
            string appName = AppDomain.CurrentDomain.FriendlyName;
            int? versionDword = Registry.GetValue(featureControlRegKey, appName, null) as int?;
            return (versionDword.HasValue) ? (versionDword / 1000) : null;
        }

        /// <summary>
        /// 设置当前程序的内置浏览器运行的 Internet Explorer 的版本。
        /// </summary>
        /// <param name="version">Internet Explorer 的版本号，应介于 7 和 11。</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="version"/> 不在 7 和 11 之间。</exception>
        /// <exception cref="SecurityException">
        /// 程序没有足够的权限修改注册表。</exception>
        public static void SetWebBrowserVersion(int version)
        {
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

#endif
