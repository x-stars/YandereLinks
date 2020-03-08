using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using XstarS.ComponentModel;
using XstarS.YandereLinks.Models;

namespace XstarS.YandereLinks.Views
{
    /// <summary>
    /// 表示主窗口的数据逻辑的模型。
    /// </summary>
    public class MainWindowModel : ObservableDataObject
    {
        /// <summary>
        /// 初始化 <see cref="MainWindowModel"/> 类的新实例。
        /// </summary>
        public MainWindowModel()
        {
            this.PageLink = this.HomePageLink;
            this.ImageLinks = string.Empty;
            this.CanExtract = true;
            this.ExtractCancellationSource = new CancellationTokenSource();
        }

        /// <summary>
        /// 主页的链接文本。
        /// </summary>
        public string HomePageLink => YanderePage.IndexPageLink;

        /// <summary>
        /// 当前页面的链接文本。
        /// </summary>
        public string PageLink
        {
            get => this.GetProperty<string>();
            set
            {
                this.SetProperty(value);
                this.PageObject = new YanderePage(value);
            }
        }

        /// <summary>
        /// 当前页面的 <see cref="YanderePage"/> 对象。
        /// </summary>
        public YanderePage PageObject { get; private set; }

        /// <summary>
        /// 指示是否可以执行提取图片链接操作。
        /// </summary>
        public bool CanExtract
        {
            get => this.GetProperty<bool>();
            private set => this.SetProperty(value);
        }

        /// <summary>
        /// 指示是否可以取消提取图片链接操作。
        /// </summary>
        public bool CanCancelExtract => !this.CanExtract;

        /// <summary>
        /// 已提取的图片链接。
        /// </summary>
        public string ImageLinks
        {
            get => this.GetProperty<string>();
            set => this.SetProperty(value);
        }

        /// <summary>
        /// 指示是否存在已提取的图片链接。
        /// </summary>
        public bool HasImageLinks => !string.IsNullOrEmpty(this.ImageLinks);

        /// <summary>
        /// 表示发送取消提取图片链接信号的信号源。
        /// </summary>
        private CancellationTokenSource ExtractCancellationSource { get; set; }

        /// <summary>
        /// 提取当前页面包含的图片链接。
        /// </summary>
        private void ExtractImageLinks()
        {
            var page = this.PageObject;
            var token = this.ExtractCancellationSource.Token;
            if (YanderePage.IsYanderePage(page) && !token.IsCancellationRequested)
            {
                foreach (var imageLink in page.ImageLinks)
                {
                    if (!this.ImageLinks.Contains(imageLink))
                    {
                        this.ImageLinks += imageLink + Environment.NewLine;
                    }
                }
            }
        }

        /// <summary>
        /// 异步提取当前页面包含的图片链接。
        /// </summary>
        public Task ExtractImageLinksAsync()
        {
            return Task.Run(() =>
            {
                if (this.CanExtract)
                {
                    this.CanExtract = false;
                    this.ExtractImageLinks();
                    this.CanExtract = true;
                }
            });
        }

        /// <summary>
        /// 异步提取当前页面到最后页面包含的图片链接。
        /// </summary>
        public Task EnumeratePageExtractAsync()
        {
            return Task.Run(() =>
            {
                if (this.CanExtract)
                {
                    this.CanExtract = false;
                    var page = this.PageObject;
                    var token = this.ExtractCancellationSource.Token;
                    if (YanderePage.IsYanderePage(page) && !token.IsCancellationRequested)
                    {
                        foreach (var nextPage in page)
                        {
                            if (token.IsCancellationRequested) { break; }
                            this.PageLink = nextPage.PageLink;
                            this.ExtractImageLinks();
                        }
                    }
                    this.CanExtract = true;
                }
            });
        }

        /// <summary>
        /// 取消提取图片链接的异步操作。
        /// </summary>
        public void CancelExtract()
        {
            if (this.CanCancelExtract)
            {
                this.ExtractCancellationSource.Cancel();
                this.ExtractCancellationSource = new CancellationTokenSource();
                this.PageObject.Refresh();
            }
        }

        /// <summary>
        /// 通知指定属性的值已更改。
        /// </summary>
        /// <param name="e">提供事件数据的对象。</param>
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            switch (e.PropertyName)
            {
                case nameof(this.CanExtract):
                    this.NotifyPropertyChanged(nameof(this.CanCancelExtract));
                    break;
                case nameof(this.ImageLinks):
                    this.NotifyPropertyChanged(nameof(this.HasImageLinks));
                    break;
                default:
                    break;
            }
        }
    }
}
