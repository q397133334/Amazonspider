using System;
using System.Collections.Generic;
using System.Reflection;

namespace Amazonspider.Core.PlugIns
{
    public interface IPlugInSource
    {

        /// <summary>
        /// 程序集信息
        /// </summary>
        Assembly Assemblie { get; set; }

        /// <summary>
        /// 插件类型
        /// </summary>
        PlugInType PlugInType { get; set; }

        /// <summary>
        /// 插件名称
        /// </summary>
        string PlugInName { get; set; }

        T GetNew<T>();

    }
}
