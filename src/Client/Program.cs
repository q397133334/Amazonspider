using Amazonspider.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Amazonspider.Client
{
    static class Program
    {

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            try
            {
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += Application_ThreadException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                Application.ApplicationExit += Application_ApplicationExit;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Main());
            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger("testApp.Logging");//获取一个日志记录器 
                log.Error("系统全局异常--asd", ex);
            }
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            //Bootstrap.Dispose();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("testApp.Logging");//获取一个日志记录器 
            log.Error("系统全局异常--CurrentDomain_UnhandledException", e.ExceptionObject as Exception);
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("testApp.Logging");//获取一个日志记录器 
            log.Error("系统全局异常--Application_ThreadException", e.Exception);
        }
    }


}
