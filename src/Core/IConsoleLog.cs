using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazonspider.Core
{
    /// <summary>
    /// 控制台输出日志
    /// </summary>
    public interface IConsoleLog
    {
        /// <summary>
        /// 控制台输入日志接口
        /// </summary>
        Action<string,ConsoleLogStatus> WriteLog { get; set; }
    }
}
