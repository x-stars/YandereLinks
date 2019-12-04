using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using XstarS.Windows.Controls;

namespace XstarS.YandereLinks.Views
{
    /// <summary>
    /// 表示一个带有导航控件的 <see cref="WebBrowser"/>。
    /// </summary>
    public partial class NaviWebBrowser : UserControl
    {
        /// <summary>
        /// 初始化 <see cref="NaviWebBrowser"/> 类的新实例。
        /// </summary>
        public NaviWebBrowser()
        {
            this.InitializeComponent();
            this.CommandBindings.AddRange(new[]
            {
                new CommandBinding(NavigationCommands.BrowseBack, (s, e) => this.GoBack()),
                new CommandBinding(NavigationCommands.BrowseForward, (s, e) => this.GoForward()),
                new CommandBinding(NavigationCommands.BrowseHome, (s, e) => this.GoHome()),
                new CommandBinding(NavigationCommands.GoToPage, (s, e) => this.Navigate(e.Parameter as string)),
                new CommandBinding(NavigationCommands.Refresh, (s, e) => this.Refresh()),
            });
            this.MainWebBrowser.Navigated += this.MainWebBrowser_Navigated;
            this.MainWebBrowser.SuppressScriptErrors(true);
        }

        /// <summary>
        /// <see cref="NaviWebBrowser.CanGoBack"/> 的依赖属性。
        /// </summary>
        public static readonly DependencyProperty CanGoBackProperty =
            DependencyProperty.Register(nameof(NaviWebBrowser.CanGoBack),
                typeof(bool), typeof(NaviWebBrowser), new PropertyMetadata(false));

        /// <summary>
        /// 指示是否可以后退到前一页面。
        /// </summary>
        public bool CanGoBack
        {
            get => (bool)this.GetValue(NaviWebBrowser.CanGoBackProperty);
            private set => this.SetValue(NaviWebBrowser.CanGoBackProperty, value);
        }

        /// <summary>
        /// 后退到前一页面。
        /// </summary>
        public void GoBack() => this.MainWebBrowser.GoBack();

        /// <summary>
        /// <see cref="NaviWebBrowser.CanGoForward"/> 的依赖属性。
        /// </summary>
        public static readonly DependencyProperty CanGoForwardProperty =
            DependencyProperty.Register(nameof(NaviWebBrowser.CanGoForward),
                typeof(bool), typeof(NaviWebBrowser), new PropertyMetadata(false));

        /// <summary>
        /// 指示是否可以前进到下一页面。
        /// </summary>
        public bool CanGoForward
        {
            get => (bool)this.GetValue(NaviWebBrowser.CanGoForwardProperty);
            private set => this.SetValue(NaviWebBrowser.CanGoForwardProperty, value);
        }

        /// <summary>
        /// 前进到下一页面。
        /// </summary>
        public void GoForward() => this.MainWebBrowser.GoForward();

        /// <summary>
        /// <see cref="NaviWebBrowser.HomeSource"/> 的依赖属性。
        /// </summary>
        public static readonly DependencyProperty HomeSourceProperty =
            DependencyProperty.Register(nameof(NaviWebBrowser.HomeSource),
                typeof(string), typeof(NaviWebBrowser), new PropertyMetadata(""));

        /// <summary>
        /// 获取主页的链接文本。
        /// </summary>
        public string HomeSource
        {
            get => (string)this.GetValue(NaviWebBrowser.HomeSourceProperty);
            set => this.SetValue(NaviWebBrowser.HomeSourceProperty, value);
        }

        /// <summary>
        /// 导航到主页。
        /// </summary>
        public void GoHome() => this.MainWebBrowser.Navigate(this.HomeSource);

        /// <summary>
        /// <see cref="NaviWebBrowser.Source"/> 的依赖属性。
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(NaviWebBrowser.Source),
                typeof(string), typeof(NaviWebBrowser), new PropertyMetadata(""));

        /// <summary>
        /// 获取当前页面的链接文本。
        /// </summary>
        public string Source
        {
            get => (string)this.GetValue(NaviWebBrowser.SourceProperty);
            set => this.SetValue(NaviWebBrowser.SourceProperty, value);
        }

        /// <summary>
        /// 导航到指定链接的页面。
        /// </summary>
        public void Navigate(string source) => this.MainWebBrowser.Navigate(source);

        /// <summary>
        /// 刷新当前页面。
        /// </summary>
        public void Refresh() => this.MainWebBrowser.Refresh();

        /// <summary>
        /// 通知指定依赖属性的值已更改。
        /// </summary>
        /// <param name="e">描述已更改的属性的事件数据。</param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == NaviWebBrowser.SourceProperty)
            {
                var source = e.NewValue as string;
                if (this.MainWebBrowser.Source?.OriginalString != source)
                {
                    this.MainWebBrowser.Navigate(source);
                }
            }
        }

        /// <summary>
        /// 当导航开始时发生。
        /// </summary>
        public event NavigatingCancelEventHandler Navigating
        {
            add => this.MainWebBrowser.Navigating += value;
            remove => this.MainWebBrowser.Navigating -= value;
        }

        /// <summary>
        /// 当导航完成时发生。
        /// </summary>
        public event NavigatedEventHandler Navigated
        {
            add => this.MainWebBrowser.Navigated += value;
            remove => this.MainWebBrowser.Navigated -= value;
        }

        /// <summary>
        /// 当文档加载完成时发生。
        /// </summary>
        public event LoadCompletedEventHandler LoadCompleted
        {
            add => this.MainWebBrowser.LoadCompleted += value;
            remove => this.MainWebBrowser.LoadCompleted -= value;
        }

        /// <summary>
        /// 显式更新链接文本框的数据源。
        /// </summary>
        private void UpdateSourceTextBoxSource()
        {
            this.SourceTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            this.MainWebBrowser.Navigate(this.Source);
            this.MainWebBrowser.Focus();
        }

        /// <summary>
        /// 网页浏览器导航完成的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void MainWebBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            this.CanGoBack = this.MainWebBrowser.CanGoBack;
            this.CanGoForward = this.MainWebBrowser.CanGoForward;
            this.Source = this.MainWebBrowser.Source?.OriginalString;
        }

        /// <summary>
        /// 链接文本框按键按下的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void SourceTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter: this.UpdateSourceTextBoxSource(); break;
                default: break;
            }
        }

        /// <summary>
        /// 转到按钮点击的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void GoToButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateSourceTextBoxSource();
        }
    }
}
