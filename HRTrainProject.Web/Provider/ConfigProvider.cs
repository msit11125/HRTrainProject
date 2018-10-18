using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRTrainProject.Web
{
    public static class ConfigProvider
    {
        public static ConfigManager ConfigManager { get; set; }
    }

    public class ConfigManager
    {
        public string HelloWorld { get; set; }
    }
}
