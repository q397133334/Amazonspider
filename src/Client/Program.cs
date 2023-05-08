using Amazonspider.Core;
using CefSharp.WinForms;
using CefSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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

#if ANYCPU
            CefRuntime.SubscribeAnyCpuAssemblyResolver();
#endif
                // Programmatically enable DPI Aweness
                // Can also be done via app.manifest or app.config
                // https://github.com/cefsharp/CefSharp/wiki/General-Usage#high-dpi-displayssupport
                // If set via app.manifest this call will have no effect.
                //Cef.EnableHighDPISupport();

                var settings = new CefSettings()
                {
                    //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
                    CachePath = Path.Combine($"{System.IO.Path.GetFullPath("CefSharp\\Cache")}")
                };

                //Example of setting a command line argument
                //Enables WebRTC
                // - CEF Doesn't currently support permissions on a per browser basis see https://bitbucket.org/chromiumembedded/cef/issues/2582/allow-run-time-handling-of-media-access
                // - CEF Doesn't currently support displaying a UI for media access permissions
                //
                //NOTE: WebRTC Device Id's aren't persisted as they are in Chrome see https://bitbucket.org/chromiumembedded/cef/issues/2064/persist-webrtc-deviceids-across-restart
                settings.CefCommandLineArgs.Add("enable-media-stream");
                //https://peter.sh/experiments/chromium-command-line-switches/#use-fake-ui-for-media-stream
                settings.CefCommandLineArgs.Add("use-fake-ui-for-media-stream");
                //For screen sharing add (see https://bitbucket.org/chromiumembedded/cef/issues/2582/allow-run-time-handling-of-media-access#comment-58677180)
                settings.CefCommandLineArgs.Add("enable-usermedia-screen-capturing");

                //Perform dependency check to make sure all relevant resources are in our output directory.
                Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

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
