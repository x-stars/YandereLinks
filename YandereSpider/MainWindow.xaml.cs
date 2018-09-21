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
using XstarS.Windows.Controls;

namespace YandereSpider
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 当前页面的链接。
        /// </summary>
        private string pageLink;
        /// <summary>
        /// yande.re 页面链接提取对象。
        /// </summary>
        private YanderePage yanderePage;
        /// <summary>
        /// 提取链接的后台线程。
        /// </summary>
        private BackgroundWorker extractLinkWorker;
        /// <summary>
        /// 遍历页面的后台线程。
        /// </summary>
        private BackgroundWorker enumeratePageWorker;

        /// <summary>
        /// 初始化 <see cref="MainWindow"/> 的新实例。
        /// </summary>
        public MainWindow()
        {
            this.pageLink = string.Empty;
            this.BindingPageLink = string.Empty;
            this.WebBrowserCanGoBack = false;
            this.WebBrowserCanGoForward = true;
            this.ImageLinks = string.Empty;

            this.extractLinkWorker = new BackgroundWorker()
            { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            this.extractLinkWorker.DoWork += this.ExtractLinkWorker_DoWork;
            this.extractLinkWorker.ProgressChanged += this.ExtractLinkWorker_ProgressChanged;
            this.extractLinkWorker.RunWorkerCompleted += this.ExtractLinkWorker_RunWorkerCompleted;

            this.enumeratePageWorker = new BackgroundWorker()
            { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            this.enumeratePageWorker.DoWork += this.EnumeratePageWorker_DoWork;
            this.enumeratePageWorker.ProgressChanged += this.EnumeratePageWorker_ProgressChanged;
            this.enumeratePageWorker.RunWorkerCompleted += this.EnumeratePageWorker_RunWorkerCompleted;

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
        public string PageLink
        {
            get => this.pageLink;

            set
            {
                this.pageLink = value;

                if (this.BindingPageLink != value)
                {
                    this.BindingPageLink.Value = value;
                }
                if (this.webBrowser?.Source?.ToString() != value)
                {
                    this.webBrowser?.Navigate(value);
                }
                if (this.yanderePage?.PageLink != value)
                {
                    this.yanderePage = new YanderePage(value);
                }
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
                int? destVersion = this.webBrowser.GetInternetExplorerVersion();
                int? currVersion = this.webBrowser.GetVersion();
                if ((currVersion is null) || (currVersion < destVersion))
                { this.webBrowser.SetVersion((int)destVersion); }
            }
            catch (SecurityException)
            {
                var appDomain = AppDomain.CurrentDomain;
                string appPath = appDomain.BaseDirectory + appDomain.FriendlyName;
                Process.Start(new ProcessStartInfo() { FileName = appPath, Verb = "RunAs" });
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.webBrowser.SuppressScriptErrors();
            this.PageLink = YanderePage.IndexPageLink;
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
                if (e.Key == Key.E) { this.ExtractButton_Click(this, new RoutedEventArgs()); }
                if (e.Key == Key.C) { this.CopyButton_Click(this, new RoutedEventArgs()); }
                if (e.Key == Key.X) { this.ClearButton_Click(this, new RoutedEventArgs()); }
            }
        }

        /// <summary>
        /// 网络浏览器导航完成的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void WebBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            this.PageLink = this.webBrowser.Source.ToString();
            this.WebBrowserCanGoBack.Value = this.webBrowser?.CanGoBack ?? false;
            this.WebBrowserCanGoForward.Value = this.webBrowser?.CanGoForward ?? false;
        }

        /// <summary>
        /// 后退按钮点击的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.webBrowser.GoBack();
        }

        /// <summary>
        /// 前进按钮点击的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void GoForwardButton_Click(object sender, RoutedEventArgs e)
        {
            this.webBrowser.GoForward();
        }

        /// <summary>
        /// 主页按钮点击的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void HomePageButton_Click(object sender, RoutedEventArgs e)
        {
            this.webBrowser.Navigate(YanderePage.IndexPageLink);
        }

        /// <summary>
        /// 转到按钮点击的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void GoToButton_Click(object sender, RoutedEventArgs e)
        {
            this.webBrowser.Navigate(this.addressTextBox.Text);
        }

        /// <summary>
        /// 地址栏文本框按键按下的事件处理。
        /// </summary>
        /// <param name="sender">触发事件的对象。</param>
        /// <param name="e">提供事件参数的对象。</param>
        private void AddressTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { this.webBrowser.Navigate(this.addressTextBox.Text); }
        }

        /// <summary>
        /// 提取链接按钮点击的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void ExtractButton_Click(object sender, RoutedEventArgs e)
        {
            const string cancelButtonContent = "取消";
            if ((this.extractButton.Content as string) != cancelButtonContent)
            {
                this.enumerateButton.IsEnabled = false;
                string extractButtonContent = this.extractButton.Content as string;
                this.extractButton.Content = cancelButtonContent;
                this.extractLinkWorker.RunWorkerAsync(extractButtonContent);
            }
            else
            {
                this.extractLinkWorker.CancelAsync();
            }
        }

        /// <summary>
        /// 遍历页面按钮点击的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void EnumerateButton_Click(object sender, RoutedEventArgs e)
        {
            const string cancelButtonContent = "取消";
            if (this.enumerateButton.Content as string != cancelButtonContent)
            {
                this.extractButton.IsEnabled = false;
                string enumerateButtonContent = this.enumerateButton.Content as string;
                this.enumerateButton.Content = cancelButtonContent;
                this.enumeratePageWorker.RunWorkerAsync(enumerateButtonContent);
            }
            else
            {
                this.enumeratePageWorker.CancelAsync();
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

            var page = this.yanderePage;
            if (!page.IsYanderePage) { return; }

            if (page.IsPoolsPage)
            {
                foreach (var poolPage in page.PoolPages)
                {
                    if (this.extractLinkWorker.CancellationPending) { return; }

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
            this.extractButton.Content = e.Result;
            this.enumerateButton.IsEnabled = true;
        }

        /// <summary>
        /// 遍历页面的后台线程运行的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void EnumeratePageWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = e.Argument;

            var firstPage = this.yanderePage;
            if (!firstPage.IsYanderePage) { return; }

            foreach (var page in firstPage)
            {
                if (this.enumeratePageWorker.CancellationPending) { return; }

                this.BindingPageLink.Value = page.PageLink;
                this.enumeratePageWorker.ReportProgress(0);

                if (page.IsPoolsPage)
                {
                    foreach (var poolPage in page.PoolPages)
                    {
                        if (this.enumeratePageWorker.CancellationPending) { return; }

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
            this.PageLink = this.BindingPageLink;
        }

        /// <summary>
        /// 遍历页面的后台线程完成的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void EnumeratePageWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.enumerateButton.Content = e.Result;
            this.extractButton.IsEnabled = true;
        }
    }
}
