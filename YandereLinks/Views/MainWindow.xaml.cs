using System;
using System.Diagnostics;
using System.Security;
using System.Windows;
using XstarS.Win32;
using XstarS.YandereLinks.ViewModels;

namespace XstarS.YandereLinks.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑。
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 初始化 <see cref="MainWindow"/> 的新实例。
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = new MainWindowModel();
        }

        /// <summary>
        /// 当前窗口加载完成的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void ThisWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.TrySetWebBrowserVersion();
        }

        /// <summary>
        /// 尝试将内置网页浏览器的版本设定为当前系统支持的最新版本。
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void TrySetWebBrowserVersion()
        {
            try
            {
                var destVersion = SystemComponents.InternetExplorer.MajorVersion;
                var currVersion = SystemComponents.WebBrowser.MajorVersion;
                if ((currVersion is null) || (currVersion < destVersion))
                {
                    SystemComponents.WebBrowser.MajorVersionInCurrentUser = (int)destVersion;
                }
            }
            catch (Exception ex)
            when ((ex is UnauthorizedAccessException) || (ex is SecurityException))
            {
                var appPath = this.GetType().Assembly.Location;
                Process.Start(new ProcessStartInfo() { FileName = appPath, Verb = "RunAs" });
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().ToString() + Environment.NewLine + ex.Message);
            }
        }
    }
}
