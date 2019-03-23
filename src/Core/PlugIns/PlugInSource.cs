using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Amazonspider.Core.PlugIns
{
    public class PlugInSource : IPlugInSource
    {
        public Assembly Assemblie { get; set; }


        public PlugInType PlugInType { get; set; }


        public string PlugInName { get; set; }

        public T GetNew<T>()
        {
            var types = Assemblie.GetExportedTypes();
            foreach(var type in types)
            {
                if(typeof(T).IsAssignableFrom(type.GetTypeInfo()))
                {
                    var i = (T)Activator.CreateInstance(type);
                    return i;
                }
            }
            return default(T);
        }

    }
}
