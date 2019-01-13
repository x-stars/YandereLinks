using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using XstarS;
using XstarS.Win32;

namespace YandereSpider
{
    /// <summary>
    /// 控制台模式的运行逻辑。
    /// </summary>
    public static class ConsoleWindow
    {
        /// <summary>
        /// 全局线程同步锁。
        /// </summary>
        private static readonly object SyncRoot = new object();
        /// <summary>
        /// 指示控制台模式的主要过程是否已经启动。
        /// </summary>
        private static bool IsStarted = false;
        /// <summary>
        /// 当前正在工作的后台线程的数量。
        /// </summary>
        private static int WorkingThreads = -1;
        /// <summary>
        /// 图片链接输出文件的路径。
        /// </summary>
        private static string OutFile = null;
        /// <summary>
        /// 所有图片的链接。
        /// </summary>
        private static readonly ICollection<string> ImageLinks = new HashSet<string>();

        /// <summary>
        /// 显示控制台窗口，并传递程序启动参数。
        /// </summary>
        /// <param name="args">程序的启动参数。</param>
        public static void Show(string[] args)
        {
            if (!ConsoleManager.HasConsole) { ConsoleManager.Show(); }
            if (!ConsoleWindow.IsStarted) { ConsoleWindow.Run(args); }
        }

        /// <summary>
        /// 隐藏命令行窗口，释放标准输入流和错误流。
        /// </summary>
        public static void Hide()
        {
            if (ConsoleManager.HasConsole) { ConsoleManager.Hide(); }
        }

        /// <summary>
        /// 控制台模式的主要过程。
        /// </summary>
        /// <param name="args">程序的启动参数。</param>
        private static void Run(string[] args)
        {
            if (ConsoleWindow.IsStarted) { return; }
            else { ConsoleWindow.IsStarted = true; }

            var param = new ParamReader(args, true, new[] { "-e", "-t", "-o" }, new[] { "-h" });
            if (param.GetSwitch("-h"))
            {
                ConsoleWindow.ShowHelp();
            }
            else
            {
                var pages = new List<YanderePage>();
                for (int i = 0; i <= ushort.MaxValue; i++)
                {
                    var pageLink = param.GetParam(i);
                    if (pageLink is null) { break; }
                    pageLink = ConsoleWindow.FormatPageLink(pageLink);
                    var page = new YanderePage(pageLink);
                    if (!YanderePage.IsYanderePage(page))
                    {
                        throw new ArgumentException(new ArgumentException().Message, "PageLink");
                    }
                    pages.Add(page);
                }
                if (pages.Count == 0) { pages.Add(new YanderePage(YanderePage.IndexPageLink)); }

                int enumCount = int.Parse(param.GetParam("-e") ?? 0.ToString());

                if (!(param.GetParam("-t") is null))
                {
                    int maxThreads = int.Parse(param.GetParam("-t"));
                    if (maxThreads < 1)
                    {
                        throw new ArgumentOutOfRangeException("-t MaxThreads");
                    }
                    ThreadPool.SetMaxThreads(maxThreads, maxThreads);
                }

                if (!(param.GetParam("-o") is null))
                {
                    ConsoleWindow.OutFile = Path.GetFullPath(param.GetParam("-o"));
                    if (!Directory.Exists(Path.GetDirectoryName(ConsoleWindow.OutFile)))
                    {
                        throw new DirectoryNotFoundException();
                    }
                }

                foreach (var page in pages)
                {
                    if (enumCount == 0) { ConsoleWindow.ExtractPage(page); }
                    else { ConsoleWindow.EnumeratePages(page, enumCount); }
                }

                while (ConsoleWindow.WorkingThreads != 0) { Thread.Sleep(10); }

                if (ConsoleWindow.ImageLinks.Count <= ushort.MaxValue)
                {
                    Clipboard.SetText(string.Join(Environment.NewLine, ConsoleWindow.ImageLinks));
                }

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(Properties.LocalizedResources.ConsoleWindow_Complete);
                Console.ReadKey();
            }
        }

        /// <summary>
        /// 格式化输入的 yande.re 页面链接。
        /// </summary>
        /// <param name="pageLink">一个 yande.re 页面链接。</param>
        /// <returns>格式化完成的 yande.re 页面链接。</returns>
        private static string FormatPageLink(string pageLink)
        {
            var indexPageUri = new Uri(YanderePage.IndexPageLink);
            var yandereHost = indexPageUri.GetComponents(UriComponents.Host, UriFormat.SafeUnescaped);

            if (pageLink.StartsWith(yandereHost))
            {
                return Uri.UriSchemeHttps + Uri.SchemeDelimiter + pageLink;
            }
            else if (pageLink.StartsWith(Uri.UriSchemeHttp + Uri.SchemeDelimiter + yandereHost))
            {
                return pageLink.Replace(Uri.UriSchemeHttp, Uri.UriSchemeHttps);
            }
            else { return pageLink; }
        }

        /// <summary>
        /// 将帮助信息输出到控制台。
        /// </summary>
        private static void ShowHelp()
        {
            Console.WriteLine();
            Console.WriteLine(Properties.LocalizedResources.ConsoleWindow_Help_Usage);
            Console.WriteLine();
            Console.WriteLine(Properties.LocalizedResources.ConsoleWindow_Help_PageLink);
            Console.WriteLine(Properties.LocalizedResources.ConsoleWindow_Help_PageCount);
            Console.WriteLine(Properties.LocalizedResources.ConsoleWindow_Help_MaxThreads);
            Console.WriteLine(Properties.LocalizedResources.ConsoleWindow_Help_OutFile);
            Console.WriteLine(Properties.LocalizedResources.ConsoleWindow_Help_Help);
            Console.WriteLine();
        }

        /// <summary>
        /// 提取页面中包含的图片链接。
        /// </summary>
        /// <param name="page">页面连接提取对象。</param>
        private static void ExtarctLinks(YanderePage page)
        {
            lock (ConsoleWindow.SyncRoot)
            {
                if (ConsoleWindow.WorkingThreads == -1)
                {
                    ConsoleWindow.WorkingThreads = 0;
                }

                ConsoleWindow.WorkingThreads++;
            }

            var imageLinks = page.ImageLinks;

            lock (ConsoleWindow.SyncRoot)
            {
                Console.WriteLine(string.Join(Environment.NewLine, imageLinks));

                if (!(ConsoleWindow.OutFile is null))
                {
                    using (var outFile = new StreamWriter(ConsoleWindow.OutFile, true))
                    {
                        outFile.WriteLine(string.Join(Environment.NewLine, imageLinks));
                    }
                }

                foreach (var imageLink in imageLinks)
                {
                    ConsoleWindow.ImageLinks.Add(imageLink);
                }

                ConsoleWindow.WorkingThreads--;
            }
        }

        /// <summary>
        /// 提取页面及其子页面中包含的图片链接。
        /// </summary>
        /// <param name="page">页面链接提取对象。</param>
        private static void ExtractPage(YanderePage page)
        {
            void callback(object state) => ConsoleWindow.ExtarctLinks(state as YanderePage);

            if (YanderePage.IsPoolsPage(page))
            {
                foreach (var poolPage in page)
                {
                    ThreadPool.QueueUserWorkItem(callback, poolPage);
                }
            }
            else
            {
                ThreadPool.QueueUserWorkItem(callback, page);
            }
        }

        /// <summary>
        /// 遍历并提取各个页面的图片链接。
        /// </summary>
        /// <param name="page">页面链接提取对象。</param>
        /// <param name="enumCount">指定遍历的页面数量；
        /// 为 -1 则遍历至最后一页，为 0 则不进行遍历。默认为 0。</param>
        private static void EnumeratePages(YanderePage page, int enumCount = -1)
        {
            enumCount = (enumCount < 0) ?
                (page.Count - page.Index + 1) :
                ((enumCount == 0) ? 1 : enumCount);
            for (int i = page.Index; i < page.Index + enumCount; i++)
            {
                ConsoleWindow.ExtractPage(page[i]);
            }
        }
    }
}
