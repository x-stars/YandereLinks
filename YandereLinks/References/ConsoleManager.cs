using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

namespace XstarS.Win32
{
    /// <summary>
    /// 提供窗口应用程序的控制台窗口管理方法。
    /// </summary>
    public static class ConsoleManager
    {
        /// <summary>
        /// 提供控制台相关的原生方法。
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
        private static class SafeNativeMethods
        {
            /// <summary>
            /// kernel32.dll 的名称。
            /// </summary>
            private const string Kernel32DllName = "kernel32.dll";

            /// <summary>
            /// 为当前应用程序分配控制台窗口。
            /// </summary>
            /// <returns>是否成功分配控制台窗口。</returns>
            [DllImport(Kernel32DllName)]
            internal static extern bool AllocConsole();

            /// <summary>
            /// 释放当前已经分配的控制台窗口。
            /// </summary>
            /// <returns>是否成功释放控制台窗口。</returns>
            [DllImport(Kernel32DllName)]
            internal static extern bool FreeConsole();

            /// <summary>
            /// 获取当前已分配的控制台窗口的句柄。
            /// </summary>
            /// <returns>当前已分配的控制台窗口的句柄。
            /// 若并未分配控制台窗口，则返回 <see cref="IntPtr.Zero"/>。</returns>
            [DllImport(Kernel32DllName)]
            internal static extern IntPtr GetConsoleWindow();
        }

        /// <summary>
        /// 指示当前是否已经分配了控制台窗口。
        /// </summary>
        public static bool HasConsole =>
            ConsoleManager.SafeNativeMethods.GetConsoleWindow() != IntPtr.Zero;

        /// <summary>  
        /// 为当前应用程序分配控制台窗口。
        /// </summary>  
        public static void Show()
        {
            if (!ConsoleManager.HasConsole)
            {
                ConsoleManager.SafeNativeMethods.AllocConsole();
                ConsoleManager.InitializeStdOutError();
            }
        }

        /// <summary>  
        /// 释放当前已经分配的控制台窗口。
        /// </summary>  
        public static void Hide()
        {
            if (ConsoleManager.HasConsole)
            {
                ConsoleManager.SetStdOutErrorNull();
                ConsoleManager.SafeNativeMethods.FreeConsole();
            }
        }

        /// <summary>
        /// 切换控制台窗口的显示状态。
        /// </summary>
        public static void Toggle()
        {
            if (ConsoleManager.HasConsole)
            {
                ConsoleManager.Hide();
            }
            else
            {
                ConsoleManager.Show();
            }
        }

        /// <summary>
        /// 重新初始化 <see cref="Console"/> 的标准输出流和错误流。
        /// </summary>
        private static void InitializeStdOutError()
        {
            var t_Console = typeof(Console);
            var staticNoPublic = BindingFlags.Static | BindingFlags.NonPublic;

            var sf__out = t_Console.GetField("_out", staticNoPublic);
            var sf__error = t_Console.GetField("_error", staticNoPublic);
            var sm_InitializeStdOutError =
                t_Console.GetMethod("InitializeStdOutError", staticNoPublic);

            sf__out.SetValue(null, null);
            sf__error.SetValue(null, null);
            sm_InitializeStdOutError.Invoke(null, new object[] { true });
        }

        /// <summary>
        /// 将 <see cref="Console"/> 的标准输出流和错误流重定向至 <see cref="TextWriter.Null"/>。
        /// </summary>
        private static void SetStdOutErrorNull()
        {
            Console.SetOut(TextWriter.Null);
            Console.SetError(TextWriter.Null);
        }
    }
}
