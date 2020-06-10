using System;
using System.Collections.Generic;
using System.Text;

namespace PluginManager
{
    public interface IServerPlugin
    {
        string PluginName { get; }
        string Description { get; }
        string Author { get; }
        string Version { get; }

        void Init();
        void Terminate();
    }
}
