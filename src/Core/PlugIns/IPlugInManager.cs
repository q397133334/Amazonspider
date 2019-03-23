using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazonspider.Core.PlugIns
{
    /// <summary>
    /// 插件管理
    /// </summary>
    public interface IPlugInManager
    {
        /// <summary>
        /// 插件列表
        /// </summary>
        PlugInSourceList PlugInSourceList { get; }
    }
}
