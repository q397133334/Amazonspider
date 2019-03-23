using Amazonspider.Core.EntityFramework;
using Microsoft.Extensions.Configuration;
using System;

namespace Amazonspider.Core.Player
{
    public interface IPlayer : IDisposable
    {
        /// <summary>
        /// 运行脚本
        /// </summary>
        void Run();

        /// <summary>
        /// 运行超时时间
        /// </summary>
        long Timeout { get; }

        int Index { get; set; }

        /// <summary>
        /// 任务信息
        /// </summary>
        TaskSchedule TaskSchedule { get; set; }

        /// <summary>
        /// 完成事件（账号信息，下一次任务信息，自身信息）
        /// </summary>
        Action<Product, ProductImage, TaskSchedule, IPlayer> Complete { get; set; }

        /// <summary>
        /// 是否使用
        /// </summary>
        bool IsUse { get; set; }

        /// <summary>
        /// 运行信息
        /// </summary>
        string RunMsg { get; set; }

        /// <summary>
        /// 控制台日志
        /// </summary>
        IConsoleLog ConsoleLog { get; set; }

        /// <summary>
        /// appsetting 配置信息
        /// </summary>
        IConfiguration Configuration { get; set; }

    }
}
