using Amazonspider.Core.EntityFramework;
using Amazonspider.Core.Player;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Amazonspider.Core
{
    public class RunInfo : IDisposable
    {
        #region 界面绑定展示信息
        //public string account => tasks == null ? "" : PlayerAccount.Account;
        public string runtime => _stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
        public string pluginname => PlugInSource == null ? "" : PlugInSource.PlugInName;
        public string simulatorname => $"{Index}号采集进程";
        public string runmsg => player == null ? "" : player.RunMsg;

        #endregion

        /// <summary>
        /// 定时信息
        /// </summary>
        public Stopwatch _stopwatch { get; set; }

        /// <summary>
        /// 任务信息
        /// </summary>
        public TaskSchedule TaskSchedule { get; set; }

        /// <summary>
        /// 游戏者（脚本）
        /// </summary>
        public IPlayer player { get; set; }

        /// <summary>
        /// 脚本插件信息
        /// </summary>
        public PlugIns.IPlugInSource PlugInSource { get; set; }

        /// <summary>
        /// 模拟器
        /// </summary>
        public int Index { get; set; }

        public RunInfo()
        {
            _stopwatch = new Stopwatch();
        }

        public void Init()
        {
            _stopwatch.Start();
            player.TaskSchedule = TaskSchedule;
            player.Index = Index;
            player.Run();
        }
        CancellationTokenSource cancellationTokenSource { get; set; }
        CancellationToken cancellationToken { get; set; }

        /// <summary>
        /// 判断是否超时
        /// </summary>
        /// <returns></returns>
        public bool CheckTimeout()
        {
            if (player == null)
                return false;
            return _stopwatch.ElapsedMilliseconds <= player.Timeout ? false : true;
        }

        //运行状态
        public int State = 0;

        public void Dispose()
        {
            //释放中，防止重复释放
            State = 1;
            //重置计时器
            _stopwatch.Reset();
            Task.Run(() =>
            {
                //cancellationTokenSource.Cancel();
                //释放玩家
                player?.Dispose();
                //释放玩家
                player = null;
                //清空账号和任务信息
                //PlayerAccount = null;
                TaskSchedule = null;
                //释放完毕，恢复状态
                State = 0;

            });

        }
    }
}
