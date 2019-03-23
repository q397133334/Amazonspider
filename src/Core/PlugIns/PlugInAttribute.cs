using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazonspider.Core.PlugIns
{
    /// <summary>
    /// 插件名称
    /// </summary>
    public class PlugInAttribute : Attribute
    {
        public PlugInAttribute(string name, PlugInType plugInType)
        {
            Name = name;
            PlugInType = plugInType;
        }

        public string Name { get; set; }

        public PlugInType PlugInType { get; set; }
    }

    public enum PlugInType
    {
        /// <summary>
        /// 模拟器
        /// </summary>
        Simulator,
        /// <summary>
        /// 玩家
        /// </summary>
        Player
    }
}
