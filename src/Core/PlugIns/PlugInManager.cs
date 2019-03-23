using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Amazonspider.Core.PlugIns
{

    /// <summary>
    /// 
    /// </summary>
    public class PlugInManager : IPlugInManager
    {
        /// <summary>
        /// 插件列表
        /// </summary>
        public PlugInSourceList PlugInSourceList { get; }

        public PlugInManager()
        {
            PlugInSourceList = new PlugInSourceList();
            //获取Plugin目录下面的插件
            string pluginSimulatorPath = System.Environment.CurrentDirectory;// + "\\Plugins\\";
            DirectoryInfo dirinfo = new DirectoryInfo(pluginSimulatorPath);
            foreach (FileSystemInfo file in dirinfo.GetFileSystemInfos())
            {
                if (file != null)
                {
                    if (file.Extension.ToLower().Equals(".dll"))
                    {
                        Assembly a = Assembly.LoadFrom(file.FullName);
                        var plugIn = (a.GetCustomAttribute(typeof(PlugInAttribute)) as PlugInAttribute);
                        if (plugIn != null)
                        {
                            PlugInSourceList.Add(
                                new PlugInSource() { Assemblie = a, PlugInType = plugIn.PlugInType, PlugInName = plugIn.Name }
                            );
                        }
                    }
                }
            }
        }
    }
}
