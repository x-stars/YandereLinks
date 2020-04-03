using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace XstarS.YandereLinks.Properties
{
    /// <summary>
    /// 提供应用程序的字符串资源。
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
    internal static class StringResources
    {
        /// <summary>
        /// 表示本地化的字符串资源存储。
        /// </summary>
        private static readonly Dictionary<CultureInfo, Dictionary<string, string>> LocalizedStrings;

        /// <summary>
        /// 初始化 <see cref="StringResources"/> 类的静态成员。
        /// </summary>
        static StringResources()
        {
            StringResources.LocalizedStrings = new Dictionary<CultureInfo, Dictionary<string, string>>()
            {
                {
                    new CultureInfo("zh-CN"),
                    new Dictionary<string, string>()
                    {
                        { nameof(MainWindow_Title), "yande.re 链接提取" },
                        { nameof(MainWindow_ExtractButton), "提取链接" },
                        { nameof(MainWindow_EnumerateButton), "遍历页面" },
                        { nameof(MainWindow_CancelButton), "取消" },
                        { nameof(MainWindow_CopyButton), "复制" },
                        { nameof(MainWindow_ClearButton), "清除" },
                        { nameof(ConsoleWindow_Complete), "——完成！——" },
                        {
                            nameof(ConsoleWindow_Help_Usage),
                            "XstarS.YandereLinks.exe PageLink [-e PageCount] [-t MaxThreads] [-o OutFile] [-h]"
                        },
                        {
                            nameof(ConsoleWindow_Help_PageLink),
                            "    PageLink        要提取链接的 yande.re 页面的 URL。支持多值输入。"
                        },
                        {
                            nameof(ConsoleWindow_Help_PageCount),
                            "    -e PageCount    指定要遍历页面的数量；" +
                            Environment.NewLine +
                            "                    为 0 则不进行遍历，为 -1 则遍历至最后一页。"
                        },
                        {
                            nameof(ConsoleWindow_Help_MaxThreads),
                            "    -t MaxThreads   指定 HTTP 访问的最大线程数。"
                        },
                        {
                            nameof(ConsoleWindow_Help_OutFile),
                            "    -o OutFile      指定输出图片链接的文本文件的路径。"
                        },
                        {
                            nameof(ConsoleWindow_Help_Help),
                            "    -h              显示此帮助信息。"
                        }
                    }
                },
                {
                    CultureInfo.InvariantCulture,
                    new Dictionary<string, string>()
                    {
                        { nameof(MainWindow_Title), "yande.re Link Extract" },
                        { nameof(MainWindow_ExtractButton), "Extract" },
                        { nameof(MainWindow_EnumerateButton), "Enumerate" },
                        { nameof(MainWindow_CancelButton), "Cancel" },
                        { nameof(MainWindow_CopyButton), "Copy" },
                        { nameof(MainWindow_ClearButton), "Clear" },
                        { nameof(ConsoleWindow_Complete), "---Complete!---" },
                        {
                            nameof(ConsoleWindow_Help_Usage),
                            "XstarS.YandereLinks.exe PageLink [-e PageCount] [-t MaxThreads] [-o OutFile] [-h]"
                        },
                        {
                            nameof(ConsoleWindow_Help_PageLink),
                            "    PageLink        URL(s) of yande.re page(s) which contains image links."
                        },
                        {
                            nameof(ConsoleWindow_Help_PageCount),
                            "    -e PageCount    Count of pages you want to enumerate from input page." +
                            Environment.NewLine +
                            "                    0 means no enumeration, -1 means enumerating to end."
                        },
                        {
                            nameof(ConsoleWindow_Help_MaxThreads),
                            "    -t MaxThreads   Maximum threads in HTTP access."
                        },
                        {
                            nameof(ConsoleWindow_Help_OutFile),
                            "    -o OutFile      Path of a text file to save to image links."
                        },
                        {
                            nameof(ConsoleWindow_Help_Help),
                            "    -h              Show this help message."
                        }
                    }
                }
            };
        }

        /// <summary>
        /// 字符串资源支持的所有区域信息。
        /// </summary>
        public static ICollection<CultureInfo> SupportedCultures =>
            StringResources.LocalizedStrings.Keys;

        /// <summary>
        /// 根据当前区域信息和资源名称获取指定类型的字符串资源的值。
        /// </summary>
        /// <param name="resourceName">字符串资源的名称。可由编译器自动获取。</param>
        /// <returns><see cref="StringResources.LocalizedStrings"/>
        /// 中对应当前区域信息且名称为 <paramref name="resourceName"/> 的字符串资源的值。</returns>
        internal static string GetString([CallerMemberName] string resourceName = null) =>
            StringResources.LocalizedStrings.ContainsKey(CultureInfo.CurrentUICulture) ?
            StringResources.LocalizedStrings[CultureInfo.CurrentUICulture][resourceName] :
            StringResources.LocalizedStrings.ContainsKey(CultureInfo.InvariantCulture) ?
            StringResources.LocalizedStrings[CultureInfo.InvariantCulture][resourceName] :
            string.Empty;

        #region 字符串资源对外接口。
        public static string MainWindow_Title => StringResources.GetString();
        public static string MainWindow_ExtractButton => StringResources.GetString();
        public static string MainWindow_EnumerateButton => StringResources.GetString();
        public static string MainWindow_CancelButton => StringResources.GetString();
        public static string MainWindow_CopyButton => StringResources.GetString();
        public static string MainWindow_ClearButton => StringResources.GetString();
        public static string ConsoleWindow_Help_Usage => StringResources.GetString();
        public static string ConsoleWindow_Help_PageLink => StringResources.GetString();
        public static string ConsoleWindow_Help_PageCount => StringResources.GetString();
        public static string ConsoleWindow_Help_MaxThreads => StringResources.GetString();
        public static string ConsoleWindow_Help_OutFile => StringResources.GetString();
        public static string ConsoleWindow_Help_Help => StringResources.GetString();
        public static string ConsoleWindow_Complete => StringResources.GetString();
        #endregion
    }
}
