using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YandereSpider
{
    /// <summary>
    /// 程序的入口点所在类。
    /// </summary>
    static class Program
    {
        /// <summary>
        /// 程序的入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
