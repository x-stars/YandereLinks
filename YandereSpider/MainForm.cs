using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace YandereSpider
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// 当前已经加载的 HTML。
        /// </summary>
        private string webBrowserHtmlForCheck;

        /// <summary>
        /// 初始化 <see cref="MainForm"/> 的新实例。
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();
            // 若干按钮初始无效。
            this.extractButton.Enabled = false;
            this.goThroughButton.Enabled = false;
            this.cancelButton.Enabled = false;
            this.backButton.Enabled = false;
            this.forwardButton.Enabled = false;
        }

        /// <summary>
        /// 提取链接。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExtractButton_Click(object sender, EventArgs e)
        {
            // 若为Pool列表，则弹窗警告。
            string pageUrl = this.webBrowser.Url.ToString();
            if (pageUrl.Contains("pool") && !pageUrl.Contains("show"))
            {
                MessageBox.Show(this, "Pool列表页面将仅能提取封面。", "警告");
            }

            // 读取 HTML 并传递至 GetImageLinks()。
            string pageHtml = this.webBrowser.DocumentText;
            if (pageHtml != string.Empty)
            {
                string imageLinks = GetImageLinks(pageHtml);
                // 不重复则添加至图片链接文本框。
                if (!this.imageLinkTextBox.Text.Contains(imageLinks))
                {
                    this.imageLinkTextBox.Text += imageLinks;
                }
            }
        }

        /// <summary>
        /// 复制到剪贴板。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyButton_Click(object sender, EventArgs e)
        {
            if (this.imageLinkTextBox.Text != string.Empty)
            {
                Clipboard.SetText(this.imageLinkTextBox.Text);
            }
        }

        /// <summary>
        /// 清除链接文本框。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearButton_Click(object sender, EventArgs e)
        {
            this.imageLinkTextBox.Clear();
        }

        /// <summary>
        /// 遍历按钮按下，提取当前页面的链接，剩余页面由后台线程完成。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoThroughButton_Click(object sender, EventArgs e)
        {
            // 控制相关按钮的可用性。
            this.extractButton.Enabled = false;
            this.goThroughButton.Enabled = false;
            this.cancelButton.Enabled = true;

            // 若为 Pool 列表，则弹窗警告。
            string pageUrl = this.webBrowser.Url.ToString();
            if (pageUrl.Contains("pool") && !pageUrl.Contains("show"))
            {
                MessageBox.Show(this, "Pool列表页面将仅能提取封面。", "警告");
            }

            // 启动提取单个页面的过程，在完成后，会由后台线程完成所有页面的遍历。
            string pageHtml = this.webBrowser.DocumentText;
            this.GoThroughSinglePage(pageHtml);
        }

        /// <summary>
        /// 取消遍历操作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            // 反激活取消按钮。
            this.cancelButton.Enabled = false;
            // 取消检测网页加载的后台线程。
            this.webBrowserDocumentTextCompleteCheckBackgroundWorker.CancelAsync();
        }

        /// <summary>
        /// 网页 URL 变化的事件处理。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            // 处理导航相关控件。
            if (this.webBrowser.CanGoBack)
            {
                this.backButton.Enabled = true;
            }
            else
            {
                this.backButton.Enabled = false;
            }
            if (this.webBrowser.CanGoForward)
            {
                this.forwardButton.Enabled = true;
            }
            else
            {
                this.forwardButton.Enabled = false;
            }
            this.addressTextBox.Text = e.Url.ToString();

            // 不允许提取链接和遍历操作，直至网页加载完成。
            this.goThroughButton.Enabled = false;
            this.extractButton.Enabled = false;

            // 启动检查网页加载完成的后台线程。
            this.webBrowserDocumentTextCompleteCheckBackgroundWorker.CancelAsync();
            Thread.Sleep(10);
            try
            {
                this.webBrowserDocumentTextCompleteCheckBackgroundWorker.RunWorkerAsync();
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// 开始循环检测网页 HTML 是否加载完成。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebBrowserDocumentTextCompleteCheckBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // 清空检测用 HTML 再启动检测过程。
            this.webBrowserHtmlForCheck = string.Empty;
            this.WebBrowserDocumentTextCompleteCheck();
        }

        /// <summary>
        /// 循环检测网页 HTML 是否加载完成。
        /// </summary>
        private void WebBrowserDocumentTextCompleteCheck()
        {
            // 循环检测 HTML 是否加载完成。
            while (!this.webBrowserHtmlForCheck.Contains("</html>") &&
                !this.webBrowserDocumentTextCompleteCheckBackgroundWorker.CancellationPending)
            {
                // 请求一次数据传递。
                this.webBrowserDocumentTextCompleteCheckBackgroundWorker.ReportProgress(0);
                Thread.Sleep(10);
            }
            this.webBrowserDocumentTextCompleteCheckBackgroundWorker.ReportProgress(100);
        }

        /// <summary>
        /// 传递网页 HTML 数据至 <see cref="MainForm.webBrowserHtmlForCheck"/>。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebBrowserDocumentTextCompleteCheckBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.webBrowserHtmlForCheck = this.webBrowser.DocumentText;
        }

        /// <summary>
        /// 网页 HTML 加载完成。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebBrowserDocumentTextCompleteCheckBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // 判断是否为线程取消。
            if (!e.Cancelled)
            {
                // 清空检测用 HTML。
                this.webBrowserHtmlForCheck = string.Empty;

                // 判断是否处在遍历状态。
                if (this.cancelButton.Enabled == true)
                {
                    // 完成单个页面的提取。
                    string pageHtml = this.webBrowser.DocumentText;
                    if (!this.GoThroughSinglePage(pageHtml))
                    {
                        // 若无下一页，允许提取和遍历操作。
                        this.extractButton.Enabled = true;
                        this.goThroughButton.Enabled = true;
                    }
                }
                else
                {
                    // 允许提取和遍历操作。
                    this.extractButton.Enabled = true;
                    this.goThroughButton.Enabled = true;
                }
            }
            else
            {
                // 允许提取和遍历操作。
                this.extractButton.Enabled = true;
                this.goThroughButton.Enabled = true;
            }
        }

        /// <summary>
        /// 遍历页面的单个页面并跳转至下一页的过程。
        /// </summary>
        /// <param name="pageHtml"></param>
        /// <returns>是否有下一页。</returns>
        private bool GoThroughSinglePage(string pageHtml)
        {
            if (pageHtml != string.Empty)
            {
                // 调用 GetImageLinks()。
                string imageLinks = this.GetImageLinks(pageHtml);
                // 不重复则添加至图片链接文本框。
                if (!this.imageLinkTextBox.Text.Contains(imageLinks))
                {
                    this.imageLinkTextBox.Text += imageLinks;
                }

                // 没有下一页则退出。
                if (!pageHtml.Contains("<a class=\"next_page\""))
                {
                    return false;
                }
                // 否则获取下一页的 URL 并重定向至下一页。
                else
                {
                    int nextPageIndex = pageHtml.IndexOf("<a class=\"next_page\"") + 38;
                    string nextPageUrl = pageHtml.Substring(nextPageIndex);
                    nextPageUrl = nextPageUrl.Remove(nextPageUrl.IndexOf('"'));
                    nextPageUrl = "https://yande.re" + nextPageUrl;
                    this.webBrowser.Navigate(nextPageUrl);
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取单个页面中所有图片链接。
        /// </summary>
        /// <param name="pageHtml">yande.re 网页 HTML。</param>
        /// <returns></returns>
        private string GetImageLinks(string pageHtml)
        {
            string fileUrls = string.Empty;
            while (pageHtml.Contains("file_url"))
            {
                int fileUrlIndex = pageHtml.IndexOf("file_url") + 11;
                string fileUrl = pageHtml.Substring(fileUrlIndex);
                fileUrl = fileUrl.Remove(fileUrl.IndexOf('"'));
                fileUrls += fileUrl + Environment.NewLine;
                pageHtml = pageHtml.Substring(fileUrlIndex);
            }
            return fileUrls;
        }

        /// <summary>
        /// 主页按钮按下。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HomeButton_Click(object sender, EventArgs e)
        {
            this.webBrowser.Navigate("https://yande.re");
        }

        /// <summary>
        /// 刷新（转到）按钮按下。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            this.webBrowser.Navigate(this.addressTextBox.Text);
        }

        /// <summary>
        /// 后退按钮按下。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, EventArgs e)
        {
            this.webBrowser.GoBack();
        }

        /// <summary>
        /// 前进按钮按下。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ForwardButton_Click(object sender, EventArgs e)
        {
            this.webBrowser.GoForward();
        }

        /// <summary>
        /// 地址栏键盘按下。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddressTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.webBrowser.Navigate(this.addressTextBox.Text);
            }
        }
    }
}
