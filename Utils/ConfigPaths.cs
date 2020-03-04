using System;
using System.Collections.Generic;
using System.Text;

namespace RealTimeCacheApp.Utils
{
    public static class ConfigPaths
    {
        public static string DataPath => Environment.CurrentDirectory + "/Data/"; 
        public static string LogPath => Environment.CurrentDirectory + "/Log/"; 
        public static string ConfigPath => Environment.CurrentDirectory + "/config.json"; 
    }
}
