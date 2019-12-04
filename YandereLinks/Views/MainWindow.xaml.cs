using System;
using System.Diagnostics;
using System.Security;
using System.Windows;
using System.Windows.Input;
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

        /// <summary>
        /// 当前窗口按键按下的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void ThisWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) ==
                ModifierKeys.Control)
            {
                var model = this.DataContext as MainWindowModel;
                switch (e.Key)
                {
                    case Key.E: model.ExtractLinksCommand.Execute(); break;
                    case Key.R: model.EnumerateExtractLinksCommand.Execute(); break;
                    case Key.C: model.CopyLinksCommand.Execute(); break;
                    case Key.X: model.ClearLinksCommand.Execute(); break;
                    default: break;
                }
            }
        }
    }
}
