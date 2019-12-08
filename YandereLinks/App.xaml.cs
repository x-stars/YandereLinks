using System.Windows;
using XstarS.YandereLinks.Views;

namespace XstarS.YandereLinks
{
    /// <summary>
    /// 表示应用程序的入口点。
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 应用程序启动的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length == 0)
            {
                new MainWindow().Show();
            }
            else
            {
                ConsoleWindow.Show(e.Args);
            }
        }
    }
}
