using Axis.Model.Common;

namespace Axis.Model.ServiceConfiguration
{
    public class ServiceConfigurationModel : BaseEntity
    {
        public int ConfigID { get; set; }
        public string ConfigName { get; set; }
        public string ConfigXML { get; set; }
        public int ConfigTypeID { get; set; }
    }
}
