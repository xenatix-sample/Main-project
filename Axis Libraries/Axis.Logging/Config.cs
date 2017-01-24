using System.IO;
namespace Axis.Logging
{ 
    public static class Config
    {
        public static void ConfigurationLog(string path)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(path));
        }
    }
}
