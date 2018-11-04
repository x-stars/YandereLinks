using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace YandereSpider.Properties
{
    /// <summary>
    /// 提供应用程序的本地化资源。
    /// </summary>
    internal static class LocalizedResources
    {
        private static readonly CultureInfo ResourceCulture = CultureInfo.CurrentUICulture;
        private static readonly IDictionary<CultureInfo, string> MainWindow_Title_ResDict;
        private static readonly IDictionary<CultureInfo, string> MainWindow_ExtractButton_ResDict;
        private static readonly IDictionary<CultureInfo, string> MainWindow_EnumerateButton_ResDict;
        private static readonly IDictionary<CultureInfo, string> MainWindow_CopyButton_ResDict;
        private static readonly IDictionary<CultureInfo, string> MainWindow_ClearButton_ResDict;
        private static readonly IDictionary<CultureInfo, string> MainWindow_CancelButton_ResDict;
        private static readonly IDictionary<CultureInfo, string> ConsoleWindow_Help_Usage_ResDict;
        private static readonly IDictionary<CultureInfo, string> ConsoleWindow_Help_PageLink_ResDict;
        private static readonly IDictionary<CultureInfo, string> ConsoleWindow_Help_PageCount_ResDict;
        private static readonly IDictionary<CultureInfo, string> ConsoleWindow_Help_MaxThreads_ResDict;
        private static readonly IDictionary<CultureInfo, string> ConsoleWindow_Help_OutFile_ResDict;
        private static readonly IDictionary<CultureInfo, string> ConsoleWindow_Help_Help_ResDict;
        private static readonly IDictionary<CultureInfo, string> ConsoleWindow_Complete_ResDict;

        /// <summary>
        /// 初始化 <see cref="LocalizedResources"/> 类的静态成员。
        /// </summary>
        static LocalizedResources()
        {
            LocalizedResources.MainWindow_Title_ResDict = new Dictionary<CultureInfo, string>()
            {
                { new CultureInfo("zh-CN"), "yande.re 链接提取" },
                { CultureInfo.InvariantCulture, "yande.re Link Extract" }
            };
            LocalizedResources.MainWindow_ExtractButton_ResDict = new Dictionary<CultureInfo, string>()
            {
                { new CultureInfo("zh-CN"), "提取链接" },
                { CultureInfo.InvariantCulture, "Extract" }
            };
            LocalizedResources.MainWindow_EnumerateButton_ResDict = new Dictionary<CultureInfo, string>()
            {
                { new CultureInfo("zh-CN"), "遍历页面" },
                { CultureInfo.InvariantCulture, "Enumerate" }
            };
            LocalizedResources.MainWindow_CopyButton_ResDict = new Dictionary<CultureInfo, string>()
            {
                { new CultureInfo("zh-CN"), "复制到剪贴板" },
                { CultureInfo.InvariantCulture, "Copy" }
            };
            LocalizedResources.MainWindow_ClearButton_ResDict = new Dictionary<CultureInfo, string>()
            {
                { new CultureInfo("zh-CN"), "清除" },
                { CultureInfo.InvariantCulture, "Clear" }
            };
            LocalizedResources.MainWindow_CancelButton_ResDict = new Dictionary<CultureInfo, string>()
            {
                { new CultureInfo("zh-CN"), "取消" },
                { CultureInfo.InvariantCulture, "Cancel" }
            };
            LocalizedResources.ConsoleWindow_Help_Usage_ResDict = new Dictionary<CultureInfo, string>()
            {
                {
                    new CultureInfo("zh-CN"),
                    "YandereSpider.exe PageLink [-e PageCount] [-t MaxThreads] [-o OutFile] [-h]"
                },
                {
                    CultureInfo.InvariantCulture,
                    "YandereSpider.exe PageLink [-e PageCount] [-t MaxThreads] [-o OutFile] [-h]"
                }
            };
            LocalizedResources.ConsoleWindow_Help_PageLink_ResDict = new Dictionary<CultureInfo, string>()
            {
                {
                    new CultureInfo("zh-CN"),
                    "    PageLink        要提取链接的 yande.re 页面的 URL。支持多值输入。"
                },
                {
                    CultureInfo.InvariantCulture,
                    "    PageLink        URL(s) of yande.re page(s) which contains image links."
                }
            };
            LocalizedResources.ConsoleWindow_Help_PageCount_ResDict = new Dictionary<CultureInfo, string>()
            {
                {
                    new CultureInfo("zh-CN"),
                    "    -e PageCount    指定要遍历页面的数量；" +
                    Environment.NewLine +
                    "                    为 0 则不进行遍历，为 -1 则遍历至最后一页。"
                },
                {
                    CultureInfo.InvariantCulture,
                    "    -e PageCount    Count of pages you want to enumerate from input page." +
                    Environment.NewLine +
                    "                    0 means no enumeration, -1 means enumerating to end."
                }
            };
            LocalizedResources.ConsoleWindow_Help_MaxThreads_ResDict = new Dictionary<CultureInfo, string>()
            {
                {
                    new CultureInfo("zh-CN"),
                    "    -t MaxThreads   指定 HTTP 访问的最大线程数。"
                },
                {
                    CultureInfo.InvariantCulture,
                    "    -t MaxThreads   Maximum threads in HTTP access."
                }
            };
            LocalizedResources.ConsoleWindow_Help_OutFile_ResDict = new Dictionary<CultureInfo, string>()
            {
                {
                    new CultureInfo("zh-CN"),
                    "    -o OutFile      指定输出图片链接的文本文件的路径。"
                },
                {
                    CultureInfo.InvariantCulture,
                    "    -o OutFile      Path of a text file to save to image links."
                }
            };
            LocalizedResources.ConsoleWindow_Help_Help_ResDict = new Dictionary<CultureInfo, string>()
            {
                {
                    new CultureInfo("zh-CN"),
                    "    -h              显示此帮助信息。"
                },
                {
                    CultureInfo.InvariantCulture,
                    "    -h              Show this help message."
                }
            };
            LocalizedResources.ConsoleWindow_Complete_ResDict = new Dictionary<CultureInfo, string>()
            {
                { new CultureInfo("zh-CN"), "——完成！——" },
                { CultureInfo.InvariantCulture, "---Complete!---" }
            };
        }

        public static string MainWindow_Title =>
            LocalizedResources.GetLocalizedResource(LocalizedResources.MainWindow_Title_ResDict);
        public static string MainWindow_ExtractButton =>
            LocalizedResources.GetLocalizedResource(LocalizedResources.MainWindow_ExtractButton_ResDict);
        public static string MainWindow_EnumerateButton =>
            LocalizedResources.GetLocalizedResource(LocalizedResources.MainWindow_EnumerateButton_ResDict);
        public static string MainWindow_CopyButton =>
            LocalizedResources.GetLocalizedResource(LocalizedResources.MainWindow_CopyButton_ResDict);
        public static string MainWindow_ClearButton =>
            LocalizedResources.GetLocalizedResource(LocalizedResources.MainWindow_ClearButton_ResDict);
        public static string MainWindow_CancelButton =>
            LocalizedResources.GetLocalizedResource(LocalizedResources.MainWindow_CancelButton_ResDict);
        public static string ConsoleWindow_Help_Usage =>
            LocalizedResources.GetLocalizedResource(LocalizedResources.ConsoleWindow_Help_Usage_ResDict);
        public static string ConsoleWindow_Help_PageLink =>
            LocalizedResources.GetLocalizedResource(LocalizedResources.ConsoleWindow_Help_PageLink_ResDict);
        public static string ConsoleWindow_Help_PageCount =>
            LocalizedResources.GetLocalizedResource(LocalizedResources.ConsoleWindow_Help_PageCount_ResDict);
        public static string ConsoleWindow_Help_MaxThreads =>
            LocalizedResources.GetLocalizedResource(LocalizedResources.ConsoleWindow_Help_MaxThreads_ResDict);
        public static string ConsoleWindow_Help_OutFile =>
            LocalizedResources.GetLocalizedResource(LocalizedResources.ConsoleWindow_Help_OutFile_ResDict);
        public static string ConsoleWindow_Help_Help =>
            LocalizedResources.GetLocalizedResource(LocalizedResources.ConsoleWindow_Help_Help_ResDict);
        public static string ConsoleWindow_Complete =>
            LocalizedResources.GetLocalizedResource(LocalizedResources.ConsoleWindow_Complete_ResDict);

        /// <summary>
        /// 根据当前区域信息获取资源字典中的值。
        /// </summary>
        /// <typeparam name="T">资源字典中值的类型。</typeparam>
        /// <param name="resDict">区域特定的资源字典。</param>
        /// <returns><paramref name="resDict"/> 中对应当前区域信息的值。</returns>
        private static T GetLocalizedResource<T>(IDictionary<CultureInfo, T> resDict) =>
            resDict.ContainsKey(LocalizedResources.ResourceCulture) ?
            resDict[LocalizedResources.ResourceCulture] :
            (resDict.ContainsKey(CultureInfo.InvariantCulture) ?
            resDict[CultureInfo.InvariantCulture] : default(T));
    }
}
