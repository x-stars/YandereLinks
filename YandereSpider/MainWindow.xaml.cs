using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XstarS.ComponentModel;
using XstarS.Win32;
using XstarS.Windows.Controls;

namespace YandereSpider
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑。
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        /// <summary>
        /// 指示当前对象是否已经被释放。
        /// </summary>
        private volatile bool IsDisposed = false;
        /// <summary>
        /// 当前页面的链接。
        /// </summary>
        private string PageLink;
        /// <summary>
        /// yande.re 页面链接提取对象。
        /// </summary>
        private YanderePage PageObject;
        /// <summary>
        /// 提取链接的后台线程。
        /// </summary>
        private readonly BackgroundWorker ExtractLinkWorker;
        /// <summary>
        /// 遍历页面的后台线程。
        /// </summary>
        private readonly BackgroundWorker EnumeratePageWorker;

        /// <summary>
        /// 初始化 <see cref="MainWindow"/> 的新实例。
        /// </summary>
        public MainWindow()
        {
            this.PageLink = string.Empty;
            this.BindingPageLink = string.Empty;
            this.WebBrowserCanGoBack = false;
            this.WebBrowserCanGoForward = false;
            this.ImageLinks = string.Empty;

            this.ExtractLinkWorker = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            this.ExtractLinkWorker.DoWork += this.ExtractLinkWorker_DoWork;
            this.ExtractLinkWorker.ProgressChanged += this.ExtractLinkWorker_ProgressChanged;
            this.ExtractLinkWorker.RunWorkerCompleted += this.ExtractLinkWorker_RunWorkerCompleted;

            this.EnumeratePageWorker = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            this.EnumeratePageWorker.DoWork += this.EnumeratePageWorker_DoWork;
            this.EnumeratePageWorker.ProgressChanged += this.EnumeratePageWorker_ProgressChanged;
            this.EnumeratePageWorker.RunWorkerCompleted += this.EnumeratePageWorker_RunWorkerCompleted;

            this.InitializeComponent();
        }

        /// <summary>
        /// 当前页面的链接。绑定到用户控件。
        /// </summary>
        public Bindable<string> BindingPageLink { get; }

        /// <summary>
        /// 指示网络浏览器是否支持后退操作。
        /// </summary>
        public Bindable<bool> WebBrowserCanGoBack { get; }

        /// <summary>
        /// 指示网络浏览器是否支持前进操作。
        /// </summary>
        public Bindable<bool> WebBrowserCanGoForward { get; }

        /// <summary>
        /// 所有图片的链接。
        /// </summary>
        public Bindable<string> ImageLinks { get; }

        /// <summary>
        /// 当前页面的链接。
        /// 更改此值会同步更改各个页面链接相关对象。
        /// </summary>
        public string UniversalPageLink
        {
            get => this.PageLink;
            set
            {
                this.PageLink = value;

                if (this.BindingPageLink != value)
                {
                    this.BindingPageLink.Value = value;
                }
                if (this.WebBrowser?.Source?.ToString() != value)
                {
                    this.WebBrowser?.Navigate(value);
                }
                if (this.PageObject?.PageLink != value)
                {
                    this.PageObject = new YanderePage(value);
                }
            }
        }

        /// <summary>
        /// 释放此实例占用的资源。
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放当前实例占用的非托管资源，并根据指示释放托管资源。
        /// </summary>
        /// <param name="disposing">指示是否释放托管资源。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    this.PageObject?.Dispose();
                    this.PageObject = null;
                    this.ExtractLinkWorker?.Dispose();
                    this.EnumeratePageWorker?.Dispose();
                }

                this.IsDisposed = true;
            }
        }

        /// <summary>
        /// 主窗口加载完成的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                int? destVersion = SystemComponents.InternetExplorer.MajorVersion;
                int? currVersion = SystemComponents.WebBrowser.MajorVersion;

                if ((currVersion is null) || (currVersion < destVersion))
                {
                    SystemComponents.WebBrowser.MajorVersionInCurrentUser = (int)destVersion;
                }
            }
            catch (Exception ex)
            when ((ex is UnauthorizedAccessException) || (ex is SecurityException))
            {
                var appDomain = AppDomain.CurrentDomain;
                string appPath = appDomain.BaseDirectory + appDomain.FriendlyName;
                Process.Start(new ProcessStartInfo() { FileName = appPath, Verb = "RunAs" });
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().ToString() + Environment.NewLine + ex.Message);
            }

            this.WebBrowser.SuppressScriptErrors();
            this.UniversalPageLink = YanderePage.IndexPageLink;
        }

        /// <summary>
        /// 主窗口按键按下的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) ==
                ModifierKeys.Control)
            {
                switch (e.Key)
                {
                    case Key.E: this.ExtractButton_Click(this, new RoutedEventArgs()); break;
                    case Key.R: this.EnumerateButton_Click(this, new RoutedEventArgs()); break;
                    case Key.C: this.CopyButton_Click(this, new RoutedEventArgs()); break;
                    case Key.X: this.ClearButton_Click(this, new RoutedEventArgs()); break;
                    default: break;
                }
            }
        }

        /// <summary>
        /// 网络浏览器导航完成的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void WebBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            this.UniversalPageLink = this.WebBrowser.Source.ToString();
            this.WebBrowserCanGoBack.Value = this.WebBrowser?.CanGoBack ?? false;
            this.WebBrowserCanGoForward.Value = this.WebBrowser?.CanGoForward ?? false;
        }

        /// <summary>
        /// 后退按钮点击的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.WebBrowser.GoBack();
        }

        /// <summary>
        /// 前进按钮点击的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void GoForwardButton_Click(object sender, RoutedEventArgs e)
        {
            this.WebBrowser.GoForward();
        }

        /// <summary>
        /// 主页按钮点击的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void HomePageButton_Click(object sender, RoutedEventArgs e)
        {
            this.WebBrowser.Navigate(YanderePage.IndexPageLink);
        }

        /// <summary>
        /// 转到按钮点击的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void GoToButton_Click(object sender, RoutedEventArgs e)
        {
            this.WebBrowser.Navigate(this.AddressTextBox.Text);
        }

        /// <summary>
        /// 地址栏文本框按键按下的事件处理。
        /// </summary>
        /// <param name="sender">触发事件的对象。</param>
        /// <param name="e">提供事件参数的对象。</param>
        private void AddressTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { this.WebBrowser.Navigate(this.AddressTextBox.Text); }
        }

        /// <summary>
        /// 提取链接按钮点击的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void ExtractButton_Click(object sender, RoutedEventArgs e)
        {
            string cancelButtonContent = Properties.LocalizedResources.MainWindow_CancelButton;
            if ((this.ExtractButton.Content as string) != cancelButtonContent)
            {
                this.EnumerateButton.IsEnabled = false;
                string extractButtonContent = this.ExtractButton.Content as string;
                this.ExtractButton.Content = cancelButtonContent;
                this.ExtractLinkWorker.RunWorkerAsync(extractButtonContent);
            }
            else
            {
                this.ExtractLinkWorker.CancelAsync();
                this.PageObject.Dispose();
                this.PageObject = new YanderePage(this.UniversalPageLink);
            }
        }

        /// <summary>
        /// 遍历页面按钮点击的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void EnumerateButton_Click(object sender, RoutedEventArgs e)
        {
            string cancelButtonContent = Properties.LocalizedResources.MainWindow_CancelButton;
            if (this.EnumerateButton.Content as string != cancelButtonContent)
            {
                this.ExtractButton.IsEnabled = false;
                string enumerateButtonContent = this.EnumerateButton.Content as string;
                this.EnumerateButton.Content = cancelButtonContent;
                this.EnumeratePageWorker.RunWorkerAsync(enumerateButtonContent);
            }
            else
            {
                this.EnumeratePageWorker.CancelAsync();
                this.PageObject.Dispose();
                this.PageObject = new YanderePage(this.UniversalPageLink);
            }
        }

        /// <summary>
        /// 复制链接按钮点击的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ImageLinks.Value.Length != 0) { Clipboard.SetText(this.ImageLinks); }
        }

        /// <summary>
        /// 清除链接按钮点击的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.ImageLinks.Value = string.Empty;
        }

        /// <summary>
        /// 提取链接的后台线程运行的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void ExtractLinkWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = e.Argument;

            var page = this.PageObject;
            if (!YanderePage.IsYanderePage(page)) { return; }

            if (YanderePage.IsPoolsPage(page))
            {
                foreach (var poolPage in page.PoolPages)
                {
                    if (this.ExtractLinkWorker.CancellationPending) { return; }

                    this.PageObject = poolPage;

                    foreach (var imageLink in poolPage.ImageLinks)
                    {
                        if (!this.ImageLinks.Value.Contains(imageLink))
                        {
                            this.ImageLinks.Value += imageLink + Environment.NewLine;
                        }
                    }
                }
            }
            else
            {
                foreach (var imageLink in page.ImageLinks)
                {
                    if (!this.ImageLinks.Value.Contains(imageLink))
                    {
                        this.ImageLinks.Value += imageLink + Environment.NewLine;
                    }
                }
            }
        }

        /// <summary>
        /// 提取链接的后台线程报告进度的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void ExtractLinkWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        /// <summary>
        /// 提取链接的后台线程完成的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void ExtractLinkWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ExtractButton.Content = e.Result;
            this.EnumerateButton.IsEnabled = true;
        }

        /// <summary>
        /// 遍历页面的后台线程运行的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void EnumeratePageWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = e.Argument;

            var firstPage = this.PageObject;
            if (!YanderePage.IsYanderePage(firstPage)) { return; }

            foreach (var page in firstPage)
            {
                if (this.EnumeratePageWorker.CancellationPending) { return; }

                this.BindingPageLink.Value = page.PageLink;
                this.EnumeratePageWorker.ReportProgress(0);
                this.PageObject = page;

                if (YanderePage.IsPoolsPage(page))
                {
                    foreach (var poolPage in page.PoolPages)
                    {
                        if (this.EnumeratePageWorker.CancellationPending) { return; }

                        this.PageObject = poolPage;

                        foreach (var imageLink in poolPage.ImageLinks)
                        {
                            if (!this.ImageLinks.Value.Contains(imageLink))
                            {
                                this.ImageLinks.Value += imageLink + Environment.NewLine;
                            }
                        }
                    }
                }
                else
                {
                    foreach (var imageLink in page.ImageLinks)
                    {
                        if (!this.ImageLinks.Value.Contains(imageLink))
                        {
                            this.ImageLinks.Value += imageLink + Environment.NewLine;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 遍历页面的后台线程报告进度的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void EnumeratePageWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.UniversalPageLink = this.BindingPageLink;
        }

        /// <summary>
        /// 遍历页面的后台线程完成的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void EnumeratePageWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.EnumerateButton.Content = e.Result;
            this.ExtractButton.IsEnabled = true;
        }
   }
}
