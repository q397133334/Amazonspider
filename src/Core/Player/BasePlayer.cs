using Amazonspider.Core.EntityFramework;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Amazonspider.Core.Player
{
    public abstract class BasePlayer : IPlayer, IDisposable
    {


        public int Index { get; set;}
        /// <summary>
        /// 超时时间
        /// </summary>
        public long Timeout { get; set; } = 30000;


        public TaskSchedule TaskSchedule { get; set; }

        /// <summary>
        /// 完成回调方法
        /// </summary>
        public Action<Product, ProductImage, TaskSchedule, IPlayer> Complete { get; set; }
        public bool IsUse { get; set; }
        public string RunMsg { get; set; }
        public IConsoleLog ConsoleLog { get; set; }
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// 是否需要修改账号信息
        /// </summary>
        public bool IsAccountModify { get; set; } = true;

        /// <summary>
        /// 字库文件
        /// </summary>
        public List<string> FontFile { get; set; } = new List<string>();

        /// <summary>
        /// 乐玩工作目录
        /// </summary>
        public string Path { get; set; }


        /// <summary>
        /// 脚本执行线程
        /// </summary>
        protected Thread thread;

        public void Run()
        {
            thread = new Thread(ThreadStart);
            thread.Start();
        }

        protected void ThreadStart()
        {
            try
            {

                WriteLog("开始执行脚本");
                ThreadRun();
            }
            catch (ThreadAbortException ex)
            {
                ThreadAbout();
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message, ConsoleLogStatus.Exption);
                ThreadException();
                log4net.ILog log = log4net.LogManager.GetLogger("testApp.Logging");//获取一个日志记录器 
                log.Error("任务执行异常--", ex);
            }
            finally
            {

            }
        }

        /// <summary>
        /// 线程运行方法
        /// </summary>
        protected abstract void ThreadRun();

        /// <summary>
        /// 结束线程后处理的事情
        /// </summary>
        protected virtual void ThreadAbout()
        {

        }
        /// <summary>
        /// 线程异常处理的事情
        /// </summary>
        protected virtual void ThreadException()
        {

        }

        //释放资源处理的事情
        public virtual void Dispose()
        {
            while (thread.ThreadState != ThreadState.Aborted && thread.ThreadState != ThreadState.Stopped)
            {
                thread.Abort();
            }

            //释放输出日志
            ConsoleLog = null;
            //释放完成时间
            Complete = null;
            //释放账号信息
            //PlayerAccount = null;
            //释放任务信息
            TaskSchedule = null;
            //释放配置信息
            Configuration = null;
        }

        /// <summary>
        /// 写入控制台信息（封装一下IConsoleLog）
        /// </summary>
        /// <param name="msg">执行消息</param>
        /// <param name="consoleLogStatus">消息状态</param>
        protected void WriteLog(string msg, ConsoleLogStatus consoleLogStatus = ConsoleLogStatus.Nomral)
        {
            RunMsg = msg;
            ConsoleLog?.WriteLog?.Invoke($"{Index}号采集进程--{msg}", consoleLogStatus);
        }



    }
}
