using System;
using System.Security;
using Microsoft.Win32;

namespace XstarS.Win32
{
    /// <summary>
    /// 表示 Win32 系统组件。
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
    public static class SystemComponents
    {
        /// <summary>
        /// 表示网络浏览器 Internet Explorer。
        /// </summary>
        public static class InternetExplorer
        {
            /// <summary>
            /// 获取计算机上运行的 Internet Explorer 的版本。
            /// </summary>
            /// <exception cref="SecurityException">
            /// 程序没有足够的权限读取注册表。</exception>
            public static Version Version
            {
                get
                {
                    using (var regKeyIE = Registry.LocalMachine.OpenSubKey(
                        @"SOFTWARE\Microsoft\Internet Explorer"))
                    {
                        var versionSz = (regKeyIE.GetValue("svcVersion") ??
                            regKeyIE.GetValue("Version")) as string;
                        return (versionSz is null) ? null : new Version(versionSz);
                    }
                }
            }

            /// <summary>
            /// 获取计算机上运行的 Internet Explorer 的主要版本。
            /// </summary>
            /// <exception cref="SecurityException">
            /// 程序没有足够的权限读取注册表。</exception>
            public static int? MajorVersion =>
                SystemComponents.InternetExplorer.Version?.Major;
        }

        /// <summary>
        /// 表示当前程序的内置网络浏览器组件。
        /// </summary>
        public static class WebBrowser
        {
            /// <summary>
            /// 获取或设置当前程序的内置网络浏览器在当前用户下运行的 Internet Explorer 的主要版本和渲染模式。
            /// <see langword="null"/> 表示运行默认主要版本 (7) 和默认渲染模式 (000)。
            /// </summary>
            /// <exception cref="ArgumentOutOfRangeException">
            /// <see langword="value"/> 的千位的不在 7 和当前 Internet Explorer 主要版本之间。</exception>
            /// <exception cref="UnauthorizedAccessException">
            /// 程序无法以写访问权限打开注册表项。</exception>
            /// <exception cref="SecurityException">
            /// 程序没有足够的权限读取注册表。</exception>
            public static int? VersionInCurrentUser
            {
                get
                {
                    using (var regKeyBrowser = Registry.CurrentUser.OpenSubKey(
                        @"SOFTWARE\Microsoft\Internet Explorer\" +
                        @"MAIN\FeatureControl\FEATURE_BROWSER_EMULATION", false))
                    {
                        var appName = AppDomain.CurrentDomain.FriendlyName;
                        return regKeyBrowser.GetValue(appName) as int?;
                    }
                }

                set
                {
                    if ((value / 1000 < 7) ||
                        (value / 1000 > SystemComponents.InternetExplorer.MajorVersion))
                    {
                        throw new ArgumentOutOfRangeException(nameof(value));
                    }

                    using (var regKeyBrowser = Registry.CurrentUser.OpenSubKey(
                        @"SOFTWARE\Microsoft\Internet Explorer\" +
                        @"MAIN\FeatureControl\FEATURE_BROWSER_EMULATION", true))
                    {
                        var appName = AppDomain.CurrentDomain.FriendlyName;
                        if (value is null)
                        {
                            regKeyBrowser.DeleteValue(appName);
                        }
                        else
                        {
                            regKeyBrowser.SetValue(appName, value, RegistryValueKind.DWord);
                        }
                    }
                }
            }

            /// <summary>
            /// 获取或设置当前程序的内置网络浏览器在当前用户下运行的 Internet Explorer 的主要版本。
            /// <see langword="null"/> 表示运行默认主要版本 (7)。
            /// </summary>
            /// <exception cref="ArgumentOutOfRangeException">
            /// <see langword="value"/> 不在 7 和当前 Internet Explorer 版本之间。</exception>
            /// <exception cref="UnauthorizedAccessException">
            /// 程序无法以写访问权限打开注册表项。</exception>
            /// <exception cref="SecurityException">
            /// 程序没有足够的权限读取注册表。</exception>
            public static int? MajorVersionInCurrentUser
            {
                get => SystemComponents.WebBrowser.VersionInCurrentUser / 1000;
                set => SystemComponents.WebBrowser.VersionInCurrentUser = value * 1000;
            }

            /// <summary>
            /// 获取或设置当前程序的内置网络浏览器在所有用户下运行的 Internet Explorer 的主要版本和渲染模式。
            /// <see langword="null"/> 表示运行默认主要版本 (7) 和默认渲染模式 (000)。
            /// </summary>
            /// <exception cref="ArgumentOutOfRangeException">
            /// <see langword="value"/> 的千位的不在 7 和当前 Internet Explorer 主要版本之间。</exception>
            /// <exception cref="UnauthorizedAccessException">
            /// 程序无法以写访问权限打开注册表项。</exception>
            /// <exception cref="SecurityException">
            /// 程序没有足够的权限读取注册表。</exception>
            public static int? VersionInLocalMachine
            {
                get
                {
                    using (var regKeyBrowser = Registry.LocalMachine.OpenSubKey(
                        @"SOFTWARE\Microsoft\Internet Explorer\" +
                        @"MAIN\FeatureControl\FEATURE_BROWSER_EMULATION", false))
                    {
                        var appName = AppDomain.CurrentDomain.FriendlyName;
                        return regKeyBrowser.GetValue(appName) as int?;
                    }
                }

                set
                {
                    if ((value / 1000 < 7) ||
                        (value / 1000 > SystemComponents.InternetExplorer.MajorVersion))
                    {
                        throw new ArgumentOutOfRangeException(nameof(value));
                    }

                    using (var regKeyBrowser = Registry.LocalMachine.OpenSubKey(
                        @"SOFTWARE\Microsoft\Internet Explorer\" +
                        @"MAIN\FeatureControl\FEATURE_BROWSER_EMULATION", true))
                    {
                        var appName = AppDomain.CurrentDomain.FriendlyName;
                        if (value is null)
                        {
                            regKeyBrowser.DeleteValue(appName);
                        }
                        else
                        {
                            regKeyBrowser.SetValue(appName, value, RegistryValueKind.DWord);
                        }
                    }
                }
            }

            /// <summary>
            /// 获取或设置当前程序的内置网络浏览器在所有用户下运行的 Internet Explorer 的主要版本。
            /// <see langword="null"/> 表示运行默认主要版本 (7)。
            /// </summary>
            /// <exception cref="ArgumentOutOfRangeException">
            /// <see langword="value"/> 不在 7 和当前 Internet Explorer 版本之间。</exception>
            /// <exception cref="UnauthorizedAccessException">
            /// 程序无法以写访问权限打开注册表项。</exception>
            /// <exception cref="SecurityException">
            /// 程序没有足够的权限读取注册表。</exception>
            public static int? MajorVersionInLocalMachine
            {
                get => SystemComponents.WebBrowser.VersionInLocalMachine / 1000;
                set => SystemComponents.WebBrowser.VersionInLocalMachine = value * 1000;
            }

            /// <summary>
            /// 获取或设置当前程序的内置网络浏览器运行的 Internet Explorer 的主要版本和渲染模式。
            /// <see langword="null"/> 表示运行默认主要版本 (7) 和默认渲染模式 (000)。
            /// </summary>
            /// <exception cref="SecurityException">
            /// 程序没有足够的权限读取注册表。</exception>
            public static int? Version =>
                SystemComponents.WebBrowser.VersionInCurrentUser ??
                SystemComponents.WebBrowser.VersionInLocalMachine;

            /// <summary>
            /// 获取当前程序的内置网络浏览器运行的 Internet Explorer 的主要版本。
            /// <see langword="null"/> 表示运行默认主要版本 (7)。
            /// </summary>
            /// <exception cref="SecurityException">
            /// 程序没有足够的权限读取注册表。</exception>
            public static int? MajorVersion =>
                SystemComponents.WebBrowser.Version / 1000;
        }
    }
}
