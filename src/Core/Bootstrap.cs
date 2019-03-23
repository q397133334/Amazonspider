using Amazonspider.Core.EntityFramework;
using Amazonspider.Core.Player;
using Amazonspider.Core.PlugIns;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Amazonspider.Core
{
    public class Bootstrap : IDisposable, IConsoleLog
    {

        /// <summary>
        /// 任务定时
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// 插件管理
        /// </summary>
        public PlugInManager PlugInManager { private get; set; }

        /// <summary>
        /// 最大玩家数量
        /// </summary>
        public int MaxPlayerCount
        {
            get { return _maxPlayerCount; }
            set
            {
                _maxPlayerCount = value;
            }
        }
        private int _maxPlayerCount = 2;

        /// <summary>
        /// 玩家列表
        /// </summary>
        public List<IPlayer> Players { get; set; }

        /// <summary>
        /// appsetting 配置信息
        /// </summary>
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// 插件列表
        /// </summary>
        public PlugInSourceList PlugInSources
        {
            get { return PlugInManager.PlugInSourceList; }


        }

        /// <summary>
        /// 运行信息
        /// </summary>

        public List<RunInfo> RunInfos { get; private set; }

        public Action<string, ConsoleLogStatus> WriteLog { get; set; }

        public Bootstrap()
        {
            InitBootstrap();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitBootstrap()
        {
            //初始化插件
            PlugInManager = new PlugInManager();

            //初始配置信息
            Configuration = new ConfigurationBuilder()
                .AddJsonFile($"{Environment.CurrentDirectory}\\appsetting.json")
                .Build();

     

            //初始化数据库操作
            RunInfos = new List<RunInfo>();
            _timer = new Timer();
        }

        /// <summary>
        /// 开始运行
        /// </summary>
        /// <returns></returns>
        public void Start(List<PlugInSource> playerPlugins)
        {
            //获取最大玩家数量
            _maxPlayerCount = Setting.ThreadNum;
            WriteLog("初始化采集程序", ConsoleLogStatus.Nomral);
            //初始化玩家
            Players = new List<IPlayer>();
            Players.AddRange(playerPlugins.Select(q => q.GetNew<IPlayer>()).Where(p => p != null).ToList());



            for (int item = 1; item <= _maxPlayerCount; item++)
            {
                RunInfos.Add(new RunInfo() { Index = item });
            }
            //运行
            WriteLog("开始运行", ConsoleLogStatus.Nomral);
            _timer.AutoReset = true;
            _timer.Interval = 1000;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }


        [STAThread]
        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {

                _timer?.Stop();

                var playerIsNullRunInfos = RunInfos.Where(q => q.player == null).ToList();
                playerIsNullRunInfos.AsParallel().ForAll((runInfo) =>
                {
                    var result = TaskSchedule.Get();
                    if (result != null)
                    {
                        runInfo.TaskSchedule = result;
                        runInfo.player = PlugInManager.PlugInSourceList.Select(q => q.GetNew<IPlayer>()).Where(p => p != null).ToList().Where(q => q.GetType().ToString() == result.PlayerType).FirstOrDefault();
                        if (runInfo.player != null)
                        {
                            runInfo.player.ConsoleLog = this;
                            runInfo.player.Configuration = Configuration;
                            runInfo.PlugInSource = PlugInManager.PlugInSourceList.Where(q => q.Assemblie == runInfo.player.GetType().Assembly).FirstOrDefault();
                            runInfo.player.Complete += (product, productImage, task, player) =>
                            {
                                if (product != null)
                                {
                                    product = Product.AddOrUpdate(product);
                                    if (task != null)
                                    {
                                        task.PlayerAccountId = product.Id;
                                    }
                                }
                                if (productImage != null)
                                {
                                    productImage = ProductImage.AddOrUpdate(productImage);
                                    if (task != null)
                                    {
                                        task.PlayerAccountId = productImage.Id;
                                    }

                                }
                                if (task != null && task.PlayerAccountId > -1)
                                {
                                    if (player != null)
                                    {
                                        task.PlayerType = player.GetType().ToString();
                                    }
                                    TaskSchedule.AddOrUpdate(task);
                                }
                                runInfo.Dispose();
                            };
                            runInfo.Init();
                            WriteLog($"当前任务:{runInfo.pluginname}--{runInfo.TaskSchedule.PlayerStep}", ConsoleLogStatus.Nomral);
                        }
                    }
                    else
                    {
                        WriteLog("未找到任务信息", ConsoleLogStatus.Nomral);
                    }
                });


                foreach (var runInfo in RunInfos)
                {
                    //超时处理
                    if (runInfo.CheckTimeout())
                    {
                        if (runInfo.State == 0)
                        {
                            WriteLog?.Invoke($"运行超时--{runInfo.simulatorname},释放资源", ConsoleLogStatus.Error);
                            runInfo.player.TaskSchedule.RunDateTime = DateTime.Now.GetTimestamp();
                            runInfo.player.Complete?.Invoke(null, null, runInfo.player.TaskSchedule, runInfo.player);
                            runInfo.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger("testApp.Logging");//获取一个日志记录器 
                log.Error("定时器异常--", ex);
            }
            finally
            {
                _timer?.Start();
            }
        }



        /// <summary>
        /// 强制停止所有
        /// </summary>
        /// <returns></returns>
        public void Stop()
        {
            _timer?.Stop();
            _timer = new Timer();
            foreach (var runInfo in RunInfos)
            {
                runInfo.Dispose();
            }
            RunInfos = new List<RunInfo>();

        }
        public void Dispose()
        {
            Stop();
        }
    }
}
