using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using XstarS;

namespace YandereSpider
{
    /// <summary>
    /// 控制台模式的交互逻辑。
    /// </summary>
    public static class ConsoleWindow
    {
        /// <summary>
        /// 当前正在工作的后台线程的数量。
        /// </summary>
        private static int WorkingThreadCount = -1;

        /// <summary>
        /// 所有图片的链接。
        /// </summary>
        private static readonly ICollection<string> ImageLinks = new HashSet<string>();

        /// <summary>
        /// 显示控制台窗口，并传递程序启动参数。
        /// </summary>
        /// <param name="args">程序启动参数。</param>
        public static void Show(string[] args)
        {
            ConsoleManager.Show();
            var param = new ParamReader(args, true, new[] { "-e", "-t" }, new[] { "-h" });

            if (param.GetSwitch("-h"))
            {
                ConsoleWindow.ShowHelp();
            }
            else
            {
                var page = new YanderePage(param.GetParam(0) ?? YanderePage.IndexPageLink);
                if (!YanderePage.IsYanderePage(page))
                {
                    throw new ArgumentException(new ArgumentException().Message, "PageLink");
                }
                
                int enumCount = int.Parse(param.GetParam("-e") ?? 0.ToString());

                if (!(param.GetParam("-t") is null))
                {
                    int maxThreads = int.Parse(param.GetParam("-t"));
                    if (maxThreads < 1) { throw new ArgumentOutOfRangeException("-t MaxThreads"); }
                    ThreadPool.SetMaxThreads(maxThreads, maxThreads);
                }

                if (enumCount == 0) { ConsoleWindow.ExtractPage(page); }
                else { ConsoleWindow.EnumeratePages(page, enumCount); }

                while (ConsoleWindow.WorkingThreadCount != 0) { Thread.Sleep(10); }
                Clipboard.SetText(string.Join(Environment.NewLine, ConsoleWindow.ImageLinks));
                Console.WriteLine();
                Console.WriteLine("Completed.");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// 隐藏命令行窗口，释放标准输入流和错误流。
        /// </summary>
        public static void Hide()
        {
            ConsoleManager.Hide();
        }

        /// <summary>
        /// 将帮助信息输出到控制台。
        /// </summary>
        private static void ShowHelp()
        {
            Console.WriteLine(@"
YandereSpider.exe PageLink [-e PageCount] [-t MaxThreads] [-h]

    PageLink        URL of yande.re page which contains image links.
                    Absolute URL only, should start with 'https://yande.re'.
    -e PageCount    Count of pages you want to enumerate from input page.
                    0 means no enumeration, -1 means enumerating to end.
    -t MaxThreads   The maximum threads in HTTP access.
    -h              Show this help message.
");
        }

        /// <summary>
        /// 提取页面中包含的图片链接。
        /// </summary>
        /// <param name="page"></param>
        private static void ExtarctLinks(YanderePage page)
        {
            if (ConsoleWindow.WorkingThreadCount == -1)
            {
                ConsoleWindow.WorkingThreadCount = 0;
            }

            ConsoleWindow.WorkingThreadCount++;
            var imageLinks = page.ImageLinks;
            Console.WriteLine(string.Join(Environment.NewLine, imageLinks));
            foreach (var imageLink in imageLinks)
            {
                ConsoleWindow.ImageLinks.Add(imageLink);
            }
            ConsoleWindow.WorkingThreadCount--;
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
