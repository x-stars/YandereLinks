using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Security;

namespace XstarS.Win32
{
    /// <summary>
    /// 提供窗口应用程序的控制台窗口管理方法。
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    internal static class ConsoleManager
    {
        /// <summary>
        /// kernel32.dll 的名称。
        /// </summary>
        private const string Kernel32DllName = "kernel32.dll";

        /// <summary>
        /// 为当前应用程序分配控制台窗口。
        /// </summary>
        /// <returns>是否成功分配控制台窗口。</returns>
        [DllImport(ConsoleManager.Kernel32DllName)]
        private static extern bool AllocConsole();

        /// <summary>
        /// 释放当前已经分配的控制台窗口。
        /// </summary>
        /// <returns>是否成功释放控制台窗口。</returns>
        [DllImport(ConsoleManager.Kernel32DllName)]
        private static extern bool FreeConsole();

        /// <summary>
        /// 获取当前已分配的控制台窗口的句柄。
        /// </summary>
        /// <returns>当前已分配的控制台窗口的句柄。
        /// 若并未分配控制台窗口，则返回 <see cref="IntPtr.Zero"/>。</returns>
        [DllImport(ConsoleManager.Kernel32DllName)]
        private static extern IntPtr GetConsoleWindow();

        /// <summary>
        /// 指示当前是否已经分配了控制台窗口。
        /// </summary>
        public static bool HasConsole =>
            ConsoleManager.GetConsoleWindow() != IntPtr.Zero;

        /// <summary>  
        /// 为当前应用程序分配控制台窗口。
        /// </summary>  
        public static void Show()
        {
            if (!ConsoleManager.HasConsole)
            {
                ConsoleManager.AllocConsole();
                ConsoleManager.InvalidateStdOutError();
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
                ConsoleManager.FreeConsole();
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
        /// 释放 <see cref="Console"/> 的标准输出流和错误流，以触发其重新初始化。
        /// </summary>
        private static void InvalidateStdOutError()
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
