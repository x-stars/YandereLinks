using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Security;
using System.Windows;
using System.Windows.Input;
using XstarS.Win32;
using XstarS.YandereLinks.ViewModels;

namespace XstarS.YandereLinks.Views
{
    /// <summary>
    /// 表示应用程序的主要窗口。
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 初始化 <see cref="MainWindow"/> 类。
        /// </summary>
        static MainWindow()
        {
            MainWindow.InitializeCommandBindings();
        }

        /// <summary>
        /// 初始化 <see cref="MainWindow"/> 类的新实例。
        /// </summary>
        public MainWindow()
        {
            this.DataContext = new MainWindowModel();
            this.InitializeComponent();
            this.Model.PropertyChanged += this.Model_PropertyChanged;
        }

        /// <summary>
        /// 当前 <see cref="MainWindow"/> 的数据逻辑模型。
        /// </summary>
        public MainWindowModel Model => (MainWindowModel)this.DataContext;

        /// <summary>
        /// 初始化 <see cref="MainWindow"/> 的命令绑定。
        /// </summary>
        private static void InitializeCommandBindings()
        {
            var commandBindings = new[]
            {
                new CommandBinding(MainWindow.ExtractImageLinksCommand,
                    (sender, e) => ((MainWindow)sender).Model.ExtractImageLinksAsync(),
                    (sender, e) => e.CanExecute = ((MainWindow)sender).Model.CanExtract),
                new CommandBinding(MainWindow.EnumeratePageExtractCommand,
                    (sender, e) => ((MainWindow)sender).Model.EnumeratePageExtractAsync(),
                    (sender, e) => e.CanExecute = ((MainWindow)sender).Model.CanExtract),
                new CommandBinding(MainWindow.CancelExtractCommand,
                    (sender, e) => ((MainWindow)sender).Model.CancelExtract(),
                    (sender, e) => e.CanExecute = ((MainWindow)sender).Model.CanCancelExtract),
                new CommandBinding(MainWindow.CopyImageLinksCommand,
                    (sender, e) => Clipboard.SetText(((MainWindow)sender).Model.ImageLinks),
                    (sender, e) => e.CanExecute = ((MainWindow)sender).Model.HasImageLinks),
                new CommandBinding(MainWindow.ClearImageLinksCommand,
                    (sender, e) => ((MainWindow)sender).Model.ImageLinks = string.Empty,
                    (sender, e) => e.CanExecute = ((MainWindow)sender).Model.HasImageLinks),
            };

            foreach (var commandBinding in commandBindings)
            {
                CommandManager.RegisterClassCommandBinding(typeof(MainWindow), commandBinding);
            }
        }

        /// <summary>
        /// 获取表示“提取图片链接”的命令的值。
        /// 默认键笔势：Ctrl+E。
        /// </summary>
        public static RoutedUICommand ExtractImageLinksCommand { get; } =
            new RoutedUICommand(
                nameof(MainWindow.ExtractImageLinksCommand),
                nameof(MainWindow.ExtractImageLinksCommand),
                typeof(MainWindow),
                new InputGestureCollection() {
                    new KeyGesture(Key.E, ModifierKeys.Control, "Ctrl+E") });

        /// <summary>
        /// 获取表示“遍历页面提取”命令的值。
        /// 默认键笔势：Ctrl+R。
        /// </summary>
        public static RoutedUICommand EnumeratePageExtractCommand { get; } =
            new RoutedUICommand(
                nameof(MainWindow.EnumeratePageExtractCommand),
                nameof(MainWindow.EnumeratePageExtractCommand),
                typeof(MainWindow),
                new InputGestureCollection() {
                    new KeyGesture(Key.R, ModifierKeys.Control, "Ctrl+R") });

        /// <summary>
        /// 获取表示“取消提取”命令的值。
        /// 默认键笔势：Ctrl+R。
        /// </summary>
        public static RoutedUICommand CancelExtractCommand { get; } =
            new RoutedUICommand(
                nameof(MainWindow.CancelExtractCommand),
                nameof(MainWindow.CancelExtractCommand),
                typeof(MainWindow),
                new InputGestureCollection() {
                    new KeyGesture(Key.D, ModifierKeys.Control, "Ctrl+D") });

        /// <summary>
        /// 获取表示“复制图片链接”命令的值。
        /// 默认键笔势：Ctrl+C。
        /// </summary>
        public static RoutedUICommand CopyImageLinksCommand { get; } =
            new RoutedUICommand(
                nameof(MainWindow.CopyImageLinksCommand),
                nameof(MainWindow.CopyImageLinksCommand),
                typeof(MainWindow),
                new InputGestureCollection() {
                    new KeyGesture(Key.C, ModifierKeys.Control, "Ctrl+C") });

        /// <summary>
        /// 获取表示“清除图片链接”命令的值。
        /// 默认键笔势：Ctrl+X。
        /// </summary>
        public static RoutedUICommand ClearImageLinksCommand { get; } =
            new RoutedUICommand(
                nameof(MainWindow.ClearImageLinksCommand),
                nameof(MainWindow.ClearImageLinksCommand),
                typeof(MainWindow),
                new InputGestureCollection() {
                    new KeyGesture(Key.X, ModifierKeys.Control, "Ctrl+X") });

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
        /// 当前窗口的数据逻辑模型的属性值更改的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(this.Model.CanExtract):
                case nameof(this.Model.CanCancelExtract):
                case nameof(this.Model.HasImageLinks):
                    this.Dispatcher.Invoke(
                        () => CommandManager.InvalidateRequerySuggested());
                    break;
                default:
                    break;
            }
        }
    }
}
