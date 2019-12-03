using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using XstarS.ComponentModel;
using YandereSpider.Models;

namespace YandereSpider.ViewModels
{
    /// <summary>
    /// 表示主窗口的数据逻辑的模型。
    /// </summary>
    public class MainWindowModel : ObservableStorage
    {
        /// <summary>
        /// 初始化 <see cref="MainWindowModel"/> 类的新实例。
        /// </summary>
        public MainWindowModel()
        {
            this.ExtractCancellationSource = new CancellationTokenSource();
            this.ExtractLinksCommand = this.CreateExtractLinksCommand();
            this.EnumerateExtractLinksCommand = this.CreateEnumerateExtractLinksCommand();
            this.CancelExtractLinksCommand = this.CreateCancelExtractLinksCommand();
            this.CopyLinksCommand = this.CreateCopyLinksCommand();
            this.ClearLinksCommand = this.CreateClearLinksCommand();

            this.PageLink = YanderePage.IndexPageLink;
            this.ImageLinks = string.Empty;
            this.CanExtract = true;
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
        private CancellationTokenSource ExtractCancellationSource;

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
        /// 表示提取图片链接的命令。
        /// </summary>
        public DelegateCommand ExtractLinksCommand { get; }

        /// <summary>
        /// 创建 <see cref="MainWindowModel.ExtractLinksCommand"/>。
        /// </summary>
        /// <returns><see cref="MainWindowModel.ExtractLinksCommand"/>。</returns>
        private DelegateCommand CreateExtractLinksCommand()
        {
            return new DelegateCommand(
                () => Task.Run(() =>
                {
                    if (this.CanExtract)
                    {
                        this.CanExtract = false;
                        this.ExtractImageLinks();
                        this.CanExtract = true;
                    }
                }));
        }

        /// <summary>
        /// 表示遍历页面并提取图片链接的命令。
        /// </summary>
        public DelegateCommand EnumerateExtractLinksCommand { get; }

        /// <summary>
        /// 创建 <see cref="MainWindowModel.EnumerateExtractLinksCommand"/>。
        /// </summary>
        /// <returns><see cref="MainWindowModel.EnumerateExtractLinksCommand"/>。</returns>
        private DelegateCommand CreateEnumerateExtractLinksCommand()
        {
            return new DelegateCommand(
                () => Task.Run(() =>
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
                }));
        }

        /// <summary>
        /// 表示取消提取图片链接的命令。
        /// </summary>
        public DelegateCommand CancelExtractLinksCommand { get; }

        /// <summary>
        /// 创建 <see cref="MainWindowModel.CancelExtractLinksCommand"/>。
        /// </summary>
        /// <returns><see cref="MainWindowModel.CancelExtractLinksCommand"/>。</returns>
        private DelegateCommand CreateCancelExtractLinksCommand()
        {
            return new DelegateCommand(
                () =>
                {
                    if (!this.CanExtract)
                    {
                        this.ExtractCancellationSource.Cancel();
                        this.ExtractCancellationSource = new CancellationTokenSource();
                        this.PageObject.Refresh();
                    }
                });
        }

        /// <summary>
        /// 表示复制已提取的图片链接到剪贴板的命令。
        /// </summary>
        public DelegateCommand CopyLinksCommand { get; }

        /// <summary>
        /// 创建 <see cref="MainWindowModel.CopyLinksCommand"/>。
        /// </summary>
        /// <returns><see cref="MainWindowModel.CopyLinksCommand"/>。</returns>
        private DelegateCommand CreateCopyLinksCommand()
        {
            return new DelegateCommand(
                () =>
                {
                    if (this.HasImageLinks)
                    {
                        Clipboard.SetText(this.ImageLinks);
                    }
                });
        }

        /// <summary>
        /// 表示清除已提取的图片链接的命令。
        /// </summary>
        public DelegateCommand ClearLinksCommand { get; }

        /// <summary>
        /// 创建 <see cref="MainWindowModel.ClearLinksCommand"/>。
        /// </summary>
        /// <returns><see cref="MainWindowModel.ClearLinksCommand"/>。</returns>
        private DelegateCommand CreateClearLinksCommand()
        {
            return new DelegateCommand(
                () =>
                {
                    if (this.HasImageLinks)
                    {
                        this.ImageLinks = string.Empty;
                    }
                });
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
