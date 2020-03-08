using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;

namespace XstarS.YandereLinks.Views
{
    /// <summary>
    /// 表示一个带有导航控件的 <see cref="WebBrowser"/>。
    /// </summary>
    public partial class NavigationWebBrowser : UserControl
    {
        /// <summary>
        /// 初始化 <see cref="NavigationWebBrowser"/> 类。
        /// </summary>
        static NavigationWebBrowser()
        {
            NavigationWebBrowser.InitializeCommandBindings();
        }

        /// <summary>
        /// 初始化 <see cref="NavigationWebBrowser"/> 类的新实例。
        /// </summary>
        public NavigationWebBrowser()
        {
            this.InitializeComponent();
            this.InitializeComponentCommandBindings();
            this.MainWebBrowser.Navigated += this.MainWebBrowser_Navigated;
        }

        /// <summary>
        /// 初始化 <see cref="NavigationWebBrowser"/> 的命令绑定。
        /// </summary>
        private static void InitializeCommandBindings()
        {
            var commandBindings = new[]
            {
                new CommandBinding(NavigationCommands.BrowseBack,
                    (sender, e) => ((NavigationWebBrowser)sender).GoBack(),
                    (sender, e) => e.CanExecute = ((NavigationWebBrowser)sender).CanGoBack),
                new CommandBinding(NavigationCommands.BrowseForward,
                    (sender, e) => ((NavigationWebBrowser)sender).GoForward(),
                    (sender, e) => e.CanExecute = ((NavigationWebBrowser)sender).CanGoForward),
                new CommandBinding(NavigationCommands.BrowseHome,
                    (sender, e) => ((NavigationWebBrowser)sender).GoHome()),
                new CommandBinding(NavigationCommands.GoToPage,
                    (sender, e) => ((NavigationWebBrowser)sender).Navigate(e.Parameter as string)),
                new CommandBinding(NavigationCommands.Refresh,
                    (sender, e) => ((NavigationWebBrowser)sender).Refresh()),
            };

            foreach (var commandBinding in commandBindings)
            {
                CommandManager.RegisterClassCommandBinding(typeof(NavigationWebBrowser), commandBinding);
            }
        }

        /// <summary>
        /// 初始化当前 <see cref="NavigationWebBrowser"/> 所包含的组件的命令绑定。
        /// </summary>
        private void InitializeComponentCommandBindings()
        {
            this.SourceTextBox.CommandBindings.AddRange(new[]
            {
                new CommandBinding(EditingCommands.EnterParagraphBreak,
                    (sender, e) => this.ExplicitUpdateSource()),
            });
        }

        /// <summary>
        /// 表示 <see cref="NavigationWebBrowser.CanGoBack"/> 的依赖属性。
        /// </summary>
        public static readonly DependencyProperty CanGoBackProperty =
            DependencyProperty.Register(nameof(NavigationWebBrowser.CanGoBack),
                typeof(bool), typeof(NavigationWebBrowser), new PropertyMetadata(false));

        /// <summary>
        /// 后退到前一页面。
        /// </summary>
        public void GoBack() => this.MainWebBrowser.GoBack();

        /// <summary>
        /// 指示是否可以后退到前一页面。
        /// </summary>
        public bool CanGoBack
        {
            get => (bool)this.GetValue(NavigationWebBrowser.CanGoBackProperty);
            private set => this.SetValue(NavigationWebBrowser.CanGoBackProperty, value);
        }

        /// <summary>
        /// 表示 <see cref="NavigationWebBrowser.CanGoForward"/> 的依赖属性。
        /// </summary>
        public static readonly DependencyProperty CanGoForwardProperty =
            DependencyProperty.Register(nameof(NavigationWebBrowser.CanGoForward),
                typeof(bool), typeof(NavigationWebBrowser), new PropertyMetadata(false));

        /// <summary>
        /// 前进到下一页面。
        /// </summary>
        public void GoForward() => this.MainWebBrowser.GoForward();

        /// <summary>
        /// 指示是否可以前进到下一页面。
        /// </summary>
        public bool CanGoForward
        {
            get => (bool)this.GetValue(NavigationWebBrowser.CanGoForwardProperty);
            private set => this.SetValue(NavigationWebBrowser.CanGoForwardProperty, value);
        }

        /// <summary>
        /// 表示 <see cref="NavigationWebBrowser.HomeSource"/> 的依赖属性。
        /// </summary>
        public static readonly DependencyProperty HomeSourceProperty =
            DependencyProperty.Register(nameof(NavigationWebBrowser.HomeSource),
                typeof(string), typeof(NavigationWebBrowser), new PropertyMetadata(string.Empty));

        /// <summary>
        /// 获取主页的链接文本。
        /// </summary>
        public string HomeSource
        {
            get => (string)this.GetValue(NavigationWebBrowser.HomeSourceProperty);
            set => this.SetValue(NavigationWebBrowser.HomeSourceProperty, value);
        }

        /// <summary>
        /// 导航到主页。
        /// </summary>
        public void GoHome() => this.MainWebBrowser.Navigate(this.HomeSource);

        /// <summary>
        /// 表示 <see cref="NavigationWebBrowser.Source"/> 的依赖属性。
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(NavigationWebBrowser.Source),
                typeof(string), typeof(NavigationWebBrowser), new PropertyMetadata(string.Empty,
                    NavigationWebBrowser.OnSourcePropertyChanged));


        /// <summary>
        /// <see cref="NavigationWebBrowser.SourceProperty"/> 依赖属性发生更改的事件处理。
        /// </summary>
        /// <param name="d">包含依赖属性的 <see cref="NavigationWebBrowser"/>。</param>
        /// <param name="e">提供事件数据的对象。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static void OnSourcePropertyChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is NavigationWebBrowser navi) && (e.NewValue is string source))
            {
                if (navi.MainWebBrowser.Source?.OriginalString != source)
                {
                    try
                    {
                        navi.MainWebBrowser.Navigate(source);
                    }
                    catch (Exception) { }
                }
            }
        }

        /// <summary>
        /// 获取当前页面的链接文本。
        /// </summary>
        public string Source
        {
            get => (string)this.GetValue(NavigationWebBrowser.SourceProperty);
            set => this.SetValue(NavigationWebBrowser.SourceProperty, value);
        }

        /// <summary>
        /// 显式更新链接文本框的数据源。
        /// </summary>
        private void ExplicitUpdateSource()
        {
            this.SourceTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            this.MainWebBrowser.Navigate(this.Source);
            this.MainWebBrowser.Focus();
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
        /// 网页浏览器导航完成的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void MainWebBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            this.CanGoBack = this.MainWebBrowser.CanGoBack;
            this.CanGoForward = this.MainWebBrowser.CanGoForward;
            this.Source = this.MainWebBrowser.Source?.OriginalString;
            this.Dispatcher.Invoke(CommandManager.InvalidateRequerySuggested);
        }
    }
}
