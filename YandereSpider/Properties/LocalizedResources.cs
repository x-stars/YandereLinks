using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace YandereSpider.Properties
{
    /// <summary>
    /// 提供应用程序的本地化资源。
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
    internal static class LocalizedResources
    {
        /// <summary>
        /// 本地化资源内部存储。
        /// </summary>
        private static readonly IDictionary<CultureInfo, IDictionary<string, object>> InternalResources;

        /// <summary>
        /// 初始化 <see cref="LocalizedResources"/> 类的静态成员。
        /// </summary>
        static LocalizedResources()
        {
            LocalizedResources.InternalResources = new Dictionary<CultureInfo, IDictionary<string, object>>()
            {
                {
                    new CultureInfo("zh-CN"),
                    new Dictionary<string, object>()
                    {
                        { nameof(MainWindow_Title), "yande.re 链接提取" },
                        { nameof(MainWindow_ExtractButton), "提取链接" },
                        { nameof(MainWindow_EnumerateButton), "遍历页面" },
                        { nameof(MainWindow_CancelButton), "取消" },
                        { nameof(MainWindow_CopyButton), "复制到剪贴板" },
                        { nameof(MainWindow_ClearButton), "清除" },
                        { nameof(ConsoleWindow_Complete), "——完成！——" },
                        {
                            nameof(ConsoleWindow_Help_Usage),
                            "YandereSpider.exe PageLink [-e PageCount] [-t MaxThreads] [-o OutFile] [-h]"
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
                    new Dictionary<string, object>()
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
                            "YandereSpider.exe PageLink [-e PageCount] [-t MaxThreads] [-o OutFile] [-h]"
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
        /// 本地化资源支持的所有区域信息。
        /// </summary>
        public static ICollection<CultureInfo> SupportedCulture =>
            LocalizedResources.InternalResources.Keys;

        #region 本地化资源对外接口。
        public static string MainWindow_Title => LocalizedResources.Get<string>();
        public static string MainWindow_ExtractButton => LocalizedResources.Get<string>();
        public static string MainWindow_EnumerateButton => LocalizedResources.Get<string>();
        public static string MainWindow_CancelButton => LocalizedResources.Get<string>();
        public static string MainWindow_CopyButton => LocalizedResources.Get<string>();
        public static string MainWindow_ClearButton => LocalizedResources.Get<string>();
        public static string ConsoleWindow_Help_Usage => LocalizedResources.Get<string>();
        public static string ConsoleWindow_Help_PageLink => LocalizedResources.Get<string>();
        public static string ConsoleWindow_Help_PageCount => LocalizedResources.Get<string>();
        public static string ConsoleWindow_Help_MaxThreads => LocalizedResources.Get<string>();
        public static string ConsoleWindow_Help_OutFile => LocalizedResources.Get<string>();
        public static string ConsoleWindow_Help_Help => LocalizedResources.Get<string>();
        public static string ConsoleWindow_Complete => LocalizedResources.Get<string>();
        #endregion

        /// <summary>
        /// 根据当前区域信息和资源名称获取指定类型的资源的值。
        /// </summary>
        /// <typeparam name="TRes">资源的类型。</typeparam>
        /// <param name="resourceName">资源的名称。可由编译器自动获取。</param>
        /// <returns><see cref="LocalizedResources.InternalResources"/>
        /// 中对应当前区域信息且名称为 <paramref name="resourceName"/> 的资源的值。</returns>
        internal static TRes Get<TRes>([CallerMemberName] string resourceName = null) =>
            LocalizedResources.InternalResources.ContainsKey(CultureInfo.CurrentUICulture) ?
            (TRes)LocalizedResources.InternalResources[CultureInfo.CurrentUICulture][resourceName] :
            LocalizedResources.InternalResources.ContainsKey(CultureInfo.InvariantCulture) ?
            (TRes)LocalizedResources.InternalResources[CultureInfo.InvariantCulture][resourceName] :
            default(TRes);
    }
}
